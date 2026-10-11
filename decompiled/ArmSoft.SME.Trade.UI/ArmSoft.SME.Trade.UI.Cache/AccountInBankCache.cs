using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.SmartEnums;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class AccountInBankCache : CacheDefinition<AccountInBankCacheRow, string>
{
	public override short CacheId => ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.AccountInBank.Value;

	public override bool StoreInLocalStorage => true;

	public override IReadOnlyList<AccountInBankCacheRow> GetList(Func<AccountInBankCacheRow, bool> predicate = null)
	{
		IReadOnlyList<AccountInBankCacheRow> list = base.GetList(predicate);
		foreach (AccountInBankCacheRow item in list)
		{
			if (string.IsNullOrWhiteSpace(item.BankingModeCode))
			{
				item.BankingModeCode = (SmartEnum<IBBankSE, string>.TryFromValue(SettlementAccountHelper.GetBankMainCode(item.SettlAccount), out var _) ? BankingModesSE.InternetBank.Value : "");
			}
		}
		return list;
	}

	public AccountInBankCacheRow GetAccountInBankBySettlementAccount(string settlementAccount)
	{
		return GetList().FirstOrDefault((AccountInBankCacheRow x) => x.SettlAccount == settlementAccount);
	}

	public AccountInBankCacheRow GetAccountInBankByCode(string code)
	{
		return GetList().FirstOrDefault((AccountInBankCacheRow x) => x.Code == code);
	}

	public AccountInBankCacheRow GetDefaultSettlementAccount()
	{
		return GetList().FirstOrDefault((AccountInBankCacheRow x) => x.IsDefault);
	}

	public string GetDefaultSettlementAccount(bool isSettlAccount)
	{
		AccountInBankCacheRow accountInBankCacheRow = GetList().FirstOrDefault((AccountInBankCacheRow x) => x.IsDefault);
		if (accountInBankCacheRow != null)
		{
			if (!isSettlAccount)
			{
				return accountInBankCacheRow.Code;
			}
			return accountInBankCacheRow.SettlAccount;
		}
		return string.Empty;
	}
}
