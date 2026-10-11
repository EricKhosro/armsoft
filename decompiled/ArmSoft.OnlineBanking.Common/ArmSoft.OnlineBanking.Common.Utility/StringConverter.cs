using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ArmSoft.OnlineBanking.Common.Extensions;

namespace ArmSoft.OnlineBanking.Common.Utility;

public static class StringConverter
{
	public static string ToArmenianUnicode(string source)
	{
		if (source == null)
		{
			return null;
		}
		if (source.Any((char c) => c > 'ÿ'))
		{
			return source;
		}
		StringBuilder stringBuilder = new StringBuilder(source.Length);
		for (int num = 0; num < source.Length; num++)
		{
			stringBuilder.Append(ToArmenianUnicode(source[num]));
		}
		return stringBuilder.ToString();
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

	public static string FromArmenianUnicode(string source)
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

	public static char FromArmenianUnicode(char source)
	{
		return (char)FromArmenianUnicodeToInt(source);
	}

	public static int FromArmenianUnicodeToInt(char source)
	{
		int num = source % 256;
		if (source >= 'ա' && source <= 'ֆ')
		{
			return num * 2 - 15;
		}
		if (source >= 'Ա' && source <= 'Ֆ')
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
		case '‐':
			return 173;
		case '«':
			return 167;
		case '»':
			return 166;
		case '“':
			return 167;
		case '”':
			return 166;
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

	public static string TransliterateFromEnglishToRussian(string text)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		text = text.TrimStart();
		for (int i = 0; i < text.Length; i++)
		{
			string text2 = text.Substring(i, 1);
			if (text2 == "'")
			{
				text2 = string.Empty;
				flag = !flag;
			}
			else if (flag)
			{
				switch (text2)
				{
				case "A":
					text2 = Convert.ToChar(1040).ToString();
					break;
				case "B":
					text2 = Convert.ToChar(1041).ToString();
					break;
				case "V":
					text2 = Convert.ToChar(1042).ToString();
					break;
				case "G":
					text2 = Convert.ToChar(1043).ToString();
					break;
				case "D":
					text2 = Convert.ToChar(1044).ToString();
					break;
				case "E":
					text2 = Convert.ToChar(1045).ToString();
					break;
				case "J":
					text2 = Convert.ToChar(1046).ToString();
					break;
				case "Z":
					text2 = Convert.ToChar(1047).ToString();
					break;
				case "I":
					text2 = Convert.ToChar(1048).ToString();
					break;
				case "i":
					text2 = Convert.ToChar(1049).ToString();
					break;
				case "K":
					text2 = Convert.ToChar(1050).ToString();
					break;
				case "L":
					text2 = Convert.ToChar(1051).ToString();
					break;
				case "M":
					text2 = Convert.ToChar(1052).ToString();
					break;
				case "N":
					text2 = Convert.ToChar(1053).ToString();
					break;
				case "O":
					text2 = Convert.ToChar(1054).ToString();
					break;
				case "P":
					text2 = Convert.ToChar(1055).ToString();
					break;
				case "R":
					text2 = Convert.ToChar(1056).ToString();
					break;
				case "S":
					text2 = Convert.ToChar(1057).ToString();
					break;
				case "T":
					text2 = Convert.ToChar(1058).ToString();
					break;
				case "U":
					text2 = Convert.ToChar(1059).ToString();
					break;
				case "F":
					text2 = Convert.ToChar(1060).ToString();
					break;
				case "H":
					text2 = Convert.ToChar(1061).ToString();
					break;
				case "C":
					text2 = Convert.ToChar(1062).ToString();
					break;
				case "c":
					text2 = Convert.ToChar(1063).ToString();
					break;
				case "Q":
					text2 = Convert.ToChar(1064).ToString();
					break;
				case "q":
					text2 = Convert.ToChar(1065).ToString();
					break;
				case "x":
					text2 = Convert.ToChar(1066).ToString();
					break;
				case "Y":
					text2 = Convert.ToChar(1067).ToString();
					break;
				case "X":
					text2 = Convert.ToChar(1068).ToString();
					break;
				case "e":
					text2 = Convert.ToChar(1069).ToString();
					break;
				case "u":
					text2 = Convert.ToChar(1070).ToString();
					break;
				case "a":
					text2 = Convert.ToChar(1071).ToString();
					break;
				}
			}
			stringBuilder.Append(text2);
		}
		return stringBuilder.ToString();
	}

	public static string TransliterateFromRussianToEnglish(string text)
	{
		if (!Regex.IsMatch(text, "\\p{IsCyrillic}"))
		{
			return text;
		}
		StringBuilder stringBuilder = new StringBuilder();
		text = text.TrimStart();
		for (int i = 0; i < text.Length; i++)
		{
			string text2 = text.Substring(i, 1);
			char c = ((i == 0) ? ' ' : text[i - 1]);
			char c2 = ((i == text.Length - 1) ? ' ' : text[i + 1]);
			int num = Convert.ToChar(text2);
			switch (num)
			{
			case 1040:
			case 1072:
				text2 = "A";
				break;
			case 1041:
			case 1073:
				text2 = "B";
				break;
			case 1042:
			case 1074:
				text2 = "V";
				break;
			case 1043:
			case 1075:
				text2 = "G";
				break;
			case 1044:
			case 1076:
				text2 = "D";
				break;
			case 1045:
			case 1077:
				text2 = "E";
				break;
			case 1046:
			case 1078:
				text2 = "J";
				break;
			case 1047:
			case 1079:
				text2 = "Z";
				break;
			case 1048:
			case 1080:
				text2 = "I";
				break;
			case 1049:
			case 1081:
				text2 = "i";
				break;
			case 1050:
			case 1082:
				text2 = "K";
				break;
			case 1051:
			case 1083:
				text2 = "L";
				break;
			case 1052:
			case 1084:
				text2 = "M";
				break;
			case 1053:
			case 1085:
				text2 = "N";
				break;
			case 1054:
			case 1086:
				text2 = "O";
				break;
			case 1055:
			case 1087:
				text2 = "P";
				break;
			case 1056:
			case 1088:
				text2 = "R";
				break;
			case 1057:
			case 1089:
				text2 = "S";
				break;
			case 1058:
			case 1090:
				text2 = "T";
				break;
			case 1059:
			case 1091:
				text2 = "U";
				break;
			case 1060:
			case 1092:
				text2 = "F";
				break;
			case 1061:
			case 1093:
				text2 = "H";
				break;
			case 1062:
			case 1094:
				text2 = "C";
				break;
			case 1063:
			case 1095:
				text2 = "c";
				break;
			case 1064:
			case 1096:
				text2 = "Q";
				break;
			case 1065:
			case 1097:
				text2 = "q";
				break;
			case 1066:
			case 1098:
				text2 = "x";
				break;
			case 1067:
			case 1099:
				text2 = "Y";
				break;
			case 1068:
			case 1100:
				text2 = "X";
				break;
			case 1069:
			case 1101:
				text2 = "e";
				break;
			case 1070:
			case 1102:
				text2 = "u";
				break;
			case 1071:
			case 1103:
				text2 = "a";
				break;
			case 39:
				text2 = string.Empty;
				break;
			default:
				if (num.IsBetween(97, 122) || num.IsBetween(65, 90))
				{
					if (c == ' ' || (!CommonExtension.IsBetween(c, 97, 122) && !CommonExtension.IsBetween(c, 65, 90)))
					{
						text2 = "'" + text2;
					}
					if (c2 == ' ' || (!CommonExtension.IsBetween(c2, 97, 122) && !CommonExtension.IsBetween(c2, 65, 90)))
					{
						text2 += "'";
					}
				}
				break;
			}
			stringBuilder.Append(text2);
		}
		return stringBuilder.ToString();
	}
}
