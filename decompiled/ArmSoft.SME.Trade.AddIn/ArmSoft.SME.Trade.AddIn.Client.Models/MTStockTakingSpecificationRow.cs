using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("E824817C-4BF4-4B76-B088-7C9ABCB1314E")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTStockTakingSpecificationRow
{
	public string MTCode { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal EstimatedQuantity { get; set; }

	public decimal ActualQuantity { get; set; }

	public decimal VarianceQuantity { get; set; }

	public decimal EstimatedAmount { get; set; }

	public decimal Price { get; set; }

	public decimal SalePrice { get; set; }

	public decimal ActualAmount { get; set; }

	public decimal SaleSumma { get; set; }

	public string MTName { get; set; } = string.Empty;

	public string BarCode { get; set; } = string.Empty;

	public List<QuantityUnitPart> UnitList { get; set; }
}
