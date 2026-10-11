using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Parameters;

namespace ArmSoft.SME.Trade.Common.Parameters;

public class Definitions : AllParamDefinitions
{
	public static ParameterDefinition<DateOnly?> InstallDate => new ParameterDefinition<DateOnly?>(3, () => "Resources.InstallDate", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, DateTime.Today.ToDateOnly());

	public static ParameterDefinition<bool> UseMTTree => new ParameterDefinition<bool>(11, () => "Resources.UseMTTree", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<DateOnly?> DateBegin => new ParameterDefinition<DateOnly?>(13, () => ArmSoft.SME.Common.Resources.BeginOfPeriodShort, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, new DateOnly(DateTime.Today.Year, 1, 1));

	public static ParameterDefinition<DateOnly?> DateEnd => new ParameterDefinition<DateOnly?>(14, () => ArmSoft.SME.Common.Resources.EndOfPeriodShort, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, new DateOnly(DateTime.Today.Year, 12, 31));

	public static ParameterDefinition<string> MTCurr => new ParameterDefinition<string>(19, () => Resources.MTCurrency, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "USD");

	public static ParameterDefinition<DateOnly?> ActualnessDate => new ParameterDefinition<DateOnly?>(20, () => Resources.ActualnessDate, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, DateTime.Today.ToDateOnly());

	public static ParameterDefinition<bool> DiscountUse => new ParameterDefinition<bool>(36, () => Resources.DiscountUse, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> CompanyAddress => new ParameterDefinition<string>(51, () => ArmSoft.SME.Common.Resources.LegalAddresses, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> CompanyBusinessAddress => new ParameterDefinition<string>(52, () => ArmSoft.SME.Common.Resources.BusinessAddresses, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> ManagerPost => new ParameterDefinition<string>(54, () => ArmSoft.SME.Common.Resources.ManagerPost, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> ManagerName => new ParameterDefinition<string>(55, () => ArmSoft.SME.Common.Resources.ManagerFIO, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> ChiefAccountantPost => new ParameterDefinition<string>(58, () => ArmSoft.SME.Common.Resources.AccountantPost, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> ChiefAccountantName => new ParameterDefinition<string>(59, () => ArmSoft.SME.Common.Resources.ChAccFIO, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> StateRegister => new ParameterDefinition<string>(62, () => ArmSoft.SME.Common.Resources.OptionsStateRegister, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> CashierName => new ParameterDefinition<string>(65, () => ArmSoft.SME.Common.Resources.OptionCashierFIO, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> PayOrdNumber => new ParameterDefinition<string>(66, () => ArmSoft.SME.Common.Resources.DocPayOrdShort, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> CashInNumber => new ParameterDefinition<string>(67, () => ArmSoft.SME.Common.Resources.DocCashInShort, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> CashOutNumber => new ParameterDefinition<string>(68, () => ArmSoft.SME.Common.Resources.DocCashOutShort, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> MTInvoiceNumber => new ParameterDefinition<string>(69, () => Resources.DocInvoice, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> MTIncomeNumber => new ParameterDefinition<string>(70, () => ArmSoft.SME.Common.Resources.DocMTIncome, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> MTExpenseNumber => new ParameterDefinition<string>(71, () => ArmSoft.SME.Common.Resources.DocMTExpense, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> MTMoveNumber => new ParameterDefinition<string>(72, () => Resources.DocMTMoveShort, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> NextPartner => new ParameterDefinition<string>(85, () => "Resources.NextPartner", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "00000001");

	public static ParameterDefinition<string> NextMaterial => new ParameterDefinition<string>(86, () => "Resources.NextMaterial", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "00001");

	public static ParameterDefinition<string> NextEmployee => new ParameterDefinition<string>(88, () => "Resources.NextEmployee", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "000000000001");

	public static ParameterDefinition<string> NextAccountInBank => new ParameterDefinition<string>(89, () => "Resources.NextAccountInBank", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> NextCash => new ParameterDefinition<string>(90, () => "Resources.NextCash", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> NextBankIn => new ParameterDefinition<string>(121, () => Resources.BankIn, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> NextBankOut => new ParameterDefinition<string>(122, () => Resources.BankOut, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> NextPartnerCredit => new ParameterDefinition<string>(123, () => Resources.PartnerRemCorrection, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> MTComplect => new ParameterDefinition<string>(124, () => Resources.Complection, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "0001");

	public static ParameterDefinition<string> MTDecomplect => new ParameterDefinition<string>(125, () => Resources.Decomplection, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "0001");

	public static ParameterDefinition<string> OverheadExpNumber => new ParameterDefinition<string>(126, () => ArmSoft.SME.Common.Resources.DocMTOverheadExpenses, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> MTReturnToSupNumber => new ParameterDefinition<string>(127, () => Resources.DocMTReturnToSupplier, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> MTInventoryNumber => new ParameterDefinition<string>(128, () => Resources.DocMTInventory, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> Revaluation => new ParameterDefinition<string>(129, () => ArmSoft.SME.Common.Resources.Revaluation, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "0001");

	public static ParameterDefinition<string> NextPartGroup => new ParameterDefinition<string>(131, () => "Resources.NextPartGroup", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "001");

	public static ParameterDefinition<string> NextStorage => new ParameterDefinition<string>(132, () => "Resources.NextStorage", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "001");

	public static ParameterDefinition<string> NextPriceType => new ParameterDefinition<string>(133, () => "Resources.NextPriceType", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "01");

	public static ParameterDefinition<string> NextMTGroup => new ParameterDefinition<string>(134, () => "Resources.NextMTGroup", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "00001");

	public static ParameterDefinition<string> MTRetFromCustNumber => new ParameterDefinition<string>(135, () => Resources.DocMTReturnFromCustomer, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "0001");

	public static ParameterDefinition<string> NextProducer => new ParameterDefinition<string>(136, () => "Resources.NextProducer", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> NextProperty => new ParameterDefinition<string>(137, () => "Resources.NextProperty", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> PhoneNumber => new ParameterDefinition<string>(138, () => ArmSoft.SME.Common.Resources.Phone, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> NextUnit => new ParameterDefinition<string>(139, () => "Resources.NextUnit", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "001");

	public static ParameterDefinition<string> NextStdComm => new ParameterDefinition<string>(140, () => "Resources.NextStdComm", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "001");

	public static ParameterDefinition<bool> ShowPartyUSEABLEDATE => new ParameterDefinition<bool>(150, () => Resources.UseLifeLong, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowPartyCountry => new ParameterDefinition<bool>(151, () => ArmSoft.SME.Common.Resources.Country, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowPartyProducer => new ParameterDefinition<bool>(152, () => ArmSoft.SME.Common.Resources.Producer, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowPartyProperty1 => new ParameterDefinition<bool>(153, () => Resources.Attribute1, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowPartyProperty2 => new ParameterDefinition<bool>(154, () => Resources.Attribute2, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowPartySUPPLIER => new ParameterDefinition<bool>(155, () => ArmSoft.SME.Common.Resources.Supplier, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowPartyCONTRACT => new ParameterDefinition<bool>(156, () => ArmSoft.SME.Common.Resources.Contract, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowPartyDocType => new ParameterDefinition<bool>(157, () => ArmSoft.SME.Common.Resources.DocumentType, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowPartyDocNum => new ParameterDefinition<bool>(158, () => ArmSoft.SME.Common.Resources.DocNumber, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowPartyIncomeDate => new ParameterDefinition<bool>(159, () => ArmSoft.SME.Common.Resources.IncomeDate, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<string> MTBillNumber => new ParameterDefinition<string>(161, () => Resources.DocMTBill, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> NextElScaleOrTerminal => new ParameterDefinition<string>(163, () => Resources.Device, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> MTIncomeImportNumber => new ParameterDefinition<string>(168, () => Resources.DocMTIncomeImport, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<decimal> Extra => new ParameterDefinition<decimal>(169, () => Resources.Extra, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 0m);

	public static ParameterDefinition<string> MTIncomeRetailNumber => new ParameterDefinition<string>(170, () => Resources.DocMTIncomeRetail, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> MTSaleRetailNumber => new ParameterDefinition<string>(171, () => ArmSoft.SME.Common.Resources.DocInvoiceRetail, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<bool> ImportExchangeRates => new ParameterDefinition<bool>(219, () => Resources.ImportExchangeRates, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<string> MTInventoryRetailNumber => new ParameterDefinition<string>(172, () => Resources.DocMTInventoryRetail, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> MTRetailRevaluationNumber => new ParameterDefinition<string>(173, () => Resources.DocMTRevaluationRetail, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> MTIncomeRemNumber => new ParameterDefinition<string>(174, () => Resources.DocMTIncomeRem, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<bool> DraftSelected => new ParameterDefinition<bool>(200, () => ArmSoft.SME.Common.Resources.OptionGroupReRecord, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> Email => new ParameterDefinition<string>(201, () => ArmSoft.SME.Common.Resources.Email, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> PartnersGroupRevaluationNumber => new ParameterDefinition<string>(202, () => Resources.PartnersGroupRevaluation, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> PartnerRemMoveNumber => new ParameterDefinition<string>(341, () => Resources.PartnerRemMove, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> PartnerRemGroupCorrectNumber => new ParameterDefinition<string>(343, () => Resources.PartnerRemGroupCorrection, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> BonusGroupCorrectNumber => new ParameterDefinition<string>(359, () => Resources.BonusGroupCorrect, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> DefaultStorage => new ParameterDefinition<string>(301, () => ArmSoft.SME.Common.Resources.Storage, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> DefaultSupplier => new ParameterDefinition<string>(302, () => ArmSoft.SME.Common.Resources.Supplier, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> DefaultCustomer => new ParameterDefinition<string>(303, () => ArmSoft.SME.Common.Resources.Buyer, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> DefaultBuyingPrice => new ParameterDefinition<string>(304, () => Resources.BuyingPriceType, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> DefaultSalePrice => new ParameterDefinition<string>(305, () => Resources.SalePriceType, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> DefaultMTCheckPriceType => new ParameterDefinition<string>(367, () => Resources.PriceType, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> AllowChangeSalePrices => new ParameterDefinition<bool>(307, () => Resources.DocAllowchangePrices, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AllowChangeMTBillGoodRow => new ParameterDefinition<bool>(310, () => Resources.AllowChangeGoodRow, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<string> DefaultCash => new ParameterDefinition<string>(308, () => ArmSoft.SME.Common.Resources.Cash, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> DefaultMTBillCurr => new ParameterDefinition<string>(309, () => ArmSoft.SME.Common.Resources.Curr, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> CalcDiscountAuto => new ParameterDefinition<bool>(311, () => ArmSoft.SME.Common.Resources.AutoCalcDiscount, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowMTInclInPrList => new ParameterDefinition<bool>(314, () => Resources.ShowMTInclInPriceList, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> DefaultSalesConsultant => new ParameterDefinition<string>(315, () => Resources.SalesConsultant, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> ShowGoodsCostInReportsAndDocs => new ParameterDefinition<bool>(333, () => Resources.ShowGoodsCostInReportsAndDocs, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AllowExceedDebtLimit => new ParameterDefinition<bool>(368, () => Resources.AllowExceedDebtLimit, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> PrintECRCheckAndNew => new ParameterDefinition<bool>(318, () => Resources.PrintECRAndProductCheck, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ECRCheckPrintCreditAsCash => new ParameterDefinition<bool>(352, () => Resources.PrintCreditAsCash, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> ASECRCheckItemName => new ParameterDefinition<string>(330, () => ArmSoft.SME.Common.Resources.ASECRCheckItemName, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> ASECRCheckItemCode => new ParameterDefinition<string>(334, () => ArmSoft.SME.Common.Resources.ASECRCheckItemCode, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> PrintMTCheckAndNew => new ParameterDefinition<bool>(347, () => Resources.PrintECRAndProductCheck, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> MTCheckTemplate => new ParameterDefinition<string>(348, () => ArmSoft.SME.Common.Resources.PrintTemplate + Resources.DocMTBill, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> AllowChangeStorageInBill => new ParameterDefinition<bool>(321, () => Resources.MTBillAllowDifferentStorages, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AllowChangeCashInBill => new ParameterDefinition<bool>(322, () => ArmSoft.SME.Common.Resources.Cash, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AllowChangeBuyerInBill => new ParameterDefinition<bool>(323, () => ArmSoft.SME.Common.Resources.Buyer, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AllowChangeSaleConsultantInBill => new ParameterDefinition<bool>(324, () => Resources.SalesConsultant, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowDiscountPercentColInBill => new ParameterDefinition<bool>(325, () => ArmSoft.SME.Common.Resources.DiscountPercent, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowDiscountedPriceColInBill => new ParameterDefinition<bool>(326, () => ArmSoft.SME.Common.Resources.DiscountedPrice, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowDiscountSummColInBill => new ParameterDefinition<bool>(327, () => Resources.DiscountSumm, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowVATColInBill => new ParameterDefinition<bool>(328, () => ArmSoft.SME.Common.Resources.OptionsVAT, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowPartyColInBill => new ParameterDefinition<bool>(329, () => ArmSoft.SME.Common.Resources.Party, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AllowChangeDateInBill => new ParameterDefinition<bool>(331, () => ArmSoft.SME.Common.Resources.DateWord, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> MTBillSimpleMode => new ParameterDefinition<bool>(332, () => Resources.MTBillSimpleMode, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowLogin => new ParameterDefinition<bool>(335, () => "Resources.AllowLogin", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowMTBillInfoPanel => new ParameterDefinition<bool>(336, () => Resources.ShowInfoPanel, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowPoleDisplay => new ParameterDefinition<bool>(337, () => Resources.PoleDisplay, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowItemPictureOnInfoPanel => new ParameterDefinition<bool>(342, () => Resources.ShowItemPictureOnInfoPanel, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> MTBillGridAutoWidth => new ParameterDefinition<bool>(338, () => Resources.GridColumnAutoWidth, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowAccumulatedBonusColInBill => new ParameterDefinition<bool>(361, () => Resources.AccumulateBonus, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowUsedBonusColInBill => new ParameterDefinition<bool>(362, () => Resources.BonusUse, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> PrintECRCheckOnCheckPrinter => new ParameterDefinition<bool>(339, () => Resources.PrintOnCheckPrinter, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> PrintECRCheckDiscount => new ParameterDefinition<bool>(340, () => Resources.PrintDiscount, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> MTBillAllowDifferentStorages => new ParameterDefinition<bool>(345, () => Resources.MTBillAllowDifferentStorages, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> ReceivedServiceNumber => new ParameterDefinition<string>(346, () => ArmSoft.SME.Common.Resources.BoughtServices, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<int> InternalBarcodePrefix => new ParameterDefinition<int>(349, () => Resources.Prefix, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 27);

	public static ParameterDefinition<bool> InternalBarcodeEANFormat => new ParameterDefinition<bool>(351, () => Resources.InternalBarcodeEANFormat, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> UseBonus => new ParameterDefinition<bool>(353, () => Resources.UseBonus, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> BonusType => new ParameterDefinition<string>(354, () => Resources.BonusType, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "1");

	public static ParameterDefinition<string> BonusAccumulationMethod => new ParameterDefinition<string>(355, () => Resources.BonusAccumulationMethod, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "1");

	public static ParameterDefinition<bool> AccumulateBonusForDiscountedGoods => new ParameterDefinition<bool>(356, () => Resources.AccumulateBonusForDiscountedGoods, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> BonusAccumulationBase => new ParameterDefinition<string>(357, () => Resources.BonusAccumulationBase, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "3");

	public static ParameterDefinition<decimal> BonusAccumulationConditionSumGreater => new ParameterDefinition<decimal>(358, () => Resources.BonusAccumulationConditionSumGreater, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 0m);

	public static ParameterDefinition<DateOnly?> BonusAccumulationConditionPeriodStart => new ParameterDefinition<DateOnly?>(363, () => Resources.BonusAccumulationConditionPeriod, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, null);

	public static ParameterDefinition<DateOnly?> BonusAccumulationConditionPeriodEnd => new ParameterDefinition<DateOnly?>(364, () => "Resources.BonusAccumulationConditionPeriodEnd", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, null);

	public static ParameterDefinition<bool> AllowChangeDiscountPrice => new ParameterDefinition<bool>(365, () => Resources.AllowChangeDiscountPrice, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<string> ScannerMode => new ParameterDefinition<string>(366, () => Resources.ScannerReadMode, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "1");

	public static ParameterDefinition<int> LoginQtyParam => new ParameterDefinition<int>(600, () => "Resources.LoginQtyParam", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 0);

	public static ParameterDefinition<int> CompanyInfoSentToB2B => new ParameterDefinition<int>(601, () => "Resources.CompanyInfoSentToB2B", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 0);

	public static ParameterDefinition<DateOnly?> CompanyInfoSentToB2BDate => new ParameterDefinition<DateOnly?>(602, () => "Resources.CompanyInfoSentToB2BDate", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, null);

	public static ParameterDefinition<int> B2BActorId => new ParameterDefinition<int>(603, () => "Resources.B2BActorId", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 0);

	public static ParameterDefinition<string> B2BActorName => new ParameterDefinition<string>(604, () => "Resources.B2BActorName", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> MappingEInvCode => new ParameterDefinition<string>(605, () => "Resources.MappingEInvCode", userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "00001");

	public static ParameterDefinition<decimal> MapLatitude => new ParameterDefinition<decimal>(606, () => ArmSoft.SME.Common.Resources.Latitude, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 0m);

	public static ParameterDefinition<decimal> MapLongitude => new ParameterDefinition<decimal>(607, () => ArmSoft.SME.Common.Resources.Longitude, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 0m);

	public static ParameterDefinition<string> MTPurchaseOrderNumber => new ParameterDefinition<string>(608, () => ArmSoft.SME.Common.Resources.DocMTPurchaseOrder, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "001");

	public static ParameterDefinition<string> InvoiceForPayNumber => new ParameterDefinition<string>(10001, () => ArmSoft.SME.Common.Resources.InvoiceForPay, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "001");

	public static ParameterDefinition<bool> EnvironmentalFeePayer => new ParameterDefinition<bool>(10002, () => ArmSoft.SME.Common.Resources.EnvironmentalFeePayer, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, defaultValue: false);

	public static ParameterDefinition<bool> DenyAccessToOtherOwnerDocs => new ParameterDefinition<bool>(10003, () => Resources.DenyAccessToOtherOwnerDocs, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> DiscountExtensionUse => new ParameterDefinition<bool>(10004, () => Resources.UseDiscountExtension, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowRowDescrColInBill => new ParameterDefinition<bool>(10005, () => ArmSoft.SME.Common.Resources.Description, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowPartnerTaxCode => new ParameterDefinition<bool>(10006, () => ArmSoft.SME.Common.Resources.ShowPartnerTaxCode, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> ReconcilationStatementNumber => new ParameterDefinition<string>(10007, () => ArmSoft.SME.Common.Resources.ReconciliationStatement, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "001");

	public static ParameterDefinition<string> MTBillInputValueHotKey => new ParameterDefinition<string>(10008, () => Resources.InputBoxCaption, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "F2");

	public static ParameterDefinition<string> MTBillSummaCashHotKey => new ParameterDefinition<string>(10009, () => Resources.CashPaymentForPrint, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "F6");

	public static ParameterDefinition<string> MTBillWeightHotKey => new ParameterDefinition<string>(10010, () => Resources.Weight, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "Shift+F9");

	public static ParameterDefinition<string> MTBillSalePriceHotKey => new ParameterDefinition<string>(10011, () => Resources.SalePriceShort, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "Shift+F8");

	public static ParameterDefinition<string> MTBillDiscountHotKey => new ParameterDefinition<string>(10012, () => Resources.GiveDiscount, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> MTBillComplectHotKey => new ParameterDefinition<string>(10013, () => ArmSoft.SME.Common.Resources.Complect, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> MTBillBarCodeEnterHotKey => new ParameterDefinition<string>(10014, () => Resources.cmdBarCodeEnter, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "F7");

	public static ParameterDefinition<string> MTBillCodeEnterHotKey => new ParameterDefinition<string>(10015, () => Resources.cmdCodeEnter, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "F8");

	public static ParameterDefinition<string> MTBillMTListLikeNameHotKey => new ParameterDefinition<string>(10016, () => Resources.cmdNameEnter, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "F9");

	public static ParameterDefinition<string> MTBillQtyEnterHotKey => new ParameterDefinition<string>(10017, () => Resources.cmdQtyEnter, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "Ctrl+F9");

	public static ParameterDefinition<string> MTBillPrintBillHotKey => new ParameterDefinition<string>(10018, () => ArmSoft.SME.Common.Resources.Print, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "F11");

	public static ParameterDefinition<string> MTBillPrintECRCheckHotKey => new ParameterDefinition<string>(10019, () => ArmSoft.SME.Common.Resources.ECRC, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "F3");

	public static ParameterDefinition<string> MTBillPlCardPaymentHotKey => new ParameterDefinition<string>(10020, () => Resources.PlasticCartPayment, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "F4");

	public static ParameterDefinition<string> MTBillCreditHotKey => new ParameterDefinition<string>(10021, () => Resources.CreditSaleShort, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "Ctrl+F11");

	public static ParameterDefinition<string> MTBillPricesHotKey => new ParameterDefinition<string>(10022, () => ArmSoft.SME.Common.Resources.Prices, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> MTBillUseBonusHotKey => new ParameterDefinition<string>(10023, () => Resources.BonusUseShort, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> MTBillCalcBonusHotKey => new ParameterDefinition<string>(10024, () => Resources.CalcBonusesShort, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> MTBillUsePrePaymentHotKey => new ParameterDefinition<string>(10025, () => Resources.UsePrePaymentShort, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> MTBillSetFocusOnGridHotKey => new ParameterDefinition<string>(10026, () => Resources.SetFocusOnGrid, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "Ctrl+1");

	public static ParameterDefinition<string> MTBillFilteredSearchHotKey => new ParameterDefinition<string>(10027, () => Resources.GroupAddition, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> MTBillFavoritesHotKey => new ParameterDefinition<string>(10028, () => ArmSoft.SME.Common.Resources.RibbonPageFavorites, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> MTRevaluationNumber => new ParameterDefinition<string>(10029, () => Resources.MaterialRevaluation, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false);

	public static ParameterDefinition<string> InternalBarcodeNext => new ParameterDefinition<string>(10030, () => Resources.InternalBarcodeNext, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "00001");

	public static ParameterDefinition<bool> ActualnessDateChangeMode => new ParameterDefinition<bool>(10031, () => ArmSoft.SME.Common.Resources.Unknown, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> UseCurrentDateInDocs => new ParameterDefinition<bool>(10032, () => Resources.UseCurrentDate, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowChangeDiscountInBill => new ParameterDefinition<bool>(10033, () => Resources.DiscountPercentSymbol, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AllowChangeDocNumberInBill => new ParameterDefinition<bool>(10034, () => ArmSoft.SME.Common.Resources.DocNumber, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowASInfoInMTBill => new ParameterDefinition<bool>(10035, () => ArmSoft.SME.Common.Resources.ShowASInfo, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ECRCheckPrintBonusAsCash => new ParameterDefinition<bool>(10036, () => Resources.PrintBonusAsCash, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowDiscountInPurchase => new ParameterDefinition<bool>(10038, () => Resources.AllowDiscountInPurchase, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowUse100PercentSale => new ParameterDefinition<bool>(10039, () => Resources.AllowUse100PercentSale, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> MTBillUsePrePaymentShowButton => new ParameterDefinition<bool>(10040, () => Resources.UsePrePaymentShort, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> MTBillCreditShowButton => new ParameterDefinition<bool>(10041, () => Resources.CreditSaleShort, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> MTBillPricesShowButton => new ParameterDefinition<bool>(10042, () => ArmSoft.SME.Common.Resources.Prices, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> MTBillPlCardPaymentShowButton => new ParameterDefinition<bool>(10043, () => Resources.PlasticCartPayment, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> MTBillPrintECRCheckShowButton => new ParameterDefinition<bool>(10044, () => ArmSoft.SME.Common.Resources.ECRC, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> MTBillPrintBillShowButton => new ParameterDefinition<bool>(10045, () => ArmSoft.SME.Common.Resources.Print, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> MTBillComplectShowButton => new ParameterDefinition<bool>(10046, () => ArmSoft.SME.Common.Resources.Complect, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> MTBillFilteredSearchShowButton => new ParameterDefinition<bool>(10047, () => Resources.GroupAddition, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> MTBillFavoritesShowButton => new ParameterDefinition<bool>(10048, () => ArmSoft.SME.Common.Resources.RibbonPageFavorites, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> DBParticipateInETL => new ParameterDefinition<bool>(10049, () => ArmSoft.SME.Common.Resources.DBParticipateInETL, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ForETLExportDefaultValue => new ParameterDefinition<bool>(10050, () => ArmSoft.SME.Common.Resources.ETLExportDefaultValue, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowChangeETLStateInBill => new ParameterDefinition<bool>(10051, () => ArmSoft.SME.Common.Resources.ETLState, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> BonusExtensionUse => new ParameterDefinition<bool>(10052, () => Resources.UseBonusExtension, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowChangePriceType => new ParameterDefinition<bool>(10053, () => Resources.AllowChangePriceType, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<string> MTBillErasePaymentsHotKey => new ParameterDefinition<string>(10054, () => Resources.ErasePayments, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> ShowUnitCodeColInBill => new ParameterDefinition<bool>(10055, () => ArmSoft.SME.Common.Resources.Unit, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowStorageAvailabilityOnInfoPanel => new ParameterDefinition<bool>(10056, () => Resources.ShowStorageAvailabilityOnInfoPanel, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowWholeAvailabilityOnInfoPanel => new ParameterDefinition<bool>(10057, () => Resources.ShowWholeAvailabilityOnInfoPanel, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> MTBillRecordAllEntries => new ParameterDefinition<bool>(10058, () => Resources.MTBillRecordAllEntries, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> MTBillRoundDegree => new ParameterDefinition<string>(10059, () => Resources.DocRoundDegree, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> MTBillSummRoundUp => new ParameterDefinition<bool>(10060, () => Resources.AlwaysRoundUp, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> SourceServerName => new ParameterDefinition<string>(10061, () => ArmSoft.SME.Common.Resources.Server, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> SourceDataBaseName => new ParameterDefinition<string>(10062, () => ArmSoft.SME.Common.Resources.Database, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> SourceUserName => new ParameterDefinition<string>(10063, () => ArmSoft.SME.Common.Resources.User, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> SourceUserPassword => new ParameterDefinition<string>(10064, () => ArmSoft.SME.Common.Resources.Password, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> AllowPOSRecordReturnFromCustomer => new ParameterDefinition<bool>(10065, () => Resources.AllowMakeReturnDocument, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowPOSChangePrice => new ParameterDefinition<bool>(10066, () => Resources.AllowEditGoodsPrice, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowPOSDeleteRow => new ParameterDefinition<bool>(10067, () => Resources.AllowDeleteRow, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowPOSDecreaseQuantity => new ParameterDefinition<bool>(10068, () => Resources.AllowDecreaseQuantity, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowPOSInputFromKeyboard => new ParameterDefinition<bool>(10069, () => Resources.AllowInputFromKeyboard, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowPOSRemoveCard => new ParameterDefinition<bool>(10070, () => Resources.AllowRemoveCard, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> NoCalcBonusesWhenRePay => new ParameterDefinition<bool>(10071, () => Resources.NoCalcBonusesWhenRePay, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> NotAllowCloseCashierShiftWhenHavePostponedDocs => new ParameterDefinition<bool>(10072, () => Resources.AllowCloseCashierShiftWhenNotHavePosponedSales, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> POSPartnerCardHotKey => new ParameterDefinition<string>(10073, () => "string.Format({0} {1}, Resources.Buyers, Resources.Card)", userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> POSReturnHotKey => new ParameterDefinition<string>(10074, () => Resources.ReturnWord, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> POSContinueHotKey => new ParameterDefinition<string>(10075, () => Resources.Continue, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> MTBillAddToBonusHotKey => new ParameterDefinition<string>(10076, () => Resources.AddToBonusShort, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> ECRDiscountType => new ParameterDefinition<string>(10077, () => Resources.DiscountType, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> ConnectTob2barmsoftDiscountServer => new ParameterDefinition<bool>(10078, () => Resources.ConnectTob2barmsoftDiscountServer, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AddMaterialToPartnersMT => new ParameterDefinition<bool>(10079, () => Resources.AddMaterialToPartnersMT, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> MTMoveOrderNumber => new ParameterDefinition<string>(10080, () => Resources.MTMoveOrder, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "001");

	public static ParameterDefinition<bool> ShowDescriptionOnInfoPanel => new ParameterDefinition<bool>(10081, () => Resources.ShowDescriptionOnInfoPanel, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<DateOnly?> PartnerDataReceivedDateFromb2b => new ParameterDefinition<DateOnly?>(10082, () => ArmSoft.SME.Common.Resources.PartnerStatisticUpdatedFromb2b, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> ECRCheckPrintCreditAsPartial => new ParameterDefinition<bool>(10083, () => Resources.PrintCreditAsCompensation, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> NextSubstituteItemGroup => new ParameterDefinition<string>(10084, () => ArmSoft.SME.Common.Resources.Unknown, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "00001");

	public static ParameterDefinition<bool> EnableSubstituteItems => new ParameterDefinition<bool>(10085, () => Resources.SubstituteItems, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> MTBillSubstituteItemsHotKey => new ParameterDefinition<string>(10086, () => Resources.SubstituteItems, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> MTBillSubstituteItemsShowButton => new ParameterDefinition<bool>(10087, () => Resources.SubstituteItems, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<string> PayXMerchant => new ParameterDefinition<string>(10088, () => ArmSoft.SME.Common.Resources.User, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> PayXToken => new ParameterDefinition<string>(10089, () => ArmSoft.SME.Common.Resources.Password, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> ActivatePayX => new ParameterDefinition<bool>(10090, () => ArmSoft.SME.Common.Resources.TemplateEnabled, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> PayXPrintTemplate => new ParameterDefinition<string>(10091, () => ArmSoft.SME.Common.Resources.PrintTemplate, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "105");

	public static ParameterDefinition<bool> ActivateIdram => new ParameterDefinition<bool>(10092, () => ArmSoft.SME.Common.Resources.TemplateEnabled, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> IdramPrintTemplate => new ParameterDefinition<string>(10093, () => ArmSoft.SME.Common.Resources.PrintTemplate, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "105");

	public static ParameterDefinition<string> IdramMerchant => new ParameterDefinition<string>(10094, () => ArmSoft.SME.Common.Resources.User, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> IdramToken => new ParameterDefinition<string>(10095, () => ArmSoft.SME.Common.Resources.Password, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> ActivateInecoPay => new ParameterDefinition<bool>(10096, () => ArmSoft.SME.Common.Resources.TemplateEnabled, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> InecoPayPrintTemplate => new ParameterDefinition<string>(10097, () => ArmSoft.SME.Common.Resources.PrintTemplate, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "105");

	public static ParameterDefinition<string> ECRCheckPrintBonusType => new ParameterDefinition<string>(10098, () => Resources.PrintBonusAs, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> InecoPayToken => new ParameterDefinition<string>(10099, () => ArmSoft.SME.Common.Resources.Token, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> ActivateEasyPay => new ParameterDefinition<bool>(10100, () => ArmSoft.SME.Common.Resources.TemplateEnabled, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> EasyPayPrintTemplate => new ParameterDefinition<string>(10101, () => ArmSoft.SME.Common.Resources.PrintTemplate, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "105");

	public static ParameterDefinition<string> EasyPayMerchant => new ParameterDefinition<string>(10102, () => ArmSoft.SME.Common.Resources.User, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> EasyPayToken => new ParameterDefinition<string>(10103, () => ArmSoft.SME.Common.Resources.Password, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> MTBillQRHotKey => new ParameterDefinition<string>(10104, () => ArmSoft.SME.Common.Resources.QRCode, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> MTBillQRShowButton => new ParameterDefinition<bool>(10105, () => ArmSoft.SME.Common.Resources.QRCode, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> MaterialCheckRepetition => new ParameterDefinition<bool>(10106, () => Resources.MaterialCheckRepetition, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> DefaultMTCheckDiscountPriceType => new ParameterDefinition<string>(10107, () => Resources.DiscountPriceType, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> AllowChangeDiscountPriceType => new ParameterDefinition<bool>(10108, () => Resources.AllowChangeDiscountPriceType, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<string> TelcellMerchant => new ParameterDefinition<string>(10113, () => ArmSoft.SME.Common.Resources.User, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> TelcellToken => new ParameterDefinition<string>(10114, () => ArmSoft.SME.Common.Resources.Password, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> ActivateTelcell => new ParameterDefinition<bool>(10115, () => ArmSoft.SME.Common.Resources.TemplateEnabled, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> TelcellPrintTemplate => new ParameterDefinition<string>(10116, () => ArmSoft.SME.Common.Resources.PrintTemplate, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "105");

	public static ParameterDefinition<bool> TelcellSellerScans => new ParameterDefinition<bool>(10117, () => ArmSoft.SME.Common.Resources.SellerScans, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<decimal> BonusUseMaxPercent => new ParameterDefinition<decimal>(10118, () => Resources.BonusUseMaxPercent, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 100m);

	public static ParameterDefinition<bool> AllowPurchaseWith0PriceOr100Percent => new ParameterDefinition<bool>(10119, () => Resources.AllowPurchaseWith0PriceOr100Percent, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ExportArmsoftIdentifier => new ParameterDefinition<bool>(10120, () => Resources.ExportArmsoftIdentifier, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> NextFavGroup => new ParameterDefinition<string>(10121, () => ArmSoft.SME.Common.Resources.Unknown, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "00001");

	public static ParameterDefinition<bool> AllowPOSCashInOut => new ParameterDefinition<bool>(10122, () => Resources.AllowCashInOut, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> DefaultTransitCash => new ParameterDefinition<string>(10123, () => Resources.TransitCashDesk, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> MTBillReturnNumber => new ParameterDefinition<string>(10124, () => Resources.DocMTBillReturn, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "001");

	public static ParameterDefinition<string> RetCheckTemplate => new ParameterDefinition<string>(10125, () => ArmSoft.SME.Common.Resources.PrintTemplate, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> PartnerEmployeeCardCodePrefixes => new ParameterDefinition<string>(10126, () => Resources.PartnerEmployeeCardCodePrefix, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> UsePartnerEmployeeSearchNewMode => new ParameterDefinition<bool>(10127, () => Resources.UsePartnerEmployeeCardcodePrefix, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AlwaysCalcDiscountAuto => new ParameterDefinition<bool>(10128, () => Resources.AlwaysCalcDiscountAuto, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> NextDiscount => new ParameterDefinition<string>(10129, () => ArmSoft.SME.Common.Resources.Code, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "001");

	public static ParameterDefinition<bool> AllowChangeECRCheckNum => new ParameterDefinition<bool>(10130, () => ArmSoft.SME.Common.Resources.AllowChangeECRCheckNum, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<int> PartnerCardLenght => new ParameterDefinition<int>(10131, () => Resources.PartnerCardCodeLenght, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 1);

	public static ParameterDefinition<bool> MTBillReturnBillShowButton => new ParameterDefinition<bool>(10132, () => Resources.ReturnWord, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> UsePartnerCardLenght => new ParameterDefinition<bool>(10133, () => Resources.UsePartnerCardCodeLenght, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowGroupDelete => new ParameterDefinition<bool>(10134, () => Resources.AllowGroupDelete, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> DiscountsMergeMethod => new ParameterDefinition<string>(10135, () => Resources.DiscountsMergeMethod, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "1");

	public static ParameterDefinition<bool> AllowPOSPrePaymentECRCheck => new ParameterDefinition<bool>(10136, () => Resources.AllowPOSPrePaymentECRCheck, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowPOSChooseBuyer => new ParameterDefinition<bool>(10137, () => Resources.AllowPOSChooseBuyer, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> DefaultDiscountPrice => new ParameterDefinition<string>(10138, () => Resources.DiscountPriceType, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> SyncContractsWithb2barmsoftDiscountServer => new ParameterDefinition<bool>(10139, () => Resources.SyncContractsWithb2barmsoftDiscountServer, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowDateInMTBill => new ParameterDefinition<bool>(10140, () => ArmSoft.SME.Common.Resources.DateWord, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowDocNumInMTBill => new ParameterDefinition<bool>(10141, () => ArmSoft.SME.Common.Resources.DocNumber, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowDiscountInMTBill => new ParameterDefinition<bool>(10142, () => Resources.DiscountPercentSymbol, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowStorageInMTBill => new ParameterDefinition<bool>(10143, () => ArmSoft.SME.Common.Resources.Storage, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowCashInMTBill => new ParameterDefinition<bool>(10144, () => ArmSoft.SME.Common.Resources.Cash, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowCashierShiftNumInMTBill => new ParameterDefinition<bool>(10145, () => Resources.CashierShiftNum, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AllowChangeCashierShiftNumInMTBill => new ParameterDefinition<bool>(10146, () => Resources.CashierShiftNum, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowPrintExpJustifyingCheckInMTBill => new ParameterDefinition<bool>(10147, () => ArmSoft.SME.Common.Resources.PrintExpenseJustifyingCheck, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AllowChangePrintExpJustifyingCheckInMTBill => new ParameterDefinition<bool>(10148, () => ArmSoft.SME.Common.Resources.PrintExpenseJustifyingCheck, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowSalesConsultantInMTBill => new ParameterDefinition<bool>(10149, () => Resources.SalesConsultant, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowCommentInMTBill => new ParameterDefinition<bool>(10150, () => ArmSoft.SME.Common.Resources.Comment, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AllowChangeCommentInMTBill => new ParameterDefinition<bool>(10151, () => ArmSoft.SME.Common.Resources.Comment, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowETLStateInMTBill => new ParameterDefinition<bool>(10152, () => ArmSoft.SME.Common.Resources.ETLState, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowOwnerInMTBill => new ParameterDefinition<bool>(10153, () => ArmSoft.SME.Common.Resources.DocOwner, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AllowChangeOwnerInMTBill => new ParameterDefinition<bool>(10154, () => ArmSoft.SME.Common.Resources.DocOwner, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> ShowECRCheckNumInMTBill => new ParameterDefinition<bool>(10155, () => ArmSoft.SME.Common.Resources.Unknown, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> OnlineShopEnableExport => new ParameterDefinition<bool>(10156, () => Resources.EnableExport, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<int> ExportInterval => new ParameterDefinition<int>(10157, () => Resources.ExportInterval, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 5);

	public static ParameterDefinition<string> RemainsStorage => new ParameterDefinition<string>(10158, () => Resources.RemainsStorages, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> Segment => new ParameterDefinition<string>(10159, () => Resources.Segment, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> RegularPriceType => new ParameterDefinition<string>(10160, () => Resources.SalePriceType, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<string> DiscountedPriceType => new ParameterDefinition<string>(10161, () => Resources.DiscountPriceType, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<DateOnly?> GoodRemsLastSyncDate => new ParameterDefinition<DateOnly?>(10162, () => ArmSoft.SME.Common.Resources.Unknown, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, null);

	public static ParameterDefinition<bool> AllowToRegisterNewBuyer => new ParameterDefinition<bool>(10163, () => Resources.AllowToRegisterNewBuyer, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> PrintECRAndProductCheck => new ParameterDefinition<bool>(10164, () => Resources.PrintECRAndProductCheck, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowToAddNewBuyerWithNotRegisteredBuyerCard => new ParameterDefinition<bool>(10165, () => Resources.AllowToAddNewBuyerWithNotRegisteredBuyerCard, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowCashDeskAmount => new ParameterDefinition<bool>(10166, () => Resources.ShowCashDeskAmount, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<int> CacheAutoupdatePeriod => new ParameterDefinition<int>(10167, () => ArmSoft.SME.Common.Resources.Unknown, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 15);

	public static ParameterDefinition<string> ActualQuantitiesCheckHotKey => new ParameterDefinition<string>(10168, () => Resources.ActualQuantitiesCheck, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> ActualQuantitiesCheckShowButton => new ParameterDefinition<bool>(10169, () => Resources.ActualQuantitiesCheck, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowLastPurchasePrices => new ParameterDefinition<bool>(10170, () => Resources.ShowLastPurchasePrices, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> RetailAccounting => new ParameterDefinition<bool>(10171, () => Resources.RetailAccounting, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowPrintMTCheck => new ParameterDefinition<bool>(10172, () => Resources.AllowPrintMTCheck, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AllowPOSShowAvailability => new ParameterDefinition<bool>(10173, () => Resources.AllowShowAvailability, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> UseMarking => new ParameterDefinition<bool>(10174, () => ArmSoft.SME.Common.Resources.UseMarking, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<decimal> VATIncluded => new ParameterDefinition<decimal>(10175, () => ArmSoft.SME.Common.Resources.CostWithInVAT, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 16.67m);

	public static ParameterDefinition<string> VATTypeRetail => new ParameterDefinition<string>(10176, () => ArmSoft.SME.Common.Resources.VATTypeRetail, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "2");

	public static ParameterDefinition<bool> ShowOnlyUserAWP => new ParameterDefinition<bool>(10177, () => Resources.ShowOnlyUserAWP, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowQuickStartCentre => new ParameterDefinition<bool>(10178, () => ArmSoft.SME.Common.Resources.ShowQuickStartCenter, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> CheckActualnessDateForReports => new ParameterDefinition<bool>(10179, () => ArmSoft.SME.Common.Resources.CheckActualnessDateForReports, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> OpenMarkingWindowImmediatly => new ParameterDefinition<bool>(10180, () => ArmSoft.SME.Common.Resources.OpenMarkingWindow, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> CheckMarkingAndBarCode => new ParameterDefinition<bool>(10181, () => Resources.CheckMarkingAndBarCode, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<string> NextSegment => new ParameterDefinition<string>(10182, () => Resources.Segment, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> NextPropertyList => new ParameterDefinition<string>(10183, () => Resources.PropertiesListsValues, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> NextDiscountGroup => new ParameterDefinition<string>(10184, () => Resources.DiscountGroup, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");

	public static ParameterDefinition<string> NextCountry => new ParameterDefinition<string>(10185, () => ArmSoft.SME.Common.Resources.Country, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "001");

	public static ParameterDefinition<bool> PriceRecalcFromSumm => new ParameterDefinition<bool>(10186, () => Resources.RecalcQty, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> InFavoritesFirtsGroups => new ParameterDefinition<bool>(10187, () => Resources.InFavoritesFirtsGroups, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> ShowBarCodeInAvailList => new ParameterDefinition<bool>(10188, () => Resources.ShowBarCodeInGoodsSelectList, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> AllowCardViewSizeChange => new ParameterDefinition<bool>(10189, () => Resources.AllowChangeCardViewSize, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> UnitCodeSwitchHotKey => new ParameterDefinition<string>(10190, () => Resources.ChangeUnit, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> AllowPOSFullDeleteRow => new ParameterDefinition<bool>(10191, () => Resources.AllowDeleteFullRow, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> GoodAddSimpleMode => new ParameterDefinition<bool>(10192, () => Resources.GoodAddSimpleMode, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<int> DiscountServerSyncInterval => new ParameterDefinition<int>(10193, () => Resources.SyncInterval, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, 5);

	public static ParameterDefinition<string> DealType => new ParameterDefinition<string>(10194, () => ArmSoft.SME.Common.Resources.DealType, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> SaveAndPrintECR => new ParameterDefinition<bool>(10195, () => Resources.SaveAndAutoPrintECR, userParam: true, roleParam: false, useFromCacheService: true, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> CPACheckMode => new ParameterDefinition<string>(10196, () => string.Format(ArmSoft.SME.Common.Resources.EInvoicingClassifierCheckLevel, Environment.NewLine), userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "2");

	public static ParameterDefinition<int> OfflineDocumentsKeepDaysCount => new ParameterDefinition<int>(10197, () => ArmSoft.SME.Common.Resources.OfflineDocumentsKeepDaysCount, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: true, 30);

	public static ParameterDefinition<string> EMarkReceiptDevice => new ParameterDefinition<string>(10198, () => Resources.EMarkReceiptDevice, userParam: true, roleParam: false, useFromCacheService: true, useFromCacheUI: true);

	public static ParameterDefinition<string> NextEMarkReceiptDoc => new ParameterDefinition<string>(10199, () => Resources.EMarkReceiptDoc, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: true, "0001");

	public static ParameterDefinition<string> NextEMarkReceipt => new ParameterDefinition<string>(10200, () => Resources.EMarkReceiptDoc, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: true, "001");

	public static ParameterDefinition<string> EMarkReceiptHotKey => new ParameterDefinition<string>(10201, () => Resources.EMarkReceiptButton, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true);

	public static ParameterDefinition<bool> EMarkReceiptShowButton => new ParameterDefinition<bool>(10202, () => Resources.EMarkReceiptButton, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> AllowSaleWithQRCodes => new ParameterDefinition<bool>(10203, () => Resources.AllowSaleWithQRCodes, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<DateOnly?> EMarkReceiptLastSyncDate => new ParameterDefinition<DateOnly?>(10204, () => Resources.CheckStatus, userParam: false, roleParam: false, useFromCacheService: true, useFromCacheUI: true, ArmSoft.SME.Common.Constants.Common.MinDateForCPACodes);

	public static ParameterDefinition<bool> POSContinueShowButton => new ParameterDefinition<bool>(10205, () => Resources.Continue, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<string> POSDeferSaleHotKey => new ParameterDefinition<string>(10206, () => Resources.Defer, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, "Ctrl+Shift+D");

	public static ParameterDefinition<bool> POSDeferSaleShowButton => new ParameterDefinition<bool>(10207, () => Resources.Defer, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> POSPartnerCardShowButton => new ParameterDefinition<bool>(10208, () => "string.Format({0} {1}, Resources.Buyers, Resources.Card)", userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> IncludeVATInCost => new ParameterDefinition<bool>(10209, () => Resources.IncludeVATInCost, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<bool> PrintPrepaymentCheckSeparately => new ParameterDefinition<bool>(10210, () => Resources.PrintPrepaymentCheckSeparately, userParam: true, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: true);

	public static ParameterDefinition<bool> MTMoveWithConfirmation => new ParameterDefinition<bool>(10211, () => Resources.MTMoveWithEntryConfirmation, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: true, defaultValue: false);

	public static ParameterDefinition<string> NextMTMoveWithConfirmationDoc => new ParameterDefinition<string>(10212, () => Resources.MTMoveWithConfirmation, userParam: false, roleParam: false, useFromCacheService: false, useFromCacheUI: false, "0001");
}
