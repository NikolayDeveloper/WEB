using Iserv.Niis.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.Model.Models.Material.Incoming
{
    public class MaterialTaskDto
    {
        public int? Id { get; set; }
        public string IncomingNumber { get; set; }
        public string OutgoingNumber { get; set; }
        public string DisplayNumber { get; set; }
        public DocumentType DocumentType { get; set; }
        public int TypeId { get; set; }
        public string TypeNameRu { get; set; }
        public int? Barcode { get; set; }
        public DateTimeOffset? DateCreate { get; set; }
        public string CurrentStageUser { get; set; }
        public int? CurrentStageUserId { get; set; }
        public string Creator { get; set; }
        public bool WasScanned { get; set; }
        public bool CanDownload { get; set; }
        /// <summary>
        /// Только просмотр
        /// </summary>
        public bool IsReadOnly { get; set; }
        public TaskPriority Priority { get; set; }
        public List<DocumentCommentDto> CommentDtos { get; set; }
        public int? StatusId { get; set; }
        public string StatusValue { get; set; }
    }
}