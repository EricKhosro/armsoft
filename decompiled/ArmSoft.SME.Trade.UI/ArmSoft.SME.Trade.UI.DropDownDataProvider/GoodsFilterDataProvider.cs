using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Client.Data;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class GoodsFilterDataProvider : DataProvider<GoodsFilterDataProvider.DataRow, GoodsFilterDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedFilterKey
	{
		public string TypeName { get; set; }

		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string Caption { get; set; }

		public string CPACode { get; set; }

		public string Group { get; set; }

		public string GroupPath { get; set; }

		public string GroupCaption { get; set; }

		public string Unit { get; set; }

		public string UnitBrief { get; set; }

		public string AltUnit { get; set; }

		public string AltUnitBrief { get; set; }

		public string BarCode { get; set; }

		public string Description { get; set; }

		public string Country { get; set; }

		public string ExpenseMethodName { get; set; }

		public bool VAT { get; set; }

		public bool ShowInPriceList { get; set; }

		public string FullCaption { get; set; }

		public bool IsWeight { get; set; }

		public bool CheckMarking { get; set; }

		public object FilterKey => MTCode;

		public bool FilterMatched(object key, object path)
		{
			if (((string)key).Trim() == Group.Trim())
			{
				return true;
			}
			return GroupPath.StartsWith(((string)path).Trim() + "!");
		}
	}

	public class Param
	{
		public string ItemType { get; set; } = "";

		public string Group { get; set; } = "";

		public bool IsShowOnlyInPriceList { get; set; }

		public string LikeName { get; set; } = "";

		public string PartnerCode { get; set; } = "";
	}

	public override string Name => "GoodsFilter";

	public GoodsFilterDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.MaterialsAndServices
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "TypeName",
				Caption = ArmSoft.SME.Common.Resources.TypeWord,
				Source = "TypeName"
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
				Name = "CPACode",
				Caption = ArmSoft.SME.Common.Resources.CPAClassification,
				Source = "CPACode"
			},
			new Column
			{
				Name = "Group",
				Caption = ArmSoft.SME.Common.Resources.Group,
				Source = "Group"
			},
			new Column
			{
				Name = "GroupCaption",
				Caption = ArmSoft.SME.Common.Resources.GroupName,
				Source = "GroupCaption"
			},
			new Column
			{
				Name = "UnitBrief",
				Caption = ArmSoft.SME.Trade.Common.Resources.QntUnit,
				Source = "UnitBrief"
			},
			new Column
			{
				Name = "AltUnitBrief",
				Caption = ArmSoft.SME.Trade.Common.Resources.ALTQUNIT,
				Source = "AltUnitBrief"
			},
			new Column
			{
				Name = "BarCode",
				Caption = ArmSoft.SME.Common.Resources.BarCode,
				Source = "BarCode"
			},
			new Column
			{
				Name = "Description",
				Caption = ArmSoft.SME.Common.Resources.Specification,
				Source = "Description"
			},
			new Column
			{
				Name = "Country",
				Caption = ArmSoft.SME.Common.Resources.Country,
				Source = "Country"
			},
			new Column
			{
				Name = "ExpenseMethodName",
				Caption = ArmSoft.SME.Common.Resources.MTExpenseMethod,
				Source = "ExpenseMethodName"
			},
			new Column
			{
				Name = "VAT",
				Caption = ArmSoft.SME.Common.Resources.OptionsVAT,
				Source = "VAT",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "ShowInPriceList",
				Caption = ArmSoft.SME.Trade.Common.Resources.ShowInPriceListes,
				Source = "ShowInPriceList",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "FullCaption",
				Caption = ArmSoft.SME.Common.Resources.FullCaption,
				Source = "FullCaption"
			},
			new Column
			{
				Name = "IsWeight",
				Caption = ArmSoft.SME.Common.Resources.IsWeight,
				Source = "IsWeight",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "CheckMarking",
				Caption = ArmSoft.SME.Trade.Common.Resources.CheckMarking,
				Source = "CheckMarking",
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
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		if (string.IsNullOrWhiteSpace(param.PartnerCode))
		{
			if (settings != null && settings.ReloadData)
			{
				TradeSettings.TradeCacheService.Material.Refresh();
			}
			IEnumerable<MaterialCacheRow> enumerable = TradeSettings.TradeCacheService.Material.GetList();
			if (!string.IsNullOrWhiteSpace(param.ItemType) && param.ItemType != "0")
			{
				enumerable = enumerable.Where((MaterialCacheRow p) => param.ItemType == p.Type);
			}
			if (!string.IsNullOrWhiteSpace(param.Group))
			{
				MTGroupCacheRow groupRow = TradeSettings.TradeCacheService.MTGroup.GetItem(param.Group);
				enumerable = enumerable.Where((MaterialCacheRow p) => param.Group == p.GroupCode || p.GroupRow.Path.StartsWith(groupRow.Path.Trim() + "!"));
			}
			if (param.IsShowOnlyInPriceList && ArmSoft.SME.Core.UI.Settings.ParametersService.ShowMTInclInPrList())
			{
				enumerable = enumerable.Where((MaterialCacheRow p) => param.IsShowOnlyInPriceList = p.ShowInPriceList);
			}
			if (!string.IsNullOrWhiteSpace(param.LikeName))
			{
				enumerable = enumerable.Where((MaterialCacheRow p) => p.Caption.Contains(param.LikeName.Trim(), StringComparison.CurrentCultureIgnoreCase));
			}
			foreach (MaterialCacheRow item2 in enumerable)
			{
				DataRow dataRow = new DataRow
				{
					TypeName = SmartEnum<GoodsTypesSE, string>.FromValue(item2.Type).Caption,
					MTID = item2.Id,
					MTCode = item2.MTCode,
					Caption = item2.Caption,
					CPACode = item2.CPACode,
					Group = item2.GroupCode,
					GroupCaption = item2.GroupRow.Caption,
					GroupPath = item2.GroupRow.Path,
					Unit = item2.Unit,
					UnitBrief = item2.UnitRow.Brief,
					AltUnit = item2.AltUnit,
					AltUnitBrief = item2.AltUnitRow.Brief,
					BarCode = item2.BarCode,
					Description = item2.Description,
					Country = item2.Country,
					VAT = item2.VAT,
					ShowInPriceList = item2.ShowInPriceList,
					FullCaption = item2.FullCaption,
					IsWeight = item2.IsWeight,
					CheckMarking = item2.CheckMarking
				};
				if (SmartEnum<MTExpenseMethodsSE, string>.TryFromValue(item2.ExpenseMethod, out var result))
				{
					dataRow.ExpenseMethodName = result.Caption;
				}
				dataProviderResult.Rows.Add(dataRow);
			}
		}
		else
		{
			if (settings != null && settings.ReloadData)
			{
				TradeSettings.TradeCacheService.PartnerGood.Refresh();
				TradeSettings.TradeCacheService.Material.Refresh();
			}
			int partID = TradeSettings.TradeCacheService.Partner.GetItem(param.PartnerCode).Id;
			foreach (PartnerGoodCacheRow item3 in TradeSettings.TradeCacheService.PartnerGood.GetList((PartnerGoodCacheRow p) => partID == p.PartID))
			{
				MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(item3.MTID);
				if (itemById != null && (string.IsNullOrWhiteSpace(param.ItemType) || param.ItemType == itemById.Type) && (string.IsNullOrWhiteSpace(param.Group) || param.Group == itemById.GroupCode || itemById.GroupRow.Path.StartsWith(param.Group.Trim() + "!")) && (!param.IsShowOnlyInPriceList || (ArmSoft.SME.Core.UI.Settings.ParametersService.ShowMTInclInPrList() && itemById.ShowInPriceList)) && (string.IsNullOrWhiteSpace(param.LikeName) || itemById.Caption.Contains(param.LikeName.Trim(), StringComparison.CurrentCultureIgnoreCase)))
				{
					DataRow item = new DataRow
					{
						TypeName = SmartEnum<ItemTypesSE, string>.FromValue(itemById.Type).Caption,
						MTID = item3.MTID,
						MTCode = itemById.MTCode,
						Caption = itemById.Caption,
						CPACode = itemById.CPACode,
						Group = itemById.GroupCode,
						GroupCaption = itemById.GroupRow.Caption,
						GroupPath = itemById.GroupRow.Path,
						Unit = itemById.Unit,
						UnitBrief = (string.IsNullOrWhiteSpace(itemById.Unit) ? "" : (TradeSettings.TradeCacheService.QntUnit.GetItem(itemById.Unit)?.Brief ?? "")),
						AltUnit = itemById.AltUnit,
						AltUnitBrief = itemById.AltUnitRow.Brief,
						BarCode = itemById.BarCode,
						Description = itemById.Description,
						Country = itemById.Country,
						ExpenseMethodName = itemById.ExpenseMethod,
						VAT = itemById.VAT,
						ShowInPriceList = itemById.ShowInPriceList,
						FullCaption = itemById.FullCaption,
						IsWeight = itemById.IsWeight,
						CheckMarking = itemById.CheckMarking
					};
					dataProviderResult.Rows.Add(item);
				}
			}
		}
		dataProviderResult.Rows = (from y in dataProviderResult.Rows
			orderby y.TypeName
			orderby y.MTCode
			select y).ToList();
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
