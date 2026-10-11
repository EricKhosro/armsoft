using System;

namespace ArmSoft.SME.Common.Extensions;

public static class TimeStamp
{
	public static bool SequenceEqual(byte[] ts1, byte[] ts2)
	{
		if (ts1 != null && ts2 != null)
		{
			if (ts1[0] == ts2[0] && ts1[1] == ts2[1] && ts1[2] == ts2[2] && ts1[3] == ts2[3] && ts1[4] == ts2[4] && ts1[5] == ts2[5] && ts1[6] == ts2[6])
			{
				return ts1[7] == ts2[7];
			}
			return false;
		}
		return false;
	}

	public static byte[] NewTS()
	{
		return new byte[8];
	}

	public static bool IsGreater(this byte[] ts1, byte[] ts2)
	{
		if (ts1 == null || ts2 == null)
		{
			return false;
		}
		if (ts1.Length != ts2.Length)
		{
			throw new ArgumentException("Timestamps must be the same length");
		}
		for (int i = 0; i < ts1.Length; i++)
		{
			int num = ts1[i].CompareTo(ts2[i]);
			if (num != 0)
			{
				return num > 0;
			}
		}
		return false;
	}

	public static byte[] Max(byte[] ts1, byte[] ts2)
	{
		if (!ts1.IsGreater(ts2))
		{
			return ts2;
		}
		return ts1;
	}
}
