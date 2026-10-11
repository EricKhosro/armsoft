using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Common.Models.DPR;

public class GoodsBySegmentsChangeResponseModel
{
	public string GoodCode { get; set; }

	public string SegmentCode { get; set; }

	public ExceptionModel Exception { get; set; }
}
