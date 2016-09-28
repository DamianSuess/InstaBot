using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Domain
{
    public class Candidate
    {
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}