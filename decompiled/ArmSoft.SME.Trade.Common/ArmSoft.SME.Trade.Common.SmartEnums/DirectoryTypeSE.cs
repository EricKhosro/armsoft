using System;
using System.Text.Json.Serialization;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Special)]
[SmartEnumProviderSupport(2, "DirectoryType")]
[SmartEnumPartial(SmartEnumType.Base)]
public class DirectoryTypeSE : ASSmartEnum<DirectoryTypeSE, short>
{
	public static readonly DirectoryTypeSE Goods = new DirectoryTypeSE("Goods", 1, () => Resources.MaterialOrService, () => Resources.MaterialOrService);

	public static readonly DirectoryTypeSE Employee = new DirectoryTypeSE("Employee", 3, () => ArmSoft.SME.Common.Resources.Employee, () => ArmSoft.SME.Common.Resources.Employee);

	public static readonly DirectoryTypeSE Partner = new DirectoryTypeSE("Partner", 2, () => ArmSoft.SME.Common.Resources.Partner, () => ArmSoft.SME.Common.Resources.Partner);

	public static readonly DirectoryTypeSE Discount = new DirectoryTypeSE("Discount", 4, () => Resources.Discount, () => Resources.Discount);

	public static readonly DirectoryTypeSE AccountInBank = new DirectoryTypeSE("AccountInBank", 5, () => ArmSoft.SME.Common.Resources.SettlementAccount, () => ArmSoft.SME.Common.Resources.SettlementAccount);

	public static readonly DirectoryTypeSE DiscountGroup = new DirectoryTypeSE("DiscountGroup", 6, () => Resources.DiscountGroup, () => Resources.DiscountGroup);

	public static readonly DirectoryTypeSE PriceType = new DirectoryTypeSE("PriceType", 7, () => ArmSoft.SME.Common.Resources.PriceListType, () => ArmSoft.SME.Common.Resources.PriceListType);

	public static readonly DirectoryTypeSE BonusScale = new DirectoryTypeSE("BonusScale", 12, () => Resources.BonusScale, () => Resources.BonusScale);

	public static readonly DirectoryTypeSE MTFavoritesGroup = new DirectoryTypeSE("MTFavoritesGroup", 13, () => Resources.FavoriteGroups, () => Resources.FavoriteGroups);

	public static readonly DirectoryTypeSE GoodsBySegment = new DirectoryTypeSE("GoodsBySegment", 15, () => Resources.GoodsBySegment, () => Resources.GoodsBySegment);

	public static readonly DirectoryTypeSE MTBarCode = new DirectoryTypeSE("MTBarCode", 16, () => ArmSoft.SME.Common.Resources.BarCode, () => Resources.MTBarCodes);

	public static readonly DirectoryTypeSE MTComplects = new DirectoryTypeSE("MTComplects", 42, () => ArmSoft.SME.Common.Resources.Complect, () => ArmSoft.SME.Common.Resources.Complect);

	public static readonly DirectoryTypeSE MTComplectsPart = new DirectoryTypeSE("MTComplectsPart", 17, () => Resources.MTComplectPart, () => Resources.MTComplectPart);

	public static readonly DirectoryTypeSE MTFavoriteMaterial = new DirectoryTypeSE("MTFavoriteMaterial", 18, () => Resources.MTFavorites, () => Resources.MTFavorites);

	public static readonly DirectoryTypeSE MTGroup = new DirectoryTypeSE("MTGroup", 19, () => Resources.MTGroup, () => Resources.MTGroup);

	public static readonly DirectoryTypeSE MTQntUnit = new DirectoryTypeSE("MTQntUnit", 20, () => Resources.MTQntUnits, () => Resources.MTQntUnits);

	public static readonly DirectoryTypeSE OrderMinMax = new DirectoryTypeSE("OrderMinMax", 21, () => Resources.MTOrderMinMax, () => Resources.MTOrderMinMax);

	public static readonly DirectoryTypeSE PartnersGood = new DirectoryTypeSE("PartnersGood", 22, () => Resources.PartnersMTAndServices, () => Resources.PartnersMTAndServices);

	public static readonly DirectoryTypeSE Price = new DirectoryTypeSE("Price", 24, () => ArmSoft.SME.Common.Resources.Price, () => ArmSoft.SME.Common.Resources.Price);

	public static readonly DirectoryTypeSE Property = new DirectoryTypeSE("Property", 25, () => Resources.Attribute, () => Resources.Attribute);

	public static readonly DirectoryTypeSE PropertyList = new DirectoryTypeSE("PropertyList", 26, () => Resources.AttributeValue, () => Resources.PropertiesListsValues);

	public static readonly DirectoryTypeSE QntUnit = new DirectoryTypeSE("QntUnit", 27, () => ArmSoft.SME.Common.Resources.MeasureUnit, () => ArmSoft.SME.Common.Resources.MeasureUnit);

	public static readonly DirectoryTypeSE Storage = new DirectoryTypeSE("Storage", 28, () => ArmSoft.SME.Common.Resources.StorageAndShop, () => ArmSoft.SME.Common.Resources.StorageAndShop);

	public static readonly DirectoryTypeSE Contract = new DirectoryTypeSE("Contract", 29, () => ArmSoft.SME.Common.Resources.Contract, () => ArmSoft.SME.Common.Resources.Contract);

	public static readonly DirectoryTypeSE PartnerAccountsInBank = new DirectoryTypeSE("PartnerAccountsInBank", 30, () => ArmSoft.SME.Common.Resources.PartnerAccountsInBank, () => ArmSoft.SME.Common.Resources.PartnerAccountsInBank);

	public static readonly DirectoryTypeSE PartnerGroup = new DirectoryTypeSE("PartnerGroup", 31, () => Resources.PartnerGroup, () => Resources.PartnerGroup);

	public static readonly DirectoryTypeSE Cash = new DirectoryTypeSE("Cash", 32, () => ArmSoft.SME.Common.Resources.Cash, () => ArmSoft.SME.Common.Resources.Cash);

	public static readonly DirectoryTypeSE CashierShift = new DirectoryTypeSE("CashierShift", 33, () => Resources.CashierShift, () => Resources.CashierShift);

	public static readonly DirectoryTypeSE Countries = new DirectoryTypeSE("Countries", 35, () => ArmSoft.SME.Common.Resources.Country, () => ArmSoft.SME.Common.Resources.Country);

	public static readonly DirectoryTypeSE Producers = new DirectoryTypeSE("Producers", 36, () => ArmSoft.SME.Common.Resources.Producer, () => ArmSoft.SME.Common.Resources.Producer);

	public static readonly DirectoryTypeSE SubstituteItemsGroups = new DirectoryTypeSE("SubstituteItemsGroups", 37, () => Resources.SubstituteItemsGroup, () => Resources.SubstituteItemsGroup);

	public static readonly DirectoryTypeSE Segments = new DirectoryTypeSE("Segments", 38, () => Resources.Segment, () => Resources.Segment);

	public static readonly DirectoryTypeSE StdCom = new DirectoryTypeSE("StdCom", 39, () => ArmSoft.SME.Common.Resources.StandartComment, () => ArmSoft.SME.Common.Resources.StandartComment);

	public static readonly DirectoryTypeSE UserCashesAccess = new DirectoryTypeSE("UserCashesAccess", 41, () => Resources.UserCashesAvailability, () => Resources.UserCashesAvailability);

	public static readonly DirectoryTypeSE UserStorageOptions = new DirectoryTypeSE("UserStorageOptions", 40, () => Resources.UserStorageAvailability, () => Resources.UserStorageAvailability);

	public static readonly DirectoryTypeSE ElScaleOrTerminal = new DirectoryTypeSE("ElScaleOrTerminal", 43, () => Resources.ElScalesAndTerminals, () => Resources.ElScalesAndTerminals);

	public static readonly DirectoryTypeSE EMarkReceipt = new DirectoryTypeSE("EMarkReceipt", 44, () => Resources.EMarkReceiptIntegration, () => Resources.EMarkReceiptIntegration);

	public const short GoodsConst = 1;

	public const short PartnerConst = 2;

	public const short EmployeeConst = 3;

	public const short DiscountConst = 4;

	public const short AccountInBankConst = 5;

	public const short DiscountGroupConst = 6;

	public const short PriceTypeConst = 7;

	public const short BonusScaleConst = 12;

	public const short MTFavoritesGroupConst = 13;

	public const short DiscountScaleConst = 14;

	public const short GoodsBySegmentConst = 15;

	public const short MTBarCodeConst = 16;

	public const short MTComplectsPartConst = 17;

	public const short MTFavoriteMaterialConst = 18;

	public const short MTGroupConst = 19;

	public const short MTQntUnitConst = 20;

	public const short OrderMinMaxConst = 21;

	public const short PartnersGoodConst = 22;

	public const short PriceConst = 24;

	public const short PropertyConst = 25;

	public const short PropertyListConst = 26;

	public const short QntUnitConst = 27;

	public const short StorageConst = 28;

	public const short ContractConst = 29;

	public const short PartnerAccountsInBankConst = 30;

	public const short PartnerGroupConst = 31;

	public const short CashConst = 32;

	public const short CashierShiftConst = 33;

	public const short CountriesConst = 35;

	public const short ProducersConst = 36;

	public const short SubstituteItemsGroupsConst = 37;

	public const short SegmentsConst = 38;

	public const short StdComConst = 39;

	public const short UserStorageOptionsConst = 40;

	public const short UserCashesAccessConst = 41;

	public const short MTComplectConst = 42;

	public const short ElScaleOrTerminalConst = 43;

	public const short EMarkReceiptConst = 44;

	[JsonIgnore]
	public readonly Func<string> FullCaptionDelegate;

	public static readonly DirectoryTypeSE Bank = new DirectoryTypeSE("Bank", 231, () => ArmSoft.SME.Common.Resources.Bank, () => ArmSoft.SME.Common.Resources.Bank);

	public static readonly DirectoryTypeSE Currency = new DirectoryTypeSE("Currency", 232, () => ArmSoft.SME.Common.Resources.Curr, () => ArmSoft.SME.Common.Resources.Currency);

	public static readonly DirectoryTypeSE RatesOfExchange = new DirectoryTypeSE("RatesOfExchange", 233, () => ArmSoft.SME.Common.Resources.ExchangeRate, () => ArmSoft.SME.Common.Resources.ExchangeRate);

	public static readonly DirectoryTypeSE CPAClassifier = new DirectoryTypeSE("CPAClassifier", 234, () => ArmSoft.SME.Common.Resources.CPAClassification, () => ArmSoft.SME.Common.Resources.CPAClassification);

	public static readonly DirectoryTypeSE Database = new DirectoryTypeSE("Database", 235, () => ArmSoft.SME.Common.Resources.Database, () => ArmSoft.SME.Common.Resources.Database);

	public static readonly DirectoryTypeSE Users = new DirectoryTypeSE("Users", 236, () => ArmSoft.SME.Common.Resources.User, () => ArmSoft.SME.Common.Resources.User);

	public const short BankConst = 231;

	public const short CurrencyConst = 232;

	public const short RatesOfExchangeConst = 233;

	public const short CPAClassifierConst = 234;

	public const short DatabaseConst = 235;

	public const short UsersConst = 236;

	public string FullCaption => FullCaptionDelegate();

	protected DirectoryTypeSE(string name, short value, Func<string> caption, Func<string> fullCaption = null)
		: base(name, value, caption)
	{
		FullCaptionDelegate = ((fullCaption == null) ? caption : fullCaption);
	}
}
