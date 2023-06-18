using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CalculateVessels.Core.Helpers.Json;

//public class InputDataConverter<T, TI> : JsonConverter<TI>
//    where T : class, TI
//{
//    public override TI? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        return JsonSerializer.Deserialize<T>(ref reader, options);
//    }

//    public override void Write(Utf8JsonWriter writer, TI value, JsonSerializerOptions options)
//    {
//        switch (value)
//        {
//            case null:
//                JsonSerializer.Serialize(writer, null, options);
//                break;
//            default:
//                {
//                    var type = value.GetType();
//                    JsonSerializer.Serialize(writer, value, type, options);
//                    break;
//                }
//        }
//    }
//}

public class CustomJsonConverter<T> : JsonConverter<T>
    where T : class
{

    private readonly IEnumerable<Type> _types;

    public CustomJsonConverter()
    {
        var type = typeof(T);
        _types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
            .ToList();
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        using (var jsonDocument = JsonDocument.ParseValue(ref reader))
        {
            if (!jsonDocument.RootElement.TryGetProperty("Type", out var typeProperty))
            {
                throw new JsonException();
            }

            var type = _types.FirstOrDefault(x => x.Name ==
                                                  typeProperty.GetString());
            if (type == null)
            {
                throw new JsonException();
            }

            var jsonObject = jsonDocument.RootElement.GetRawText();
            var result = (T)JsonSerializer.Deserialize(jsonObject, type, options);

            return result;
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case null:
                JsonSerializer.Serialize(writer, null, options);
                break;
            default:
                {
                    var type = value.GetType();
                    JsonSerializer.Serialize(writer, value, type, options);
                    break;
                }
        }
    }
}
