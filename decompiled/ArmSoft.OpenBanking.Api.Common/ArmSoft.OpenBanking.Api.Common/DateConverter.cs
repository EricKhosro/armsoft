using Newtonsoft.Json.Converters;

namespace ArmSoft.OpenBanking.Api.Common;

public class DateConverter : IsoDateTimeConverter
{
	public DateConverter()
	{
		DateTimeFormat = "yyyy-MM-dd";
	}
}
