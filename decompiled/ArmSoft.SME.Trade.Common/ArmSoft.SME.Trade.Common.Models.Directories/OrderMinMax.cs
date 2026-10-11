namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class OrderMinMax : TradeDirectory
{
	public string StorageCode { get; set; }

	public decimal MinQty { get; set; }

	public decimal MaxQty { get; set; }

	public string StorageCodeOld { get; set; }

	public string OldCode { get; set; }
}
