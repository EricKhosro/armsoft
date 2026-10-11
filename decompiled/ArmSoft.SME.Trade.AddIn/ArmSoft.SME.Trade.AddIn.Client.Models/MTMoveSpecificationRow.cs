using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("B3623BD2-2BEF-4AC1-99EE-AC9000BC7477")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTMoveSpecificationRow
{
	public string MTCode { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal SalePrice { get; set; }

	public decimal SaleAmount { get; set; }

	public string PartyMethod { get; set; } = string.Empty;

	public string RowDescription { get; set; } = string.Empty;

	public List<QuantityUnitPart> UnitList { get; set; }
}
