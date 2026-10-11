using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("2D4C796B-53C1-450F-82CB-E099797E1227")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class ReceivedServiceSpecificationRow
{
	public string Code { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal Amount { get; set; }

	public string UnitCode { get; set; } = string.Empty;

	public bool VAT { get; set; }

	public List<QuantityUnitPart> UnitList { get; set; }
}
