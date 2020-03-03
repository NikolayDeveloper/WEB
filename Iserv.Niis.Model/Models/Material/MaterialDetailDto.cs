using System;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Models.Material
{
    public class MaterialDetailDto
    {
        public MaterialDetailDto()
        {
            IsReadOnly = false;
            WorkflowDtos = new MaterialWorkflowDto[0];
            CommentDtos = new DocumentCommentDto[0];
        }

        public int? Id { get; set; }
        public int TypeId { get; set; }
        public int? MainAttachmentId { get; set; }
        public AttachmentDto Attachment { get; set; }
        public bool? WasScanned { get; set; }
        public int? PageCount { get; set; }
        public MaterialOwnerDto[] Owners { get; set; }
        public Owner.Type OwnerType { get; set; }
        public DocumentType DocumentType { get; set; }
        public int? Barcode { get; set; }
        public DateTimeOffset? DateCreate { get; set; }
        public bool HasSecondaryAttachment { get; set; }
        public string Code { get;set; }

        /// <summary>
        /// Список всех этапов
        /// </summary>
        public MaterialWorkflowDto[] WorkflowDtos { get; set; }
        public DocumentCommentDto[] CommentDtos { get; set; }

        public DocumentLinkDto[] DocumentLinkDtos { get; set; }
        public DocumentLinkDto[] DocumentParentLinkDtos { get; set; }

        /// <summary>
        /// Текущий этап
        /// </summary>
        //public int? CurrentWorkflowId { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public int? StatusId { get; set; }
        public string StatusCode { get; set; }
        public string StatusNameRu { get; set; }

        /// <summary>
        /// Только просмотр
        /// </summary>
        public bool IsReadOnly { get; set; }
    }
}
