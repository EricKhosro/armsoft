namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaGiftCardType
{
	public int Code { get; set; }

	public string Name { get; set; }

	public decimal DeliveryAmount { get; set; }

	public string Icon { get; set; }

	public decimal MaxAmount { get; set; }

	public decimal MinAmount { get; set; }

	public bool PartnerHas { get; set; }

	public bool BankHas { get; set; }
}
