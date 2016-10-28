using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Domain
{
    public class Tag
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("media_count")]
        public string MediaCount { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}