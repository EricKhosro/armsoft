using System;
using System.Text.Json.Serialization;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Special)]
[SmartEnumProviderSupport(2, "DocumentType")]
[SmartEnumPartial(SmartEnumType.Base)]
public class DocumentTypeSE : ASSmartEnum<DocumentTypeSE, short>
{
	public static readonly DocumentTypeSE PayOrd = new DocumentTypeSE("PayOrd", 1, () => ArmSoft.SME.Common.Resources.DocPayOrdShort, () => ArmSoft.SME.Common.Resources.PaymentOrder, 0);

	public static readonly DocumentTypeSE CashIn = new DocumentTypeSE("CashIn", 2, () => ArmSoft.SME.Common.Resources.DocCashInShort, () => ArmSoft.SME.Common.Resources.CashInputOrder, 0);

	public static readonly DocumentTypeSE CashOut = new DocumentTypeSE("CashOut", 3, () => ArmSoft.SME.Common.Resources.DocCashOutShort, () => ArmSoft.SME.Common.Resources.CashOutputOrder, 0);

	public static readonly DocumentTypeSE BankIn = new DocumentTypeSE("BankIn", 4, () => Resources.BankIn, () => Resources.BankIn, 0);

	public static readonly DocumentTypeSE BankOut = new DocumentTypeSE("BankOut", 5, () => Resources.BankOut, () => Resources.BankOut, 0);

	public static readonly DocumentTypeSE PartnerRemCorrect = new DocumentTypeSE("PartnerRemCorrect", 6, () => Resources.PartnerRemCorrection, () => Resources.PartnerRemCorrection, 0);

	public static readonly DocumentTypeSE MTIncome = new DocumentTypeSE("MTIncome", 7, () => ArmSoft.SME.Common.Resources.DocMTIncomeShort, () => ArmSoft.SME.Common.Resources.DocMTIncome, 0);

	public static readonly DocumentTypeSE MTInvoice = new DocumentTypeSE("MTInvoice", 8, () => ArmSoft.SME.Common.Resources.DocInvoiceShort, () => Resources.DocInvoice, 0);

	public static readonly DocumentTypeSE MTExpense = new DocumentTypeSE("MTExpense", 9, () => ArmSoft.SME.Common.Resources.DocMTExpenseShort, () => ArmSoft.SME.Common.Resources.DocMTExpense, 0);

	public static readonly DocumentTypeSE MTMove = new DocumentTypeSE("MTMove", 10, () => Resources.DocMTMoveShort, () => Resources.DocMTMove, 0);

	public static readonly DocumentTypeSE MTComplectation = new DocumentTypeSE("MTComplectation", 11, () => Resources.DocMTComplectationShort, () => Resources.Complection, 0);

	public static readonly DocumentTypeSE MTDeComplectation = new DocumentTypeSE("MTDeComplectation", 12, () => Resources.Decomplection, () => Resources.Decomplection, 0);

	public static readonly DocumentTypeSE MTOverheadExp = new DocumentTypeSE("MTOverheadExp", 13, () => ArmSoft.SME.Common.Resources.DocMTOverheadExpShort, () => ArmSoft.SME.Common.Resources.DocMTOverheadExpenses, 0);

	public static readonly DocumentTypeSE MTReturnToSupplier = new DocumentTypeSE("MTReturnToSupplier", 14, () => Resources.DocMTReturnToSupplier, () => Resources.DocMTReturnToSupplier, 0);

	public static readonly DocumentTypeSE MTReturnFromCustomer = new DocumentTypeSE("MTReturnFromCustomer", 15, () => Resources.DocMTReturnFromCustomer, () => Resources.DocMTReturnFromCustomer, 0);

	public static readonly DocumentTypeSE MTStockTaking = new DocumentTypeSE("MTStockTaking", 16, () => Resources.DocMTInventoryShort, () => Resources.DocMTInventoryShort, 0);

	public static readonly DocumentTypeSE ReceivedService = new DocumentTypeSE("ReceivedService", 29, () => ArmSoft.SME.Common.Resources.BoughtServicesShort, () => ArmSoft.SME.Common.Resources.BoughtServices, 0);

	public static readonly DocumentTypeSE Revaluation = new DocumentTypeSE("Revaluation", 17, () => ArmSoft.SME.Common.Resources.Revaluation, () => ArmSoft.SME.Common.Resources.Revaluation, 0);

	public static readonly DocumentTypeSE MTBill = new DocumentTypeSE("MTBill", 18, () => Resources.DocMTBill, () => Resources.DocMTBill, 0);

	public static readonly DocumentTypeSE MTBillReturn = new DocumentTypeSE("MTBillReturn", 38, () => Resources.DocMTBillReturn, () => Resources.DocMTBillReturn, 0);

	public static readonly DocumentTypeSE MTIncomeImport = new DocumentTypeSE("MTIncomeImport", 19, () => Resources.DocMTIncomeImportShort, () => Resources.DocMTIncomeImport, 0);

	public static readonly DocumentTypeSE MTSaleRetail = new DocumentTypeSE("MTSaleRetail", 21, () => ArmSoft.SME.Common.Resources.DocInvoiceRetailShort, () => ArmSoft.SME.Common.Resources.DocInvoiceRetail, 0);

	public static readonly DocumentTypeSE MTIncomeRetail = new DocumentTypeSE("MTIncomeRetail", 20, () => Resources.DocMTIncomeRetail, () => Resources.DocMTIncomeRetail, 0);

	public static readonly DocumentTypeSE MTStockTakingRetail = new DocumentTypeSE("MTStockTakingRetail", 22, () => Resources.DocMTInventoryRetail, () => Resources.DocMTInventoryRetail, 0);

	public static readonly DocumentTypeSE MTRevaluationRetail = new DocumentTypeSE("MTRevaluationRetail", 23, () => Resources.DocMTRevaluationRetail, () => Resources.DocMTRevaluationRetail, 0);

	public static readonly DocumentTypeSE MTRevaluation = new DocumentTypeSE("MTRevaluation", 35, () => Resources.MaterialRevaluation, () => Resources.MaterialRevaluation, 0);

	public static readonly DocumentTypeSE MTIncomeRem = new DocumentTypeSE("MTIncomeRem", 24, () => Resources.DocMTIncomeRem, () => Resources.DocMTIncomeRem, 0);

	public static readonly DocumentTypeSE PartnersGroupRevaluation = new DocumentTypeSE("PartnersGroupRevaluation", 25, () => Resources.PartnersGroupRevaluationShort, () => Resources.PartnersGroupRevaluation, 0);

	public static readonly DocumentTypeSE PartnerRemMove = new DocumentTypeSE("PartnerRemMove", 27, () => Resources.PartnerRemMove, () => Resources.PartnerRemMove, 0);

	public static readonly DocumentTypeSE PartnerRemGroupCorrect = new DocumentTypeSE("PartnerRemGroupCorrect", 28, () => Resources.PartnerRemGroupCorrection, () => Resources.PartnerRemGroupCorrection, 0);

	public static readonly DocumentTypeSE BonusGroupCorrect = new DocumentTypeSE("BonusGroupCorrect", 30, () => Resources.BonusGroupCorrect, () => Resources.BonusGroupCorrect, 0);

	public static readonly DocumentTypeSE MTPurchaseOrder = new DocumentTypeSE("MTPurchaseOrder", 32, () => ArmSoft.SME.Common.Resources.DocMTPurchaseOrder, () => ArmSoft.SME.Common.Resources.DocMTPurchaseOrder, 0);

	public static readonly DocumentTypeSE InvoiceForPay = new DocumentTypeSE("InvoiceForPay", 33, () => ArmSoft.SME.Common.Resources.InvoiceForPay, () => ArmSoft.SME.Common.Resources.InvoiceForPay, 0);

	public static readonly DocumentTypeSE Reconciliation = new DocumentTypeSE("Reconciliation", 34, () => ArmSoft.SME.Common.Resources.ReconciliationStatement, () => ArmSoft.SME.Common.Resources.ReconciliationStatement, 0);

	public static readonly DocumentTypeSE MTMoveOrder = new DocumentTypeSE("MTMoveOrder", 36, () => Resources.MTMoveOrder, () => Resources.MTMoveOrder, 0);

	public static readonly DocumentTypeSE ReceivedTaxInvoice = new DocumentTypeSE("ReceivedTaxInvoice", 26, () => ArmSoft.SME.Common.Resources.ReceivedTaxInvoice, () => ArmSoft.SME.Common.Resources.ReceivedTaxInvoice, 0);

	public static readonly DocumentTypeSE EMarkReceipt = new DocumentTypeSE("EMarkReceipt", 37, () => Resources.EMarkReceiptDoc, () => Resources.EMarkReceiptDoc, 0);

	public static readonly DocumentTypeSE MTMoveWithConfirmation = new DocumentTypeSE("MTMoveWithConfirmation", 39, () => Resources.MTMoveWithConfirmation, () => Resources.MTMoveWithConfirmation, 0);

	public const short PayOrdConst = 1;

	public const short CashInConst = 2;

	public const short CashOutConst = 3;

	public const short BankInConst = 4;

	public const short BankOutConst = 5;

	public const short PartnerRemCorrectConst = 6;

	public const short MTIncomeConst = 7;

	public const short MTInvoiceConst = 8;

	public const short MTExpenseConst = 9;

	public const short MTMoveConst = 10;

	public const short MTComplectationConst = 11;

	public const short MTDeComplectationConst = 12;

	public const short MTOverheadExpConst = 13;

	public const short MTReturnToSupplierConst = 14;

	public const short MTReturnFromCustomerConst = 15;

	public const short MTStockTakingConst = 16;

	public const short RevaluationConst = 17;

	public const short MTBillConst = 18;

	public const short MTIncomeImportConst = 19;

	public const short MTIncomeRetailConst = 20;

	public const short MTSaleRetailConst = 21;

	public const short MTStockTakingRetailConst = 22;

	public const short MTRevaluationRetailConst = 23;

	public const short MTIncomeRemConst = 24;

	public const short PartnersGroupRevaluationConst = 25;

	public const short ReceivedTaxInvoiceConst = 26;

	public const short PartnerRemMoveConst = 27;

	public const short PartnerRemGroupCorrectConst = 28;

	public const short ReceivedServiceConst = 29;

	public const short BonusGroupCorrectConst = 30;

	public const short MTPurchaseOrderConst = 32;

	public const short InvoiceForPayConst = 33;

	public const short ReconciliationConst = 34;

	public const short MTRevaluationConst = 35;

	public const short MTMoveOrderConst = 36;

	public const short EMarkReceiptConst = 37;

	public const short MTBillReturnConst = 38;

	public const short MTMoveWithConfirmationConst = 39;

	public static readonly DocumentTypeSE TaxDocument = new DocumentTypeSE("TaxDocument", 231, () => ArmSoft.SME.Common.Resources.SettlementDocument, () => ArmSoft.SME.Common.Resources.SettlementDocument, 0);

	public const short TaxDocumentConst = 231;

	[JsonIgnore]
	public readonly Func<string> FullCaptionDelegate;

	public readonly short DataProviderType;

	public string FullCaption => FullCaptionDelegate();

	public DocumentTypeSE(string name, short value, Func<string> caption, Func<string> fullCaption = null, short dataProviderType = 0)
		: base(name, value, caption)
	{
		FullCaptionDelegate = ((fullCaption == null) ? caption : fullCaption);
		DataProviderType = dataProviderType;
	}
}
