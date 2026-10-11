using ArmSoft.SME.Client.Data.MetaData;
using ArmSoft.SME.Core.UI.DataViews;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(22)]
public class UserScriptsDefinitions : DropDownView<UserScriptsDefinitionsDataProvider.DataRow, UserScriptsDefinitionsDataProvider.Param>
{
	public UserScriptsDefinitions()
	{
		DataProvider = new UserScriptsDefinitionsDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	public override short ValueMemberLength()
	{
		return 32;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Name;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
