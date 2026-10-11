namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaMobileLoanCalculateParams
{
	public int ApplicationId { get; set; }

	public int Amount { get; set; }

	public int Duration { get; set; }

	public decimal Percent { get; set; }
}
