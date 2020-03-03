using System;

namespace Iserv.Niis.Exceptions
{
    public class FilterException : ApplicationException
    {
        public FilterException(string message = "Incorrect filter conditions", Exception innerException = null) : base(message, innerException)
        {
        }
    }
}