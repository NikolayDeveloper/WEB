namespace Iserv.Niis.Model.Models.Role
{
    public class RoleDetailsDto
    {
        public int Id { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public string[] Permissions { get; set; }
        public int[] RoleStages { get; set; }
    }
}