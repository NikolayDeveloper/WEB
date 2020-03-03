using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries.DicMain
{
    public class DicDepartmentType : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        #region Public codes

        public static string Department = "1";
        public static string Position = "2";
        public static string Division = "3";

        #endregion

        public enum Codes
        {
            None,
            Department,
            Position,
            Division
        }

        public static string GetCode(Codes code)
        {
            switch (code)
            {
                case Codes.Department:
                    return Department;
                case Codes.Position:
                    return Position;
                case Codes.Division:
                    return Division;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }
    }
}