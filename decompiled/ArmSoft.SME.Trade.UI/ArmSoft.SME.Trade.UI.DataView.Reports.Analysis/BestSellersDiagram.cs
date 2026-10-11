using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.DataView.Reports.Analysis;

public class BestSellersDiagram : AsReport
{
	public class BestSellersChartRow
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string Sum { get; set; }

		public string ForPie { get; set; }
	}

	private AsChart ch;

	private ComboBoxExt cmb;

	private ComboBoxExt cmb2;

	private ComboBoxExt cmb3;

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

	public CommonConst.SalesSumType SumType
	{
		get
		{
			return (CommonConst.SalesSumType)_parameters["SumType"].Value;
		}
		set
		{
			_parameters["SumType"].Value = value;
		}
	}

	public CommonConst.BestSellersCompareMode CompareMode
	{
		get
		{
			return (CommonConst.BestSellersCompareMode)_parameters["GoodsCompareMode"].Value;
		}
		set
		{
			_parameters["GoodsCompareMode"].Value = value;
		}
	}

	public string Group
	{
		get
		{
			return (string)_parameters["GoodGroup"].Value;
		}
		set
		{
			_parameters["GoodGroup"].Value = value;
		}
	}

	public int TopCount
	{
		get
		{
			return (int)_parameters["TopCount"].Value;
		}
		set
		{
			_parameters["TopCount"].Value = value;
		}
	}

	private bool _bringDialog { get; set; }

	public override string Name => "BestSellersReport";

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

	public BestSellersDiagram(bool showDialogOnNew = true)
	{
		DialogSupported = true;
		ReportPanel.IsSpreadSheetMode = false;
		_bringDialog = showDialogOnNew;
		HelpId = "Trade_Reports_Analysis_Bestsellers";
		AddParameter("DateBegin", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("DateEnd", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("GoodGroup", ArmSoft.SME.Common.Resources.Group, "");
		AddParameter("ChartKind", ArmSoft.SME.Common.Resources.DiagramKind, ChartType.Bars2d);
		AddParameter("SumType", ArmSoft.SME.Common.Resources.TypeWord, CommonConst.SalesSumType.SumNCC);
		AddParameter("GoodsCompareMode", ArmSoft.SME.Common.Resources.CompareFeature, CommonConst.BestSellersCompareMode.Quantity);
		AddParameter("TopCount", ArmSoft.SME.Common.Resources.Top, 5);
	}

	public override string Caption()
	{
		return ArmSoft.SME.Common.Resources.BestSellers;
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
		dialogWindow.Caption = ArmSoft.SME.Common.Resources.BestSellers;
		dialogWindow.Name = Name;
		dialogWindow.HelpId = HelpId;
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		DateEditControl dateEditControl = dialogWindow.AddDateControl(container, "DateBegin", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, storeValue: true, ArmSoft.SME.Common.Resources.DatePeriodBegin);
		dateEditControl.EditValue = (useParamValues ? StartDate : Settings.HostingEnvironment.DateBegin);
		DateEditControl dateEditControl2 = dialogWindow.AddDateControl(container, "DateEnd", "", useLongDate: false, isRequired: true, storeValue: true, ArmSoft.SME.Common.Resources.DatePeriodEnd);
		dateEditControl2.EditValue = (useParamValues ? EndDate : Settings.HostingEnvironment.DateEnd);
		dialogWindow.AddSpecialDateButton(container, dateEditControl, dateEditControl2);
		if (!isRefreshMode)
		{
			dialogWindow.AddStoreValuesButton(container);
		}
		DropDownControl dropDownControl = dialogWindow.AddDropDownControl("GoodGroup", ArmSoft.SME.Trade.Common.Resources.MTGroup, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, storeValue: true);
		dropDownControl.Code = Group;
		dialogWindow.AddLine();
		cmb = dialogWindow.AddDiagramKind(null, "cmbDiagramKind", storeValue: true);
		cmb.SelectedIndex = (int)ChartKind;
		cmb2 = new ComboBoxExt();
		cmb2.ItemsSource = FiltersHelper.SumTypeList();
		dialogWindow.AddControl("SumType", ArmSoft.SME.Trade.Common.Resources.SummaType, cmb2, storeValue: true);
		cmb2.IsEnabled = true;
		if (SumType == CommonConst.SalesSumType.SumNCC)
		{
			cmb2.SelectedIndex = 0;
		}
		else if (SumType == CommonConst.SalesSumType.WithVAT)
		{
			cmb2.SelectedIndex = 1;
		}
		else
		{
			cmb2.SelectedIndex = 2;
		}
		cmb3 = new ComboBoxExt();
		cmb3.ItemsSource = FiltersHelper.CompareFeaturesList();
		dialogWindow.AddControl("CompareType", ArmSoft.SME.Common.Resources.CompareFeature, cmb3, storeValue: true);
		cmb3.IsEnabled = true;
		cmb3.SelectedIndex = (int)CompareMode;
		SpinEditExt spinEditExt = dialogWindow.AddSpinEdit("TopCount", ArmSoft.SME.Common.Resources.Top, 1m, 100m, 0.0, storeValue: true);
		if (TopCount >= 1 && TopCount <= 100)
		{
			spinEditExt.EditValue = TopCount;
		}
		else
		{
			spinEditExt.EditValue = 5;
		}
		dialogWindow.WindowValidation += DiagramAnalysisHelper.DialogValidation;
		if (dialogWindow.ShowDialog().Value)
		{
			StartDate = (DateOnly)dialogWindow["DateBegin"];
			EndDate = (DateOnly)dialogWindow["DateEnd"];
			ChartKind = (ChartType)Convert.ToInt16(dialogWindow["cmbDiagramKind"]);
			if (cmb2.SelectedIndex == 1)
			{
				SumType = CommonConst.SalesSumType.WithVAT;
			}
			else if (cmb2.SelectedIndex == 2)
			{
				SumType = CommonConst.SalesSumType.SumCur;
			}
			else
			{
				SumType = CommonConst.SalesSumType.SumNCC;
			}
			Group = dropDownControl.Code;
			CompareMode = (CommonConst.BestSellersCompareMode)cmb3.SelectedIndex;
			TopCount = Convert.ToInt32(dialogWindow["TopCount"]);
			return true;
		}
		return false;
	}

	public override bool BeforeShowReport()
	{
		Mouse.OverrideCursor = Cursors.Wait;
		List<AsRepColumn> list = new List<AsRepColumn>();
		short num = 80;
		short num2 = 240;
		short num3 = 100;
		AsRepColumn asRepColumn = new AsRepColumn(ArmSoft.SME.Common.Resources.Code, num);
		asRepColumn.ValuesStyle = AsRepColStyle.TextStyle;
		asRepColumn.CaptionAlignment = TextAlignment.Left;
		AsRepColumn asRepColumn2 = new AsRepColumn(ArmSoft.SME.Common.Resources.Name, num2);
		asRepColumn2.ValuesStyle = AsRepColStyle.TextStyle;
		asRepColumn2.CaptionAlignment = TextAlignment.Left;
		AsRepColumn asRepColumn3 = new AsRepColumn(DiagramAnalysisHelper.GetRepColName(CompareMode), num3);
		asRepColumn3.ValuesStyle = AsRepColStyle.CurrencyStyle;
		asRepColumn3.CaptionAlignment = TextAlignment.Center;
		if (CompareMode == CommonConst.BestSellersCompareMode.ProfitPercent || CompareMode == CommonConst.BestSellersCompareMode.Quantity)
		{
			asRepColumn3.CurrencyPrecision = 3;
		}
		else
		{
			asRepColumn3.CurrencyPrecision = 2;
		}
		list.Add(asRepColumn);
		list.Add(asRepColumn2);
		list.Add(asRepColumn3);
		ReportPanel.ClearAll();
		ReportPanel.UnmovableHeaderLinesCount = 3;
		ReportPanel.AddString(Caption() + "  " + StartDate.FormatDDMMYY() + " - " + EndDate.FormatDDMMYY(), AsRepRowStyle.CaptionRow);
		string text = DiagramAnalysisHelper.SumTypeText(SumType);
		if (!string.IsNullOrEmpty(Group))
		{
			text = text + ", " + TradeSettings.TradeCacheService.MTGroup.GetItem(Group)?.Caption;
		}
		if (CompareMode != CommonConst.BestSellersCompareMode.Quantity)
		{
			ReportPanel.AddString(text, AsRepRowStyle.CaptionRow);
		}
		ReportPanel.AddString(string.Format(ArmSoft.SME.Common.Resources.BestSellersTopCount, TopCount), AsRepRowStyle.CaptionRow);
		ReportPanel.AddTableDefinition("Table", list);
		ReportPanel.AddTableColumnsHeader("Table");
		ch = new AsChart();
		ch.ChartTitle = Caption() + ((CompareMode == CommonConst.BestSellersCompareMode.Quantity) ? "" : ("\r\n" + text));
		ch.ShowLegend = true;
		ch.Type = ChartKind;
		ch.ShowAxisXLabels = true;
		ch.AxisXTitle = ArmSoft.SME.Trade.Common.Resources.Goods;
		ch.AxisYTitle = DiagramAnalysisHelper.GetRepColName(CompareMode);
		ch.DataSource = GetChartRows(StartDate, EndDate, CompareMode, TopCount, Group, SumType, out var _, ReportPanel);
		if (ChartKind != ChartType.Pie2D && ChartKind != ChartType.Pie3D)
		{
			ch.SetSeriesInfo("Code", "Sum", "Caption");
		}
		else
		{
			ch.SetSeriesInfo("Caption", "Sum", "ForPie");
		}
		ReportPanel.EvenOddStyle = true;
		ReportPanel.Chart = ch;
		ch.UpdateLayout();
		Mouse.OverrideCursor = null;
		return true;
	}

	public static BindingList<BestSellersChartRow> GetChartRows(DateOnly startDate, DateOnly endDate, CommonConst.BestSellersCompareMode compareMode, int topCount, string group, CommonConst.SalesSumType sumType, out BindingList<BestSellersChartRow> bestSellerRows, AsReportViewer reportPanel = null)
	{
		List<BestSellersDataProvider.DataRow> rows = new BestSellersDataProvider(TradeSettings.CreateTradeClient()).GetRows(new BestSellersDataProvider.Param
		{
			StartDate = startDate,
			EndDate = endDate,
			CompareMode = (byte)compareMode,
			TopsCount = topCount,
			SalesSumType = (byte)sumType,
			ItemGroup = group
		});
		bestSellerRows = ((reportPanel == null) ? new BindingList<BestSellersChartRow>() : null);
		BindingList<BestSellersChartRow> bindingList = new BindingList<BestSellersChartRow>();
		foreach (BestSellersDataProvider.DataRow item2 in rows)
		{
			if (reportPanel != null)
			{
				List<object> values = new List<object> { item2.ItemCode, item2.Caption, item2.Summ };
				reportPanel.AddTableRow("Table", values);
			}
			else
			{
				BestSellersChartRow item = new BestSellersChartRow
				{
					Code = item2.ItemCode,
					Caption = item2.Caption,
					Sum = item2.Summ.ToString()
				};
				bestSellerRows.Add(item);
			}
			BestSellersChartRow bestSellersChartRow = new BestSellersChartRow
			{
				Code = item2.Caption,
				Caption = item2.ItemCode + " " + item2.Caption,
				ForPie = "1"
			};
			switch (compareMode)
			{
			case CommonConst.BestSellersCompareMode.ProfitPercent:
				bestSellersChartRow.Sum = item2.Summ.CurrencyFormat(6, 3);
				break;
			case CommonConst.BestSellersCompareMode.Quantity:
				bestSellersChartRow.Sum = item2.Summ.CurrencyFormat(20, 3);
				break;
			default:
				bestSellersChartRow.Sum = item2.Summ.CurrencyFormat(20, 2);
				break;
			}
			bindingList.Add(bestSellersChartRow);
		}
		return bindingList;
	}
}
