using ArmSoft.SME.Trade.Common.CustomTypes;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class PartnerRemMove : TradeDocument
{
	public PartnerContractGroup CorrPartnerContractGroup { get; set; }

	public string CorrCurrCode { get; set; }

	public decimal? PartnerRem { get; set; }

	public string PartnerName { get; set; }

	public string CorrPartnerName { get; set; }

	public decimal? ReceiverRem { get; set; }
}
