using System;

namespace Iserv.Niis.Exceptions
{
    public class DataNotFoundException : ApplicationException
    {
        public DataNotFoundException(string entityName, OperationType operationType, int id) : base(
            $"{entityName} {operationType.ToString().ToLower()} error. Entry with id {id} does not exist in DB.")
        {
        }

        public DataNotFoundException(string entityName, OperationType operationType, string searchValue) : base(
            $"{entityName} {operationType.ToString().ToLower()} error. Entry with search value \"{searchValue}\" does not exist in DB.")
        {
        }

        public enum OperationType
        {
            None = 0,
            Read = 1,
            Update = 2,
            Delete = 3,
        }
    }
}