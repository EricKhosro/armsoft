using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("E48A5C07-7C0B-4E64-9E5D-9D8F22C955AD")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTDecomplectationSpecificationRow
{
	public string ItemCode { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal PricePart { get; set; }

	public decimal AmountPart { get; set; }

	public DateTime? UseFulLife { get; set; }

	public string Country { get; set; } = string.Empty;

	public string Producer { get; set; } = string.Empty;

	public string Property1 { get; set; } = string.Empty;

	public string Property2 { get; set; } = string.Empty;

	public List<QuantityUnitPart> UnitList { get; set; }
}
