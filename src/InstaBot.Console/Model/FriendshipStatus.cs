using Newtonsoft.Json;

namespace InstaBot.Console.Model
{
    public class FriendshipStatus
    {

        [JsonProperty("following")]
        public bool Following { get; set; }
        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }
    }
}