using ArmSoft.SME.Common.Models.InternetBank;
using ArmSoft.SME.Trade.Common.CustomTypes;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class CashOut : TradeDocument
{
	public string OpType { get; set; }

	public string CashInDesk { get; set; }

	public string Employee { get; set; }

	public PartnerContractGroup BankAcquiringPartnerGroup { get; set; }

	public string AccountInBankCode { get; set; }

	public short? NonCashPaymentMethod { get; set; }

	public string POSTerminalType { get; set; }

	public string ReceiverName { get; set; }

	public string Base { get; set; }

	public string Appendix { get; set; }

	public string NPassport { get; set; }

	public string ECRReturnCheckNum { get; set; }

	public decimal PartnerRem { get; set; }

	public bool PaymentRefundAccepted { get; set; }

	public string POSTerminalAddress { get; set; }

	public IBTransactionMappingRequest IBKey { get; set; }
}
