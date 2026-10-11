using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class MTFavoriteCache : CacheDefinition<MTFavoriteCacheRow, MTFavoriteCacheRow.RowKey>
{
	public override short CacheId => CacheIdSE.MTFavorites.Value;

	public override bool StoreInLocalStorage => false;

	public override bool StoreInUserBasedLocalStorage => Settings.OfflineModeIsAvailable;
}
