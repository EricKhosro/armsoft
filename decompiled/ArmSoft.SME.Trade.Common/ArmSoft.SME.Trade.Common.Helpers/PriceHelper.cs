using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Trade.Common.Helpers;

public static class PriceHelper
{
	public static decimal CalcSumDependOnVATType(decimal sum, string vatType, string rowVATType, bool itemVAT, decimal constVAT, decimal vatIncluded)
	{
		decimal result = 0m;
		switch (rowVATType)
		{
		case "1":
			if (vatType.InList("1", "3", "4"))
			{
				return sum;
			}
			return sum + CalcVATAmountByType(sum, "1", itemVAT, constVAT, vatIncluded);
		case "2":
			if (vatType.InList("1", "3", "4"))
			{
				return sum - CalcVATAmountByType(sum, "2", itemVAT, constVAT, vatIncluded);
			}
			return sum;
		case "5":
			if (vatType.InList("1", "3", "4"))
			{
				return sum - CalcVATAmountByType(sum, "5", itemVAT, constVAT, vatIncluded);
			}
			return sum;
		default:
			return result;
		}
	}

	public static decimal CalcVATAmountByType(decimal sum, string vatType, bool itemVAT, decimal constVAT, decimal vatIncluded)
	{
		if (!itemVAT)
		{
			return 0m;
		}
		return vatType switch
		{
			"1" => (sum * constVAT / 100m).RoundAs(2), 
			"2" => (sum * constVAT / (100m + constVAT)).RoundAs(2), 
			"5" => (sum * vatIncluded / 100m).RoundAs(2), 
			_ => 0m, 
		};
	}

	public static decimal GetSumCurr(decimal sumNNC, ExchangeRate exchangeRate, bool roundResult = true, int roundPrecision = 2)
	{
		if (exchangeRate.Rate == 0m || exchangeRate.Base == 0m)
		{
			return 0m;
		}
		decimal num = sumNNC * exchangeRate.Base / exchangeRate.Rate;
		if (roundResult)
		{
			num = num.RoundAs(roundPrecision);
		}
		return num;
	}

	public static decimal GetSumNNC(decimal sumCurr, ExchangeRate exchangeRate, bool roundResult = true, int roundPrecision = 2)
	{
		if (exchangeRate.Rate == 0m || exchangeRate.Base == 0m)
		{
			return 0m;
		}
		decimal num = sumCurr * exchangeRate.Rate / exchangeRate.Base;
		if (roundResult)
		{
			num = num.RoundAs(roundPrecision);
		}
		return num;
	}

	public static void CalcNNCAndCurWithoutVATSumAndVATSum(decimal sum, string vatType, bool itemVAT, decimal constVAT, decimal vatIncluded, string currCode, ExchangeRate exchangeRate, string optionCurrCode, ExchangeRate optionExchangeRate, out decimal sumWithoutVATNCC, out decimal sumWithoutVATCurr, out decimal vatSumNCC, out decimal vatSumCurr)
	{
		sumWithoutVATNCC = 0m;
		sumWithoutVATCurr = 0m;
		vatSumNCC = 0m;
		vatSumCurr = 0m;
		if (currCode == "AMD")
		{
			vatSumNCC = CalcVATAmountByType(sum, vatType, itemVAT, constVAT, vatIncluded);
			vatSumCurr = GetSumCurr(vatSumNCC, optionExchangeRate);
			if (vatType != "2")
			{
				sumWithoutVATNCC = sum;
				sumWithoutVATCurr = GetSumCurr(sum, optionExchangeRate) + vatSumCurr;
			}
			else
			{
				sumWithoutVATNCC = sum - vatSumNCC;
				sumWithoutVATCurr = GetSumCurr(sum, optionExchangeRate);
			}
		}
		else if (currCode == optionCurrCode)
		{
			vatSumCurr = CalcVATAmountByType(sum, vatType, itemVAT, constVAT, vatIncluded);
			vatSumNCC = GetSumNNC(vatSumCurr, exchangeRate);
			if (vatType != "2")
			{
				sumWithoutVATNCC = GetSumNNC(sum, exchangeRate);
				sumWithoutVATCurr = sum + vatSumCurr;
			}
			else
			{
				sumWithoutVATNCC = GetSumNNC(sum, exchangeRate) - vatSumNCC;
				sumWithoutVATCurr = sum;
			}
		}
		else
		{
			vatSumNCC = GetSumNNC(CalcVATAmountByType(sum, vatType, itemVAT, constVAT, vatIncluded), exchangeRate);
			vatSumCurr = GetSumCurr(vatSumNCC, optionExchangeRate);
			if (vatType != "2")
			{
				sumWithoutVATNCC = GetSumNNC(sum, exchangeRate);
				sumWithoutVATCurr = (sum * exchangeRate.Rate / exchangeRate.Base / optionExchangeRate.Rate * optionExchangeRate.Base + vatSumCurr).RoundAs(2);
			}
			else
			{
				sumWithoutVATNCC = GetSumNNC(sum, exchangeRate) - vatSumNCC;
				sumWithoutVATCurr = (sum * exchangeRate.Rate / exchangeRate.Base / optionExchangeRate.Rate * optionExchangeRate.Base).RoundAs(2);
			}
		}
	}

	public static decimal GetEnvironmentalFeeAmount(decimal saleSumma, decimal envFeePercent, bool vat, string vatType, decimal constVAT, decimal VATIncluded)
	{
		if (saleSumma != 0m && envFeePercent != 0m)
		{
			decimal num = saleSumma;
			if (vat && vatType == "2")
			{
				num -= CalcVATAmountByType(saleSumma, vatType, vat, constVAT, VATIncluded);
			}
			return num - (num / (1m + envFeePercent / 100m)).RoundAs(2);
		}
		return 0m;
	}
}
