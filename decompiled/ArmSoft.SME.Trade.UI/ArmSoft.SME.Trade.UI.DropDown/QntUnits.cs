using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class QntUnits : DropDownView<QntUnitsDataProvider.DataRow, QntUnitsDataProvider.Param>
{
	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public QntUnits()
	{
		DataProvider = new QntUnitsDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("Code");
	}

	public override short ValueMemberLength()
	{
		return 3;
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
