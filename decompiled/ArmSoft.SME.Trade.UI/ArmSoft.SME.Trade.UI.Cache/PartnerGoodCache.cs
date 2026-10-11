using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class PartnerGoodCache : CacheDefinition<PartnerGoodCacheRow, PartnerGoodCacheRow.RowKey>
{
	public override short CacheId => CacheIdSE.PartnerGoods.Value;

	public IReadOnlyList<PartnerGoodCacheRow> GetPartnerGoodsList(string partnerCode)
	{
		int partnerId = TradeSettings.TradeCacheService.Partner.GetItem(partnerCode).Id;
		return GetList((PartnerGoodCacheRow p) => p.PartID == partnerId);
	}

	public PartnerGoodCacheRow GetItemFromMapping(int partId, string partItemCode)
	{
		return GetList().FirstOrDefault((PartnerGoodCacheRow p) => p.PartID == partId && p.PartItemCode == partItemCode);
	}
}
