using System;

namespace ArmSoft.PossoftPayClient;

public class Helpers
{
	public static long GenerateCRN()
	{
		return long.Parse(DateTime.Now.ToString("MMddHHmmss") + "0000");
	}

	public static long ExchangeToCoins(decimal amount)
	{
		return decimal.ToInt64(decimal.Floor(amount * 100m));
	}
}
