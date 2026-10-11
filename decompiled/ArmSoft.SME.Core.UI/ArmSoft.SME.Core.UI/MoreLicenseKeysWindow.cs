using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Core.UI;

public class MoreLicenseKeysWindow : DialogWindow
{
	private DocumentGridControl _firstGrid;

	private Button _openKeyInfoButton;

	private readonly string _accessToken;

	public MoreLicenseKeysWindow(List<KeyInfoExt> licenseKeysInfosList, string accessToken)
	{
		Title = ArmSoft.SME.Common.Resources.AvailableKeys;
		Width = 700.0;
		Height = 450.0;
		WindowStyle = WindowStyle.SingleBorderWindow;
		ShowIcon = false;
		SizeToContent = SizeToContent.Manual;
		WindowState = WindowState.Normal;
		_accessToken = accessToken;
		_txtStatus.Visibility = Visibility.Collapsed;
		_okButton.Visibility = Visibility.Collapsed;
		_cancelButton.Visibility = Visibility.Collapsed;
		AddLicenseKeysGridControl(licenseKeysInfosList);
		AddButtons();
	}

	private void AddLicenseKeysGridControl(List<KeyInfoExt> licenseKeysInfosList)
	{
		_firstGrid = AddGridControl("FirstGrid");
		_firstGrid.AllowAdd = false;
		_firstGrid.AllowDelete = false;
		_firstGrid.AllowDrop = false;
		_firstGrid.AllowEdit = false;
		_firstGrid.AllowInsert = false;
		_firstGrid.ShowButtons = false;
		_firstGrid.View.AutoWidth = true;
		_firstGrid.MouseDoubleClick += new SafeEventHandler<RoutedEventArgs>(SerialHyperlinkRequestNavigate).Handler;
		_firstGrid.ItemsSource = licenseKeysInfosList;
		DocumentGridColumn documentGridColumn = _firstGrid.AddStringColumn("Serial", ArmSoft.SME.Common.Resources.Serial, 10);
		documentGridColumn.ReadOnly = true;
		documentGridColumn.Width = 150.0;
		_firstGrid.AddStringColumn("Customername", ArmSoft.SME.Common.Resources.CustomerName, 100).ReadOnly = true;
	}

	private void AddButtons()
	{
		LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Center;
		layoutGroupExt.VerticalAlignment = VerticalAlignment.Center;
		layoutGroupExt.Margin = new Thickness(0.0, 12.0, 0.0, 0.0);
		_openKeyInfoButton = AddButton(layoutGroupExt, "OpenKeyInfoButton", "", ArmSoft.SME.Common.Resources.ShowOnlineLicenses);
		_openKeyInfoButton.VerticalAlignment = VerticalAlignment.Center;
		_openKeyInfoButton.Click += new SafeEventHandler<RoutedEventArgs>(SerialHyperlinkRequestNavigate).Handler;
	}

	private void SerialHyperlinkRequestNavigate(object sender, RoutedEventArgs e)
	{
		if (_firstGrid.Grid.GetFocusedRow() is KeyInfoExt keyInfo)
		{
			new OnlineLicensesWindow(keyInfo, _accessToken).ShowDialog();
		}
	}
}
