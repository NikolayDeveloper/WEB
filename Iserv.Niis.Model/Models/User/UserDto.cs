namespace Iserv.Niis.Model.Models.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string NameRu { get; set; }
        public string RoleNameRu { get; set; }
        public string Email { get; set; }
        public int? DepartmentId { get; set; }
        public string DivisionNameRu { get; set; }
        public string DepartmentNameRu { get; set; }
        public string PositionNameRu { get; set; }
        public string PositionCode { get; set; }
        public string PositionPositionTypeCode { get; set; }
        public string PositionPositionTypeNameRu { get; set; }
    }
}