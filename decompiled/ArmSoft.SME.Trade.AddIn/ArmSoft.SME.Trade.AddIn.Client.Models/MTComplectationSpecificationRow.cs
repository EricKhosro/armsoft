using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("524C7BFC-6A6C-4D53-ABA1-0AF88FF29B69")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTComplectationSpecificationRow
{
	public string ItemType { get; set; } = string.Empty;

	public string ItemCode { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal Summ { get; set; }

	public string PartyMethod { get; set; } = string.Empty;

	public List<QuantityUnitPart> UnitList { get; set; }
}
