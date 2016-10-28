using System;
using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Domain.JSonConverter
{
    public abstract class JsonCreationConverter<T> : JsonConverter
    {

        protected abstract T Create(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            T target = Create(reader, objectType, existingValue, serializer);
            return target;
        }

        public override void WriteJson(JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}