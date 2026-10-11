namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class PayOrder : TradeDocument
{
	public string AccountInBankCode { get; set; }

	public string PayerName { get; set; }

	public string PayerSettlAcc { get; set; }

	public string PayerTaxCode { get; set; }

	public string ReceiverName { get; set; }

	public string ReceiverSettlAcc { get; set; }

	public string ReceiverTaxCode { get; set; }

	public string ReceiverLegalStatus { get; set; }

	public string PaymentAim { get; set; }

	public bool? IsCreateFromConvert { get; set; }

	public decimal? PartnerRem { get; set; }
}
