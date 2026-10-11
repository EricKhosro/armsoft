using System.Text.Json;

namespace ArmSoft.TaxService;

public class DiscountConverter : DecimalNullableConverter
{
	public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
	{
		if (!value.HasValue || value.Value == 0m)
		{
			writer.WriteNullValue();
		}
		else
		{
			writer.WriteNumberValue(value.Value);
		}
	}
}
