namespace ArmSoft.OpenBanking.Api.Common;

public class AccountBalance
{
	public decimal? OpeningBalance { get; set; }

	public decimal? ClosingBalance { get; set; }

	public decimal? OpeningExpectedBalance { get; set; }

	public decimal? ClosingExpectedBalance { get; set; }
}
