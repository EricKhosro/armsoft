using System;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class CardRow
{
	public string DisplayCardNumber { get; set; }

	public string DisplayAccountNumber { get; set; }

	public string Currency { get; set; }

	public string DisplayCardType { get; set; }

	public string Name { get; set; }

	public string EMail { get; set; }

	public DateOnly FromDate { get; set; }

	public DateOnly ToDate { get; set; }

	public string Note { get; set; }

	public string MainCardNumber { get; set; }

	public decimal? AccountBalance { get; set; }

	public decimal? ExpectedBalance { get; set; }
}
