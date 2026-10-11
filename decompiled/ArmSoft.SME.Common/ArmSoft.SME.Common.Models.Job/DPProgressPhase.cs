namespace ArmSoft.SME.Common.Models.Job;

public class DPProgressPhase : ProgressPhaseBase
{
	public override ProgressPhaseBase ToModel()
	{
		return new DPProgressPhase
		{
			DisableCancel = DisableCancel,
			EndDate = EndDate,
			Name = Name,
			Overwritable = Overwritable,
			Row = Row,
			StartDate = StartDate,
			Total = Total,
			Id = Id
		};
	}
}
