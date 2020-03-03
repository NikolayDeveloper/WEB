using Newtonsoft.Json;

namespace Iserv.Niis.Notifications.Models
{
    public class SmsResponceModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("cnt")]
        public int Cnt { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
    }
}
