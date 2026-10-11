using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("ECD69914-DAAF-473A-8FC8-F74C2239E50B")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTRetToSupSpecificationRow
{
	public string Storage { get; set; } = string.Empty;

	public string Code { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal Discount { get; set; }

	public decimal DiscountPrice { get; set; }

	public decimal Amount { get; set; }

	public decimal SalePrice { get; set; }

	public decimal SaleAmount { get; set; }

	public bool VAT { get; set; }

	public string PartyMethod { get; set; } = string.Empty;

	public List<QuantityUnitPart> UnitList { get; set; }
}
