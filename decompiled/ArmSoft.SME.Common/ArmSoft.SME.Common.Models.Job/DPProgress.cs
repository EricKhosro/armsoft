using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Common.Models.Job;

public class DPProgress : ProgressBase<DPProgressPhase>
{
	public DPProgress()
	{
	}

	public DPProgress(ExecutionProgress progress)
		: base(progress)
	{
	}
}
