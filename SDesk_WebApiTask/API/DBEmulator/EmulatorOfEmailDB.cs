using System.Collections.Generic;
using System.Linq;
using Epam.Sdesk.Model;
using SDSK.API.DBEmulator.Exceptions;

namespace SDSK.API.DBEmulator
{
    internal static class EmulatorOfEmailDb
    {
        private static List<Mail> _mails;
        private static List<Attachement> _attachements;
        private static List<Status> _statuses;
        private static List<JiraItem> _jiraItems; 
        
        static EmulatorOfEmailDb()
        {
            _mails = new List<Mail>()
            {
                new Mail() { Id = 1, Cc = "Theme1", Body = "Description1"},
                new Mail() { Id = 2, Cc = "Theme2", Body = "Description2"},
                new Mail() { Id = 3, Cc = "Theme3", Body = "Description3"}
            };

            _attachements = new List<Attachement>()
            {
                new Attachement() { Id = 1, MailId = 1, FileName = "File1", FileExtention = ".doc", StatusId = 1},
                new Attachement() { Id = 2, MailId = 1, FileName = "File2", FileExtention = ".xls", StatusId = 2},
                new Attachement() { Id = 3, MailId = 3, FileName = "File3", FileExtention = ".doc", StatusId = 1}
            };

            _statuses = new List<Status>()
            {
                new Status() { Id = 1, Name = "Sent"},
                new Status() { Id = 2, Name = "Not sent"}
            };

            _jiraItems = new List<JiraItem>()
            {
                new JiraItem() { JiraItemId = 1, JiraNumber = 1 },
                new JiraItem() { JiraItemId = 2, JiraNumber = 1 },
                new JiraItem() { JiraItemId = 3, JiraNumber = 1 },
                new JiraItem() { JiraItemId = 4, JiraNumber = 1 },
            };
        }

        #region MailsMethods

        internal static IEnumerable<Mail> FindAllMails()
        {
            return _mails;
        }

        internal static Mail FindMail(int id)
        {
            Mail searchingMail = _mails.FirstOrDefault(m => m.Id == id);

            if (searchingMail == null) throw new NoExistItemException("Mail", id);

            return searchingMail;
        }

        internal static void CreateMail(Mail mail)
        {
            mail.Id = _mails.Count() + 1;
            _mails.Add(mail);
        }

        internal static void RemoveMail(int id)
        {
            Mail deletingMail = FindMail(id);

            _mails.Remove(deletingMail);
        }

        #endregion

        #region AttachmentsMethods

        internal static IEnumerable<Attachement> FindAllAttachmentsToMail(int mailId)
        {
            FindMail(mailId); // <- to throw exception if not exist such mail

            return _attachements.Where(a => a.MailId == mailId);
        }

        internal static IEnumerable<Attachement> FindAllAttachmentsByExtensionAndByStatusToMail(int mailId, string extension, string status = null)
        {
            FindMail(mailId); // <- to throw exception if not exist such mail

            int statusId = _statuses.Where(s => status != null && s.Name == status).Select( s => s.Id).FirstOrDefault(); // if status null or not exist we have here 0

            return _attachements.Where(a => a.MailId == mailId && a.FileExtention == extension && (status == null || a.StatusId == statusId) );
        }

        internal static Attachement FindAttachmentToMail(int mailId, int attachmentId)
        {
            FindMail(mailId); // <- to throw exception if not exist such mail

            Attachement searchingAttachement =
                _attachements.FirstOrDefault(a => a.MailId == mailId && a.Id == attachmentId);

            if (searchingAttachement == null) throw  new NoExistItemException("Attachment", attachmentId);

            return searchingAttachement;
        }

        internal static void CreateAttachment(int mailId, Attachement attachement)
        {
            FindMail(mailId); // <- to throw exception if not exist such mail

            attachement.Id = _attachements.Count() + 1;
            _attachements.Add(attachement);
        }

        internal static void RemoveAttachment(int mailId, int attachmentId)
        {
            Attachement attachment = FindAttachmentToMail(mailId, attachmentId); // <- throw appropriate exception

            _attachements.Remove(attachment);
        }

        #endregion

        #region JiraItemsMethods

        internal static JiraItem FindJiraItem(int jiraItemId)
        {
            JiraItem searchingJiraItem = _jiraItems.FirstOrDefault(j => j.JiraItemId == jiraItemId);

            if (searchingJiraItem == null) throw new NoExistItemException("JiraItem", jiraItemId);

            return searchingJiraItem;
        }

        #endregion
    }
}