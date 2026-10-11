using ArmSoft.SME.Common.Documents;

namespace ArmSoft.SME.Trade.Common.Documents.Definition;

public class TradeAllDocDefinitions : AllDocDefinitions
{
	public static DocDefinition BankIn => TradeDefinitionCreation.CreateBankIn();

	public static DocDefinition BankOut => TradeDefinitionCreation.CreateBankOut();

	public static DocDefinition PayOrder => TradeDefinitionCreation.PayOrder();

	public static DocDefinition CashIn => TradeDefinitionCreation.CashIn();

	public static DocDefinition Revaluation => TradeDefinitionCreation.Revaluation();

	public static DocDefinition PartnerRemCorrect => TradeDefinitionCreation.PartnerRemCorrect();

	public static DocDefinition PartnerRemMove => TradeDefinitionCreation.PartnerRemMove();

	public static DocDefinition ReceivedService => TradeDefinitionCreation.ReceivedService();

	public static DocDefinition MTIncome => TradeDefinitionCreation.MTIncome();

	public static DocDefinition MTIncomeRetail => TradeDefinitionCreation.MTIncomeRetail();

	public static DocDefinition CashOut => TradeDefinitionCreation.CashOut();

	public static DocDefinition MTMove => TradeDefinitionCreation.CreateMTMove();

	public static DocDefinition MTMoveWithConfirmation => TradeDefinitionCreation.CreateMTMoveWithConfirmation();

	public static DocDefinition MTMoveOrder => TradeDefinitionCreation.CreateMTMoveOrder();

	public static DocDefinition MTBill => TradeDefinitionCreation.CreateMTBill();

	public static DocDefinition MTPurchaseOrder => TradeDefinitionCreation.CreateMTPurchaseOrder();

	public static DocDefinition MTComplectation => TradeDefinitionCreation.MTComplectation();

	public static DocDefinition MTDecomplectation => TradeDefinitionCreation.MTDecomplectation();

	public static DocDefinition ReconcilationStatement => TradeDefinitionCreation.CreateReconcilationStatement();

	public static DocDefinition BonusGroupCorrect => TradeDefinitionCreation.BonusGroupCorrect();

	public static DocDefinition InvoiceForPay => TradeDefinitionCreation.CreateInvoiceForPay();

	public static DocDefinition MTInvoice => TradeDefinitionCreation.CreateMTInvoice();

	public static DocDefinition MTInvoiceRetail => TradeDefinitionCreation.CreateMTInvoiceRetail();

	public static DocDefinition MTRevaluationRetail => TradeDefinitionCreation.CreateMTRevaluationRetail();

	public static DocDefinition MTRevaluation => TradeDefinitionCreation.CreateMTRevaluation();

	public static DocDefinition MTStockTaking => TradeDefinitionCreation.CreateMTStockTaking();

	public static DocDefinition MTStockTakingRetail => TradeDefinitionCreation.CreateMTStockTakingRetail();

	public static DocDefinition PartnerGroupCorrect => TradeDefinitionCreation.PartnerGroupCorrect();

	public static DocDefinition MTReturnToSupplier => TradeDefinitionCreation.CreateMTReturnToSupplier();

	public static DocDefinition MTIncomeRem => TradeDefinitionCreation.CreateMTIncomeRem();

	public static DocDefinition PartnersGroupRevaluation => TradeDefinitionCreation.CreatePartnersGroupRevaluation();

	public static DocDefinition MTOverheadExp => TradeDefinitionCreation.CreateMTOverheadExp();

	public static DocDefinition MTIncomeImport => TradeDefinitionCreation.CreateMTIncomeImport();

	public static DocDefinition MTExpense => TradeDefinitionCreation.CreateMTExpense();

	public static DocDefinition MTBillReturn => TradeDefinitionCreation.CreateMTBillReturn();

	public static DocDefinition MTReturnFromCustomer => TradeDefinitionCreation.CreateMTReturnFromCustomer();

	public static DocDefinition ReceivedTaxInvoice => TradeDefinitionCreation.CreateReceivedTaxInvoice();

	public static DocDefinition EMarkReceipt => TradeDefinitionCreation.CreateEMarkReceipt();
}
