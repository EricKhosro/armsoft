using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDownsData;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(18)]
public class OurCurrenciesDataView : DropDownView<AvailableCurrenciesDataProvider.DataRow, AvailableCurrenciesDataProvider.Param>
{
	public OurCurrenciesDataView()
	{
		DataProvider = new AvailableCurrenciesDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("Curr");
	}

	public override short ValueMemberLength()
	{
		return 3;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Curr;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
