using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmSoft.TaxService;

public class TimeZoneConverter : JsonConverter<DateTime>
{
	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (DateTime.TryParse(Convert.ToString(reader.GetString()), out var result))
		{
			if (HasTimeComponent(result))
			{
				return result;
			}
			return result.Date;
		}
		return DateTime.MinValue;
	}

	public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
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
