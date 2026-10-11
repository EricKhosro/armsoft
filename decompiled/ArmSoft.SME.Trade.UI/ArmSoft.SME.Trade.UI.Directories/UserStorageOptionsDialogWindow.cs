using System;
using System.Windows;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Directories;

[DirectoryWindow(40)]
public class UserStorageOptionsDialogWindow : DirectoryDialogWindow<UserStorageOptions>
{
	private CheckEditExt _allowOper;

	private CheckEditExt _allowView;

	private CheckEditExt _allowNegExp;

	private CheckEditExt _salesWithZeroCost;

	public UserStorageOptionsDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public UserStorageOptionsDialogWindow(UserStorageOptions directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		UserStorageOptionsDataProvider.DataRow dataRow = (UserStorageOptionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		Caption = dataRow.Caption;
		Mode = Mode;
		HelpId = HelpId;
		AddDropDownControl(Definition.Fields["Code"], new Storages()).IsReadOnly = true;
		_allowOper = AddBooleanControl(Definition.Fields["AllowOper"]);
		_allowOper.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Allow_EditValueChanged).Handler;
		_allowView = AddBooleanControl(Definition.Fields["AllowReportView"]);
		_allowNegExp = AddBooleanControl(Definition.Fields["AllowNegExp"]);
		_allowNegExp.IsReadOnly = true;
		_allowNegExp.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(AllowNegExp_EditValueChanged).Handler;
		_salesWithZeroCost = AddBooleanControl(Definition.Fields["SalesWithZeroCost"]);
		_salesWithZeroCost.IsReadOnly = true;
		if (dataRow != null)
		{
			_allowOper.IsChecked = dataRow.AllowOper;
			_allowView.IsChecked = dataRow.AllowReportView;
			_allowNegExp.IsChecked = dataRow.AllowNegativExp;
			_salesWithZeroCost.IsChecked = dataRow.SalesWithZeroCost;
		}
		SetFocusOnActivation = _allowOper;
	}

	private void Allow_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		try
		{
			_allowNegExp.IsReadOnly = !_allowOper.IsChecked.Value;
			if (_allowNegExp.IsReadOnly)
			{
				_allowNegExp.IsChecked = false;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AllowNegExp_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		try
		{
			if (!_allowNegExp.IsChecked.Value)
			{
				_salesWithZeroCost.IsChecked = false;
			}
			_salesWithZeroCost.IsReadOnly = !_allowNegExp.IsChecked.Value;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		UserStorageOptionsDataProvider.DataRow dataRow = (UserStorageOptionsDataProvider.DataRow)dvp.FocusedRow();
		UserStorageOptionsDataProvider.DataRow.UserStorageOptionsRowKey key = new UserStorageOptionsDataProvider.DataRow.UserStorageOptionsRowKey
		{
			Storage = dataRow.Storage,
			UserId = dataRow.UserId
		};
		dvp.Update(key);
	}
}
