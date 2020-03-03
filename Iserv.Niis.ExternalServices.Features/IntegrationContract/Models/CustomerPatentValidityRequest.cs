using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationContract.Models
{
    /// <summary>
    /// Входны параметры для запроса
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "http://niis-efilling-service.kz")]
    public class CustomerPatentValidityRequest : SystemInfo
    {
        /// <summary>
        /// Тип(ищется в типе заявки)
        /// </summary>
        public ReferenceInfo PatentType { get; set; }

        /// <summary>
        /// Гос. Номер патента
        /// </summary>
        public string GosNumber { get; set; }
    }
}
