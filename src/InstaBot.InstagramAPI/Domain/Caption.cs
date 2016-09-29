using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Domain
{
    public class Caption
    {

        [JsonProperty("pk")]
        public long Pk { get; set; }
        [JsonProperty("bit_flags")]
        public short Flags { get; set; }
        [JsonProperty("comment")]
        public string ContentType { get; set; }
        [JsonProperty("created_at")]
        public long CreateAt { get; set; }
        [JsonProperty("created_at_utc")]
        public long CreateAtUtc { get; set; }
        [JsonProperty("media_id")]
        public long MediaId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("type")]
        public short Type { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }
        [JsonProperty("user_id")]
        public long UserId { get; set; }
    }
}