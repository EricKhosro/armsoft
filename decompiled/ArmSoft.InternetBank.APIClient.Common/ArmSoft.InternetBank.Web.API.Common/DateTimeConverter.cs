using Newtonsoft.Json.Converters;

namespace ArmSoft.InternetBank.Web.API.Common;

public class DateTimeConverter : IsoDateTimeConverter
{
	public DateTimeConverter()
	{
		DateTimeFormat = "yyyy-MM-dd H:mm";
	}
}
