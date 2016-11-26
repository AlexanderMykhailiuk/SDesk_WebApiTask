using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;

namespace SDSK.API.Constraints
{
    public class ApiVersionConstraint : IHttpRouteConstraint
    {
        private readonly int _requiredVersion;

        public int RequiredVersion {
            get { return _requiredVersion;  }
        }

        private const int MinVersion = 1;

        public ApiVersionConstraint(int requiredVersion)
        {
            _requiredVersion = requiredVersion;
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, 
            HttpRouteDirection routeDirection)
        {
            bool noDoubleRoutes = !request.RequestUri.ToString().Contains("attachments"); /* <- I should play with it because 
            "Web API (1.x-2.x) does not support multiple attribute routes with the same path on different controllers".*/

            if (noDoubleRoutes && _requiredVersion == MinVersion) return true;

            IEnumerable<string> headerValues;

            if (request.Headers.TryGetValues("api-version", out headerValues))
            {
                int version = GetMaxVersion(headerValues.FirstOrDefault());
                
                if (version == _requiredVersion) return true;
            }

            return false;
        }

        private int GetVersion(string headerValue)
        {
            int rez;

            if (headerValue != null && int.TryParse(headerValue, out rez))
            {
                return rez;
            }

            return MinVersion;
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