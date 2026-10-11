namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class UserStorageOptions : TradeDirectory
{
	public int UserId { get; set; }

	public bool AllowOper { get; set; }

	public bool AllowReportView { get; set; }

	public bool AllowNegExp { get; set; }

	public bool SalesWithZeroCost { get; set; }
}
