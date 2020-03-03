using Iserv.Niis.Model.Models.Subject;
using System;

namespace Iserv.Niis.Model.Models.Material.Outgoing
{
    public class MaterialOutgoingDetailDto : MaterialDetailDto
    {
        public MaterialOutgoingDetailDto()
        {
            ContactInfos = new ContactInfoDto[0];
        }

        public int? Barcode { get; set; }
        public int? AddresseeId { get; set; }
        public int? TypeId { get; set; }
        public DateTimeOffset? DateCreate { get; set; }
        public DateTimeOffset? DocumentDate { get; set; }
        public string OutgoingNumber { get; set; }
        public string AddresseeAddress { get; set; }
        public string AddresseeNameRu { get; set; }
        public string Apartment { get; set; }
        public UserInputDto UserInput { get; set; }
        public string Description { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string NameKz { get; set; }
        public string AddresseeXin { get; set; }
        public string AddresseeCity { get; set; }
        public string AddresseeOblast { get; set; }
        public string AddresseeRepublic { get; set; }
        public string AddresseeRegion { get; set; }
        public string AddresseeStreet { get; set; }
        public string DocumentNum { get; set; }
        public string AddresseeEmail { get; set; }
        public string AddresseeShortAddress { get; set; }
        public ContactInfoDto[] ContactInfos { get; set; }
        public int? SendTypeId { get; set; }

        /// <summary>
        /// № счёта на оплату
        /// </summary>
        public string NumberForPayment { get; set; }

        /// <summary>
        /// Дата счёта
        /// </summary>
        public DateTimeOffset? PaymentDate { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public int? PaymentInvoiceId { get; set; }
        public string PaymentInvoiceCode { get; set; }


        /// <summary>
        /// № входящего документа
        /// Номер входящего документа с которым связан исходящий документ
        /// TODO пока строка, должно замениться на ссылку
        /// </summary>
        public string IncomingDocumentNumber { get; set; }

        /// <summary>
        /// Трэк номер
        /// </summary>
        public string TrackNumber { get; set; }

        /// <summary>
        /// Ссылка на воходящий в исходящем(Ответ на входящий) для завершения входящего при присвоении рег. номера.
        /// </summary>
        public int? IncomingAnswerId { get; set; }
        public string IncomingAnswerNumber { get; set; }
    }
}
