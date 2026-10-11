using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmSoft.TaxService;

public class TimeZoneNullableConverter : JsonConverter<DateTime?>
{
	public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (DateTime.TryParse(Convert.ToString(reader.GetString()), null, DateTimeStyles.AssumeLocal, out var result))
		{
			if (HasTimeComponent(result))
			{
				return result;
			}
			return result.Date;
		}
		return null;
	}

	public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
	{
		if (value.HasValue)
		{
			writer.WriteStringValue(value.Value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
		}
		else
		{
			writer.WriteNullValue();
		}
	}

	private bool HasTimeComponent(DateTime? dateTime)
	{
		if (Convert.ToDateTime(dateTime).ToUniversalTime().TimeOfDay != TimeSpan.Zero)
		{
			return Convert.ToDateTime(dateTime).TimeOfDay != new TimeSpan(4L);
		}
		return false;
	}
}
