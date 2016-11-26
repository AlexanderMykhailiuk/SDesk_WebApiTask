using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using SDSK.API.DBEmulator.Exceptions;
using log4net;

namespace SDSK.API.Filters
{
    public class NoExistExceptionFilter : ExceptionFilterAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger("AppLogger");

        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NoExistItemException)
            {
                Log.Error(string.Format("Manipulating with not existed item. Deteils: {0}",context.Exception.Message));

                context.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}