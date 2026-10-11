using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(30, "DropDownTypes")]
[SmartEnumPartial(SmartEnumType.Base)]
public class DropDownTypeSE : ASSmartEnum<DropDownTypeSE, short>
{
	public static readonly DropDownTypeSE AccountInBanks = new DropDownTypeSE("AccountInBanks", 1, () => Resources.AccountInBank, canUsedInExtensions: true);

	public static readonly DropDownTypeSE PartnerAccountInBanks = new DropDownTypeSE("PartnerAccountInBanks", 2, () => Resources.PartnerAccountsInBank, canUsedInExtensions: true);

	public static readonly DropDownTypeSE Trees = new DropDownTypeSE("Trees", 3, () => Resources.TreeDefs);

	public static readonly DropDownTypeSE Users = new DropDownTypeSE("Users", 4, () => Resources.Users, canUsedInExtensions: true);

	public static readonly DropDownTypeSE CPACodes = new DropDownTypeSE("CPACodes", 5, () => Resources.CPAClassifications, canUsedInExtensions: true, CacheIdSE.CPAClassifiers);

	public static readonly DropDownTypeSE Currencies = new DropDownTypeSE("Currencies", 6, () => Resources.Curres, canUsedInExtensions: true, CacheIdSE.Currencies);

	public static readonly DropDownTypeSE B2BUnitedProducts = new DropDownTypeSE("B2BUnitedProducts", 7, () => Resources.Unitedb2bCloudGoodsCatalog);

	public static readonly DropDownTypeSE Backups = new DropDownTypeSE("Backups", 8, () => Resources.Backups);

	public static readonly DropDownTypeSE Templates = new DropDownTypeSE("Templates", 9, () => Resources.PrintTemplates);

	public static readonly DropDownTypeSE Databases = new DropDownTypeSE("Databases", 10, () => Resources.Databases);

	public static readonly DropDownTypeSE DocumentTypes = new DropDownTypeSE("DocumentTypes", 11, () => Resources.DocumentTypes);

	public static readonly DropDownTypeSE DropBoxFiles = new DropDownTypeSE("DropBoxFiles", 12, () => Resources.FilesFromDropBox);

	public static readonly DropDownTypeSE OnlineReportTemplates = new DropDownTypeSE("OnlineReportTemplates", 13, () => Resources.OnlineReportsTemplates);

	public static readonly DropDownTypeSE SmartEnum = new DropDownTypeSE("SmartEnum", 14, () => "Smart enum");

	public static readonly DropDownTypeSE StringResources = new DropDownTypeSE("StringResources", 15, () => Resources.ResourcesCaption);

	public static readonly DropDownTypeSE TreeNodes = new DropDownTypeSE("TreeNodes", 16, () => Resources.TreeDef);

	public static readonly DropDownTypeSE UsersFiltersByIds = new DropDownTypeSE("UsersFiltersByIds", 17, () => Resources.Users);

	public static readonly DropDownTypeSE AvailableCurrencies = new DropDownTypeSE("AvailableCurrencies", 18, () => Resources.Currencies);

	public static readonly DropDownTypeSE AWPDefinitions = new DropDownTypeSE("AWPDefinitions", 19, () => Resources.AWPDefinitions);

	public static readonly DropDownTypeSE DataViewDefinitions = new DropDownTypeSE("DataViewDefinitions", 20, () => Resources.DataViewsExtensions);

	public static readonly DropDownTypeSE SingleElements = new DropDownTypeSE("SingleElements", 21, () => "Single elements");

	public static readonly DropDownTypeSE UserScriptsDefinitions = new DropDownTypeSE("UserScriptsDefinitions", 22, () => Resources.UserScriptsDefinitions);

	public static readonly DropDownTypeSE StdComm = new DropDownTypeSE("StdComm", 23, () => Resources.StandartComments);

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
