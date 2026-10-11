namespace ArmSoft.SME.Common.Models.Job;

public class DPRProgressPhase : ProgressPhaseBase
{
	public override ProgressPhaseBase ToModel()
	{
		return new DPRProgressPhase
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
