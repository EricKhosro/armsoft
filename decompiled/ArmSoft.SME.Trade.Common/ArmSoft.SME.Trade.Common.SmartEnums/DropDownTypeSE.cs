using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Special)]
[SmartEnumProviderSupport(30, "DropDownTypes")]
[SmartEnumPartial(SmartEnumType.Base)]
public class DropDownTypeSE : ASSmartEnum<DropDownTypeSE, short>
{
	public static readonly DropDownTypeSE Goods = new DropDownTypeSE("Goods", 10001, () => ArmSoft.SME.Common.Resources.Products, canUsedInExtensions: true);

	public static readonly DropDownTypeSE Partners = new DropDownTypeSE("Partners", 10002, () => ArmSoft.SME.Common.Resources.Partners, canUsedInExtensions: true);

	public static readonly DropDownTypeSE Employees = new DropDownTypeSE("Employees", 10003, () => ArmSoft.SME.Common.Resources.Employees);

	public static readonly DropDownTypeSE Storages = new DropDownTypeSE("Storages", 10004, () => ArmSoft.SME.Common.Resources.Storages);

	public static readonly DropDownTypeSE PriceTypes = new DropDownTypeSE("PriceTypes", 10005, () => ArmSoft.SME.Common.Resources.PriceListTypes);

	public const short GoodsConst = 10001;

	public const short PartnersConst = 10002;

	public const short EmployeesConst = 10003;

	public const short StoragesConst = 10004;

	public const short PriceTypesConst = 10005;

	public static readonly DropDownTypeSE AccountInBanks = new DropDownTypeSE("AccountInBanks", 1, () => ArmSoft.SME.Common.Resources.AccountInBank, canUsedInExtensions: true);

	public static readonly DropDownTypeSE PartnerAccountInBanks = new DropDownTypeSE("PartnerAccountInBanks", 2, () => ArmSoft.SME.Common.Resources.PartnerAccountsInBank, canUsedInExtensions: true);

	public static readonly DropDownTypeSE Trees = new DropDownTypeSE("Trees", 3, () => ArmSoft.SME.Common.Resources.TreeDefs);

	public static readonly DropDownTypeSE Users = new DropDownTypeSE("Users", 4, () => ArmSoft.SME.Common.Resources.Users, canUsedInExtensions: true);

	public static readonly DropDownTypeSE CPACodes = new DropDownTypeSE("CPACodes", 5, () => ArmSoft.SME.Common.Resources.CPAClassifications, canUsedInExtensions: true, CacheIdSE.CPAClassifiers);

	public static readonly DropDownTypeSE Currencies = new DropDownTypeSE("Currencies", 6, () => ArmSoft.SME.Common.Resources.Curres, canUsedInExtensions: true, CacheIdSE.Currencies);

	public static readonly DropDownTypeSE B2BUnitedProducts = new DropDownTypeSE("B2BUnitedProducts", 7, () => ArmSoft.SME.Common.Resources.Unitedb2bCloudGoodsCatalog);

	public static readonly DropDownTypeSE Backups = new DropDownTypeSE("Backups", 8, () => ArmSoft.SME.Common.Resources.Backups);

	public static readonly DropDownTypeSE Templates = new DropDownTypeSE("Templates", 9, () => ArmSoft.SME.Common.Resources.PrintTemplates);

	public static readonly DropDownTypeSE Databases = new DropDownTypeSE("Databases", 10, () => ArmSoft.SME.Common.Resources.Databases);

	public static readonly DropDownTypeSE DocumentTypes = new DropDownTypeSE("DocumentTypes", 11, () => ArmSoft.SME.Common.Resources.DocumentTypes);

	public static readonly DropDownTypeSE DropBoxFiles = new DropDownTypeSE("DropBoxFiles", 12, () => ArmSoft.SME.Common.Resources.FilesFromDropBox);

	public static readonly DropDownTypeSE OnlineReportTemplates = new DropDownTypeSE("OnlineReportTemplates", 13, () => ArmSoft.SME.Common.Resources.OnlineReportsTemplates);

	public static readonly DropDownTypeSE SmartEnum = new DropDownTypeSE("SmartEnum", 14, () => "Smart enum");

	public static readonly DropDownTypeSE StringResources = new DropDownTypeSE("StringResources", 15, () => ArmSoft.SME.Common.Resources.ResourcesCaption);

	public static readonly DropDownTypeSE TreeNodes = new DropDownTypeSE("TreeNodes", 16, () => ArmSoft.SME.Common.Resources.TreeDef);

	public static readonly DropDownTypeSE UsersFiltersByIds = new DropDownTypeSE("UsersFiltersByIds", 17, () => ArmSoft.SME.Common.Resources.Users);

	public static readonly DropDownTypeSE AvailableCurrencies = new DropDownTypeSE("AvailableCurrencies", 18, () => ArmSoft.SME.Common.Resources.Currencies);

	public static readonly DropDownTypeSE AWPDefinitions = new DropDownTypeSE("AWPDefinitions", 19, () => ArmSoft.SME.Common.Resources.AWPDefinitions);

	public static readonly DropDownTypeSE DataViewDefinitions = new DropDownTypeSE("DataViewDefinitions", 20, () => ArmSoft.SME.Common.Resources.DataViewsExtensions);

	public static readonly DropDownTypeSE SingleElements = new DropDownTypeSE("SingleElements", 21, () => "Single elements");

	public static readonly DropDownTypeSE UserScriptsDefinitions = new DropDownTypeSE("UserScriptsDefinitions", 22, () => ArmSoft.SME.Common.Resources.UserScriptsDefinitions);

	public static readonly DropDownTypeSE StdComm = new DropDownTypeSE("StdComm", 23, () => ArmSoft.SME.Common.Resources.StandartComments);

	public const short AccountInBanksConst = 1;

	public const short PartnerAccountInBanksConst = 2;

	public const short TreesConst = 3;

	public const short UsersConst = 4;

	public const short CPACodesConst = 5;

	public const short CurrenciesConst = 6;

	public const short B2BUnitedProductsConst = 7;

	public const short BackupsConst = 8;

	public const short TemplatesConst = 9;

	public const short DatabasesConst = 10;

	public const short DocumentTypesConst = 11;

	public const short DropBoxFilesConst = 12;

	public const short OnlineReportTemplatesConst = 13;

	public const short SmartEnumConst = 14;

	public const short StringResourcesConst = 15;

	public const short TreeNodesConst = 16;

	public const short UsersFiltersByIdsConst = 17;

	public const short AvailableCurrenciesConst = 18;

	public const short AWPDefinitionsConst = 19;

	public const short DataViewDefinitionsConst = 20;

	public const short SingleElementsConst = 21;

	public const short UserScriptsDefinitionsConst = 22;

	public const short StdCommConst = 23;

	public bool CanUsedInExtensions { get; }

	public CacheIdSE CacheId { get; }

	public string ValueUI => Name;

	protected DropDownTypeSE(string name, short value, Func<string> caption = null, bool canUsedInExtensions = false, CacheIdSE cacheId = null)
		: base(name, value, caption)
	{
		CanUsedInExtensions = canUsedInExtensions;
		CacheId = cacheId;
	}
}
