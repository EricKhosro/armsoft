namespace ArmSoft.EMarkReceiptClient.Models;

public class GetReceiptRequest
{
	public int receipt_id { get; set; }

	public string crn { get; set; } = string.Empty;
}
