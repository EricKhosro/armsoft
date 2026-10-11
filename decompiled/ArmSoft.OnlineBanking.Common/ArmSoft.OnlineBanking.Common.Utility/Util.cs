using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OnlineBanking.Common.Resources;

namespace ArmSoft.OnlineBanking.Common.Utility;

public static class Util
{
	private const string CharsLowerCase = "abcdefgijkmnopqrstwxyz";

	private const string CharsUpperCase = "ABCDEFGHJKLMNPQRSTWXYZ";

	private const string CharsNumeric = "23456789";

	private const string PasswordCharsSpecial = "#$%&@";

	public static long IPAddressToNumber(IPAddress address)
	{
		long num = 0L;
		byte[] addressBytes = address.GetAddressBytes();
		for (int i = 0; i <= 3; i++)
		{
			long num2 = ((addressBytes[i] < 0) ? (addressBytes[i] + 256) : addressBytes[i]);
			num += num2 << (3 - i) * 8;
		}
		return num;
	}

	public static string GetAssemblyVersion(Assembly assembly)
	{
		return FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
	}

	public static string GetBrowserNameWithVersion(string userAgent)
	{
		string result = string.Empty;
		if (userAgent.IndexOf("Edg") > -1)
		{
			result = "Edge " + userAgent.Split(new string[1] { "Edg/" }, StringSplitOptions.None)[1].Split('.')[0];
		}
		else if (userAgent.IndexOf("Chrome") > -1)
		{
			result = "Chrome " + userAgent.Split(new string[1] { "Chrome/" }, StringSplitOptions.None)[1].Split('.')[0];
		}
		else if (userAgent.IndexOf("Safari") > -1)
		{
			result = "Safari " + userAgent.Split(new string[1] { "Safari/" }, StringSplitOptions.None)[1].Split('.')[0];
		}
		else if (userAgent.IndexOf("Firefox") > -1)
		{
			result = "Firefox " + userAgent.Split(new string[1] { "Firefox/" }, StringSplitOptions.None)[1].Split('.')[0];
		}
		else if (userAgent.IndexOf("rv") > -1)
		{
			result = "Internet Explorer " + userAgent.Split(new string[1] { "rv:" }, StringSplitOptions.None)[1].Split('.')[0];
		}
		else if (userAgent.IndexOf("MSIE") > -1)
		{
			result = "Internet Explorer " + userAgent.Split(new string[1] { "MSIE" }, StringSplitOptions.None)[1].Split('.')[0];
		}
		else if (userAgent.IndexOf("Other") > -1)
		{
			result = "Other " + userAgent.Split(new string[1] { "Other" }, StringSplitOptions.None)[1].Split('.')[0];
		}
		return result;
	}

	public static string GetUserPlatform(string userAgent)
	{
		string result = null;
		if (!string.IsNullOrEmpty(userAgent))
		{
			if (userAgent.Contains("Android"))
			{
				result = "Android " + GetPlatformVersion(userAgent, "Android");
			}
			else if (userAgent.Contains("iPad"))
			{
				result = "iPad OS " + GetPlatformVersion(userAgent, "OS");
			}
			else if (userAgent.Contains("iPhone"))
			{
				result = "iPhone OS " + GetPlatformVersion(userAgent, "OS");
			}
			else if (userAgent.Contains("Linux") && userAgent.Contains("KFAPWI"))
			{
				result = "Kindle Fire";
			}
			else if (userAgent.Contains("RIM Tablet") || (userAgent.Contains("BB") && userAgent.Contains("Mobile")))
			{
				result = "Black Berry";
			}
			else if (userAgent.Contains("Windows Phone"))
			{
				result = "Windows Phone " + GetPlatformVersion(userAgent, "Windows Phone");
			}
			else if (userAgent.Contains("Mac OS"))
			{
				result = "Mac OS X " + GetPlatformVersion(userAgent, "Mac OS X");
			}
			else if (userAgent.Contains("Windows NT 5.1") || userAgent.Contains("Windows NT 5.2"))
			{
				result = "Windows XP";
			}
			else if (userAgent.Contains("Windows NT 6.0"))
			{
				result = "Windows Vista";
			}
			else if (userAgent.Contains("Windows NT 6.1"))
			{
				result = "Windows 7";
			}
			else if (userAgent.Contains("Windows NT 6.2"))
			{
				result = "Windows 8";
			}
			else if (userAgent.Contains("Windows NT 6.3"))
			{
				result = "Windows 8.1";
			}
			else if (userAgent.Contains("Windows NT 10"))
			{
				result = "Windows 10";
			}
		}
		return result;
	}

	public static string GetPlatformVersion(string userAgent, string device)
	{
		string text = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
		StringBuilder stringBuilder = new StringBuilder();
		string text2 = text;
		foreach (char c in text2)
		{
			bool flag = false;
			if (char.IsNumber(c))
			{
				stringBuilder.Append(c);
				flag = true;
			}
			if (c == '.' || c == '_')
			{
				stringBuilder.Append(".");
				flag = true;
			}
			if (!flag)
			{
				break;
			}
		}
		return stringBuilder.ToString();
	}

	public static UtilityType GetUtilityType(string utilityCode, string utilityTypeCode)
	{
		string text = (utilityCode ?? string.Empty).Trim();
		if (text == string.Empty)
		{
			return UtilityType.Unknown;
		}
		if (text == "M")
		{
			return UtilityType.Mobile;
		}
		return (utilityTypeCode ?? string.Empty).Trim() switch
		{
			"MO" => UtilityType.UComMobile, 
			"MV" => UtilityType.VivacellMobile, 
			"MA" => UtilityType.TeamMobile, 
			"MK" => UtilityType.KharabakhTelecomMobile, 
			"P" => UtilityType.FixedPhone, 
			"E" => UtilityType.Electricity, 
			"G" => UtilityType.Gas, 
			"W" => UtilityType.Water, 
			"R" => UtilityType.GarbageCollection, 
			_ => UtilityType.Unknown, 
		};
	}

	public static string GenerateNextDocumentNumber(string previousNumber)
	{
		int num = 6;
		StringBuilder stringBuilder = new StringBuilder(previousNumber.Trim());
		int length = stringBuilder.Length;
		if (num >= length && length > 0)
		{
			for (int num2 = length - 1; num2 >= 0; num2--)
			{
				char c = stringBuilder[num2];
				if (c >= '0' && c < '9')
				{
					stringBuilder[num2] = (char)(c + 1);
					break;
				}
				if (c == '9')
				{
					stringBuilder[num2] = '0';
				}
				else
				{
					if ((c >= 'a' && c < 'z') || (c >= 'A' && c < 'Z'))
					{
						if (num == length || num2 == length - 1)
						{
							stringBuilder[num2] = (char)(c + 1);
						}
						else
						{
							stringBuilder.Insert(num2, '1');
						}
						break;
					}
					if (c != 'z' && c != 'Z')
					{
						if (num > length && num2 < length - 1)
						{
							stringBuilder.Insert(num2, '1');
						}
						break;
					}
					if (num != length && num2 != length - 1)
					{
						stringBuilder.Insert(num2, '1');
						break;
					}
					stringBuilder[num2] = (char)(c - 25);
				}
			}
		}
		return stringBuilder.ToString();
	}

	public static string GetAttachedCardColor(string bankName)
	{
		string result = null;
		bankName = bankName.ToLower().Trim();
		if (bankName.Contains("abb") || (bankName.Contains("business") && bankName.Contains("arm")))
		{
			result = "#E30005";
		}
		else if (bankName.Contains("ameria"))
		{
			result = "#72BF44";
		}
		else if (bankName.Contains("ararat"))
		{
			result = "#FAB914";
		}
		else if (bankName.Contains("swiss"))
		{
			result = "#003B78";
		}
		else if (bankName.Contains("byblos"))
		{
			result = "#513069";
		}
		else if (bankName.Contains("converse"))
		{
			result = "#002366";
		}
		else if (bankName.Contains("evoca"))
		{
			result = "#6400DC";
		}
		else if (bankName.Contains("acba"))
		{
			result = "#18A183";
		}
		else if (bankName.Contains("ardshin"))
		{
			result = "#203D8C";
		}
		else if (bankName.Contains("ineco"))
		{
			result = "#085D35";
		}
		else if (bankName.Contains("aeb") || bankName.Contains("econom"))
		{
			result = "#104B83";
		}
		else if (bankName.Contains("artsakh"))
		{
			result = "#266D4F";
		}
		else if (bankName.Contains("hsbc"))
		{
			result = "#E12524";
		}
		else if (bankName.Contains("id"))
		{
			result = "#E9862F";
		}
		else if (bankName.Contains("uni"))
		{
			result = "#02296A";
		}
		else if (bankName.Contains("amio"))
		{
			result = "#56D1E6";
		}
		return result;
	}

	public static List<DateTime> SplitDateInterval(DateTime startDate, DateTime endDate, int internalPointsCount)
	{
		List<DateTime> list = new List<DateTime>();
		int num = (int)Math.Ceiling((endDate - startDate).TotalDays / (double)(internalPointsCount + 1));
		DateTime dateTime = startDate.Date;
		while (dateTime < endDate.Date)
		{
			list.Add(dateTime);
			dateTime = dateTime.AddDays(num);
		}
		list.Add(endDate.Date);
		return list;
	}

	public static bool IsCardTransferReceiverNameDefault(string receiverName)
	{
		if (!string.IsNullOrEmpty(receiverName))
		{
			if (!receiverName.Equals("Card Holder", StringComparison.CurrentCultureIgnoreCase))
			{
				return receiverName.Equals("Firstname Lastname", StringComparison.CurrentCultureIgnoreCase);
			}
			return true;
		}
		return false;
	}

	public static (DateTime dateFrom, DateTime dateTo) GetTimePeriodInterval(Period timePeriod)
	{
		DateTime item = DateTime.Today;
		switch (timePeriod)
		{
		case Period.Yesterday:
			item = DateTime.Today.AddDays(-1.0);
			break;
		case Period.PreviousMonth:
			item = DateTime.Today.AddDays(-DateTime.Today.Day);
			break;
		}
		DateTime item2 = DateTime.Today;
		switch (timePeriod)
		{
		case Period.Today:
			item2 = DateTime.Today;
			break;
		case Period.Yesterday:
			item2 = DateTime.Today.AddDays(-1.0);
			break;
		case Period.Last3Days:
			item2 = DateTime.Today.AddDays(-2.0);
			break;
		case Period.Last7Days:
			item2 = DateTime.Today.AddDays(-6.0);
			break;
		case Period.Last31Days:
			item2 = DateTime.Today.AddDays(-30.0);
			break;
		case Period.Last3Months:
			item2 = DateTime.Today.AddMonths(-3);
			break;
		case Period.Last6Months:
			item2 = DateTime.Today.AddMonths(-6);
			break;
		case Period.LastYear:
			item2 = DateTime.Today.AddMonths(-12);
			break;
		case Period.CurrentMonth:
			item2 = DateTime.Today.AddDays(1 - DateTime.Today.Day);
			break;
		case Period.PreviousMonth:
			item2 = DateTime.Today.AddDays(1 - DateTime.Today.Day).AddMonths(-1);
			break;
		case Period.CurrentYear:
			item2 = DateTime.Today.AddDays(1 - DateTime.Today.Day).AddMonths(1 - DateTime.Today.Month);
			break;
		}
		return (dateFrom: item2, dateTo: item);
	}

	public static string FormatFixedRate(decimal value)
	{
		return value.ToString("#0.00") + CultureInfo.InvariantCulture.NumberFormat.PercentSymbol;
	}

	public static int? GetImportedDocumentsCount(Stream xmlStream)
	{
		if (xmlStream == null || !xmlStream.CanRead)
		{
			return null;
		}
		try
		{
			xmlStream.Position = 0L;
			using StreamReader streamReader = new StreamReader(xmlStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, 1024, leaveOpen: true);
			string xml = streamReader.ReadToEnd();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			int num = 0;
			string[] array = new string[7] { "PayBudg", "PayFor", "PayOrd", "PayMltp", "PassPay", "PayForRub", "PayAdvancedOrd" };
			foreach (string name in array)
			{
				foreach (XmlNode item in xmlDocument.GetElementsByTagName(name))
				{
					if (item.Attributes != null && item.Attributes["CAPTION"] == null)
					{
						num++;
					}
				}
			}
			return num;
		}
		catch (XmlException)
		{
			return null;
		}
	}

	public static string GetUsernamePolicyHint(int minRequiredUsernameLength, int minRequiredNumericCharacters)
	{
		if (minRequiredNumericCharacters > 0)
		{
			return string.Format(ArmSoft.OnlineBanking.Common.Resources.Resources.UsernamePolicy, minRequiredUsernameLength, minRequiredNumericCharacters);
		}
		return string.Format(ArmSoft.OnlineBanking.Common.Resources.Resources.UsernamePolicy, minRequiredUsernameLength);
	}

	public static string GenerateRandomString(int length, bool useOnlyNumbers = false)
	{
		if (length <= 0)
		{
			throw new ArgumentException("length");
		}
		char[][] array = ((!useOnlyNumbers) ? new char[4][]
		{
			"abcdefgijkmnopqrstwxyz".ToCharArray(),
			"ABCDEFGHJKLMNPQRSTWXYZ".ToCharArray(),
			"23456789".ToCharArray(),
			"#$%&@".ToCharArray()
		} : new char[1][] { "23456789".ToCharArray() });
		int[] array2 = new int[array.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = array[i].Length;
		}
		int[] array3 = new int[array.Length];
		for (int j = 0; j < array3.Length; j++)
		{
			array3[j] = j;
		}
		byte[] array4 = new byte[4];
		RandomNumberGenerator.Create().GetBytes(array4);
		Random random = new Random(((array4[0] & 0x7F) << 24) | (array4[1] << 16) | (array4[2] << 8) | array4[3]);
		char[] array5 = new char[length];
		int num = array3.Length - 1;
		for (int k = 0; k < array5.Length; k++)
		{
			int num2 = ((num != 0) ? random.Next(0, num) : 0);
			int num3 = array3[num2];
			int num4 = array2[num3] - 1;
			int num5 = ((num4 != 0) ? random.Next(0, num4 + 1) : 0);
			array5[k] = array[num3][num5];
			if (num4 == 0)
			{
				array2[num3] = array[num3].Length;
			}
			else
			{
				if (num4 != num5)
				{
					char c = array[num3][num4];
					array[num3][num4] = array[num3][num5];
					array[num3][num5] = c;
				}
				array2[num3]--;
			}
			if (num == 0)
			{
				num = array3.Length - 1;
				continue;
			}
			if (num != num2)
			{
				int num6 = array3[num];
				array3[num] = array3[num2];
				array3[num2] = num6;
			}
			num--;
		}
		return new string(array5);
	}

	public static string ConvertToString(object obj)
	{
		string result = null;
		if (obj != null)
		{
			Type type = obj.GetType();
			if (type.IsEnum)
			{
				object obj2 = Convert.ChangeType(obj, Enum.GetUnderlyingType(type), CultureInfo.InvariantCulture);
				result = TypeDescriptor.GetConverter(obj2.GetType()).ConvertToString(null, CultureInfo.InvariantCulture, obj2);
			}
			else
			{
				result = TypeDescriptor.GetConverter(type).ConvertToString(null, CultureInfo.InvariantCulture, obj);
			}
		}
		return result;
	}

	public static string GetContentType(string extension)
	{
		return extension.ToLowerInvariant() switch
		{
			".pdf" => "application/pdf", 
			".html" => "text/html", 
			".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
			".xls" => "application/vnd.ms-excel", 
			".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
			".doc" => "application/msword", 
			".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation", 
			".ppt" => "application/vnd.ms-powerpoint", 
			".csv" => "text/csv", 
			".jpg" => "image/jpeg", 
			".jpeg" => "image/jpeg", 
			".png" => "image/png", 
			".gif" => "image/gif", 
			".zip" => "application/zip", 
			_ => "application/octet-stream", 
		};
	}

	public static bool IsMobileVersionSupported(string clientVersion, Version defaultMinVersion, params (int minor, Version minVersion)[] minorMinVersions)
	{
		if (!Version.TryParse(clientVersion, out Version result))
		{
			return false;
		}
		if (result.Major > defaultMinVersion.Major)
		{
			return true;
		}
		if (result.Major < defaultMinVersion.Major)
		{
			return false;
		}
		for (int i = 0; i < minorMinVersions.Length; i++)
		{
			var (num, version) = minorMinVersions[i];
			if (result.Minor == num)
			{
				return result >= version;
			}
		}
		return result >= defaultMinVersion;
	}
}
