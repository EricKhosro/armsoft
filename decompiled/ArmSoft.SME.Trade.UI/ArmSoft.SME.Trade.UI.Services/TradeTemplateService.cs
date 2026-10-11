using Ardalis.SmartEnum;
using ArmSoft.SME.Core.UI.Printing;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Printing;

namespace ArmSoft.SME.Trade.UI.Services;

internal class TradeTemplateService : TemplateService
{
	public override bool PrintASInfo(short printModelType)
	{
		return true;
	}

	public override object InitDesignerByDocumentType(short printModelType, out string displayName)
	{
		displayName = SmartEnum<PrintTemplateDocumentTypeSE, string>.FromValue(printModelType.ToString().PadLeft(2, '0')).Name;
		return printModelType switch
		{
			104 => new DesignerBinding<CashierShiftReportPrint>(displayName), 
			2 => new DesignerBinding<CashIn>(displayName), 
			3 => new DesignerBinding<CashOut>(displayName), 
			102 => new DesignerBinding<IDCard>(displayName), 
			33 => new DesignerBinding<InvoiceForPay>(displayName), 
			101 => new DesignerBinding<Label>(displayName), 
			18 => new DesignerBinding<MTBill>(displayName), 
			38 => new DesignerBinding<MTBillReturn>(displayName), 
			11 => new DesignerBinding<MTComplectation>(displayName), 
			12 => new DesignerBinding<MTDecomplectation>(displayName), 
			9 => new DesignerBinding<MTExpense>(displayName), 
			7 => new DesignerBinding<MTIncome>(displayName), 
			19 => new DesignerBinding<MTIncomeImport>(displayName), 
			24 => new DesignerBinding<MTIncomeRem>(displayName), 
			20 => new DesignerBinding<MTIncomeRetail>(displayName), 
			8 => new DesignerBinding<MTInvoice>(displayName), 
			21 => new DesignerBinding<MTInvoiceRetail>(displayName), 
			10 => new DesignerBinding<MTMove>(displayName), 
			36 => new DesignerBinding<MTMoveOrder>(displayName), 
			32 => new DesignerBinding<MTPurchaseOrder>(displayName), 
			15 => new DesignerBinding<MTReturnFromCustomer>(displayName), 
			14 => new DesignerBinding<MTReturnToSupplier>(displayName), 
			35 => new DesignerBinding<MTRevaluation>(displayName), 
			23 => new DesignerBinding<MTRevaluationRetail>(displayName), 
			16 => new DesignerBinding<MTStockTaking>(displayName), 
			22 => new DesignerBinding<MTStockTakingRetail>(displayName), 
			6 => new DesignerBinding<PartnerRemCorrect>(displayName), 
			27 => new DesignerBinding<PartnerRemMove>(displayName), 
			1 => new DesignerBinding<PayOrder>(displayName), 
			29 => new DesignerBinding<ReceivedService>(displayName), 
			34 => new DesignerBinding<Reconcilation>(displayName), 
			107 => new DesignerBinding<PosTerminalReceipt>(displayName), 
			39 => new DesignerBinding<MTMoveWithConfirmation>(displayName), 
			_ => base.InitDesignerByDocumentType(printModelType, out displayName), 
		};
	}
}
