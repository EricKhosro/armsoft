using ArmSoft.SME.Common.Models.DPR;

namespace ArmSoft.SME.Common.Models.Job;

public class DPRJobEnqueueResponse : JobEnqueueResponse
{
	public DPRInfoExtended Info { get; set; }
}
