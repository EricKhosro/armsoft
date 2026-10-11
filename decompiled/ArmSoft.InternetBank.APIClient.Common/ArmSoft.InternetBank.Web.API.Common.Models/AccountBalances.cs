namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AccountBalances
{
	public decimal? OpeningBalance { get; set; }

	public decimal? ClosingBalance { get; set; }

	public decimal? OpeningExpectedBalance { get; set; }

	public decimal? ClosingExpectedBalance { get; set; }
}
