using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.DPR;

public class GoodsBySegmentsSelectedChangeRequestModel
{
	public List<string> GoodsCodes { get; set; }

	public string AddToSegmentCode { get; set; }

	public string RemoveFromSegmentCode { get; set; }
}
