using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Models;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class DocumentsJournalDataProvider : DataProvider<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public class DataRow : TradeDocumentDataRow, IRowWithISN
	{
		public string ECRCheckNum { get; set; } = "";

		public string PrePaymentECRCheckNum { get; set; } = "";

		public int PartID { get; set; }

		public string PartCode { get; set; } = "";

		public string PartName { get; set; } = "";

		public string PartTaxCode { get; set; } = "";

		public int EmplID { get; set; }

		public string EmplCode { get; set; } = "";

		public string EmplName { get; set; } = "";

		public string Storage { get; set; } = "";

		public string StorageCaption { get; set; } = "";

		public string Info { get; set; } = "";

		public string DocOwner { get; set; } = "";

		public int DocOwnerID { get; set; } = -1;

		public string SerialAndNumber { get; set; } = "";

		public string Serial { get; set; } = "";

		public string Number { get; set; } = "";

		public string TaxExportType { get; set; } = "";

		public string TaxExportCaption { get; set; } = "";

		public int CreatedDocsCount { get; set; }

		public string POSPartnerName { get; set; } = "";

		public string POSPartContractName { get; set; } = "";

		public string QRPaymentID { get; set; } = "";

		public bool QRPaymentAccepted { get; set; }

		public string POState { get; set; } = "";

		public decimal SummaVAT { get; set; }

		public decimal SummaWithVAT { get; set; }

		public DateOnly? SubmissionDate { get; set; }

		public DateOnly SupplyDate { get; set; }

		public string SupplierTaxCode { get; set; } = "";

		public string SupplierName { get; set; } = "";

		public string SupplyAddress { get; set; } = "";

		public string TaxInvType { get; set; } = "";

		public string BaseInvSeriaAndNumber { get; set; } = "";

		public DateOnly? BaseInvDate { get; set; }

		public string InvoiceNumber { get; set; } = "";

		public DateOnly? InvoiceDate { get; set; }

		public string SpecField { get; set; } = "";

		public string PartnerEMail { get; set; } = "";

		public decimal SummaCash { get; set; }

		public decimal SummaBank { get; set; }

		public decimal SummaPrePay { get; set; }

		public decimal SummaBonus { get; set; }

		public decimal SummaCredit { get; set; }

		public string Cashdesk { get; set; } = "";

		public string CashdeskCaption { get; set; } = "";

		public string CashierShiftNumber { get; set; } = "";

		public bool ExportedToDiscountServer { get; set; }

		public Guid TransactionISN { get; set; }

		public string Link { get; set; } = "";

		public byte[] TS { get; set; }

		public string PaymentMethod { get; set; } = "";

		public PartnerCacheRow PartnerCacheRow { get; set; }

		public bool MarkingsExported { get; set; }

		public bool MoveIsDone { get; set; }

		public override object ReferenceValue(string name, string source)
		{
			if (PartnerCacheRow != null)
			{
				return PartnerCacheRow.GetReferenceValue(source);
			}
			return null;
		}
	}

	public class Param
	{
		public DateOnly? StartDate { get; set; }

		public DateOnly? EndDate { get; set; }

		public DateOnly? DelStartDate { get; set; }

		public DateOnly? DelEndDate { get; set; }

		public string ReportType { get; set; }

		public string DocType { get; set; } = "";

		public string DocNum { get; set; } = "";

		public string CurrCode { get; set; } = "";

		public string EmplCode { get; set; } = "";

		public string PartnerCode { get; set; } = "";

		public string GoodCode { get; set; } = "";

		public string StorageCode { get; set; } = "";

		public string CashCode { get; set; } = "";

		public string ShiftNumber { get; set; } = "";

		public string DocState { get; set; } = "";

		public Guid BaseISN { get; set; }

		public string Source { get; set; } = "";

		public string DocOwner { get; set; } = "";

		public bool ShowOutSummsAsNegatives { get; set; }

		public bool ShowPayments { get; set; }

		public Guid DocISN { get; set; }
	}

	public override string Name
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public DocumentsJournalDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
