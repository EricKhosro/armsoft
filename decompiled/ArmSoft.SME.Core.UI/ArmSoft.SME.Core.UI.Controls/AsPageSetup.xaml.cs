using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Report;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class AsPageSetup : UserControl, IComponentConnector
{
	public PrintInfo PI { get; set; }

	public object ActivePanel { get; set; }

	public string ReportKind { get; set; } = string.Empty;

	public AsPageSetup()
	{
		InitializeComponent();
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		Style style = (Style)Settings.GenericResources["PopupCalcEditStyle"];
		txtMarginsLeft.Style = style;
		txtMarginsRight.Style = style;
		txtMarginsTop.Style = style;
		txtMarginsBottom.Style = style;
		gbPaperOrientation.Header = ArmSoft.SME.Common.Resources.PaperOrientation;
		txtPortrait.Text = ArmSoft.SME.Common.Resources.PaperPortrait;
		txtLandscape.Text = ArmSoft.SME.Common.Resources.PaperLandscape;
		gbMargins.Header = ArmSoft.SME.Common.Resources.PaperMargins;
		txtMarginsLeft.Mask = DecimalExtension.GetCurrencyFormat(2, 1);
		txtMarginsLeft.MaskUseAsDisplayFormat = true;
		MarginsLeft.Label = ArmSoft.SME.Common.Resources.MarginLeft;
		txtMarginsRight.Mask = DecimalExtension.GetCurrencyFormat(2, 1);
		txtMarginsRight.MaskUseAsDisplayFormat = true;
		MarginsRight.Label = ArmSoft.SME.Common.Resources.MarginRight;
		MarginsTop.Label = ArmSoft.SME.Common.Resources.MarginTop;
		txtMarginsTop.Mask = DecimalExtension.GetCurrencyFormat(2, 1);
		txtMarginsTop.MaskUseAsDisplayFormat = true;
		MarginsBottom.Label = ArmSoft.SME.Common.Resources.MarginBottom;
		txtMarginsBottom.Mask = DecimalExtension.GetCurrencyFormat(2, 1);
		txtMarginsBottom.MaskUseAsDisplayFormat = true;
		CompanyName.Content = ArmSoft.SME.Common.Resources.ShowCompanyName;
		AsInfo.Content = ArmSoft.SME.Common.Resources.ShowAsInfoInPrint;
		ShowLines.Content = ArmSoft.SME.Common.Resources.ShowLinesInPrintForms;
		gbPrintTitle.Header = ArmSoft.SME.Common.Resources.OptionPrintHeader;
		PrintTitleAlways.Content = ArmSoft.SME.Common.Resources.PrintTitleAlways;
		PrintTitleIfOpen.Content = ArmSoft.SME.Common.Resources.PrintTitleIfOpen;
		PrintTitleDoNotPrint.Content = ArmSoft.SME.Common.Resources.PrintTitleDoNotPrint;
		PrintDate.Content = ArmSoft.SME.Common.Resources.OptionPrintDate;
		FitToPage.Content = ArmSoft.SME.Common.Resources.FitToPage;
		PrintFilter.Content = ArmSoft.SME.Common.Resources.PrintFilterCondition;
		BreakPages.Content = ArmSoft.SME.Common.Resources.PrintEnablePageBreaks;
		PrintPageNumbers.Content = ArmSoft.SME.Common.Resources.OptionPrintPageNumbers;
		Scale.Label = ArmSoft.SME.Common.Resources.ScalePercent;
	}

	public void FillSettings()
	{
		bool flag = ActivePanel is DataViewPanel;
		bool flag2 = ActivePanel is AsReportViewer;
		bool flag3 = ActivePanel is AsSpreadSheet;
		PI = new PrintInfo(ReportKind);
		if (flag3)
		{
			Scale.Visibility = Visibility.Visible;
			FitToPage.Visibility = Visibility.Collapsed;
			PrintTitleAlways.IsEnabled = false;
			PrintTitleAlways.IsChecked = false;
		}
		else
		{
			Scale.Visibility = Visibility.Collapsed;
			FitToPage.Visibility = Visibility.Visible;
			PrintTitleAlways.IsEnabled = true;
		}
		FitToPage.IsChecked = PI.FitToPage;
		gbPrintTitle.Visibility = Visibility.Visible;
		txtMarginsLeft.EditValue = Convert.ToDouble(PI.MarginLeft);
		txtMarginsRight.EditValue = Convert.ToDouble(PI.MarginRight);
		txtMarginsTop.EditValue = Convert.ToDouble(PI.MarginTop);
		txtMarginsBottom.EditValue = Convert.ToDouble(PI.MarginBottom);
		BreakPages.IsChecked = Convert.ToBoolean(PI.EnablePageBreaks);
		BreakPages.IsEnabled = !flag;
		string printHeader = PI.PrintHeader;
		PrintFilter.IsChecked = PI.PrintFilterCondition;
		PrintTitleAlways.IsChecked = printHeader == "Always";
		PrintTitleDoNotPrint.IsChecked = printHeader == "DoNotPrint";
		PrintTitleIfOpen.IsChecked = printHeader == "PrintIfOpen";
		if (flag2)
		{
			PrintFilter.Visibility = Visibility.Collapsed;
		}
		PrintTitleDoNotPrint.IsEnabled = true;
		PrintTitleIfOpen.IsEnabled = true;
		PrintDate.IsChecked = PI.PrintDate;
		PrintPageNumbers.IsChecked = PI.PrintPageNumbers;
		chkPortrait.IsChecked = PI.PaperOrientation == "Portrait";
		chkLandscape.IsChecked = PI.PaperOrientation != "Portrait";
		AsInfo.Visibility = ((!string.IsNullOrWhiteSpace(ReportKind)) ? Visibility.Collapsed : Visibility.Visible);
		ShowLines.Visibility = Visibility.Visible;
		AsInfo.IsChecked = PI.PrintASInfo;
		ShowLines.IsChecked = PI.ShowLines;
		CompanyName.IsChecked = PI.PrintCompanyName;
		txtScale.EditValue = PI.ScalePercent;
	}

	public void SaveOptions()
	{
		if (Convert.ToBoolean(chkPortrait.IsChecked))
		{
			PI.PaperOrientation = "Portrait";
		}
		else
		{
			PI.PaperOrientation = "Landscape";
		}
		PI.MarginLeft = Convert.ToSingle(txtMarginsLeft.EditValue);
		PI.MarginRight = Convert.ToSingle(txtMarginsRight.EditValue);
		PI.MarginTop = Convert.ToSingle(txtMarginsTop.EditValue);
		PI.MarginBottom = Convert.ToSingle(txtMarginsBottom.EditValue);
		if (Convert.ToBoolean(PrintTitleAlways.IsChecked))
		{
			PI.PrintHeader = "Always";
		}
		if (Convert.ToBoolean(PrintTitleDoNotPrint.IsChecked))
		{
			PI.PrintHeader = "DoNotPrint";
		}
		if (Convert.ToBoolean(PrintTitleIfOpen.IsChecked))
		{
			PI.PrintHeader = "PrintIfOpen";
		}
		PI.PrintDate = Convert.ToBoolean(PrintDate.IsChecked);
		PI.PrintPageNumbers = Convert.ToBoolean(PrintPageNumbers.IsChecked);
		PI.FitToPage = Convert.ToBoolean(FitToPage.IsChecked);
		PI.EnablePageBreaks = Convert.ToBoolean(BreakPages.IsChecked);
		PI.PrintFilterCondition = Convert.ToBoolean(PrintFilter.IsChecked);
		PI.PrintASInfo = Convert.ToBoolean(AsInfo.IsChecked);
		PI.ShowLines = Convert.ToBoolean(ShowLines.IsChecked);
		PI.PrintCompanyName = Convert.ToBoolean(CompanyName.IsChecked);
		int scalePercent = Convert.ToInt32(txtScale.EditValue);
		PI.ScalePercent = scalePercent;
		PI.StoreSettings();
	}
}
