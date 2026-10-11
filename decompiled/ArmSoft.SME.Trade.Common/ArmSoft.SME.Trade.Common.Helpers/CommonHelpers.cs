using System.Collections.Generic;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.Helpers;

public class CommonHelpers
{
	public static List<short> GetDocumentTypesByReportType(string reportType = null, bool showReceivedTaxInvoice = true, bool useMarking = true)
	{
		List<short> list = new List<short>();
		if (reportType == null)
		{
			reportType = ReportTypesSE.AllDocument;
		}
		if (reportType == ReportTypesSE.AllDocument.Value || reportType == ReportTypesSE.DeletedDocument.Value)
		{
			list.Add(DocumentTypeSE.PayOrd);
			list.Add(DocumentTypeSE.CashIn);
			list.Add(DocumentTypeSE.CashOut);
			list.Add(DocumentTypeSE.BankIn);
			list.Add(DocumentTypeSE.BankOut);
			list.Add(DocumentTypeSE.PartnerRemCorrect);
			list.Add(DocumentTypeSE.PartnerRemGroupCorrect);
			list.Add(DocumentTypeSE.PartnerRemMove);
			list.Add(DocumentTypeSE.BonusGroupCorrect);
			list.Add(DocumentTypeSE.MTIncome);
			list.Add(DocumentTypeSE.MTInvoice);
			list.Add(DocumentTypeSE.InvoiceForPay);
			list.Add(DocumentTypeSE.MTExpense);
			list.Add(DocumentTypeSE.MTMove);
			list.Add(DocumentTypeSE.MTMoveWithConfirmation);
			list.Add(DocumentTypeSE.MTComplectation);
			list.Add(DocumentTypeSE.MTDeComplectation);
			list.Add(DocumentTypeSE.MTOverheadExp);
			list.Add(DocumentTypeSE.MTReturnToSupplier);
			list.Add(DocumentTypeSE.MTReturnFromCustomer);
			list.Add(DocumentTypeSE.MTStockTaking);
			list.Add(DocumentTypeSE.MTRevaluation);
			list.Add(DocumentTypeSE.Revaluation);
			list.Add(DocumentTypeSE.MTBill);
			list.Add(DocumentTypeSE.MTBillReturn);
			list.Add(DocumentTypeSE.MTIncomeImport);
			list.Add(DocumentTypeSE.MTPurchaseOrder);
			list.Add(DocumentTypeSE.MTIncomeRetail);
			list.Add(DocumentTypeSE.MTSaleRetail);
			list.Add(DocumentTypeSE.MTStockTakingRetail);
			list.Add(DocumentTypeSE.MTRevaluationRetail);
			list.Add(DocumentTypeSE.MTIncomeRem);
			if (showReceivedTaxInvoice)
			{
				list.Add(DocumentTypeSE.ReceivedTaxInvoice);
			}
			list.Add(DocumentTypeSE.PartnersGroupRevaluation);
			list.Add(DocumentTypeSE.ReceivedService);
			list.Add(DocumentTypeSE.Reconciliation);
			list.Add(DocumentTypeSE.MTMoveOrder);
			if (useMarking)
			{
				list.Add(DocumentTypeSE.EMarkReceipt);
			}
		}
		else if (reportType == ReportTypesSE.CashDocument.Value)
		{
			list.Add(DocumentTypeSE.CashIn);
			list.Add(DocumentTypeSE.CashOut);
			list.Add(DocumentTypeSE.MTBill);
			list.Add(DocumentTypeSE.MTBillReturn);
			list.Add(DocumentTypeSE.MTReturnFromCustomer);
			list.Add(DocumentTypeSE.Revaluation);
		}
		else if (reportType == ReportTypesSE.BankDocument.Value)
		{
			list.Add(DocumentTypeSE.PayOrd);
			list.Add(DocumentTypeSE.BankIn);
			list.Add(DocumentTypeSE.BankOut);
			list.Add(DocumentTypeSE.Revaluation);
		}
		else if (reportType == ReportTypesSE.PurchaseDocument.Value)
		{
			list.Add(DocumentTypeSE.MTIncome);
			list.Add(DocumentTypeSE.MTOverheadExp);
			list.Add(DocumentTypeSE.MTReturnToSupplier);
			list.Add(DocumentTypeSE.MTIncomeImport);
			list.Add(DocumentTypeSE.MTIncomeRetail);
			list.Add(DocumentTypeSE.ReceivedService);
			list.Add(DocumentTypeSE.MTIncomeRem);
			list.Add(DocumentTypeSE.Reconciliation);
		}
		else if (reportType == ReportTypesSE.SaleDocument.Value)
		{
			list.Add(DocumentTypeSE.MTInvoice);
			list.Add(DocumentTypeSE.MTReturnFromCustomer);
			list.Add(DocumentTypeSE.MTBill);
			list.Add(DocumentTypeSE.MTBillReturn);
			list.Add(DocumentTypeSE.MTSaleRetail);
			list.Add(DocumentTypeSE.Reconciliation);
		}
		else if (reportType == ReportTypesSE.WarehouseDocument.Value)
		{
			list.Add(DocumentTypeSE.MTIncome);
			list.Add(DocumentTypeSE.MTMove);
			list.Add(DocumentTypeSE.MTMoveWithConfirmation);
			list.Add(DocumentTypeSE.MTExpense);
			list.Add(DocumentTypeSE.MTComplectation);
			list.Add(DocumentTypeSE.MTDeComplectation);
			list.Add(DocumentTypeSE.MTStockTaking);
			list.Add(DocumentTypeSE.MTStockTakingRetail);
			list.Add(DocumentTypeSE.MTRevaluation);
			list.Add(DocumentTypeSE.MTRevaluationRetail);
			list.Add(DocumentTypeSE.MTIncomeRetail);
			list.Add(DocumentTypeSE.MTIncomeImport);
			list.Add(DocumentTypeSE.MTIncomeRem);
			list.Add(DocumentTypeSE.MTMoveOrder);
		}
		else if (reportType == ReportTypesSE.TaxInvoices.Value)
		{
			list.Add(DocumentTypeSE.MTInvoice);
			list.Add(DocumentTypeSE.MTReturnFromCustomer);
			list.Add(DocumentTypeSE.MTBill);
			list.Add(DocumentTypeSE.MTSaleRetail);
		}
		else if ((reportType == ReportTypesSE.ReceivedTaxInvoices.Value) & showReceivedTaxInvoice)
		{
			list.Add(DocumentTypeSE.ReceivedTaxInvoice);
		}
		else if (reportType == ReportTypesSE.TransportationDocument.Value)
		{
			list.Add(DocumentTypeSE.MTMove);
			list.Add(DocumentTypeSE.MTMoveWithConfirmation);
			list.Add(DocumentTypeSE.MTMoveOrder);
		}
		else if (reportType == ReportTypesSE.SentInvoicesForPayment.Value)
		{
			list.Add(DocumentTypeSE.InvoiceForPay);
		}
		else if (reportType == ReportTypesSE.ECRCheckDocument.Value)
		{
			list.Add(DocumentTypeSE.MTBill);
			list.Add(DocumentTypeSE.MTReturnFromCustomer);
			list.Add(DocumentTypeSE.MTBillReturn);
			list.Add(DocumentTypeSE.CashIn);
			list.Add(DocumentTypeSE.CashOut);
		}
		else if (reportType == ReportTypesSE.DeferredDocument.Value)
		{
			list.Add(DocumentTypeSE.MTBill);
			list.Add(DocumentTypeSE.MTReturnFromCustomer);
			list.Add(DocumentTypeSE.MTBillReturn);
		}
		else if (reportType == ReportTypesSE.IBCorrespondent.Value)
		{
			list.Add(DocumentTypeSE.PayOrd);
			list.Add(DocumentTypeSE.CashIn);
			list.Add(DocumentTypeSE.CashOut);
			list.Add(DocumentTypeSE.BankIn);
			list.Add(DocumentTypeSE.BankOut);
		}
		else if (reportType == ReportTypesSE.BonusDocument.Value)
		{
			list.Add(DocumentTypeSE.BonusGroupCorrect);
			list.Add(DocumentTypeSE.MTBill);
			list.Add(DocumentTypeSE.MTBillReturn);
			list.Add(DocumentTypeSE.MTReturnFromCustomer);
		}
		else if (reportType == ReportTypesSE.PurchaseOrderDocument.Value)
		{
			list.Add(DocumentTypeSE.MTPurchaseOrder);
		}
		else if (reportType == ReportTypesSE.BillDocument.Value)
		{
			list.Add(DocumentTypeSE.MTBill);
		}
		else if (reportType == ReportTypesSE.ReconciliationStatements.Value)
		{
			list.Add(DocumentTypeSE.Reconciliation);
		}
		else if (reportType == ReportTypesSE.DiscountDocument.Value)
		{
			list.Add(DocumentTypeSE.MTInvoice);
			list.Add(DocumentTypeSE.MTBill);
		}
		else if (reportType == ReportTypesSE.PartnerDocument.Value)
		{
			list.Add(DocumentTypeSE.PayOrd);
			list.Add(DocumentTypeSE.CashIn);
			list.Add(DocumentTypeSE.CashOut);
			list.Add(DocumentTypeSE.BankIn);
			list.Add(DocumentTypeSE.BankOut);
			list.Add(DocumentTypeSE.PartnerRemCorrect);
			list.Add(DocumentTypeSE.PartnerRemGroupCorrect);
			list.Add(DocumentTypeSE.PartnerRemMove);
			list.Add(DocumentTypeSE.BonusGroupCorrect);
			list.Add(DocumentTypeSE.MTIncome);
			list.Add(DocumentTypeSE.MTInvoice);
			list.Add(DocumentTypeSE.InvoiceForPay);
			list.Add(DocumentTypeSE.MTOverheadExp);
			list.Add(DocumentTypeSE.MTReturnToSupplier);
			list.Add(DocumentTypeSE.MTReturnFromCustomer);
			list.Add(DocumentTypeSE.Revaluation);
			list.Add(DocumentTypeSE.MTBill);
			list.Add(DocumentTypeSE.MTBillReturn);
			list.Add(DocumentTypeSE.MTIncomeImport);
			list.Add(DocumentTypeSE.MTPurchaseOrder);
			list.Add(DocumentTypeSE.MTIncomeRetail);
			list.Add(DocumentTypeSE.MTSaleRetail);
			list.Add(DocumentTypeSE.MTIncomeRem);
			list.Add(DocumentTypeSE.PartnersGroupRevaluation);
			list.Add(DocumentTypeSE.ReceivedService);
			list.Add(DocumentTypeSE.Reconciliation);
		}
		return list;
	}
}
