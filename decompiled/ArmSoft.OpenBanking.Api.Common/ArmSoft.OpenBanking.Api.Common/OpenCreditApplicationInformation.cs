namespace ArmSoft.OpenBanking.Api.Common;

public class OpenCreditApplicationInformation
{
	public decimal CreditAmount { get; set; }

	public string Currency { get; set; } = string.Empty;

	public int CreditPeriodMonths { get; set; }

	public string CreditType { get; set; } = string.Empty;

	public string Purpose { get; set; } = string.Empty;
}
