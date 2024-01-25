using System;

using Application.Common.Extensions;

using Newtonsoft.Json;

namespace Application.Common.Converters
{
    public class BooleanConverter : JsonConverter
    {
        private readonly JsonSerializer DefaultSerializer = new();

        public override bool CanConvert(Type type)
        {
            return type.IsBooleanType();
        }

        public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Boolean:
                    return DefaultSerializer.Deserialize(reader, type);
                case JsonToken.Integer:
                    {
                        if (Convert.ToInt32(reader.Value) > 1)
                            throw new JsonSerializationException($"{reader.Value} is not a valid boolean value.");

                        return DefaultSerializer.Deserialize(reader, type);
                    }
                case JsonToken.Null:
                    throw new JsonSerializationException($"{reader.Path} must not be null.");
                default:
                    throw new JsonSerializationException($"Expected boolean value but got {reader.Value}");
            }
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}