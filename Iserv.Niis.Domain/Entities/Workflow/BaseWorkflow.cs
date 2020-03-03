using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.Workflow
{
    public abstract class BaseWorkflow : Entity<int>, IHaveConcurrencyToken
    {
        public int? FromStageId { get; set; }
        public DicRouteStage FromStage { get; set; }
        public int? FromUserId { get; set; }
        public ApplicationUser FromUser { get; set; }
        public int? CurrentStageId { get; set; }
        public DicRouteStage CurrentStage { get; set; }
        public int? CurrentUserId { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public bool? IsComplete { get; set; }
        public bool? IsSystem { get; set; }
        public bool IsMain { get; set; }
        public string Description { get; set; }
        public int? RouteId { get; set; }
        public DicRoute Route { get; set; }
        public DateTimeOffset? ControlDate { get; set; }
        public DateTimeOffset? DateReceived { get; set; }

        /// <summary>
        /// Предыдущий этап
        /// </summary>
        public int? PreviousWorkflowId { get; set; }
    }
}