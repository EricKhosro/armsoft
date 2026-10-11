using System.Collections.Generic;
using ArmSoft.SME.Trade.Common.CustomTypes;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTBill : MTInvoiceBase
{
	public decimal? SummaChange { get; set; }

	public PartnerContractGroup BankAcquiring { get; set; } = new PartnerContractGroup();

	public decimal? SummaCash { get; set; }

	public decimal? SummaPlCard { get; set; }

	public decimal? SummaCredit { get; set; }

	public decimal? SummaPayByBonus { get; set; }

	public decimal? SummaAccumulatedBonus { get; set; }

	public decimal? SumAddedBonus { get; set; }

	public decimal? SummaPrePayment { get; set; }

	public bool? PrintPartnerTinOnCheck { get; set; }

	public string POSTerminalID { get; set; }

	public string POSTerminalAddress { get; set; }

	public string POSTerminalType { get; set; }

	public string POSCRN { get; set; }

	public string ECRCheckNum { get; set; }

	public string PrePaymentECRCheckNum { get; set; }

	public short NonCashPaymentMethod { get; set; }

	public string PriceListType { get; set; }

	public string DiscountPriceListType { get; set; }

	public PartnerContractGroup PrepaymentGroup { get; set; } = new PartnerContractGroup();

	public string PaymentId { get; set; }

	public bool PaymentAccepted { get; set; }

	public List<DiscountLogRow> DiscountLogsList { get; set; } = new List<DiscountLogRow>();

	public bool IsCheckLimitAnswerYesforOtherPartnerPrepayment { get; set; }

	public string ECRCRN { get; set; }

	public string OnlineStore { get; set; } = string.Empty;

	public string OnlineStoreOrderNumber { get; set; } = string.Empty;

	public string OnlineStoreOrderStatus { get; set; } = string.Empty;

	public decimal PrePaymentCashAmount { get; set; }

	public decimal PrePaymentCardAmount { get; set; }
}
