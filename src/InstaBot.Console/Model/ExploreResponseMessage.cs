using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstaBot.Console.Model
{
    public class Item
    {
        public Item()
        {
            LikeCount = 0;
        }

        [JsonProperty("pk")]
        public string Pk { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("like_count")]
        public int LikeCount { get; set; }
    }
    public class ExploreResponseMessage : BaseResponseMessage
    {
        [JsonProperty("num_results")]
        public int CountResult { get; set; }
        [JsonProperty("auto_load_more_enabled")]
        public bool IsAutoLoadMoreEnabled { get; set; }
        [JsonProperty("more_available")]
        public bool IsMoreAvailable { get; set; }
        [JsonProperty("next_max_id")]
        public int? NextMaxId { get; set; }
        [JsonProperty("max_id")]
        public int? MaxId { get; set; }
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }
}