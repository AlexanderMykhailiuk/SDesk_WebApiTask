using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Epam.Sdesk.Model;
using SDSK.API.Filters;
using static SDSK.API.DBEmulator.EmulatorOfEmailDb;

namespace SDSK.API.Controllers
{
    [NoExistExceptionFilter]
    public class MailsController : ApiController
    {
        public HttpResponseMessage GetAllMails()
        {
            var response = Request.CreateResponse<IEnumerable<Mail>>(HttpStatusCode.OK, FindAllMails());
            
            return response;
        }

        public HttpResponseMessage GetMail(int id)
        {
            Mail gettingMail = FindMail(id);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, gettingMail);
            
            return response;
        }

        [HttpPost]
        public HttpResponseMessage AddMail(Mail newMail)
        {
            if(ModelState.IsValid) //<- needing add attributes for mail in class definition
            {
                CreateMail(newMail);

                return  new HttpResponseMessage(HttpStatusCode.Created);
            }

            return  new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [HttpPut]
        public HttpResponseMessage UpdateMail(int id, Mail updatedMail)
        {
            if(ModelState.IsValid)
            {
                Mail oldMail = FindMail(id);
                
                var mapper = new MapperConfiguration( cfg => cfg.CreateMap<Mail,Mail>() ).CreateMapper(); // I prefer to use automapper to update fields of mail
                mapper.Map(updatedMail, oldMail);
                oldMail.Id = id;

                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        public HttpResponseMessage DeleteMail(int id)
        {
            RemoveMail(id);
            
            return  new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
