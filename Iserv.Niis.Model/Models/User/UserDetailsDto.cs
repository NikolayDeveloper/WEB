namespace Iserv.Niis.Model.Models.User
{
    public class UserDetailsDto
    {
        public int Id { get; set; }
        public bool? IsLocked { get; set; }
        public string Xin { get; set; }
        public string NameRu { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int DivisionId { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public string PositionTypeNameRu { get; set; }
        public string PositionTypeCode { get; set; }
        public int? CustomerId { get; set; }
        public int[] RoleIds { get; set; }
        public int[] IcgsIds { get; set; }
        public int[] IpcIds { get; set; }

        /// <summary>
        /// Пароль от сертефиката
        /// </summary>
        public string CertPassword { get; set; }

        /// <summary>
        /// Путь к сертефикату
        /// </summary>
        public string CertStoragePath { get; set; }
    }
}