using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Dialog;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Layout.Core;

namespace ArmSoft.SME.Trade.UI.DataView.Reports.Analysis;

public partial class AnalysisDashBoardWindow : DocumentPanel, IComponentConnector, IStyleConnector
{
	private class BestSellersParams
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public CommonConst.SalesSumType ChartSumType { get; set; }

		public CommonConst.BestSellersCompareMode ChartCompareMode { get; set; }

		public int TopCount { get; set; }

		public ChartType ChartKind { get; set; }

		public string GoodGroup { get; set; }
	}

	private class SalesParams
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public CommonConst.SalesSumType SumType { get; set; }

		public DatePeriodTypeSE PeriodType { get; set; }

		public ChartType DiagramKind { get; set; }

		public string GoodGroup { get; set; }
	}

	private class CashBankParams
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public DatePeriodTypeSE PeriodType { get; set; }

		public ChartType DiagramKind { get; set; }
	}

	private class DebtorsParams
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public DatePeriodTypeSE PeriodType { get; set; }

		public ChartType DiagramKind { get; set; }
	}

	private class DockPosition
	{
		public LayoutGroup Group { get; set; }

		public DockType DockType { get; set; }
	}

	private PopupMenu ReportContextMenu = new PopupMenu();

	private BestSellersParams BSParams;

	private CashBankParams CBParams;

	private DebtorsParams DBParams;

	private SalesParams SParams;

	private Dictionary<LayoutPanel, DockPosition> dctPanels = new Dictionary<LayoutPanel, DockPosition>();

	public AnalysisDashBoardWindow()
	{
		InitializeComponent();
		InitMenu();
		RibbonPageHome.Caption = ArmSoft.SME.Common.Resources.Home;
		RibbonPageGroupDataView.Caption = ArmSoft.SME.Common.Resources.DataView;
		BarButtonHelp.Content = ArmSoft.SME.Common.Resources.Help;
		RibbonPageHelp.Caption = ArmSoft.SME.Common.Resources.Help;
		RibbonPageGroupHelp.Caption = ArmSoft.SME.Common.Resources.Help;
		bRefreshDataView.Content = ArmSoft.SME.Common.Resources.Refresh;
		bShowDataViewDialog.Content = ArmSoft.SME.Common.Resources.ShowDialog;
		bRefreshAll.Content = ArmSoft.SME.Common.Resources.RefreshAll;
		bDetails.Content = ArmSoft.SME.Common.Resources.ViewDetails;
		InitPanelsDct();
		ContextMenuOpening += DashBoardWindow_ContextMenuOpening;
		BarButtonHelp.ItemClick += BarButtonHelp_ItemClick;
		ASFontHelper.SetFontSettings(this);
	}

	public void InitMenu()
	{
		BarButtonItemLink barButtonItemLink = new BarButtonItemLink();
		barButtonItemLink = (BarButtonItemLink)bRefreshDataView.CreateLink();
		barButtonItemLink.IsEnabled = true;
		ReportContextMenu.ItemLinks.Add(barButtonItemLink);
		barButtonItemLink = new BarButtonItemLink();
		barButtonItemLink = (BarButtonItemLink)bShowDataViewDialog.CreateLink();
		barButtonItemLink.IsEnabled = true;
		ReportContextMenu.ItemLinks.Add(barButtonItemLink);
		barButtonItemLink = new BarButtonItemLink();
		barButtonItemLink = (BarButtonItemLink)bRefreshAll.CreateLink();
		barButtonItemLink.IsEnabled = true;
		ReportContextMenu.ItemLinks.Add(barButtonItemLink);
		barButtonItemLink = new BarButtonItemLink();
		barButtonItemLink = (BarButtonItemLink)bDetails.CreateLink();
		barButtonItemLink.IsEnabled = true;
		ReportContextMenu.ItemLinks.Add(barButtonItemLink);
		BarManager.SetDXContextMenu(dockManager, ReportContextMenu);
	}

	public void InitRefresh()
	{
		LoadBestSellers();
		LoadCashBank();
		LoadSales();
		LoadDebtorsCreditors();
	}

	public void LoadSales(SalesAnalysisDiagram rep = null)
	{
		try
		{
			string itemType = "";
			string storage = "";
			CommonConst.SalesSumType sumType = CommonConst.SalesSumType.SumNCC;
			if (rep == null)
			{
				if (SParams == null)
				{
					DialogValues values = Settings.DialogService.GetValues("SalesCalculationReport");
					bool storeValues = values.StoreValues;
					Dictionary<string, string> values2 = values.Values;
					if (storeValues && values2 != null && values2.Count > 0)
					{
						SParams = new SalesParams
						{
							StartDate = values2["DATEBEGIN"].YYYYMMDDToDateOnly(),
							EndDate = values2["DATEEND"].YYYYMMDDToDateOnly(),
							DiagramKind = (ChartType)Convert.ToInt16(values2["CMBDIAGRAMKIND"]),
							SumType = (CommonConst.SalesSumType)Convert.ToInt16(values2["SUMTYPE"]),
							PeriodType = (DatePeriodTypeSE)(SmartEnum<DatePeriodTypeSE, short>)Convert.ToInt16(values2["PERIODTYPE"])
						};
						if (values2.TryGetValue("GOODGROUP", out var value))
						{
							SParams.GoodGroup = value;
						}
						else
						{
							SParams.GoodGroup = "";
						}
					}
					else
					{
						SParams = new SalesParams
						{
							StartDate = Settings.HostingEnvironment.DateBegin,
							EndDate = Settings.HostingEnvironment.DateEnd,
							DiagramKind = ChartType.Bars2d,
							SumType = CommonConst.SalesSumType.SumNCC,
							PeriodType = DatePeriodTypeSE.Month,
							GoodGroup = ""
						};
					}
				}
			}
			else
			{
				SParams = new SalesParams
				{
					StartDate = rep.StartDate,
					EndDate = rep.EndDate,
					DiagramKind = rep.ChartKind,
					SumType = rep.SumType,
					PeriodType = rep.CalcPeriod,
					GoodGroup = rep.Group
				};
			}
			SalesPanel.Caption = ArmSoft.SME.Common.Resources.Sales + " " + SParams.StartDate.FormatDDMMYY() + " - " + SParams.EndDate.FormatDDMMYY();
			txtSalesCaption.Text = DiagramAnalysisHelper.SumTypeText(SParams.SumType) + ((!string.IsNullOrEmpty(SParams.GoodGroup)) ? (", " + DiagramAnalysisHelper.GetGroupName(SParams.GoodGroup)) : "");
			AsChart asChart = chtSales;
			asChart.ShowLegend = true;
			asChart.Legend.HorizontalPosition = HorizontalPosition.Left;
			asChart.Legend.VerticalPosition = VerticalPosition.BottomOutside;
			asChart.Type = SParams.DiagramKind;
			asChart.ShowAxisXLabels = true;
			asChart.AxisXTitle = DiagramAnalysisHelper.GetPeriodName(SParams.PeriodType);
			asChart.AxisYTitle = ArmSoft.SME.Common.Resources.Summa;
			asChart.DataSource = SalesAnalysisDiagram.GetChartRows(SParams.StartDate, SParams.EndDate, SParams.PeriodType, itemType, SParams.GoodGroup, storage, sumType);
			asChart.SetSeriesInfo("ChartDate", "Sum", "Code");
			asChart.VerticalAlignment = VerticalAlignment.Stretch;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public void LoadBestSellers(BestSellersDiagram rep = null)
	{
		try
		{
			txtCode.Text = ArmSoft.SME.Common.Resources.Code;
			txtCaption.Text = ArmSoft.SME.Common.Resources.Name;
			if (rep == null)
			{
				if (BSParams == null)
				{
					DialogValues values = Settings.DialogService.GetValues("BestSellersReport");
					bool storeValues = values.StoreValues;
					Dictionary<string, string> values2 = values.Values;
					if (storeValues && values2 != null && values2.Count > 0)
					{
						BSParams = new BestSellersParams
						{
							StartDate = values2["DATEBEGIN"].YYYYMMDDToDateOnly(),
							EndDate = values2["DATEEND"].YYYYMMDDToDateOnly(),
							ChartKind = (ChartType)Convert.ToInt16(values2["CMBDIAGRAMKIND"]),
							ChartSumType = (CommonConst.SalesSumType)Convert.ToInt16(values2["SUMTYPE"]),
							TopCount = Convert.ToInt32(values2["TOPCOUNT"]),
							ChartCompareMode = (CommonConst.BestSellersCompareMode)Convert.ToByte(values2["COMPARETYPE"])
						};
						if (values2.TryGetValue("GOODGROUP", out var value))
						{
							BSParams.GoodGroup = value;
						}
						else
						{
							BSParams.GoodGroup = "";
						}
					}
					else
					{
						BSParams = new BestSellersParams
						{
							StartDate = Settings.HostingEnvironment.DateBegin,
							EndDate = Settings.HostingEnvironment.DateEnd,
							ChartSumType = CommonConst.SalesSumType.SumNCC,
							TopCount = 5,
							ChartKind = ChartType.Bars2d,
							ChartCompareMode = CommonConst.BestSellersCompareMode.Quantity,
							GoodGroup = ""
						};
					}
				}
			}
			else
			{
				BSParams = new BestSellersParams
				{
					StartDate = rep.StartDate,
					EndDate = rep.EndDate,
					ChartKind = rep.ChartKind,
					ChartSumType = rep.SumType,
					TopCount = rep.TopCount,
					ChartCompareMode = rep.CompareMode,
					GoodGroup = rep.Group
				};
			}
			txtSumm.Text = DiagramAnalysisHelper.GetRepColName(BSParams.ChartCompareMode);
			txtBestSellersCaption.Text = ((BSParams.ChartCompareMode == CommonConst.BestSellersCompareMode.Quantity) ? "" : DiagramAnalysisHelper.SumTypeText(BSParams.ChartSumType));
			if (!string.IsNullOrEmpty(BSParams.GoodGroup))
			{
				if (txtBestSellersCaption.Text.Trim() == "")
				{
					txtBestSellersCaption.Text = DiagramAnalysisHelper.GetGroupName(BSParams.GoodGroup);
				}
				else
				{
					TextBlock textBlock = txtBestSellersCaption;
					textBlock.Text = textBlock.Text + ", " + DiagramAnalysisHelper.GetGroupName(BSParams.GoodGroup);
				}
			}
			BestSellersPanel.Caption = ArmSoft.SME.Common.Resources.BestSellers + " " + BSParams.StartDate.FormatDDMMYY() + " - " + BSParams.EndDate.FormatDDMMYY();
			BindingList<BestSellersDiagram.BestSellersChartRow> bestSellerRows = new BindingList<BestSellersDiagram.BestSellersChartRow>();
			BindingList<BestSellersDiagram.BestSellersChartRow> chartRows = BestSellersDiagram.GetChartRows(BSParams.StartDate, BSParams.EndDate, BSParams.ChartCompareMode, BSParams.TopCount, BSParams.GoodGroup, BSParams.ChartSumType, out bestSellerRows);
			BestSellersList.ItemsSource = bestSellerRows;
			AsChart asChart = chtBestsellers;
			asChart.Type = BSParams.ChartKind;
			asChart.ShowLegend = true;
			asChart.Legend.Template = (ControlTemplate)Application.Current.Resources["ChartLegendWrap"];
			asChart.Legend.HorizontalPosition = HorizontalPosition.Left;
			asChart.Legend.VerticalPosition = VerticalPosition.BottomOutside;
			asChart.ShowAxisXLabels = true;
			asChart.AxisXTitle = ArmSoft.SME.Trade.Common.Resources.Goods;
			asChart.AxisYTitle = DiagramAnalysisHelper.GetRepColName(BSParams.ChartCompareMode);
			asChart.DataSource = chartRows;
			if (BSParams.ChartKind != ChartType.Pie2D && BSParams.ChartKind != ChartType.Pie3D)
			{
				asChart.SetSeriesInfo("Caption", "Sum", "Code");
			}
			else
			{
				asChart.SetSeriesInfo("Code", "Sum", "ForPie");
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public void LoadCashBank(CashBankDiagram rep = null)
	{
		try
		{
			txtCashBankCaption.Text = ArmSoft.SME.Trade.Common.Resources.NCCName;
			if (rep == null)
			{
				if (CBParams == null)
				{
					DialogValues values = Settings.DialogService.GetValues("AnalysisCashBank");
					bool storeValues = values.StoreValues;
					Dictionary<string, string> values2 = values.Values;
					if (storeValues && values2 != null && values2.Count > 0)
					{
						CBParams = new CashBankParams
						{
							StartDate = values2["DATEBEGIN"].YYYYMMDDToDateOnly(),
							EndDate = values2["DATEEND"].YYYYMMDDToDateOnly(),
							PeriodType = (DatePeriodTypeSE)(SmartEnum<DatePeriodTypeSE, short>)Convert.ToInt16(values2["PERIODTYPE"]),
							DiagramKind = (ChartType)Convert.ToInt16(values2["CMBDIAGRAMKIND"])
						};
					}
					else
					{
						CBParams = new CashBankParams
						{
							StartDate = Settings.HostingEnvironment.DateBegin,
							EndDate = Settings.HostingEnvironment.DateEnd,
							PeriodType = DatePeriodTypeSE.Month,
							DiagramKind = ChartType.Lines
						};
					}
				}
				rep = new CashBankDiagram();
			}
			else
			{
				CBParams = new CashBankParams
				{
					StartDate = rep.StartDate,
					EndDate = rep.EndDate,
					PeriodType = rep.CalcPeriod,
					DiagramKind = rep.ChartKind
				};
			}
			CashBankPanel.Caption = ArmSoft.SME.Common.Resources.CashBankReport + " " + CBParams.StartDate.FormatDDMMYY() + " - " + CBParams.EndDate.FormatDDMMYY();
			AsChart asChart = chtCashBank;
			asChart.ShowLegend = true;
			asChart.Type = CBParams.DiagramKind;
			asChart.ShowAxisXLabels = true;
			asChart.AxisXTitle = DiagramAnalysisHelper.GetPeriodName(CBParams.PeriodType);
			asChart.AxisYTitle = ArmSoft.SME.Common.Resources.Summa;
			asChart.DataSource = CashBankDiagram.GetChartRows(CBParams.StartDate, CBParams.EndDate, CBParams.PeriodType);
			asChart.SetSeriesInfo("ChartDate", "Sum", "Code");
			asChart.Legend.HorizontalPosition = HorizontalPosition.Left;
			asChart.Legend.VerticalPosition = VerticalPosition.BottomOutside;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public void LoadDebtorsCreditors(DebitorsCreditorsDiagram rep = null)
	{
		try
		{
			if (rep == null)
			{
				if (DBParams == null)
				{
					DialogValues values = Settings.DialogService.GetValues("AnalysisDebitorsCreditors");
					bool storeValues = values.StoreValues;
					Dictionary<string, string> values2 = values.Values;
					if (storeValues && values2 != null && values2.Count > 0)
					{
						DBParams = new DebtorsParams
						{
							StartDate = values2["DATEBEGIN"].YYYYMMDDToDateOnly(),
							EndDate = values2["DATEEND"].YYYYMMDDToDateOnly(),
							PeriodType = (DatePeriodTypeSE)(SmartEnum<DatePeriodTypeSE, short>)Convert.ToInt16(values2["PERIODTYPE"]),
							DiagramKind = (ChartType)Convert.ToInt16(values2["CMBDIAGRAMKIND"])
						};
					}
					else
					{
						DBParams = new DebtorsParams
						{
							StartDate = Settings.HostingEnvironment.DateBegin,
							EndDate = Settings.HostingEnvironment.DateEnd,
							PeriodType = DatePeriodTypeSE.Month,
							DiagramKind = ChartType.Lines
						};
					}
				}
				rep = new DebitorsCreditorsDiagram();
			}
			else
			{
				DBParams = new DebtorsParams
				{
					StartDate = rep.StartDate,
					EndDate = rep.EndDate,
					PeriodType = rep.CalcPeriod,
					DiagramKind = rep.ChartKind
				};
			}
			DebtorsPanel.Caption = ArmSoft.SME.Common.Resources.DbCrRems + " " + DBParams.StartDate.FormatDDMMYY() + " - " + DBParams.EndDate.FormatDDMMYY();
			txtDbCrCaption.Text = ArmSoft.SME.Trade.Common.Resources.NCCName;
			Mouse.OverrideCursor = null;
			AsChart asChart = chtDebtors;
			asChart.ShowLegend = true;
			asChart.Type = DBParams.DiagramKind;
			asChart.ShowAxisXLabels = true;
			asChart.AxisXTitle = DiagramAnalysisHelper.GetPeriodName(DBParams.PeriodType);
			asChart.AxisYTitle = ArmSoft.SME.Common.Resources.Summa;
			asChart.DataSource = DebitorsCreditorsDiagram.GetChartRows(DBParams.StartDate, DBParams.EndDate, DBParams.PeriodType);
			asChart.SetSeriesInfo("ChartDate", "Sum", "Code");
			asChart.Legend.HorizontalPosition = HorizontalPosition.Left;
			asChart.Legend.VerticalPosition = VerticalPosition.BottomOutside;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private BestSellersDiagram InitBestSellersReport()
	{
		return new BestSellersDiagram(showDialogOnNew: false)
		{
			StartDate = BSParams.StartDate,
			EndDate = BSParams.EndDate,
			ChartKind = BSParams.ChartKind,
			CompareMode = BSParams.ChartCompareMode,
			TopCount = BSParams.TopCount,
			SumType = BSParams.ChartSumType,
			Group = BSParams.GoodGroup
		};
	}

	private CashBankDiagram InitCashBankReport()
	{
		return new CashBankDiagram(showDialogOnNew: false)
		{
			StartDate = CBParams.StartDate,
			EndDate = CBParams.EndDate,
			ChartKind = CBParams.DiagramKind,
			CalcPeriod = CBParams.PeriodType
		};
	}

	private DebitorsCreditorsDiagram InitDbCrReport()
	{
		return new DebitorsCreditorsDiagram(showDialogOnNew: false)
		{
			StartDate = DBParams.StartDate,
			EndDate = DBParams.EndDate,
			ChartKind = DBParams.DiagramKind,
			CalcPeriod = DBParams.PeriodType
		};
	}

	private SalesAnalysisDiagram InitsalesReport()
	{
		return new SalesAnalysisDiagram(showDialogOnNew: false)
		{
			StartDate = SParams.StartDate,
			EndDate = SParams.EndDate,
			SumType = SParams.SumType,
			ChartKind = SParams.DiagramKind,
			CalcPeriod = SParams.PeriodType,
			Group = SParams.GoodGroup
		};
	}

	private void ShowBSReportDialog(object sender, RoutedEventArgs e)
	{
		BestSellersDiagram bestSellersDiagram = InitBestSellersReport();
		if (bestSellersDiagram.ShowDialog(useParamValues: true, isRefreshMode: true))
		{
			LoadBestSellers(bestSellersDiagram);
		}
	}

	private void ShowCBReportDialog(object sender, RoutedEventArgs e)
	{
		CashBankDiagram cashBankDiagram = InitCashBankReport();
		if (cashBankDiagram.ShowDialog(useParamValues: true, isRefreshMode: true))
		{
			LoadCashBank(cashBankDiagram);
		}
	}

	private void ShowDbReportDialog(object sender, RoutedEventArgs e)
	{
		DebitorsCreditorsDiagram debitorsCreditorsDiagram = InitDbCrReport();
		if (debitorsCreditorsDiagram.ShowDialog(useParamValues: true, isRefreshMode: true))
		{
			LoadDebtorsCreditors(debitorsCreditorsDiagram);
		}
	}

	private void ShowSalesReportDialog(object sender, RoutedEventArgs e)
	{
		SalesAnalysisDiagram salesAnalysisDiagram = InitsalesReport();
		if (salesAnalysisDiagram.ShowDialog(useParamValues: true, isRefreshMode: true))
		{
			LoadSales(salesAnalysisDiagram);
		}
	}

	private void DashBoardWindow_ContextMenuOpening(object sender, ContextMenuEventArgs e)
	{
		ContextMenuManager.ShowElementContextMenu(this);
	}

	private void HandleButtonClick(object sender, ItemClickEventArgs e)
	{
		if (sender == bRefreshDataView)
		{
			if (BestSellersPanel.IsActive)
			{
				LoadBestSellers();
			}
			else if (SalesPanel.IsActive)
			{
				LoadSales();
			}
			else if (CashBankPanel.IsActive)
			{
				LoadCashBank();
			}
			else if (DebtorsPanel.IsActive)
			{
				LoadDebtorsCreditors();
			}
		}
		else if (sender == bRefreshAll)
		{
			InitRefresh();
		}
		else if (sender == bShowDataViewDialog)
		{
			if (BestSellersPanel.IsActive)
			{
				ShowBSReportDialog(null, null);
			}
			else if (SalesPanel.IsActive)
			{
				ShowSalesReportDialog(null, null);
			}
			else if (CashBankPanel.IsActive)
			{
				ShowCBReportDialog(null, null);
			}
			else if (DebtorsPanel.IsActive)
			{
				ShowDbReportDialog(null, null);
			}
		}
		else if (sender == bDetails)
		{
			AsReport asReport = null;
			if (BestSellersPanel.IsActive)
			{
				asReport = InitBestSellersReport();
			}
			else if (SalesPanel.IsActive)
			{
				asReport = InitsalesReport();
			}
			else if (CashBankPanel.IsActive)
			{
				asReport = InitCashBankReport();
			}
			else if (DebtorsPanel.IsActive)
			{
				asReport = InitDbCrReport();
			}
			asReport?.Show();
		}
	}

	private void InitPanelsDct()
	{
		DockPosition value = new DockPosition
		{
			Group = LeftGroup,
			DockType = DockType.Top
		};
		dctPanels.Add(BestSellersPanel, value);
		value = new DockPosition
		{
			Group = LeftGroup,
			DockType = DockType.Bottom
		};
		dctPanels.Add(CashBankPanel, value);
		value = new DockPosition
		{
			Group = RightGroup,
			DockType = DockType.Top
		};
		dctPanels.Add(SalesPanel, value);
		value = new DockPosition
		{
			Group = RightGroup,
			DockType = DockType.Bottom
		};
		dctPanels.Add(DebtorsPanel, value);
	}

	private void MaximizePanel(object sender, RoutedEventArgs e)
	{
		foreach (LayoutPanel key in dctPanels.Keys)
		{
			DockPosition dockPosition = dctPanels[key];
			if (key.IsActive && key.IsAutoHidden)
			{
				dockManager.DockController.Restore(key);
				dockManager.DockController.Dock(key, dockPosition.Group, dockPosition.DockType);
			}
		}
		foreach (LayoutPanel key2 in dctPanels.Keys)
		{
			if (!key2.IsActive && !key2.IsAutoHidden)
			{
				dockManager.DockController.Hide(key2);
			}
		}
	}

	private void RestorePanel(object sender, RoutedEventArgs e)
	{
		foreach (LayoutPanel key in dctPanels.Keys)
		{
			DockPosition dockPosition = dctPanels[key];
			if (key.IsAutoHidden)
			{
				dockManager.DockController.Dock(key, dockPosition.Group, dockPosition.DockType);
			}
		}
	}

	private void HidePanel(object sender, RoutedEventArgs e)
	{
		AutoHideGroup autoHideGroup = new AutoHideGroup
		{
			DockType = Dock.Top
		};
		foreach (LayoutPanel key in dctPanels.Keys)
		{
			if (key.IsActive && !key.IsAutoHidden)
			{
				autoHideGroup.Add(key);
				dockManager.AutoHideGroups.Add(autoHideGroup);
			}
		}
	}

	private void UnHidePanel(object sender, RoutedEventArgs e)
	{
		foreach (LayoutPanel key in dctPanels.Keys)
		{
			DockPosition dockPosition = dctPanels[key];
			if (key.IsActive && key.IsAutoHidden)
			{
				dockManager.DockController.Dock(key, dockPosition.Group, dockPosition.DockType);
			}
		}
	}

	private void BarButtonHelp_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			string helpId = "Trade_Reports_Analysis_ManagementDashboard";
			if (BestSellersPanel.IsActive)
			{
				helpId = InitBestSellersReport().HelpId;
			}
			else if (SalesPanel.IsActive)
			{
				helpId = InitsalesReport().HelpId;
			}
			else if (CashBankPanel.IsActive)
			{
				helpId = InitCashBankReport().HelpId;
			}
			else if (DebtorsPanel.IsActive)
			{
				helpId = InitDbCrReport().HelpId;
			}
			Settings.HostingEnvironment.ShowHelp(helpId);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		e.Handled = true;
	}
}
