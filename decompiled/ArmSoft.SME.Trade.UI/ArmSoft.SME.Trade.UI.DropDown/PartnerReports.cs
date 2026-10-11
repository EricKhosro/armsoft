using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class PartnerReports : DropDownView<PartnerReportsDataProvider.DataRow, PartnerReportsDataProvider.Param>
{
	public PartnerReports()
	{
		DataProvider = new PartnerReportsDataProvider();
		FillColumnsFromDataProvider();
		Columns["Code"].Visible = false;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code.ToString();
	}
}
