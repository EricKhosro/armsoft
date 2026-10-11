using System;
using System.Text;

namespace ArmSoft.PrintTemplates.Text;

internal class Encoding
{
	private static System.Text.Encoding mARMSCII8;

	private static System.Text.Encoding mARMSCII8Throw;

	private static System.Text.Encoding mISO_8859_1;

	private static System.Text.Encoding mWindows1252;

	public static System.Text.Encoding ASCII => System.Text.Encoding.ASCII;

	public static System.Text.Encoding BigEndianUnicode => System.Text.Encoding.BigEndianUnicode;

	public static System.Text.Encoding Default => System.Text.Encoding.Default;

	public static System.Text.Encoding Unicode => System.Text.Encoding.Unicode;

	public static System.Text.Encoding UTF7 => System.Text.Encoding.UTF7;

	public static System.Text.Encoding UTF8 => System.Text.Encoding.UTF8;

	public static System.Text.Encoding ARMSCII8Throw
	{
		get
		{
			if (mARMSCII8Throw == null)
			{
				mARMSCII8Throw = new ARMSCII8Encoding(throwOnInvalidBytes: true);
			}
			return mARMSCII8Throw;
		}
	}

	public static System.Text.Encoding ARMSCII8
	{
		get
		{
			if (mARMSCII8 == null)
			{
				mARMSCII8 = new ARMSCII8Encoding();
			}
			return mARMSCII8;
		}
	}

	public static System.Text.Encoding ISO_8859_1
	{
		get
		{
			if (mISO_8859_1 == null)
			{
				mISO_8859_1 = System.Text.Encoding.GetEncoding("ISO-8859-1");
			}
			return mISO_8859_1;
		}
	}

	public static System.Text.Encoding Windows1252
	{
		get
		{
			if (mWindows1252 == null)
			{
				mWindows1252 = System.Text.Encoding.GetEncoding("Windows-1252");
			}
			return mWindows1252;
		}
	}

	private Encoding()
	{
	}

	public static byte[] Convert(System.Text.Encoding srcEncoding, System.Text.Encoding dstEncoding, byte[] bytes, int index, int count)
	{
		return System.Text.Encoding.Convert(srcEncoding, dstEncoding, bytes, index, count);
	}

	public static byte[] Convert(System.Text.Encoding srcEncoding, System.Text.Encoding dstEncoding, byte[] bytes)
	{
		return System.Text.Encoding.Convert(srcEncoding, dstEncoding, bytes);
	}

	public static string Convert(System.Text.Encoding srcEncoding, System.Text.Encoding dstEncoding, string source)
	{
		if (srcEncoding == null)
		{
			throw new ArgumentNullException("srcEncoding");
		}
		if (dstEncoding == null)
		{
			throw new ArgumentNullException("dstEncoding");
		}
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if ((source ?? "") == (string.Empty ?? ""))
		{
			return string.Empty;
		}
		return dstEncoding.GetString(srcEncoding.GetBytes(source));
	}

	public static string Convert(System.Text.Encoding srcEncoding, System.Text.Encoding dstEncoding, char source)
	{
		if (srcEncoding == null)
		{
			throw new ArgumentNullException("srcEncoding");
		}
		if (dstEncoding == null)
		{
			throw new ArgumentNullException("dstEncoding");
		}
		if (source.ToString() == string.Empty)
		{
			return string.Empty;
		}
		return dstEncoding.GetString(srcEncoding.GetBytes(new char[1] { source }));
	}

	public static System.Text.Encoding GetEncoding(string name)
	{
		if (name.ToUpper() == "ARMSCII8")
		{
			return ARMSCII8;
		}
		return System.Text.Encoding.GetEncoding(name);
	}

	public static System.Text.Encoding GetEncoding(int codepage)
	{
		if (codepage == 1067)
		{
			return ARMSCII8;
		}
		return System.Text.Encoding.GetEncoding(codepage);
	}
}
