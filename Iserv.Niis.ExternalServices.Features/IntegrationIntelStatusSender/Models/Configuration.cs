namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Models
{
    public class Configuration
    {
        public string ConStringNiisWeb { get; set; }
        public string ConStringNiisIntegration { get; set; }
        public double CheckPeriodInMinutes { get; set; }
        public string KazPatentWebServiceUrl { get; set; }
        public int DocWaitTimeInMinutes { get; set; }
        public string PepUrl { get; set; }
        public int RequestTimeoutInMinutes { get; set; }
        public string LogXmlDir { get; set; }
    }
}