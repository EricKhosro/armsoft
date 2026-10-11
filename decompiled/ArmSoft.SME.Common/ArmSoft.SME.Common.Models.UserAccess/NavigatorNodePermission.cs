namespace ArmSoft.SME.Common.Models.UserAccess;

public class NavigatorNodePermission
{
	public bool AllowView { get; set; }

	public bool AllowOperation { get; set; }

	public NavigatorNodePermission()
	{
	}

	public NavigatorNodePermission(bool allowView, bool allowOperation)
	{
		AllowView = allowView;
		AllowOperation = allowOperation;
	}
}
