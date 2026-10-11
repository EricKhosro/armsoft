using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class CashierShift : DropDownView<CashierShiftDataProvider.DataRow, CashierShiftDataProvider.Param>
{
	public string CashDesk { get; set; }

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public CashierShift(string cashDesk = "")
	{
		CashDesk = cashDesk;
		DataProvider = new CashierShiftDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("CashDesk", descending: true);
		Columns.AddSorting("ShiftNumber");
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.CashDesk = CashDesk;
	}

	public override short ValueMemberLength()
	{
		return 12;
	}

	public override string ValueMember(int index)
	{
		return Data[index].ShiftNumber;
	}

	public override string CommentMember(int index)
	{
		return Data[index].ShiftNumber;
	}
}
