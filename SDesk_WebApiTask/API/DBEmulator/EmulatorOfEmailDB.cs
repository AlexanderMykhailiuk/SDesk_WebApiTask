using System.Collections.Generic;
using Epam.Sdesk.Model;

namespace SDSK.API.DBEmulator
{
    internal static class EmulatorOfEmailDb
    {
        private static List<Mail> _mails;

        internal static List<Mail> Mails
        {
            get { return _mails; }
        }

        static EmulatorOfEmailDb()
        {
            _mails = new List<Mail>()
            {
                new Mail() { Id = 1, Cc = "Theme1", Body = "Description1"},
                new Mail() { Id = 2, Cc = "Theme2", Body = "Description2"},
                new Mail() { Id = 3, Cc = "Theme3", Body = "Description3"}
            };
        }
    }
}