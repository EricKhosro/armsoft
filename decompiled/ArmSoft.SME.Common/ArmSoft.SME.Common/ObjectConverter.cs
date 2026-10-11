using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common;

public class ObjectConverter : JsonConverter<object>
{
	public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		switch (reader.TokenType)
		{
		case JsonTokenType.String:
		{
			string text = reader.GetString();
			if (DateOnly.TryParseExact(text, "yyyy-MM-dd", out var result))
			{
				return result;
			}
			return text;
		}
		case JsonTokenType.Number:
		{
			int value;
			return reader.TryGetInt32(out value) ? ((decimal)value) : reader.GetDecimal();
		}
		case JsonTokenType.True:
			return true;
		case JsonTokenType.False:
			return false;
		case JsonTokenType.Null:
			return null;
		default:
			return JsonDocument.ParseValue(ref reader).RootElement.Clone();
		}
	}

	public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		if (value == null)
		{
			writer.WriteNullValue();
		}
		else if (!(value is int value2))
		{
			if (!(value is decimal value3))
			{
				if (!(value is bool value4))
				{
					if (!(value is string value5))
					{
						if (value is DateOnly dateOnly)
						{
							writer.WriteStringValue(dateOnly.ToString("yyyy-MM-dd"));
						}
						else
						{
							JsonSerializer.Serialize(writer, value, value.GetType(), options);
						}
					}
					else
					{
						writer.WriteStringValue(value5);
					}
				}
				else
				{
					writer.WriteBooleanValue(value4);
				}
			}
			else
			{
				writer.WriteNumberValue(value3);
			}
		}
		else
		{
			writer.WriteNumberValue(value2);
		}
	}
}
