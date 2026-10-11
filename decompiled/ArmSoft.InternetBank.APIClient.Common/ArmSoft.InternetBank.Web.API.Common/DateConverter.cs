using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArmSoft.InternetBank.Web.API.Common;

public class DateConverter : IsoDateTimeConverter
{
	public DateConverter()
	{
		DateTimeFormat = "yyyy-MM-dd";
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.Value is DateTime)
		{
			return (DateTime)reader.Value;
		}
		string text = reader.Value?.ToString();
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		string s = text;
		if (text.Contains("AM") || text.Contains("PM"))
		{
			s = text.Replace("T", " ");
		}
		if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault, out var result))
		{
			return result;
		}
		return base.ReadJson(reader, objectType, RuntimeHelpers.GetObjectValue(existingValue), serializer);
	}
}
