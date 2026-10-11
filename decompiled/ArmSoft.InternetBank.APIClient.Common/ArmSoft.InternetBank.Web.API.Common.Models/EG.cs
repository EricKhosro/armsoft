using ArmSoft.OnlineBanking.Common.Evoca;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class EG : Application
{
	public string PayerAcc { get; set; }

	public int CardType { get; set; }

	public string CardTypeName { get; set; }

	public EvocaGiftCardReceivingType ReceiveType { get; set; }

	public int PackageType { get; set; }

	public string BranchCode { get; set; }

	public string BranchName { get; set; }

	public string RegionCode { get; set; }

	public string CommunityCode { get; set; }

	public string Address { get; set; }

	public string DisplayDeliveryAddress { get; set; }

	public decimal DeliveryAmount { get; set; }

	public decimal Amount { get; set; }

	public string Email { get; set; }

	public string PhoneNumber { get; set; }

	public string Passport { get; set; }

	public bool IsForAnotherPerson { get; set; }

	public string AnotherPersonFirstName { get; set; }

	public string AnotherPersonLastName { get; set; }
}
