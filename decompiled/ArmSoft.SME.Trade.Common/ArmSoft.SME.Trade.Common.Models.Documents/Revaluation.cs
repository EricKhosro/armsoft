namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class Revaluation : TradeDocument
{
	public bool IsRevalCashes { get; set; }

	public bool IsRevalBankAccs { get; set; }

	public bool IsRevalPartners { get; set; }
}
