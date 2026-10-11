using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
public class MTIncomeSpecificationRow
{
	public string Code { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal Discount { get; set; }

	public decimal DiscountPrice { get; set; }

	public decimal Amount { get; set; }

	public bool VAT { get; set; }

	public decimal ExtraPercent { get; set; }

	public decimal SalePrice { get; set; }

	public decimal SaleAmount { get; set; }

	public DateTime? UseFulLife { get; set; }

	public string Country { get; set; } = string.Empty;

	public string Producer { get; set; } = string.Empty;

	public string Property1 { get; set; } = string.Empty;

	public string Property2 { get; set; } = string.Empty;

	public List<QuantityUnitPart> UnitList { get; set; }

	public string Description { get; set; } = string.Empty;
}
