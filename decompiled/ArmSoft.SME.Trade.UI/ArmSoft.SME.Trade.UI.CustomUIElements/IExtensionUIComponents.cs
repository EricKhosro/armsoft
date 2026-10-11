using System;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public interface IExtensionUIComponents
{
	void MakeDialogWindow(string name = "", string caption = "");

	bool ShowDialogWindow();

	void CloseDialogWindowWithOK();

	void SubscribeForDialogWindowOnScannerDataReceivedEvent(EventHandler onScannerDataReceivedEventHandler);

	void AddDialogWindowNumericControl(string name, string caption, short length, short precision = 0, bool isRequired = false, bool allowNegativeNumbers = false, bool useCurrencyFormat = true, bool storeValue = false);

	void AddDialogWindowDateControl(string name, string caption, bool useLongDate = false, bool isRequired = false, bool storeValue = false);

	void AddDialogWindowBooleanControl(string name, string caption, bool storeValue = false);

	void AddDialogWindowStringControl(string name, string caption, short length, bool isRequired = false, bool storeValue = false);

	string GetDialogWindowScannerData();

	decimal GetDialogWindowNumericControlValue(string name);

	DateOnly GetDialogWindowDateControlValue(string name);

	bool GetDialogWindowBooleanControlValue(string name);

	string GetDialogWindowStringControlValue(string name);

	void ShowMessageBox(string messageBoxText, string caption = "");

	void SendNotification(string caption, string text, string description = "");

	void ErrorHandler(Exception ex);

	string ShowSaveFileDialog(string defaultFileExtension, string filesFilter, string fileName, bool overwritePrompt = true);
}
