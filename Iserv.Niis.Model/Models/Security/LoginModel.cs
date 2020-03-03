namespace Iserv.Niis.Model.Models.Security
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CertData { get; set; }
        public bool IsCertificate { get; set; }
        public string PlainData { get; set; }
        public string SignedPlainData { get; set; }
        public int ?CurrentUserId {get;set;}
        public int ?DocumentId { get; set; }
        public string UserXin { get; set; }
    }
}
