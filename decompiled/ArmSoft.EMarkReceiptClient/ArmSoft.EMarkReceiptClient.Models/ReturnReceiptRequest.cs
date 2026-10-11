namespace ArmSoft.EMarkReceiptClient.Models;

public class ReturnReceiptRequest : PrintReceiptBaseRequest
{
	public string crn { get; set; } = string.Empty;

	public int receipt_id { get; set; }
}
