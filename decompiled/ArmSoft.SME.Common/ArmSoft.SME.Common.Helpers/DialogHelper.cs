using System;
using System.Collections.Generic;
using System.Text;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Common.Helpers;

public static class DialogHelper
{
	private static readonly string[] _separator = new string[1] { "\r\n" };

	public static Dictionary<string, string> GetNameValuePairs(string body)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string[] array = body.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
		foreach (string text in array)
		{
			int num = text.InStr(":");
			if (num > 1)
			{
				dictionary.Add(text.Left(num - 1), text.Mid(num + 1).TrimEnd());
			}
		}
		return dictionary;
	}

	public static string GetBodyString(Dictionary<string, string> values)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(" ");
		if (values != null)
		{
			foreach (KeyValuePair<string, string> value in values)
			{
				stringBuilder.Append(value.Key);
				stringBuilder.Append(":");
				stringBuilder.AppendLine(Convert.ToString(value.Value));
			}
		}
		return stringBuilder.ToString();
	}
}
