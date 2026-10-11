using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.ASReport;

public class ECRReportResult
{
	public class SalesRow
	{
		public DateOnly Date { get; set; }

		public decimal Count { get; set; }

		public bool DbCr { get; set; }

		public decimal VATSaleSumm { get; set; }

		public decimal SaleSumm { get; set; }
	}

	public class ZreportBodyRow
	{
		public DateOnly Date { get; set; }

		public decimal Count { get; set; }

		public bool IsPrePayment { get; set; }

		public decimal Summ { get; set; }

		public string SummDescription { get; set; }
	}

	public List<SalesRow> SaleList { get; set; }

	public List<ZreportBodyRow> ListOfTotalPrepaymentSumm { get; set; }

	public List<ZreportBodyRow> ListOfTotalReturnPrepaymentSumm { get; set; }

	public decimal WithUsingPrePaymentSumm { get; set; }
}
