using System.Collections.Generic;
using ArmSoft.SME.Trade.Common.Models.Documents;

namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class MTBillReturnWithECRResponse : DocWithECRResponse
{
	public ECRCheckInfo<MTInvoiceBase.MTInvoiceSpecificationRow> MTBillReturnECRCheck { get; set; } = new ECRCheckInfo<MTInvoiceBase.MTInvoiceSpecificationRow>();

	public decimal AccumulatedBonusSum { get; set; }

	public string BaseDocPrePaymentECRNum { get; set; }

	public decimal PrePaymentCashOutAmount { get; set; }

	public decimal PrePaymentCardOutAmount { get; set; }

	public decimal PrePaymentTotalOutAmount { get; set; }

	public List<MTReturnFromCustomerBase.MTRetFromCustSpecificationRow> MTBillReturnList { get; set; }

	public MTBillReturn MTBillReturnDoc { get; set; }
}
