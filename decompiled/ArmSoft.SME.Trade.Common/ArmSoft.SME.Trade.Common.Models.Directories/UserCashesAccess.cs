namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class UserCashesAccess : TradeDirectory
{
	public int UserId { get; set; }

	public bool Allow { get; set; }
}
