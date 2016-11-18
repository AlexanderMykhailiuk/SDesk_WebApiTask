using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Epam.Sdesk.Model;
using static SDSK.API.DBEmulator.EmulatorOfEmailDb;

namespace SDSK.API.Controllers
{
    public class MailsController : ApiController
    {
        public HttpResponseMessage GetAllMails()
        {
            var response = Request.CreateResponse<IEnumerable<Mail>>(HttpStatusCode.OK, Mails);

            return response;
        }

        public HttpResponseMessage GetMail(int id)
        {
            Mail gettingMail = Mails.FirstOrDefault(m => m.Id == id);

            HttpResponseMessage response;

            if (gettingMail !=null) response = Request.CreateResponse(HttpStatusCode.OK, gettingMail);
            else response = new HttpResponseMessage(HttpStatusCode.NotFound);

            return response;
        }

        [HttpPost]
        public HttpResponseMessage AddMail(Mail newMail)
        {
            if(ModelState.IsValid) //<- needing add attributes for mail in class definition
            {

                newMail.Id = Mails.Count() + 1;
                Mails.Add(newMail);

                return  new HttpResponseMessage(HttpStatusCode.Created);
            }

            return  new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [HttpPut]
        public HttpResponseMessage UpdateMail(int id, Mail updatedMail)
        {
            if(ModelState.IsValid)
            {
                Mail oldMail = Mails.FirstOrDefault(m => m.Id == id);

                if (oldMail == null) return  new HttpResponseMessage(HttpStatusCode.NotFound);

                var mapper = new MapperConfiguration( cfg => cfg.CreateMap<Mail,Mail>() ).CreateMapper(); // I prefer to use automapper to update fields of mail
                mapper.Map(updatedMail, oldMail);
                oldMail.Id = id;

                return  new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        public HttpResponseMessage DeleteMail(int id)
        {
            Mail deletingMail = Mails.FirstOrDefault(m => m.Id == id);

            HttpStatusCode status;

            if (deletingMail != null)
            {
                Mails.Remove(deletingMail);
                status = HttpStatusCode.NoContent;
            }
            else status = HttpStatusCode.NotFound;

            return  new HttpResponseMessage(status);
        }
    }
}
