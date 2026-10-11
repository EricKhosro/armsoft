using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class CashierShift : CacheDefinition<CashierShiftCacheRow, CashierShiftCacheRow.RowKey>
{
	public override short CacheId => CacheIdSE.CashierShift.Value;

	public override bool StoreInLocalStorage => true;
}
