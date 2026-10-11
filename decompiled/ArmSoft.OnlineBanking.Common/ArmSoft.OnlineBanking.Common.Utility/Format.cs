using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using ArmSoft.OnlineBanking.Common.Constants;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OnlineBanking.Common.Resources;

namespace ArmSoft.OnlineBanking.Common.Utility;

public static class Format
{
	public static string Amount(decimal? amount, string currency = "", string defaultText = "", string format = "#,###,###,##0.00")
	{
		if (!amount.HasValue)
		{
			return defaultText;
		}
		string text = amount.Value.ToString(format, CultureInfo.InvariantCulture);
		if (!string.IsNullOrEmpty(currency))
		{
			text = text + " " + currency;
		}
		return text;
	}

	public static string AmountToWords(decimal amount, string currencyCode, string currencyUnitName, string currencyFractionName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = amount.ToString("##############0.00", CultureInfo.InvariantCulture);
		string text2 = string.Empty;
		string text3 = string.Empty;
		int num = 0;
		int num2 = text.IndexOf(".");
		if (num2 > 0)
		{
			text2 = text.Substring(0, num2).Trim();
			string text4 = (text.Substring(num2 + 1) + "00").Substring(0, 2);
			text3 = (text4.Equals("00") ? string.Empty : text4);
		}
		if (amount < 1m)
		{
			stringBuilder.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Zero);
		}
		else
		{
			string empty = string.Empty;
			num = (int)Math.Ceiling((double)text2.Length / 3.0);
			text2 = text2.PadLeft(num * 3, '0');
			for (int i = 0; i < text2.Length; i += 3)
			{
				empty = text2.Substring(i, 3);
				if (empty != "000")
				{
					AppendHundreds(stringBuilder, empty, num == 2 || num == 1);
					string value = empty.Substring(empty.Length - 2);
					stringBuilder.Append(" ");
					stringBuilder.Append(DecodeNumberTripletName(num, Convert.ToInt32(value)));
					stringBuilder.Append(" ");
				}
				num--;
			}
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		if (text3 != string.Empty)
		{
			if (text3.StartsWith("0"))
			{
				AppendDigit(stringBuilder2, text3, changeSuffix: true);
			}
			else
			{
				AppendTens(stringBuilder2, text3, changeSuffix: true);
			}
		}
		if (!string.IsNullOrEmpty(currencyCode))
		{
			if (Thread.CurrentThread.CurrentUICulture.Name.Equals("ru-RU"))
			{
				if (stringBuilder2.Length > 0)
				{
					if (text2.EndsWith("1") && !text2.EndsWith("11"))
					{
						stringBuilder2.Insert(0, " целая ");
					}
					else
					{
						stringBuilder2.Insert(0, " целых ");
					}
					if (text3.EndsWith("1") && !text3.EndsWith("11"))
					{
						stringBuilder2.Append(" сотая");
					}
					else
					{
						stringBuilder2.Append(" сотых");
					}
				}
				StringBuilder stringBuilder3 = stringBuilder2;
				StringBuilder stringBuilder4 = stringBuilder3;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder3);
				handler.AppendLiteral(" ");
				handler.AppendFormatted(currencyCode);
				stringBuilder4.Append(ref handler);
			}
			else
			{
				StringBuilder stringBuilder3 = stringBuilder;
				StringBuilder stringBuilder5 = stringBuilder3;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder3);
				handler.AppendLiteral(" ");
				handler.AppendFormatted(currencyUnitName);
				stringBuilder5.Append(ref handler);
				if (stringBuilder2.Length > 0)
				{
					stringBuilder2.Insert(0, " ");
					stringBuilder3 = stringBuilder2;
					StringBuilder stringBuilder6 = stringBuilder3;
					handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder3);
					handler.AppendLiteral(" ");
					handler.AppendFormatted(currencyFractionName);
					stringBuilder6.Append(ref handler);
				}
			}
		}
		stringBuilder.Append(stringBuilder2);
		string text5 = stringBuilder.ToString();
		return text5.Substring(0, 1).ToUpper() + text5.Substring(1, stringBuilder.Length - 1);
	}

	public static string Commission(decimal commissionAmount, decimal commissionAmount2, string commissionCurrency, string commissionCurrency2)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (commissionAmount == 0m && commissionAmount2 == 0m)
		{
			stringBuilder.Append(Amount(0m));
		}
		else
		{
			decimal num = commissionAmount;
			decimal num2 = commissionAmount2;
			if (commissionCurrency2 == commissionCurrency)
			{
				num += num2;
				num2 = 0m;
			}
			if (num > 0m)
			{
				stringBuilder.Append(Amount(num)).Append(" ").Append(commissionCurrency);
			}
			if (num2 > 0m)
			{
				if (num > 0m)
				{
					stringBuilder.Append(" + ");
				}
				stringBuilder.Append(Amount(num2)).Append(" ").Append(commissionCurrency2);
			}
		}
		return stringBuilder.ToString();
	}

	public static string CommissionToWords(decimal commissionAmount, decimal commissionAmount2, string commissionCurrency, string commissionCurrency2, string currencyCode, string currencyUnitName, string currencyFractionName, string currency2Code, string currency2UnitName, string currency2FractionName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (commissionAmount == 0m && commissionAmount2 == 0m)
		{
			stringBuilder.Append(AmountToWords(0m, null, null, null));
		}
		else
		{
			decimal num = commissionAmount;
			decimal num2 = commissionAmount2;
			if (commissionCurrency2 == commissionCurrency)
			{
				num += num2;
				num2 = 0m;
			}
			if (num > 0m)
			{
				stringBuilder.Append(AmountToWords(num, currencyCode, currencyUnitName, currencyFractionName));
			}
			if (num2 > 0m)
			{
				if (num > 0m)
				{
					stringBuilder.Append(" + ");
				}
				stringBuilder.Append(AmountToWords(num2, currency2Code, currency2UnitName, currency2FractionName));
			}
		}
		return stringBuilder.ToString();
	}

	public static string Date(DateTime? date, string format = "dd/MM/yyyy")
	{
		return date?.ToString(format, CultureInfo.InvariantCulture) ?? string.Empty;
	}

	public static string Decimal(decimal value, string format = "#0.00")
	{
		return value.ToString(format, CultureInfo.InvariantCulture);
	}

	public static string CardNumber(string cardNumber, string mask)
	{
		if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(mask))
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		mask = mask.Trim();
		int num;
		if (mask.First() == '#')
		{
			num = 0;
			for (int i = 0; i < mask.Length; i++)
			{
				if (num >= cardNumber.Length)
				{
					break;
				}
				if (mask[i] == '#')
				{
					stringBuilder.Append(cardNumber[num++]);
				}
				else if (mask[i] == '*')
				{
					stringBuilder.Append('*');
					num++;
				}
				else
				{
					stringBuilder.Append(mask[i]);
				}
			}
			return stringBuilder.ToString();
		}
		num = cardNumber.Length - 1;
		int num2 = mask.Length - 1;
		while (num2 >= 0 && num >= 0)
		{
			if (mask[num2] == '#')
			{
				stringBuilder.Append(cardNumber[num--]);
			}
			else if (mask[num2] == '*')
			{
				stringBuilder.Append('*');
				num--;
			}
			else
			{
				stringBuilder.Append(mask[num2]);
			}
			num2--;
		}
		return new string(stringBuilder.ToString().Reverse().ToArray());
	}

	public static string GetCardNumberVisibleDigits(string cardNumber, string mask)
	{
		return new string(CardNumber(cardNumber, mask).Where(char.IsDigit).ToArray());
	}

	public static string MaskReceiverName(string name)
	{
		if (string.IsNullOrWhiteSpace(name) || Util.IsCardTransferReceiverNameDefault(name))
		{
			return string.Empty;
		}
		List<string> list = new List<string>();
		string[] array = name.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 0)
		{
			list.Add(MaskLeavingFirstLetters(array[0], 1));
			for (int i = 1; i < array.Length; i++)
			{
				list.Add(MaskLeavingFirstLetters(array[i], 2));
			}
		}
		return string.Join(" ", list);
	}

	public static string MaskReceiverId(string receiverId, string maskSymbols = "***")
	{
		string result = string.Empty;
		if (receiverId != string.Empty)
		{
			result = receiverId.Substring(0, 5) + maskSymbols + receiverId.Substring(receiverId.Length - 3, 3);
		}
		return result;
	}

	public static string ReceiverName(string name, string docType, DocumentState state)
	{
		string text = DocumentTypes.MaskReceiverName(docType, state, name);
		if (text != null)
		{
			if (text == null || text.Length != 0)
			{
				if (text == "***")
				{
					return MaskReceiverName(name);
				}
				return name;
			}
			return string.Empty;
		}
		if (!Util.IsCardTransferReceiverNameDefault(name))
		{
			return name;
		}
		return string.Empty;
	}

	public static string ReceiverId(string receiverId, string docType, DocumentState state)
	{
		string text = DocumentTypes.MaskReceiverId(docType, state);
		if (text != null)
		{
			if (text == null || text.Length != 0)
			{
				if (text == "***")
				{
					return MaskReceiverId(receiverId, text);
				}
				return receiverId;
			}
			return string.Empty;
		}
		return receiverId;
	}

	public static string AttachmentFileName(string fileName, long fileSize)
	{
		string result = string.Empty;
		if (!string.IsNullOrEmpty(fileName))
		{
			result = fileName + " (" + AttachmentFileSize(fileSize) + ")";
		}
		return result;
	}

	public static string AttachedCardNumber(string cardNumber)
	{
		int num = 16 - cardNumber.Length;
		if (num > 0)
		{
			int num2 = cardNumber.IndexOf('*');
			string text = cardNumber.Substring(0, num2);
			text += new string('*', num);
			return text + cardNumber.Substring(num2);
		}
		return cardNumber;
	}

	private static string AttachmentFileSize(long fileSize)
	{
		string[] array = new string[4] { "bytes", "Kb", "Mb", "Gb" };
		double num = fileSize;
		int num2 = 0;
		while (num >= 1024.0 && num2 + 1 < array.Length)
		{
			num2++;
			num /= 1024.0;
		}
		return $"{num:0.##} {array[num2]}";
	}

	private static void AppendDigit(StringBuilder result, string digit, bool changeSuffix)
	{
		switch (Convert.ToInt32(digit))
		{
		case 1:
			result.Append((Thread.CurrentThread.CurrentUICulture.Name.Equals("ru-RU") && !changeSuffix) ? "один" : ArmSoft.OnlineBanking.Common.Resources.Resources.One);
			break;
		case 2:
			result.Append((Thread.CurrentThread.CurrentUICulture.Name.Equals("ru-RU") && !changeSuffix) ? "два" : ArmSoft.OnlineBanking.Common.Resources.Resources.Two);
			break;
		case 3:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Three);
			break;
		case 4:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Four);
			break;
		case 5:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Five);
			break;
		case 6:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Six);
			break;
		case 7:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Seven);
			break;
		case 8:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Eight);
			break;
		case 9:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Nine);
			break;
		}
	}

	private static void AppendTens(StringBuilder result, string tens, bool changeSuffix)
	{
		int num = Convert.ToInt32(tens);
		if (num >= 20)
		{
			num -= num % 10;
		}
		switch (num)
		{
		case 10:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Ten);
			break;
		case 11:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Eleven);
			break;
		case 12:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Twelve);
			break;
		case 13:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Thirteen);
			break;
		case 14:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Fourteen);
			break;
		case 15:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Fifteen);
			break;
		case 16:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Sixteen);
			break;
		case 17:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Seventeen);
			break;
		case 18:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Eighteen);
			break;
		case 19:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Nineteen);
			break;
		case 20:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Twenty);
			break;
		case 30:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Thirty);
			break;
		case 40:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Forty);
			break;
		case 50:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Fifty);
			break;
		case 60:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Sixty);
			break;
		case 70:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Seventy);
			break;
		case 80:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Eighty);
			break;
		case 90:
			result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Ninety);
			break;
		}
		if (!tens.StartsWith("1"))
		{
			if (!Thread.CurrentThread.CurrentUICulture.Name.Equals("hy-AM"))
			{
				result.Append(" ");
			}
			AppendDigit(result, tens.Substring(1, 1), changeSuffix);
		}
	}

	private static void AppendHundreds(StringBuilder result, string number, bool changeSuffix)
	{
		int num = Convert.ToInt32(number);
		if (!number.StartsWith("0"))
		{
			if (Thread.CurrentThread.CurrentUICulture.Name.Equals("ru-RU"))
			{
				switch (num / 100)
				{
				case 1:
					result.Append("сто ");
					break;
				case 2:
					result.Append("двести ");
					break;
				case 3:
					result.Append("триста ");
					break;
				case 4:
					result.Append("четыреста ");
					break;
				case 5:
					result.Append("пятьсот ");
					break;
				case 6:
					result.Append("шестьсот ");
					break;
				case 7:
					result.Append("семьсот ");
					break;
				case 8:
					result.Append("восемьсот ");
					break;
				case 9:
					result.Append("девятьсот ");
					break;
				}
			}
			else
			{
				AppendDigit(result, number.Substring(0, 1), changeSuffix: false);
				result.Append(" ");
				result.Append(ArmSoft.OnlineBanking.Common.Resources.Resources.Hundred);
				result.Append(" ");
			}
		}
		if (number.Substring(1, 1) != "0")
		{
			AppendTens(result, number.Substring(1, 2), changeSuffix);
		}
		else
		{
			AppendDigit(result, number.Substring(2, 1), changeSuffix);
		}
	}

	private static string DecodeNumberTripletName(int tripletsCount, int tens)
	{
		string result = string.Empty;
		int num = tens % 10;
		int num2 = tens / 10;
		switch (tripletsCount)
		{
		case 2:
			if (Thread.CurrentThread.CurrentUICulture.Name.Equals("ru-RU"))
			{
				if (num2 == 1)
				{
					result = "тысяч";
					break;
				}
				switch (num)
				{
				case 1:
					result = ArmSoft.OnlineBanking.Common.Resources.Resources.Thousand;
					break;
				case 2:
				case 3:
				case 4:
					result = "тысячи";
					break;
				default:
					result = "тысяч";
					break;
				}
			}
			else
			{
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Thousand;
			}
			break;
		case 3:
			if (Thread.CurrentThread.CurrentUICulture.Name.Equals("ru-RU"))
			{
				if (num2 == 1)
				{
					result = "миллионов";
					break;
				}
				switch (num)
				{
				case 1:
					result = ArmSoft.OnlineBanking.Common.Resources.Resources.Million;
					break;
				case 2:
				case 3:
				case 4:
					result = "миллиона";
					break;
				default:
					result = "миллионов";
					break;
				}
			}
			else
			{
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Million;
			}
			break;
		case 4:
			if (Thread.CurrentThread.CurrentUICulture.Name.Equals("ru-RU"))
			{
				if (num2 == 1)
				{
					result = "миллиардов";
					break;
				}
				switch (num)
				{
				case 1:
					result = ArmSoft.OnlineBanking.Common.Resources.Resources.Milliard;
					break;
				case 2:
				case 3:
				case 4:
					result = "миллиарда";
					break;
				default:
					result = "миллиардов";
					break;
				}
			}
			else
			{
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Milliard;
			}
			break;
		case 5:
			if (Thread.CurrentThread.CurrentUICulture.Name.Equals("ru-RU"))
			{
				if (num2 == 1)
				{
					result = "триллионов";
					break;
				}
				switch (num)
				{
				case 1:
					result = ArmSoft.OnlineBanking.Common.Resources.Resources.Trillion;
					break;
				case 2:
				case 3:
				case 4:
					result = "триллиона";
					break;
				default:
					result = "триллионов";
					break;
				}
			}
			else
			{
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Trillion;
			}
			break;
		}
		return result;
	}

	private static string MaskLeavingFirstLetters(string value, int letterCountToLeave)
	{
		if (string.IsNullOrEmpty(value))
		{
			return string.Empty;
		}
		if (value.Length <= letterCountToLeave)
		{
			letterCountToLeave = value.Length - 1;
		}
		return value.Substring(0, letterCountToLeave) + new string('*', value.Length - letterCountToLeave);
	}
}
