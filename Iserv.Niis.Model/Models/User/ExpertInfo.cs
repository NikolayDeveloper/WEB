namespace Iserv.Niis.Model.Models.User
{
    public class ExpertInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string IpcCodes { get; set; }
        public string RequestNumbers { get; set; }
        public int CountRequests { get; set; }
        public int CountCompletedRequestsCurrentYear { get; set; }
        public double EmploymentIndexExpert { get; set; }
    }
}
