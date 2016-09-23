using Newtonsoft.Json;

namespace InstaBot.Console.Model
{
    public class Item
    {
        [JsonProperty("media")]
        public Media Media { get; set; }
    }
}