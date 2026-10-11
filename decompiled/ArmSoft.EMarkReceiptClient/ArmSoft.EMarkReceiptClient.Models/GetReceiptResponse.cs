using System.Collections.Generic;

namespace ArmSoft.EMarkReceiptClient.Models;

public class GetReceiptResponse
{
	public string parent_crn { get; set; } = string.Empty;

	public int parent_number { get; set; }

	public string crn { get; set; } = string.Empty;

	public int receipt_id { get; set; }

	public long receipt_print_time { get; set; }

	public List<EMarkReceiptItem> receipt { get; set; } = new List<EMarkReceiptItem>();
}
