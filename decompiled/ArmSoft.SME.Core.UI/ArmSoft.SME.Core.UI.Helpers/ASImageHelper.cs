using System;

namespace ArmSoft.SME.Core.UI.Helpers;

public class ASImageHelper
{
	private static string CombineDayDateCalendarImageUri(DateOnly currentDate)
	{
		return $"Date_{currentDate:dd}";
	}

	public static string CombineDayDateCalendarLargeImageUri(DateOnly currentDate)
	{
		return CombineDayDateCalendarImageUri(currentDate) + "_32.svg";
	}

	public static string CombineDayDateCalendarSmallImageUri(DateOnly currentDate)
	{
		return CombineDayDateCalendarImageUri(currentDate) + "_16.svg";
	}
}
