using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using SDSK.API.DBEmulator.Exceptions;

namespace SDSK.API.Filters
{
    public class NoExistExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NoExistItemException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}