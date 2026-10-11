namespace ArmSoft.SME.Core.UI.MetaData;

public interface IDataViewSettingsHandler
{
	bool DefaultViewEnabled();

	bool IsViewEnabled(string name);
}
