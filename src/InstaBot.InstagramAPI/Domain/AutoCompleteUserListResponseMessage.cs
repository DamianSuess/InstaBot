using System.Collections.Generic;
using InstaBot.InstagramAPI.Domain;
using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Domain
{
    public class AutoCompleteUserListResponseMessage : BaseResponseMessage
    {

        [JsonProperty("expires")]
        public string Expire { get; set; }

        [JsonProperty("users")]
        public List<User> Users { get; set; }
    }
}