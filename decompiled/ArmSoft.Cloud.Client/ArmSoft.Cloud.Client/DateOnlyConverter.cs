using System.Runtime.InteropServices;
using Newtonsoft.Json.Converters;

namespace ArmSoft.Cloud.Client;

[ComVisible(false)]
public class DateOnlyConverter : IsoDateTimeConverter
{
	public DateOnlyConverter()
	{
		DateTimeFormat = "yyyy'-'MM'-'dd";
	}
}
