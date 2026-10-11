using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;

namespace ArmSoft.AS8X.Common.Language.Extensions;

internal static class LanguageExtantions
{
	private static Dictionary<string, string> ArmenianANSIValues { get; set; } = new Dictionary<string, string>();

	public static bool ContainsOnlyWindows1252(this string value)
	{
		if (value == null)
		{
			return true;
		}
		foreach (char c in value)
		{
			if (c > 'ÿ' && c.FromWindows1252ToANSI() == -1)
			{
				return false;
			}
		}
		return true;
	}

	public static bool CanBeConvertedToArmenianANSI(this string value, out char? invalidChar)
	{
		invalidChar = null;
		if (value == null)
		{
			return true;
		}
		for (int i = 0; i < value.Length; i++)
		{
			if (!CanBeConvertedToArmenianANSI(value[i]))
			{
				invalidChar = value[i];
				return false;
			}
		}
		return true;
	}

	public static bool CanBeConvertedToRussianANSI(this string value, out char? invalidChar)
	{
		invalidChar = null;
		if (value == null)
		{
			return true;
		}
		for (int i = 0; i < value.Length; i++)
		{
			if (value[i] >= '\u0080' && !IsRussianUnicode(value[i]))
			{
				invalidChar = value[i];
				return false;
			}
		}
		return true;
	}

	public static bool CanBeConvertedToRussianUnicode(this string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return true;
		}
		if (value.ToRussianUnicode().ToRussianANSI() == value)
		{
			return true;
		}
		return false;
	}

	public static string ToArmenianANSI(this string value)
	{
		return value.ToANSI();
	}

	public static string ToArmenianANSICached(this string value)
	{
		if (ArmenianANSIValues.TryGetValue(value, out var value2))
		{
			return value2;
		}
		lock (ArmenianANSIValues)
		{
			if (!ArmenianANSIValues.TryGetValue(value, out value2))
			{
				value2 = value.ToArmenianANSI();
				ArmenianANSIValues.Add(value, value2);
			}
			return value2;
		}
	}

	public static char ToANSI(this char source)
	{
		return (char)FromUnicodeToInt(source);
	}

	public static string ToRussianANSI(this string value)
	{
		if (value.CanBeConvertedToRussianANSI(out var _))
		{
			return value.ToANSI();
		}
		return value;
	}

	public static string ToRussianUnicode(this string value)
	{
		if (value == null)
		{
			return null;
		}
		string text = string.Empty;
		for (int i = 0; i < value.Length; i++)
		{
			text = ((Strings.Asc(value[i]) >= 256) ? (text + value[i]) : (text + Encoding.GetEncoding(1251).GetString(Encoding.GetEncoding(1252).GetBytes(new char[1] { value[i] }))));
		}
		return text;
	}

	public static string ToArmenianUnicode(this string value)
	{
		if (value == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder(value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			stringBuilder.Append(value[i].ToArmenianUnicode());
		}
		return stringBuilder.ToString();
	}

	public static byte[] ToArmenianANSIByteArray(this string value)
	{
		if (value == null)
		{
			return null;
		}
		byte[] array = new byte[value.Length];
		for (int i = 0; i < value.Length; i++)
		{
			array[i] = (byte)FromUnicodeToInt(value[i]);
		}
		return array;
	}

	public static byte[] ANSIToByteArray(this string value)
	{
		if (value == null)
		{
			return null;
		}
		byte[] array = new byte[value.Length];
		for (int i = 0; i < value.Length; i++)
		{
			array[i] = (byte)value[i];
		}
		return array;
	}

	public static char ToArmenianUnicode(this char source)
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
			130 => '‚', 
			131 => 'ƒ', 
			132 => '„', 
			133 => '…', 
			134 => '†', 
			135 => '‡', 
			136 => 'ˆ', 
			137 => '‰', 
			138 => 'Š', 
			139 => '‹', 
			140 => 'Œ', 
			172 => '─', 
			_ => source, 
		};
	}

	public static int FromANSIToWindows1252(this char source)
	{
		switch (source)
		{
		case '\u0080':
			return 8364;
		case '\u0082':
			return 8218;
		case '\u0083':
			return 402;
		case '\u0084':
			return 8222;
		case '\u0085':
			return 8230;
		case '\u0086':
			return 8224;
		case '\u0087':
			return 8225;
		case '\u0088':
			return 710;
		case '\u0089':
			return 8240;
		case '\u008a':
			return 352;
		case '\u008b':
			return 8249;
		case '\u008c':
			return 338;
		case '\u008e':
			return 381;
		case '\u0091':
			return 8216;
		case '\u0092':
			return 8217;
		case '\u0093':
			return 8220;
		case '\u0094':
			return 8221;
		case '\u0095':
			return 8226;
		case '\u0096':
			return 8211;
		case '\u0097':
			return 8212;
		case '\u0098':
			return 732;
		case '\u0099':
			return 8482;
		case '\u009a':
			return 353;
		case '\u009b':
			return 8250;
		case '\u009c':
			return 339;
		case '\u009e':
			return 382;
		case '\u009f':
			return 376;
		default:
			if (source <= 'ÿ')
			{
				return source;
			}
			return -1;
		}
	}

	public static string FromGeorgianUnicode(string source)
	{
		if (source == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder(source.Length);
		for (int i = 0; i < source.Length; i++)
		{
			stringBuilder.Append(FromGeorgianUnicode(source[i]));
		}
		return stringBuilder.ToString();
	}

	public static char FromGeorgianUnicode(char source)
	{
		return (char)FromGeorgianUnicodeToInt(source);
	}

	public static int FromGeorgianUnicodeToInt(char source)
	{
		int num = source % 256;
		switch (source)
		{
		case 'ა':
			return 192;
		case 'ბ':
			return 193;
		case 'გ':
			return 194;
		case 'დ':
			return 195;
		case 'ე':
			return 196;
		case 'ვ':
			return 197;
		case 'ზ':
			return 198;
		case 'თ':
			return 200;
		case 'ი':
			return 201;
		case 'კ':
			return 202;
		case 'ლ':
			return 203;
		case 'მ':
			return 204;
		case 'ნ':
			return 205;
		case 'ო':
			return 207;
		case 'პ':
			return 208;
		case 'ჟ':
			return 209;
		case 'რ':
			return 210;
		case 'ს':
			return 211;
		case 'ტ':
			return 212;
		case 'უ':
			return 214;
		case 'ფ':
			return 215;
		case 'ქ':
			return 216;
		case 'ღ':
			return 217;
		case 'ყ':
			return 218;
		case 'შ':
			return 219;
		case 'ჩ':
			return 220;
		case 'ც':
			return 221;
		case 'ძ':
			return 222;
		case 'წ':
			return 223;
		case 'ჭ':
			return 224;
		case 'ხ':
			return 225;
		case 'ჯ':
			return 227;
		case 'ჰ':
			return 228;
		case 'ჱ':
			return 199;
		case 'ჲ':
			return 206;
		case 'ჳ':
			return 213;
		case 'ჴ':
			return 226;
		case 'ჵ':
			return 229;
		case 'ჶ':
			return 230;
		case '჻':
			return 238;
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

	public static string ToGeorgianUnicode(string source)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < source.Length; i++)
		{
			int num = source[i];
			switch (num)
			{
			case 192:
				num = 4304;
				break;
			case 193:
				num = 4305;
				break;
			case 194:
				num = 4306;
				break;
			case 195:
				num = 4307;
				break;
			case 196:
				num = 4308;
				break;
			case 197:
				num = 4309;
				break;
			case 198:
				num = 4310;
				break;
			case 200:
				num = 4311;
				break;
			case 201:
				num = 4312;
				break;
			case 202:
				num = 4313;
				break;
			case 203:
				num = 4314;
				break;
			case 204:
				num = 4315;
				break;
			case 205:
				num = 4316;
				break;
			case 207:
				num = 4317;
				break;
			case 208:
				num = 4318;
				break;
			case 209:
				num = 4319;
				break;
			case 210:
				num = 4320;
				break;
			case 211:
				num = 4321;
				break;
			case 212:
				num = 4322;
				break;
			case 214:
				num = 4323;
				break;
			case 215:
				num = 4324;
				break;
			case 216:
				num = 4325;
				break;
			case 217:
				num = 4326;
				break;
			case 218:
				num = 4327;
				break;
			case 219:
				num = 4328;
				break;
			case 220:
				num = 4329;
				break;
			case 221:
				num = 4330;
				break;
			case 222:
				num = 4331;
				break;
			case 223:
				num = 4332;
				break;
			case 224:
				num = 4333;
				break;
			case 225:
				num = 4334;
				break;
			case 227:
				num = 4335;
				break;
			case 228:
				num = 4336;
				break;
			case 199:
				num = 4337;
				break;
			case 206:
				num = 4338;
				break;
			case 213:
				num = 4339;
				break;
			case 226:
				num = 4340;
				break;
			case 229:
				num = 4341;
				break;
			case 230:
				num = 4342;
				break;
			case 238:
				num = 4347;
				break;
			case 167:
				num = 1369;
				break;
			case 166:
				num = 1370;
				break;
			}
			stringBuilder.Append((num < 128) ? source[i] : ((char)num));
		}
		return stringBuilder.ToString();
	}

	private static string ToANSI(this string value)
	{
		if (value == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder(value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			stringBuilder.Append(value[i].ToANSI());
		}
		return stringBuilder.ToString();
	}

	private static int FromWindows1252ToANSI(this char source)
	{
		switch (source)
		{
		case '€':
			return 128;
		case '‚':
			return 130;
		case 'ƒ':
			return 131;
		case '„':
			return 132;
		case '…':
			return 133;
		case '†':
			return 134;
		case '‡':
			return 135;
		case 'ˆ':
			return 136;
		case '‰':
			return 137;
		case 'Š':
			return 138;
		case '‹':
			return 139;
		case 'Œ':
			return 140;
		case 'Ž':
			return 142;
		case '‘':
			return 145;
		case '’':
			return 146;
		case '“':
			return 147;
		case '”':
			return 148;
		case '•':
			return 149;
		case '–':
			return 150;
		case '—':
			return 151;
		case '\u02dc':
			return 152;
		case '™':
			return 153;
		case 'š':
			return 154;
		case '›':
			return 155;
		case 'œ':
			return 156;
		case 'ž':
			return 158;
		case 'Ÿ':
			return 159;
		default:
			if (source <= 'ÿ')
			{
				return source;
			}
			return -1;
		}
	}

	private static int FromUnicodeToInt(char source)
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
		case '․':
			return 46;
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

	private static bool CanBeConvertedToArmenianANSI(char source)
	{
		if (source < 'ÿ')
		{
			return true;
		}
		if ((source >= 'ա') & (source <= 'ֆ'))
		{
			return true;
		}
		if ((source >= 'Ա') & (source <= 'Ֆ'))
		{
			return true;
		}
		return source switch
		{
			'և' => true, 
			'ՙ' => true, 
			'՚' => true, 
			'՛' => true, 
			'՜' => true, 
			'՝' => true, 
			'՞' => true, 
			'՟' => true, 
			'։' => true, 
			'֊' => true, 
			'«' => true, 
			'»' => true, 
			'“' => true, 
			'”' => true, 
			'․' => true, 
			_ => false, 
		};
	}

	private static bool IsRussianUnicode(char ch)
	{
		if (ch >= 'Ѐ')
		{
			return ch <= 'ӿ';
		}
		return false;
	}
}
