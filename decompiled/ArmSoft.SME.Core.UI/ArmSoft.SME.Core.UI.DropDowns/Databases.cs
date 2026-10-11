using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DataViewsData;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(10)]
public class Databases : DropDownView<DatabasesDataProvider.DataRow, DatabasesDataProvider.Param>
{
	private readonly string _serviceAddress;

	public override string Caption => Resources.Databases;

	public Databases()
	{
		DataProvider = new DatabasesDataProvider();
		FillColumnsFromDataProvider();
		Columns["Size"].Visible = false;
		Columns["FreeSpace"].Visible = false;
	}

	public Databases(string serviceAddress)
	{
		DataProvider = new DatabasesDataProvider();
		_serviceAddress = serviceAddress;
		FillColumnsFromDataProvider();
		Columns["Size"].Visible = false;
		Columns["FreeSpace"].Visible = false;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.ServiceAddress = _serviceAddress;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Name;
	}

	public override short ValueMemberLength()
	{
		return 20;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
