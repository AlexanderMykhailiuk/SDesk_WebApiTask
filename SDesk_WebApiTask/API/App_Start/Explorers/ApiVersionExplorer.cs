using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Routing;

namespace SDSK.API.Explorers
{

    public class ApiVersionExplorer<TVersionConstraint> : IApiExplorer
    {
        private readonly IApiExplorer _innerApiExplorer;
        private readonly HttpConfiguration _configuration;
        private readonly Lazy<Collection<ApiDescription>> _apiDescriptions;
        private MethodInfo _apiDescriptionPopulator;

        public ApiVersionExplorer(IApiExplorer apiExplorer, HttpConfiguration configuration)
        {
            _innerApiExplorer = apiExplorer;
            _configuration = configuration;
            _apiDescriptions = new Lazy<Collection<ApiDescription>>(Init);
        }

        public Collection<ApiDescription> ApiDescriptions => _apiDescriptions.Value;

        private Collection<ApiDescription> Init()
        {
            var descriptions = _innerApiExplorer.ApiDescriptions;

            var controllerSelector = _configuration.Services.GetHttpControllerSelector();
            var controllerMappings = controllerSelector.GetControllerMapping();

            var flatRoutes = FlattenRoutes(_configuration.Routes);
            var result = new Collection<ApiDescription>();

            foreach (var description in descriptions)
            {
                result.Add(description);

                if (controllerMappings != null && description.Route.Constraints.Any(c => c.Value is TVersionConstraint))
                {
                    var matchingRoutes = flatRoutes.Where(r => r.RouteTemplate == description.Route.RouteTemplate && r != description.Route);

                    foreach (var route in matchingRoutes)
                        GetRouteDescriptions(route, result);
                }
            }
            return result;
        }

        private void GetRouteDescriptions(IHttpRoute route, Collection<ApiDescription> apiDescriptions)
        {
            var actionDescriptor = route.DataTokens["actions"] as IEnumerable<HttpActionDescriptor>;

            if (actionDescriptor != null && actionDescriptor.Any())
                GetPopulateMethod().Invoke(_innerApiExplorer, new object[] { actionDescriptor.First(), route, route.RouteTemplate, apiDescriptions });
        }

        private MethodInfo GetPopulateMethod()
        {
            return _apiDescriptionPopulator ??
                   (_apiDescriptionPopulator =
                       _innerApiExplorer.GetType()
                           .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                           .FirstOrDefault(
                               m => m.Name == "PopulateActionDescriptions" && m.GetParameters().Length == 4));
        }

        public static IEnumerable<IHttpRoute> FlattenRoutes(IEnumerable<IHttpRoute> routes)
        {
            foreach (var route in routes)
            {
                if (route is HttpRoute)
                    yield return route;

                var subRoutes = route as IReadOnlyCollection<IHttpRoute>;
                if (subRoutes != null)
                    foreach (IHttpRoute subRoute in FlattenRoutes(subRoutes))
                        yield return subRoute;
            }
        }
    }
}