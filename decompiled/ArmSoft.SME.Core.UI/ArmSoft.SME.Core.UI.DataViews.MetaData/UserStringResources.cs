using System.Windows;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.MetaData;

namespace ArmSoft.SME.Core.UI.DataViews.MetaData;

[DataView(39)]
public class UserStringResources : DataView<UserResourcesDataProvider.DataRow, ExtendableParameter>
{
	public override bool AllowAdd => true;

	public override bool AllowEdit => true;

	public override bool AllowDelete => true;

	public UserStringResources()
	{
		DataProvider = new UserResourcesDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Name");
		}
	}

	public override void Add()
	{
		UserStringResourceWindow userStringResourceWindow = new UserStringResourceWindow(DialogWindowMode.NewMode);
		if (userStringResourceWindow.Invoke())
		{
			Settings.HostingEnvironment.CurrentViewPanel.Update(userStringResourceWindow.NameEdit.Text);
		}
	}

	public override void Edit()
	{
		UserResourcesDataProvider.DataRow dataRow = (UserResourcesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		UserStringResourceWindow userStringResourceWindow = new UserStringResourceWindow(DialogWindowMode.EditMode, dataRow.Name);
		if (userStringResourceWindow.Invoke())
		{
			Settings.HostingEnvironment.CurrentViewPanel.Update(userStringResourceWindow.NameEdit.Text);
		}
	}

	public override void Delete()
	{
		UserResourcesDataProvider.DataRow dataRow = (UserResourcesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null && MessageBox.Show(string.Format(Resources.ConfRecordDel, dataRow.Name), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			Settings.StringResourceService.Delete(dataRow.Name);
			Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.Name);
		}
	}
}
