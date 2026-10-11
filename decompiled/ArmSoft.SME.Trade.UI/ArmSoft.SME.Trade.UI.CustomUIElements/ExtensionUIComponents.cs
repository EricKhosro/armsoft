using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.BackgroundWorks;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.StandartWindow;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class ExtensionUIComponents : DialogWindow, IExtensionUIComponents
{
	public class DialogWindowWithScannerEvent : DialogWindow
	{
		internal string ScannerData = string.Empty;

		public ExtensionUIComponents extensionUIComponents;

		public event EventHandler OnScannerDataReceived;

		public DialogWindowWithScannerEvent(Window owner = null)
			: base(owner)
		{
		}

		public override void OnScanner(string scannerData)
		{
			ScannerData = scannerData;
			OnScannerDataReceived?.Invoke(extensionUIComponents, null);
		}
	}

	private DialogWindowWithScannerEvent dialogWindow;

	private Dictionary<string, object> dialogWindowControls;

	private Window ownerWindow;

	public ExtensionUIComponents(Window ownerWindow = null)
	{
		this.ownerWindow = ownerWindow;
	}

	public void SubscribeForDialogWindowOnScannerDataReceivedEvent(EventHandler onScannerDataReceivedEventHandler)
	{
		dialogWindow.extensionUIComponents = this;
		dialogWindow.OnScannerDataReceived += onScannerDataReceivedEventHandler;
	}

	private void CheckDialogWindowExistence()
	{
		if (dialogWindow == null)
		{
			throw new ASException("MakeDialogWindow must be called first");
		}
	}

	public void MakeDialogWindow(string name = "", string caption = "")
	{
		dialogWindow = new DialogWindowWithScannerEvent(ownerWindow)
		{
			Name = name,
			Caption = caption
		};
		dialogWindowControls = new Dictionary<string, object>();
	}

	public bool ShowDialogWindow()
	{
		CheckDialogWindowExistence();
		return dialogWindow.ShowDialog().Value;
	}

	public void CloseDialogWindowWithOK()
	{
		CheckDialogWindowExistence();
		dialogWindow.DialogResult = true;
	}

	public void AddDialogWindowNumericControl(string name, string caption, short length, short precision = 0, bool isRequired = false, bool allowNegativeNumbers = false, bool useCurrencyFormat = true, bool storeValue = false)
	{
		CheckDialogWindowExistence();
		dialogWindowControls.Add(name, dialogWindow.AddNumericControl(name, caption, length, precision, isRequired, allowNegativeNumbers, useCurrencyFormat, storeValue));
	}

	public void AddDialogWindowDateControl(string name, string caption, bool useLongDate = false, bool isRequired = false, bool storeValue = false)
	{
		CheckDialogWindowExistence();
		dialogWindowControls.Add(name, dialogWindow.AddDateControl(name, caption, useLongDate, isRequired, storeValue));
	}

	public void AddDialogWindowBooleanControl(string name, string caption, bool storeValue = false)
	{
		CheckDialogWindowExistence();
		dialogWindowControls.Add(name, dialogWindow.AddBooleanControl(name, caption, storeValue));
	}

	public void AddDialogWindowStringControl(string name, string caption, short length, bool isRequired = false, bool storeValue = false)
	{
		CheckDialogWindowExistence();
		dialogWindowControls.Add(name, dialogWindow.AddStringControl(name, caption, length, isRequired, storeValue));
	}

	public string GetDialogWindowScannerData()
	{
		CheckDialogWindowExistence();
		return dialogWindow.ScannerData;
	}

	public decimal GetDialogWindowNumericControlValue(string name)
	{
		CheckDialogWindowExistence();
		object value = null;
		PopupCalcEditExt popupCalcEditExt = null;
		if (dialogWindowControls.TryGetValue(name, out value))
		{
			popupCalcEditExt = value as PopupCalcEditExt;
		}
		if (popupCalcEditExt == null)
		{
			throw new ASException("Numeric control with such name is not present");
		}
		return popupCalcEditExt.Value;
	}

	public DateOnly GetDialogWindowDateControlValue(string name)
	{
		CheckDialogWindowExistence();
		object value = null;
		DateEditControl dateEditControl = null;
		if (dialogWindowControls.TryGetValue(name, out value))
		{
			dateEditControl = value as DateEditControl;
		}
		if (dateEditControl == null)
		{
			throw new ASException("Date control with such name is not present");
		}
		return dateEditControl.EditValue.GetValueOrDefault();
	}

	public bool GetDialogWindowBooleanControlValue(string name)
	{
		CheckDialogWindowExistence();
		object value = null;
		CheckEditExt checkEditExt = null;
		if (dialogWindowControls.TryGetValue(name, out value))
		{
			checkEditExt = value as CheckEditExt;
		}
		if (checkEditExt == null)
		{
			throw new ASException("Boolean control with such name is not present");
		}
		return checkEditExt.IsChecked.Value;
	}

	public string GetDialogWindowStringControlValue(string name)
	{
		CheckDialogWindowExistence();
		object value = null;
		TextEditExt textEditExt = null;
		if (dialogWindowControls.TryGetValue(name, out value))
		{
			textEditExt = value as TextEditExt;
		}
		if (textEditExt == null)
		{
			throw new ASException("String control with such name is not present");
		}
		return textEditExt.Text;
	}

	public void ShowMessageBox(string messageBoxText, string caption = "")
	{
		ArmSoft.SME.Core.UI.MessageBox.Show(messageBoxText, caption);
	}

	public void SendNotification(string caption, string text, string description = "")
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
		{
			Caption = caption,
			HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification),
			Image = ImageService.GetImage("accept_32.svg"),
			Text = text
		};
		toastNotificationViewModel.HistoryItem.Description = (string.IsNullOrWhiteSpace(description) ? text : description);
		toastNotificationViewModel.HistoryItem.Details = (string.IsNullOrWhiteSpace(description) ? text : description);
		toastNotificationViewModel.HistoryItem.Icon = "accept_16.svg";
		Settings.HostingEnvironment.ShowNotificationAsync(toastNotificationViewModel);
	}

	public void ErrorHandler(Exception ex)
	{
		Settings.ErrorHandler(ex);
	}

	public string ShowSaveFileDialog(string defaultFileExtension, string filesFilter, string fileName, bool overwritePrompt = true)
	{
		return Settings.CommonCoreHelper.GetFileNameFromExportDialog(defaultFileExtension, filesFilter, fileName, overwritePrompt);
	}
}
