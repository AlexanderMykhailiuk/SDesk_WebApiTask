using System;

namespace SDSK.API.DBEmulator.Exceptions
{
    public class NoExistItemException : Exception
    {
        public NoExistItemException(string itemName, int id)
            : base(string.Format("Not exist {0} with key {1}", itemName, id))
        {

        }
    }
}