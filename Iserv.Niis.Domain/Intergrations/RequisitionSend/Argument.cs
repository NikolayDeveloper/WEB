using System.Xml.Serialization;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.BlockClassifications;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.BlockColors;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.BlockCustomers;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.BlockEarlyRegs;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.BlockFiles;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.BlockMessageFiles;
using Iserv.Niis.Domain.Intergrations.RequisitionSend.RequisitionFiles;

namespace Iserv.Niis.Domain.Intergrations.RequisitionSend
{
    /// <summary>
    /// Респонс отправки заявки в ЛК
    /// </summary
    public class Argument
    {
        /// <remarks/>
        public DocInfo DocInfo { get; set; }

        /// <remarks/>
        public string Stage { get; set; }

        /// <remarks/>
        public string AdrCustomerName { get; set; }

        /// <remarks/>
        public string BreedingNumber { get; set; }

        /// <remarks/>
        public BreedCountry BreedCountry { get; set; }

        /// <remarks/>
        public string ProductType { get; set; }

        /// <remarks/>
        public string ProductSpecialProp { get; set; }

        /// <remarks/>
        public string ProductPalce { get; set; }

        /// <remarks/>
        public string Breed { get; set; }

        /// <remarks/>
        public string Genus { get; set; }

        /// <remarks/>
        public string GenusEn { get; set; }

        /// <remarks/>
        public PatentType PatentType { get; set; }

        /// <remarks/>
        public bool RequirementsLaw { get; set; }

        /// <remarks/>
        public string EarlyRegNum { get; set; }

        /// <remarks/>
        public bool LawTTWp2s10 { get; set; }

        /// <remarks/>
        public bool AssignmentTPT { get; set; }

        /// <remarks/>
        public bool AssignmentAuthorTAT { get; set; }

        /// <remarks/>
        public bool HeirshipTN { get; set; }

        /// <remarks/>
        public bool IsCollectiveTradeMark { get; set; }

        /// <remarks/>
        public bool IsTMInStandartFont { get; set; }

        /// <remarks/>
        public bool IsTMVolume { get; set; }

        /// <remarks/>
        public string Translation { get; set; }

        /// <remarks/>
        public string Transliteration { get; set; }

        /// <remarks/>
        public CustomerType CustomerType { get; set; }

        /// <remarks/>
        public string CustomerName { get; set; }

        /// <remarks/>
        public string Login { get; set; }

        /// <remarks/>
        public string Xin { get; set; }

        /// <remarks/>
        public string Phone { get; set; }

        /// <remarks/>
        public string Fax { get; set; }

        /// <remarks/>
        public string Email { get; set; }

        /// <remarks/>
        public AdrCountry AdrCountry { get; set; }

        /// <remarks/>
        public string AdrPostCode { get; set; }

        /// <remarks/>
        public string AdrStreet { get; set; }

        /// <remarks/>
        public string AdrRegion { get; set; }

        /// <remarks/>
        public string AdrPhone { get; set; }

        /// <remarks/>
        public string AdrFax { get; set; }

        /// <remarks/>
        public string AdrEmail { get; set; }

        /// <remarks/>
        public string NameEn { get; set; }

        /// <remarks/>
        public string NameRu { get; set; }

        /// <remarks/>
        public string NameKz { get; set; }

        /// <remarks/>
        public string PageCount { get; set; }

        /// <remarks/>
        public string CopyCount { get; set; }

        /// <remarks/>
        public RequisitionFile RequisitionFile { get; set; }

        /// <remarks/>
        public Pay Pay { get; set; }

        /// <remarks/>
        public BlockColor BlockColor { get; set; }

        /// <remarks/>
        public BlockClassification BlockClassification { get; set; }

        /// <remarks/>
        public BlockEarlyReg BlockEarlyReg { get; set; }

        /// <remarks/>
        public BlockCustomer BlockCustomer { get; set; }

        /// <remarks/>
        public BlockFile BlockFile { get; set; }

        /// <remarks/>
        public BlockMessageFile BlockMessageFile { get; set; }
    }
}