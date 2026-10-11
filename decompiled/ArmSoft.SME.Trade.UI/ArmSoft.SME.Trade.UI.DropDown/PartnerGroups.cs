using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class PartnerGroups : DropDownView<PartnerGroupDataProvider.DataRow, PartnerGroupDataProvider.Param>
{
	public override string Caption => Resources.PartnersGroups;

	public override string Name => "PartnerGroup";

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public PartnerGroups(string parentAccount = "", bool showAsTree = true, bool filteredSearch = false, bool applyDefaultDataViewDefinition = true)
	{
		DataProvider = new PartnerGroupDataProvider();
		if (filteredSearch)
		{
			Columns.Add("Code", Resources.Code);
			Columns.Add("Caption", Resources.Name);
			return;
		}
		if (showAsTree)
		{
			Columns.Add("CodeForTree", Resources.Code);
		}
		else
		{
			Columns.Add("Code", Resources.Code);
		}
		Columns.Add("Caption", Resources.Name);
		Columns.Add("Parent", Resources.Parent);
		Columns.Add("Path");
		Columns["Path"].Visible = false;
		Columns.AddSorting("Path");
	}

	public override short ValueMemberLength()
	{
		return 5;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}

	public override string ParentFieldName()
	{
		return "Parent";
	}

	public override string KeyFieldName()
	{
		return "Code";
	}
}
