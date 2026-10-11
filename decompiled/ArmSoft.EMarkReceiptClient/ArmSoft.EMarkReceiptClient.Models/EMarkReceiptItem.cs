using System.Collections.Generic;

namespace ArmSoft.EMarkReceiptClient.Models;

public class EMarkReceiptItem
{
	public string product_name { get; set; } = string.Empty;

	public int quantity { get; set; }

	public string measurement_unit { get; set; } = string.Empty;

	public string tnved { get; set; } = string.Empty;

	public string gtin { get; set; } = string.Empty;

	public List<string> emarks { get; set; } = new List<string>();
}
