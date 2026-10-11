namespace ArmSoft.SME.Core.UI.MetaData;

public abstract class DataViewSettingsHandler : IDataViewSettingsHandler
{
	public virtual bool DefaultViewEnabled()
	{
		return true;
	}

	public virtual bool IsViewEnabled(string name)
	{
		return true;
	}
}
