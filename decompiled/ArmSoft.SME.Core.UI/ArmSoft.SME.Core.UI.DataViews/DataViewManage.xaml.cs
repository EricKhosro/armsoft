using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.DataViews;

public partial class DataViewManage : ThemedWindow, IComponentConnector
{
	private class LVSetting : ListViewItem
	{
		public DataViewSetting dvs;
	}

	private const string _layoutCaptionConst = "LayoutCaption";

	public IDataView IDV { get; set; }

	public List<DataViewSetting> DVSList { get; set; }

	public DataViewManage()
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		Title = ArmSoft.SME.Common.Resources.ManageDataViewSettings;
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		btCopy.Content = ArmSoft.SME.Common.Resources.Copy;
		btEdit.Content = ArmSoft.SME.Common.Resources.Edit;
		btRemove.Content = ArmSoft.SME.Common.Resources.Delete;
		btMoveDown.Content = ArmSoft.SME.Common.Resources.Down;
		btMoveUp.Content = ArmSoft.SME.Common.Resources.Up;
		btSave.Content = ArmSoft.SME.Common.Resources.Save;
		Icon = ImageService.GetImage("options_16.svg");
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(DataViewManage_PreviewKeyDown).Handler;
		ViewNames.SelectionChanged += ViewNames_SelectionChanged;
		btCopy.Click += new SafeEventHandler<RoutedEventArgs>(ButtonCopy_Click).Handler;
		btEdit.Click += new SafeEventHandler<RoutedEventArgs>(ButtonEdit_Click).Handler;
		btRemove.Click += new SafeEventHandler<RoutedEventArgs>(ButtonRemove_Click).Handler;
		btMoveUp.Click += new SafeEventHandler<RoutedEventArgs>(ButtonMoveUp_Click).Handler;
		btMoveDown.Click += new SafeEventHandler<RoutedEventArgs>(ButtonMoveDown_Click).Handler;
		btSave.Click += new SafeEventHandler<RoutedEventArgs>(ButtonSave_Click).Handler;
	}

	public void FillViewNames()
	{
		ViewNames.Items.Clear();
		foreach (DataViewSetting dVS in DVSList)
		{
			AddToViewNames(dVS);
		}
		if (ViewNames.SelectedItem == null && ViewNames.Items.Count > 0)
		{
			ViewNames.SelectedItem = ViewNames.Items[0];
		}
	}

	private LVSetting AddToViewNames(DataViewSetting dvs)
	{
		LVSetting lVSetting = new LVSetting
		{
			dvs = dvs.Clone(),
			Content = dvs.Caption
		};
		ViewNames.Items.Add(lVSetting);
		return lVSetting;
	}

	private void RefreshButtonsState()
	{
		int count = ViewNames.Items.Count;
		if (count == 0)
		{
			btRemove.IsEnabled = false;
		}
		else
		{
			btRemove.IsEnabled = ViewNames.SelectedItem is LVSetting lVSetting && lVSetting != null;
		}
		btCopy.IsEnabled = btRemove.IsEnabled;
		btEdit.IsEnabled = btRemove.IsEnabled;
		int selectedIndex = ViewNames.SelectedIndex;
		btMoveUp.IsEnabled = selectedIndex > 0;
		btMoveDown.IsEnabled = selectedIndex < count - 1;
	}

	private void ViewNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		RefreshButtonsState();
	}

	private void DataViewManage_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Key == 13 && (int)Keyboard.Modifiers == 0)
		{
			Close();
			e.Handled = true;
		}
	}

	private void ButtonCopy_Click(object sender, RoutedEventArgs e)
	{
		if (ViewNames.SelectedIndex >= 0)
		{
			LVSetting lVSetting = (LVSetting)ViewNames.SelectedItem;
			DialogWindow dialogWindow = new DialogWindow
			{
				Owner = this,
				Title = ArmSoft.SME.Common.Resources.DataViewDefinition + ArmSoft.SME.Common.Resources.CaptionNew
			};
			dialogWindow.AddStringControl("LayoutCaption", ArmSoft.SME.Common.Resources.Name, 50, isRequired: true).EditValue = (string)lVSetting.Content;
			dialogWindow.WindowValidation += LayoutWindowEditValidation;
			if (dialogWindow.ShowDialog().Value)
			{
				string caption = ((string)dialogWindow["LayoutCaption"]).Trim();
				DataViewSetting dataViewSetting = lVSetting.dvs.Clone();
				dataViewSetting.Id = (short)(ViewNames.Items.Count + 1);
				dataViewSetting.Caption = caption;
				AddToViewNames(dataViewSetting);
				ListBox viewNames = ViewNames;
				ItemCollection items = ViewNames.Items;
				viewNames.SelectedItem = items[items.Count - 1];
				RefreshButtonsState();
			}
		}
	}

	private void ButtonEdit_Click(object sender, RoutedEventArgs e)
	{
		if (ViewNames.SelectedIndex >= 0)
		{
			LVSetting lVSetting = (LVSetting)ViewNames.SelectedItem;
			if (new DataViewSettingsWindow(IDV, this, lVSetting.dvs, isDefault: false)
			{
				CheckCaption = CaptionDelegate
			}.ShowDialog().Value)
			{
				lVSetting.Content = lVSetting.dvs.Caption;
			}
		}
	}

	private void ButtonRemove_Click(object sender, RoutedEventArgs e)
	{
		int selectedIndex = ViewNames.SelectedIndex;
		if (selectedIndex >= 0)
		{
			LVSetting removeItem = (LVSetting)ViewNames.SelectedItem;
			ViewNames.Items.Remove(removeItem);
			if (ViewNames.Items.Count > selectedIndex)
			{
				ViewNames.SelectedIndex = selectedIndex;
			}
			else
			{
				ViewNames.SelectedIndex = ViewNames.Items.Count - 1;
			}
		}
	}

	private void ButtonMoveUp_Click(object sender, RoutedEventArgs e)
	{
		int selectedIndex = ViewNames.SelectedIndex;
		if (selectedIndex > 0)
		{
			LVSetting lVSetting = (LVSetting)ViewNames.SelectedItem;
			ViewNames.Items.Remove(lVSetting);
			ViewNames.Items.Insert(selectedIndex - 1, lVSetting);
			ViewNames.SelectedItem = lVSetting;
			ViewNames.ScrollIntoView(lVSetting);
		}
	}

	private void ButtonMoveDown_Click(object sender, RoutedEventArgs e)
	{
		int selectedIndex = ViewNames.SelectedIndex;
		if (selectedIndex < ViewNames.Items.Count - 1)
		{
			LVSetting lVSetting = (LVSetting)ViewNames.SelectedItem;
			ViewNames.Items.Remove(lVSetting);
			ViewNames.Items.Insert(selectedIndex + 1, lVSetting);
			ViewNames.SelectedItem = lVSetting;
			ViewNames.ScrollIntoView(lVSetting);
		}
	}

	private void ButtonSave_Click(object sender, RoutedEventArgs e)
	{
		Settings.CreateApiClient().DataViewSettingsRoutes.RemoveAll(IDV.Name, "");
		DVSList.Clear();
		short num = 0;
		foreach (LVSetting item in (IEnumerable)ViewNames.Items)
		{
			DataViewSetting dvs = item.dvs;
			num = (dvs.Id = (short)(num + 1));
			dvs.Caption = (string)item.Content;
			Settings.CreateApiClient().DataViewSettingsRoutes.Store(dvs);
			DVSList.Add(dvs);
		}
		DialogResult = true;
		Close();
	}

	private void LayoutWindowEditValidation(DialogWindow dialog, DialogWindowValidationArgs args)
	{
		string text = ((string)dialog["LayoutCaption"]).Trim();
		foreach (LVSetting item in (IEnumerable)ViewNames.Items)
		{
			if (((string)item.Content).Trim() == text)
			{
				MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.DataViewNameDuplicate, text));
				args.isValid = false;
				break;
			}
		}
	}

	private void CaptionDelegate(string caption, ref bool exists)
	{
		exists = false;
		foreach (LVSetting item in (IEnumerable)ViewNames.Items)
		{
			if (((string)item.Content).Trim() == caption)
			{
				exists = true;
				break;
			}
		}
	}
}
