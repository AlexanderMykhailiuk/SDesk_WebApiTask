using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http.Routing;
using SDSK.API.Constraints;

namespace SDSK.API.Attributes
{
    public class JiraItemRouteAttribute : RouteFactoryAttribute
    {
        private readonly int _maxId;

        public JiraItemRouteAttribute(string template, int maxId) : base(template)
        {
            _maxId = maxId;
        }

        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary();
                constraints.Add("jiraid", new JiraIdConstraint(_maxId));
                return constraints;
            }
        }
    }
}