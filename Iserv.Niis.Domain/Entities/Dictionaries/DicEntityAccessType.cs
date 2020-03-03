using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// 
    /// </summary>
    public class DicEntityAccessType : DictionaryEntity<int>, IHaveConcurrencyToken
    {
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(NameRu))
                return NameRu;
            if (!string.IsNullOrEmpty(NameEn))
                return NameEn;
            if (!string.IsNullOrEmpty(NameKz))
                return NameKz;
            if (!string.IsNullOrEmpty(Code))
                return Code;
            return Id.ToString();
        }

        #region Public codes

        public const string All = "All";
        public const string Create = "Create";
        public const string Read = "Read";
        public const string Update = "Update";
        public const string Delete = "Delete";

        #endregion

        public enum Codes
        {
            None,
            All,
            Create,
            Read,
            Update,
            Delete
        }

        public static string GetCode(Codes code)
        {
            switch (code)
            {
                case Codes.All:
                    return All;
                case Codes.Create:
                    return Create;
                case Codes.Read:
                    return Read;
                case Codes.Update:
                    return Update;
                case Codes.Delete:
                    return Delete;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }
    }
}