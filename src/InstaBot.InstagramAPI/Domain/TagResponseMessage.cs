using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Domain
{
    public class TagResponseMessage : BaseResponseMessage
    {
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }

        [JsonProperty("results")]
        public List<Tag> Results { get; set; }
    }
}