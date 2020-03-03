using System;

namespace Iserv.Niis.Exceptions
{
    public class SecurityException : ApplicationException
    {
        public SecurityException(string message) : base(message)
        {
            
        }
    }
}
