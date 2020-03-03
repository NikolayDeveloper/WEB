using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Models
{
    public class ConvertDto
    {
        public int OwnerId { get; set; }
        public Owner.Type OwnerType { get; set; }
        public string ColectiveTrademarkParticipantsInfo { get; set; }
    }

    public class ConvertResponseDto
    {
        public string ColectiveTrademarkParticipantsInfo { get; set; }
        public int? SpeciesTradeMarkId { get; set; }
        public string SpeciesTrademarkCode { get; set; }
    }
}
