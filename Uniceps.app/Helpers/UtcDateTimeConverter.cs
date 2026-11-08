using System.Text.Json;
using System.Text.Json.Serialization;

namespace Uniceps.app.Helpers
{
    public class UtcDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // لما الموبايل يبعث تاريخ للـ API (لو احتجت تقرأه)
            return DateTime.SpecifyKind(reader.GetDateTime(), DateTimeKind.Utc);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // لما الـ API يبعث تاريخ للموبايل: بتحوله لـ UTC وبتحط Z بالآخر
            var utcDateTime = value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
                : value.ToUniversalTime();

            writer.WriteStringValue(utcDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }
    }
}
