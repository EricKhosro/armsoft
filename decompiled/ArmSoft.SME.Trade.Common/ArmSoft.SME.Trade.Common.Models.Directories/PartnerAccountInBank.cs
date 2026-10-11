namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class PartnerAccountInBank : TradeDirectory
{
	public string SettlementAccount { get; set; }

	public string SettlementAccountCaption { get; set; }

	public bool IsDefault { get; set; }

	public string OldSettlementAccount { get; set; }
}
