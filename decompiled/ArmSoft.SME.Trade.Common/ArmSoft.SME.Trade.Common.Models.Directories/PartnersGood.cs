namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class PartnersGood : TradeDirectory
{
	public string PartCodeOld { get; set; }

	public string PartTIN { get; set; }

	public string Type { get; set; }

	public string PartItemCode { get; set; }

	public string PartItemCodeOld { get; set; }

	public string PartItemCaption { get; set; }

	public string PartItemUnit { get; set; }

	public string PartItemUnitOld { get; set; }

	public short PartItemUnitID { get; set; }

	public string PartItemUnitName { get; set; }

	public string PartItemUnitNameOld { get; set; }

	public decimal Coeff { get; set; }

	public int ItemID { get; set; }

	public string ItemCode { get; set; }

	public string ItemCaption { get; set; }

	public string ItemUnit { get; set; }

	public string ItemUnitName { get; set; }
}
