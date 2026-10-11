using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDownsData;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(8)]
public class Backups : DropDownView<BackupsDataProvider.DataRow, BackupsDataProvider.Param>
{
	private readonly string _serviceAddress;

	public Backups()
	{
		DataProvider = new BackupsDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("DateCreated", descending: true);
	}

	public Backups(string serviceAddress)
	{
		DataProvider = new BackupsDataProvider();
		_serviceAddress = serviceAddress;
		FillColumnsFromDataProvider();
		Columns.AddSorting("DateCreated", descending: true);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.ServiceAddress = _serviceAddress;
	}

	public override string ValueMember(int index)
	{
		return Data[index].FileName;
	}

	public override short ValueMemberLength()
	{
		return 500;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Comment;
	}
}
