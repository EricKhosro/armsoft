using ArmSoft.OnlineBanking.Common.Evoca;

namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaMobileLoanCalculateParamsV2
{
	public int ApplicationId { get; set; }

	public int Amount { get; set; }

	public int ReqPeriod { get; set; }

	public EvocaMobileLoanDurationType DurationType { get; set; }

	public decimal Percent { get; set; }
}
