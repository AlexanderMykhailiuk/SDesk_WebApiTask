using System.Net;
using System.Net.Http;
using System.Web.Http;
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
    }
}
