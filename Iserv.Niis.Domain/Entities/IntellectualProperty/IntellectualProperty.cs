using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Domain.EntitiesHistory.Patent;
using static System.String;

namespace Iserv.Niis.Domain.Entities.IntellectualProperty
{
    public class IntellectualProperty : Entity<int>, IHistorySupport, IHaveConcurrencyToken
    {
        public IntellectualProperty()
        {
            //LicenseInfoStateRegisters = new HashSet<string>();
            //Applicants = new HashSet<string>();
            //Authors = new HashSet<string>();
        }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }

        public int TypeId { get; set; }
        public DicProtectionDocType Type { get; set; }
        public int? ProtectionDocSubTypeId { get; set; } //SUBTYPE_ID
        public DicProtectionDocSubType ProtectionDocSubType { get; set; }
        public string DeclaredName { get; set; } //new_field
        public string RefusalToPublish { get; set; } //new_field
        public string AdditionalInfo { get; set; }
        public string FirstPubInfo { get; set; } //new_field
        //public ICollection<string> Applicants { get; set; } //new_field
        public string Attorney { get; set; } //new_field
        public string Confidant { get; set; } //new_field
        public string Patentee { get; set; } //new_field
        //public ICollection<string> Authors { get; set; } //new_field
        public int AddressId { get; set; }
        public DicAddress Address { get; set; }
        public int ApplicantTypeId { get; set; }
        public DicApplicantType ApplicantType { get; set; }
        public int StatusId { get; set; }
        public DicIntellectualPropertyStatus Status { get; set; } //new_field
        public int RequestId { get; set; }
        public Request.Request Request { get; set; }
        public string GosNumber { get; set; } //GOS_NUMBER_11
        public DateTimeOffset IssuePatentDate { get; set; } //DVIDOD
        public DateTimeOffset ValidDate { get; set; } //STZ17
        public DateTimeOffset ExtensionDate { get; set; }
        public string NumberBulletin { get; set; } //NBY
        public DateTimeOffset? BulletinDate { get; set; } //DBY
        public int? ConventionTypeId { get; set; }
        public DicConventionType ConventionType { get; set; }
        public int? ImageId { get; set; }
        public string Abstract { get; set; } //new_field
        public DateTimeOffset? EarlyTerminationDate { get; set; } //DATPDPAT
        public string NumberApxWork { get; set; } //KOD_OEI
        //public ICollection<string> LicenseInfoStateRegisters { get; set; } //LICENZ1
        public string NumberCopyrightCertificate { get; set; } //NAC
        public string PaperworkStateRegister { get; set; } //DVPP
        public DateTimeOffset? RecoveryPetitionDate { get; set; } //DHODVOST
        public string Transliteration { get; set; } //TRASLITERATION
        public ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }

        public Type GetHistoryEntity()
        {
            return typeof(PatentHistory);
        }

        public override string ToString()
        {
            var result = DateCreate.ToString("dd.MM.yyyy");
            var name = Empty;
            if (!IsNullOrEmpty(NameRu))
                name = NameRu;
            else if (!IsNullOrEmpty(NameKz))
                name = NameKz;
            else if (!IsNullOrEmpty(NameEn))
                name = NameEn;
            if (!IsNullOrEmpty(GosNumber))
                result = (IsNullOrEmpty(result) ? Empty : result + " - ") + GosNumber;
            if (!IsNullOrEmpty(name))
                result = (IsNullOrEmpty(result) ? Empty : result + " - ") + name;
            if (IsNullOrEmpty(result))
                return Id + Empty;
            return result;
        }
    }
}