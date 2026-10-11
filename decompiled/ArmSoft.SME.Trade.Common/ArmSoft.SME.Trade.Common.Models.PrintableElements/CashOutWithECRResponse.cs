using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class CashOutWithECRResponse : DocWithECRResponse
{
	public ECRCheckInfo<CashCacheRow> CashOutECRCheck { get; set; } = new ECRCheckInfo<CashCacheRow>();
}
