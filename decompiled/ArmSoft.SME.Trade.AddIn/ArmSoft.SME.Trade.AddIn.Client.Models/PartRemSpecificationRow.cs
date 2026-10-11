using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
public class PartRemSpecificationRow
{
	public string PartnerCode { get; set; } = string.Empty;

	public string ContractCode { get; set; } = string.Empty;

	public decimal Amount { get; set; }

	public decimal AmountNCC { get; set; }
}
