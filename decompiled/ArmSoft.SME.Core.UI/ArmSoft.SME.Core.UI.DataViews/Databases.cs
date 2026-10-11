using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.AppManager;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.DataViewsData;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.StandartWindow.Backups;

namespace ArmSoft.SME.Core.UI.DataViews;

[DataView(8)]
public class Databases : DataView<DatabasesDataProvider.DataRow, DatabasesDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly Databases _dataView;

		private readonly CheckEditExt _showDatabasesSizes;

		private readonly CheckEditExt _showAutoBackupOptions;

		public DataViewDialog(Databases dataView, bool useParamValues)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			_showDatabasesSizes = AddBooleanControl("ShowDatabasesSizes", ArmSoft.SME.Common.Resources.ShowDatabasesSizes, !useParamValues);
			_showDatabasesSizes.IsChecked = useParamValues && _dataView.ParamShowDatabasesSizes;
			_showAutoBackupOptions = AddBooleanControl("ShowAutoBackupOptions", ArmSoft.SME.Common.Resources.ShowAutoBackupOptions, !useParamValues);
			_showAutoBackupOptions.IsChecked = useParamValues && _dataView.ParamShowAutoBackupOptions;
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.ParamShowDatabasesSizes = _showDatabasesSizes.IsChecked.HasValue && _showDatabasesSizes.IsChecked.Value;
			_dataView.ParamShowAutoBackupOptions = _showAutoBackupOptions.IsChecked.HasValue && _showAutoBackupOptions.IsChecked.Value;
		}
	}

	public override bool AllowAdd
	{
		get
		{
			if (Settings.LoginService.IsDBAdmin)
			{
				return !Settings.LoginService.DemoMode;
			}
			return false;
		}
	}

	public override bool AllowEdit => true;

	public override bool AllowView => true;

	public override bool DialogSupported => true;

	public override string HelpId => "AdminTools_Databases";

	public bool ParamShowDatabasesSizes
	{
		get
		{
			return (bool)Parameters["ShowDatabasesSizes"].Value;
		}
		set
		{
			Parameters["ShowDatabasesSizes"].Value = value;
		}
	}

	public bool ParamShowAutoBackupOptions
	{
		get
		{
			return (bool)Parameters["ShowAutoBackupOptions"].Value;
		}
		set
		{
			Parameters["ShowAutoBackupOptions"].Value = value;
		}
	}

	public Databases()
	{
		DataProvider = new DatabasesDataProvider();
		AddParameter("ShowDatabasesSizes", Resources.ShowDatabasesSizes, false);
		AddParameter("ShowAutoBackupOptions", Resources.ShowAutoBackupOptions, false);
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnSortOrder("Name");
		dvp.Set_ColumnVisible("Size", ParamShowDatabasesSizes);
		dvp.Set_ColumnVisible("FreeSpace", ParamShowDatabasesSizes);
		dvp.Set_ColumnVisible("BackupPolicy", ParamShowAutoBackupOptions);
	}

	public override void InitContextFunctions()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		DatabasesDataProvider.DataRow dataRow = (DatabasesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			bool isEnabled = dataRow.DbId == Settings.LoginService.DatabaseId;
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "RestoreDatabase", Resources.Restore, RestoreDatabase, "", "", storeOpContext: false, isEnabled);
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ReIndex", Resources.ReIndex, ReIndex, "", "", storeOpContext: false, isEnabled);
			if (!ConfigHelper.ASAdminLogin)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "AutoBackupOptions", Resources.AutoBackupOptions, AutoBackupOptions);
			}
		}
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues);
	}

	protected override void ApplyParametersToDataProvider()
	{
		base.ApplyParametersToDataProvider();
		ParametersProvider.ShowDatabasesSizes = ParamShowDatabasesSizes;
		ParametersProvider.ShowAutoBackupOptions = ParamShowAutoBackupOptions;
	}

	public override void Add()
	{
		Database database = new Database
		{
			UsersRoles = DatabaseHelper.GetNewDatabaseUserRoles()
		};
		new DatabaseWindow(DialogWindowMode.NewMode, database).ShowDialog();
	}

	public override void Edit()
	{
		ShowDatabaseWindow(DialogWindowMode.EditMode);
	}

	public override void View()
	{
		ShowDatabaseWindow(DialogWindowMode.ViewMode);
	}

	private static void ShowDatabaseWindow(DialogWindowMode mode)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			DatabasesDataProvider.DataRow dataRow = (DatabasesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				Database database = new Database
				{
					DbId = dataRow.DbId,
					Name = dataRow.Name,
					Caption = dataRow.Caption,
					UsersRoles = DatabaseHelper.GetDatabaseUserRoles(dataRow.DbId)
				};
				new DatabaseWindow(mode, database).ShowDialog();
			}
		}
	}

	private void RestoreDatabase()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			DatabasesDataProvider.DataRow dataRow = (DatabasesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null && MessageBox.Show(Resources.ConfirmRestore, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				new BackupRestoreDialogWindow(dataRow.ServiceAddress, dataRow.Name).ShowDialog();
			}
		}
	}

	private void ReIndex()
	{
		DBIndexerRequest dBIndexerRequest = new DBIndexerRequest();
		if (MessageBox.Show(Resources.DeleteIncorrectSymbols, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			dBIndexerRequest.DeleteSpecSymbols = true;
		}
		try
		{
			DataProcessingRequestHelper.Execute<DBIndexerRequest, DBIndexerResponse>(DPRType.Other, "DbIndexer", dBIndexerRequest);
			MessageBox.Show(Resources.ReindexingSuccesfully, Settings.ProgramName, MessageBoxButton.OK);
		}
		catch
		{
			MessageBox.Show(Resources.ReindexingFailed, Settings.ProgramName, MessageBoxButton.OK);
		}
	}

	private void AutoBackupOptions()
	{
		List<DatabasesDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<DatabasesDataProvider.DataRow>();
		if (selectedOrFocusedRows == null || selectedOrFocusedRows.Count == 0 || !Convert.ToBoolean(new AutoBackupOptionsWindow(selectedOrFocusedRows).ShowDialog()))
		{
			return;
		}
		if (selectedOrFocusedRows.Count == 1)
		{
			DataViewPanel dVP = DVP;
			List<object> list = new List<object>();
			foreach (int item in selectedOrFocusedRows.Select((DatabasesDataProvider.DataRow x) => x.DbId))
			{
				list.Add(item);
			}
			dVP.Update(list);
		}
		else
		{
			DVP.Refresh();
		}
	}
}
