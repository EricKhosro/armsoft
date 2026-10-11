using ArmSoft.OnlineBanking.Common.Evoca;

namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaMobileLoanSettingsInformationV2
{
	public string Currency { get; set; }

	public string MinSum { get; set; }

	public string MaxSum { get; set; }

	public int MinPeriod { get; set; }

	public int MaxPeriod { get; set; }

	public EvocaMobileLoanDurationType DurationType { get; set; }

	public string MinLoanPercent { get; set; }

	public bool IsDafault { get; set; }
}
