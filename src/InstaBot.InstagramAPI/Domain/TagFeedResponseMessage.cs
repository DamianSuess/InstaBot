using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Domain
{
    public class TagFeedResponseMessage : BaseResponseMessage
    {
        [JsonProperty("num_results")]
        public int CountResult { get; set; }
        [JsonProperty("ranked_items;")]
        public bool RankedItems { get; set; }
        [JsonProperty("auto_load_more_enabled")]
        public bool IsAutoLoadMoreEnabled { get; set; }
        [JsonProperty("more_available")]
        public bool IsMoreAvailable { get; set; }
        [JsonProperty("next_max_id")]
        public string NextMaxId { get; set; }
        [JsonProperty("max_id")]
        public int? MaxId { get; set; }
        [JsonProperty("items")]
        public List<Media> Items { get; set; }
    }
}