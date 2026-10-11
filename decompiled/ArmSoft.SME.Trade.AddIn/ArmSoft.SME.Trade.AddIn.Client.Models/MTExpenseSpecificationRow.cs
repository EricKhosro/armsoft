using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("5D164512-5F40-460B-B00D-1A840240550A")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTExpenseSpecificationRow
{
	public string MTCode { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal Amount { get; set; }

	public string PartyMethod { get; set; } = string.Empty;

	public List<QuantityUnitPart> UnitList { get; set; }
}
