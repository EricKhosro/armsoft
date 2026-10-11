using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("CDA4E86A-4234-4316-B396-D5297546DA00")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTIncomeRemSpecificationRow
{
	public string Storage { get; set; } = string.Empty;

	public string Code { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal Amount { get; set; }

	public decimal CurrPrice { get; set; }

	public decimal CurrAmount { get; set; }

	public decimal SalePrice { get; set; }

	public decimal SaleAmount { get; set; }

	public bool VAT { get; set; }

	public decimal VATPrice { get; set; }

	public decimal VATAmount { get; set; }

	public DateTime? UseFulLife { get; set; }

	public string Country { get; set; } = string.Empty;

	public string Producer { get; set; } = string.Empty;

	public string Property1 { get; set; } = string.Empty;

	public string Property2 { get; set; } = string.Empty;

	public List<QuantityUnitPart> UnitList { get; set; }
}
