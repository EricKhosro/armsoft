using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class CashInWithECRResponse : DocWithECRResponse
{
	public ECRCheckInfo<CashCacheRow> CashInECRCheck { get; set; } = new ECRCheckInfo<CashCacheRow>();
}
