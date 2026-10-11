using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.Documents.Goods;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.Navigation;
using ArmSoft.SME.Trade.UI.OtherWindows;
using ArmSoft.SME.Trade.UI.ReportViewers.Cash_Bank;
using ArmSoft.SME.Trade.UI.SmartEnums;
using DevExpress.Xpf.Core.MvvmSample;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Navigation;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.View;

public partial class PointOfSaleView : ModuleView, IComponentConnector
{
	private bool _firstActivated = true;

	private CashierDisplayInfo _cashierDisplayInfoInstance = CashierDisplayInfo.GetInstance();

	public PointOfSaleView()
	{
		InitializeComponent();
		PointOfSalePageTitle.Content = ArmSoft.SME.Trade.Common.Resources.PointOfSalePOS;
		if (!Settings.LoginService.IsAdmin && Settings.UserAccessService.GetAccessibleSubsystems().Count == 1)
		{
			PointOfSalePageTitle.Glyph = null;
		}
		PointOfSalePageTitle.VerticalAlignment = VerticalAlignment.Center;
		PointOfSalePageTitle.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
		PointOfSalePageTitle.FontSize = 17.0;
		Background = Settings.GetBackgroundBrush();
		foreach (TileBarItem item in BaseView.SystemsUIList(CommonConst.SubSystemTypes.PointOfSaleSS))
		{
			TileView.TileBar.Items.Add(item);
		}
		Loaded += new SafeEventHandler<EventArgs>(PointOfSaleView_Initialized).Handler;
		CloseCashierShiftTile.Click += new SafeEventHandler<EventArgs>(CloseCashierShiftTile_Click).Handler;
		SaleTile.Click += new SafeEventHandler<EventArgs>(SaleTile_Click).Handler;
		ReportsTile.Click += new SafeEventHandler<EventArgs>(ReportsTile_Click).Handler;
		PrintECRReportShiftTile.Click += new SafeEventHandler<EventArgs>(PrintECRReportShiftTile_Click).Handler;
		ECRSetupTile.Click += new SafeEventHandler<EventArgs>(ECRSetupTile_Click).Handler;
		WatchVideoTile.Click += new SafeEventHandler<EventArgs>(WatchVideoTile_Click).Handler;
		ASFontHelper.SetFontSettings(this);
	}

	private void PointOfSaleView_Initialized(object sender, EventArgs e)
	{
		if (_firstActivated)
		{
			if (Settings.UserAccessService.GetNavigatorNodePermission(7, UserAccessIdSE.Sale).AllowView || (Settings.IsOffline && Settings.UserAccessService.IsSubsystemAccessible(2) && Settings.UserAccessService.GetNavigatorNodePermission(2, UserAccessIdSE.MTBill).AllowView))
			{
				SaleTile.Visibility = Visibility.Visible;
			}
			else
			{
				SaleTile.Visibility = Visibility.Collapsed;
			}
			SaleTile.ToolTip = ArmSoft.SME.Trade.Common.Resources.Sale;
			SaleTile_TextBlock.Text = ArmSoft.SME.Trade.Common.Resources.Sale.ToUpper();
			SaleTile.HeaderTemplate = (DataTemplate)Resources["TileHeaderTemplate"];
			if (Settings.UserAccessService.GetNavigatorNodePermission(7, UserAccessIdSE.PrintECRReportPOS).AllowView)
			{
				PrintECRReportShiftTile.Visibility = Visibility.Visible;
			}
			else
			{
				PrintECRReportShiftTile.Visibility = Visibility.Collapsed;
			}
			PrintECRReportShiftTile_TextBlock.Text = ArmSoft.SME.Common.Resources.PrintECRXZReport.ToUpper();
			PrintECRReportShiftTile.ToolTip = ArmSoft.SME.Common.Resources.PrintECRXZReport;
			PrintECRReportShiftTile.HeaderTemplate = (DataTemplate)Resources["TileHeaderTemplate"];
			if (Settings.UserAccessService.GetNavigatorNodePermission(7, UserAccessIdSE.CloseCashierShift).AllowView)
			{
				CloseCashierShiftTile.Visibility = Visibility.Visible;
			}
			else
			{
				CloseCashierShiftTile.Visibility = Visibility.Collapsed;
			}
			CloseCashierShiftTile_TextBlock.Text = ArmSoft.SME.Trade.Common.Resources.CashierShiftClose.ToUpper();
			CloseCashierShiftTile.ToolTip = ArmSoft.SME.Trade.Common.Resources.CashierShiftClose;
			CloseCashierShiftTile.HeaderTemplate = (DataTemplate)Resources["TileHeaderTemplate"];
			if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.CashierShiftReport).AllowView)
			{
				ReportsTile.Visibility = Visibility.Visible;
			}
			else
			{
				ReportsTile.Visibility = Visibility.Collapsed;
			}
			ReportsTile_TextBlock.Text = ArmSoft.SME.Common.Resources.Reports.ToUpper();
			ReportsTile.ToolTip = ArmSoft.SME.Common.Resources.Reports;
			ReportsTile.HeaderTemplate = (DataTemplate)Resources["TileHeaderTemplate"];
			if (Settings.UserAccessService.GetNavigatorNodePermission(7, UserAccessIdSE.ECRConfiguration.Value).AllowView)
			{
				ECRSetupTile.Visibility = Visibility.Visible;
			}
			else
			{
				ECRSetupTile.Visibility = Visibility.Collapsed;
			}
			ECRSetupTile_TextBlock.Text = ArmSoft.SME.Trade.Common.Resources.ECRConfiguration.ToUpper();
			ECRSetupTile.ToolTip = ArmSoft.SME.Trade.Common.Resources.ECRConfiguration;
			ECRSetupTile.HeaderTemplate = (DataTemplate)Resources["TileHeaderTemplate"];
			WatchVideoTile.ToolTip = ArmSoft.SME.Trade.Common.Resources.WatchPOSVideo;
			WatchVideoTile_TextBlock.Text = ArmSoft.SME.Trade.Common.Resources.WatchPOSVideo;
			WatchVideoTile.HeaderTemplate = (DataTemplate)Resources["TileHeaderTemplate"];
			WatchVideoTile.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6f6f6f");
			WatchVideoTile.FontWeight = FontWeights.Bold;
			_firstActivated = false;
		}
	}

	private void SaleTile_Click(object sender, EventArgs e)
	{
		try
		{
			bool flag = CashierShiftHelper.ExistsOpenedCashierShift() || Settings.IsOffline || (CashierShiftHelper.OpenCashierShift() ? true : false);
			if (Settings.IsOffline && SaleWindowHelper.IsDefaultStorageRetail())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.StorageMustBeWholesale);
			}
			if (flag)
			{
				if (Settings.ParametersService.AutoSave() && (TradeDocument)Settings.LocalDBDocumentService.GetLastAutoSavedDocument() != null)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.HappendEmergencyStopOfApplication + "\n" + ArmSoft.SME.Trade.Common.Resources.FoundAutoSavedSale, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
				if (_cashierDisplayInfoInstance.TouchMode == POSScreenModesSE.SampleScreen.Value || _cashierDisplayInfoInstance.TouchMode == POSScreenModesSE.SampleScreenWithFavorites.Value)
				{
					new SamplePOSSaleWindow(Settings.HostingEnvironment, _cashierDisplayInfoInstance.TouchMode, _cashierDisplayInfoInstance.FullScreen, _cashierDisplayInfoInstance.Width, _cashierDisplayInfoInstance.Height).ShowDialog();
				}
				else
				{
					new SaleWindow(Settings.HostingEnvironment, _cashierDisplayInfoInstance.TouchMode, _cashierDisplayInfoInstance.FullScreen, _cashierDisplayInfoInstance.Width, _cashierDisplayInfoInstance.Height).ShowDialog();
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CloseCashierShiftTile_Click(object sender, EventArgs e)
	{
		try
		{
			CashierShiftHelper.CloseCashierShift();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ReportsTile_Click(object sender, EventArgs e)
	{
		CashierShiftReportViewer cashierShiftReportViewer = new CashierShiftReportViewer();
		cashierShiftReportViewer.AllowOperation = Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.CashierShiftReport).AllowOperation;
		cashierShiftReportViewer.Show();
	}

	private void ECRSetupTile_Click(object sender, EventArgs e)
	{
		NavigationBarsController.OpenECRConfiguration();
	}

	private void PrintECRReportShiftTile_Click(object sender, EventArgs e)
	{
		try
		{
			new ECRPrintReportWindow().ShowDialog();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void WatchVideoTile_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "https://www.youtube.com/watch?v=VvnGy4MTuk4",
				UseShellExecute = true
			});
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void WatchVideoTile_MouseEnter(object sender, MouseEventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = (Tile)sender;
		SolidColorBrush foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#439467"));
		LinearGradientBrush linearGradientBrush = new LinearGradientBrush
		{
			StartPoint = new Point(0.5, 0.0),
			EndPoint = new Point(0.5, 1.0)
		};
		linearGradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#F38020"), 0.0));
		linearGradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFFFFF"), 1.3));
		if (tile.Name == "WatchVideoTile")
		{
			WatchVideoTile_Border.Background = linearGradientBrush;
			WatchVideoTile_TextBlock.Foreground = foreground;
		}
	}

	private void WatchVideoTile_MouseLeave(object sender, MouseEventArgs e)
	{
		Tile tile = (Tile)sender;
		SolidColorBrush background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#83C887"));
		SolidColorBrush foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#83C887"));
		if (tile.Name == "WatchVideoTile")
		{
			WatchVideoTile_Border.Background = background;
			WatchVideoTile_TextBlock.Foreground = foreground;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
