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

public class MTBarCodeDataProvider : DataProvider<MTBarCodeDataProvider.DataRow, MTBarCodeDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string BarCode { get; set; }

		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string MTCaption { get; set; }

		public string CPACode { get; set; }

		public string QntUnit { get; set; }

		public bool IsWeight { get; set; }

		public string QntUnitCaption { get; set; }

		public string QntUnitBrief { get; set; }

		public string ImgStr { get; set; }

		public bool MTShowInPriceList { get; set; }

		public byte[] TS { get; set; }
	}

	public class Param
	{
		public string MTCode { get; set; }

		public bool IsShowOnlyComplects { get; set; }

		public string Group { get; set; }

		public string ItemType { get; set; }

		public bool IsShowOnlyInclInPriceList { get; set; }

		public bool ShowAlsoMTQntUnitsEmptyBarCodes { get; set; }
	}

	public override string Name => "MTBarCode";

	public MTBarCodeDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.MTBarCode
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
				Name = "CPACode",
				Caption = ArmSoft.SME.Common.Resources.CPAClassification,
				Source = "CPACode"
			},
			new Column
			{
				Name = "MTShowInPriceList",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "MTShowInPriceList",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "QntUnit",
				Caption = ArmSoft.SME.Trade.Common.Resources.QntUnit,
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
				Name = "IsWeight",
				Caption = ArmSoft.SME.Trade.Common.Resources.WeightItem,
				Source = "IsWeight",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "BarCode",
				Caption = ArmSoft.SME.Common.Resources.BarCode,
				Source = "BarCode"
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
		HashSet<int> hashSet = new HashSet<int>();
		IEnumerable<MaterialCacheRow> materials;
		if (!string.IsNullOrWhiteSpace(param.MTCode))
		{
			materials = new _003C_003Ez__ReadOnlySingleElementList<MaterialCacheRow>(TradeSettings.TradeCacheService.Material.GetItem(param.MTCode));
		}
		else
		{
			materials = TradeSettings.TradeCacheService.Material.GetList();
		}
		if (param.IsShowOnlyComplects)
		{
			materials = materials.Where((MaterialCacheRow p) => p.IsComplect);
		}
		if (!string.IsNullOrWhiteSpace(param.Group))
		{
			IEnumerable<string> mtGroupCodes = from p in TradeSettings.TradeCacheService.MTGroup.GetList()
				where materials.Any((MaterialCacheRow a) => a.GroupCode == param.Group && a.BarCode == p.Code)
				select p.Code;
			materials = materials.Where((MaterialCacheRow p) => mtGroupCodes.Contains(p.MTCode));
		}
		if (!string.IsNullOrWhiteSpace(param.ItemType) && param.ItemType != ItemTypesSE.All)
		{
			materials = materials.Where((MaterialCacheRow p) => materials.Where((MaterialCacheRow x) => x.Type == param.ItemType).Any((MaterialCacheRow x) => x.MTCode == p.MTCode));
		}
		foreach (MaterialCacheRow item3 in materials)
		{
			if (param.IsShowOnlyInclInPriceList && (!param.IsShowOnlyInclInPriceList || !item3.ShowInPriceList))
			{
				continue;
			}
			if (item3.BarCodes.Count > 0)
			{
				foreach (MaterialCacheRow.BarCodeRow barCode in item3.BarCodes)
				{
					QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(barCode.QntUnit);
					DataRow item2 = new DataRow
					{
						BarCode = barCode.BarCode,
						QntUnit = barCode.QntUnit,
						QntUnitBrief = item.Brief,
						QntUnitCaption = item.Caption,
						MTID = item3.Id,
						MTCode = item3.MTCode,
						MTCaption = item3.Caption,
						CPACode = item3.CPACode,
						MTShowInPriceList = item3.ShowInPriceList,
						IsWeight = item3.IsWeight,
						ImgStr = item3.Id + "," + barCode.QntUnit.ToString(),
						TS = item3.TS
					};
					hashSet.Add(item3.Id);
					dataProviderResult.Rows.Add(item2);
				}
			}
			else
			{
				if (!param.ShowAlsoMTQntUnitsEmptyBarCodes)
				{
					continue;
				}
				foreach (MaterialCacheRow.QntUnitRow qntUnit in item3.QntUnits)
				{
					QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(qntUnit.QntUnit);
					DataRow item2 = new DataRow
					{
						MTID = item3.Id,
						MTCode = item3.MTCode,
						MTCaption = item3.Caption,
						MTShowInPriceList = item3.ShowInPriceList,
						CPACode = item3.CPACode,
						IsWeight = item3.IsWeight,
						QntUnit = qntUnit.QntUnit,
						QntUnitBrief = item.Brief,
						QntUnitCaption = item.Caption,
						ImgStr = item3.Id + "," + qntUnit.QntUnit.ToString()
					};
					dataProviderResult.Rows.Add(item2);
				}
			}
		}
		dataProviderResult.Rows = (from x in dataProviderResult.Rows
			orderby !string.IsNullOrWhiteSpace(x.BarCode) descending, x.MTCode, x.QntUnit
			select x).ToList();
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
