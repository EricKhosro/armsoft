using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.AppManager;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.DBIntegrity;

public class CheckIntegrityDialogWindow : DialogWindow
{
	public class IntegrityElementRow
	{
		public bool Selected { get; set; }

		public string IntegrityElement { get; set; }

		public string IntegrityElementName { get; set; }

		public IntegrityElementRow(string code, string caption)
		{
			IntegrityElement = code;
			IntegrityElementName = caption;
			Selected = true;
		}
	}

	private readonly BindingList<IntegrityElementRow> _integrityElementRows;

	private readonly DocumentGridControl _integrityElementsGrid;

	protected virtual string DprNameConst { get; } = "DBIntegrityCheck";

	public CheckIntegrityDialogWindow()
		: base(Settings.HostingEnvironment)
	{
		Title = ArmSoft.SME.Common.Resources.DataIntegrityChecking;
		HelpId = "AdminTools_DataIntegrity";
		ResizeMode = ResizeMode.CanResize;
		SizeToContent = SizeToContent.Manual;
		Width = SizeCalculator.Width(600.0);
		Height = SizeCalculator.Height(350.0);
		_integrityElementsGrid = AddGridControl("IntegrityElements");
		_integrityElementsGrid.AllowAdd = false;
		_integrityElementsGrid.AllowDelete = false;
		_integrityElementsGrid.AllowDuplicate = false;
		_integrityElementsGrid.ShowButtons = false;
		DocumentGridColumn documentGridColumn = _integrityElementsGrid.AddBooleanColumn("Selected", "");
		documentGridColumn.Width = 50.0;
		documentGridColumn.AllowSorting = DefaultBoolean.True;
		_integrityElementsGrid.AddStringColumn("IntegrityElementName", "", 100).AllowEditing = DefaultBoolean.False;
		TableView view = _integrityElementsGrid.View;
		view.AllowMoveColumnToDropArea = false;
		view.IsColumnMenuEnabled = false;
		view.AllowSorting = false;
		view.AutoWidth = true;
		view.ColumnHeaderClick += new SafeEventHandler<ColumnHeaderClickEventArgs>(IntegrityElementsView_ColumnHeaderClick).Handler;
		_integrityElementRows = GetIntegrityElements();
		_integrityElementsGrid.ItemsSource = _integrityElementRows;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(CheckIntegrityDialogWindow_WindowValidation).Handler;
	}

	private void CheckIntegrityDialogWindow_WindowValidation(object sender, DialogWindowValidationArgs e)
	{
		if (!_integrityElementRows.Any((IntegrityElementRow x) => x.Selected))
		{
			e.isValid = false;
			return;
		}
		DBCheckIntegrityRequest dBCheckIntegrityRequest = new DBCheckIntegrityRequest();
		foreach (IntegrityElementRow item in _integrityElementRows.Where((IntegrityElementRow x) => x.Selected))
		{
			dBCheckIntegrityRequest.Elements.Add(Convert.ToInt16(item.IntegrityElement));
		}
		DBCheckIntegrityResponse dBCheckIntegrityResponse = DataProcessingRequestHelper.Execute<DBCheckIntegrityRequest, DBCheckIntegrityResponse>(DPRType.Other, DprNameConst, dBCheckIntegrityRequest);
		if (dBCheckIntegrityResponse == null)
		{
			return;
		}
		if (dBCheckIntegrityResponse.ErrorMessages.Count == 0)
		{
			MessageBox.Show(ArmSoft.SME.Common.Resources.OperationSucceeded, Settings.ProgramName, MessageBoxButton.OK);
			return;
		}
		CheckIntegrityReport checkIntegrityReport = new CheckIntegrityReport();
		foreach (GrouppedErrors errorMessage in dBCheckIntegrityResponse.ErrorMessages)
		{
			checkIntegrityReport.AddError(errorMessage.Header, errorMessage.Errors);
		}
		checkIntegrityReport.Show();
	}

	private static BindingList<IntegrityElementRow> GetIntegrityElements()
	{
		BindingList<IntegrityElementRow> bindingList = new BindingList<IntegrityElementRow>();
		foreach (CodeName item in from x in SmartEnumHelper.GetPartialSmartEnumCodeCaption(typeof(DBCheckIntegrityElementSE))
			orderby x.Code
			select x)
		{
			bindingList.Add(new IntegrityElementRow(item.Code, item.Name));
		}
		return bindingList;
	}

	private void IntegrityElementsView_ColumnHeaderClick(object sender, ColumnHeaderClickEventArgs e)
	{
		if (!(e.Column.FieldName == "Selected") || _integrityElementsGrid.Grid.VisibleRowCount == 0)
		{
			return;
		}
		bool selected = !_integrityElementRows.All((IntegrityElementRow x) => x.Selected);
		foreach (IntegrityElementRow integrityElementRow in _integrityElementRows)
		{
			integrityElementRow.Selected = selected;
		}
		_integrityElementsGrid.Grid.RefreshData();
		e.Handled = true;
	}
}
