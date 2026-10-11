using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Special)]
[SmartEnumProviderSupport(20, "DocumentTypes")]
[SmartEnumPartial(SmartEnumType.Base)]
public class PrintTemplateDocumentTypeSE : ASSmartEnum<PrintTemplateDocumentTypeSE, string>
{
	public static readonly PrintTemplateDocumentTypeSE PayOrd = new PrintTemplateDocumentTypeSE(DocumentTypeSE.PayOrd);

	public static readonly PrintTemplateDocumentTypeSE CashIn = new PrintTemplateDocumentTypeSE(DocumentTypeSE.CashIn);

	public static readonly PrintTemplateDocumentTypeSE CashOut = new PrintTemplateDocumentTypeSE(DocumentTypeSE.CashOut);

	public static readonly PrintTemplateDocumentTypeSE PartnerRemCorrect = new PrintTemplateDocumentTypeSE(DocumentTypeSE.PartnerRemCorrect);

	public static readonly PrintTemplateDocumentTypeSE MTIncome = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTIncome);

	public static readonly PrintTemplateDocumentTypeSE MTInvoice = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTInvoice);

	public static readonly PrintTemplateDocumentTypeSE MTExpense = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTExpense);

	public static readonly PrintTemplateDocumentTypeSE MTMove = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTMove);

	public static readonly PrintTemplateDocumentTypeSE MTComplectation = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTComplectation);

	public static readonly PrintTemplateDocumentTypeSE MTDeComplectation = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTDeComplectation);

	public static readonly PrintTemplateDocumentTypeSE MTReturnToSupplier = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTReturnToSupplier);

	public static readonly PrintTemplateDocumentTypeSE MTReturnFromCustomer = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTReturnFromCustomer);

	public static readonly PrintTemplateDocumentTypeSE MTStockTaking = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTStockTaking);

	public static readonly PrintTemplateDocumentTypeSE ReceivedService = new PrintTemplateDocumentTypeSE(DocumentTypeSE.ReceivedService);

	public static readonly PrintTemplateDocumentTypeSE MTBill = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTBill);

	public static readonly PrintTemplateDocumentTypeSE MTBillReturn = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTBillReturn);

	public static readonly PrintTemplateDocumentTypeSE MTIncomeImport = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTIncomeImport);

	public static readonly PrintTemplateDocumentTypeSE MTSaleRetail = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTSaleRetail);

	public static readonly PrintTemplateDocumentTypeSE MTIncomeRetail = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTIncomeRetail);

	public static readonly PrintTemplateDocumentTypeSE MTStockTakingRetail = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTStockTakingRetail);

	public static readonly PrintTemplateDocumentTypeSE MTRevaluationRetail = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTRevaluationRetail);

	public static readonly PrintTemplateDocumentTypeSE MTRevaluation = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTRevaluation);

	public static readonly PrintTemplateDocumentTypeSE MTIncomeRem = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTIncomeRem);

	public static readonly PrintTemplateDocumentTypeSE PartnerRemMove = new PrintTemplateDocumentTypeSE(DocumentTypeSE.PartnerRemMove);

	public static readonly PrintTemplateDocumentTypeSE MTPurchaseOrder = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTPurchaseOrder);

	public static readonly PrintTemplateDocumentTypeSE InvoiceForPay = new PrintTemplateDocumentTypeSE(DocumentTypeSE.InvoiceForPay);

	public static readonly PrintTemplateDocumentTypeSE Reconciliation = new PrintTemplateDocumentTypeSE(DocumentTypeSE.Reconciliation);

	public static readonly PrintTemplateDocumentTypeSE MTMoveOrder = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTMoveOrder);

	public static readonly PrintTemplateDocumentTypeSE MTMoveWithConfirmation = new PrintTemplateDocumentTypeSE(DocumentTypeSE.MTMoveWithConfirmation);

	public static readonly PrintTemplateDocumentTypeSE PriceList = new PrintTemplateDocumentTypeSE("PriceList", "100", () => ArmSoft.SME.Common.Resources.PriceList);

	public static readonly PrintTemplateDocumentTypeSE Label = new PrintTemplateDocumentTypeSE("Label", "101", () => Resources.Label);

	public static readonly PrintTemplateDocumentTypeSE IDCard = new PrintTemplateDocumentTypeSE("IDCard", "102", () => Resources.IDCard);

	public static readonly PrintTemplateDocumentTypeSE DiscountsDeclaration = new PrintTemplateDocumentTypeSE("DiscountsDeclaration", "103", () => Resources.DiscountsDeclaration);

	public static readonly PrintTemplateDocumentTypeSE CashierShiftReport = new PrintTemplateDocumentTypeSE("CashierShiftReport", "104", () => Resources.CashierShiftReport);

	public static readonly PrintTemplateDocumentTypeSE PaymentInfo = new PrintTemplateDocumentTypeSE("PaymentInfo", "105", () => ArmSoft.SME.Common.Resources.PaymentSystem);

	public static readonly PrintTemplateDocumentTypeSE DocWithECRResponse = new PrintTemplateDocumentTypeSE("DocWithECRResponse", "106", () => "DocWithECRResponse");

	public static readonly PrintTemplateDocumentTypeSE PosTerminalReceipt = new PrintTemplateDocumentTypeSE("PosTerminalReceipt", "107", () => Resources.PosTerminalReceipt);

	public const short PayOrderConst = 1;

	public const short CashInConst = 2;

	public const short CashOutConst = 3;

	public const short PartnerRemCorrectConst = 6;

	public const short MTIncomeConst = 7;

	public const short MTInvoiceConst = 8;

	public const short MTExpenseConst = 9;

	public const short MTMoveConst = 10;

	public const short MTComplectationConst = 11;

	public const short MTDecomplectationConst = 12;

	public const short MTReturnToSupplierConst = 14;

	public const short MTReturnFromCustomerConst = 15;

	public const short MTStockTakingConst = 16;

	public const short MTBillConst = 18;

	public const short MTIncomeImportConst = 19;

	public const short MTIncomeRetailConst = 20;

	public const short MTSaleRetailConst = 21;

	public const short MTStockTakingRetailConst = 22;

	public const short MTRevaluationRetailConst = 23;

	public const short MTIncomeRemConst = 24;

	public const short PartnerRemMoveConst = 27;

	public const short ReceivedServiceConst = 29;

	public const short MTPurchaseOrderConst = 32;

	public const short InvoiceForPayConst = 33;

	public const short ReconciliationConst = 34;

	public const short MTRevaluationConst = 35;

	public const short MTMoveOrderConst = 36;

	public const short MTBillReturnConst = 38;

	public const short MTMoveWithConfirmationConst = 39;

	public const short PriceListConst = 100;

	public const short LabelConst = 101;

	public const short IDCardConst = 102;

	public const short DiscountsDeclarationConst = 103;

	public const short CashierShiftReportConst = 104;

	public const short PaymentInfoConst = 105;

	public const short DocWithECRResponseConst = 106;

	public const short PosTerminalReceiptConst = 107;

	public PrintTemplateDocumentTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}

	public PrintTemplateDocumentTypeSE(DocumentTypeSE docType)
		: base(docType.Name, docType.Value.ToString().PadLeft(2, '0'), docType.FullCaptionDelegate)
	{
	}
}
