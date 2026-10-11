using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class MaterialCache : CacheDefinition<MaterialCacheRow, string>
{
	private Dictionary<string, int> _itemIdsByBarCodes;

	public override short CacheId => ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Goods.Value;

	public override bool StoreInLocalStorage => true;

	protected override bool CacheAlsoWithId => true;

	public IReadOnlyDictionary<string, int> ItemIdsByBarCodes
	{
		get
		{
			if (_itemIdsByBarCodes == null)
			{
				_itemIdsByBarCodes = new Dictionary<string, int>();
				foreach (MaterialCacheRow item in GetList())
				{
					if (item.BarCodes.Count <= 0)
					{
						continue;
					}
					foreach (MaterialCacheRow.BarCodeRow barCode in item.BarCodes)
					{
						_itemIdsByBarCodes.TryAdd(barCode.BarCode, item.Id);
					}
				}
			}
			return _itemIdsByBarCodes;
		}
	}

	public MaterialCache()
	{
		MaterialCacheRow.GroupUIResolver = (string code) => TradeSettings.TradeCacheService.MTGroup.GetItem(code, throwExceptionNotExits: true);
		MaterialCacheRow.UnitRowUIResolver = (string code) => TradeSettings.TradeCacheService.QntUnit.GetItem(code, throwExceptionNotExits: true);
		MaterialCacheRow.AltUnitRowUIResolver = (string code) => TradeSettings.TradeCacheService.QntUnit.GetItem(code, throwExceptionNotExits: true);
		MaterialCacheRow.MainSupplierRowUIResolver = (int id) => (id <= 0) ? null : TradeSettings.TradeCacheService.Partner.GetItemById(id, throwExceptionNotExits: true);
		MaterialCacheRow.ExtensionUIResolver = (string code, string fieldName) => ExtensionUIResolverFunction(TradeSettings.TradeCacheService.MaterialExtensions.GetItem(code), fieldName);
	}

	protected override bool InnerRefresh(bool checkChanging, bool ignoreDeleted, out List<string> updatedKeys)
	{
		bool flag = base.InnerRefresh(checkChanging, ignoreDeleted, out updatedKeys);
		if (flag)
		{
			_itemIdsByBarCodes = null;
		}
		return flag;
	}

	public override void Clear()
	{
		base.Clear();
		_itemIdsByBarCodes?.Clear();
		_itemIdsByBarCodes = null;
	}

	public MaterialCacheRow.BarCodeRow GetBarCode(int mtId, string qntUnit, bool throwExceptionNotExits = false)
	{
		return GetBarCodes(mtId, qntUnit, throwExceptionNotExits).FirstOrDefault();
	}

	public List<MaterialCacheRow.BarCodeRow> GetBarCodes(int mtId, string qntUnit, bool throwExceptionNotExits = false)
	{
		MaterialCacheRow itemById = GetItemById(mtId, throwExceptionNotExits);
		List<MaterialCacheRow.BarCodeRow> list = itemById?.BarCodes.Where((MaterialCacheRow.BarCodeRow p) => p.QntUnit == qntUnit)?.OrderBy((MaterialCacheRow.BarCodeRow p) => p.BarCode).ToList();
		if (throwExceptionNotExits && list == null)
		{
			throw new RESTException(string.Format(ArmSoft.SME.Trade.Common.Resources.InvalidMTQntUnit, itemById.MTCode, qntUnit));
		}
		return list;
	}

	public MaterialCacheRow.BarCodeRow GetBarCode(MaterialCacheRow materialCacheRow, string qntUnit, bool throwExceptionNotExits = false)
	{
		return GetBarCodes(materialCacheRow, qntUnit, throwExceptionNotExits).FirstOrDefault();
	}

	public List<MaterialCacheRow.BarCodeRow> GetBarCodes(MaterialCacheRow materialCacheRow, string qntUnit, bool throwExceptionNotExits = false)
	{
		List<MaterialCacheRow.BarCodeRow> list = materialCacheRow?.BarCodes.Where((MaterialCacheRow.BarCodeRow p) => p.QntUnit == qntUnit)?.OrderBy((MaterialCacheRow.BarCodeRow p) => p.BarCode).ToList();
		if (throwExceptionNotExits && list == null)
		{
			throw new RESTException(string.Format(ArmSoft.SME.Trade.Common.Resources.InvalidMTQntUnit, materialCacheRow.MTCode, qntUnit));
		}
		return list;
	}

	public MaterialCacheRow GetItemByBarCode(string barCode, out string qntUnit, bool throwExceptionNotExits = false)
	{
		MaterialCacheRow materialCacheRow = null;
		qntUnit = null;
		if (ItemIdsByBarCodes.TryGetValue(barCode, out var value))
		{
			materialCacheRow = GetItemById(value, throwExceptionNotExits);
			qntUnit = materialCacheRow.BarCodes.FirstOrDefault((MaterialCacheRow.BarCodeRow p) => p.BarCode == barCode)?.QntUnit;
		}
		else if (throwExceptionNotExits)
		{
			throw new RESTException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeNotExist, barCode));
		}
		return materialCacheRow;
	}

	public MaterialCacheRow GetItemByBarCode(string barCode, bool throwExceptionNotExits = false)
	{
		string qntUnit;
		return GetItemByBarCode(barCode, out qntUnit, throwExceptionNotExits);
	}

	public MaterialCacheRow GetFirstItemByCaption(string caption, int itemID = -1)
	{
		return GetList().FirstOrDefault((MaterialCacheRow item) => item.Caption.Trim() == caption.Trim() && (itemID == -1 || item.Id != itemID));
	}

	public List<MaterialCacheRow> GetItems(string group)
	{
		MTGroupCacheRow groupRow = TradeSettings.TradeCacheService.MTGroup.GetItem(group);
		List<MaterialCacheRow> result = new List<MaterialCacheRow>();
		if (groupRow != null)
		{
			result = GetList((MaterialCacheRow p) => p.GroupCode == groupRow.Code || p.GroupRow.Path.StartsWith(groupRow.Path.Trim() + "!")).ToList();
		}
		return result;
	}

	public bool HasSubstitution(string itemCode)
	{
		if (ItemsWithKeys.TryGetValue(itemCode, out var value))
		{
			string substituteItemsGroup = value.SubstituteItemsGroup;
			if (string.IsNullOrWhiteSpace(substituteItemsGroup))
			{
				return false;
			}
			return GetList().Any((MaterialCacheRow item) => item.MTCode != itemCode && item.SubstituteItemsGroup == substituteItemsGroup);
		}
		return false;
	}

	public bool IsBonusIgnoredForItem(string itemCode)
	{
		return GetItem(itemCode)?.IgnoreBonus ?? false;
	}

	public bool IsBonusIgnoredForItem(int itemId)
	{
		return GetItemById(itemId)?.IgnoreBonus ?? false;
	}

	public MaterialCacheRow.QntUnitRow GetQntUnit(int mtId, string qntUnit, bool throwExceptionNotExits = false)
	{
		MaterialCacheRow itemById = GetItemById(mtId, throwExceptionNotExits);
		MaterialCacheRow.QntUnitRow qntUnitRow = itemById?.QntUnits.FirstOrDefault((MaterialCacheRow.QntUnitRow p) => p.QntUnit == qntUnit);
		if (throwExceptionNotExits && qntUnitRow == null)
		{
			throw new RESTException(string.Format(ArmSoft.SME.Trade.Common.Resources.InvalidMTQntUnit, itemById.MTCode, qntUnit));
		}
		return qntUnitRow;
	}

	public MaterialCacheRow.QntUnitRow GetQntUnit(string mtCode, string qntUnit, bool throwExceptionNotExits = false)
	{
		MaterialCacheRow item = GetItem(mtCode, throwExceptionNotExits);
		MaterialCacheRow.QntUnitRow qntUnitRow = item?.QntUnits.FirstOrDefault((MaterialCacheRow.QntUnitRow p) => p.QntUnit == qntUnit);
		if (throwExceptionNotExits && qntUnitRow == null)
		{
			throw new RESTException(string.Format(ArmSoft.SME.Trade.Common.Resources.InvalidMTQntUnit, item.MTCode, qntUnit));
		}
		return qntUnitRow;
	}

	public MaterialCacheRow.QntUnitRow GetQntUnit(MaterialCacheRow materialCacheRow, string qntUnit, bool throwExceptionNotExits = false)
	{
		MaterialCacheRow.QntUnitRow qntUnitRow = materialCacheRow?.QntUnits.FirstOrDefault((MaterialCacheRow.QntUnitRow p) => p.QntUnit == qntUnit);
		if (throwExceptionNotExits && qntUnitRow == null)
		{
			throw new RESTException(string.Format(ArmSoft.SME.Trade.Common.Resources.InvalidMTQntUnit, materialCacheRow.MTCode, qntUnit));
		}
		return qntUnitRow;
	}

	public static List<ViewColumn> GetDataViewColumns(string group, string namePrefix, string captionPrefix, bool includeCode = false, bool includeCaption = false, bool includeUnitBrief = false, bool includeGroup = true, bool includeSpecification = true, bool onlyExtendedFields = false)
	{
		List<ViewColumn> list = new List<ViewColumn>();
		if (!onlyExtendedFields)
		{
			if (includeCode)
			{
				list.Add(new ViewColumn(namePrefix + "MTCode", captionPrefix + ArmSoft.SME.Common.Resources.Code, "MTCode", DataColumnType.String, group, "MaterialCache"));
			}
			if (includeCaption)
			{
				list.Add(new ViewColumn(namePrefix + "Caption", captionPrefix + ArmSoft.SME.Common.Resources.Name, "Caption", DataColumnType.String, group, "MaterialCache"));
			}
			list.Add(new ViewColumn(namePrefix + "FullCaption", captionPrefix + ArmSoft.SME.Common.Resources.FullCaption, "FullCaption", DataColumnType.String, group, "MaterialCache"));
			if (includeGroup)
			{
				list.Add(new ViewColumn(namePrefix + "GroupCode", captionPrefix + ArmSoft.SME.Common.Resources.Group, "GroupCode", DataColumnType.String, group, "MaterialCache"));
			}
			list.Add(new ViewColumn(namePrefix + "GroupRowCaption", captionPrefix + ArmSoft.SME.Common.Resources.GroupName, "GroupRowCaption", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "Type", captionPrefix + ArmSoft.SME.Common.Resources.TypeWord, "Type", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "MainSupplierRowPartCode", captionPrefix + ArmSoft.SME.Trade.Common.Resources.MainSupplier, "MainSupplierRowPartCode", DataColumnType.String, group, "MaterialCache"));
			if (includeUnitBrief)
			{
				list.Add(new ViewColumn(namePrefix + "UnitRowBrief", captionPrefix + ArmSoft.SME.Trade.Common.Resources.BaseQUNIT, "UnitRowBrief", DataColumnType.String, group, "MaterialCache"));
			}
			list.Add(new ViewColumn(namePrefix + "AltUnitRowBrief", captionPrefix + ArmSoft.SME.Trade.Common.Resources.ALTQUNIT, "AltUnitRowBrief", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "BarCode", captionPrefix + ArmSoft.SME.Common.Resources.BarCode, "BarCode", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "Country", captionPrefix + ArmSoft.SME.Common.Resources.Country, "Country", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "Producer", captionPrefix + ArmSoft.SME.Common.Resources.Producer, "Producer", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "Property1", captionPrefix + ArmSoft.SME.Trade.Common.Resources.Attribute1, "Property1", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "Property2", captionPrefix + ArmSoft.SME.Trade.Common.Resources.Attribute2, "Property2", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "CPACode", captionPrefix + ArmSoft.SME.Common.Resources.CPACodeShort, "CPACode", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "VAT", captionPrefix + ArmSoft.SME.Common.Resources.OptionsVAT, "VAT", DataColumnType.Boolean, group, "MaterialCache"));
			if (includeSpecification)
			{
				list.Add(new ViewColumn(namePrefix + "Description", captionPrefix + ArmSoft.SME.Common.Resources.Specification, "Description", DataColumnType.String, group, "MaterialCache"));
			}
			list.Add(new ViewColumn(namePrefix + "ExternalCode", captionPrefix + ArmSoft.SME.Common.Resources.ExternalCode, "ExternalCode", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "ExpenseMethod", captionPrefix + ArmSoft.SME.Common.Resources.MTExpenseMethod, "ExpenseMethod", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "ShowInPriceList", captionPrefix + ArmSoft.SME.Trade.Common.Resources.ShowInPriceListes, "ShowInPriceList", DataColumnType.Boolean, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "MinQty", captionPrefix + ArmSoft.SME.Common.Resources.MinQty, "MinQty", DataColumnType.Decimal, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "MaxQty", captionPrefix + ArmSoft.SME.Common.Resources.MaxQty, "MaxQty", DataColumnType.Decimal, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "Extra", captionPrefix + ArmSoft.SME.Trade.Common.Resources.Extra, "Extra", DataColumnType.Decimal, group, "MaterialCache"));
			if (Settings.ParametersService.UseBonus())
			{
				list.Add(new ViewColumn(caption: captionPrefix + ((!(Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)) ? string.Format(ArmSoft.SME.Trade.Common.Resources.Bonus, ArmSoft.SME.Common.Resources.Unit) : string.Format(ArmSoft.SME.Trade.Common.Resources.Bonus, ArmSoft.SME.Trade.Common.Resources.PercentSymbol)), name: namePrefix + "Bonus", source: "Bonus", type: DataColumnType.Decimal, group: group, cachePrefix: "MaterialCache"));
			}
			list.Add(new ViewColumn(namePrefix + "Coeff", captionPrefix + ArmSoft.SME.Common.Resources.OverHeadExpCoeff, "Coeff", DataColumnType.Decimal, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "Discount", captionPrefix + ArmSoft.SME.Trade.Common.Resources.Discount + " (%)", "Discount", DataColumnType.Decimal, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "IsWeight", captionPrefix + ArmSoft.SME.Trade.Common.Resources.WeightItem, "IsWeight", DataColumnType.Boolean, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "PLUCode", captionPrefix + ArmSoft.SME.Trade.Common.Resources.PLUCodeInElScale, "PLUCode", DataColumnType.Integer, group, "MaterialCache"));
			if (Settings.ParametersService.UseMarking())
			{
				list.Add(new ViewColumn(namePrefix + "CheckMarking", captionPrefix + ArmSoft.SME.Trade.Common.Resources.CheckMarking, "CheckMarking", DataColumnType.Boolean, group, "MaterialCache"));
			}
			if (Settings.ParametersService.EnableSubstituteItems())
			{
				list.Add(new ViewColumn(namePrefix + "SubstituteItemsGroup", captionPrefix + ArmSoft.SME.Trade.Common.Resources.SubstituteItemsGroup, "SubstituteItemsGroup", DataColumnType.String, group, "MaterialCache"));
			}
			list.Add(new ViewColumn(namePrefix + "CaptionEn", captionPrefix + ArmSoft.SME.Trade.Common.Resources.MTNameEn, "CaptionEn", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "CaptionRu", captionPrefix + ArmSoft.SME.Trade.Common.Resources.MTNameRu, "CaptionRu", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "AdditionalDescr1", captionPrefix + ArmSoft.SME.Trade.Common.Resources.AdditionalDescription + "1", "AdditionalDescr1", DataColumnType.String, group, "MaterialCache"));
			list.Add(new ViewColumn(namePrefix + "AdditionalDescr2", captionPrefix + ArmSoft.SME.Trade.Common.Resources.AdditionalDescription + "2", "AdditionalDescr2", DataColumnType.String, group, "MaterialCache"));
		}
		List<ViewColumn> collection = ViewColumn.AddXMLColumns(POTypeSE.Directory.Value, ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value, group, "MaterialCache", addGroupToCaption: false);
		list.AddRange(collection);
		return list;
	}
}
