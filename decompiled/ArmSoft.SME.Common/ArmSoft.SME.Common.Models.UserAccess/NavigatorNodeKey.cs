namespace ArmSoft.SME.Common.Models.UserAccess;

public struct NavigatorNodeKey
{
	public short SubsystemId { get; set; }

	public short NodeAccessId { get; set; }

	public NavigatorNodeKey()
	{
		SubsystemId = 0;
		NodeAccessId = 0;
	}

	public NavigatorNodeKey(short subsystemId, short nodeAccessId)
	{
		SubsystemId = subsystemId;
		NodeAccessId = nodeAccessId;
	}
}
