using System.ComponentModel.DataAnnotations.Schema;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;

namespace Iserv.Niis.Domain.Entities.Request
{
    public class RequestInfo : Entity<int>, IHaveConcurrencyToken
    {
        public bool FlagNine { get; set; }
        public bool FlagTth { get; set; }
        public bool FlagTtw { get; set; }
        public bool FlagTpt { get; set; }
        public bool FlagTat { get; set; }
        public bool FlagHeirship { get; set; } //FLAG_TN
        public bool? IzCollectiveTZ { get; set; } //COL_TZ
        public bool? IsConventionPriority { get; set; } //PRIOR_TZ
        public string Priority { get; set; } //TM_PRIORITET
        public bool? IsExhibitPriority { get; set; } //AWARD_TZ
        public bool? IsStandardFont { get; set; } //FONT_TZ
        [NotMapped]
        public bool? IsTransliteration { get; set; } //TRANS_TZ
        public string Transliteration { get; set; } //TM_TRANSLIT
        [NotMapped]
        public bool? IsTranslation { get; set; } //INVERT_TZ
        public string Translation { get; set; } //TM_TRANSLATE
        public bool? IsVolumeTZ { get; set; } //D3_TZ
        public bool? IsColorPerformance { get; set; } //COLOR_TZ
        public bool? AcceptAgreement { get; set; } //REG_TZ
        public string BreedingNumber { get; set; } //SEL_NOMER
        public string Breed { get; set; } //SEL_ROOT
        public string Genus { get; set; } //SEL_FAMILY
        public string ProductType { get; set; } //pn_goods
        public string ProductSpecialProp { get; set; } //pn_dsc и FlProductSpecialProp
        public string ProductPlace { get; set; } //PN_PLACE и FlProductPalce
        public int? BreedCountryId { get; set; }
        public DicCountry BreedCountry { get; set; }
        public int RequestId { get; set; }
        public Entities.Request.Request Request { get; set; }
    }
}