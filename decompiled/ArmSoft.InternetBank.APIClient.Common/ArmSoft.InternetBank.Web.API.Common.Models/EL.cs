namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class EL : Application
{
	public int LoanId { get; set; }

	public string LoanCode { get; set; }

	public string Currency { get; set; }

	public decimal Rate { get; set; }

	public int Duration { get; set; }

	public string CardNumber { get; set; }

	public int PreferredDay { get; set; }

	public decimal Amount { get; set; }

	public string CardDeliveryRegionKey { get; set; }

	public string CardDeliveryCommunityKey { get; set; }

	public string CardDeliveryCommunityAddress { get; set; }

	public string CardDeliveryAddress { get; set; }
}
