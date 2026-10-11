using System;

namespace ArmSoft.SME.Core.AddIn;

public static class DecimalExtension
{
	public static decimal RoundAs(this decimal number, int precision)
	{
		if (precision < 0)
		{
			decimal d = Convert.ToDecimal(Math.Pow(10.0, precision));
			return decimal.Divide(decimal.Round(decimal.Multiply(number, d), 0, MidpointRounding.AwayFromZero), d);
		}
		return decimal.Round(number, precision, MidpointRounding.AwayFromZero);
	}
}
