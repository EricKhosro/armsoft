using System.Runtime.InteropServices;
using Newtonsoft.Json.Converters;

namespace ArmSoft.Cloud.Client;

[ComVisible(false)]
public class OnlyDateConverter : IsoDateTimeConverter
{
	public OnlyDateConverter()
	{
		DateTimeFormat = "yyyyMMdd";
	}
}
