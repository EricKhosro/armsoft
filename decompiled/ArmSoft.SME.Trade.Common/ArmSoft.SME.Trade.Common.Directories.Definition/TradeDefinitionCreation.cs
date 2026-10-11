using System;
using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Trade.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.Directories.Definition;

public static class TradeDefinitionCreation
{
	public static TradeDirectoryDefinition CreateAccountInBank()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.AccountInBank)
		{
			NextCodeParamId = 89,
			HelpId = "Purchases_Lists_Otherlists_AccountsInBank",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.AccountsInBank.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.AccountInBank.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 8, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("SettlementAccount", () => ArmSoft.SME.Common.Resources.SettlementAccount, 22, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Curr", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("NextPayOrdNumber", () => Resources.NextPONum, 12));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsDefault", () => ArmSoft.SME.Common.Resources.Default));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("BankingMode", () => ArmSoft.SME.Common.Resources.ElSupportSystem, typeof(BankingModesSE)));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateBonusScale()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.BonusScale)
		{
			HelpId = "Sales_Lists_BonusScale",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.BonusScale.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new SumFieldDefinition("BeginSumma", () => Resources.StartingSumm));
		tradeDirectoryDefinition.AddFieldDefinition(new SumFieldDefinition("EndSumma", () => Resources.EndSumma, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new PercentFieldDefinition("Percent", () => ArmSoft.SME.Common.Resources.Percent));
		tradeDirectoryDefinition.AddFieldDefinition(new SumFieldDefinition("Point", () => Resources.Point));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateCash()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Cash)
		{
			NextCodeParamId = 90,
			HelpId = "Purchases_Lists_Otherlists_Cash",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashDesks.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Cashes.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 8, isRequired: true, logChanges: true, null, "Code"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true, logChanges: true, null, "Name"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("NextNumIn", () => ArmSoft.SME.Common.Resources.DocNextCashInNumbers, 12, isRequired: false, logChanges: true, null, "NextCashInputOrderNumber"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("NextNumOut", () => ArmSoft.SME.Common.Resources.DocNextCashOutNumbers, 12, isRequired: false, logChanges: true, null, "NextCashOutputOrderNumber"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("NextMTBillNum", () => Resources.DocNextMTBillNumber, 12, isRequired: false, logChanges: true, null, "NextSaleNumber"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("NextMTBillReturnNum", () => Resources.DocNextMTBillReturnNumber, 12, isRequired: false, logChanges: true, null, "NextSaleReturnNumber"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("NextCashierShiftNum", () => Resources.CashierShiftNextNum, 12, isRequired: false, logChanges: true, null, "NextShiftNumber"));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsDefault", () => ArmSoft.SME.Common.Resources.Default, isRequired: false, logChanges: true, null, "Default"));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsECR", () => ArmSoft.SME.Common.Resources.ECR, isRequired: false, logChanges: true, null, "ECR"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ECRSN", () => Resources.ECRSN, 20));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ECRCrn", () => ArmSoft.SME.Common.Resources.ECRCrn, 12));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ECRIP", () => ArmSoft.SME.Common.Resources.ECR + " " + ArmSoft.SME.Common.Resources.IPAddress, 15));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ECRPort", () => ArmSoft.SME.Common.Resources.TCPPort, 5));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ECRMacAddress", () => ArmSoft.SME.Common.Resources.ECRMacAddress, 17));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ECRPassword", () => ArmSoft.SME.Common.Resources.Password, 20));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ECROperator", () => ArmSoft.SME.Common.Resources.ECROperator, 12));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ECROperatorPassword", () => ArmSoft.SME.Common.Resources.ECROperatorPassword, 16));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ECRDepWithVAT", () => Resources.ECRDepWithVAT, 20));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ECRDepWithOutVAT", () => Resources.ECRDepWithOutVAT, 20));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("ShowECRTracking", () => ArmSoft.SME.Common.Resources.ShowPrintTrackingWindow));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("CashSummLimitType", () => ArmSoft.SME.Common.Resources.CashLimit, typeof(CashLimitsSE), isRequired: true, logChanges: true, ValueMember.Code, null, null, "CashLimit"));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateCashierShift()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.CashierShift)
		{
			HelpId = "Purchases_Lists_Otherlists_Shifts",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.CashierShift.Value },
			RelatedCacheIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.CashierShift.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Cashes.Value
			}
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("CashDesk", () => ArmSoft.SME.Common.Resources.Cash, 8, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ShiftNumber", () => Resources.CashierShiftNum, 12, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new DateFieldDefinition("StartDate", () => Resources.CashierShiftStart, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new DateFieldDefinition("EndDate", () => Resources.CashierShiftEnd));
		tradeDirectoryDefinition.AddFieldDefinition(new SumFieldDefinition("InitialRemain", () => Resources.InitRem));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateContract()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Contract)
		{
			HelpId = "Purchases_Lists_Partnerssublists_contracts",
			IsExchangeable = true,
			AllowLog = false,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartContracts.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Partners.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new PartnerFieldDefinition("PartCode", () => ArmSoft.SME.Common.Resources.Partner, isSupplier: false, isCustomer: false, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => Resources.PriceType, 2));
		tradeDirectoryDefinition.AddFieldDefinition(new DateFieldDefinition("ContractDate", () => ArmSoft.SME.Common.Resources.DateWord));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Description", () => Resources.TemplatesDescription, 50));
		tradeDirectoryDefinition.AddFieldDefinition(new PercentFieldDefinition("DiscountPercent", () => ArmSoft.SME.Common.Resources.DiscountPercent));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("ControlReceivables", () => Resources.ControlReceivables));
		tradeDirectoryDefinition.AddFieldDefinition(new SumFieldDefinition("ReceivablesLimit", () => Resources.ReceivablesLimit));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsDefault", () => ArmSoft.SME.Common.Resources.Default));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("DiscountCard", () => Resources.DiscountCardCode, 50));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsBonusCard", () => Resources.BonusCard));
		tradeDirectoryDefinition.AddFieldDefinition(new PercentFieldDefinition("Bonus", () => string.Format(Resources.Bonus, Resources.PercentSymbol)));
		tradeDirectoryDefinition.AddFieldDefinition(new SumFieldDefinition("Point", () => string.Format(Resources.Bonus, Resources.Point)));
		tradeDirectoryDefinition.AddFieldDefinition(new DateFieldDefinition("BonusCardValidPeriodStart", () => Resources.BonusCardValidationPeriod));
		tradeDirectoryDefinition.AddFieldDefinition(new DateFieldDefinition("BonusCardValidPeriodEnd", () => ""));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("BonusCardCanceled", () => Resources.BonusCardCanceled));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateCountry()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Countries)
		{
			NextCodeParamId = 10185,
			HelpId = "Purchases_Lists_Otherlists_Countries",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Countries.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Countries.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateDiscount()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Discount)
		{
			NextCodeParamId = 10129,
			HelpId = "Sales_Lists_Discounts",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Discounts.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Discounts.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 5, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsActive", () => Resources.IsActive));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 255, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("DiscountGroupCode", () => Resources.DiscountGroup, 5));
		tradeDirectoryDefinition.AddFieldDefinition(new DateFieldDefinition("StartDate", () => Resources.Period, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new DateFieldDefinition("EndDate", () => string.Empty));
		tradeDirectoryDefinition.AddFieldDefinition(new SpinFieldDefinition("Priority", () => string.Empty, isRequired: false, 0m, 99m, isNonNegative: true));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("DiscountDocType", () => ArmSoft.SME.Common.Resources.DocumentType, typeof(DiscountDocTypesSE)));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Storage", () => ArmSoft.SME.Common.Resources.Storage, 5));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("UseType", () => Resources.Method, typeof(UsageMethodsSE), isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("GiftGoodCode", () => Resources.Gift, 20));
		GridDefinition gridDefinition = new GridDefinition("DiscountScale", () => Resources.DiscountScale);
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("BeginAmount", () => Resources.StartingSumm));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("EndAmount", () => Resources.EndSumma));
		gridDefinition.AddFieldDefinition(new PercentFieldDefinition("DiscountScalePercent", () => ArmSoft.SME.Common.Resources.Percent));
		tradeDirectoryDefinition.AddGridDefinition(gridDefinition);
		tradeDirectoryDefinition.AddFieldDefinition(new PercentFieldDefinition("Percent", () => ArmSoft.SME.Common.Resources.DiscountPercent));
		tradeDirectoryDefinition.AddFieldDefinition(new NumberFieldDefinition("Amount", () => Resources.DiscountSumm, 16, 0));
		tradeDirectoryDefinition.AddFieldDefinition(new QuantityFieldDefinition("GiftQuantity", () => Resources.GiftQuantity));
		tradeDirectoryDefinition.AddFieldDefinition(new PercentFieldDefinition("GiftDiscountPercent", () => "%"));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("SelectGift", () => Resources.SelectGift));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsDistrInProportion", () => Resources.IsDistrInProportion));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsMultiple", () => Resources.IsMultipleToConditions));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => Resources.DiscountPriceType, 2));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Message", () => Resources.Message, 255));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Segment", () => Resources.Segment, 5));
		tradeDirectoryDefinition.AddFieldDefinition(new ConditionDefinition("DiscountCondition", () => Resources.DiscountConditionTypes)
		{
			Type = new SmartEnumFieldDefinition("Type", () => ArmSoft.SME.Common.Resources.TypeWord, typeof(DiscountConditionTypesSE), isRequired: true),
			Limit = new QuantityFieldDefinition("Limit", () => Resources.ValueGreaterOrEqual, isRequired: true),
			ConditionStart = new DateFieldDefinition("ConditionStart", () => Resources.Period),
			ConditionEnd = new DateFieldDefinition("ConditionEnd", () => string.Empty),
			IncludeCurrentSale = new BoolFieldDefinition("IncludeCurrentSale", () => Resources.IncludeCurrentSale),
			Segment = new StringFieldDefinition("Segment", () => Resources.Segment, 5),
			PartnerGroup = new StringFieldDefinition("PartnerGroup", () => Resources.PartnerGroup, 5),
			IsBirthDayDiscount = new BoolFieldDefinition("IsBirthDayDiscount", () => Resources.BirthDayDiscount),
			BeforBDay = new DateFieldDefinition("BeforBDay", () => Resources.BeforBDay),
			AfterBDay = new DateFieldDefinition("AfterBDay", () => Resources.AfterBDay),
			UseExtension = new BoolFieldDefinition("UseExtension", () => Resources.UseScriptInDiscount)
		});
		tradeDirectoryDefinition.AddFieldDefinition(new DateFieldDefinition("UseDayHours", () => Resources.DiscountDaysHours));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Description", () => ArmSoft.SME.Common.Resources.Description, 255));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateDiscountGroup()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.DiscountGroup)
		{
			NextCodeParamId = 10184,
			HelpId = "Sales_Lists_DiscountsGroups",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.DiscountGroups.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.DiscountGroups.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 5, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("Type", () => Resources.DiscountsMergeMethod, typeof(DiscountsMergeMethodsSE), isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new SpinFieldDefinition("Priority", () => Resources.Priority, isRequired: false, 0m, 99m, isNonNegative: false, typeof(byte)));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateElScaleOrTerminal()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.ElScaleOrTerminal)
		{
			NextCodeParamId = 163,
			HelpId = "Sales_lists_el_scales_terminals(off-line)",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.ElScalesAndTerminals.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsActive", () => Resources.IsActive));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("ElType", () => Resources.DeviceType, typeof(DeviceTypesSE), isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ElModel", () => Resources.DeviceModel, 2, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("AsNameChoose", () => Resources.NameInterpretation, typeof(NameInterpretationsSE)));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("AsCodeExport", () => string.Format(Resources.ExportElScaleNextCodeAs, "\r\n"), typeof(ElScaleExportNextCodesSE)));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Group", () => ArmSoft.SME.Common.Resources.Group, 5));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Segment", () => Resources.Segment, 5));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("PriceList", () => ArmSoft.SME.Common.Resources.PriceList, 2, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ExportForAdditionalQntUnit", () => Resources.ExportForAltUnit, 1));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("DeleteAllItemsBeforeExporting", () => Resources.EraseBeforeExporting));
		tradeDirectoryDefinition.AddFieldDefinition(new DateFieldDefinition("ExportDate", () => Resources.ExportDate));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateEMarkReceipt()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.EMarkReceipt)
		{
			NextCodeParamId = 10200,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.EMarkReceiptApp.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateEmployee()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Employee)
		{
			NextCodeParamId = 88,
			HelpId = "Sales_Lists_Employees",
			IsExchangeable = true,
			AllowLog = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Employees.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Employees.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 12, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.FIO, 50, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Address", () => ArmSoft.SME.Common.Resources.Address, 255));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Phone", () => ArmSoft.SME.Common.Resources.Phone, 50));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Passport", () => ArmSoft.SME.Common.Resources.PassportNum, 25));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("EmployeeCard", () => Resources.DiscountCardCode, 50));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsSalesConsultant", () => Resources.SalesConsultant));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateGood()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods)
		{
			NextCodeParamId = 86,
			HelpId = "Purchases_Lists_ProductsandServices",
			IsExchangeable = true,
			AllowLog = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Goods.Value },
			RelatedCacheIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Goods.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.GoodExtensions.Value
			},
			SqlTableName = "MATERIALS",
			IsExtendable = POExtendableType.Simple
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true, logChanges: true, null, "Code"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 200, isRequired: true, logChanges: true, null, "Name"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("FullName", () => ArmSoft.SME.Common.Resources.FullCaption, 255, isRequired: false, logChanges: true, null, "FullName"));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("Type", () => ArmSoft.SME.Common.Resources.TypeWord, typeof(InvoiceRowTypeSE), isRequired: true, logChanges: true, ValueMember.Code, null, null, "Type"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Group", () => ArmSoft.SME.Common.Resources.Group, 5, isRequired: true, logChanges: true, null, "Group"));
		tradeDirectoryDefinition.AddFieldDefinition(new PartnerFieldDefinition("MainSupplier", () => Resources.MainSupplier, isSupplier: true, isCustomer: false));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("SubstituteItemsGroup", () => Resources.SubstituteItemsGroup, 5, isRequired: false, logChanges: true, null, "SubstituteItemsGroup"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("CPACode", () => ArmSoft.SME.Common.Resources.CPAClassification, 12, isRequired: false, logChanges: true, null, "CPAClassifier"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Unit", () => Resources.BaseQUNIT, 3, isRequired: true, logChanges: true, null, "BaseUnitMeasure"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("AltUnit", () => Resources.ALTQUNIT, 3, isRequired: true, logChanges: true, null, "AlternativeUnitMeasure"));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("ExpenseMethod", () => ArmSoft.SME.Common.Resources.MTExpenseMethod, typeof(MTExpenseMethodsSE), isRequired: false, logChanges: true, ValueMember.Code, null, null, "CostingMethod"));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("ShowInPriceList", () => Resources.ShowInPriceListes, isRequired: false, logChanges: true, null, "ShowInPriceLists"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Description", () => ArmSoft.SME.Common.Resources.Specification, 255, isRequired: false, logChanges: true, null, "Specification"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ExternalCode", () => ArmSoft.SME.Common.Resources.ExternalCode, 20, isRequired: false, logChanges: true, null, "ExternalCode"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("LabelUnit", () => Resources.LabelUnit, 3, isRequired: false, logChanges: true, null, "LabelUnitMeasure"));
		tradeDirectoryDefinition.AddFieldDefinition(new NumberFieldDefinition("LabelCoeff", () => Resources.AltUnitCoeff, 19, 10, isNonNegative: true, isRequired: false, null, logChanges: true, null, "LabelUnitMeasureCoefficient"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Country", () => ArmSoft.SME.Common.Resources.Country, 3, isRequired: false, logChanges: true, null, "Country"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Producer", () => ArmSoft.SME.Common.Resources.Producer, 4, isRequired: false, logChanges: true, null, "Producer"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Property1", () => Resources.Attribute1, 4, isRequired: false, logChanges: true, null, "Property1"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Property2", () => Resources.Attribute2, 4, isRequired: false, logChanges: true, null, "Property2"));
		tradeDirectoryDefinition.AddFieldDefinition(new QuantityFieldDefinition("MinQTY", () => ArmSoft.SME.Common.Resources.MinQty, isRequired: false, isNonNegative: true, logChanges: true, null, "MinimumQuantity"));
		tradeDirectoryDefinition.AddFieldDefinition(new QuantityFieldDefinition("MaxQTY", () => ArmSoft.SME.Common.Resources.MaxQty, isRequired: false, isNonNegative: true, logChanges: true, null, "MaximumQuantity"));
		tradeDirectoryDefinition.AddFieldDefinition(new NumberFieldDefinition("Extra", () => Resources.Extra, 6, 2, isNonNegative: false, isRequired: false, null, logChanges: true, null, "AddedValuePercent"));
		tradeDirectoryDefinition.AddFieldDefinition(new PercentFieldDefinition("Bonus", () => string.Format(Resources.Bonus, Resources.PercentSymbol), isRequired: false, isNonNegative: true, logChanges: true, null, "BonusPercent"));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IgnoreBonus", () => Resources.IgnoreBonus, isRequired: false, logChanges: true, null, "IgnoreBonus"));
		tradeDirectoryDefinition.AddFieldDefinition(new SumFieldDefinition("Point", () => string.Format(Resources.Bonus, ArmSoft.SME.Common.Resources.Unit), isRequired: false, isNonNegative: true, logChanges: true, null, "BonusPoint"));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("VAT", () => ArmSoft.SME.Common.Resources.OptionsVAT, isRequired: false, logChanges: true, null, "VAT"));
		tradeDirectoryDefinition.AddFieldDefinition(new NumberFieldDefinition("EcoFeeRate", () => ArmSoft.SME.Common.Resources.EcoFeeRate, 5, 2, isNonNegative: true, isRequired: false, null, logChanges: true, null, "EnvironmentalFeePercent"));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsWeight", () => ArmSoft.SME.Common.Resources.IsWeight, isRequired: false, logChanges: true, null, "Weight"));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("CheckMarking", () => Resources.CheckMarking, isRequired: false, logChanges: true, null, "CheckMarking"));
		tradeDirectoryDefinition.AddFieldDefinition(new NumberFieldDefinition("Coeff", () => ArmSoft.SME.Common.Resources.OverHeadExpCoeff, 7, 3, isNonNegative: true, isRequired: true, null, logChanges: true, null, "AOECoefficient"));
		tradeDirectoryDefinition.AddFieldDefinition(new PercentFieldDefinition("Discount", () => Resources.Discount + " (%)", isRequired: false, isNonNegative: true, logChanges: true, null, "DiscountPercent"));
		tradeDirectoryDefinition.AddFieldDefinition(new NumberFieldDefinition("PLUCode", () => Resources.PLUCodeInElScale, 9, 0, isNonNegative: true, isRequired: false, typeof(int), logChanges: true, null, "PLUCode"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("BarCode", () => ArmSoft.SME.Common.Resources.BarCode, 20, isRequired: false, logChanges: true, null, "Barcode"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("CaptionEN", () => Resources.MTNameEn, 200, isRequired: false, logChanges: true, null, "NameEN"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("CaptionRU", () => Resources.MTNameRu, 200, isRequired: false, logChanges: true, null, "NameRU"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("AdditionalDescr1", () => Resources.AdditionalDescription + " 1", 255, isRequired: false, logChanges: true, null, "AdditionalSpecification1"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("AdditionalDescr2", () => Resources.AdditionalDescription + " 2", 255, isRequired: false, logChanges: true, null, "AdditionalSpecification2"));
		tradeDirectoryDefinition.AddFieldDefinition(new FieldDefinition("Img", () => Resources.Picture));
		GridDefinition gridDefinition = new GridDefinition("GoodQntUnits", () => ArmSoft.SME.Common.Resources.MeasureUnit, isRequired: false, logChanges: true, null, "MeasureUnits");
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 3, isRequired: true, logChanges: true, null, "Code"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Brief", () => ArmSoft.SME.Common.Resources.Brief, 6, isRequired: false, logChanges: true, null, "Abbreviation"));
		gridDefinition.AddFieldDefinition(new NumberFieldDefinition("Coeff", () => ArmSoft.SME.Common.Resources.Coefficient, 19, 10, isNonNegative: true, isRequired: true, null, logChanges: true, null, "Coefficient"));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("IsDefault", () => ArmSoft.SME.Common.Resources.Main, isRequired: false, logChanges: true, null, "Default"));
		GridDefinition gridDefinition2 = new GridDefinition("GoodBarCodes", () => ArmSoft.SME.Common.Resources.BarCodes, isRequired: false, logChanges: true, null, "Barcodes");
		gridDefinition2.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.QuantityUnitShort, 3, isRequired: true, logChanges: true, null, "UnitMeasure"));
		gridDefinition2.AddFieldDefinition(new StringFieldDefinition("Brief", () => ArmSoft.SME.Common.Resources.Brief, 6, isRequired: false, logChanges: true, null, "Abbreviation"));
		gridDefinition2.AddFieldDefinition(new StringFieldDefinition("BarCode", () => ArmSoft.SME.Common.Resources.BarCode, 20, isRequired: true, logChanges: true, null, "Barcode"));
		tradeDirectoryDefinition.AddGridDefinition(gridDefinition);
		tradeDirectoryDefinition.AddGridDefinition(gridDefinition2);
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateGoodsBySegment()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.GoodsBySegment)
		{
			HelpId = "Purchases_Lists_ProductsAndServicesSublist_ProductsBySegment",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.GoodsBySegment.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => Resources.MaterialOrService, 20, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => Resources.Segment, 5, isRequired: true));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateItemBarCodes()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.MTBarCode)
		{
			HelpId = "Purchases_Lists_Productsandservicessublist_Barcodes",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MTBarCodes.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Goods.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => Resources.MaterialOrService, 20, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("QntUnit", () => ArmSoft.SME.Common.Resources.MeasureUnit, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("BarCode", () => ArmSoft.SME.Common.Resources.BarCode, 20, isRequired: true));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateMTComplects()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.MTComplectsPart)
		{
			HelpId = "Purchases_Lists_Productsandservicessublist_ComplectationList",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MTComplectsParts.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Goods.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("ItemType", () => ArmSoft.SME.Common.Resources.TypeWord, typeof(GoodsTypesSE), isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Complect, 20, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("QntUnit", () => Resources.QntUnit, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new NumberFieldDefinition("Quantity", () => ArmSoft.SME.Common.Resources.Quantity, 13, 6, isNonNegative: true, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new QuantityFieldDefinition("ComplectQuantity", () => ArmSoft.SME.Common.Resources.ComplectQty, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new NumberFieldDefinition("SummaPart", () => Resources.SummaPart, 16, 3));
		tradeDirectoryDefinition.AddFieldDefinition(new NumberFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa, 16, 3));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateMTFavoriteMaterial()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.MTFavoriteMaterial)
		{
			HelpId = "MTFavorites",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Users.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Goods.Value
			}
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("QntUnit", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("GroupCode", () => ArmSoft.SME.Common.Resources.Group, 5));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("HotKey", () => ArmSoft.SME.Common.Resources.HotKey, 15));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateMTFavoritesGroup()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.MTFavoritesGroup)
		{
			NextCodeParamId = 10121,
			HelpId = "MTFavorites",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Users.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Goods.Value
			}
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 5, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("HotKey", () => ArmSoft.SME.Common.Resources.HotKey, 15));
		tradeDirectoryDefinition.AddFieldDefinition(new FieldDefinition("Img", () => Resources.Picture));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateMTGroup()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.MTGroup)
		{
			NextCodeParamId = 134,
			HelpId = "Purchases_Lists_Productsandservicessublist_Productsandservicesgroups",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MTGroups.Value },
			RelatedCacheIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.MTGroup.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Goods.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.PartnerGoods.Value
			}
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 5, isRequired: true, logChanges: true, null, "Code"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true, logChanges: true, null, "Name"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("CaptionEn", () => Resources.MTNameEn, 200, isRequired: false, logChanges: true, null, "NameEN"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("CaptionRu", () => Resources.MTNameRu, 200, isRequired: false, logChanges: true, null, "NameRU"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Parent", () => ArmSoft.SME.Common.Resources.Parent, 5, isRequired: false, logChanges: true, null, "Parent"));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateMTQntUnit()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.MTQntUnit)
		{
			HelpId = "Purchases_Lists_Productsandservicessublist_Productsandservicesunits",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MTQntUnits.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Goods.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("MTCode", () => Resources.MaterialOrService, 20, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => Resources.QntUnit, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("QntUnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6));
		tradeDirectoryDefinition.AddFieldDefinition(new NumberFieldDefinition("Coef", () => Resources.MtCoefLong, 19, 10, isNonNegative: true, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsDefault", () => ArmSoft.SME.Common.Resources.Main));
		tradeDirectoryDefinition.AddFieldDefinition(new FieldDefinition("Image", () => Resources.Picture));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateOrderMinMax()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.OrderMinMax)
		{
			HelpId = "Purchases_Lists_Productsandservicessublist_OrderFormattingParameters",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.OrderMinMax.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("StorageCode", () => ArmSoft.SME.Common.Resources.Storage, 5));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new QuantityFieldDefinition("MinQty", () => ArmSoft.SME.Common.Resources.MinQty));
		tradeDirectoryDefinition.AddFieldDefinition(new QuantityFieldDefinition("MaxQty", () => ArmSoft.SME.Common.Resources.MaxQty));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreatePartner()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Partner)
		{
			NextCodeParamId = 85,
			HelpId = "Purchases_Lists_Partners",
			IsExchangeable = true,
			AllowLog = true,
			AttachmentsTypes = (AttachType.File | AttachType.Link),
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			Grids = new Dictionary<string, GridDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Partners.Value },
			RelatedCacheIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Partners.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.PartnerExtensions.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Cashes.Value
			},
			SqlTableName = "PARTNERS",
			IsExtendable = POExtendableType.Simple
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true, logChanges: true, null, "Code"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true, logChanges: true, null, "Name"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("FullCaption", () => ArmSoft.SME.Common.Resources.FullCaption, 255, isRequired: false, logChanges: true, null, "FullName"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Type", () => ArmSoft.SME.Common.Resources.Group, 5, isRequired: false, logChanges: true, null, "Group"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("TaxCode", () => ArmSoft.SME.Common.Resources.OptionsTaxCode, 20, isRequired: false, logChanges: true, null, "TaxCode"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Address", () => ArmSoft.SME.Common.Resources.LegalAddresses, 255, isRequired: false, logChanges: true, null, "LegalAddress"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("BusinessAddress", () => ArmSoft.SME.Common.Resources.BusinessAddresses, 255, isRequired: false, logChanges: true, null, "BusinessAddress"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Phone", () => ArmSoft.SME.Common.Resources.PhoneNumber, 50, isRequired: false, logChanges: true, null, "PhoneNumber"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Email", () => ArmSoft.SME.Common.Resources.EMailShort, 129, isRequired: false, logChanges: true, null, "Email"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ManagerFIO", () => ArmSoft.SME.Common.Resources.ManagerFIO, 50, isRequired: false, logChanges: true, null, "ManagerName"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ManagerPost", () => ArmSoft.SME.Common.Resources.ManagerPost, 50, isRequired: false, logChanges: true, null, "ManagerPosition"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ChiefaccFIO", () => ArmSoft.SME.Common.Resources.ChAccFIO, 50, isRequired: false, logChanges: true, null, "AccountantName"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ChiefaccPost", () => ArmSoft.SME.Common.Resources.AccountantPost, 50, isRequired: false, logChanges: true, null, "AccountantPosition"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("PartStateRegister", () => Resources.PartStateRegister, 50, isRequired: false, logChanges: true, null, "StateRegisterNumber"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Passport", () => ArmSoft.SME.Common.Resources.PassportNum, 32, isRequired: false, logChanges: true, null, "PassportNumber"));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("IDDocType", () => Resources.IDDocType, typeof(IdentityTypeSE), isRequired: false, logChanges: true, ValueMember.Code, null, null, "IdDocumentType"));
		tradeDirectoryDefinition.AddFieldDefinition(new DateFieldDefinition("Birthday", () => ArmSoft.SME.Common.Resources.DateOfBirth, isRequired: false, logChanges: true, null, "BirthDate"));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("Sex", () => ArmSoft.SME.Common.Resources.Sex, typeof(GenderSE)));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("MainAIM", () => ArmSoft.SME.Common.Resources.PaymentMainAim, 50));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("Supplier", () => ArmSoft.SME.Common.Resources.Supplier, isRequired: false, logChanges: true, null, "Supplier"));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("Customer", () => ArmSoft.SME.Common.Resources.Buyer, isRequired: false, logChanges: true, null, "Customer"));
		GridDefinition gridDefinition = new GridDefinition("PartnerContracts", () => ArmSoft.SME.Common.Resources.Contracts, isRequired: false, logChanges: true, null, "Contracts");
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Contract", () => ArmSoft.SME.Common.Resources.Code, 3, isRequired: true, logChanges: true, null, "Code"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Caption", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true, logChanges: true, null, "Name"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("CurrCode", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true, logChanges: true, null, "Currency"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => Resources.PriceType, 2, isRequired: false, logChanges: true, null, "PriceType"));
		gridDefinition.AddFieldDefinition(new DateFieldDefinition("ContractDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: false, logChanges: true, null, "Date"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("Description", () => Resources.TemplatesDescription, 50, isRequired: false, logChanges: true, null, "Description"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Limit", () => Resources.ReceivablesLimit));
		gridDefinition.AddFieldDefinition(new PercentFieldDefinition("Percent", () => ArmSoft.SME.Common.Resources.DiscountPercent, isRequired: false, isNonNegative: true, logChanges: true, null, "DiscountPercent"));
		gridDefinition.AddFieldDefinition(new StringFieldDefinition("DiscountCard", () => Resources.DiscountCardCode, 50, isRequired: false, logChanges: true, null, "CardCode"));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("IsBonusCard", () => Resources.BonusCard, isRequired: false, logChanges: true, null, "BonusCard"));
		gridDefinition.AddFieldDefinition(new PercentFieldDefinition("Bonus", () => string.Format(Resources.Bonus, Resources.PercentSymbol), isRequired: false, isNonNegative: true, logChanges: true, null, "BonusPercent"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("Point", () => string.Format(Resources.Bonus, ArmSoft.SME.Common.Resources.Unit), isRequired: false, isNonNegative: true, logChanges: true, null, "BonusPoint"));
		gridDefinition.AddFieldDefinition(new DateFieldDefinition("BonusCardValidPeriodStart", () => Resources.BonusCardValidationPeriod + " - " + ArmSoft.SME.Common.Resources.BeginOfPeriodShort, isRequired: false, logChanges: true, null, "ValidationPeriodStart"));
		gridDefinition.AddFieldDefinition(new DateFieldDefinition("BonusCardValidPeriodEnd", () => Resources.BonusCardValidationPeriod + " - " + ArmSoft.SME.Common.Resources.EndOfPeriodShort, isRequired: false, logChanges: true, null, "ValidationPeriodEnd"));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("BonusCardCanceled", () => Resources.BonusCardCanceled, isRequired: false, logChanges: true, null, "Canceled"));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("ControlReceivables", () => Resources.ControlReceivables, isRequired: false, logChanges: true, null, "ReceivablesControl"));
		gridDefinition.AddFieldDefinition(new SumFieldDefinition("ReceivablesLimit", () => Resources.ReceivablesLimit, isRequired: false, isNonNegative: true, logChanges: true, null, "ReceivablesLimit"));
		gridDefinition.AddFieldDefinition(new BoolFieldDefinition("IsDefault", () => ArmSoft.SME.Common.Resources.Default, isRequired: false, logChanges: true, null, "Default"));
		GridDefinition gridDefinition2 = new GridDefinition("SettlementAccounts", () => ArmSoft.SME.Common.Resources.SettlementAccounts, isRequired: false, logChanges: true, null, "SettlementAccounts");
		gridDefinition2.AddFieldDefinition(new StringFieldDefinition("SettlAccount", () => ArmSoft.SME.Common.Resources.Account, 50, isRequired: false, logChanges: true, null, "Account"));
		gridDefinition2.AddFieldDefinition(new StringFieldDefinition("Caption", () => ArmSoft.SME.Common.Resources.Name, 255, isRequired: false, logChanges: true, null, "Name"));
		gridDefinition2.AddFieldDefinition(new BoolFieldDefinition("IsDefault", () => ArmSoft.SME.Common.Resources.Default, isRequired: false, logChanges: true, null, "Default"));
		GridDefinition gridDefinition3 = new GridDefinition("PartnerBusinessAddresses", () => ArmSoft.SME.Common.Resources.BusinessAddress, isRequired: false, logChanges: true, null, "AdditionalAddresses");
		gridDefinition3.AddFieldDefinition(new StringFieldDefinition("Caption", () => ArmSoft.SME.Common.Resources.BusinessAddresses, 255, isRequired: false, logChanges: true, null, "Name"));
		gridDefinition3.AddFieldDefinition(new BoolFieldDefinition("IsDefault", () => ArmSoft.SME.Common.Resources.Default, isRequired: false, logChanges: true, null, "Default"));
		tradeDirectoryDefinition.AddGridDefinition(gridDefinition);
		tradeDirectoryDefinition.AddGridDefinition(gridDefinition2);
		tradeDirectoryDefinition.AddGridDefinition(gridDefinition3);
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreatePartnerAccountInBank()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.PartnerAccountsInBank)
		{
			HelpId = "Purchases_Lists_Partnerssublists_PartnerSettlementAccounts",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartnerAccountsInBank.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Partners.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new PartnerFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Partner, isSupplier: false, isCustomer: false, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("SettlementAccount", () => ArmSoft.SME.Common.Resources.SettlementAccount, 50, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("SettlementAccountCaption", () => ArmSoft.SME.Common.Resources.Name, 255, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsDefault", () => ArmSoft.SME.Common.Resources.Default));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreatePartnerGroup()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.PartnerGroup)
		{
			NextCodeParamId = 131,
			HelpId = "Purchases_Lists_Partnerssublists_PartnerTypes",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartGroups.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.PartnerGroup.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 5, isRequired: true, logChanges: true, null, "Code"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true, logChanges: true, null, "Name"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Parent", () => ArmSoft.SME.Common.Resources.Parent, 5, isRequired: false, logChanges: true, null, "Parent"));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreatePartnersGood()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.PartnersGood)
		{
			HelpId = "Purchases_Lists_ProductsAndServicesSubList_SuppliersMaterials",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PartnersMT.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.PartnerGoods }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new PartnerFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Partner, isSupplier: false, isCustomer: false, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("PartTIN", () => ArmSoft.SME.Common.Resources.OptionsTaxCode, 20));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("Type", () => ArmSoft.SME.Common.Resources.TypeWord, typeof(GoodsTypesSE), isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("PartItemCode", () => ArmSoft.SME.Common.Resources.ExternalCode, 60, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("PartItemCaption", () => ArmSoft.SME.Common.Resources.ExternalName, 255, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("PartItemUnitName", () => ArmSoft.SME.Common.Resources.ExternalUnit, 50, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new NumberFieldDefinition("Coeff", () => ArmSoft.SME.Common.Resources.Coefficient, 10, 4));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ItemCode", () => ArmSoft.SME.Common.Resources.InternalCode, 20));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("ItemUnit", () => ArmSoft.SME.Common.Resources.InternalUnit, 3));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreatePrice()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Price)
		{
			HelpId = "Purchases_Lists_Productsandservicessublist_Prices",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.MTPrices.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.CurrentPrices.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Product, 20, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new DateFieldDefinition("PriceDate", () => ArmSoft.SME.Common.Resources.DateWord, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("PriceType", () => ArmSoft.SME.Common.Resources.PriceListType, 2, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("QntUnit", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Curr", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new SumFieldDefinition("ItemPrice", () => ArmSoft.SME.Common.Resources.Price, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Comment", () => ArmSoft.SME.Common.Resources.Comment, 255));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreatePriceType()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.PriceType)
		{
			NextCodeParamId = 133,
			HelpId = "Purchases_Lists_Productsandservicessublist_PricelistTypes",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PriceTypes.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.PriceTypes }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 2, isRequired: true, logChanges: true, null, "Code"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true, logChanges: true, null, "Name"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Curr", () => ArmSoft.SME.Common.Resources.Curr, 3, isRequired: true, logChanges: true, null, "Currency"));
		tradeDirectoryDefinition.AddFieldDefinition(new SmartEnumFieldDefinition("RoundDegree", () => ArmSoft.SME.Common.Resources.DegreeOfRound, typeof(RoundDegreesSE), isRequired: true, logChanges: true, ValueMember.Code, null, null, "RoundingDegree"));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("IsWithInVAT", () => ArmSoft.SME.Common.Resources.CostWithInVAT, isRequired: false, logChanges: true, null, "IncludingVAT"));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateProducer()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Producers)
		{
			NextCodeParamId = 136,
			HelpId = "Purchases_Lists_Otherlists_Manufacturers",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Makers.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Producers.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 4, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateProperty()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Property)
		{
			NextCodeParamId = 137,
			HelpId = "Purchases_Lists_Otherlists_PropertiesTypes",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PropertiesTypes.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Properties.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 4, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreatePropertyList()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.PropertyList)
		{
			NextCodeParamId = 10183,
			HelpId = "Purchases_Lists_Otherlists_PropertiesValues",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.PropertiesListsValues.Value },
			RelatedCacheIds = new HashSet<short>
			{
				ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Properties.Value,
				ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.PropertiesList.Value
			}
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("DestCode", () => Resources.AttributeType, 4, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateQntUnit()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.QntUnit)
		{
			NextCodeParamId = 139,
			HelpId = "Purchases_Lists_Productsandservicessublist_Units",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.QntUnits.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.QntUnits.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Brief", () => ArmSoft.SME.Common.Resources.Brief, 6, isRequired: true));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateSegment()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Segments)
		{
			NextCodeParamId = 10182,
			HelpId = "Purchases_Lists_ProductsAndServicesSublist_ProductsSegments",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Segments.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Segments.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 5, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateStdCom()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.StdCom)
		{
			NextCodeParamId = 140,
			HelpId = "Purchases_Lists_Otherlists_Comments",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.StandardComments.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.StandardComments.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 3, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateStorage()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Storage)
		{
			NextCodeParamId = 132,
			HelpId = "Purchases_Lists_Storages",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.Storages.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Storages.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 5, isRequired: true, logChanges: true, null, "Code"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true, logChanges: true, null, "Name"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Address", () => ArmSoft.SME.Common.Resources.Address, 255, isRequired: false, logChanges: true, null, "Address"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Response", () => ArmSoft.SME.Common.Resources.Stockkeeper, 50, isRequired: false, logChanges: true, null, "Stockkeeper"));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("IsRetail", () => Resources.RetailWord, 50, isRequired: false, logChanges: true, null, "Retail"));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateSubstituteItemsGroups()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.SubstituteItemsGroups)
		{
			NextCodeParamId = 10084,
			HelpId = "Purchases_Lists_Otherlists_SubstituteItemGroups",
			IsExchangeable = true,
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.SubstituteItemsGroups.Value },
			RelatedCacheIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.SubstituteItemsGroups.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 5, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Name", () => ArmSoft.SME.Common.Resources.Name, 50, isRequired: true));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateUserCashesAccess()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.UserCashesAccess)
		{
			HelpId = "AdminTools_Users",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.UserCashesAccess.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 8, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("Allow", () => Resources.Available));
		return tradeDirectoryDefinition;
	}

	public static TradeDirectoryDefinition CreateUserStorageOptions()
	{
		TradeDirectoryDefinition tradeDirectoryDefinition = new TradeDirectoryDefinition(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.UserStorageOptions)
		{
			HelpId = "AdminTools_Users",
			Fields = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase),
			RelatedProvidersAccessIds = new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.UserAccessIdSE.UserStorageOptions.Value }
		};
		tradeDirectoryDefinition.AddFieldDefinition(new StringFieldDefinition("Code", () => ArmSoft.SME.Common.Resources.Code, 5, isRequired: true));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("AllowOper", () => Resources.OperAvailability));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("AllowReportView", () => Resources.ViewAvailability));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("AllowNegExp", () => Resources.AllowNegativExp));
		tradeDirectoryDefinition.AddFieldDefinition(new BoolFieldDefinition("SalesWithZeroCost", () => Resources.SalesWithZeroCost));
		return tradeDirectoryDefinition;
	}
}
