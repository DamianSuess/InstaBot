using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Domain
{
    public class TimelineFeedResponseMessage : BaseResponseMessage
    {
        [JsonProperty("num_results")]
        public int CountResult { get; set; }
        [JsonProperty("is_direct_v2_enabled")]
        public bool IsDirectV2Enabled { get; set; }
        [JsonProperty("auto_load_more_enabled")]
        public bool IsAutoLoadMoreEnabled { get; set; }
        [JsonProperty("more_available")]
        public bool IsMoreAvailable { get; set; }
        [JsonProperty("next_max_id")]
        public string NextMaxId { get; set; }
    }
}