using System;
using System.Text;

namespace ArmSoft.SME.Core.AddIn;

public static class StringExtension
{
	public static string Right(this string str, int Length)
	{
		if (Length < 0)
		{
			throw new ArgumentException("Argument 'Length' must be greater or equal to zero.");
		}
		if (Length == 0 || str == null)
		{
			return "";
		}
		int length = str.Length;
		if (Length >= length)
		{
			return str;
		}
		return str.Substring(checked(length - Length), Length);
	}

	public static string ToArmenianUnicode(this string source)
	{
		if (source == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder(source.Length);
		for (int i = 0; i < source.Length; i++)
		{
			stringBuilder.Append(ToArmenianUnicode(source[i]));
		}
		return stringBuilder.ToString();
	}

	public static bool IsNumeric(this string str)
	{
		double result;
		return double.TryParse(str, out result);
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

	public static string LeftAlign(this string value, int length)
	{
		return value.PadRight(length).Substring(0, length);
	}

	public static string RightAlign(this string value, int length)
	{
		string text = value.PadLeft(length);
		return text.Substring(text.Length - length);
	}

	public static char ToArmenianUnicode(char source)
	{
		int num = source;
		if (num >= 178 && num <= 253)
		{
			int num2 = num - 178;
			num = ((num % 2 != 0) ? (1377 + (num2 - 1) / 2) : (1329 + num2 / 2));
			return (char)num;
		}
		return num switch
		{
			168 => 'և', 
			167 => '«', 
			166 => '»', 
			177 => '՞', 
			176 => '՛', 
			170 => '՝', 
			175 => '՜', 
			130 => '┌', 
			131 => '┐', 
			132 => '┴', 
			133 => '┤', 
			134 => '├', 
			135 => '└', 
			136 => '┘', 
			137 => '─', 
			138 => '┼', 
			139 => '│', 
			140 => '┬', 
			172 => '─', 
			_ => source, 
		};
	}
}
