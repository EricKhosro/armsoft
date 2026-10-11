using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Utility;

public static class DocumentUtil
{
	public static void CalculateDebitCreditAmounts(ref decimal amountDebit, ref decimal amountCredit, RecalculatingCurrency recalculatingCurrency, string currencyDebit, string currencyCredit, string rateCurrency1, short rateFor, decimal rateValue)
	{
		if (string.IsNullOrEmpty(currencyDebit) || string.IsNullOrEmpty(currencyCredit) || string.Equals(currencyDebit, currencyCredit) || !(rateValue > 0m) || rateFor <= 0)
		{
			return;
		}
		int decimals = ((currencyDebit == "AMD") ? 1 : 2);
		int decimals2 = ((currencyCredit == "AMD") ? 1 : 2);
		if (recalculatingCurrency == RecalculatingCurrency.CurrencyPurchase)
		{
			amountDebit = decimal.Round(amountDebit, decimals);
			if (rateCurrency1 == currencyDebit)
			{
				amountCredit = decimal.Round(amountDebit * (decimal)rateFor / rateValue, decimals2);
			}
			else
			{
				amountCredit = decimal.Round(amountDebit * rateValue / (decimal)rateFor, decimals2);
			}
		}
		else
		{
			amountCredit = decimal.Round(amountCredit, decimals2);
			if (rateCurrency1 == currencyDebit)
			{
				amountDebit = decimal.Round(amountCredit * rateValue / (decimal)rateFor, decimals);
			}
			else
			{
				amountDebit = decimal.Round(amountCredit * (decimal)rateFor / rateValue, decimals);
			}
		}
	}

	public static decimal CalculateCreditAmount(decimal amountDebit, string currencyDebit, string currencyCredit, string rateCurrency1, short rateFor, decimal rateValue)
	{
		if (string.IsNullOrEmpty(currencyDebit) || string.IsNullOrEmpty(currencyCredit) || currencyDebit == currencyCredit)
		{
			return amountDebit;
		}
		decimal amountCredit = 0m;
		if (rateValue > 0m && rateFor > 0)
		{
			CalculateDebitCreditAmounts(ref amountDebit, ref amountCredit, RecalculatingCurrency.CurrencyPurchase, currencyDebit, currencyCredit, rateCurrency1, rateFor, rateValue);
		}
		return amountCredit;
	}

	public static decimal CalculateDebitAmount(decimal amountCredit, string currencyDebit, string currencyCredit, string rateCurrency1, short rateFor, decimal rateValue)
	{
		if (string.IsNullOrEmpty(currencyDebit) || string.IsNullOrEmpty(currencyCredit) || currencyDebit == currencyCredit)
		{
			return amountCredit;
		}
		decimal amountDebit = 0m;
		if (rateValue > 0m && rateFor > 0)
		{
			CalculateDebitCreditAmounts(ref amountDebit, ref amountCredit, RecalculatingCurrency.CurrencySell, currencyDebit, currencyCredit, rateCurrency1, rateFor, rateValue);
		}
		return amountDebit;
	}

	public static decimal CalculateAmountByCrossRate(decimal sourceAmount, decimal sourceRateValue, short sourceRateFor, decimal targetRateValue, short targetRateFor)
	{
		if (sourceRateFor == 0 || targetRateFor == 0 || sourceRateValue == 0m || targetRateValue == 0m)
		{
			throw new ArgumentException("Invalid exchange rate values");
		}
		return decimal.Round(sourceAmount * sourceRateValue / (decimal)sourceRateFor / targetRateValue * (decimal)targetRateFor, 2);
	}
}
