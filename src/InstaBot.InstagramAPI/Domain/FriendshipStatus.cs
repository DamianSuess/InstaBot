using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Domain
{
    public class FriendshipStatus
    {

        [JsonProperty("following")]
        public bool Following { get; set; }
        [JsonProperty("outgoing_request")]
        public bool? OutgoingRequest { get; set; }
        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }
    }
}