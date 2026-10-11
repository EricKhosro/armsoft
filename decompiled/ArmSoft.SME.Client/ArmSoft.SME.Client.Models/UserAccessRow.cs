namespace ArmSoft.SME.Client.Models;

public class UserAccessRow
{
	public short SubSystem { get; set; }

	public short AccessID { get; set; }

	public bool View { get; set; }

	public bool Operation { get; set; }
}
