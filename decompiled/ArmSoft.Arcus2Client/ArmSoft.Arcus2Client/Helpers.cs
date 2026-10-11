using System.Text;

namespace ArmSoft.Arcus2Client;

internal static class Helpers
{
	internal static string RemoveTrailingZerosAfterPoint(decimal value)
	{
		return value.ToString("0.####");
	}

	internal static byte[] ExchangeToCoinsAndConvertToBytes(decimal value)
	{
		string s = RemoveTrailingZerosAfterPoint(value * 100m);
		return Encoding.ASCII.GetBytes(s);
	}

	internal static bool Arcus2TransactionAccepted(int code)
	{
		if (code != 0 && code != 1 && code != 3)
		{
			return code == 20;
		}
		return true;
	}
}
