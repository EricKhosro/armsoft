using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.Models;

public class FavoriteItem
{
	public int MTID { get; }

	public string MTCode { get; set; }

	public string GroupCode { get; }

	public string GroupCaption { get; }

	public string Caption { get; set; }

	public string UnitCode { get; }

	public string UnitBrief { get; set; }

	public bool HasUnit { get; }

	public decimal Price { get; set; }

	public bool HasPrice { get; }

	public bool HasImage { get; }

	public bool IsGroup { get; }

	public string HotKey { get; }

	public bool HasHotKey { get; }

	public int UserID { get; }

	public int OrderNum { get; }

	public byte FavType { get; }

	public string ImgStr { get; }

	public FavoriteItem(MTFavoriteGroupCacheRow favoriteGroupsCacheRow)
	{
		MTID = favoriteGroupsCacheRow.Id;
		MTCode = favoriteGroupsCacheRow.Code;
		GroupCode = favoriteGroupsCacheRow.Code;
		GroupCaption = favoriteGroupsCacheRow.Name;
		Caption = favoriteGroupsCacheRow.Name;
		UnitCode = "";
		UnitBrief = "";
		HasUnit = false;
		Price = 0m.RoundAs(2);
		HasPrice = false;
		HasImage = favoriteGroupsCacheRow.ImageExists;
		IsGroup = true;
		HotKey = favoriteGroupsCacheRow.HotKey;
		HasHotKey = !string.IsNullOrWhiteSpace(favoriteGroupsCacheRow.HotKey);
		UserID = Settings.LoginService.UserId;
		OrderNum = favoriteGroupsCacheRow.OrderNum;
		FavType = 2;
		ImgStr = $"{MTID},";
	}

	public FavoriteItem(MTFavoriteCacheRow favoriteMaterialCacheRow, string priceListType, string vATType)
	{
		MTID = favoriteMaterialCacheRow.Id;
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(MTID);
		MTCode = itemById.MTCode;
		GroupCode = favoriteMaterialCacheRow.Group;
		GroupCaption = itemById.GroupRow.Caption;
		Caption = itemById.Caption;
		UnitCode = favoriteMaterialCacheRow.UnitCode;
		MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(MTCode, UnitCode);
		UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(qntUnit.QntUnit).Brief;
		HasUnit = true;
		CurrentPricesCacheRow item = TradeSettings.TradeCacheService.CurrentPrices.GetItem(new CurrentPricesCacheRow.RowKey
		{
			MTID = MTID,
			PriceType = priceListType,
			Unit = UnitCode
		});
		if (item != null)
		{
			Price = item.Price.RoundAs(2);
		}
		HasPrice = true;
		HasImage = itemById.ImageExists;
		IsGroup = false;
		HotKey = favoriteMaterialCacheRow.HotKey;
		HasHotKey = !string.IsNullOrWhiteSpace(favoriteMaterialCacheRow.HotKey);
		UserID = Settings.LoginService.UserId;
		OrderNum = favoriteMaterialCacheRow.OrderNum;
		FavType = 1;
		ImgStr = $"{MTID},{UnitCode}";
	}

	public FavoriteItem(MTFavoritesDataProvider.DataRow favoriteItemDataProviderRow)
	{
		MTID = favoriteItemDataProviderRow.MTID;
		MTCode = favoriteItemDataProviderRow.MTCode;
		GroupCode = favoriteItemDataProviderRow.GroupCode;
		GroupCaption = favoriteItemDataProviderRow.GroupCaption;
		Caption = favoriteItemDataProviderRow.Caption;
		UnitCode = favoriteItemDataProviderRow.UnitCode;
		UnitBrief = favoriteItemDataProviderRow.UnitBrief;
		HasUnit = favoriteItemDataProviderRow.HasUnit;
		Price = favoriteItemDataProviderRow.Price.RoundAs(2);
		HasPrice = favoriteItemDataProviderRow.HasPrice;
		HasImage = favoriteItemDataProviderRow.HasImage;
		IsGroup = favoriteItemDataProviderRow.IsGroup;
		HotKey = favoriteItemDataProviderRow.HotKey;
		HasHotKey = favoriteItemDataProviderRow.HasHotKey;
		UserID = favoriteItemDataProviderRow.UserID;
		OrderNum = favoriteItemDataProviderRow.OrderNum;
		FavType = favoriteItemDataProviderRow.FavType;
		ImgStr = favoriteItemDataProviderRow.ImgStr;
	}
}
