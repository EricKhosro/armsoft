using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class Properties : DropDownView<PropertyDataProvider.DataRow, PropertyDataProvider.Param>
{
	public override string Caption => Resources.Properties;

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public Properties()
	{
		DataProvider = new PropertyDataProvider();
		FillColumnsFromDataProvider();
		Columns["NextProperty"].Visible = false;
		Columns.AddSorting("Code");
	}

	public override short ValueMemberLength()
	{
		return 4;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
