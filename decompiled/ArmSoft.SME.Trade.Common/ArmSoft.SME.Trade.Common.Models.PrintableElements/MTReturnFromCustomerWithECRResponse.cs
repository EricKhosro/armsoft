using ArmSoft.SME.Trade.Common.Models.Documents;

namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class MTReturnFromCustomerWithECRResponse : DocWithECRResponse
{
	public ECRCheckInfo<MTReturnFromCustomerBase.MTRetFromCustSpecificationRow> MTIncomeECRCheck { get; set; } = new ECRCheckInfo<MTReturnFromCustomerBase.MTRetFromCustSpecificationRow>();
}
