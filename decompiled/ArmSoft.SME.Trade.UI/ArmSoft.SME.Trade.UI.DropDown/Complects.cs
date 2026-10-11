using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class Complects : DropDownView<ComplectDataProvider.DataRow, ComplectDataProvider.Param>
{
	public string Group { get; set; }

	public override string Caption => Resources.Complects;

	public override string Name => "Complects";

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public Complects(string group = "")
	{
		DataProvider = new ComplectDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("MTCode");
		Group = group;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.Group = Group;
	}

	public override short ValueMemberLength()
	{
		return 20;
	}

	public override string ValueMember(int index)
	{
		return Data[index].MTCode;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
