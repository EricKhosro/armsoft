namespace ArmSoft.SME.Common.Models.UserAccess;

public class POPermissionWithType : POPermission
{
	public short POType { get; set; }

	public POPermissionWithType()
	{
	}

	public POPermissionWithType(short poType, bool allowAdd, bool allowEdit, bool allowView, bool allowDelete)
		: base(allowAdd, allowEdit, allowView, allowDelete)
	{
		POType = poType;
	}
}
