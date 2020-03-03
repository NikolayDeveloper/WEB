using System;
using Iserv.Niis.Model.Models.Subject;

namespace Iserv.Niis.Model.Models.Material.Incoming
{
    public class MaterialIncomingDetailDto: MaterialDetailDto
    {
        public MaterialIncomingDetailDto()
        {
            ContactInfos = new ContactInfoDto[0];
        }

        public int? Id { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public int ReceiveTypeId { get; set; }
        public int? AddresseeId { get; set; }
        
        public string AddresseeXin { get; set; }
        public string AddresseeCity { get; set; }
        public string AddresseeOblast { get; set; }
        public string AddresseeRepublic { get; set; }
        public string AddresseeRegion { get; set; }
        public string AddresseeStreet { get; set; }
        public int TypeId { get; set; }
        public DateTimeOffset? DateCreate { get; set; }
        public DateTimeOffset? DocumentDate { get; set; }
        public string OutgoingNumber { get; set; }
        public string IncomingNumber { get; set; }
        public string IncomingNumberFilial { get; set; }
        public int? Barcode { get; set; }
        public string AddresseeAddress { get; set; }
        public string AddresseeShortAddress { get; set; }
        public ContactInfoDto[] ContactInfos { get; set; }
        public string AddresseeNameRu { get; set; }
        public string Apartment { get; set; }
        public int? DepartmentId { get; set; }
        public int? DivisionId { get; set; }

        /// <summary>
        /// Отметка контроля
        /// </summary>
        public bool? ControlMark { get; set; }

        /// <summary>
        /// Дата контроля
        /// </summary>
        public DateTimeOffset? ControlDate { get; set; }

        /// <summary>
        /// Резолюция по продлению даты контроля\снятию с контроля
        /// </summary>
        public string ResolutionExtensionControlDate { get; set; }

        /// <summary>
        /// Снят с контроля
        /// </summary>
        public bool? OutOfControl { get; set; }

        /// <summary>
        /// Дата снятия с контроля
        /// </summary>
        public DateTimeOffset? DateOutOfControl { get; set; }

        /// <summary>
        /// Признак наличия платёжного документа
        /// </summary>
        public bool? IsHasPaymentDocument { get; set; }

        /// <summary>
        /// Количество требуемых платежек по ходатайству
        /// </summary>
        public int? AttachedPaymentsCount { get; set; }
    }
}
