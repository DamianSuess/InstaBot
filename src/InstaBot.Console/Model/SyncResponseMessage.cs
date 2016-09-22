using Newtonsoft.Json;

namespace InstaBot.Console.Model
{
    public class SyncResponseMessage : BaseResponseMessage
    {
        [JsonProperty("experiments")]
        public string Experiments { get; set; }
    }
}