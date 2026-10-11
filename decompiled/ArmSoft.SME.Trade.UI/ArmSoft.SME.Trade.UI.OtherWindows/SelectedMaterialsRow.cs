using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class SelectedMaterialsRow
{
	public bool Skipped;

	public byte[] TS = TimeStamp.NewTS();

	public short RowNum { get; set; }

	public int MTID { get; set; } = -1;

	public string MTCode { get; set; } = "";

	public string MTName { get; set; } = "";

	public string QntUnit { get; set; } = "";

	public string QntUnitName { get; set; } = "";

	public decimal BasePrice { get; set; }

	public decimal RetailAvailabilityPrice { get; set; }

	public decimal SalePrice { get; set; }

	public decimal PurchasePrice { get; set; }

	public decimal DiscountPrice { get; set; }

	public decimal OldPrice { get; set; }

	public decimal Percent { get; set; }

	public decimal NewPrice { get; set; }

	public bool IsChecked { get; set; } = true;

	public string Comment { get; set; } = "";
}
