using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace SDSK.API.Constraints
{
    public class JiraIdConstraint : IHttpRouteConstraint
    {
        private int _maxId;

        public JiraIdConstraint(int maxId)
        {
            _maxId = maxId;
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values,
            HttpRouteDirection routeDirection)
        {
            object value;
            
            if (values.TryGetValue("id", out value))
            {
                int id;

                if (int.TryParse(value.ToString(), out id) && id > 0 && id <= _maxId) return true;
            }

            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }
    }
}