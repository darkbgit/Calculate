using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Cylindrical;

namespace CalculateVessels.Core.Helpers.Json;


public class CommonDataConverter : JsonConverter<ShellCalculatedCommonData>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(ShellCalculatedCommonData).IsAssignableFrom(typeToConvert);
    }

    public override ShellCalculatedCommonData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (var jsonDoc = JsonDocument.ParseValue(ref reader))
        {
            //if the property isn't there, let it blow up
            switch (jsonDoc.RootElement.GetProperty("Type").GetString())
            {
                case nameof(CylindricalShellCalculatedCommon):
                    return jsonDoc.RootElement.Deserialize<CylindricalShellCalculatedCommon>(options);
                //warning: If you're not using the JsonConverter attribute approach,
                //make a copy of options without this converter
                default:
                    throw new JsonException("'Type' doesn't match a known derived type");
            }

        }
    }

    public override void Write(Utf8JsonWriter writer, ShellCalculatedCommonData data, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)data, options);
        //warning: If you're not using the JsonConverter attribute approach,
        //make a copy of options without this converter
    }
}
