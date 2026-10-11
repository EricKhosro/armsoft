namespace ArmSoft.Cloud.Client.Discount;

public class CardInfo : CardCreateRequest
{
	public int CardId { get; set; }

	public decimal Balance { get; set; }

	public decimal PurchaseAmount { get; set; }
}
