using System;
using System.IO;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing;
using SDSK.API.Constraints;
using SDSK.API.Explorers;

namespace SDSK.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // register route constraint
            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("jiraid", typeof(JiraIdConstraint));
            
            // Web API routes
            config.MapHttpAttributeRoutes(constraintResolver);

            config.Routes.MapHttpRoute(
                name: "JiraTestConstraintApi",
                routeTemplate: "api/jiraitems/{id=1}",
                defaults: new { controller = "jiraitems", id = RouteParameter.Optional },
                constraints: new { id = new JiraIdConstraint(3)}
            ); // <- create this map only for testing JiraIdConstarint

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Config swagger
            var apiExplorer = config.Services.GetApiExplorer();
            config.Services.Replace(typeof(IApiExplorer), new ApiVersionExplorer<ApiVersionConstraint>(apiExplorer, config));
            SwaggerConfig.Register(config);

            // log4net config
            log4net.Config.XmlConfigurator.ConfigureAndWatch(
                new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
        }
    }
}
