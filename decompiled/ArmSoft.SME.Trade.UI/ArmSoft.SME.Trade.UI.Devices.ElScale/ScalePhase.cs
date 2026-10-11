using ArmSoft.SME.Common.Models.Job;

namespace ArmSoft.SME.Trade.UI.Devices.ElScale;

public class ScalePhase : ProgressPhaseBase
{
	public override ProgressPhaseBase ToModel()
	{
		return new ScalePhase
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
