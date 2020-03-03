using System;

namespace Iserv.Niis.Domain.Entities
{
    public class SearchRequestViewEntity
    {
        public int Id { get; set; }

        public int Barcode { get; set; }

        public int? DocTypeId { get; set; }

        public string DocTypeName { get; set; }

        public int? ProtectionDocTypeId { get; set; }

        public string ProtectionDocTypeName { get; set; }

        public int? RequestSubTypeId { get; set; }

        public string RequestSubTypeName { get; set; }

        public int? RequestTypeId { get; set; }

        public string RequestTypeName { get; set; }

        public string IncomingNumber { get; set; }

        public DateTimeOffset DateCreate { get; set; }

        public string RegNumber { get; set; }

        public int? ReceiveTypeId { get; set; }

        public string ReceiveTypeName { get; set; }

        public string NameRu { get; set; }

        public string NameKz { get; set; }

        public string NameEn { get; set; }

        public DateTimeOffset? RegDate { get; set; }

        public string ProtectionDocNumber { get; set; }

        public int? ProtectionDocMaintainYear { get; set; }

        public DateTimeOffset? ProtectionDocValidDate { get; set; }

        public DateTimeOffset? ProtectionDocExtensionDate { get; set; }

        public int? SelectionAchieveTypeId { get; set; }

        public string SelectionAchieveTypeName { get; set; }

        public string BreedingNumber { get; set; }

        public DateTimeOffset? ProtectionDocDate { get; set; }

        public DateTimeOffset? ProtectionDocOutgoingDate { get; set; }

        public string DisclaimerRu { get; set; }

        public string DisclaimerKz { get; set; }

        public int? RequestStatusId { get; set; }

        public string RequestStatusName { get; set; }

        public int? ProtectionDocStatusId { get; set; }

        public string ProtectionDocStatusName { get; set; }

        public string IcisCodes { get; set; }

        public string IcfemCodes { get; set; }

        public string IpcCodes { get; set; }

        public string IcgsCodes { get; set; }

        public string DeclarantNames { get; set; }

        public string PatentOwnerNames { get; set; }

        public string AuthorNames { get; set; }

        public string PatentAttorneyNames { get; set; }

        public string CorrespondenceNames { get; set; }

        public string ConfidantNames { get; set; }

        public bool AuthorsAreNotMentions { get; set; }

        public string AuthorsCertificateNumbers { get; set; }

        public string NumberBulletin { get; set; }

        public byte[] Image { get; set; }
    }
}