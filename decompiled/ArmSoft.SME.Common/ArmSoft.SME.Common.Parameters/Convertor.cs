using System;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Common.Parameters;

public static class Convertor
{
	public static string ConvertStringToString(string value)
	{
		if (value == null)
		{
			return "";
		}
		return value.ToString();
	}

	public static DateTime? ConvertStringToDate(string parValue)
	{
		if (string.IsNullOrWhiteSpace(parValue))
		{
			return null;
		}
		return parValue.YYYYMMDDHHMMSSToNullableDate();
	}

	public static DateOnly? ConvertStringToDateOnly(string parValue)
	{
		parValue = parValue.Trim();
		if (parValue != "00000000" && !string.IsNullOrWhiteSpace(parValue))
		{
			return parValue.YYYYMMDDToDateOnly();
		}
		return null;
	}

	public static int ConvertStringToInt(string parValue)
	{
		if (int.TryParse(parValue, out var result))
		{
			return result;
		}
		return 0;
	}

	public static decimal ConvertStringToDecimal(string parValue)
	{
		if (decimal.TryParse(parValue, out var result))
		{
			return result;
		}
		return 0m;
	}

	public static bool ConvertStringToBool(string parValue)
	{
		return parValue.CBool();
	}

	public static string ConvertDateToString(DateTime? parValue)
	{
		return parValue.NullableDateToYYYYMMDDHHMMSS();
	}

	public static string ConvertDateOnlyToString(DateOnly? parValue)
	{
		return parValue.FormatYYYYMMDD();
	}
}
