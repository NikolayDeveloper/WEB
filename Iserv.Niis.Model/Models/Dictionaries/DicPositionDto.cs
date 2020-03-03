namespace Iserv.Niis.Model.Models.Dictionaries
{
    public class DicPositionDto : BaseDictionaryDto
    {
        public int? DepartmentId { get; set; }

        public int PositionTypeId { get; set; }
        public int PositionTypeCode { get; set; }
    }
}
