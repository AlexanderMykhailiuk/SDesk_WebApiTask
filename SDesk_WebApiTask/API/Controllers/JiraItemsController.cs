using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using Epam.Sdesk.Model;
using SDSK.API.Filters;
using static SDSK.API.DBEmulator.EmulatorOfEmailDb;

namespace SDSK.API.Controllers
{
    [NoExistExceptionFilter]
    public class JiraItemsController : ApiController
    {
        public HttpResponseMessage GetJiraItem(int id=1)
        {
            JiraItem gettingJiraItem = FindJiraItem(id);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, gettingJiraItem);

            return response;
        }

        [Route("api/jiraitems/Jira-{id:jiraid(3)}")] 
        public HttpResponseMessage GetJiraItemCheckAttributeRoute(int id)
        {
            return GetJiraItem(id);
        }
    }
}
