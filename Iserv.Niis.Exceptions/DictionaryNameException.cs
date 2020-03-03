using System;

namespace Iserv.Niis.Exceptions
{
    public class DictionaryNameException : ApplicationException
    {
        public DictionaryNameException() : base("Incorrect dictionary name.")
        {
        }
    }
}