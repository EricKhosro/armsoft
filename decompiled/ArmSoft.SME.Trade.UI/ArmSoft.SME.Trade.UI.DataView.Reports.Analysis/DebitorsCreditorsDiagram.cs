using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.DataView.Reports.Analysis;

public class DebitorsCreditorsDiagram : AsReport
{
	public class RemByDbCr
	{
		public string Date { get; set; }

		public decimal DbRem { get; set; }

		public decimal CrRem { get; set; }

		public string DbCaption { get; set; } = Resources.DebitorRems;

		public string CrCaption { get; set; } = Resources.CreditorRems;
	}

	private AsChart ch;

	private bool _bringDialog;

	private ComboBoxExt cmb;

	private ComboBoxExt cmb2;

	public ChartType ChartKind
	{
		get
		{
			return (ChartType)_parameters["ChartKind"].Value;
		}
		set
		{
			_parameters["ChartKind"].Value = value;
		}
	}

	public DatePeriodTypeSE CalcPeriod
	{
		get
		{
			return SmartEnum<DatePeriodTypeSE, short>.FromValue(Convert.ToInt16(_parameters["CalcPeriod"].Value));
		}
		set
		{
			_parameters["CalcPeriod"].Value = value.Value;
		}
	}

	public override string Name => "AnalysisDebitorsCreditors";

	public DateOnly StartDate
	{
		get
		{
			return (DateOnly)_parameters["DateBegin"].Value;
		}
		set
		{
			_parameters["DateBegin"].Value = value;
		}
	}

	public DateOnly EndDate
	{
		get
		{
			return (DateOnly)_parameters["DateEnd"].Value;
		}
		set
		{
			_parameters["DateEnd"].Value = value;
		}
	}

	public DebitorsCreditorsDiagram(bool showDialogOnNew = true)
	{
		DialogSupported = true;
		ReportPanel.IsSpreadSheetMode = false;
		_bringDialog = showDialogOnNew;
		HelpId = "Trade_Reports_Analysis_ReceivableandPayableRem";
		AddParameter("DateBegin", Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("DateEnd", Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("ChartKind", Resources.DiagramKind, ChartType.Bars2d);
		AddParameter("CalcPeriod", Resources.ChartsBy, DatePeriodTypeSE.Month.Value);
	}

	public override string Caption()
	{
		return Resources.DbCrRems + " AMD";
	}

	public override string ReportTitle()
	{
		return DiagramAnalysisHelper.GetReportTitle(CaptionToolTip(), _parameters);
	}

	public override bool ShowDialog(bool useParamValues, bool isRefreshMode)
	{
		if (!_bringDialog && !isRefreshMode)
		{
			return true;
		}
		DialogWindow dialogWindow = new DialogWindow();
		dialogWindow.Caption = Resources.DbCrRems;
		dialogWindow.Name = Name;
		dialogWindow.HelpId = HelpId;
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		if (!isRefreshMode)
		{
			dialogWindow.AddStoreValuesButton(container);
		}
		DateEditControl dateEditControl = dialogWindow.AddDateControl(container, "DateBegin", Resources.DatePeriod, useLongDate: false, isRequired: true, storeValue: true, Resources.DatePeriodBegin);
		dateEditControl.EditValue = (useParamValues ? StartDate : Settings.HostingEnvironment.DateBegin);
		DateEditControl dateEditControl2 = dialogWindow.AddDateControl(container, "DateEnd", "", useLongDate: false, isRequired: true, storeValue: true, Resources.DatePeriodEnd);
		dateEditControl2.EditValue = (useParamValues ? EndDate : Settings.HostingEnvironment.DateEnd);
		dialogWindow.AddSpecialDateButton(container, dateEditControl, dateEditControl2);
		dialogWindow.AddLine();
		cmb = dialogWindow.AddDiagramKind(null, "cmbDiagramKind", storeValue: true, onlyBarsAndLines: true);
		cmb.SelectedIndex = (int)ChartKind;
		cmb2 = new ComboBoxExt();
		cmb2.ItemsSource = FiltersHelper.PeriodList();
		dialogWindow.AddControl("PeriodType", Resources.ChartsBy, cmb2, storeValue: true);
		cmb2.IsEnabled = true;
		cmb2.SelectedIndex = CalcPeriod.Value;
		dialogWindow.WindowValidation += DiagramAnalysisHelper.DialogValidation;
		if (dialogWindow.ShowDialog().Value)
		{
			StartDate = (DateOnly)dialogWindow["DateBegin"];
			EndDate = (DateOnly)dialogWindow["DateEnd"];
			ChartKind = (ChartType)Convert.ToInt16(dialogWindow["cmbDiagramKind"]);
			CalcPeriod = SmartEnum<DatePeriodTypeSE, short>.FromValue(Convert.ToInt16(cmb2.SelectedIndex));
			return true;
		}
		return false;
	}

	public override bool BeforeShowReport()
	{
		List<AsRepColumn> list = new List<AsRepColumn>();
		short num = 130;
		short num2 = 120;
		short num3 = 120;
		AsRepColumn asRepColumn = new AsRepColumn(CalcPeriod.Caption, num);
		asRepColumn.ValuesStyle = AsRepColStyle.TextStyle;
		asRepColumn.CaptionAlignment = TextAlignment.Left;
		AsRepColumn asRepColumn2 = new AsRepColumn(Resources.DebitorRems, num2);
		asRepColumn2.ValuesStyle = AsRepColStyle.CurrencyStyle;
		asRepColumn2.CaptionAlignment = TextAlignment.Center;
		AsRepColumn asRepColumn3 = new AsRepColumn(Resources.CreditorRems, num3);
		asRepColumn3.ValuesStyle = AsRepColStyle.CurrencyStyle;
		asRepColumn3.CaptionAlignment = TextAlignment.Center;
		list.Add(asRepColumn);
		list.Add(asRepColumn2);
		list.Add(asRepColumn3);
		ReportPanel.ClearAll();
		ReportPanel.UnmovableHeaderLinesCount = 2;
		ReportPanel.AddString(Caption() + "  " + StartDate.FormatDDMMYY() + " - " + EndDate.FormatDDMMYY(), AsRepRowStyle.CaptionRow);
		ReportPanel.AddTableDefinition("Table", list);
		ReportPanel.AddTableColumnsHeader("Table");
		Mouse.OverrideCursor = Cursors.Wait;
		ReportPanel.EvenOddStyle = true;
		ch = new AsChart();
		ch.ChartTitle = Caption();
		ch.ShowLegend = true;
		ch.Type = ChartKind;
		ch.ShowAxisXLabels = true;
		ch.AxisXTitle = CalcPeriod.Caption;
		ch.AxisYTitle = Resources.Summa;
		ch.DataSource = GetChartRows(StartDate, EndDate, CalcPeriod, ReportPanel);
		ch.SetSeriesInfo("ChartDate", "Sum", "Code");
		ReportPanel.Chart = ch;
		ch.UpdateLayout();
		Mouse.OverrideCursor = null;
		return true;
	}

	public static BindingList<ChartRow> GetChartRows(DateOnly startDate, DateOnly endDate, DatePeriodTypeSE periodType, AsReportViewer reportPanel = null)
	{
		List<DebitorsCreditorsDataProvider.DataRow> rows = new DebitorsCreditorsDataProvider(TradeSettings.CreateTradeClient()).GetRows(new DebitorsCreditorsDataProvider.Param
		{
			StartDate = startDate,
			EndDate = endDate,
			PeriodMode = periodType.Value
		});
		BindingList<ChartRow> bindingList = new BindingList<ChartRow>();
		foreach (DebitorsCreditorsDataProvider.DataRow item in rows)
		{
			if (reportPanel != null)
			{
				List<object> list = new List<object>();
				list.Add(item.PeriodCaption);
				list.Add(item.DbRem);
				list.Add(item.CrRem);
				reportPanel.AddTableRow("Table", list);
			}
			bindingList.Add(new ChartRow
			{
				Code = Resources.DebitorRems,
				Sum = item.DbRem,
				ChartDate = item.PeriodCaption
			});
			bindingList.Add(new ChartRow
			{
				Code = Resources.CreditorRems,
				Sum = item.CrRem,
				ChartDate = item.PeriodCaption
			});
		}
		return bindingList;
	}
}
