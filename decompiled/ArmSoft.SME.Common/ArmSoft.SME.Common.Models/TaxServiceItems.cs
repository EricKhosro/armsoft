using ArmSoft.SME.Common.Models.ASReport;

namespace ArmSoft.SME.Common.Models;

public class TaxServiceItems
{
	public InvoiceDetails InvoiceDetails { get; set; }

	public string DocNumbers { get; set; }

	public bool IsAssigned { get; set; }

	public string SerialNumReason { get; set; }
}
