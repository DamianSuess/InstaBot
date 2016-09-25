using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstaBot.Console.Model
{
    public class Media
    {
        public Media()
        {
            LikeCount = 0;
            CommentCount = 0;
        }

        [JsonProperty("pk")]
        public long Pk { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }
        [JsonProperty("caption")]
        public Caption Caption { get; set; }
        [JsonProperty("caption_is_edited")]
        public bool IsCaptionEdited { get; set; }
        [JsonProperty("client_cache_key")]
        public string ClientCacheKey { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("like_count")]
        public int LikeCount { get; set; }
        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }
        [JsonProperty("comment")]
        public List<Comment> Comments { get; set; }
        [JsonProperty("has_more_comments")]
        public bool HasMoreComments { get; set; }
        [JsonProperty("device_timestamp")]
        public long DeviceTimestamp { get; set; }
        [JsonProperty("explore_context")]
        public string ExploreContext { get; set; }
        [JsonProperty("filter_type")]
        public int FilterType { get; set; }
        [JsonProperty("has_liked")]
        public bool HasLiked { get; set; }
        //TODO
        //[JsonProperty("image_versions2")]
        //public List<Candidate> ImageV2 { get; set; }
        [JsonProperty("impression_token")]
        public string ImpressionToken { get; set; }
        [JsonProperty("max_num_visible_preview_comments")]
        public int MaxNumVisiblePreviewComments { get; set; }
        [JsonProperty("media_type")]
        public int MediaType { get; set; }
        [JsonProperty("next_max_id")]
        public string NextMaxId { get; set; }
        [JsonProperty("organic_tracking_token")]
        public string OrganicTrackingToken { get; set; } 
        [JsonProperty("original_height")]
        public int OriginalHeight { get; set; } 
        [JsonProperty("original_width")]
        public int OriginalWidth { get; set; } 
        [JsonProperty("photo_of_you")]
        public bool PhotoOfYou { get; set; } 
        [JsonProperty("preview_comments")]
        public List<Comment> PreviewCommentsList { get; set; } 
        [JsonProperty("taken_at")]
        public long TakenAt { get; set; } 
        [JsonProperty("user")]
        public User User { get; set; } 
    }
}