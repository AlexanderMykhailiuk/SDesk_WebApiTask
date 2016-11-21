using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Epam.Sdesk.Model;
using SDSK.API.Filters;
using  static SDSK.API.DBEmulator.EmulatorOfEmailDb;

namespace SDSK.API.Controllers
{
    [NoExistExceptionFilter]
    [RoutePrefix("api/mails/{id:int}/attachments/{attId:int?}")]
    public class AttachmentsController : ApiController
    {
        [Route("")]
        public HttpResponseMessage GetAllAttachmentsToMail(int id)
        {
            var searchingAttachments = FindAllAttachmentsToMail(id);

            if ( !searchingAttachments.Any() ) throw new HttpResponseException(HttpStatusCode.NoContent);

            var response = Request.CreateResponse<IEnumerable<Attachement>>(HttpStatusCode.OK, searchingAttachments);

            return response;
        }

        [Route("")]
        public HttpResponseMessage GetAttachmentToMail(int id, int attId)
        {
            Attachement gettingAttachement = FindAttachmentToMail(id, attId);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, gettingAttachement);

            return response;
        }

        [Route("")] // using '?' is default way to path parametrs to action, so I don't add here modifications
        public HttpResponseMessage GetAllAttachmentsToMail(int id, string extention, string status = null)
        {
            var searchingAttachments = FindAllAttachmentsByExtensionAndByStatusToMail(id, extention, status);

            if (!searchingAttachments.Any()) throw new HttpResponseException(HttpStatusCode.NoContent);

            var response = Request.CreateResponse<IEnumerable<Attachement>>(HttpStatusCode.OK, searchingAttachments);

            return response;
        }

        [Route("")]
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

        [Route("")]
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

        [Route("")]
        public HttpResponseMessage DeleteAttachment(int id, int attid)
        {
            RemoveAttachment(id, attid);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
