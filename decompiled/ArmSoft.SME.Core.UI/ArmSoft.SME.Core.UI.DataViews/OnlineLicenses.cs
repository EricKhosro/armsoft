using System;
using System.Windows;
using ArmSoft.Cloud.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViewsData;
using ArmSoft.SME.Core.UI.Documents;

namespace ArmSoft.SME.Core.UI.DataViews;

[DataView(25)]
public class OnlineLicenses : DataView<OnlineLicensesDataProvider.DataRow, OnlineLicensesDataProvider.Param>
{
	public override string HelpId => "AdminTools_OnlineLicenses";

	public override bool AllowDelete => Settings.LoginService.IsAdmin;

	public OnlineLicenses()
	{
		DataProvider = new OnlineLicensesDataProvider();
		FillColumnsFromDataProvider();
	}

	public override void Delete()
	{
		OnlineLicensesDataProvider.DataRow dataRow = (OnlineLicensesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null)
		{
			return;
		}
		if (dataRow.LicenseId == Settings.LoginService.LicenseId.ToString())
		{
			throw new ASException(Resources.LicenseCannotBeRevoked);
		}
		if (MessageBox.Show(Resources.ConfirmLicenseRevocation, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			using (CloudClient cloudClient = Settings.CloudClient)
			{
				cloudClient.Token = Settings.GetApplicationToken();
				cloudClient.AS7X.RemoveOlineLicense(Guid.Parse(dataRow.LicenseId));
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.LicenseId);
			}
		}
	}

	public override void InitContextFunctions()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowLicenses", Resources.ViewProcessesAttachedToTheLicense, ShowLicenses);
		}
	}

	private void ShowLicenses()
	{
		OnlineLicensesDataProvider.DataRow dataRow = (OnlineLicensesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			new OnlineLicenseInstances(dataRow.LicenseId).Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
		}
	}
}
