using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Common.Extensions;

public static class StringExtensions
{
	private static readonly CompareInfo invariantCompareInfo = CultureInfo.InvariantCulture.CompareInfo;

	public const string Space = " ";

	public static StringBuilder AppendWhereOrAnd(this StringBuilder sb, string condition, ref bool whereAdded)
	{
		if (whereAdded)
		{
			StringBuilder stringBuilder = sb;
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(5, 1, stringBuilder);
			handler.AppendLiteral(" and ");
			handler.AppendFormatted(condition);
			stringBuilder2.Append(ref handler);
		}
		else
		{
			StringBuilder stringBuilder = sb;
			StringBuilder stringBuilder3 = stringBuilder;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(7, 1, stringBuilder);
			handler.AppendLiteral(" where ");
			handler.AppendFormatted(condition);
			stringBuilder3.Append(ref handler);
			whereAdded = true;
		}
		return sb;
	}

	public static string AppendWhereOrAnd(this string sql, string condition, ref bool whereAdded)
	{
		if (whereAdded)
		{
			sql = sql + "\r\n  and " + condition;
		}
		else
		{
			sql = sql + "\r\nwhere " + condition;
			whereAdded = true;
		}
		return sql;
	}

	public static DateTime YYYYMMDDToDate(this string source)
	{
		int year = int.Parse(source.Substring(0, 4));
		int month = int.Parse(source.Substring(4, 2));
		int day = int.Parse(source.Substring(6, 2));
		return new DateTime(year, month, day);
	}

	public static DateOnly YYYYMMDDToDateOnly(this string source)
	{
		int year = int.Parse(source.Substring(0, 4));
		int month = int.Parse(source.Substring(4, 2));
		int day = int.Parse(source.Substring(6, 2));
		return new DateOnly(year, month, day);
	}

	public static DateOnly DDMMYYYYToDateOnly(this string source)
	{
		int day = int.Parse(source.Substring(0, 2));
		int month = int.Parse(source.Substring(3, 2));
		return new DateOnly(int.Parse(source.Substring(6, 4)), month, day);
	}

	public static PrintTemplateFileFormatSE ExtensionToTemplateFormat(this string extension)
	{
		PrintTemplateFileFormatSE result = null;
		if (!string.IsNullOrWhiteSpace(extension))
		{
			SmartEnum<PrintTemplateFileFormatSE, string>.TryFromValue(extension.Right(extension.Length - 1).ToUpper(), out result);
		}
		return result;
	}

	public static DateTime DDMMYYYYToDate(this string source)
	{
		int day = int.Parse(source.Substring(0, 2));
		int month = int.Parse(source.Substring(3, 2));
		return new DateTime(int.Parse(source.Substring(6, 4)), month, day);
	}

	public static DateTime DDMMYYToDate(this string source)
	{
		int day = int.Parse(source.Substring(0, 2));
		int month = int.Parse(source.Substring(3, 2));
		return new DateTime(int.Parse(source.Substring(6, 2)) + 2000, month, day);
	}

	public static bool CBool(this string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}
		if (value == "1")
		{
			return true;
		}
		if (value == "0")
		{
			return false;
		}
		return Convert.ToBoolean(value);
	}

	public static bool ToBoolean(this string value)
	{
		if (string.IsNullOrWhiteSpace(value) || value == "0")
		{
			return false;
		}
		if (value == "1")
		{
			return true;
		}
		value = value.Trim();
		if (string.Compare(value, "true", ignoreCase: true) == 0)
		{
			return true;
		}
		if (string.Compare(value, "false", ignoreCase: true) == 0)
		{
			return false;
		}
		if (bool.TryParse(value, out var result))
		{
			return result;
		}
		return false;
	}

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

	public static bool IsNumeric(this string str)
	{
		double result;
		return double.TryParse(str, out result);
	}

	public static string Left(this string str, int length)
	{
		if (length < 0)
		{
			throw new ArgumentException("Argument 'Length' must be greater or equal to zero.");
		}
		if (length == 0 || str == null)
		{
			return "";
		}
		if (length >= str.Length)
		{
			return str;
		}
		return str.Substring(0, length);
	}

	public static string LeftAlign(this string value, int length)
	{
		return value.PadRight(length).Substring(0, length);
	}

	public static string Mid(this string str, int start)
	{
		return str?.Mid(start, str.Length);
	}

	public static string Mid(this string str, int start, int length)
	{
		if (start <= 0)
		{
			throw new ArgumentException("Argument 'Start' must be greater than zero.");
		}
		if (length < 0)
		{
			throw new ArgumentException("Argument 'Length' must be greater or equal to zero.");
		}
		if (length == 0 || str == null)
		{
			return "";
		}
		int length2 = str.Length;
		if (start > length2)
		{
			return "";
		}
		checked
		{
			if (start + length > length2)
			{
				return str.Substring(start - 1);
			}
			return str.Substring(start - 1, length);
		}
	}

	public static int InStr(this string string1, string string2, bool binaryCompare = true)
	{
		checked
		{
			if (binaryCompare)
			{
				return InternalInStrBinary(0, string1, string2) + 1;
			}
			return InternalInStrText(0, string1, string2) + 1;
		}
	}

	public static int InStr(int start, string string1, string string2, bool binaryCompare = true)
	{
		if (start < 1)
		{
			throw new ArgumentException("Argument 'Start' must be greater than zero.");
		}
		checked
		{
			if (binaryCompare)
			{
				return InternalInStrBinary(start - 1, string1, string2) + 1;
			}
			return InternalInStrText(start - 1, string1, string2) + 1;
		}
	}

	private static int InternalInStrBinary(int startPos, string sSrc, string sFind)
	{
		int num = sSrc?.Length ?? 0;
		if (startPos > num || num == 0)
		{
			return -1;
		}
		if (sFind == null || sFind.Length == 0)
		{
			return startPos;
		}
		return invariantCompareInfo.IndexOf(sSrc, sFind, startPos, CompareOptions.Ordinal);
	}

	private static int InternalInStrText(int lStartPos, string sSrc, string sFind)
	{
		int num = sSrc?.Length ?? 0;
		if (lStartPos > num || num == 0)
		{
			return -1;
		}
		if (sFind == null || sFind.Length == 0)
		{
			return lStartPos;
		}
		return CultureInfo.CurrentCulture.CompareInfo.IndexOf(sSrc, sFind, lStartPos, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
	}

	public static string Replace(this string expression, string find, string replacement, int start = 1, int count = -1, bool binaryCompare = true)
	{
		if (count < -1)
		{
			throw new ArgumentException("Argument 'Count' must be greater than or equal to -1.");
		}
		if (start <= 0)
		{
			throw new ArgumentException("Argument 'Start' must be greater than zero.");
		}
		if (expression == null || start > expression.Length)
		{
			return null;
		}
		if (start != 1)
		{
			expression = expression.Substring(start - 1);
		}
		if (find == null || find.Length == 0 || count == 0)
		{
			return expression;
		}
		if (count == -1)
		{
			count = expression.Length;
		}
		return expression.ReplaceInternal(find, replacement, count, binaryCompare);
	}

	public static int InStrRev(this string stringCheck, string stringMatch, int start = -1, bool binaryCompare = true)
	{
		if (start == 0 || start < -1)
		{
			throw new ArgumentException("Argument 'Start' must be greater than or equal to -1.");
		}
		int num = stringCheck?.Length ?? 0;
		if (start == -1)
		{
			start = num;
		}
		if (start > num || num == 0)
		{
			return 0;
		}
		if (stringMatch == null || stringMatch.Length == 0)
		{
			return start;
		}
		checked
		{
			if (binaryCompare)
			{
				return invariantCompareInfo.LastIndexOf(stringCheck, stringMatch, start - 1, start, CompareOptions.Ordinal) + 1;
			}
			return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf(stringCheck, stringMatch, start - 1, start, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) + 1;
		}
	}

	private static string ReplaceInternal(this string expression, string find, string replacement, int count, bool binaryCompare)
	{
		int length = expression.Length;
		int length2 = find.Length;
		StringBuilder stringBuilder = new StringBuilder(length);
		CompareInfo compareInfo;
		CompareOptions options;
		if (binaryCompare)
		{
			compareInfo = invariantCompareInfo;
			options = CompareOptions.Ordinal;
		}
		else
		{
			compareInfo = CultureInfo.CurrentCulture.CompareInfo;
			options = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
		}
		int num = 0;
		int num2 = 0;
		checked
		{
			while (num < length)
			{
				if (num2 == count)
				{
					stringBuilder.Append(expression.Substring(num));
					break;
				}
				int num3 = compareInfo.IndexOf(expression, find, num, options);
				if (num3 < 0)
				{
					stringBuilder.Append(expression.Substring(num));
					break;
				}
				stringBuilder.Append(expression.Substring(num, num3 - num));
				stringBuilder.Append(replacement);
				num2++;
				num = num3 + length2;
			}
			return stringBuilder.ToString();
		}
	}

	public static string FirstLine(this string value)
	{
		if (value == null)
		{
			return value;
		}
		int num = value.IndexOf('\r');
		int num2 = value.IndexOf('\n');
		if (num2 < num)
		{
			num = num2;
		}
		if (num >= 0)
		{
			return value.Substring(0, num);
		}
		return value;
	}

	public static string NextDocNum(string prevCode, int maxLength)
	{
		StringBuilder stringBuilder = new StringBuilder(prevCode.Trim(), maxLength);
		int num = stringBuilder.Length - 1;
		if (maxLength < stringBuilder.Length || num == -1)
		{
			return prevCode;
		}
		bool flag = false;
		bool flag2 = true;
		do
		{
			char c = stringBuilder[num];
			if ((c >= '0') & (c < '9'))
			{
				stringBuilder[num] = (char)(c + 1);
				flag = true;
			}
			else if (c == '9')
			{
				stringBuilder[num] = '0';
			}
			else if ((c >= 'A') & (c < 'Z'))
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else if (flag2)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
				}
				flag = true;
			}
			else if (c == 'Z')
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = 'A';
				}
				else if (flag2)
				{
					stringBuilder[num] = 'A';
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
					flag = true;
				}
			}
			else if ((c >= 'a') & (c < 'z'))
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else if (flag2)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
				}
				flag = true;
			}
			else if (c == 'z')
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = 'a';
				}
				else if (flag2)
				{
					stringBuilder[num] = 'a';
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
					flag = true;
				}
			}
			else if ((c >= 'ა') & (c < 'ჰ'))
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else if (flag2)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
				}
				flag = true;
			}
			else if (c == 'ჰ')
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = 'ა';
				}
				else if (flag2)
				{
					stringBuilder[num] = 'ა';
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
					flag = true;
				}
			}
			else if ((c >= 'Ա') & (c < 'Ֆ'))
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else if (flag2)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
				}
				flag = true;
			}
			else if (c == 'Ֆ')
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = 'Ա';
				}
				else if (flag2)
				{
					stringBuilder[num] = 'Ա';
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
					flag = true;
				}
			}
			else if ((c >= 'ա') & (c < 'և'))
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else if (flag2)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
				}
				flag = true;
			}
			else if (c == 'և')
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = 'ա';
				}
				else if (flag2)
				{
					stringBuilder[num] = 'ա';
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
					flag = true;
				}
			}
			else if ((c >= 'А') & (c < 'Я'))
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else if (flag2)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
				}
				flag = true;
			}
			else if (c == 'Я')
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = 'А';
				}
				else if (flag2)
				{
					stringBuilder[num] = 'А';
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
					flag = true;
				}
			}
			else if ((c >= 'а') & (c < 'я'))
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else if (flag2)
				{
					stringBuilder[num] = (char)(c + 1);
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
				}
				flag = true;
			}
			else if (c == 'я')
			{
				if (maxLength == stringBuilder.Length)
				{
					stringBuilder[num] = 'а';
				}
				else if (flag2)
				{
					stringBuilder[num] = 'а';
				}
				else
				{
					stringBuilder.Insert(num + 1, '1');
					flag = true;
				}
			}
			else
			{
				if (maxLength > stringBuilder.Length && !flag2)
				{
					stringBuilder.Insert(num + 1, '1');
				}
				flag = true;
			}
			num--;
			flag2 = false;
		}
		while (!flag && num != -1);
		if (!flag && maxLength > stringBuilder.Length)
		{
			stringBuilder = stringBuilder.Insert(0, '1');
		}
		return stringBuilder.ToString();
	}

	public static bool InList(this string sValue, params string[] lValues)
	{
		return SettingsHelper.InList(sValue, lValues);
	}

	public static string CreateListCondition(string list, string field, string delimiter = " ", bool includeConditionAND = true)
	{
		list = list.Trim();
		if (string.IsNullOrEmpty(list))
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (includeConditionAND)
		{
			stringBuilder.Append(" and ");
		}
		else
		{
			stringBuilder.Append(' ');
		}
		stringBuilder.Append(field);
		stringBuilder.Append(' ');
		if (list.Substring(0, 1) == "+")
		{
			stringBuilder.Append(" in (");
			CreateListOp(list.Substring(1), delimiter, stringBuilder);
			stringBuilder.Append(')');
			return stringBuilder.ToString();
		}
		if (list.Substring(0, 1) == "-")
		{
			stringBuilder.Append(" not in (");
			CreateListOp(list.Substring(1), delimiter, stringBuilder);
			stringBuilder.Append(')');
			return stringBuilder.ToString();
		}
		stringBuilder.Append(" = N'");
		stringBuilder.Append(list);
		stringBuilder.Append("' ");
		return stringBuilder.ToString();
	}

	public static string CreateListCondition(List<string> condList, string field, string opType, bool includeConditionAND = true)
	{
		if (condList == null)
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (includeConditionAND)
		{
			stringBuilder.Append(" and ");
		}
		else
		{
			stringBuilder.Append(' ');
		}
		stringBuilder.Append(field);
		stringBuilder.Append(' ');
		if (opType == "+")
		{
			stringBuilder.Append(" in (");
			for (int i = 0; i < condList.Count; i++)
			{
				if (!string.IsNullOrEmpty(condList[i]))
				{
					stringBuilder.Append(" N'");
					stringBuilder.Append(condList[i]);
					if (i != condList.Count - 1)
					{
						stringBuilder.Append("',");
					}
				}
			}
			stringBuilder.Append("')");
			return stringBuilder.ToString();
		}
		if (opType == "-")
		{
			stringBuilder.Append(" not in (");
			for (int j = 0; j < condList.Count; j++)
			{
				if (!string.IsNullOrEmpty(condList[j]))
				{
					stringBuilder.Append(" N'");
					stringBuilder.Append(condList[j]);
					if (j != condList.Count - 1)
					{
						stringBuilder.Append("',");
					}
				}
			}
			stringBuilder.Append("')");
			return stringBuilder.ToString();
		}
		stringBuilder.Append(" = N'");
		stringBuilder.Append(condList[0]);
		stringBuilder.Append("' ");
		return stringBuilder.ToString();
	}

	private static void CreateListOp(string list, string delimiter, StringBuilder sb)
	{
		string[] separator = new string[1] { delimiter };
		string[] array = list.Trim().Split(separator, StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			string value = array[i].Trim();
			if (!string.IsNullOrEmpty(value))
			{
				sb.Append(" N'");
				sb.Append(value);
				if (i + 1 == array.Length)
				{
					sb.Append('\'');
				}
				else
				{
					sb.Append("',");
				}
			}
		}
	}

	public static List<string> SplitSelected(this string strList)
	{
		if (string.IsNullOrWhiteSpace(strList))
		{
			return new List<string>();
		}
		return strList.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
	}

	public static string ConvertSelected<T>(this string strList) where T : IEquatable<T>, IComparable<T>
	{
		return string.Join(",", strList.SplitSelected<T>());
	}

	public static string CreateSQLLikeCondtitions(string list, string field, char separator = ' ')
	{
		list = list.Trim();
		if (string.IsNullOrEmpty(list))
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		string[] array = list.Split(separator);
		foreach (string text in array)
		{
			if (text != string.Empty)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(" or ");
				}
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(9, 2, stringBuilder2);
				handler.AppendFormatted(field);
				handler.AppendLiteral(" like N'");
				handler.AppendFormatted(text.Trim());
				handler.AppendLiteral("'");
				stringBuilder2.Append(ref handler);
			}
		}
		return stringBuilder.ToString();
	}

	public static string CreateSQLListCondition(IEnumerable<string> list, string field, bool addAND = true, bool addNOT = false)
	{
		int num = list.Count();
		if (num == 0)
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (addAND)
		{
			stringBuilder.Append(" and ");
		}
		else
		{
			stringBuilder.Append(' ');
		}
		stringBuilder.Append(field);
		if (num == 1)
		{
			if (addNOT)
			{
				stringBuilder.Append(" <> N'" + list.First() + "' ");
			}
			else
			{
				stringBuilder.Append(" = N'" + list.First() + "' ");
			}
		}
		else
		{
			if (addNOT)
			{
				stringBuilder.Append(" not in (");
			}
			else
			{
				stringBuilder.Append(" in (");
			}
			bool flag = true;
			foreach (string item in list)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append('N');
				stringBuilder.Append('\'');
				stringBuilder.Append(item);
				stringBuilder.Append('\'');
			}
			stringBuilder.Append(')');
		}
		return stringBuilder.ToString();
	}

	public static string TranslitArmenian(this string source, bool withoutApostrophes = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = null;
		for (int i = 0; i < source.Length; i++)
		{
			char c = source[i];
			bool flag = false;
			if (c == 'Ո')
			{
				if (i < source.Length - 1 && (source[i + 1] == 'Ւ' || source[i + 1] == 'ւ'))
				{
					text = "U";
					flag = true;
				}
				else if (i == 0 || source[i - 1] == ' ')
				{
					text = "Vo";
					flag = true;
				}
			}
			if (c == 'ո')
			{
				if (i < source.Length - 1 && (source[i + 1] == 'Ւ' || source[i + 1] == 'ւ'))
				{
					text = "u";
					flag = true;
				}
				else if (i == 0 || source[i - 1] == ' ')
				{
					text = "vo";
					flag = true;
				}
			}
			if (!flag)
			{
				text = c switch
				{
					'Ա' => "A", 
					'Բ' => "B", 
					'Գ' => "G", 
					'Դ' => "D", 
					'Ե' => "E", 
					'Զ' => "Z", 
					'Է' => "E'", 
					'Ը' => "Y'", 
					'Թ' => "T'", 
					'Ժ' => "Zh", 
					'Ի' => "I", 
					'Լ' => "L", 
					'Խ' => "X", 
					'Ծ' => "C'", 
					'Կ' => "K", 
					'Հ' => "H", 
					'Ձ' => "Dz", 
					'Ղ' => "Gh", 
					'Ճ' => "Ch", 
					'Մ' => "M", 
					'Յ' => "Y", 
					'Ն' => "N", 
					'Շ' => "Sh", 
					'Ո' => "O", 
					'Չ' => "Ch", 
					'Պ' => "P", 
					'Ջ' => "J", 
					'Ռ' => "R'", 
					'Ս' => "S", 
					'Վ' => "V", 
					'Տ' => "T", 
					'Ր' => "R", 
					'Ց' => "C", 
					'Ւ' => "", 
					'Փ' => "P'", 
					'Ք' => "Q", 
					'Օ' => "O", 
					'Ֆ' => "F", 
					'ա' => "a", 
					'բ' => "b", 
					'գ' => "g", 
					'դ' => "d", 
					'ե' => "e", 
					'զ' => "z", 
					'է' => "e'", 
					'ը' => "y'", 
					'թ' => "t'", 
					'ժ' => "zh", 
					'ի' => "i", 
					'լ' => "l", 
					'խ' => "x", 
					'ծ' => "c'", 
					'կ' => "k", 
					'հ' => "h", 
					'ձ' => "dz", 
					'ղ' => "gh", 
					'ճ' => "ch", 
					'մ' => "m", 
					'յ' => "y", 
					'ն' => "n", 
					'շ' => "sh", 
					'ո' => "o", 
					'չ' => "ch", 
					'պ' => "p", 
					'ջ' => "j", 
					'ռ' => "r'", 
					'ս' => "s", 
					'վ' => "v", 
					'տ' => "t", 
					'ր' => "r", 
					'ց' => "c", 
					'ւ' => "", 
					'փ' => "p'", 
					'ք' => "q", 
					'օ' => "o", 
					'ֆ' => "f", 
					'և' => "ev", 
					'։' => ":", 
					'՞' => "?", 
					'՝' => "'", 
					'՛' => "'", 
					'“' => "\"", 
					'„' => "\"", 
					_ => (c < '\u0080') ? c.ToString() : " ", 
				};
			}
			if (withoutApostrophes & (text.Length == 2))
			{
				text = text.Replace("'", "");
			}
			stringBuilder.Append(text);
		}
		return stringBuilder.ToString();
	}

	public static string ToArmenianANSI(this string value)
	{
		return FromArmenianUnicode(value);
	}

	public static DateOnly ToDateOnly(this string value)
	{
		return Convert.ToDateTime(value).ToDateOnly();
	}

	private static string FromArmenianUnicode(string source)
	{
		if (source == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder(source.Length);
		for (int i = 0; i < source.Length; i++)
		{
			stringBuilder.Append(FromArmenianUnicode(source[i]));
		}
		return stringBuilder.ToString();
	}

	private static char FromArmenianUnicode(char source)
	{
		return (char)FromArmenianUnicodeToInt(source);
	}

	private static int FromArmenianUnicodeToInt(char source)
	{
		int num = source % 256;
		if ((source >= 'ա') & (source <= 'ֆ'))
		{
			return num * 2 - 15;
		}
		if ((source >= 'Ա') & (source <= 'Ֆ'))
		{
			return num * 2 + 80;
		}
		switch (source)
		{
		case 'և':
			return 168;
		case 'ՙ':
			return 39;
		case '՚':
			return 39;
		case '՛':
			return 176;
		case '՜':
			return 175;
		case '՝':
			return 170;
		case '՞':
			return 177;
		case '՟':
			return 174;
		case '։':
			return 58;
		case '֊':
			return 173;
		case '«':
			return 167;
		case '»':
			return 166;
		case '“':
			return 8220;
		case '”':
			return 8221;
		default:
			if (source / 256 == 4)
			{
				return num switch
				{
					81 => 229, 
					1 => 197, 
					_ => num + 176, 
				};
			}
			return num;
		}
	}

	public static string PutInBrackets(this string source)
	{
		return " (" + source.Trim() + ") ";
	}

	public static string GetValueFromJson(this string jsonString, string key)
	{
		using (JsonDocument jsonDocument = JsonDocument.Parse(jsonString))
		{
			if (jsonDocument.RootElement.TryGetProperty(key, out var value))
			{
				return value.ToString();
			}
		}
		return null;
	}

	public static string CaseInsensitive(this string value)
	{
		string text = string.Empty;
		if (!string.IsNullOrWhiteSpace(value))
		{
			value = value.TrimStart();
			string text2 = value.ToUpper();
			string text3 = value.ToLower();
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i].ToString().IsNumeric())
				{
					text += value[i];
				}
				else
				{
					text = ((value[i] == ' ') ? (text + " ") : (text + $"[{text2[i]}{text3[i]}]"));
				}
			}
		}
		return text;
	}

	private static string WithoutDelimiters(this string text)
	{
		return text.Replace("/", "").Replace("\\", "").Replace(",", "")
			.Replace("\"", "")
			.Replace("'", "")
			.Replace("?", "")
			.Replace(".", "");
	}

	public static bool CheckEquals(this string strA, string strB, bool ignoreCase = true, bool ignoreDelimiters = true, string[] ignoringWords = null)
	{
		if (string.IsNullOrWhiteSpace(strA) && string.IsNullOrWhiteSpace(strB))
		{
			return true;
		}
		if (string.IsNullOrWhiteSpace(strA) ^ string.IsNullOrWhiteSpace(strB))
		{
			return false;
		}
		if (ignoreDelimiters)
		{
			strA = strA.WithoutDelimiters();
			strB = strB.WithoutDelimiters();
		}
		if (ignoringWords != null)
		{
			foreach (string text in ignoringWords)
			{
				strA = strA.Replace(text.Trim() + " ", "");
				strB = strB.Replace(text.Trim() + " ", "");
				strA = strA.Replace(" " + text.Trim(), "");
				strB = strB.Replace(" " + text.Trim(), "");
			}
		}
		if (ignoreDelimiters)
		{
			strA = strA.Replace(" ", "");
			strB = strB.Replace(" ", "");
		}
		if (ignoreCase)
		{
			return string.Equals(strA, strB, StringComparison.CurrentCultureIgnoreCase);
		}
		return string.Equals(strA, strB);
	}
}
