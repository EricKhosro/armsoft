namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CreditTemplate
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string Group { get; set; }

	public string Currency { get; set; }

	public decimal MinAmount { get; set; }

	public decimal MaxAmount { get; set; }

	public Duration MinDuration { get; set; }

	public Duration MaxDuration { get; set; }

	public InterestRate MinRate { get; set; }

	public InterestRate MaxRate { get; set; }

	public InterestRate Rate { get; set; }
}
