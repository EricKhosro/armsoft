using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("EB601FA1-4EC5-4FA0-BE60-CEE03FE41B3E")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTIncomeImportSpecificationRow
{
	public string Code { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal Amount { get; set; }

	public bool VAT { get; set; }

	public decimal CustomAmount { get; set; }

	public decimal CustomPercent { get; set; }

	public decimal CustomDutyAmount { get; set; }

	public decimal VATAmount { get; set; }

	public DateTime? UseFulLife { get; set; }

	public string Country { get; set; } = string.Empty;

	public string Producer { get; set; } = string.Empty;

	public string Property1 { get; set; } = string.Empty;

	public string Property2 { get; set; } = string.Empty;

	public List<QuantityUnitPart> UnitList { get; set; }

	public string RowDescription { get; set; } = string.Empty;
}
