namespace ArmSoft.SME.Common.Models.UserAccess;

public class NavigatorNodeAccess
{
	public short SubsystemId { get; set; }

	public short NodeAccessId { get; set; }

	public bool AllowView { get; set; }

	public bool AllowOperation { get; set; }

	public NavigatorNodeAccess()
	{
	}

	public NavigatorNodeAccess(short subsystemId, short nodeAccessId, bool allowView, bool allowOperation)
	{
		SubsystemId = subsystemId;
		NodeAccessId = nodeAccessId;
		AllowView = allowView;
		AllowOperation = allowOperation;
	}
}
