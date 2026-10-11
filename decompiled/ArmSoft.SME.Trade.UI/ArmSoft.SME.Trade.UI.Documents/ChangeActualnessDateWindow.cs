using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Models.DPR;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Documents;

public class ChangeActualnessDateWindow : DialogWindow
{
	private class DocumentsRow
	{
		public bool Selected { get; set; }

		public string DocTypeCaption { get; set; } = string.Empty;

		public short DocType { get; set; }
	}

	public bool ResultDialog;

	private readonly DateEditControl _actualnessDate;

	private readonly CheckEditExt _bringIncomesToDayStart;

	private readonly CheckEditExt _bringExpensesToDayEnd;

	private readonly DropDownControl _bringComplectationsType;

	private readonly DropDownControl _bringMovesType;

	private readonly DocumentGridControl _documentsGrid;

	private readonly DocumentGridColumn _selectedCol;

	private readonly DocumentGridColumn _docTypeCaption;

	private readonly TableView _documentsView;

	private readonly BindingList<DocumentsRow> _documentsRows = new BindingList<DocumentsRow>();

	public EnumerateErrorsReportViewer ReportErrorList { get; set; }

	public ChangeActualnessDateWindow()
	{
		MaxHeight = Screen.PrimaryScreen.WorkingArea.Height;
		HelpId = "ChangeActualnessDate";
		Caption = ArmSoft.SME.Trade.Common.Resources.ChangeActualnessDate;
		_actualnessDate = AddDateControl("_actualnessDate", ArmSoft.SME.Trade.Common.Resources.ActualnessDate, useLongDate: false, isRequired: true);
		_actualnessDate.EditValue = Settings.ParametersService.GetGeneralValue(Definitions.ActualnessDate.Id).YYYYMMDDToDateOnly();
		_bringIncomesToDayStart = AddBooleanControl("_bringIncomesToDayStart", ArmSoft.SME.Trade.Common.Resources.BringIncomesToDayBegin);
		_bringExpensesToDayEnd = AddBooleanControl("_bringExpensesToDayEnd", ArmSoft.SME.Trade.Common.Resources.BringExpensesToDayEnd);
		_bringComplectationsType = AddDropDownControl("_bringComplectationsType", ArmSoft.SME.Trade.Common.Resources.DocMTComplectation, new SmartEnum(typeof(ChangeDocOrderNumTypesSE)));
		_bringMovesType = AddDropDownControl("_bringMovesType", ArmSoft.SME.Trade.Common.Resources.DocMTMove, new SmartEnum(typeof(ChangeDocOrderNumTypesSE)));
		LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup(null, LayoutGroupView.GroupBox, ArmSoft.SME.Trade.Common.Resources.ProcessUnprocessed);
		layoutGroupExt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
		_documentsGrid = AddGridControl(layoutGroupExt, "Documents");
		_documentsGrid.Width = SizeCalculator.Width(450.0);
		_documentsGrid.Height = SizeCalculator.Height(450.0);
		_documentsGrid.AllowAdd = false;
		_documentsGrid.AllowDelete = false;
		_documentsGrid.AllowDuplicate = false;
		_documentsGrid.ShowButtons = false;
		foreach (DocumentTypeSE item in SmartEnum<DocumentTypeSE, short>.List)
		{
			if (DefinitionMappingsService.TryResolveDocumentDefinition(item.Value, out var definition) && definition is TradeDocDefinition { ChangingActualnessDate: not false })
			{
				_documentsRows.Add(new DocumentsRow
				{
					DocTypeCaption = item.Caption,
					DocType = item.Value
				});
			}
		}
		_documentsGrid.ItemsSource = _documentsRows;
		_documentsView = _documentsGrid.View;
		_documentsView.AllowMoveColumnToDropArea = false;
		_documentsView.IsColumnMenuEnabled = false;
		_documentsView.AllowSorting = false;
		_documentsView.CellValueChanging += new SafeEventHandler<CellValueChangedEventArgs>(DocumentsView_CellValueChanging).Handler;
		_documentsView.PreviewKeyDown += new SafeEventHandler<System.Windows.Input.KeyEventArgs>(DocumentsView_PreviewKeyDown).Handler;
		_documentsView.ColumnHeaderClick += new SafeEventHandler<ColumnHeaderClickEventArgs>(DocumentsView_ColumnHeaderClick).Handler;
		_selectedCol = _documentsGrid.AddBooleanColumn("Selected", string.Empty);
		_selectedCol.Width = 50.0;
		_selectedCol.AllowSorting = DefaultBoolean.True;
		_docTypeCaption = _documentsGrid.AddStringColumn("DocTypeCaption", ArmSoft.SME.Common.Resources.Document, 50);
		_docTypeCaption.AllowEditing = DefaultBoolean.False;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(ChangeActualnessDateWindow_WindowValidation).Handler;
	}

	private void ChangeActualnessDateWindow_WindowValidation(object window, DialogWindowValidationArgs args)
	{
		try
		{
			ResultDialog = false;
			DateOnly value = _actualnessDate.EditValue.Value;
			bool value2 = _bringIncomesToDayStart.IsChecked.Value;
			bool value3 = _bringExpensesToDayEnd.IsChecked.Value;
			DateOnly value4 = Settings.ParametersService.ActualnessDate().Value;
			_documentsGrid.View.CommitEditing();
			Settings.CommonCoreHelper.CheckWorkingPeriod(value, "ActualnessDate");
			if (value < value4)
			{
				Settings.ParametersService.ActualnessDate_set(value);
				ResultDialog = true;
			}
			else
			{
				if (!(value > value4))
				{
					return;
				}
				Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
				List<short> list = new List<short>();
				foreach (DocumentsRow documentsRow in _documentsRows)
				{
					if (documentsRow.Selected)
					{
						list.Add(documentsRow.DocType);
					}
				}
				DocumentsReProcessParam request = new DocumentsReProcessParam
				{
					EndActualnessDate = value,
					BringIncomesToDayBegin = value2,
					BringExpensesToDayEnd = value3,
					BringComplectationsType = _bringComplectationsType.Code,
					BringMovesType = _bringMovesType.Code,
					DocTypes = list
				};
				DocumentsReProcessResult documentsReProcessResult = DataProcessingRequestHelper.Execute<DocumentsReProcessParam, DocumentsReProcessResult>(DPRType.Other, "DocumentsReProcess", request);
				if (documentsReProcessResult.Errors.Count > 0)
				{
					ReportErrorList = new EnumerateErrorsReportViewer(ArmSoft.SME.Trade.Common.Resources.ChangeActualnessDate + "  " + value4.FormatDDMMYY() + " -> " + value.FormatDDMMYY(), "ShowChangeActualnessDateErrors");
					foreach (KeyValuePair<string, Guid> error in documentsReProcessResult.Errors)
					{
						ReportErrorList.AddError(error.Key, error.Value);
					}
					Settings.ParametersService.Refresh();
					ResultDialog = false;
				}
				else
				{
					Settings.ParametersService.ActualnessDate_set(value);
					ResultDialog = true;
				}
				Mouse.OverrideCursor = null;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
			args.SetFocusWhenInvalidSave = _actualnessDate;
			ResultDialog = false;
		}
	}

	private void DocumentsView_CellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		_documentsGrid.View.CommitEditing();
		_documentsGrid.Grid.RefreshRow(e.RowHandle);
	}

	private void DocumentsView_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		if ((int)e.Key == 84)
		{
			InvertSelection();
			e.Handled = true;
		}
		else if ((int)e.Key == 141 || (int)e.Key == 85)
		{
			SelectOrDeselectAll(isSelect: true);
			e.Handled = true;
		}
		else if ((int)e.Key == 143 || (int)e.Key == 87)
		{
			SelectOrDeselectAll(isSelect: false);
			e.Handled = true;
		}
		else if ((int)Keyboard.Modifiers == 0 && (int)e.Key == 90)
		{
			Settings.HostingEnvironment.ShowHelp(HelpId);
			e.Handled = true;
		}
	}

	private void DocumentsView_ColumnHeaderClick(object sender, ColumnHeaderClickEventArgs e)
	{
		if (e.Column == _selectedCol && _documentsGrid.Grid.VisibleRowCount != 0)
		{
			InvertSelection();
			e.Handled = true;
		}
	}

	private void InvertSelection()
	{
		if (_selectedCol.AllowEditing == DefaultBoolean.False)
		{
			return;
		}
		foreach (DocumentsRow documentsRow in _documentsRows)
		{
			documentsRow.Selected = !documentsRow.Selected;
		}
		_documentsView.CommitEditing();
		_documentsGrid.Grid.RefreshData();
	}

	private void SelectOrDeselectAll(bool isSelect)
	{
		if (_selectedCol.AllowEditing == DefaultBoolean.False)
		{
			return;
		}
		foreach (DocumentsRow documentsRow in _documentsRows)
		{
			documentsRow.Selected = isSelect;
		}
		_documentsView.CommitEditing();
		_documentsGrid.Grid.RefreshData();
	}
}
