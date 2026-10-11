namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaGiftCardPackageType
{
	public int Id { get; set; }

	public string Name { get; set; }

	public decimal PackageAmount { get; set; }

	public string Image { get; set; }

	public decimal DeliveryAmountInArmenia { get; set; }

	public decimal DeliveryAmountInYerevan { get; set; }
}
