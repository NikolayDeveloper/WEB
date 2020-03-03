using System;

namespace Iserv.Niis.Model.Models.Material.Incoming
{
    public class MaterialIncomingDataDto
    {
        public int? ParentId { get; set; }
        public string ParentType { get; set; } //Request, ...
        public AttachmentDto[] Attachments { get; set; }
    }
}
