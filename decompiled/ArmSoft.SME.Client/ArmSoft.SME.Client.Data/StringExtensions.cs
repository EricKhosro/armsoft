using System;

namespace ArmSoft.SME.Client.Data;

public static class StringExtensions
{
	public static DateTime YYYYMMDDToDate(this string source)
	{
		int year = int.Parse(source.Substring(0, 4));
		int month = int.Parse(source.Substring(4, 2));
		int day = int.Parse(source.Substring(6, 2));
		return new DateTime(year, month, day);
	}
}
