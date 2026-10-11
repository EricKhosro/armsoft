using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ArmSoft.OnlineBanking.Common.Extensions;

public static class CommonExtension
{
	public static StringBuilder AppendLineFormat(this StringBuilder sb, string format, params object[] args)
	{
		sb.AppendFormat(CultureInfo.InvariantCulture, format, args);
		sb.AppendLine();
		return sb;
	}

	public static string Join<T>(this IEnumerable<T> values)
	{
		return string.Join(",", values);
	}

	public static bool IsBetween(this int num, int lower, int upper)
	{
		if (lower <= num)
		{
			return num <= upper;
		}
		return false;
	}

	public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
	{
		if (collection != null)
		{
			return collection.Count == 0;
		}
		return true;
	}
}
