using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.DataView.Reports.Analysis;

public class SalesAnalysisDiagram : AsReport
{
	private AsChart ch;

	private bool _bringDialog;

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

	public string ItemType
	{
		get
		{
			return (string)_parameters["ItemType"].Value;
		}
		set
		{
			_parameters["ItemType"].Value = value.Trim();
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
			_parameters["GoodGroup"].Value = value.Trim();
		}
	}

	public string Storage
	{
		get
		{
			return (string)_parameters["Storage"].Value;
		}
		set
		{
			_parameters["Storage"].Value = value.Trim();
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

	public override string Name => "SalesCalculationReport";

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

	public SalesAnalysisDiagram(bool showDialogOnNew = true)
	{
		DialogSupported = true;
		ReportPanel.IsSpreadSheetMode = false;
		_bringDialog = showDialogOnNew;
		HelpId = "Trade_Reports_Analysis_Sales";
		AddParameter("DateBegin", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("DateEnd", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("ItemType", ArmSoft.SME.Common.Resources.Type, "");
		AddParameter("GoodGroup", ArmSoft.SME.Common.Resources.Group, "");
		AddParameter("Storage", ArmSoft.SME.Common.Resources.Storage, "");
		AddParameter("ChartKind", ArmSoft.SME.Common.Resources.DiagramKind, ChartType.Bars2d);
		AddParameter("SumType", ArmSoft.SME.Common.Resources.TypeWord, CommonConst.SalesSumType.SumNCC);
		AddParameter("CalcPeriod", ArmSoft.SME.Common.Resources.ChartsBy, DatePeriodTypeSE.Month.Value);
	}

	public override string Caption()
	{
		return ArmSoft.SME.Common.Resources.Sales;
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
		dialogWindow.Caption = Caption();
		dialogWindow.Name = Name;
		dialogWindow.HelpId = HelpId;
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		if (!isRefreshMode)
		{
			dialogWindow.AddStoreValuesButton(container);
		}
		DateEditControl dateEditControl = dialogWindow.AddDateControl(container, "DateBegin", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, storeValue: true, ArmSoft.SME.Common.Resources.DatePeriodBegin);
		dateEditControl.EditValue = (useParamValues ? StartDate : Settings.HostingEnvironment.DateBegin);
		DateEditControl dateEditControl2 = dialogWindow.AddDateControl(container, "DateEnd", "", useLongDate: false, isRequired: true, storeValue: true, ArmSoft.SME.Common.Resources.DatePeriodEnd);
		dateEditControl2.EditValue = (useParamValues ? EndDate : Settings.HostingEnvironment.DateEnd);
		dialogWindow.AddSpecialDateButton(container, dateEditControl, dateEditControl2);
		DropDownControl dropDownControl = dialogWindow.AddDropDownControl("ItemType", ArmSoft.SME.Common.Resources.TypeWord, new SmartEnum(typeof(GoodsTypesSE)), isRequired: false, allowAdditionButtons: false, storeValue: true);
		dropDownControl.Code = ItemType;
		DropDownControl dropDownControl2 = dialogWindow.AddDropDownControl("GoodGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, storeValue: true);
		dropDownControl2.Code = Group;
		string storage = ArmSoft.SME.Common.Resources.Storage;
		bool? checkViewOrOperationAccess = true;
		DropDownControl dropDownControl3 = dialogWindow.AddDropDownControl("Storage", storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(null, checkViewOrOperationAccess), isRequired: false, allowAdditionButtons: false, storeValue: true);
		dropDownControl3.Code = Storage;
		dialogWindow.AddLine();
		cmb = dialogWindow.AddDiagramKind(null, "cmbDiagramKind", storeValue: true, onlyBarsAndLines: true);
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
		cmb3.ItemsSource = FiltersHelper.PeriodList();
		dialogWindow.AddControl("PeriodType", ArmSoft.SME.Common.Resources.ChartsBy, cmb3, storeValue: true);
		cmb3.IsEnabled = true;
		cmb3.SelectedIndex = CalcPeriod.Value;
		dialogWindow.WindowValidation += DiagramAnalysisHelper.DialogValidation;
		if (dialogWindow.ShowDialog().Value)
		{
			StartDate = (DateOnly)dialogWindow["DateBegin"];
			EndDate = (DateOnly)dialogWindow["DateEnd"];
			ChartKind = (ChartType)(int)dialogWindow["cmbDiagramKind"];
			ItemType = dropDownControl.Code;
			Group = dropDownControl2.Code;
			Storage = dropDownControl3.Code;
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
			CalcPeriod = SmartEnum<DatePeriodTypeSE, short>.FromValue(Convert.ToInt16(cmb3.SelectedIndex));
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
		AsRepColumn asRepColumn2 = new AsRepColumn(ArmSoft.SME.Common.Resources.SelfCost, num2);
		asRepColumn2.ValuesStyle = AsRepColStyle.CurrencyStyle;
		asRepColumn2.CaptionAlignment = TextAlignment.Center;
		AsRepColumn asRepColumn3 = new AsRepColumn(ArmSoft.SME.Common.Resources.Income, num3);
		asRepColumn3.ValuesStyle = AsRepColStyle.CurrencyStyle;
		asRepColumn3.CaptionAlignment = TextAlignment.Center;
		AsRepColumn asRepColumn4 = new AsRepColumn(ArmSoft.SME.Common.Resources.ProfitWord, num3);
		asRepColumn4.ValuesStyle = AsRepColStyle.CurrencyStyle;
		asRepColumn4.CaptionAlignment = TextAlignment.Center;
		list.Add(asRepColumn);
		list.Add(asRepColumn2);
		list.Add(asRepColumn3);
		list.Add(asRepColumn4);
		ReportPanel.ClearAll();
		ReportPanel.UnmovableHeaderLinesCount = 2;
		ReportPanel.AddString(Caption() + "  " + StartDate.FormatDDMMYY() + " - " + EndDate.FormatDDMMYY(), AsRepRowStyle.CaptionRow);
		string text = "";
		if (!string.IsNullOrWhiteSpace(ItemType))
		{
			text = text + ((ItemType == ItemTypesSE.Goods.Value) ? ArmSoft.SME.Common.Resources.Product : ArmSoft.SME.Common.Resources.Service) + ", ";
		}
		text += DiagramAnalysisHelper.SumTypeText(SumType);
		if (!string.IsNullOrWhiteSpace(Group))
		{
			text = text + ", " + DiagramAnalysisHelper.GetGroupName(Group);
		}
		if (!string.IsNullOrWhiteSpace(Storage))
		{
			text = text + ", " + DiagramAnalysisHelper.GetStorageName(Storage);
		}
		ReportPanel.AddString(text, AsRepRowStyle.CaptionRow);
		ReportPanel.AddTableDefinition("Table", list);
		ReportPanel.AddTableColumnsHeader("Table");
		Mouse.OverrideCursor = Cursors.Wait;
		ch = new AsChart
		{
			ChartTitle = Caption() + "\r\n" + text,
			ShowLegend = true,
			Type = ChartKind,
			ShowAxisXLabels = true,
			AxisXTitle = CalcPeriod.Caption,
			AxisYTitle = ArmSoft.SME.Common.Resources.Summa,
			DataSource = GetChartRows(StartDate, EndDate, CalcPeriod, ItemType, Group, Storage, SumType, ReportPanel)
		};
		ch.SetSeriesInfo("ChartDate", "Sum", "Code");
		ch.UpdateLayout();
		ReportPanel.EvenOddStyle = true;
		ReportPanel.Chart = ch;
		Mouse.OverrideCursor = null;
		return true;
	}

	public static BindingList<ChartRow> GetChartRows(DateOnly startDate, DateOnly endDate, DatePeriodTypeSE periodType, string itemType, string group, string storage, CommonConst.SalesSumType sumType, AsReportViewer reportPanel = null)
	{
		List<SalesDataProvider.DataRow> rows = new SalesDataProvider(TradeSettings.CreateTradeClient()).GetRows(new SalesDataProvider.Param
		{
			StartDate = startDate,
			EndDate = endDate,
			PeriodMode = periodType.Value,
			ItemType = itemType,
			ItemGroup = group,
			SalesSumType = (short)sumType,
			Storage = storage
		});
		BindingList<ChartRow> bindingList = new BindingList<ChartRow>();
		foreach (SalesDataProvider.DataRow item in rows)
		{
			if (reportPanel != null)
			{
				int num = 4;
				List<object> list = new List<object>(num);
				CollectionsMarshal.SetCount(list, num);
				Span<object> span = CollectionsMarshal.AsSpan(list);
				span[0] = item.PeriodCaption;
				span[1] = item.Cost;
				span[2] = item.Income;
				span[3] = item.Profit;
				List<object> values = list;
				reportPanel.AddTableRow("Table", values);
			}
			bindingList.Add(new ChartRow
			{
				Code = ArmSoft.SME.Common.Resources.SelfCost,
				Sum = item.Cost,
				ChartDate = item.PeriodCaption
			});
			bindingList.Add(new ChartRow
			{
				Code = ArmSoft.SME.Common.Resources.Income,
				Sum = item.Income,
				ChartDate = item.PeriodCaption
			});
			bindingList.Add(new ChartRow
			{
				Code = ArmSoft.SME.Common.Resources.ProfitWord,
				Sum = item.Profit,
				ChartDate = item.PeriodCaption
			});
		}
		return bindingList;
	}
}
