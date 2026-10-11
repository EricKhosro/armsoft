using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

public class ReportTypesSE : ASSmartEnum<ReportTypesSE, string>
{
	public static readonly ReportTypesSE AllDocument = new ReportTypesSE("AllDocument", "1", () => ArmSoft.SME.Common.Resources.DocumentsJournal);

	public static readonly ReportTypesSE CashDocument = new ReportTypesSE("CashDocument", "2", () => Resources.CashDocumentsJournal);

	public static readonly ReportTypesSE BankDocument = new ReportTypesSE("BankDocument", "3", () => Resources.BankDocumentsJournal);

	public static readonly ReportTypesSE PurchaseDocument = new ReportTypesSE("PurchaseDocument", "4", () => Resources.PurchaseDocumentsJournal);

	public static readonly ReportTypesSE SaleDocument = new ReportTypesSE("SaleDocument", "5", () => ArmSoft.SME.Common.Resources.SaleDocuments);

	public static readonly ReportTypesSE WarehouseDocument = new ReportTypesSE("WarehouseDocument", "6", () => Resources.WarehouseDocumentsJournal);

	public static readonly ReportTypesSE TaxInvoices = new ReportTypesSE("TaxInvoices", "7", () => Resources.WriteOffTaxInvoicesJournal);

	public static readonly ReportTypesSE ReceivedTaxInvoices = new ReportTypesSE("ReceivedTaxInvoices", "8", () => Resources.ReceivedTaxInvoicesJournal);

	public static readonly ReportTypesSE IBCorrespondent = new ReportTypesSE("IBCorrespondent", "9", () => ArmSoft.SME.Common.Resources.DocumentsJournal);

	public static readonly ReportTypesSE TransportationDocument = new ReportTypesSE("TransportationDocument", "10", () => ArmSoft.SME.Common.Resources.TransportationDocumentsJournal);

	public static readonly ReportTypesSE PurchaseOrderDocument = new ReportTypesSE("PurchaseOrderDocument", "11", () => ArmSoft.SME.Common.Resources.PurchaseOrderDocsJournal);

	public static readonly ReportTypesSE SentInvoicesForPayment = new ReportTypesSE("SentInvoicesForPayment", "12", () => ArmSoft.SME.Common.Resources.SentInvoicesForPayment);

	public static readonly ReportTypesSE ReconciliationStatements = new ReportTypesSE("ReconciliationStatements", "13", () => Resources.ReconciliationStatementsJournal);

	public static readonly ReportTypesSE BillDocument = new ReportTypesSE("BillDocument", "14", () => ArmSoft.SME.Common.Resources.DocumentsJournal);

	public static readonly ReportTypesSE DeletedDocument = new ReportTypesSE("DeletedDocument", "15", () => ArmSoft.SME.Common.Resources.DeletedDocumentsJournal);

	public static readonly ReportTypesSE ECRCheckDocument = new ReportTypesSE("ECRCheckDocument", "16", () => ArmSoft.SME.Common.Resources.ECRChecksJournal);

	public static readonly ReportTypesSE DeferredDocument = new ReportTypesSE("DeferredDocument", "17", () => ArmSoft.SME.Common.Resources.ECRChecksJournal);

	public static readonly ReportTypesSE BonusDocument = new ReportTypesSE("BonusDocument", "18", () => ArmSoft.SME.Common.Resources.DocumentsJournal);

	public static readonly ReportTypesSE DiscountDocument = new ReportTypesSE("DiscountDocument", "19", () => ArmSoft.SME.Common.Resources.DocumentsJournal);

	public static readonly ReportTypesSE PartnerDocument = new ReportTypesSE("PartnerDocument", "20", () => Resources.PartnerOpJournal);

	public ReportTypesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
