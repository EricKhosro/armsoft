namespace ArmSoft.SME.Trade.Common.Models.Other;

public class BonusRemAndTurnRow
{
	public string CardNumber { get; set; } = string.Empty;

	public decimal DBSum { get; set; }

	public decimal CRSum { get; set; }

	public decimal InitialRem { get; set; }

	public decimal EndRem { get; set; }
}
