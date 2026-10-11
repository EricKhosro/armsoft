using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Special)]
[SmartEnumPartial(SmartEnumType.Base)]
public class CacheIdSE : ASSmartEnum<CacheIdSE, short>
{
	public static readonly CacheIdSE Goods = new CacheIdSE("Goods", 10001, () => Resources.MaterialOrService);

	public static readonly CacheIdSE Employees = new CacheIdSE("Employees", 10003, () => ArmSoft.SME.Common.Resources.Employee);

	public static readonly CacheIdSE Partners = new CacheIdSE("Partners", 10004, () => ArmSoft.SME.Common.Resources.Partner);

	public static readonly CacheIdSE AccountInBank = new CacheIdSE("AccountInBank", 10005, () => ArmSoft.SME.Common.Resources.AccountInBank);

	public static readonly CacheIdSE Cashes = new CacheIdSE("Cashes", 10006, () => ArmSoft.SME.Common.Resources.Cash);

	public static readonly CacheIdSE QntUnits = new CacheIdSE("QntUnits", 10007, () => ArmSoft.SME.Common.Resources.MeasureUnit);

	public static readonly CacheIdSE Storages = new CacheIdSE("Storages", 10008, () => ArmSoft.SME.Common.Resources.Storage);

	public static readonly CacheIdSE MTGroup = new CacheIdSE("MTGroup", 10009, () => Resources.MTGroup);

	public static readonly CacheIdSE Properties = new CacheIdSE("Properties", 10010, () => Resources.Attribute);

	public static readonly CacheIdSE PartnerGroup = new CacheIdSE("PartnerGroup", 10011, () => ArmSoft.SME.Common.Resources.PartnersGroup);

	public static readonly CacheIdSE CashierShift = new CacheIdSE("CashierShift", 10014, () => Resources.CashierShift);

	public static readonly CacheIdSE PartnerGoods = new CacheIdSE("PartnerGoods", 10015, () => Resources.PartnersMTAndServices);

	public static readonly CacheIdSE DiscountGroups = new CacheIdSE("DiscountGroups", 10016, () => Resources.DiscountGroup);

	public static readonly CacheIdSE Discounts = new CacheIdSE("Discounts", 10017, () => Resources.Discount);

	public static readonly CacheIdSE StandardComments = new CacheIdSE("StandardComments", 10022, () => ArmSoft.SME.Common.Resources.Comment);

	public static readonly CacheIdSE Countries = new CacheIdSE("Countries", 10023, () => ArmSoft.SME.Common.Resources.Country);

	public static readonly CacheIdSE Producers = new CacheIdSE("Producers", 10024, () => ArmSoft.SME.Common.Resources.Producer);

	public static readonly CacheIdSE SubstituteItemsGroups = new CacheIdSE("SubstituteItemsGroups", 10025, () => Resources.SubstituteItemsGroup);

	public static readonly CacheIdSE Segments = new CacheIdSE("Segments", 10026, () => Resources.Segment);

	public static readonly CacheIdSE PropertiesList = new CacheIdSE("PropertiesList", 10028, () => Resources.PropertiesListsValues);

	public static readonly CacheIdSE PriceTypes = new CacheIdSE("PriceTypes", 10029, () => ArmSoft.SME.Common.Resources.PriceListType);

	public static readonly CacheIdSE MTFavoriteGroups = new CacheIdSE("MTFavoriteGroups", 10030, () => Resources.Groups);

	public static readonly CacheIdSE MTFavorites = new CacheIdSE("MTFavorites", 10031, () => Resources.MTFavorites);

	public static readonly CacheIdSE CurrentPrices = new CacheIdSE("CurrentPrices", 10032, () => Resources.CurrentPrices);

	public static readonly CacheIdSE GoodExtensions = new CacheIdSE("GoodExtensions", 10033, () => Resources.MaterialOrService);

	public static readonly CacheIdSE PartnerExtensions = new CacheIdSE("PartnerExtensions", 10034, () => ArmSoft.SME.Common.Resources.Partner);

	public static readonly CacheIdSE Currencies = new CacheIdSE("Currencies", 2, () => ArmSoft.SME.Common.Resources.Currency);

	public static readonly CacheIdSE Parameters = new CacheIdSE("Parameters", 4, () => ArmSoft.SME.Common.Resources.Parameter);

	public static readonly CacheIdSE UserParameters = new CacheIdSE("UserParameters", 5, () => ArmSoft.SME.Common.Resources.UserParameter);

	public static readonly CacheIdSE Templates = new CacheIdSE("Templates", 7, () => ArmSoft.SME.Common.Resources.PrintTemplate);

	public static readonly CacheIdSE Users = new CacheIdSE("Users", 8, () => ArmSoft.SME.Common.Resources.User);

	public static readonly CacheIdSE UserAccess = new CacheIdSE("UserAccess", 9, () => ArmSoft.SME.Common.Resources.UserAccess);

	public static readonly CacheIdSE Tree = new CacheIdSE("Tree", 10, () => ArmSoft.SME.Common.Resources.TreeDef);

	public static readonly CacheIdSE Banks = new CacheIdSE("Banks", 108, () => ArmSoft.SME.Common.Resources.Bank);

	public static readonly CacheIdSE CPAClassifiers = new CacheIdSE("CPAClassifiers", 109, () => ArmSoft.SME.Common.Resources.CPAClassification);

	public static readonly CacheIdSE SystemConfig = new CacheIdSE("SystemConfig", 200, () => "SystemConfig");

	public static readonly CacheIdSE StringResources = new CacheIdSE("StringResources", 110, () => "StringResources");

	public static readonly CacheIdSE DocumentExtensions = new CacheIdSE("DocumentExtensions", 111, () => "DocumentExtensions");

	public static readonly CacheIdSE CurrentExchangeRates = new CacheIdSE("CurrentExchangeRates", 112, () => ArmSoft.SME.Common.Resources.ExchangeRates);

	public CacheIdSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
