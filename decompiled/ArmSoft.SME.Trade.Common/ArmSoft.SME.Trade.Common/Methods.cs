using System;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Trade.Common;

public class Methods
{
	public static decimal RoundPrice(decimal price, string roundDegree, bool roundToUpper = true)
	{
		if (price == 0m || string.IsNullOrWhiteSpace(roundDegree))
		{
			return price;
		}
		if (Convert.ToDecimal(roundDegree) == 0m)
		{
			throw new RESTException(string.Format(Resources.ErrFieldIsGtZero, Resources.PriceRoundDegree.ToLower()));
		}
		decimal result = 0m;
		if (roundToUpper)
		{
			decimal num = price / Convert.ToDecimal(roundDegree);
			decimal num2 = decimal.Truncate(num);
			if (num - num2 != 0m)
			{
				return price + Math.Abs(Convert.ToDecimal(roundDegree) - (price - num2 * Convert.ToDecimal(roundDegree)));
			}
			return price;
		}
		switch (roundDegree)
		{
		case "0.01":
			return price.RoundAs(2);
		case "0.05":
		case "0.1":
			return price.RoundAs(1);
		case "0.5":
		case "1":
			return price.RoundAs(0);
		case "5":
		case "10":
			return price.RoundAs(-1);
		case "100":
		case "50":
			return price.RoundAs(-2);
		case "1000":
		case "500":
			return price.RoundAs(-3);
		default:
			return result;
		}
	}

	public static string VATAndRetailVATMapping(string vatType, bool oppositeMapping)
	{
		if (oppositeMapping)
		{
			switch (vatType)
			{
			case "1":
				return vatType;
			case "2":
			case "5":
				return "2";
			default:
				return vatType;
			}
		}
		switch (vatType)
		{
		case "1":
		case "2":
		case "5":
			return "5";
		default:
			return vatType;
		}
	}
}
