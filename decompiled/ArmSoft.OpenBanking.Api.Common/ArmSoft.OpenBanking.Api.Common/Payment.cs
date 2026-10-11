namespace ArmSoft.OpenBanking.Api.Common;

public class Payment : Document
{
	public string Receiver { get; set; } = string.Empty;

	public decimal Amount { get; set; }

	public string Currency { get; set; } = string.Empty;
}
