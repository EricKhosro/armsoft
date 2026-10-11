using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDownsData;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(4)]
public class UsersFilter : DropDownView<UserDataProvider.DataRow, UserDataProvider.Param>
{
	public bool ParamOnlyDbUsers { get; set; }

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public UsersFilter()
	{
		DataProvider = new UserDataProvider();
		FillColumnsFromDataProvider();
		Columns["UserId"].Visible = false;
	}

	public override short ValueMemberLength()
	{
		return 50;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Name;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Description;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.OnlyDbUsers = ParamOnlyDbUsers;
	}
}
