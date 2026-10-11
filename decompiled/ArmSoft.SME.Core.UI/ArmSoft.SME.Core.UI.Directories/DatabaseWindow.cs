using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.Cloud.Client.Edge;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI.Directories;

public class DatabaseWindow : DialogWindow
{
	private TextEditExt _name;

	private TextEditExt _comment;

	private DocumentGridControl _usersRolesGrid;

	private List<Database.UserRoles> _userRoles;

	private readonly Database _database;

	public DatabaseWindow(DialogWindowMode mode, Database database)
	{
		Mode = mode;
		_database = database;
		ConstructUI();
		LoadDataToControls();
		SetControlsReadOnly();
		Caption = ArmSoft.SME.Common.Resources.Database;
		HelpId = "AdminTools_Databases";
	}

	private void ConstructUI()
	{
		_name = AddStringControl("Name", ArmSoft.SME.Common.Resources.Name, 20, isRequired: true);
		_comment = AddStringControl("Caption", ArmSoft.SME.Common.Resources.Comment, 50, isRequired: true);
		LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
		layoutGroupExt.View = LayoutGroupView.GroupBox;
		layoutGroupExt.Header = ArmSoft.SME.Common.Resources.UserAccessEditDbLabel;
		_usersRolesGrid = AddGridControl(layoutGroupExt, "UsersRoles");
		if (Settings.SystemType == SystemTypes.SalesManagement)
		{
			layoutGroupExt.Visibility = Visibility.Collapsed;
			_usersRolesGrid.Visibility = Visibility.Collapsed;
		}
		_usersRolesGrid.Width = SizeCalculator.Width(880.0);
		_usersRolesGrid.Height = SizeCalculator.Height(250.0);
		_usersRolesGrid.AllowAdd = false;
		_usersRolesGrid.AllowDelete = false;
		_usersRolesGrid.AllowDelete = false;
		_usersRolesGrid.AllowDuplicate = false;
		_usersRolesGrid.ShowButtons = false;
		_usersRolesGrid.View.AllowMoveColumnToDropArea = false;
		_usersRolesGrid.View.IsColumnMenuEnabled = false;
		_usersRolesGrid.View.AutoWidth = true;
		_usersRolesGrid.Grid.CustomUnboundColumnData += new SafeEventHandler<GridColumnDataEventArgs>(Grid_CustomUnboundColumnData).Handler;
		DocumentGridColumn documentGridColumn = _usersRolesGrid.AddStringColumn("UserName", ArmSoft.SME.Common.Resources.UserName, 50);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.CellTemplate = (DataTemplate)Settings.GenericResources["DisabledStringCellTemplate"];
		DocumentGridColumn documentGridColumn2 = _usersRolesGrid.AddStringColumn("UserCaption", ArmSoft.SME.Common.Resources.UserDescription, 50);
		documentGridColumn2.AllowEditing = DefaultBoolean.False;
		documentGridColumn2.CellTemplate = (DataTemplate)Settings.GenericResources["DisabledStringCellTemplate"];
		foreach (KeyValuePair<short, string> item in new Dictionary<short, string> { 
		{
			0,
			ArmSoft.SME.Common.Resources.All
		} }.Union(Settings.HostingEnvironment.Subsystems))
		{
			DocumentGridColumn documentGridColumn3 = _usersRolesGrid.AddBooleanColumn($"Role_{item.Key}", item.Value);
			documentGridColumn3.UnboundDataType = typeof(bool);
			documentGridColumn3.CellTemplate = (DataTemplate)Settings.GenericResources["DisabledBooleanCellTemplate"];
		}
		if (Mode == DialogWindowMode.EditMode)
		{
			SetFocusOnActivation = _comment;
		}
	}

	private void Grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
	{
		Database.UserRoles userRoles = (Database.UserRoles)e.Source.GetRowByListIndex(e.ListSourceRowIndex);
		if (e.IsGetData)
		{
			if (e.Column.FieldName == "Role_0")
			{
				e.Value = userRoles.SelectAll;
			}
			else
			{
				e.Value = userRoles.Roles.Contains(GetRoleFromByColumnName(e.Column.FieldName));
			}
		}
		else if (e.IsSetData)
		{
			if (e.Column.FieldName == "Role_0")
			{
				userRoles.SelectAll = (bool?)e.Value;
			}
			else if ((bool)e.Value)
			{
				userRoles.Roles.Add(GetRoleFromByColumnName(e.Column.FieldName));
			}
			else
			{
				userRoles.Roles.Remove(GetRoleFromByColumnName(e.Column.FieldName));
			}
		}
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		try
		{
			base.DialogValidation(args);
			_usersRolesGrid.View.CommitEditing();
			if (!ConfigHelper.ASAdminLogin && Settings.SystemType != SystemTypes.SalesManagement && !_userRoles.Any((Database.UserRoles p) => p.UserIsAdmin && p.UserLoginEnabled && p.Roles.Count > 0))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.OneMustBeAdmin);
			}
			if (Mode == DialogWindowMode.NewMode)
			{
				using (CloudClient cloudClient = Settings.CloudClient)
				{
					string text = cloudClient.AS7X.CheckCloudDatabaseName(_name.Text);
					if (!string.IsNullOrEmpty(text))
					{
						throw new ASException(text);
					}
					DatabaseReservationRequest request = new DatabaseReservationRequest
					{
						DbName = _name.Text,
						Caption = _comment.Text,
						Key = Settings.LoginService.Key,
						PSMode = Settings.LoginService.PSMode,
						AppService = Settings.LoginService.ServiceAddress
					};
					DatabaseReservationResponse databaseReservationResponse = cloudClient.AS7X.ReserveCloudDatabase(request);
					DatabaseCreateRequest databaseCreateRequest = new DatabaseCreateRequest();
					databaseCreateRequest.DbId = databaseReservationResponse.DbId;
					databaseCreateRequest.Template = databaseReservationResponse.SuggestedTemplate;
					new CreateDatabaseProgressWindow(databaseCreateRequest, databaseReservationResponse.EdgeService).ShowDialog();
					DatabaseHelper.UpdateUsersRoles(databaseReservationResponse.DbId, _userRoles);
					if (Settings.HostingEnvironment.CurrentViewPanel != null)
					{
						Settings.HostingEnvironment.CurrentViewPanel.Update(databaseReservationResponse.DbId);
					}
					return;
				}
			}
			if (!ConfigHelper.ASAdminLogin)
			{
				DatabaseHelper.UpdateDatabaseInfo(_database.DbId, _comment.Text);
			}
			DatabaseHelper.UpdateUsersRoles(_database.DbId, _userRoles);
			if (Settings.HostingEnvironment.CurrentViewPanel != null)
			{
				Settings.HostingEnvironment.CurrentViewPanel.Update(_database.DbId);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
		}
	}

	private static short GetRoleFromByColumnName(string name)
	{
		return Convert.ToInt16(name.Split('_').LastOrDefault());
	}

	private void LoadDataToControls()
	{
		_name.EditValue = _database.Name;
		_comment.EditValue = _database.Caption;
		_userRoles = _database.UsersRoles.ToList();
		_usersRolesGrid.ItemsSource = _userRoles;
	}

	private void SetControlsReadOnly()
	{
		bool flag = Mode == DialogWindowMode.ViewMode;
		_name.IsReadOnly = (Mode == DialogWindowMode.EditMode) | flag;
		_comment.IsReadOnly = flag;
		_usersRolesGrid.IsReadOnly = flag;
	}
}
