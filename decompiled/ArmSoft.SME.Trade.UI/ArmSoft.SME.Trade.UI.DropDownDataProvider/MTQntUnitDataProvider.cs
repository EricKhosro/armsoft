using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class MTQntUnitDataProvider : DataProvider<MTQntUnitDataProvider.DataRow, MTQntUnitDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public class Key
		{
			public int MTID { get; set; }

			public string QntUnit { get; set; }
		}

		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string MTCaption { get; set; }

		public bool MTShowInPriceList { get; set; }

		public string QntUnit { get; set; }

		public string QntUnitCaption { get; set; }

		public string QntUnitBrief { get; set; }

		public decimal Coef { get; set; }

		public bool IsDefault { get; set; }

		public byte[] Image { get; set; }

		public bool ImageExists { get; set; }

		public string ImgStr { get; set; }
	}

	public class Param
	{
		public string MTCode { get; set; }

		public string GroupCode { get; set; }

		public bool IsShowOnlyComplects { get; set; }

		public string ItemType { get; set; }
	}

	public override string Name => "MTQntUnit";

	public MTQntUnitDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.MTQntUnits
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "MTCode",
				Caption = ArmSoft.SME.Trade.Common.Resources.MaterialOrService,
				Source = "MTCode"
			},
			new Column
			{
				Name = "MTCaption",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "MTCaption"
			},
			new Column
			{
				Name = "QntUnit",
				Caption = ArmSoft.SME.Common.Resources.Code,
				Source = "QntUnit"
			},
			new Column
			{
				Name = "QntUnitBrief",
				Caption = ArmSoft.SME.Common.Resources.Brief,
				Source = "QntUnitBrief"
			},
			new Column
			{
				Name = "Coef",
				Caption = ArmSoft.SME.Common.Resources.Coefficient,
				Source = "Coef",
				Type = SupportedType.Decimal
			},
			new Column
			{
				Name = "IsDefault",
				Caption = ArmSoft.SME.Common.Resources.Default,
				Source = "IsDefault",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "ImageExists",
				Caption = ArmSoft.SME.Common.Resources.Image,
				Source = "ImageExists",
				Type = SupportedType.Boolean
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
			TradeSettings.TradeCacheService.Material.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		IEnumerable<MaterialCacheRow> enumerable;
		if (string.IsNullOrWhiteSpace(param.MTCode))
		{
			enumerable = (string.IsNullOrWhiteSpace(param.GroupCode) ? TradeSettings.TradeCacheService.Material.GetList() : TradeSettings.TradeCacheService.Material.GetItems(param.GroupCode));
		}
		else
		{
			enumerable = new _003C_003Ez__ReadOnlySingleElementList<MaterialCacheRow>(TradeSettings.TradeCacheService.Material.GetItem(param.MTCode));
		}
		if (param.IsShowOnlyComplects)
		{
			enumerable = enumerable.Where((MaterialCacheRow p) => p.IsComplect);
		}
		if (!string.IsNullOrWhiteSpace(param.ItemType) && param.ItemType != ItemTypesSE.All)
		{
			enumerable = enumerable.Where((MaterialCacheRow x) => x.Type == param.ItemType);
		}
		foreach (MaterialCacheRow item2 in enumerable)
		{
			if (item2 == null)
			{
				continue;
			}
			foreach (MaterialCacheRow.QntUnitRow qntUnit in item2.QntUnits)
			{
				DataRow dataRow = new DataRow
				{
					MTID = item2.Id,
					MTCode = item2.MTCode,
					MTCaption = item2.Caption,
					QntUnit = qntUnit.QntUnit,
					Coef = qntUnit.Coef,
					IsDefault = qntUnit.IsDefault,
					ImageExists = qntUnit.ImageGUID.HasValue,
					MTShowInPriceList = item2.ShowInPriceList,
					ImgStr = item2.Id + "," + qntUnit.QntUnit.ToString()
				};
				QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(qntUnit.QntUnit);
				if (item != null)
				{
					dataRow.QntUnitBrief = item?.Brief;
					dataRow.QntUnitCaption = item?.Caption;
				}
				dataProviderResult.Rows.Add(dataRow);
			}
		}
		dataProviderResult.Rows = dataProviderResult.Rows.OrderBy((DataRow x) => x.QntUnit).ToList();
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
