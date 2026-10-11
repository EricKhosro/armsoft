using System;

namespace ArmSoft.SME.Trade.Common.Models.Other;

public class BonusTransactionRow
{
	public string CardNumber { get; set; } = string.Empty;

	public DateTime TransactionDate { get; set; }

	public decimal Sum { get; set; }

	public byte OpTypeCode { get; set; }

	public bool DBCR { get; set; }
}
