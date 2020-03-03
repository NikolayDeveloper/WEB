using System;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal.Dto
{
    public class DocumentsSearchParametersDto
    {
        private DateTime? requestCreateDateFrom;
        private DateTime? requestCreateDateTo;
        private DateTime? requestRegDateFrom;
        private DateTime? requestRegDateTo;
        private DateTime? protectionDocValidDate;
        private DateTime? protectionDocExtensionDate;
        private DateTime? protectionDocDate;
        private DateTime? protectionDocOutgoingDate;
        private DateTime? confidantDateFrom;
        private DateTime? confidantDateTo;
        private DateTime? bulletinDate;

        public DocumentCategory DocumentCategory { get; set; }

        public int? DocTypeId { get; set; }

        public int? Barcode { get; set; }

        public int? ReceiveTypeId { get; set; }

        public int? RequestSubTypeId { get; set; }

        public int? RequestTypeId { get; set; }

        public string RequestIncomingNumber { get; set; }

        public string RequestRegNumber { get; set; }

        public DateTime? RequestCreateDateFrom
        {
            get => this.requestCreateDateFrom;
            set => this.requestCreateDateFrom = value?.ToUniversalTime();
        }

        public DateTime? RequestCreateDateTo
        {
            get => this.requestCreateDateTo;
            set => this.requestCreateDateTo = value?.ToUniversalTime();
        }

        public DateTime? RequestRegDateFrom
        {
            get => this.requestRegDateFrom;
            set => this.requestRegDateFrom = value?.ToUniversalTime();
        }

        public DateTime? RequestRegDateTo
        {
            get => this.requestRegDateTo;
            set => this.requestRegDateTo = value?.ToUniversalTime();
        }

        public int? RequestStatusId { get; set; }

        public string NameRu { get; set; }

        public string NameKz { get; set; }

        public string NameEn { get; set; }

        public string ProtectionDocNumber { get; set; }

        public int? ProtectionDocMaintainYear { get; set; }

        public DateTime? ProtectionDocValidDate
        {
            get => this.protectionDocValidDate;
            set => this.protectionDocValidDate = value?.ToUniversalTime();
        }

        public DateTime? ProtectionDocExtensionDate
        {
            get => this.protectionDocExtensionDate;
            set => this.protectionDocExtensionDate = value?.ToUniversalTime();
        }

        public int? ProtectionDocStatusId { get; set; }

        public DateTime? ProtectionDocDate
        {
            get => this.protectionDocDate;
            set => this.protectionDocDate = value?.ToUniversalTime();
        }

        public DateTime? ProtectionDocOutgoingDate
        {
            get => this.protectionDocOutgoingDate;
            set => this.protectionDocOutgoingDate = value?.ToUniversalTime();
        }

        public int? IcgsId { get; set; }

        public int? SelectionAchieveTypeId { get; set; }

        public string DeclarantName { get; set; }

        public string PatentOwnerName { get; set; }

        public string AuthorName { get; set; }

        public string PatentAttorneyName { get; set; }

        public string CorrespondenceName { get; set; }

        public string ConfidantName { get; set; }

        public bool IsNotMention { get; set; }

        public DateTime? ConfidantDateFrom
        {
            get => this.confidantDateFrom;
            set => this.confidantDateFrom = value?.ToUniversalTime();
        }

        public DateTime? ConfidantDateTo
        {
            get => this.confidantDateTo;
            set => this.confidantDateTo = value?.ToUniversalTime();
        }

        public string AuthorCertificateNumber { get; set; }

        public string BulletinNumber { get; set; }

        public DateTime? BulletinDate
        {
            get => this.bulletinDate;
            set => this.bulletinDate = value?.ToUniversalTime();
        }
    }
}