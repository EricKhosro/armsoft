using ArmSoft.SME.Common.Models.InternetBank;
using ArmSoft.SME.Trade.Common.CustomTypes;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class CashIn : TradeDocument
{
	public string OpType { get; set; }

	public string CashOutDesk { get; set; }

	public string Employee { get; set; }

	public string AccountInBankCode { get; set; }

	public PartnerContractGroup BankAcquiringPartnerGroup { get; set; } = new PartnerContractGroup();

	public short? NonCashPaymentMethod { get; set; }

	public string POSTerminalType { get; set; }

	public string PayerName { get; set; }

	public string Base { get; set; }

	public string Appendix { get; set; }

	public string ECRCheckNum { get; set; }

	public string PaymentId { get; set; }

	public bool PrintPartnerTinOnCheck { get; set; }

	public string ChiefAccountant { get; set; }

	public string Cashier { get; set; }

	public bool? PaymentAccepted { get; set; }

	public decimal PartnerRem { get; set; }

	public string ECRCRN { get; set; }

	public string POSTerminalID { get; set; }

	public string POSTerminalAddress { get; set; }

	public IBTransactionMappingRequest IBKey { get; set; }
}
