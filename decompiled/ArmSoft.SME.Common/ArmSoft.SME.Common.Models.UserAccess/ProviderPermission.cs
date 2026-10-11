namespace ArmSoft.SME.Common.Models.UserAccess;

public class ProviderPermission
{
	public bool AllowView { get; set; }

	public bool AllowOperation { get; set; }

	public ProviderPermission()
	{
	}

	public ProviderPermission(bool allowView, bool allowOperation)
	{
		AllowView = allowView;
		AllowOperation = allowOperation;
	}
}
