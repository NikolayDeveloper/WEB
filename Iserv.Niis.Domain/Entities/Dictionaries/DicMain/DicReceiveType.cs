using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Enums;

namespace Iserv.Niis.Domain.Entities.Dictionaries.DicMain
{
    public class DicReceiveType : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        /// <summary>
        /// Группа (Бумажный/Эллектронный)
        /// </summary>
        public ReceiveTypeGroupEnum Group { get; set; }

        public static class Codes
        {
            #region Public codes

            /// <summary>
            /// Нарочно
            /// </summary>
            public const string Courier = "4";

            #endregion
        }

    }
}