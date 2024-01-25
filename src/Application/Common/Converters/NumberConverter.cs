using System;

using Application.Common.Exceptions;
using Application.Common.Extensions;

using FluentValidation.Results;

using Newtonsoft.Json;

namespace Application.Common.Converters
{
    public class NumberConverter : JsonConverter
    {
        private readonly JsonSerializer DefaultSerializer = new JsonSerializer();

        public override bool CanConvert(Type type)
        {
            return type.IsNumberType();
        }

        public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    if (reader.Value.ToString().Length <= 10)
                    {
                        return DefaultSerializer.Deserialize(reader, type);
                    }
                    throw new ValidationException(new ValidationFailure(reader.Path, "Length must be 10 Digit or fewer"));
                case JsonToken.Null:
                    return DefaultSerializer.Deserialize(reader, type);
                case JsonToken.String:
                    if (int.TryParse(reader.Value.ToString(), out int value))
                    {
                        return DefaultSerializer.Deserialize(reader, type);
                    };
                    throw new ValidationException(new ValidationFailure(reader.Path, $"Expected number value but got {reader.Value}"));
                default:
                    throw new ValidationException(new ValidationFailure(reader.Path, $"Expected number value but got {reader.Value}"));
            }
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}