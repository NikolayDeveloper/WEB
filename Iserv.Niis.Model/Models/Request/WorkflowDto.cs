using System;

namespace Iserv.Niis.Model.Models.Request
{
    public class WorkflowDto
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
        public string CurrentUserNameRu { get; set; }
        public bool? IsComplete { get; set; }
        public bool? IsSystem { get; set; }
        public bool IsAuto { get; set; }
        public bool IsMain { get; set; }
        public string Description { get; set; }
        public int? RouteId { get; set; }
        public string RouteNameRu { get; set; }
        public DateTimeOffset? ControlDate { get; set; }
        public DateTimeOffset? DateReceived { get; set; }

        // Extra data fields
        public int? StatusId { get; set; }
        public DateTimeOffset? ContractGosDate { get; set; }
        public int? FullExpertiseExecutorId { get; set; }
        public string ContractNum { get; set; }
        public DateTimeOffset? ApplicationDateCreate { get; set; }
    }
}