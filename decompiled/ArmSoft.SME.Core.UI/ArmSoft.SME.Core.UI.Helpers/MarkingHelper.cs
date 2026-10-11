using System.Collections.Generic;
using System.Text;
using ArmSoft.SME.Common;

namespace ArmSoft.SME.Core.UI.Helpers;

public static class MarkingHelper
{
	private static readonly HashSet<char> _invalidChars = new HashSet<char> { '\u0001', '\u0002', '\u0003', '\u0017', '\u001a', '\u001b' };

	public static string GetMarkingStatus(int markingListCount, decimal rowQuantity)
	{
		if (markingListCount == 0)
		{
			return Resources.NeedToFill;
		}
		if ((decimal)markingListCount == rowQuantity)
		{
			return Resources.Complete;
		}
		if ((decimal)markingListCount > rowQuantity)
		{
			return Resources.GreaterThanCountInRow;
		}
		return Resources.InComplete;
	}

	public static void ValidateMarkingCode(string markingCode)
	{
		if (markingCode.Length < 29 || markingCode.Length > 110)
		{
			throw new ASException(string.Format(Resources.WrongMarkingFormat, (short)29, (short)110, markingCode));
		}
	}

	public static string CleanScannerSymbols(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			return "";
		}
		if (input.StartsWith('\u001d'))
		{
			input = input.Substring(1);
		}
		StringBuilder stringBuilder = new StringBuilder(input.Length);
		string text = input;
		foreach (char c in text)
		{
			if (!_invalidChars.Contains(c))
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}
}
