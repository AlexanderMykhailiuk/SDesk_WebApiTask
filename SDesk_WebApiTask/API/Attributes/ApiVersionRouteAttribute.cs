using System.Collections.Generic;
using System.Web.Http.Routing;
using SDSK.API.Constraints;

namespace SDSK.API.Attributes
{
    public class ApiVersionRouteAttribute : RouteFactoryAttribute
    {
        public ApiVersionRouteAttribute(string template, int requredVersion)
            : base(template)
        {
            _requredVersion = requredVersion;
        }

        private readonly int _requredVersion;
        
        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary();
                constraints.Add("jiraid", new ApiVersionConstraint(_requredVersion));
                return constraints;
            }
        }
    }
}