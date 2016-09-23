using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstaBot.Console.Model
{
    public class TagResponseMessage : BaseResponseMessage
    {
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }

        [JsonProperty("results")]
        public List<Tag> Results { get; set; }
    }
}