using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common;

public class IgnoreTimeZoneConverter : JsonConverter<DateTime>
{
	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return DateTimeOffset.Parse(reader.GetString()).DateTime;
	}

	public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
	}
}
