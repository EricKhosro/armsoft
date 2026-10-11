using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common;

public class OnlyDateNullableConverter : JsonConverter<DateOnly?>
{
	public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		string text = reader.GetString();
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		return DateOnly.ParseExact(text, "yyyy'-'MM'-'dd", CultureInfo.InvariantCulture);
	}

	public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
	{
		if (value.HasValue)
		{
			writer.WriteStringValue(value.Value.ToString("yyyy'-'MM'-'dd"));
		}
		else
		{
			writer.WriteNullValue();
		}
	}
}
