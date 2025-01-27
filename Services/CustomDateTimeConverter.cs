using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Rentbook.Services
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string dateString = reader.GetString();
            return DateTime.Parse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("dddd, MMMM d, yyyy HH:mm:ss", CultureInfo.InvariantCulture));
        }
    }
}
