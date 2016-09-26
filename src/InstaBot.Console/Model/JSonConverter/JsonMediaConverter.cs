using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace InstaBot.Console.Model.JSonConverter
{
    public class JsonItemConverter : JsonCreationConverter<List<Media>>
    {
        private class Item
        {
            [JsonProperty("media")]
            public Media Media { get; set; }
        }

        protected override List<Media> Create(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var medias = new List<Media>();
            var surrogate = serializer.Deserialize<List<Item>>(reader);
            medias = surrogate.Select(x => x.Media).ToList();
            return medias;
        }
    }
}
