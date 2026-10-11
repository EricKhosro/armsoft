namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class AccountInBank : TradeDirectory
{
	public string SettlementAccount { get; set; } = string.Empty;

	public string Curr { get; set; } = string.Empty;

	public string NextPayOrdNumber { get; set; } = string.Empty;

	public bool IsDefault { get; set; }

	public string BankingMode { get; set; } = string.Empty;
}
