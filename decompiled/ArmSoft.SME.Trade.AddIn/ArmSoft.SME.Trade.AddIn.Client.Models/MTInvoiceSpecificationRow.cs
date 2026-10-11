using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("7AE963F7-4155-4C0B-A13C-DE01051F54A5")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTInvoiceSpecificationRow
{
	public string ItemType { get; set; } = string.Empty;

	public string Storage { get; set; } = string.Empty;

	public string ItemCode { get; set; } = string.Empty;

	public string ItemName { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal Discount { get; set; }

	public decimal DiscountPrice { get; set; }

	public decimal Amount { get; set; }

	public decimal DiscountAmount { get; set; }

	public decimal AmountWithoutVAT { get; set; }

	public decimal BeginAmount { get; set; }

	public decimal EnvironmentalFeePercent { get; set; }

	public decimal EnvironmentalFeeAmount { get; set; }

	public bool VAT { get; set; }

	public string DealType { get; set; } = string.Empty;

	public string BarCode { get; set; } = string.Empty;

	public decimal UsedBonus { get; set; }

	public decimal AccumulatedBonus { get; set; }

	public string PartyMethod { get; set; } = string.Empty;

	public string RowDescription { get; set; } = string.Empty;

	public bool Deleted { get; set; }

	public bool Gift { get; set; }

	public List<QuantityUnitPart> UnitList { get; set; }
}
