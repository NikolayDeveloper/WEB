using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Models
{
    /// <summary>
    /// Контрагенты
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "http://niis-efilling-service.kz")]
    public class PatentOwner
    {
        /// <summary>
        /// ИИН
        /// </summary>
        public string Xin { get; set; }

        /// <summary>
        /// Роль контранета
        /// </summary>
        public int CustomerType { get; set; }

        /// <summary>
        /// Наименование En
        /// </summary>
        public string OwnerNameEn { get; set; }

        /// <summary>
        /// Наименование Ru
        /// </summary>
        public string OwnerNameRu { get; set; }

        /// <summary>
        /// Наименование Kz
        /// </summary>
        public string OwnerNameKz { get; set; }

        /// <summary>
        /// Страна
        /// </summary>
        public ReferenceInfo Location { get; set; }

        /// <summary>
        /// Адрес En
        /// </summary>
        public string AddressEn { get; set; }

        /// <summary>
        /// Адрес Ru
        /// </summary>
        public string AddressRu { get; set; }

        /// <summary>
        /// Адрес Kz
        /// </summary>
        public string AddressKz { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string AddressPostCode { get; set; }
    }
}
