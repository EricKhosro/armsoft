using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common;

public class IgnoreTimeZoneNullableConverter : JsonConverter<DateTime?>
{
	public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (string.IsNullOrEmpty(reader.GetString()))
		{
			return null;
		}
		return DateTimeOffset.Parse(reader.GetString()).DateTime;
	}

	public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
	{
		if (value.HasValue)
		{
			writer.WriteStringValue(value.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
		}
		else
		{
			writer.WriteNullValue();
		}
	}
}
