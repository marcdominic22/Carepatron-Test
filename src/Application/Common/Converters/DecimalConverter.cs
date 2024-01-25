using System;

using Application.Common.Extensions;

using Newtonsoft.Json;

namespace Application.Common.Converters
{
    public class DecimalConverter : JsonConverter
    {
        private readonly JsonSerializer DefaultSerializer = new();

        public override bool CanConvert(Type type)
        {
            return (type == typeof(decimal) || type == typeof(decimal?));
        }

        public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Float:
                    return DefaultSerializer.Deserialize(reader, type);
                case JsonToken.Integer:
                    return DefaultSerializer.Deserialize(reader, type);
                case JsonToken.String:
                    {
                        bool isDecimal = Decimal.TryParse(reader.Value.ToString(), out _);
                        if (isDecimal)
                        {
                            return DefaultSerializer.Deserialize(reader, type);
                        }
                        throw new JsonSerializationException($"{reader.Value} is not a valid decimal value.");
                    }
                case JsonToken.Null:
                    return null;
                default:
                    throw new JsonSerializationException($"Expected decimal value but got {reader.Value}");
            }
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}