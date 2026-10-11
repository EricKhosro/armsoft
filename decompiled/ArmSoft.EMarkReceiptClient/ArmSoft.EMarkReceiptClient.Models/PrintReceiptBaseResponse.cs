namespace ArmSoft.EMarkReceiptClient.Models;

public class PrintReceiptBaseResponse : ErrorDetailsResponse
{
	public bool succeed { get; set; }

	public long receipt_print_time { get; set; }

	public string crn { get; set; } = string.Empty;

	public int receipt_id { get; set; }
}
