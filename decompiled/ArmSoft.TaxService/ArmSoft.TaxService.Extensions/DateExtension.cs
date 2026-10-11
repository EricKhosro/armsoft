using System;

namespace ArmSoft.TaxService.Extensions;

public static class DateExtension
{
	public static string FormatDDMMYY(this DateTime Value)
	{
		return Value.Day.ToString("00") + "/" + Value.Month.ToString("00") + "/" + ("00" + Value.Year).Right(2);
	}
}
