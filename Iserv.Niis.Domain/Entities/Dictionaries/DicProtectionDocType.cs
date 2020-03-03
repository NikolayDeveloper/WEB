using System;
using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Типы патентов
    /// </summary>
    public class DicProtectionDocType : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        public DicProtectionDocType()
        {
            DicDocumentTypes = new HashSet<DicDocumentTypeDicProtectionDocTypeRelation>();
        }

        public string DocTypeText { get; set; }
        public string DocTypeTextKz { get; set; }
        public string DkCode { get; set; }

        #region Relationships

        public int? DepatmentId { get; set; }
        public DicDepartment Department { get; set; }
        public int RouteId { get; set; }
        public DicRoute Route { get; set; }
        public ICollection<DicDocumentTypeDicProtectionDocTypeRelation> DicDocumentTypes { get; set; }

        #endregion

        public override string ToString()
        {
            return NameRu;
        }

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
            /// Патент на изобретение
            /// </summary>
            public const string Invention = "B";

            /// <summary>
            /// Патент на Полезную Модель
            /// </summary>
            public const string UsefulModel = "U";

            /// <summary>
            /// Патент на Промышленный Образец
            /// </summary>
            public const string IndustrialModel = "S2";

            /// <summary>
            /// Товарные Знаки
            /// </summary>
            public const string Trademark = "TM";

            /// <summary>
            /// Наименование Мест Происхождения Товаров
            /// </summary>
            public const string PlaceOfOrigin = "PN";

            /// <summary>
            /// Патент на селекционные достижения
            /// </summary>
            public const string SelectiveAchievement = "SA";

            /// <summary>
            /// Предварительный патент на Промышленный Образец
            /// </summary>
            public const string IndustrialDesignPreliminary = "S1";

            /// <summary>
            /// Инновационный патент
            /// </summary>
            public const string InnovativePatent = "A4";

            /// <summary>
            /// Предварительный патент на изобретение
            /// </summary>
            public const string InventionPreliminary = "A";

            /// <summary>
            /// Международные Товарные Знаки
            /// </summary>
            public const string InternationalTrademark = "ITM";

            /// <summary>
            /// ПРОЧИЕ
            /// </summary>
            public const string Other = "PR";

            /// <summary>
            /// Договор коммерциализации
            /// </summary>
            public const string CommercializationContract = "DK";

            /// <summary>
            /// Авторское право
            /// </summary>
            public const string Copyright = "AP";

            #endregion
        }
    }
}