namespace ArmSoft.SME.Common.Models.UserAccess;

public class ProviderPermissionWithType : ProviderPermission
{
	public short ProviderType { get; set; }

	public ProviderPermissionWithType()
	{
	}

	public ProviderPermissionWithType(short providerType, bool allowView, bool allowOperation)
		: base(allowView, allowOperation)
	{
		ProviderType = providerType;
	}
}
