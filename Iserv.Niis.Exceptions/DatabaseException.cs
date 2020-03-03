using System;

namespace Iserv.Niis.Exceptions
{
    public class DatabaseException : ApplicationException
    {
        public DatabaseException(string message) : base(message)
        {
        }
    }
}