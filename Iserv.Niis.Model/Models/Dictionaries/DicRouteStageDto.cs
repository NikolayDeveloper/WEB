namespace Iserv.Niis.Model.Models.Dictionaries
{
    public class DicRouteStageDto : BaseDictionaryDto
    {
        public bool IsSystem { get; set; }
        public int? Interval { get; set; }
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }
        public bool IsMultiUser { get; set; }
        public bool? IsReturnable { get; set; }
        public bool IsAuto { get; set; }
        public bool IsMain { get; set; }
        public int? OnlineRequisitionStatusId { get; set; }
        public int? RouteId { get; set; }
    }
}