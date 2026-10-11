namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AccountBalance
{
	public string Number { get; set; }

	public decimal? Balance { get; set; }

	public decimal? ExpectedBalance { get; set; }
}
