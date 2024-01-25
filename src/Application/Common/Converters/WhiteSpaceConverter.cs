using Newtonsoft.Json;
using System;

namespace Application.Common.Converters
{
    public class WhiteSpaceConverter : JsonConverter
    {
        private readonly JsonSerializer DefaultSerializer = new();

        public override bool CanConvert(Type type)
        {
            return (type == typeof(string));
        }

        public override bool CanWrite { get { return false; } }

        public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
        {
            var trimmedValue = reader.Value?.ToString().Trim();

            return trimmedValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}