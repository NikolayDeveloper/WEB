using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class RequisitionSendArgument : SystemInfoMessage
    {
        public string AdrCustomerName { get; set; }
        public string BreedingNumber { get; set; }
        public RefKey BreedCountry { get; set; }

        public string ProductType { get; set; }
        public string ProductSpecialProp { get; set; }
        public string ProductPalce { get; set; }

        public string Breed { get; set; }
        public string Genus { get; set; }
        public string GenusEn { get; set; }

        public RefKey PatentType { get; set; }
        public bool RequirementsLaw { get; set; }
        public string EarlyRegNum { get; set; }
        public bool LawTTWp2s10 { get; set; }
        public bool AssignmentTPT { get; set; }
        public bool AssignmentAuthorTAT { get; set; }
        public bool HeirshipTN { get; set; }
        public bool IsCollectiveTradeMark { get; set; }
        public bool IsTMInStandartFont { get; set; }
        public bool IsTMVolume { get; set; }
        public string PatentSubType { get; set; }
        public string Translation { get; set; }
        public string Transliteration { get; set; }

        public RefKey CustomerType { get; set; }
        public string CustomerName { get; set; }
        public string Login { get; set; }
        public string Xin { get; set; }

        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        public RefKey AdrCountry { get; set; }
        public string AdrPostCode { get; set; }
        public string AdrStreet { get; set; }
        public string AdrRegion { get; set; }

        public string AdrPhone { get; set; }
        public string AdrFax { get; set; }
        public string AdrEmail { get; set; }

        public string NameEn { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }

        public int PageCount { get; set; }
        public int CopyCount { get; set; }
        public File RequisitionFile { get; set; }

        public EGovPay Pay { get; set; }

        public RefKey[] BlockColor { get; set; }
        public RefKey[] BlockClassification { get; set; }
        public EarlyReg[] BlockEarlyReg { get; set; }
        public Customer[] BlockCustomer { get; set; }
        public AttachedFile[] BlockFile { get; set; }
    }
}