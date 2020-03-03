using System;
using System.Collections.Generic;

namespace Iserv.Niis.BusinessLogic.Search.DTOs
{
    public class ExpertSearchFilterConditionDto
    {
        public string Author { get; set; }
        public string Name { get; set; }
        public string Formula { get; set; }
        public string Referat { get; set; }
        public string Description { get; set; }
        public string Transliteration { get; set; }
        public string Translation { get; set; }
        public string RequestNumber { get; set; }
        public string GosNumber { get; set; }
        public string DeclarantName { get; set; }
        public int? DeclarantCountryId { get; set; }
        public string DeclarantOblast { get; set; }
        public string DeclarantCity { get; set; }
        public string PatentAttorneyName { get; set; }
        public string PatentAttorneyNumber { get; set; }
        public string OwnerName { get; set; }
        public int? OwnerCountryId { get; set; }
        public string OwnerOblast { get; set; }
        public string OwnerCity { get; set; }
        public List<int> RequestStatusIds { get; set; }
        public List<int> ProtectionDocStatusIds { get; set; }
        public List<int> IcgsIds { get; set; }
        public List<int> IcfemIds { get; set; }
        public List<int> Icis { get; set; }
        public int? TrademarkTypeId { get; set; }
        public int? TrademarkKindId { get; set; }

        public List<string> IpcCodes { get; set; }
        public List<string> IpcDescriptions { get; set; }

        public List<string> IcgsDescriptions { get; set; }
        public DateTimeOffset? RegisterDateFrom { get; set; }
        public DateTimeOffset? RegisterDateTo { get; set; }
        public DateTimeOffset? RequestDateFrom { get; set; }
        public DateTimeOffset? RequestDateTo { get; set; }
        public DateTimeOffset? PublishDateFrom { get; set; }
        public DateTimeOffset? PublishDateTo { get; set; }
        public DateTimeOffset? GosDateFrom { get; set; }
        public DateTimeOffset? GosDateTo { get; set; }
        public bool? IsWellKnown { get; set; }
    }
}