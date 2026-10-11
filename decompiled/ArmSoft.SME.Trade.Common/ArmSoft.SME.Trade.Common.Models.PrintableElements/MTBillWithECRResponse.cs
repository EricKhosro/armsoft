using System.Collections.Generic;
using ArmSoft.SME.Trade.Common.Models.Documents;

namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class MTBillWithECRResponse : DocWithECRResponse
{
	public ECRCheckInfo<MTInvoiceBase.MTInvoiceSpecificationRow> MTBillECRCheck { get; set; } = new ECRCheckInfo<MTInvoiceBase.MTInvoiceSpecificationRow>();

	public decimal SumAddedBonus { get; set; }

	public decimal SummaPrePayment { get; set; }

	public decimal SummaAccumulatedBonus { get; set; }

	public decimal PrePaymentCashAmount { get; set; }

	public decimal PrePaymentCardAmount { get; set; }

	public decimal PrePaymentTotalAmount { get; set; }

	public List<MTInvoiceBase.MTInvoiceSpecificationRow> MTList { get; set; }

	public MTBill MTBillDoc { get; set; }
}
