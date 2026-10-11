using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class MTCachedFavoritesDataProvider : DataProvider<MTCachedFavoritesDataProvider.DataRow, MTCachedFavoritesDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int MTID { get; set; } = -1;

		public string MTCode { get; set; } = "";

		public string GroupCode { get; set; } = "";

		public string GroupCaption { get; set; } = "";

		public string Caption { get; set; } = "";

		public string UnitCode { get; set; } = "";

		public string UnitBrief { get; set; } = "";

		public bool HasUnit { get; set; }

		public decimal Price { get; set; }

		public bool HasPrice { get; set; }

		public bool HasImage { get; set; }

		public bool IsGroup { get; set; }

		public string HotKey { get; set; } = "";

		public bool HasHotKey { get; set; }

		public int UserID { get; set; } = -1;

		public int OrderNum { get; set; }

		public byte FavType { get; set; } = 1;

		public string ImgStr { get; set; } = "";
	}

	public class Param
	{
		public bool OnlyMaterials { get; set; } = true;

		public bool OnlyGroups { get; set; }

		public string GroupCode { get; set; } = "";

		public int UserId { get; set; }

		public string PriceType { get; set; } = "";

		public DateOnly Date { get; set; }

		public string VatType { get; set; }

		public string CurrCode { get; set; }
	}

	public override string Name => "MTFavorites";

	public MTCachedFavoritesDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.MTFavorites
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "OrderNum",
				Caption = ArmSoft.SME.Common.Resources.RowNumber,
				Source = "OrderNum"
			},
			new Column
			{
				Name = "MTID",
				Caption = "MTID",
				Source = "MTID"
			},
			new Column
			{
				Name = "MTCode",
				Caption = ArmSoft.SME.Common.Resources.Code,
				Source = "MTCode"
			},
			new Column
			{
				Name = "Caption",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "UnitCode",
				Caption = ArmSoft.SME.Common.Resources.Unit,
				Source = "UnitCode"
			},
			new Column
			{
				Name = "UnitBrief",
				Caption = ArmSoft.SME.Common.Resources.Brief,
				Source = "UnitBrief"
			},
			new Column
			{
				Name = "HasUnit",
				Caption = "HasUnit",
				Source = "HasUnit"
			},
			new Column
			{
				Name = "Price",
				Caption = ArmSoft.SME.Common.Resources.Price,
				Source = "Price"
			},
			new Column
			{
				Name = "HasPrice",
				Caption = "HasPrice",
				Source = "HasPrice"
			},
			new Column
			{
				Name = "HasImage",
				Caption = "HasImage",
				Source = "HasImage"
			},
			new Column
			{
				Name = "HotKey",
				Caption = ArmSoft.SME.Common.Resources.HotKey,
				Source = "HotKey"
			},
			new Column
			{
				Name = "IsGroup",
				Caption = "IsGroup",
				Source = "IsGroup"
			},
			new Column
			{
				Name = "GroupCode",
				Caption = ArmSoft.SME.Common.Resources.Code,
				Source = "GroupCode"
			},
			new Column
			{
				Name = "GroupCaption",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "GroupCaption"
			},
			new Column
			{
				Name = "HasHotKey",
				Caption = "HasHotKey",
				Source = "HasHotKey"
			},
			new Column
			{
				Name = "UserID",
				Caption = "UserID",
				Source = "UserID"
			},
			new Column
			{
				Name = "FavType",
				Caption = "FavType",
				Source = "FavType"
			},
			new Column
			{
				Name = "ImgStr",
				Caption = "ImgStr",
				Source = "ImgStr"
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override Task<Schema> GetSchemaAsync(Param param, ExecutionSettings settings, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		if (settings != null && settings.ReloadData)
		{
			TradeSettings.TradeCacheService.MTFavorite.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		IEnumerable<MTFavoriteCacheRow> enumerable = TradeSettings.TradeCacheService.MTFavorite.GetList();
		if (enumerable != null && enumerable.Count() > 0)
		{
			enumerable = enumerable.OrderBy((MTFavoriteCacheRow mf) => mf.OrderNum);
		}
		if (param.OnlyMaterials)
		{
			foreach (MTFavoriteCacheRow item in enumerable)
			{
				MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(item.Id);
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(itemById, item.UnitCode);
				if (param.GroupCode == item.Group)
				{
					dataProviderResult.Rows.Add(new DataRow
					{
						MTID = item.Id,
						MTCode = itemById.MTCode,
						GroupCode = item.Group,
						GroupCaption = itemById.GroupRow.Caption,
						Caption = itemById.Caption,
						UnitCode = item.UnitCode,
						UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(qntUnit.QntUnit).Brief,
						HasUnit = true,
						Price = PriceHelper.GetPriceForGivenPriceListAndVATTypeFromCache(param.PriceType, param.VatType, itemById.MTCode, item.UnitCode),
						HasPrice = true,
						HasImage = itemById.ImageExists,
						IsGroup = false,
						HotKey = item.HotKey,
						HasHotKey = !string.IsNullOrWhiteSpace(item.HotKey),
						UserID = ArmSoft.SME.Core.UI.Settings.LoginService.UserId,
						OrderNum = item.OrderNum,
						FavType = 1,
						ImgStr = $"{item.Id},{item.UnitCode}"
					});
				}
			}
		}
		else
		{
			foreach (MTFavoriteGroupCacheRow item2 in TradeSettings.TradeCacheService.MTFavoriteGroup.GetList())
			{
				dataProviderResult.Rows.Add(new DataRow
				{
					MTID = item2.Id,
					MTCode = item2.Code,
					GroupCode = item2.Code,
					GroupCaption = item2.Name,
					Caption = item2.Name,
					UnitCode = "",
					UnitBrief = "",
					HasUnit = false,
					Price = 0m,
					HasPrice = false,
					HasImage = item2.ImageExists,
					IsGroup = true,
					HotKey = item2.HotKey,
					HasHotKey = !string.IsNullOrWhiteSpace(item2.HotKey),
					UserID = ArmSoft.SME.Core.UI.Settings.LoginService.UserId,
					OrderNum = item2.OrderNum,
					FavType = 2,
					ImgStr = $"{item2.Id},"
				});
			}
			foreach (MTFavoriteCacheRow item3 in enumerable)
			{
				if (string.IsNullOrWhiteSpace(item3.Group))
				{
					MaterialCacheRow itemById2 = TradeSettings.TradeCacheService.Material.GetItemById(item3.Id);
					MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(itemById2.MTCode, item3.UnitCode);
					dataProviderResult.Rows.Add(new DataRow
					{
						MTID = item3.Id,
						MTCode = itemById2.MTCode,
						GroupCode = item3.Group,
						GroupCaption = itemById2.GroupRow.Caption,
						Caption = itemById2.Caption,
						UnitCode = item3.UnitCode,
						UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(qntUnit2.QntUnit).Brief,
						HasUnit = true,
						Price = PriceHelper.GetPriceForGivenPriceListAndVATTypeFromCache(param.PriceType, param.VatType, itemById2.MTCode, item3.UnitCode),
						HasPrice = true,
						HasImage = itemById2.ImageExists,
						IsGroup = false,
						HotKey = item3.HotKey,
						HasHotKey = !string.IsNullOrWhiteSpace(item3.HotKey),
						UserID = ArmSoft.SME.Core.UI.Settings.LoginService.UserId,
						OrderNum = item3.OrderNum,
						FavType = 1,
						ImgStr = $"{item3.Id},{item3.UnitCode}"
					});
				}
			}
		}
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
