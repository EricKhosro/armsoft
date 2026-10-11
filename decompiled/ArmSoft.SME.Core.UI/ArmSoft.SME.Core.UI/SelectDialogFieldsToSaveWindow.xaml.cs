using System.Windows;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI;

public partial class SelectDialogFieldsToSaveWindow : ThemedWindow, IComponentConnector
{
	private readonly DialogWindow _dialog;

	public SelectDialogFieldsToSaveWindow(DialogWindow dialog)
	{
		InitializeComponent();
		Title = string.Format(ArmSoft.SME.Common.Resources.FieldsToSaveForDialog, dialog.Title);
		Owner = dialog;
		_dialog = dialog;
		WindowStartupLocation = WindowStartupLocation.CenterOwner;
		ButtonOK.Content = ArmSoft.SME.Common.Resources.Ok;
		ButtonOK.Click += new SafeEventHandler<RoutedEventArgs>(ButtonOK_Click).Handler;
		ButtonReset.Click += new SafeEventHandler<RoutedEventArgs>(ButtonReset_Click).Handler;
		ButtonCancel.Content = ArmSoft.SME.Common.Resources.Cancel;
		ButtonReset.Content = ArmSoft.SME.Common.Resources.Reset;
		foreach (DialogWindow.StoreValueDescriptor storedValue in dialog.StoredValues)
		{
			if (!storedValue.ShareStoreFlagWithPreviousControl)
			{
				FieldsListBox.Items.Add(new ListBoxEditItem
				{
					Name = storedValue.Name,
					Content = storedValue.Caption,
					IsSelected = storedValue.ActualStoreFlag
				});
			}
		}
		ASFontHelper.SetFontSettings(this);
	}

	private void ButtonReset_Click(object sender, RoutedEventArgs e)
	{
		for (int i = 0; i <= FieldsListBox.Items.Count - 1; i++)
		{
			ListBoxEditItem listBoxEditItem = (ListBoxEditItem)FieldsListBox.Items[i];
			foreach (DialogWindow.StoreValueDescriptor storedValue in _dialog.StoredValues)
			{
				if (storedValue.Name == listBoxEditItem.Name)
				{
					listBoxEditItem.IsSelected = storedValue.DefaultStoreFlag;
					break;
				}
			}
		}
	}

	private void ButtonOK_Click(object sender, RoutedEventArgs e)
	{
		for (int i = 0; i <= FieldsListBox.Items.Count - 1; i++)
		{
			ListBoxEditItem listBoxEditItem = (ListBoxEditItem)FieldsListBox.Items[i];
			for (int j = 0; j < _dialog.StoredValues.Count; j++)
			{
				if (_dialog.StoredValues[j].Name == listBoxEditItem.Name)
				{
					_dialog.StoredValues[j].ActualStoreFlag = listBoxEditItem.IsSelected;
					for (j++; j < _dialog.StoredValues.Count && _dialog.StoredValues[j].ShareStoreFlagWithPreviousControl; j++)
					{
						_dialog.StoredValues[j].ActualStoreFlag = listBoxEditItem.IsSelected;
					}
					break;
				}
			}
		}
		DialogResult = true;
	}
}
