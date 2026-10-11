using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class Reconcilation : TradeDocument
{
	public class ReconcilationSpecificationRow
	{
		public string Type { get; set; }

		public short? RowNum { get; set; }

		public DateOnly? Date { get; set; }

		public string ContractCode { get; set; }

		public Guid? BaseDocISN { get; set; }

		public string BaseDocDescr { get; set; }

		public string TransactionComment { get; set; }

		public decimal? SummIn { get; set; }

		public decimal? SummOut { get; set; }
	}

	public DateOnly PeriodStart { get; set; }

	public DateOnly PeriodEnd { get; set; }

	public POStatesSE ReconcilationState { get; set; }

	public Guid? IdAsGuid { get; set; }

	public decimal? OurStartRem { get; set; }

	public decimal? OurEndRem { get; set; }

	public List<ReconcilationSpecificationRow> OurCalcsList { get; set; } = new List<ReconcilationSpecificationRow>();
}
