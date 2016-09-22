using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstaBot.Console.Model
{
    public class AutoCompleteUserListResponseMessage : BaseResponseMessage
    {

        [JsonProperty("expires")]
        public string Expire { get; set; }

        [JsonProperty("users")]
        public List<User> Users { get; set; }
    }
}