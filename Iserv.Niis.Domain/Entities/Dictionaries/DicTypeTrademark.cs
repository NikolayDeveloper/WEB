using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    public class DicTypeTrademark : DictionaryEntity<int>, IHaveConcurrencyToken
    {
        public static class Codes
        {
            public enum CodesEnum
            {
                None,
            }

            public static string GetCode(CodesEnum code)
            {
                switch (code)
                {
                    default:
                        throw new ArgumentOutOfRangeException(nameof(code), code, null);
                }
            }
            #region Public codes
            /// <summary>
            /// Комбинированный
            /// </summary>
            public static string Combined = "05";

            /// <summary>
            /// Словесный
            /// </summary>
            public static string Verbal = "08";

            /// <summary>
            /// Буквенный
            /// </summary>
            public static string Literal = "01";

            /// <summary>
            /// Звуковой
            /// </summary>
            public static string Volume = "94";
            #endregion
        }
    }
}
