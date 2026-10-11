using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class PaymentHistoryDialogWindow : DialogWindow
{
	public PaymentHistoryDialogWindow(List<DocumentHistoryRow> list)
	{
		Title = ArmSoft.SME.Common.Resources.History;
		Height = 450.0;
		Width = 700.0;
		WindowStyle = WindowStyle.SingleBorderWindow;
		ShowIcon = false;
		_txtStatus.Visibility = Visibility.Collapsed;
		SizeToContent = SizeToContent.Manual;
		WindowState = WindowState.Normal;
		DocumentGridControl documentGridControl = AddGridControl("HistoryGrod");
		documentGridControl.AllowAdd = false;
		documentGridControl.AllowDelete = false;
		documentGridControl.AllowDrop = false;
		documentGridControl.AllowEdit = false;
		documentGridControl.ShowButtons = false;
		documentGridControl.View.AutoWidth = true;
		documentGridControl.AddStringColumn("OperationDate", ArmSoft.SME.Common.Resources.DateWord, 7).ReadOnly = true;
		documentGridControl.AddStringColumn("DisplayOperationType", ArmSoft.SME.Common.Resources.Operation, 15).ReadOnly = true;
		documentGridControl.AddStringColumn("UserName", ArmSoft.SME.Common.Resources.LoginName, 7).ReadOnly = true;
		documentGridControl.AddStringColumn("DisplaySystemType", ArmSoft.SME.Common.Resources.ApplicationType, 10).ReadOnly = true;
		documentGridControl.AddStringColumn("Location", ArmSoft.SME.Common.Resources.Country, 10).ReadOnly = true;
		documentGridControl.AddStringColumn("DeviceModel", ArmSoft.SME.Common.Resources.DeviceModel, 10).ReadOnly = true;
		documentGridControl.AddStringColumn("DevicePlatform", ArmSoft.SME.Common.Resources.OperatingSystem, 10).ReadOnly = true;
		documentGridControl.ItemsSource = list;
	}
}
