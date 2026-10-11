using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Client.Data;

public class PrimaryPhaseEventArgs
{
	public List<ExecutionPhase> Phases { get; internal set; }
}
