using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmSoft.TaxService;

public class DecimalNullableConverter : JsonConverter<decimal?>
{
	public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		decimal value;
		return reader.TryGetDecimal(out value) ? value : 0m;
	}

	public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
	{
		if (value.HasValue)
		{
			writer.WriteNumberValue(value.Value);
		}
		else
		{
			writer.WriteNullValue();
		}
	}
}
