using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.Caches;

public class BankCache : CacheDefinition<BanksCacheRow, string>
{
	public override short CacheId => CacheIdSE.Banks.Value;

	public override bool StoreInLocalStorage => true;

	public string GetBankCaptionBySettlementAccount(string settlementAccount, out string bankCode)
	{
		string result = "";
		bankCode = "";
		if (!string.IsNullOrWhiteSpace(settlementAccount))
		{
			string key = settlementAccount.LeftAlign(5);
			BanksCacheRow item = GetItem(key);
			if (item != null)
			{
				result = item.Name;
				bankCode = item.Code;
			}
		}
		return result;
	}
}
