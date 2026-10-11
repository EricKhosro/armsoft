using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.SmartEnum;
using ArmSoft.SME.Accountant.UI.Enums;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.SmartEnums;

namespace ArmSoft.SME.Core.UI.HelpClasses;

public class SettlementAccountHelper
{
	public static string GetSettlementAccountKey(string receiverSettlementAcc)
	{
		int num = 0;
		string text = receiverSettlementAcc.Left(5);
		string value = receiverSettlementAcc.Mid(6);
		if (string.IsNullOrWhiteSpace(value))
		{
			return "";
		}
		value = value.LeftAlign(11);
		string text2 = text.Left(5) + value.Left(6);
		text2 = text2.Replace(" ", "0");
		for (int i = 1; i <= 11; i++)
		{
			if (!int.TryParse(text2.Mid(i, 1), out var result))
			{
				result = 1;
			}
			result *= 1 + i % 2;
			num += ((result > 9) ? (1 + result - 10) : result);
			num = ((num > 10) ? (num - 10) : num);
		}
		return text + value.Left(6) + Convert.ToString(10 - num) + value.Right(4);
	}

	public static bool Is_HSBC_Account(string settlementAccount)
	{
		if (settlementAccount.Length < 3)
		{
			return false;
		}
		return settlementAccount.Left(3) == ((short)217).ToString();
	}

	public static bool Is_Budget_Account(string settlementAccount)
	{
		if (settlementAccount.Length < 1)
		{
			return false;
		}
		return settlementAccount.Left(1) == ((short)9).ToString();
	}

	public static string GetEating6And7SSymbolsHSBCAccount(string settlementAccount)
	{
		if (settlementAccount.Length < 8)
		{
			return settlementAccount;
		}
		return settlementAccount.Left(5) + settlementAccount.Mid(8);
	}

	public static string GetBankMainCode(string settlementAccount)
	{
		if (settlementAccount.Length < 3)
		{
			return settlementAccount;
		}
		return settlementAccount.Left(3);
	}

	public static void CheckBelongToSameBank(List<string> accounts)
	{
		if (accounts.GroupBy(GetBankMainCode).Count() != 1)
		{
			throw new ASException(Resources.BelongToSameBankAccount);
		}
	}

	public static SettlementAccountType GetAccountType(string settlementAccount, string bankingMode = "")
	{
		if (!string.IsNullOrWhiteSpace(settlementAccount))
		{
			bankingMode = bankingMode.Trim();
			string bankMainCode = GetBankMainCode(settlementAccount);
			if (!SmartEnum<IBBankSE, string>.TryFromValue(bankMainCode, out var _))
			{
				string bankCode;
				throw new ASException(string.Format(Resources.IntegrationWithBankIsNotAvailable, Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(bankMainCode + "00", out bankCode)));
			}
			if (string.IsNullOrWhiteSpace(bankingMode))
			{
				bankingMode = ((short)1).ToString();
			}
		}
		if (string.IsNullOrWhiteSpace(bankingMode))
		{
			return SettlementAccountType.None;
		}
		if (bankingMode == BankingModesSE.InternetBank.Value)
		{
			return SettlementAccountType.InternetBank;
		}
		return SettlementAccountType.None;
	}
}
