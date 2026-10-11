namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTBillReturn : MTReturnFromCustomerBase
{
	public decimal SummaChange { get; set; }

	public decimal BaseDocSummaCash { get; set; }

	public decimal BaseDocSummaPlCard { get; set; }

	public decimal SummaCredit { get; set; }

	public decimal BaseDocSummaPrePayment { get; set; }

	public string POSTerminalType { get; set; }

	public string PrePaymentECRCheckNum { get; set; }

	public string BasePrePaymentECRCheckNum { get; set; }

	public decimal PrePaymentCashOutAmount { get; set; }

	public decimal PrePaymentCardOutAmount { get; set; }

	public short NonCashPaymentMethod { get; set; }

	public string POSTerminalAddress { get; set; }
}
