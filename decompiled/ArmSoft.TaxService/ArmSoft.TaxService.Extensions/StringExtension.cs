using System;

namespace ArmSoft.TaxService.Extensions;

public static class StringExtension
{
	public static string Right(this string str, int length)
	{
		if (length < 0)
		{
			throw new ArgumentException("Argument 'Length' must be greater or equal to zero.");
		}
		if (length == 0 || str == null)
		{
			return "";
		}
		int length2 = str.Length;
		if (length >= length2)
		{
			return str;
		}
		return str.Substring(checked(length2 - length), length);
	}
}
