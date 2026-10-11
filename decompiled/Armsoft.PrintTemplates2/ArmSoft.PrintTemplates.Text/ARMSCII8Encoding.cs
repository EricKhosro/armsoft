using System;
using System.Collections.Generic;
using System.Text;
using ArmSoft.PrintTemplates.My.Resources;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ArmSoft.PrintTemplates.Text;

internal class ARMSCII8Encoding : System.Text.Encoding
{
	private static readonly Dictionary<int, int> mARMSCII8ToUnicode;

	private static readonly Dictionary<int, int> mUnicodeToARMSCII8;

	private readonly bool mThrowOnInvalidBytes;

	public override string BodyName => "armscii-8";

	public override string HeaderName => BodyName;

	public override string EncodingName => "ARMSCII-8";

	public override int WindowsCodePage => 1252;

	public override bool IsBrowserDisplay => false;

	public override bool IsBrowserSave => false;

	public override bool IsMailNewsDisplay => false;

	public override bool IsMailNewsSave => false;

	public ARMSCII8Encoding()
		: this(throwOnInvalidBytes: false)
	{
	}

	public ARMSCII8Encoding(bool throwOnInvalidBytes)
		: base(1067)
	{
		mThrowOnInvalidBytes = throwOnInvalidBytes;
	}

	static ARMSCII8Encoding()
	{
		mARMSCII8ToUnicode = new Dictionary<int, int>();
		mUnicodeToARMSCII8 = new Dictionary<int, int>();
		AddMapping(0, 0);
		AddMapping(1, 1);
		AddMapping(2, 2);
		AddMapping(3, 3);
		AddMapping(4, 4);
		AddMapping(5, 5);
		AddMapping(6, 6);
		AddMapping(7, 7);
		AddMapping(8, 8);
		AddMapping(9, 9);
		AddMapping(10, 10);
		AddMapping(11, 11);
		AddMapping(12, 12);
		AddMapping(13, 13);
		AddMapping(14, 14);
		AddMapping(15, 15);
		AddMapping(16, 16);
		AddMapping(17, 17);
		AddMapping(18, 18);
		AddMapping(19, 19);
		AddMapping(20, 20);
		AddMapping(21, 21);
		AddMapping(22, 22);
		AddMapping(23, 23);
		AddMapping(24, 24);
		AddMapping(25, 25);
		AddMapping(26, 26);
		AddMapping(27, 27);
		AddMapping(28, 28);
		AddMapping(29, 29);
		AddMapping(30, 30);
		AddMapping(31, 31);
		AddMapping(32, 32);
		AddMapping(33, 33);
		AddMapping(34, 34);
		AddMapping(35, 35);
		AddMapping(36, 36);
		AddMapping(37, 37);
		AddMapping(38, 38);
		AddMapping(39, 39);
		AddMapping(40, 40);
		AddMapping(41, 41);
		AddMapping(42, 42);
		AddMapping(43, 43);
		AddMapping(44, 44);
		AddMapping(45, 45);
		AddMapping(46, 46);
		AddMapping(47, 47);
		AddMapping(48, 48);
		AddMapping(49, 49);
		AddMapping(50, 50);
		AddMapping(51, 51);
		AddMapping(52, 52);
		AddMapping(53, 53);
		AddMapping(54, 54);
		AddMapping(55, 55);
		AddMapping(56, 56);
		AddMapping(57, 57);
		AddMapping(58, 58);
		AddMapping(59, 59);
		AddMapping(60, 60);
		AddMapping(61, 61);
		AddMapping(62, 62);
		AddMapping(63, 63);
		AddMapping(64, 64);
		AddMapping(65, 65);
		AddMapping(66, 66);
		AddMapping(67, 67);
		AddMapping(68, 68);
		AddMapping(69, 69);
		AddMapping(70, 70);
		AddMapping(71, 71);
		AddMapping(72, 72);
		AddMapping(73, 73);
		AddMapping(74, 74);
		AddMapping(75, 75);
		AddMapping(76, 76);
		AddMapping(77, 77);
		AddMapping(78, 78);
		AddMapping(79, 79);
		AddMapping(80, 80);
		AddMapping(81, 81);
		AddMapping(82, 82);
		AddMapping(83, 83);
		AddMapping(84, 84);
		AddMapping(85, 85);
		AddMapping(86, 86);
		AddMapping(87, 87);
		AddMapping(88, 88);
		AddMapping(89, 89);
		AddMapping(90, 90);
		AddMapping(91, 91);
		AddMapping(92, 92);
		AddMapping(93, 93);
		AddMapping(94, 94);
		AddMapping(95, 95);
		AddMapping(96, 96);
		AddMapping(97, 97);
		AddMapping(98, 98);
		AddMapping(99, 99);
		AddMapping(100, 100);
		AddMapping(101, 101);
		AddMapping(102, 102);
		AddMapping(103, 103);
		AddMapping(104, 104);
		AddMapping(105, 105);
		AddMapping(106, 106);
		AddMapping(107, 107);
		AddMapping(108, 108);
		AddMapping(109, 109);
		AddMapping(110, 110);
		AddMapping(111, 111);
		AddMapping(112, 112);
		AddMapping(113, 113);
		AddMapping(114, 114);
		AddMapping(115, 115);
		AddMapping(116, 116);
		AddMapping(117, 117);
		AddMapping(118, 118);
		AddMapping(119, 119);
		AddMapping(120, 120);
		AddMapping(121, 121);
		AddMapping(122, 122);
		AddMapping(123, 123);
		AddMapping(124, 124);
		AddMapping(125, 125);
		AddMapping(126, 126);
		AddMapping(127, 127);
		AddMapping(130, 9484);
		AddMapping(131, 9488);
		AddMapping(132, 9524);
		AddMapping(133, 9508);
		AddMapping(134, 9500);
		AddMapping(135, 9492);
		AddMapping(136, 9496);
		AddMapping(137, 9472);
		AddMapping(139, 9474);
		AddMapping(140, 9516);
		AddMapping(160, 160);
		AddMapping(162, 1415);
		AddMapping(163, 1417);
		AddMapping(164, 41);
		AddMapping(165, 40);
		AddMapping(166, 187);
		AddMapping(167, 171);
		AddMapping(168, 8212);
		AddMapping(169, 46);
		AddMapping(170, 1373);
		AddMapping(171, 44);
		AddMapping(172, 45);
		AddMapping(173, 1418);
		AddMapping(174, 8230);
		AddMapping(175, 1372);
		AddMapping(176, 1371);
		AddMapping(177, 1374);
		AddMapping(178, 1329);
		AddMapping(179, 1377);
		AddMapping(180, 1330);
		AddMapping(181, 1378);
		AddMapping(182, 1331);
		AddMapping(183, 1379);
		AddMapping(184, 1332);
		AddMapping(185, 1380);
		AddMapping(186, 1333);
		AddMapping(187, 1381);
		AddMapping(188, 1334);
		AddMapping(189, 1382);
		AddMapping(190, 1335);
		AddMapping(191, 1383);
		AddMapping(192, 1336);
		AddMapping(193, 1384);
		AddMapping(194, 1337);
		AddMapping(195, 1385);
		AddMapping(196, 1338);
		AddMapping(197, 1386);
		AddMapping(198, 1339);
		AddMapping(199, 1387);
		AddMapping(200, 1340);
		AddMapping(201, 1388);
		AddMapping(202, 1341);
		AddMapping(203, 1389);
		AddMapping(204, 1342);
		AddMapping(205, 1390);
		AddMapping(206, 1343);
		AddMapping(207, 1391);
		AddMapping(208, 1344);
		AddMapping(209, 1392);
		AddMapping(210, 1345);
		AddMapping(211, 1393);
		AddMapping(212, 1346);
		AddMapping(213, 1394);
		AddMapping(214, 1347);
		AddMapping(215, 1395);
		AddMapping(216, 1348);
		AddMapping(217, 1396);
		AddMapping(218, 1349);
		AddMapping(219, 1397);
		AddMapping(220, 1350);
		AddMapping(221, 1398);
		AddMapping(222, 1351);
		AddMapping(223, 1399);
		AddMapping(224, 1352);
		AddMapping(225, 1400);
		AddMapping(226, 1353);
		AddMapping(227, 1401);
		AddMapping(228, 1354);
		AddMapping(229, 1402);
		AddMapping(230, 1355);
		AddMapping(231, 1403);
		AddMapping(232, 1356);
		AddMapping(233, 1404);
		AddMapping(234, 1357);
		AddMapping(235, 1405);
		AddMapping(236, 1358);
		AddMapping(237, 1406);
		AddMapping(238, 1359);
		AddMapping(239, 1407);
		AddMapping(240, 1360);
		AddMapping(241, 1408);
		AddMapping(242, 1361);
		AddMapping(243, 1409);
		AddMapping(244, 1362);
		AddMapping(245, 1410);
		AddMapping(246, 1363);
		AddMapping(247, 1411);
		AddMapping(248, 1364);
		AddMapping(249, 1412);
		AddMapping(250, 1365);
		AddMapping(251, 1413);
		AddMapping(252, 1366);
		AddMapping(253, 1414);
		AddMapping(254, 1370);
	}

	private static void AddMapping(int armscii8Point, int unicodePoint)
	{
		if (armscii8Point < 0 || armscii8Point > 255)
		{
			throw new ArgumentOutOfRangeException("armscii8Point");
		}
		if (unicodePoint < Strings.AscW('\0') || unicodePoint > Strings.AscW('\uffff'))
		{
			throw new ArgumentOutOfRangeException("unicodePoint");
		}
		if (!mARMSCII8ToUnicode.ContainsKey(armscii8Point))
		{
			mARMSCII8ToUnicode.Add(armscii8Point, unicodePoint);
		}
		if (!mUnicodeToARMSCII8.ContainsKey(unicodePoint))
		{
			mUnicodeToARMSCII8.Add(unicodePoint, armscii8Point);
		}
	}

	public override int GetByteCount(char[] chars, int index, int count)
	{
		if (chars == null)
		{
			throw new ArgumentNullException("chars");
		}
		if (index < chars.GetLowerBound(0) || index > chars.GetUpperBound(0))
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (count < 0 || index + count - 1 > chars.GetUpperBound(0))
		{
			throw new ArgumentOutOfRangeException("count");
		}
		int num = 0;
		while (count > 0)
		{
			count--;
			int key = Strings.AscW(chars[index]);
			index++;
			if (mUnicodeToARMSCII8.ContainsKey(key))
			{
				num++;
			}
			else if (mThrowOnInvalidBytes)
			{
				throw new ArgumentException(Resources.Encoding_InvalidCharacters, "chars");
			}
		}
		return num;
	}

	public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
	{
		if (chars == null)
		{
			throw new ArgumentNullException("chars");
		}
		if (bytes == null)
		{
			throw new ArgumentNullException("bytes");
		}
		if (charIndex < chars.GetLowerBound(0) || charIndex > chars.GetUpperBound(0))
		{
			throw new ArgumentOutOfRangeException("charIndex");
		}
		if (charCount < 0 || charIndex + charCount - 1 > chars.GetUpperBound(0))
		{
			throw new ArgumentOutOfRangeException("charCount");
		}
		if (chars.Length > 1 && (byteIndex < bytes.GetLowerBound(0) || byteIndex > bytes.GetUpperBound(0)))
		{
			throw new ArgumentOutOfRangeException("byteIndex");
		}
		int num = 0;
		int i = 0;
		for (int num2 = charCount - 1; i <= num2; i++)
		{
			int key = Strings.AscW(chars[charIndex + i]);
			if (mUnicodeToARMSCII8.ContainsKey(key))
			{
				int num3 = mUnicodeToARMSCII8[key];
				if (byteIndex > bytes.GetUpperBound(0))
				{
					throw new ArgumentException(string.Format(Resources.Encoding_ConversionOverflowBytes, EncodingName), "bytes");
				}
				bytes[byteIndex] = Conversions.ToByte(num3);
				byteIndex++;
				num++;
			}
			else if (mThrowOnInvalidBytes)
			{
				throw new ArgumentException(Resources.Encoding_InvalidCharacters, "chars");
			}
		}
		return num;
	}

	public override int GetCharCount(byte[] bytes, int index, int count)
	{
		if (bytes == null)
		{
			throw new ArgumentNullException("bytes");
		}
		if (index < bytes.GetLowerBound(0) || index > bytes.GetUpperBound(0))
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (count < 0 || index + count - 1 > bytes.GetUpperBound(0))
		{
			throw new ArgumentOutOfRangeException("count");
		}
		int num = 0;
		while (count > 0)
		{
			count--;
			int key = Conversions.ToInteger(bytes[index]);
			index++;
			if (mARMSCII8ToUnicode.ContainsKey(key))
			{
				num++;
			}
			else if (mThrowOnInvalidBytes)
			{
				throw new ArgumentException(Resources.Encoding_InvalidCharacters, "bytes");
			}
		}
		return num;
	}

	public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
	{
		if (bytes == null)
		{
			throw new ArgumentNullException("bytes");
		}
		if (chars == null)
		{
			throw new ArgumentNullException("chars");
		}
		if (byteIndex < bytes.GetLowerBound(0) || byteIndex > bytes.GetUpperBound(0))
		{
			throw new ArgumentOutOfRangeException("byteIndex");
		}
		if (byteCount < 0 || byteIndex + byteCount - 1 > bytes.GetUpperBound(0))
		{
			throw new ArgumentOutOfRangeException("byteCount");
		}
		if (charIndex < chars.GetLowerBound(0) || charIndex > chars.GetUpperBound(0))
		{
			throw new ArgumentOutOfRangeException("charIndex");
		}
		int num = 0;
		int i = 0;
		for (int num2 = byteCount - 1; i <= num2; i++)
		{
			int key = Conversions.ToInteger(bytes[byteIndex + i]);
			if (mARMSCII8ToUnicode.ContainsKey(key))
			{
				int num3 = mARMSCII8ToUnicode[key];
				if (charIndex > chars.GetUpperBound(0))
				{
					throw new ArgumentException(string.Format(Resources.Encoding_ConversionOverflowChars, EncodingName), "chars");
				}
				chars[charIndex] = (char)num3;
				charIndex++;
				num++;
			}
			else if (mThrowOnInvalidBytes)
			{
				throw new ArgumentException(Resources.Encoding_InvalidCharacters, "bytes");
			}
		}
		return num;
	}

	public override int GetMaxByteCount(int charCount)
	{
		return charCount;
	}

	public override int GetMaxCharCount(int byteCount)
	{
		return byteCount;
	}
}
