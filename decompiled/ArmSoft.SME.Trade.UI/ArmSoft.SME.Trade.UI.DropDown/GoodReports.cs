using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class GoodReports : DropDownView<GoodReportsDataProvider.DataRow, GoodReportsDataProvider.Param>
{
	public GoodReports()
	{
		DataProvider = new GoodReportsDataProvider();
		FillColumnsFromDataProvider();
		Columns["Code"].Visible = false;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code.ToString();
	}
}
