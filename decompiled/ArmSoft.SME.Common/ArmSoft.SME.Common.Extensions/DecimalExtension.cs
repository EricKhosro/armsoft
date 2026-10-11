using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ArmSoft.SME.Common.Extensions;

public static class DecimalExtension
{
	private const double EarthRadius = 6370693.48565264;

	public static decimal RoundAs(this decimal number, int precision)
	{
		if (precision < 0)
		{
			decimal d = Convert.ToDecimal(Math.Pow(10.0, precision));
			return decimal.Divide(decimal.Round(decimal.Multiply(number, d), 0, MidpointRounding.AwayFromZero), d);
		}
		return decimal.Round(number, precision, MidpointRounding.AwayFromZero);
	}

	public static decimal RoundAs(this decimal number, int precision, bool truncate)
	{
		if (truncate)
		{
			return decimal.Truncate(number);
		}
		return number.RoundAs(precision);
	}

	public static string NumberFormat(this decimal value, short length, short precision)
	{
		string numberFormat = GetNumberFormat(length, precision);
		return value.ToString(numberFormat);
	}

	public static string CurrencyFormat(this decimal value, short length, short precision)
	{
		string currencyFormat = GetCurrencyFormat(length, precision);
		return value.ToString(currencyFormat);
	}

	public static string CompactFormat(this decimal value)
	{
		return value.ToString("G4");
	}

	public static string GetNumberFormat(short length, short precision)
	{
		string text;
		if (precision == 0)
		{
			text = "0";
			if (length > 1)
			{
				text = text.PadLeft(length, '#');
			}
		}
		else
		{
			text = "0." + new string('0', precision);
			if (length > text.Length)
			{
				text = text.PadLeft(length, '#');
			}
		}
		return text;
	}

	public static string GetCurrencyFormat(short length, short precision)
	{
		string numberFormat = GetNumberFormat(length, precision);
		int num = numberFormat.IndexOf('.');
		string text;
		string text2;
		if (num == -1)
		{
			text = numberFormat;
			text2 = "";
		}
		else
		{
			text = numberFormat.Substring(0, num);
			text2 = numberFormat.Substring(num);
		}
		while (text.Length > 3)
		{
			text2 = string.Concat(",", MemoryExtensions.AsSpan(text, text.Length - 3, 3), text2);
			string text3 = text;
			text = text3.Substring(0, text3.Length - 3);
		}
		return text + text2;
	}

	public static double GetGeographicDistance(double? lat1, double? lon1, double? lat2, double? lon2)
	{
		if (!lat1.HasValue || !lon1.HasValue || !lat2.HasValue || !lon2.HasValue)
		{
			return -1.0;
		}
		if (lat1 == lat2 && lon1 == lon2)
		{
			return 0.0;
		}
		lat1 = lat1 * Math.PI / 180.0;
		lon1 = lon1 * Math.PI / 180.0;
		lat2 = lat2 * Math.PI / 180.0;
		lon2 = lon2 * Math.PI / 180.0;
		double d = Math.Sin(lat1.Value) * Math.Sin(lat2.Value) + Math.Cos(lat1.Value) * Math.Cos(lat2.Value) * Math.Cos((lon1 - lon2).Value);
		return 6370693.48565264 * Math.Acos(d);
	}

	public static string STOC(this decimal value, string nameInteger, string nameDecimal, bool toUpperFirstChar = true, bool isPrintMode = true)
	{
		string text = value.ToString("000000000000000.00", CultureInfo.InvariantCulture);
		string[,] array = new string[5, 5];
		for (int i = 0; i < 5; i++)
		{
			array[i, 0] = text[i * 3 + 2].ToString();
			array[i, 1] = text[i * 3 + 1].ToString();
			array[i, 2] = array[i, 1] + array[i, 0];
			array[i, 3] = text[i * 3].ToString();
		}
		array[0, 4] = Resources.StocStringTrillion;
		array[1, 4] = Resources.StocStringMilliard;
		array[2, 4] = Resources.StocStringMillion;
		array[3, 4] = Resources.StocStringThousand;
		array[4, 4] = string.Empty;
		string stocStringHundred = Resources.StocStringHundred;
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{
				"0",
				Resources.StocString00
			},
			{
				"1",
				Resources.StocString01
			},
			{
				"2",
				Resources.StocString02
			},
			{
				"3",
				Resources.StocString03
			},
			{
				"4",
				Resources.StocString04
			},
			{
				"5",
				Resources.StocString05
			},
			{
				"6",
				Resources.StocString06
			},
			{
				"7",
				Resources.StocString07
			},
			{
				"8",
				Resources.StocString08
			},
			{
				"9",
				Resources.StocString09
			},
			{
				"10",
				Resources.StocString10
			},
			{
				"11",
				Resources.StocString11
			},
			{
				"12",
				Resources.StocString12
			},
			{
				"13",
				Resources.StocString13
			},
			{
				"14",
				Resources.StocString14
			},
			{
				"15",
				Resources.StocString15
			},
			{
				"16",
				Resources.StocString16
			},
			{
				"17",
				Resources.StocString17
			},
			{
				"18",
				Resources.StocString18
			},
			{
				"19",
				Resources.StocString19
			},
			{
				"20",
				Resources.StocString20
			},
			{
				"30",
				Resources.StocString30
			},
			{
				"40",
				Resources.StocString40
			},
			{
				"50",
				Resources.StocString50
			},
			{
				"60",
				Resources.StocString60
			},
			{
				"70",
				Resources.StocString70
			},
			{
				"80",
				Resources.StocString80
			},
			{
				"90",
				Resources.StocString90
			},
			{
				"00",
				string.Empty
			}
		};
		string stocSeparatorTwoDigits = Resources.StocSeparatorTwoDigits;
		string stocSeparatorHundreds = Resources.StocSeparatorHundreds;
		string stocSeparatorWords = Resources.StocSeparatorWords;
		if (!bool.TryParse(Resources.StocShowOneHundredInTheMiddle, out var result))
		{
			result = false;
		}
		string value2 = ((nameInteger != null && !(nameInteger == string.Empty)) ? nameInteger : Resources.StocSeparatorDecimals);
		StringBuilder stringBuilder = new StringBuilder();
		for (int j = 0; j <= array.GetUpperBound(0); j++)
		{
			bool flag = false;
			if (Convert.ToInt32(array[j, 3]) > 0)
			{
				if ((Convert.ToInt32(array[j, 3]) != 1 || stringBuilder.Length == 0) | result)
				{
					stringBuilder.Append(dictionary[array[j, 3]]);
					stringBuilder.Append(stocSeparatorWords);
				}
				stringBuilder.Append(stocStringHundred);
				stringBuilder.Append(stocSeparatorWords);
				flag = true;
			}
			if (Convert.ToInt32(array[j, 2]) > 0)
			{
				if (flag)
				{
					stringBuilder.Append(stocSeparatorHundreds);
				}
				if ((Convert.ToInt32(array[j, 2]) >= 10) & (Convert.ToInt32(array[j, 2]) <= 19))
				{
					stringBuilder.Append(dictionary[array[j, 2]]);
					stringBuilder.Append(stocSeparatorWords);
				}
				else if ((Convert.ToInt32(array[j, 1]) > 0) & (Convert.ToInt32(array[j, 0]) > 0))
				{
					stringBuilder.Append(dictionary[array[j, 1] + "0"]);
					stringBuilder.Append(stocSeparatorTwoDigits);
					stringBuilder.Append(dictionary[array[j, 0]]);
					stringBuilder.Append(stocSeparatorWords);
				}
				else if (Convert.ToInt32(array[j, 1]) > 0)
				{
					stringBuilder.Append(dictionary[array[j, 1] + "0"]);
					stringBuilder.Append(stocSeparatorWords);
				}
				else if (Convert.ToInt32(array[j, 0]) > 0)
				{
					stringBuilder.Append(dictionary[array[j, 0]]);
					stringBuilder.Append(stocSeparatorWords);
				}
				flag = true;
			}
			if (flag && array[j, 4].Length > 0)
			{
				stringBuilder.Append(array[j, 4]);
				stringBuilder.Append(stocSeparatorWords);
			}
		}
		if (stringBuilder.Length == 0)
		{
			stringBuilder.Append(dictionary["0"]);
			stringBuilder.Append(stocSeparatorWords);
		}
		stringBuilder.Append(value2);
		string text2 = text[16].ToString();
		string text3 = text[17].ToString();
		string text4 = text2 + text3;
		if (int.TryParse(text4, out var _) && Convert.ToInt32(text4) > 0)
		{
			stringBuilder.Append(stocSeparatorWords);
			if ((Convert.ToInt32(text4) >= 10) & (Convert.ToInt32(text4) <= 19))
			{
				stringBuilder.Append(dictionary[text4]);
				stringBuilder.Append(stocSeparatorWords);
			}
			else if ((Convert.ToInt32(text2) > 0) & (Convert.ToInt32(text3) > 0))
			{
				stringBuilder.Append(dictionary[text2 + "0"]);
				stringBuilder.Append(stocSeparatorTwoDigits);
				stringBuilder.Append(dictionary[text3]);
				stringBuilder.Append(stocSeparatorWords);
			}
			else if (Convert.ToInt32(text2) > 0)
			{
				stringBuilder.Append(dictionary[text2 + "0"]);
				stringBuilder.Append(stocSeparatorWords);
			}
			else if (Convert.ToInt32(text3) > 0)
			{
				stringBuilder.Append(dictionary[text3]);
				stringBuilder.Append(stocSeparatorWords);
			}
			if (nameDecimal != null && nameDecimal != string.Empty)
			{
				stringBuilder.Append(nameDecimal);
				stringBuilder.Append(stocSeparatorWords);
			}
		}
		else
		{
			stringBuilder.Append(stocSeparatorWords);
		}
		if (toUpperFirstChar)
		{
			stringBuilder[0] = char.ToUpper(stringBuilder[0]);
		}
		return stringBuilder.ToString().TrimEnd();
	}
}
