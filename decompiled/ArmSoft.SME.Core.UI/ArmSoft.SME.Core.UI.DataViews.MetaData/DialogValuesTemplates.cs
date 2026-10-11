using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;

namespace ArmSoft.SME.Core.UI.DataViews.MetaData;

[DataView(40)]
public class DialogValuesTemplates : DataView<DialogValueTemplatesDataProvider.DataRow, DialogValueTemplatesDataProvider.Param>
{
	private class CopyDialog : DialogWindow
	{
		private readonly TextEditExt _dialogNameControl;

		private readonly TextEditExt _userNameControl;

		private readonly TextEditExt _templateNameControl;

		private readonly MultiSelectDropDownControl _userNames;

		private readonly DialogValueTemplatesDataProvider.DataRow _currentRow;

		public CopyDialog()
		{
			Mode = DialogWindowMode.CreateCopyMode;
			Caption = ArmSoft.SME.Common.Resources.DialogValuesGroup;
			_currentRow = (DialogValueTemplatesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			_dialogNameControl = AddStringControl("_dialogNameControl", ArmSoft.SME.Common.Resources.DialogName, 30);
			_dialogNameControl.EditValue = _currentRow.Group;
			_dialogNameControl.IsReadOnly = true;
			_userNameControl = AddStringControl("_userNameControl", ArmSoft.SME.Common.Resources.User, 50);
			_userNameControl.EditValue = _currentRow.UserName;
			_userNameControl.IsReadOnly = true;
			_templateNameControl = AddStringControl("_templateNameControl", ArmSoft.SME.Common.Resources.ValuesGroupName, 100, isRequired: true);
			_templateNameControl.EditValue = _currentRow.Caption;
			_templateNameControl.IsReadOnly = true;
			_userNames = AddMultiSelectDropDownControl("_userNames", ArmSoft.SME.Common.Resources.Users, new UsersFilter(), isRequired: true);
		}

		internal void CopyValues()
		{
			Mouse.OverrideCursor = Cursors.Wait;
			string[] userNames = _userNames.SelectedString.Split(',');
			List<int> list = Settings.DialogService.CopyTemplate(_currentRow.Id, userNames);
			if (list.Count > 0)
			{
				Settings.HostingEnvironment.CurrentViewPanel.Update(list.Cast<object>().ToList());
			}
			Mouse.OverrideCursor = null;
		}
	}

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowCreateCopy => AllowOperation;

	public override bool AllowDeleteSelected => true;

	public DialogValuesTemplates()
	{
		DataProvider = new DialogValueTemplatesDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
		Columns["Id"].Visible = false;
		Columns["UserId"].Visible = false;
		Columns["Body"].Visible = false;
	}

	public override void CreateCopy()
	{
		CopyDialog copyDialog = new CopyDialog();
		if (copyDialog.ShowDialog().Value)
		{
			copyDialog.CopyValues();
		}
	}

	public override void Delete()
	{
		DialogValueTemplatesDataProvider.DataRow dataRow = (DialogValueTemplatesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			Settings.DialogService.DeleteTemplate(dataRow.Id);
			Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.Id);
		}
	}

	public override void DeleteSelected()
	{
		IEnumerable<int> enumerable = from r in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<DialogValueTemplatesDataProvider.DataRow>()
			select r.Id;
		Settings.DialogService.DeleteTemplates(enumerable);
		Settings.HostingEnvironment.CurrentViewPanel.Update(enumerable.Cast<object>().ToList());
	}
}
