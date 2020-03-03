using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    /// <summary>
    /// ИНформация о патенте по заявителю
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class CustomerPatentInfo
    {
        /// <summary>
        /// Баркод патента
        /// </summary>
        public int PatentId { get; set; }

        /// <summary>
        /// Гос. Номер
        /// </summary>
        public string CertificateNumber { get; set; }

        /// <summary>
        /// Тип патента
        /// </summary>
        public int PatentTypeId { get; set; }

        /// <summary>
        /// Наименование на английском
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// Наименование на русском
        /// </summary>
        public string NameRu { get; set; }

        /// <summary>
        /// Наименование на казахском
        /// </summary>
        public string NameKz { get; set; }

        /// <summary>
        /// Срок действия патента
        /// </summary>
        public DateTime? ValidityDate { get; set; }
    }
}