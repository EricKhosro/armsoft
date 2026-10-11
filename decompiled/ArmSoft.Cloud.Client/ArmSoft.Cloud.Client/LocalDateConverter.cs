using System.Runtime.InteropServices;
using Newtonsoft.Json.Converters;

namespace ArmSoft.Cloud.Client;

[ComVisible(false)]
public class LocalDateConverter : IsoDateTimeConverter
{
	public LocalDateConverter()
	{
		DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
	}
}
