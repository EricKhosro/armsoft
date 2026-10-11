using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class CashCache : CacheDefinition<CashCacheRow, string>
{
	public override short CacheId => CacheIdSE.Cashes.Value;

	public override bool StoreInLocalStorage => true;

	public List<CashCacheRow.NonCashInfo> GetNonCashInfoListByCashCode(string code)
	{
		return GetList().FirstOrDefault((CashCacheRow x) => string.IsNullOrWhiteSpace(code) || x.Code == code)?.NonCashInfoList;
	}
}
