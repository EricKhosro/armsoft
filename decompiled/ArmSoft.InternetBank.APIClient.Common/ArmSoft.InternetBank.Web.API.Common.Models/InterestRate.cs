using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class InterestRate
{
	public decimal Rate { get; set; }

	public RateDivisor Divisor { get; set; }
}
