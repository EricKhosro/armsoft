using ArmSoft.SME.Client.Data.MetaData;
using ArmSoft.SME.Core.UI.DataViews;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(19)]
public class AWPDefinitions : DropDownView<AWPDefinitionsDataProvider.DataRow, AWPDefinitionsDataProvider.Param>
{
	public AWPDefinitions()
	{
		DataProvider = new AWPDefinitionsDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	public override short ValueMemberLength()
	{
		return 20;
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
