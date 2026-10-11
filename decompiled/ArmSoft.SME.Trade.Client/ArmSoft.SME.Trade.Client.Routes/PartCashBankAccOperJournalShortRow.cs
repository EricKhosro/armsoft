using System;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.Client.Routes;

public class PartCashBankAccOperJournalShortRow
{
	public DateOnly Date { get; set; }

	public Guid BaseISN { get; set; }

	public CommonConst.AccountingsTypes AccountType { get; set; }

	public string AccountCode { get; set; } = "";

	public string CurrCode { get; set; } = "";

	public int PartID { get; set; }

	public bool DbCr { get; set; }

	public decimal SummIn { get; set; }

	public decimal CurrSummIn { get; set; }

	public decimal SummOut { get; set; }

	public decimal CurrSummOut { get; set; }

	public string Comment { get; set; } = "";
}
