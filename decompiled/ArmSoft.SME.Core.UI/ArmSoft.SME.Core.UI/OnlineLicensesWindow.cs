using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViewsData;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI;

public class OnlineLicensesWindow : DialogWindow
{
	private readonly DocumentGridControl _firstGrid;

	private readonly List<OnlineLicensesWithInstancesDataProvider.DataRow> _firstList;

	private readonly LayoutGroup _workingAppsLayout;

	private readonly DocumentGridControl _secondGrid;

	private readonly string _userToken;

	private readonly KeyInfoExt _keyInfo;

	private List<OnlineLicensesWithInstancesDataProvider.Instance> _secondList;

	public OnlineLicensesWindow(KeyInfoExt keyInfo, string userToken)
	{
		_userToken = userToken;
		_keyInfo = keyInfo;
		bool flag = true;
		using (CloudClient cloudClient = new CloudClient("", Settings.B2BCloudBaseAddress))
		{
			cloudClient.UserToken = userToken;
			flag = cloudClient.AS7X.IsMeOwnerForCustomer(keyInfo.CustomerId);
		}
		OnlineLicensesWithInstancesDataProvider onlineLicensesWithInstancesDataProvider = new OnlineLicensesWithInstancesDataProvider();
		_firstList = onlineLicensesWithInstancesDataProvider.Execute(new OnlineLicensesWithInstancesDataProvider.Param
		{
			Key = keyInfo.Serial,
			UserToken = userToken
		}, null).Rows;
		Title = ArmSoft.SME.Common.Resources.OnlineLicenses;
		Height = 450.0;
		Width = 700.0;
		WindowStyle = WindowStyle.SingleBorderWindow;
		ShowIcon = false;
		_txtStatus.Visibility = Visibility.Collapsed;
		SizeToContent = SizeToContent.Manual;
		WindowState = WindowState.Normal;
		TextEditExt textEditExt = AddStringControl("KeySerial", ArmSoft.SME.Common.Resources.Key, 500);
		textEditExt.IsReadOnly = true;
		textEditExt.EditValue = keyInfo.Serial;
		textEditExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		textEditExt.Width = 450.0;
		TextEditExt textEditExt2 = AddStringControl("Customer", ArmSoft.SME.Common.Resources.Client, 500);
		textEditExt2.IsReadOnly = true;
		textEditExt2.EditValue = keyInfo.Customername;
		textEditExt2.HorizontalAlignment = HorizontalAlignment.Stretch;
		textEditExt2.Width = 450.0;
		TextEditExt textEditExt3 = AddStringControl("WSLimit", ArmSoft.SME.Common.Resources.WorkstationsLimit, 500);
		textEditExt3.IsReadOnly = true;
		textEditExt3.EditValue = keyInfo.WSLimit;
		textEditExt3.Width = 450.0;
		_firstGrid = AddGridControl("firstGrid");
		_firstGrid.AllowAdd = false;
		_firstGrid.AllowDelete = false;
		_firstGrid.AllowDrop = false;
		_firstGrid.AllowEdit = false;
		_firstGrid.ShowButtons = false;
		_firstGrid.View.AutoWidth = true;
		_firstGrid.AddStringColumn("Station", ArmSoft.SME.Common.Resources.Workstation, 8).ReadOnly = true;
		DocumentGridColumn documentGridColumn = _firstGrid.AddStringColumn("LastRenewed", ArmSoft.SME.Common.Resources.RenewDate, 7);
		documentGridColumn.ReadOnly = true;
		documentGridColumn.Width = 150.0;
		_firstGrid.AddStringColumn("IPAddress", ArmSoft.SME.Common.Resources.IPAddress, 7).ReadOnly = true;
		_firstGrid.AddStringColumn("User", ArmSoft.SME.Common.Resources.UserName, 10).ReadOnly = true;
		if (flag)
		{
			DocumentGridColumn documentGridColumn2 = _firstGrid.AddStringColumn("Remove", "", 6);
			documentGridColumn2.ReadOnly = true;
			HyperlinkEditSettings hyperlinkEditSettings = new HyperlinkEditSettings
			{
				TextWrapping = TextWrapping.NoWrap
			};
			hyperlinkEditSettings.RequestNavigation += new SafeEventHandler<HyperlinkEditRequestNavigationEventArgs>(Hyperlink_RemoveRow).Handler;
			documentGridColumn2.EditSettings = hyperlinkEditSettings;
		}
		_firstGrid.ItemsSource = _firstList;
		object focusedRow = _firstGrid.Grid.GetFocusedRow();
		if (focusedRow != null)
		{
			_secondList = ((OnlineLicensesWithInstancesDataProvider.DataRow)focusedRow).Instances;
		}
		_workingAppsLayout = AddVerticalLayoutGroup(null, LayoutGroupView.GroupBox, string.Format(ArmSoft.SME.Common.Resources.CurrentWorkingApps, (_secondList != null) ? _secondList.Count.ToString() : "0"));
		_workingAppsLayout.IsCollapsible = true;
		_workingAppsLayout.IsCollapsed = true;
		_secondGrid = AddGridControl(_workingAppsLayout, "secondGrid");
		_secondGrid.AllowAdd = false;
		_secondGrid.AllowDelete = false;
		_secondGrid.AllowDrop = false;
		_secondGrid.AllowEdit = false;
		_secondGrid.ShowButtons = false;
		_secondGrid.View.AutoWidth = true;
		_secondGrid.AddStringColumn("DBName", ArmSoft.SME.Common.Resources.DataBaseName, 10).ReadOnly = true;
		_secondGrid.AddStringColumn("User", ArmSoft.SME.Common.Resources.UserName, 13).ReadOnly = true;
		_secondGrid.AddStringColumn("ServerName", ArmSoft.SME.Common.Resources.Server, 10).ReadOnly = true;
		_secondGrid.AddStringColumn("Version", ArmSoft.SME.Common.Resources.VersionCapital, 5).ReadOnly = true;
		_secondGrid.AddStringColumn("ProcessId", ArmSoft.SME.Common.Resources.ProcessId, 5).ReadOnly = true;
		_firstGrid.View.FocusedRowHandleChanged += new SafeEventHandler<FocusedRowHandleChangedEventArgs>(OnRowInfoChanged).Handler;
		OnRowInfoChanged();
	}

	private void Hyperlink_RemoveRow(object sender, HyperlinkEditRequestNavigationEventArgs e)
	{
		OnlineLicensesWithInstancesDataProvider.DataRow dataRow = (OnlineLicensesWithInstancesDataProvider.DataRow)_firstGrid.Grid.GetFocusedRow();
		if (dataRow != null && MessageBox.Show(ArmSoft.SME.Common.Resources.ConfirmLicenseRevocation, "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			using (CloudClient cloudClient = new CloudClient("", Settings.B2BCloudBaseAddress))
			{
				cloudClient.UserToken = _userToken;
				cloudClient.AS7X.RemoveOlineLicense(Guid.Parse(dataRow.LicenseId.ToString()), _keyInfo.Serial);
			}
			int focusedRowHandle = _firstGrid.View.FocusedRowHandle;
			_firstGrid.View.DeleteRow(_firstGrid.Grid.GetRowVisibleIndexByHandle(focusedRowHandle));
			_firstGrid.Grid.RefreshData();
			if (_firstList == null || _firstList.Count == 0)
			{
				_secondGrid.View.DeleteRow(0);
				_workingAppsLayout.Header = string.Format(ArmSoft.SME.Common.Resources.CurrentWorkingApps, "0");
				_workingAppsLayout.UpdateLayout();
			}
		}
	}

	private void OnRowInfoChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		OnRowInfoChanged();
	}

	private void OnRowInfoChanged()
	{
		try
		{
			OnlineLicensesWithInstancesDataProvider.DataRow dataRow = (OnlineLicensesWithInstancesDataProvider.DataRow)_firstGrid.Grid.GetFocusedRow();
			if (dataRow != null)
			{
				_secondList = dataRow.Instances;
				_secondGrid.ItemsSource = _secondList;
				_workingAppsLayout.Header = string.Format(ArmSoft.SME.Common.Resources.CurrentWorkingApps, (_secondList != null) ? _secondList.Count.ToString() : "0");
				_workingAppsLayout.UpdateLayout();
			}
		}
		catch (Exception)
		{
		}
	}
}
