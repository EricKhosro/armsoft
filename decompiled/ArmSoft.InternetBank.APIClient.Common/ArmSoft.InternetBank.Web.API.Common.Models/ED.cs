namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ED : Application
{
	public string PayerAcc { get; set; }

	public int CardType { get; set; }

	public string CardTypeName { get; set; }

	public decimal Amount { get; set; }

	public string BuyerEmail { get; set; }

	public string BuyerPhoneNumber { get; set; }

	public string AnotherPersonFirstName { get; set; }

	public string AnotherPersonLastName { get; set; }

	public string AnotherPersonEmail { get; set; }

	public string AnotherPersonPhoneNumber { get; set; }

	public string ReceivingScheduleDate { get; set; }

	public string ReceivingScheduleTime { get; set; }

	public string WishBody { get; set; }

	public int DesignGroupId { get; set; }

	public string DesignUrl { get; set; }

	public string DesignName { get; set; }

	public int GroupId { get; set; }

	public string GroupDesign { get; set; }

	public string Currency { get; set; }
}
