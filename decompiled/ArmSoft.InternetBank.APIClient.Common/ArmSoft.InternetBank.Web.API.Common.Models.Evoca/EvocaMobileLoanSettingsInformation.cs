namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaMobileLoanSettingsInformation
{
	public string Currency { get; set; }

	public string MinSum { get; set; }

	public string MaxSum { get; set; }

	public string MinMonth { get; set; }

	public string MaxMonth { get; set; }

	public string MinLoanPercent { get; set; }

	public bool IsDafault { get; set; }
}
