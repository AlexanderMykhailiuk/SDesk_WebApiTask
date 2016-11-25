using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace SDSK.API.Constraints
{
    public class ApiVersionConstraint : IHttpRouteConstraint
    {
        private readonly int _requiredVersion;

        public ApiVersionConstraint(int requiredVersion)
        {
            _requiredVersion = requiredVersion;
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, 
            HttpRouteDirection routeDirection)
        {
            IEnumerable<string> headerValues;

            if (request.Headers.TryGetValues("api-version", out headerValues))
            {
                int version = GetMaxVersion(headerValues.FirstOrDefault());

                if (version == _requiredVersion) return true;
            }

            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        private int GetVersion(string headerValue)
        {
            int rez;

            if (headerValue != null && int.TryParse(headerValue, out rez))
            {
                return rez;
            }

            return 1;
        }

        private int GetMaxVersion(string headerValues)
        {
            string[] versions = headerValues.Split(',');

            int maxVersion = GetVersion(versions[0]);

            for (int i = 1; i < versions.Length; i++)
            {
                int version = GetVersion(versions[i]);
                if (maxVersion < version) maxVersion = version;
            }

            return maxVersion;
        }
    }
}