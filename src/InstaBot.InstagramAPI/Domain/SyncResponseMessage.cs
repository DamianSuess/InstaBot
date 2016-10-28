using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Domain
{
    public class SyncResponseMessage : BaseResponseMessage
    {
        [JsonProperty("experiments")]
        public string Experiments { get; set; }
    }
}