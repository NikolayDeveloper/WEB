using System.Collections.Generic;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Models.Material
{
    public class UserInputDto
    {
        public int OwnerId { get; set; }
        public int DocumentId { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public IList<KeyValuePair<string, string>> Fields { get; set; }
        public IList<int> SelectedRequestIds { get; set; }
        public int? PageCount { get; set; }
        public Owner.Type OwnerType { get; set; }
        public int? Index { get; set; }
    }
}