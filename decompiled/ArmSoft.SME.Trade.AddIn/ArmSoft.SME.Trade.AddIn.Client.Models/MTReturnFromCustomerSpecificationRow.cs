using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("87B6EB5D-3293-4768-9380-C2683741C876")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTReturnFromCustomerSpecificationRow
{
	public string ItemType { get; set; } = string.Empty;

	public string Storage { get; set; } = string.Empty;

	public string Code { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal Amount { get; set; }

	public decimal Discount { get; set; }

	public decimal BeginPrice { get; set; }

	public decimal BeginAmount { get; set; }

	public decimal CostPrice { get; set; }

	public decimal CostPriceVAT { get; set; }

	public decimal CurrCostPrice { get; set; }

	public bool VAT { get; set; }

	public string DealType { get; set; } = string.Empty;

	public int BaseRowID { get; set; } = -1;

	public DateTime? UseFulLife { get; set; }

	public string Country { get; set; } = string.Empty;

	public string Producer { get; set; } = string.Empty;

	public string Property1 { get; set; } = string.Empty;

	public string Property2 { get; set; } = string.Empty;

	public string RowDescription { get; set; } = string.Empty;

	public decimal UsedBonus { get; set; }

	public decimal AccumulatedBonus { get; set; }

	public List<QuantityUnitPart> UnitList { get; set; }
}
