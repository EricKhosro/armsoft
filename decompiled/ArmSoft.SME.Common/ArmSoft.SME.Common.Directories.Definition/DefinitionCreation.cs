using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Common.Directories.Definition;

public static class DefinitionCreation
{
	public static DirectoryDefinition CreateBank()
	{
		DirectoryDefinition directoryDefinition = new DirectoryDefinition(DirectoryTypeSE.Bank)
		{
			HelpId = "Purchases_Lists_OtherLists_Banks",
			IsExchangeable = true,
			SqlTableName = "BANKAM",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { UserAccessIdSE.Banks.Value },
			RelatedCacheIds = new HashSet<short> { CacheIdSE.Banks.Value }
		};
		directoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => Resources.Code, 5, isRequired: true));
		directoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => Resources.Name, 255, isRequired: true));
		directoryDefinition.AddFieldDefinition(new StringFieldDefinition("SWIFTCODE", () => Resources.SWIFTCODE, 11));
		return directoryDefinition;
	}

	public static DirectoryDefinition CreateCPACodes()
	{
		DirectoryDefinition directoryDefinition = new DirectoryDefinition(DirectoryTypeSE.CPAClassifier)
		{
			HelpId = "Sales_Lists_CPAClassifications",
			IsExchangeable = true,
			SqlTableName = "CPACODES",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { UserAccessIdSE.CPAClassifications.Value },
			RelatedCacheIds = new HashSet<short> { CacheIdSE.CPAClassifiers.Value }
		};
		directoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => Resources.Code, 12, isRequired: true));
		directoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => Resources.Name, 255, isRequired: true));
		directoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("Type", () => Resources.TypeWord, typeof(CPACodesTypeSE), isRequired: true));
		directoryDefinition.AddFieldDefinition(new DateFieldDefinition("MarkInitDate", () => Resources.MarkableFrom));
		return directoryDefinition;
	}

	public static DirectoryDefinition CreateCurrency()
	{
		DirectoryDefinition directoryDefinition = new DirectoryDefinition(DirectoryTypeSE.Currency)
		{
			HelpId = "Directory_Currencies",
			IsExchangeable = true,
			SqlTableName = "CURR",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { UserAccessIdSE.Currencies.Value },
			RelatedCacheIds = new HashSet<short> { CacheIdSE.Currencies.Value }
		};
		directoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => Resources.Code, 3, isRequired: true));
		directoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => Resources.Name, 50, isRequired: true));
		directoryDefinition.AddFieldDefinition(new StringFieldDefinition("UnitName", () => Resources.CurrencyUnit, 10));
		directoryDefinition.AddFieldDefinition(new StringFieldDefinition("CentName", () => Resources.SmallChange, 10));
		return directoryDefinition;
	}

	public static DirectoryDefinition CreateRatesOfExchange()
	{
		DirectoryDefinition directoryDefinition = new DirectoryDefinition(DirectoryTypeSE.RatesOfExchange)
		{
			HelpId = "Purchases_Lists_OtherLists_ExchangeRates",
			IsExchangeable = true,
			SqlTableName = "CURREXCHG",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { UserAccessIdSE.ExchangeRates.Value },
			RelatedCacheIds = new HashSet<short> { CacheIdSE.CurrentExchangeRates.Value }
		};
		directoryDefinition.AddFieldDefinition(new DateFieldDefinition("Date", () => Resources.DateWord, isRequired: true));
		directoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => Resources.Currency, 3, isRequired: true));
		directoryDefinition.AddFieldDefinition(new NumberFieldDefinition("Rate", () => Resources.ExchangeRateShort, 11, 4, isNonNegative: true, isRequired: true));
		directoryDefinition.AddFieldDefinition(new NumberFieldDefinition("Base", () => Resources.At, 5, 0, isNonNegative: true, isRequired: true));
		return directoryDefinition;
	}
}
