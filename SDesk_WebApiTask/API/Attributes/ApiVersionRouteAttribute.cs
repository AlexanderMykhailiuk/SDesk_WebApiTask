using System.Collections.Generic;
using System.Web.Http.Routing;
using SDSK.API.Constraints;

namespace SDSK.API.Attributes
{
    public class ApiVersionRouteAttribute : RouteFactoryAttribute
    {
        private readonly int _requredVersion;

        private const int MinVersion = 1;

        public ApiVersionRouteAttribute(string template, int requredVersion = 1)
            : base(template)
        {
            _requredVersion = requredVersion;
        }
        
        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary();

                constraints.Add("apiVersion", new ApiVersionConstraint(_requredVersion));
                
                return constraints;
            }
        }
    }
}