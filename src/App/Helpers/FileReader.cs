using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace App.Helpers;

public static class FileReader
{
    public static T? DeserializeJson<T>(string jsonPath)
    {
        var json = File.ReadAllText(jsonPath);
        return JsonSerializer.Deserialize<T>(json, Options);
    }

    private static JsonSerializerOptions Options => new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new DateConverter() }
    };
}

internal class DateConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.ParseExact(reader.GetString() ?? "", Configuration.DateFormat, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Configuration.DateFormat));
    }
}
