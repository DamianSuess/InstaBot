using Newtonsoft.Json;

namespace InstaBot.Console.Model
{
    public class BaseResponseMessage : BaseMessage
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}