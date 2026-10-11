using System;

namespace ArmSoft.SME.Common.Extensions;

public static class TimeSpanExtensions
{
	public static string FormatHHMMSSOrMMSS(this TimeSpan value, string seperator = "")
	{
		if (value.Hours <= 0)
		{
			return value.ToString("mm\\" + seperator + "ss");
		}
		return value.ToString($"hh\\{seperator}mm\\{seperator}ss");
	}
}
