using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Layout;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

[DataView(24)]
public class DataViewLayouts : DataView<DataViewLayoutsDataProvider.DataRow, DataViewLayoutsDataProvider.Param>
{
	private class EditLayoutDialog : DialogWindow
	{
		private TextEditExt _layoutCaption;

		private DataViewLayoutsDataProvider.DataRow _currentRow;

		private readonly DataViewLayouts _dataView;

		public EditLayoutDialog(DataViewLayouts idv)
		{
			Caption = ArmSoft.SME.Common.Resources.LayoutTitle + " (" + ArmSoft.SME.Common.Resources.Edit + ")";
			_dataView = idv;
		}

		internal bool Invoke()
		{
			_currentRow = (DataViewLayoutsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			HelpId = _dataView.HelpId;
			_layoutCaption = AddStringControl("Caption", ArmSoft.SME.Common.Resources.LayoutName, 50, isRequired: true);
			_layoutCaption.EditValue = _currentRow.Caption;
			WindowValidation += EditLayoutDialog_WindowValidation;
			return Convert.ToBoolean(ShowDialog());
		}

		private void EditLayoutDialog_WindowValidation(DialogWindow window, DialogWindowValidationArgs args)
		{
			try
			{
				EditValidation();
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}

		private void EditValidation()
		{
			Mouse.OverrideCursor = Cursors.Wait;
			string text = _layoutCaption.Text.Trim();
			Settings.LayoutService.EditCaption(new LayoutRow
			{
				UserId = _currentRow.UserID,
				Name = _currentRow.DataView,
				Caption = _currentRow.Caption
			}, text);
			Settings.HostingEnvironment.CurrentViewPanel.Update(new DataViewLayoutsDataProvider.DataRow.DataRowKey
			{
				UserID = _currentRow.UserID,
				DataView = _currentRow.DataView,
				Caption = _currentRow.Caption
			});
			Settings.HostingEnvironment.CurrentViewPanel.Update(new DataViewLayoutsDataProvider.DataRow.DataRowKey
			{
				UserID = _currentRow.UserID,
				DataView = _currentRow.DataView,
				Caption = text
			});
			Mouse.OverrideCursor = null;
		}
	}

	private class CopyLayoutDialog : DialogWindow
	{
		private readonly DataViewLayouts _dataView;

		private TextEditExt _dataViewTE;

		private TextEditExt _dataViewCaptionTE;

		private TextEditExt _userNameTE;

		private TextEditExt _layoutCaption;

		private MultiSelectDropDownControl _userNames;

		private DataViewLayoutsDataProvider.DataRow _currentRow;

		public CopyLayoutDialog(DataViewLayouts idv)
		{
			Caption = ArmSoft.SME.Common.Resources.LayoutTitle + " (" + ArmSoft.SME.Common.Resources.Copy + ")";
			_dataView = idv;
		}

		internal bool Invoke()
		{
			_currentRow = (DataViewLayoutsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			HelpId = _dataView.HelpId;
			_dataViewTE = AddStringControl("DataView", ArmSoft.SME.Common.Resources.DataViewDefinition, 50);
			_dataViewTE.EditValue = _currentRow.DataView;
			_dataViewTE.IsReadOnly = true;
			_dataViewCaptionTE = AddStringControl("DataViewCaption", ArmSoft.SME.Common.Resources.DataViewCaption, 255);
			_dataViewCaptionTE.EditValue = _currentRow.DataViewCaption;
			_dataViewCaptionTE.IsReadOnly = true;
			_userNameTE = AddStringControl("USER", ArmSoft.SME.Common.Resources.User, 50);
			_userNameTE.EditValue = _currentRow.UserName;
			_userNameTE.IsReadOnly = true;
			_layoutCaption = AddStringControl("Caption", ArmSoft.SME.Common.Resources.LayoutName, 50, isRequired: true);
			_layoutCaption.EditValue = _currentRow.Caption;
			_userNames = AddMultiSelectDropDownControl("UserNames", ArmSoft.SME.Common.Resources.Users, new UsersFilter(), isRequired: true);
			WindowValidation += CopyLayoutDialog_WindowValidation;
			return Convert.ToBoolean(ShowDialog());
		}

		private void CopyLayoutDialog_WindowValidation(DialogWindow window, DialogWindowValidationArgs args)
		{
			try
			{
				CopyValidation();
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
				args.SetFocusWhenInvalidSave = _userNames;
			}
		}

		private void CopyValidation()
		{
			Mouse.OverrideCursor = Cursors.Wait;
			string[] array = _userNames.SelectedString.Split(',');
			string body = Settings.LayoutService.GetBody(new LayoutRow
			{
				UserId = _currentRow.UserID,
				Name = _currentRow.DataView,
				Caption = _currentRow.Caption
			});
			string text = _layoutCaption.Text.Trim();
			List<int> list = new List<int>();
			string[] array2 = array;
			foreach (string name in array2)
			{
				int userId = Settings.CoreCacheService.User.GetItemByName(name).UserId;
				if (userId != _currentRow.UserID || _currentRow.Caption != text)
				{
					Settings.LayoutService.Store(new LayoutRow
					{
						UserId = userId,
						Name = _currentRow.DataView,
						Caption = text,
						LayoutType = (byte)_currentRow.Type,
						Body = body
					});
					list.Add(userId);
				}
			}
			foreach (int item in list)
			{
				Settings.HostingEnvironment.CurrentViewPanel.Update(new DataViewLayoutsDataProvider.DataRow.DataRowKey
				{
					UserID = item,
					DataView = _currentRow.DataView,
					Caption = text
				});
			}
			Mouse.OverrideCursor = null;
		}
	}

	public override string HelpId => "DataViewLayouts";

	public override bool AllowEdit => true;

	public override bool AllowDelete => true;

	public override bool AllowCreateCopy => true;

	public DataViewLayouts()
	{
		DataProvider = new DataViewLayoutsDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("UserID", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("DataView");
			dvp.Set_ColumnSortOrder("Caption");
			dvp.Set_ColumnSortOrder("UserName");
		}
	}

	public override void Edit()
	{
		new EditLayoutDialog(this).Invoke();
	}

	public override void Delete()
	{
		DataViewLayoutsDataProvider.DataRow dataRow = (DataViewLayoutsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null && MessageBox.Show(Resources.ConfirmLayoutDelete, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			Settings.LayoutService.Delete(new LayoutRow
			{
				UserId = dataRow.UserID,
				Name = dataRow.DataView,
				Caption = dataRow.Caption
			});
			Settings.HostingEnvironment.CurrentViewPanel.Update(new DataViewLayoutsDataProvider.DataRow.DataRowKey
			{
				UserID = dataRow.UserID,
				DataView = dataRow.DataView,
				Caption = dataRow.Caption
			});
		}
	}

	public override void CreateCopy()
	{
		new CopyLayoutDialog(this).Invoke();
	}
}
