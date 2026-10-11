using System.Collections.Generic;

namespace ArmSoft.EMarkReceiptClient.Models;

public class PrintReceiptBaseRequest
{
	public List<EMarkReceiptItem> receipt { get; set; } = new List<EMarkReceiptItem>();
}
