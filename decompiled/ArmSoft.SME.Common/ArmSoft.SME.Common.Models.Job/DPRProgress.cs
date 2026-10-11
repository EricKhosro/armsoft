using ArmSoft.SME.Common.Models.UIRequests;

namespace ArmSoft.SME.Common.Models.Job;

public class DPRProgress : ProgressBase<DPRProgressPhase>
{
	public UIRequestData UIRequestData { get; set; }
}
