namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class MTBarCode : TradeDirectory
{
	public string BarCode { get; set; }

	public string BarCodeOld { get; set; }

	public int MTID { get; set; }

	public string MTCode { get; set; }

	public string MTCaption { get; set; }

	public string QntUnit { get; set; }

	public string QntUnitBrief { get; set; }

	public bool IsWeight { get; set; }
}
