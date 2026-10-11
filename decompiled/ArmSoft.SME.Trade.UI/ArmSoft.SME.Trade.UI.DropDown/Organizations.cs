using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class Organizations : DropDownView<OrganizationsDataProvider.DataRow, OrganizationsDataProvider.Param>
{
	public string OrgName { get; set; }

	public override string Caption => Resources.Partners;

	public Organizations(string name)
	{
		OrgName = name;
		DataProvider = new OrganizationsDataProvider();
		FillColumnsFromDataProvider();
		Columns["Caption"].Visible = false;
		Columns["Address"].Visible = false;
		Columns["BusinessAddress"].Visible = false;
		Columns["ManagerPost"].Visible = false;
		Columns["ManagerFIO"].Visible = false;
		Columns["ChiefaccFIO"].Visible = false;
		Columns["ChiefaccPost"].Visible = false;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.OrgName = OrgName;
	}
}
