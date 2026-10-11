namespace ArmSoft.SME.Core.UI.Services;

public interface IAppUpdateService
{
	bool UpdateAvailable { get; set; }

	void TryUpdateAppFiles(bool updateRequired = false);

	void TryUpdateApp();
}
