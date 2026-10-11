using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class MaterialCacheRow : CacheRowBaseWithIdName<string>
{
	public class QntUnitRow
	{
		public string QntUnit { get; }

		public decimal Coef { get; }

		public bool IsDefault { get; }

		public Guid? ImageGUID { get; }

		[JsonConstructor]
		public QntUnitRow(string qntUnit, decimal coef, bool isDefault, Guid? imageGUID)
		{
			QntUnit = qntUnit;
			Coef = coef;
			IsDefault = isDefault;
			ImageGUID = imageGUID;
		}
	}

	public class BarCodeRow
	{
		public string BarCode { get; }

		public string QntUnit { get; }

		[JsonConstructor]
		public BarCodeRow(string barCode, string qntUnit)
		{
			BarCode = barCode;
			QntUnit = qntUnit;
		}
	}

	public class ComplectPartRow
	{
		public string ItemType { get; }

		public int ItemId { get; }

		public string QntUnit { get; }

		public decimal ComplectQnt { get; }

		public decimal Quantity { get; }

		public decimal PricePart { get; }

		public decimal Sum { get; }

		[JsonConstructor]
		public ComplectPartRow(string itemType, int itemId, string qntUnit, decimal complectQnt, decimal quantity, decimal pricePart, decimal sum)
		{
			ItemType = itemType;
			ItemId = itemId;
			QntUnit = qntUnit;
			ComplectQnt = complectQnt;
			Quantity = quantity;
			PricePart = pricePart;
			Sum = sum;
		}
	}

	public const string MaterialCache = "MaterialCache";

	private readonly Func<string, MTGroupCacheRow> _groupLookup;

	private readonly Func<string, QntUnitCacheRow> _unitLookup;

	private readonly Func<string, QntUnitCacheRow> _altUnitLookup;

	private readonly Func<int, PartnerCacheRow> _mainSupplierLookup;

	public string MTCode { get; set; }

	public string Caption { get; set; }

	public string FullCaption { get; set; }

	public string GroupCode { get; set; }

	public string Unit { get; set; }

	public string AltUnit { get; set; }

	public string BarCode { get; set; }

	public string CPACode { get; set; }

	public string Type { get; set; }

	public string Description { get; set; }

	public string ExternalCode { get; set; }

	public bool ShowInPriceList { get; set; }

	public string ExpenseMethod { get; set; }

	public bool VAT { get; set; }

	public decimal EcoFeeRate { get; set; }

	public string Producer { get; set; }

	public string Country { get; set; }

	public string Property1 { get; set; }

	public string Property2 { get; set; }

	public string SubstituteItemsGroup { get; set; }

	public bool IsWeight { get; set; }

	public bool CheckMarking { get; set; }

	public string LabelUnit { get; set; }

	public decimal LabelCoeff { get; set; }

	public decimal Coeff { get; set; }

	public decimal Extra { get; set; }

	public decimal Bonus { get; set; }

	public bool IgnoreBonus { get; set; }

	public decimal Point { get; set; }

	public decimal Discount { get; set; }

	public int? PLUCode { get; set; }

	public decimal MinQty { get; set; }

	public decimal MaxQty { get; set; }

	public string CaptionEn { get; set; }

	public string CaptionRu { get; set; }

	public string AdditionalDescr1 { get; set; }

	public string AdditionalDescr2 { get; set; }

	public int MainSupplierId { get; set; }

	public bool ImageExists { get; set; }

	[JsonIgnore]
	public bool IsComplect
	{
		get
		{
			List<ComplectPartRow> complectParts = ComplectParts;
			if (complectParts == null)
			{
				return false;
			}
			return complectParts.Count > 0;
		}
	}

	public List<QntUnitRow> QntUnits { get; set; }

	public List<BarCodeRow> BarCodes { get; set; }

	public List<ComplectPartRow> ComplectParts { get; set; }

	[JsonIgnore]
	public MTGroupCacheRow GroupRow
	{
		get
		{
			if (_groupLookup != null)
			{
				return _groupLookup(GroupCode);
			}
			return GroupUIResolver?.Invoke(GroupCode);
		}
	}

	[JsonIgnore]
	public static Func<string, MTGroupCacheRow> GroupUIResolver { get; set; }

	[JsonIgnore]
	public QntUnitCacheRow UnitRow
	{
		get
		{
			if (_unitLookup != null)
			{
				return _unitLookup(Unit);
			}
			return UnitRowUIResolver?.Invoke(Unit);
		}
	}

	[JsonIgnore]
	public static Func<string, QntUnitCacheRow> UnitRowUIResolver { get; set; }

	[JsonIgnore]
	public QntUnitCacheRow AltUnitRow
	{
		get
		{
			if (_altUnitLookup != null)
			{
				return _altUnitLookup(AltUnit);
			}
			return AltUnitRowUIResolver?.Invoke(AltUnit);
		}
	}

	[JsonIgnore]
	public static Func<string, QntUnitCacheRow> AltUnitRowUIResolver { get; set; }

	[JsonIgnore]
	public PartnerCacheRow MainSupplierRow
	{
		get
		{
			if (_mainSupplierLookup != null)
			{
				return _mainSupplierLookup(MainSupplierId);
			}
			return MainSupplierRowUIResolver?.Invoke(MainSupplierId);
		}
	}

	[JsonIgnore]
	public static Func<int, PartnerCacheRow> MainSupplierRowUIResolver { get; set; }

	[JsonIgnore]
	public static Func<string, string, object> ExtensionUIResolver { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(MTCode);

	public MaterialCacheRow()
	{
		Id = -1;
		MTCode = "";
		Caption = "";
		FullCaption = "";
		Type = "";
		GroupCode = "";
		Unit = "";
		AltUnit = "";
		BarCode = "";
		CPACode = "";
		VAT = false;
		Description = "";
		ExternalCode = "";
		ExpenseMethod = "";
		ShowInPriceList = false;
		Country = "";
		Producer = "";
		Property1 = "";
		Property2 = "";
		SubstituteItemsGroup = "";
		Extra = 0m;
		Bonus = 0m;
		IgnoreBonus = false;
		Point = 0m;
		EcoFeeRate = 0m;
		IsWeight = false;
		CheckMarking = false;
		LabelUnit = "";
		LabelCoeff = 0m;
		Coeff = 0m;
		Discount = 0m;
		PLUCode = null;
		MinQty = 0m;
		MaxQty = 0m;
		CaptionEn = "";
		CaptionRu = "";
		AdditionalDescr1 = "";
		AdditionalDescr2 = "";
		MainSupplierId = -1;
	}

	public MaterialCacheRow(int id, string mtCode, string caption, string fullCaption, string unit, string type, string groupCode, string substituteItemsGroup, string barCode, string altUnit, string cPACode, string description, string externalCode, string expenseMethod, bool showInPriceList, string country, string producer, string property1, string property2, decimal minQty, decimal maxQty, decimal extra, decimal bonus, bool ignoreBonus, decimal point, bool vat, decimal ecoFeeRate, bool isWeight, bool checkMarking, string labelUnit, decimal labelCoeff, decimal coeff, decimal discount, int? pluCode, string captionEn, string captionRu, string additionalDescr1, string additionalDescr2, int mainSupplierId, bool imageExists, byte[] ts, List<QntUnitRow> qntUnits, List<BarCodeRow> barCodes, List<ComplectPartRow> complectParts, Func<string, MTGroupCacheRow> groupLookup, Func<string, QntUnitCacheRow> unitLookup, Func<string, QntUnitCacheRow> altUnitLookup, Func<int, PartnerCacheRow> mainSupplierLookup)
	{
		Id = id;
		MTCode = mtCode;
		Caption = caption;
		FullCaption = fullCaption;
		Type = type;
		GroupCode = groupCode;
		BarCode = barCode;
		CPACode = cPACode;
		Unit = unit;
		AltUnit = altUnit;
		Description = description;
		ExternalCode = externalCode;
		ExpenseMethod = expenseMethod;
		ShowInPriceList = showInPriceList;
		Country = country;
		Producer = producer;
		Property1 = property1;
		Property2 = property2;
		SubstituteItemsGroup = substituteItemsGroup;
		MinQty = minQty;
		MaxQty = maxQty;
		Extra = extra;
		Bonus = bonus;
		IgnoreBonus = ignoreBonus;
		Point = point;
		VAT = vat;
		EcoFeeRate = ecoFeeRate;
		IsWeight = isWeight;
		CheckMarking = checkMarking;
		LabelUnit = labelUnit;
		LabelCoeff = labelCoeff;
		Coeff = coeff;
		Discount = discount;
		PLUCode = pluCode;
		CaptionEn = captionEn;
		CaptionRu = captionRu;
		AdditionalDescr1 = additionalDescr1;
		AdditionalDescr2 = additionalDescr2;
		MainSupplierId = mainSupplierId;
		ImageExists = imageExists;
		TS = ts;
		QntUnits = qntUnits;
		BarCodes = barCodes;
		ComplectParts = complectParts;
		_groupLookup = groupLookup;
		_unitLookup = unitLookup;
		_altUnitLookup = altUnitLookup;
		_mainSupplierLookup = mainSupplierLookup;
	}

	public object GetReferenceValue(string name)
	{
		return name switch
		{
			"MaterialCacheMTCode" => MTCode, 
			"MaterialCacheCaption" => Caption, 
			"MaterialCacheFullCaption" => FullCaption, 
			"MaterialCacheGroupCode" => GroupCode, 
			"MaterialCacheGroupRowCaption" => GroupRow.Caption, 
			"MaterialCacheType" => (Type == ItemTypesSE.Goods.Value) ? GoodsTypesSE.Good.Caption : GoodsTypesSE.Service.Caption, 
			"MaterialCacheUnitRowBrief" => UnitRow.Brief, 
			"MaterialCacheAltUnitRowBrief" => AltUnitRow.Brief, 
			"MaterialCacheBarCode" => BarCode, 
			"MaterialCacheCountry" => Country, 
			"MaterialCacheProducer" => Producer, 
			"MaterialCacheProperty1" => Property1, 
			"MaterialCacheProperty2" => Property2, 
			"MaterialCacheCPACode" => CPACode, 
			"MaterialCacheVAT" => VAT, 
			"MaterialCacheDescription" => Description, 
			"MaterialCacheExternalCode" => ExternalCode, 
			"MaterialCacheExpenseMethod" => ExpenseMethod, 
			"MaterialCacheShowInPriceList" => ShowInPriceList, 
			"MaterialCacheMinQty" => MinQty, 
			"MaterialCacheMaxQty" => MaxQty, 
			"MaterialCacheExtra" => Extra, 
			"MaterialCacheBonus" => Bonus, 
			"MaterialCacheIgnoreBonus" => IgnoreBonus, 
			"MaterialCacheCoeff" => Coeff, 
			"MaterialCacheDiscount" => Discount, 
			"MaterialCachePLUCode" => PLUCode, 
			"MaterialCacheIsWeight" => IsWeight, 
			"MaterialCacheCheckMarking" => CheckMarking, 
			"MaterialCacheSubstituteItemsGroup" => SubstituteItemsGroup, 
			"MaterialCacheCaptionEn" => CaptionEn, 
			"MaterialCacheCaptionRu" => CaptionRu, 
			"MaterialCacheAdditionalDescr1" => AdditionalDescr1, 
			"MaterialCacheAdditionalDescr2" => AdditionalDescr2, 
			"MaterialCacheMainSupplierId" => MainSupplierId, 
			"MaterialCacheMainSupplierRowPartCode" => MainSupplierRow?.PartCode, 
			_ => ExtensionUIResolver?.Invoke(MTCode, name.Substring("MaterialCache".Length)), 
		};
	}
}
