using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class DiscountGroups : DropDownView<DiscountGroupDataProvider.DataRow, DiscountGroupDataProvider.Param>
{
	private readonly string _currCode;

	public override string Caption => Resources.DiscountGroups;

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public DiscountGroups(string currCode = "")
	{
		_currCode = currCode;
		DataProvider = new DiscountGroupDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("Code");
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
}
