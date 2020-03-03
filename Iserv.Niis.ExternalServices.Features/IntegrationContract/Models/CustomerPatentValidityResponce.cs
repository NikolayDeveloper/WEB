using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Models
{
    /// <summary>
    /// Результат поиска ОД
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "http://niis-efilling-service.kz")]
    public class CustomerPatentValidityResponce : SystemInfo
    {
        /// <summary>
        /// Баркод ОД
        /// </summary>
        public int PatentId { get; set; }

        /// <summary>
        /// Баркод Заявки
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        /// Наименование патента EN
        /// </summary>
        public string PatentNameEn { get; set; }

        /// <summary>
        /// Наименование патента Ru
        /// </summary>
        public string PatentNameRu { get; set; }
        /// <summary>
        /// Наименование патента Kz
        /// </summary>
        public string PatentNameKz { get; set; }

        /// <summary>
        /// Срок действия патента
        /// </summary>
        public DateTime ValidityDate { get; set; }

        /// <summary>
        /// Контрагенты
        /// </summary>
        public PatentOwner[] Owners { get; set; }
    }

}
