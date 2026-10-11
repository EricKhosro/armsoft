namespace ArmSoft.SME.Common.Models.AppManager;

public class ChangeDbStateRequest
{
	public bool Operational { get; set; }

	public string LogComment { get; set; }
}
