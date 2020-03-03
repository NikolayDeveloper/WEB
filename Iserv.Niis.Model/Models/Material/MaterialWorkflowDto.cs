using System;

namespace Iserv.Niis.Model.Models.Material
{
    public class MaterialWorkflowDto
    {
        public int Id { get; set; }
        public int? OwnerId { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public int? FromStageId { get; set; }
        public string FromStageCode { get; set; }
        public string FromStageNameRu { get; set; }
        public int? CurrentStageId { get; set; }
        public string CurrentStageCode { get; set; }
        public bool CurrentStageIsSign { get; set; }
        public string CurrentStageNameRu { get; set; }
        public int? FromUserId { get; set; }
        public string FromUserNameRu { get; set; }
        public int? CurrentUserId { get; set; }
        public int? CurrentUserDepartmentId { get; set; }
        public string CurrentUserNameRu { get; set; }
        public bool? IsComplete { get; set; }
        public bool? IsSystem { get; set; }
        public string Description { get; set; }
        public int? RouteId { get; set; }
        public string RouteNameRu { get; set; }
        public DateTimeOffset? ControlDate { get; set; }
        public DateTimeOffset? DateReceived { get; set; }
        public string OutgoingNum { get; set; }
        public DateTimeOffset? DocumentDate { get; set; }
        public bool IsSigned { get; set; }

        /// <summary>
        /// Текущий этап
        /// </summary>
        public bool IsCurent { get; set; }

        /// <summary>
        /// Предыдущий этап
        /// </summary>
        public int? PreviousWorkflowId { get; set; }
    }
}
