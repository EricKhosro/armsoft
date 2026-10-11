using ArmSoft.SME.Common.Models.Job;

namespace ArmSoft.SME.Client.Data;

public class ProcessPhaseEventArgs
{
	public bool Canceled { get; set; }

	public DPProgressPhase Phase { get; set; }
}
