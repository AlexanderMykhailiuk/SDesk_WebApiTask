using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Epam.Sdesk.Model;
using SDSK.API.Attributes;
using SDSK.API.Filters;
using static SDSK.API.DBEmulator.EmulatorOfEmailDb;

namespace SDSK.API.Controllers
{
    
    [NoExistExceptionFilter]
    [RoutePrefix("api/mails")]
    public class Mails2Controller : ApiController
    {
        private const int RequiredVersion = 2;

        #region MailsActions
        
        [ApiVersionRoute("", RequiredVersion)]
        public HttpResponseMessage GetAllMails()
        {
            var response = Request.CreateResponse<IEnumerable<Mail>>(HttpStatusCode.OK, FindAllMails());

            return response;
        }

                
        [ApiVersionRoute("{id}", RequiredVersion)]
        public HttpResponseMessage GetMail(int id)
        {
            Mail gettingMail = FindMail(id);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, gettingMail);

            return response;
        }

        [ApiVersionRoute("", RequiredVersion)]
        [HttpPost]
        public HttpResponseMessage AddMail(Mail newMail)
        {
            if (ModelState.IsValid) //<- needing add attributes for mail in class definition
            {
                CreateMail(newMail);

                return new HttpResponseMessage(HttpStatusCode.Created);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [ApiVersionRoute("{id}", RequiredVersion)]
        [HttpPut]
        public HttpResponseMessage UpdateMail(int id, Mail updatedMail)
        {
            if (ModelState.IsValid)
            {
                Mail oldMail = FindMail(id);

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Mail, Mail>()).CreateMapper(); // I prefer to use automapper to update fields of mail
                mapper.Map(updatedMail, oldMail);
                oldMail.Id = id;

                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [ApiVersionRoute("{id}", RequiredVersion)]
        public HttpResponseMessage DeleteMail(int id)
        {
            RemoveMail(id);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
        
        #endregion
        
        #region AttachmentsActions

        [ApiVersionRoute("{id}/attachments", RequiredVersion)]
        public HttpResponseMessage GetAllAttachmentsToMail(int id)
        {
            var searchingAttachments = FindAllAttachmentsToMail(id);

            if (!searchingAttachments.Any()) throw new HttpResponseException(HttpStatusCode.NoContent);

            var response = Request.CreateResponse<IEnumerable<Attachement>>(HttpStatusCode.OK, searchingAttachments);

            return response;
        }

        [ApiVersionRoute("{id}/attachments/{attid}", RequiredVersion)]
        public HttpResponseMessage GetAttachmentToMail(int id, int attId)
        {
            Attachement gettingAttachement = FindAttachmentToMail(id, attId);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, gettingAttachement);

            return response;
        }

        [ApiVersionRoute("{id}/attachments", RequiredVersion)]
        public HttpResponseMessage GetAllAttachmentsToMail(int id, string extention, string status = null)
        {
            var searchingAttachments = FindAllAttachmentsByExtensionAndByStatusToMail(id, extention, status);

            if (!searchingAttachments.Any()) throw new HttpResponseException(HttpStatusCode.NoContent);

            var response = Request.CreateResponse<IEnumerable<Attachement>>(HttpStatusCode.OK, searchingAttachments);

            return response;
        }

        [ApiVersionRoute("{id}/attachments", RequiredVersion)]
        [HttpPost]
        public HttpResponseMessage AddAttachment(int id, Attachement attachement)
        {
            if (ModelState.IsValid) //<- needing add attributes for attachment in class definition
            {
                CreateAttachment(id, attachement);

                return new HttpResponseMessage(HttpStatusCode.Created);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [ApiVersionRoute("{id}/attachments/{attid}", RequiredVersion)]
        [HttpPut]
        public HttpResponseMessage UpdateAttachment(int id, int attid, Attachement updatedAttachement)
        {
            if (ModelState.IsValid)
            {
                Attachement oldAttachement = FindAttachmentToMail(id, attid);

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Attachement, Attachement>()).CreateMapper(); // I prefer to use automapper to update fields of mail
                mapper.Map(updatedAttachement, oldAttachement);
                oldAttachement.Id = attid;

                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [ApiVersionRoute("{id}/attachments/{attid}", RequiredVersion)]
        public HttpResponseMessage DeleteAttachment(int id, int attid)
        {
            RemoveAttachment(id, attid);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        #endregion
    }
}
