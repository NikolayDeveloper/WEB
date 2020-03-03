using System;

namespace Iserv.Niis.Exceptions
{
    public class ComException : Exception
    {
        public ComExceptionType ExceptionType { get; set; }

        public ComException(ComExceptionType exceptionType)
        {
            ExceptionType = exceptionType;
            HResult = (int) exceptionType;
        }

        public ComException(ComExceptionType exceptionType, string message) : base(message)
        {
            ExceptionType = exceptionType;
            HResult = (int)exceptionType;
        }

        public ComException(ComExceptionType exceptionType, string message, Exception innerException) : base(message, innerException)
        {
            ExceptionType = exceptionType;
            HResult = (int)exceptionType;
        }
    }
}