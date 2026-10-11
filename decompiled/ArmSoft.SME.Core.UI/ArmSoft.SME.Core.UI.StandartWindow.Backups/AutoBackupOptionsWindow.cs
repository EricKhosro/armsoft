using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViewsData;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.StandartWindow.Backups;

internal class AutoBackupOptionsWindow : DialogWindow
{
	private CheckEditExt _dailyBackupEnabled;

	private SpinEditExt _dailyBackupCount;

	private CheckEditExt _monthlyBackupEnabled;

	private SpinEditExt _monthlyBackupDay;

	private SpinEditExt _monthlyBackupCount;

	private CheckEditExt _annualBackupEnabled;

	private SpinEditExt _annualBackupDay;

	private SpinEditExt _annualBackupMonth;

	private SpinEditExt _annualBackupCount;

	private CheckEditExt _createOnExistedLogin;

	private Button _restoreAutoBackupDefaultValues;

	private readonly List<DatabasesDataProvider.DataRow> _selectedDatabases;

	public AutoBackupOptionsWindow(List<DatabasesDataProvider.DataRow> selectedDatabases)
	{
		_selectedDatabases = selectedDatabases;
		if (selectedDatabases.Count == 1)
		{
			Caption = ArmSoft.SME.Common.Resources.AutoBackupOptions + " - " + selectedDatabases[0].Name;
		}
		else
		{
			Caption = ArmSoft.SME.Common.Resources.AutoBackupOptions;
		}
		ConstructUI();
		LoadDataToControls();
		DailyBackupEnabled_EditValueChanged();
		MonthlyBackupEnabled_EditValueChanged();
		AnnualBackupEnabled_EditValueChanged();
		SetEvents();
	}

	private void ConstructUI()
	{
		Width = 560.0;
		SizeToContent = SizeToContent.Height;
		if (_selectedDatabases.Count > 1)
		{
			AddTextBlock(string.Format(ArmSoft.SME.Common.Resources.DifferentBackupPolicies, _selectedDatabases.Count), TextBlockImage.Warning).Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#f07174" : "#e40000");
		}
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		AddTextBlock(container, ArmSoft.SME.Common.Resources.Periodicity, TextBlockImage.None);
		((Control)(object)AddTextBlock(container, ArmSoft.SME.Common.Resources.CountOfLastCopies, TextBlockImage.None).Parent).HorizontalAlignment = HorizontalAlignment.Right;
		LayoutGroupExt container2 = AddHorizontalLayoutGroup();
		_dailyBackupEnabled = AddBooleanControl(container2, "_dailyBackupEnabled", ArmSoft.SME.Common.Resources.Daily);
		_dailyBackupCount = AddSpinEdit(container2, "_dailyBackupCount", "", 1m, 45m);
		((Control)(object)_dailyBackupCount.Parent).HorizontalAlignment = HorizontalAlignment.Right;
		LayoutGroupExt container3 = AddHorizontalLayoutGroup();
		_monthlyBackupEnabled = AddBooleanControl(container3, "_monthlyBackupEnabled", ArmSoft.SME.Common.Resources.PeriodicityMonthly);
		_monthlyBackupDay = AddSpinEdit(container3, "_monthlyBackupDay", ArmSoft.SME.Common.Resources.Day, 1m, 31m);
		_monthlyBackupCount = AddSpinEdit(container3, "_monthlyBackupCount", "", 1m, 15m);
		((Control)(object)_monthlyBackupCount.Parent).HorizontalAlignment = HorizontalAlignment.Right;
		LayoutGroupExt container4 = AddHorizontalLayoutGroup();
		_annualBackupEnabled = AddBooleanControl(container4, "_annualBackupEnabled", ArmSoft.SME.Common.Resources.Annual);
		_annualBackupDay = AddSpinEdit(container4, "_annualBackupDay", ArmSoft.SME.Common.Resources.Day, 1m, 31m);
		_annualBackupMonth = AddSpinEdit(container4, "_annualBackupMonth", ArmSoft.SME.Common.Resources.Month.ToLower(), 1m, 12m);
		_annualBackupCount = AddSpinEdit(container4, "_annualBackupCount", "", 1m, 5m);
		((Control)(object)_annualBackupCount.Parent).HorizontalAlignment = HorizontalAlignment.Right;
		AddLine();
		LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Left;
		_createOnExistedLogin = AddBooleanControl(layoutGroupExt, "_createOnExistedLogin", ArmSoft.SME.Common.Resources.BackupOnExistedLogin);
		((Control)(object)_createOnExistedLogin.Parent).HorizontalAlignment = HorizontalAlignment.Center;
		_restoreAutoBackupDefaultValues = AddButton(null, "_restoreAutoBackupDefaultValues", "", ArmSoft.SME.Common.Resources.RestoreDefaultValues);
		_restoreAutoBackupDefaultValues.Margin = new Thickness(10.0, 0.0, 0.0, 0.0);
		_restoreAutoBackupDefaultValues.Click += new SafeEventHandler<RoutedEventArgs>(RestoreDefaultValues_Click).Handler;
	}

	private void LoadDataToControls()
	{
		if (_selectedDatabases.Count == 1)
		{
			DatabasesDataProvider.DataRow dataRow = _selectedDatabases[0];
			using CloudClient cloudClient = Settings.CloudClient;
			cloudClient.Token = Settings.GetApplicationToken();
			CloudDatabaseInfo cloudDatabase = cloudClient.AS7X.GetCloudDatabase(dataRow.DbId);
			_dailyBackupEnabled.IsChecked = cloudDatabase.DailyBackupEnabled;
			_dailyBackupCount.Value = cloudDatabase.DailyBackupCount;
			_monthlyBackupEnabled.IsChecked = cloudDatabase.MonthlyBackupEnabled;
			_monthlyBackupDay.Value = cloudDatabase.MonthlyBackupDay;
			_monthlyBackupCount.Value = cloudDatabase.MonthlyBackupCount;
			_annualBackupEnabled.IsChecked = cloudDatabase.AnnualBackupEnabled;
			_annualBackupDay.Value = cloudDatabase.AnnualBackupDay;
			_annualBackupMonth.Value = cloudDatabase.AnnualBackupMonth;
			_annualBackupCount.Value = cloudDatabase.AnnualBackupCount;
			_createOnExistedLogin.IsChecked = cloudDatabase.CreateOnExistedLogin;
			return;
		}
		RestoreDefaultValues();
	}

	private void SetEvents()
	{
		_dailyBackupEnabled.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DailyBackupEnabled_EditValueChanged).Handler;
		_monthlyBackupEnabled.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MonthlyBackupEnabled_EditValueChanged).Handler;
		_annualBackupEnabled.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(AnnualBackupEnabled_EditValueChanged).Handler;
	}

	private void DailyBackupEnabled_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		DailyBackupEnabled_EditValueChanged();
	}

	private void DailyBackupEnabled_EditValueChanged()
	{
		_dailyBackupCount.IsEnabled = _dailyBackupEnabled.IsChecked.Value;
	}

	private void MonthlyBackupEnabled_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		MonthlyBackupEnabled_EditValueChanged();
	}

	private void MonthlyBackupEnabled_EditValueChanged()
	{
		_monthlyBackupDay.IsEnabled = _monthlyBackupEnabled.IsChecked.Value;
		_monthlyBackupCount.IsEnabled = _monthlyBackupEnabled.IsChecked.Value;
	}

	private void AnnualBackupEnabled_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		AnnualBackupEnabled_EditValueChanged();
	}

	private void AnnualBackupEnabled_EditValueChanged()
	{
		_annualBackupDay.IsEnabled = _annualBackupEnabled.IsChecked.Value;
		_annualBackupMonth.IsEnabled = _annualBackupEnabled.IsChecked.Value;
		_annualBackupCount.IsEnabled = _annualBackupEnabled.IsChecked.Value;
	}

	private void RestoreDefaultValues_Click(object sender, RoutedEventArgs e)
	{
		RestoreDefaultValues();
	}

	private void RestoreDefaultValues()
	{
		_dailyBackupEnabled.IsChecked = true;
		_dailyBackupCount.EditValue = 35;
		_monthlyBackupEnabled.IsChecked = true;
		_monthlyBackupDay.EditValue = 1;
		_monthlyBackupCount.EditValue = 11;
		_annualBackupEnabled.IsChecked = true;
		_annualBackupDay.EditValue = 15;
		_annualBackupMonth.EditValue = 1;
		_annualBackupCount.EditValue = 5;
		_createOnExistedLogin.IsChecked = true;
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		try
		{
			base.DialogValidation(args);
			CloudDatabaseBackupUpdateRequest request = new CloudDatabaseBackupUpdateRequest
			{
				DailyBackupEnabled = _dailyBackupEnabled.IsChecked.Value,
				DailyBackupCount = (short)_dailyBackupCount.Value,
				MonthlyBackupEnabled = _monthlyBackupEnabled.IsChecked.Value,
				MonthlyBackupDay = (short)_monthlyBackupDay.Value,
				MonthlyBackupCount = (short)_monthlyBackupCount.Value,
				AnnualBackupEnabled = _annualBackupEnabled.IsChecked.Value,
				AnnualBackupDay = (short)_annualBackupDay.Value,
				AnnualBackupMonth = (short)_annualBackupMonth.Value,
				AnnualBackupCount = (short)_annualBackupCount.Value,
				CreateOnExistedLogin = _createOnExistedLogin.IsChecked.Value
			};
			using CloudClient cloudClient = Settings.CloudClient;
			cloudClient.Token = Settings.GetApplicationToken();
			foreach (DatabasesDataProvider.DataRow selectedDatabase in _selectedDatabases)
			{
				cloudClient.AS7X.UpdateCloudDatabaseBackupSettings(selectedDatabase.DbId, request);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
