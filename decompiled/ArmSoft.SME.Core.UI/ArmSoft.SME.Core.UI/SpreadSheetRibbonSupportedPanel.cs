using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpf.Spreadsheet;

namespace ArmSoft.SME.Core.UI;

public class SpreadSheetRibbonSupportedPanel : RibbonSupportedPanelBase
{
	protected SpreadsheetFormulaBarControl _formulaBar;

	protected SpreadsheetControl _mainDocument;

	public override MergingType BarItemMergingType => MergingType.WithName;

	public SpreadSheetRibbonSupportedPanel()
		: base(allowHomePanel: false)
	{
	}

	protected override void BuildBarManagerItems()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		base.BuildBarManagerItems();
		ExportDocumentWordBarButtonItem = AddBarButtonItem("bExportDocumentWord", ArmSoft.SME.Common.Resources.ExportToWord, ImageService.GetImage("doc_export_word_16.svg"), ImageService.GetImage("doc_export_word_32.svg"), new KeyGesture((Key)96, (ModifierKeys)2));
		ExportDocumentPDFBarButtonItem = AddBarButtonItem("bExportDocumentPDF", ArmSoft.SME.Common.Resources.ExportToPDF, ImageService.GetImage("doc_export_pdf_16.svg"), ImageService.GetImage("doc_export_pdf_32.svg"));
		_bExportPDF = AddBarButtonItem("bExportPDF", ArmSoft.SME.Common.Resources.ExportToPDF, ImageService.GetImage("table_export_pdf_16.svg"), ImageService.GetImage("table_export_pdf_32.svg"));
		_bExportDocumentXML = AddBarButtonItem("bExportDocumentXML", ArmSoft.SME.Common.Resources.ExportToXML, ImageService.GetImage("doc_export_xml_16.svg"), ImageService.GetImage("doc_export_xml_32.svg"));
		_chkReportDetails = AddBarCheckItem("chkReportDetails", ArmSoft.SME.Common.Resources.ReportDetails, ImageService.GetImage("table_show_details_16.svg"), ImageService.GetImage("table_show_details_32.svg"));
		_statusBarCurrentRow = AddBarStaticItem("statusBarCurrentRow", "", null, null);
		_statusBarCurrentRow.ItemMinWidth = 150.0;
		_statusBarCurrentRow.AutoSizeMode = BarItemAutoSizeMode.Fill;
		_statusBarCurrentRow.ContentAlignment = HorizontalAlignment.Center;
		_statusBarCurrentRow.Focusable = false;
	}

	protected override void BuildBarManagerUI()
	{
		base.BuildBarManagerUI();
		DockPanel dockPanel = new DockPanel
		{
			LastChildFill = true,
			UseLayoutRounding = true
		};
		BarManager.Child = dockPanel;
		BarContainerControl element = new BarContainerControl
		{
			Focusable = false,
			Orientation = Orientation.Vertical,
			UseLayoutRounding = true
		};
		DockPanel.SetDock(element, Dock.Left);
		dockPanel.Children.Add(element);
		BarContainerControl element2 = new BarContainerControl
		{
			Focusable = false
		};
		DockPanel.SetDock(element2, Dock.Bottom);
		dockPanel.Children.Add(element2);
		RibbonStatusBar = new RibbonStatusBarControl
		{
			IsSizeGripVisible = false,
			Focusable = false,
			Visibility = Visibility.Visible
		};
		RibbonStatusBar.LeftItemLinks.Add(new BarStaticItemLink
		{
			BarItemName = "statusBarCurrentRow",
			MergeType = BarItemMergeType.Remove
		});
		DockPanel.SetDock(RibbonStatusBar, Dock.Bottom);
		dockPanel.Children.Add(RibbonStatusBar);
		_formulaBar = new SpreadsheetFormulaBarControl
		{
			Margin = new Thickness(-1.0),
			Padding = new Thickness(0.0),
			IsExpanded = false
		};
		DockPanel.SetDock(_formulaBar, Dock.Top);
		dockPanel.Children.Add(_formulaBar);
		_mainDocument = new SpreadsheetControl
		{
			CommandBarStyle = CommandBarStyle.Ribbon,
			ReadOnly = false,
			BarManager = BarManager
		};
		_formulaBar.SpreadsheetControl = _mainDocument;
		dockPanel.Children.Add(_mainDocument);
	}

	protected RibbonPage CreateMainPage()
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		RibbonPage ribbonPage = new RibbonPage
		{
			Name = "RibbonPageHome",
			Caption = ArmSoft.SME.Common.Resources.Home,
			MergeType = RibbonMergeType.MergeItems,
			CaptionTemplate = (DataTemplate)Settings.GenericResources["RibbonPageHeaderTemplate"],
			IsSelected = true
		};
		RibbonPageGroupRecords = RibbonSupportedPanelBase.CreateRibbonPageGroup(ribbonPage, "RibbonPageGroupRecords", ArmSoft.SME.Common.Resources.Records, ImageService.GetImage("add_small_32.svg"));
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupRecords, "bDataViewAdd", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupRecords, "bDataViewEdit", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupRecords, "bDataViewDelete", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupRecords, "bDataViewCreateCopy", RibbonItemStyles.SmallWithText);
		RibbonPageGroup ribbonPageGroup = RibbonSupportedPanelBase.CreateRibbonPageGroup(ribbonPage, "RibbonPageGroupDataView", ArmSoft.SME.Common.Resources.DataView, ImageService.GetImage("table_16.svg"));
		ribbonPageGroup.AllowCollapse = false;
		ribbonPageGroup.MergeType = RibbonMergeType.Replace;
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup, "bShowDataViewDialog", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup, "bRefreshDataView", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddCheckItemLink(ribbonPageGroup, "chkReportDetails", RibbonItemStyles.SmallWithText);
		RibbonPageGroup ribbonPageGroup2 = RibbonSupportedPanelBase.CreateRibbonPageGroup(ribbonPage, "RibbonPagePrint", ArmSoft.SME.Common.Resources.Print, ImageService.GetImage("print_16.svg"));
		ribbonPageGroup2.AllowCollapse = false;
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup2, "bPrintPreview", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup2, "bPrint", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup2, "bPrintOptions", RibbonItemStyles.SmallWithText);
		RibbonPageGroupDocument = RibbonSupportedPanelBase.CreateRibbonPageGroup(ribbonPage, "RibbonPageGroupDocument", ArmSoft.SME.Common.Resources.Documents, ImageService.GetImage("print_16.svg"));
		RibbonPageGroupDocument.AllowCollapse = false;
		RibbonPageGroupDocument.MergeType = RibbonMergeType.Replace;
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupDocument, "bDataViewDocument", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupDocument, "bExportDocumentExcel", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupDocument, "bDocumentHistory", RibbonItemStyles.SmallWithText);
		RibbonPageGroup ribbonPageGroup3 = RibbonSupportedPanelBase.CreateRibbonPageGroup(ribbonPage, "RibbonPageGroupExport", ArmSoft.SME.Common.Resources.Export, null);
		ribbonPageGroup3.AllowCollapse = false;
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup3, "bExportDocumentWord");
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup3, "bExportDocumentExcel");
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup3, "bExportDocumentXML");
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup3, "bExportDocumentPDF");
		RibbonSupportedPanelBase.AddSubLink(ribbonPageGroup3, "bExportToExcel");
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup3, "bExportPDF");
		return ribbonPage;
	}

	public static RibbonControl FindRibbon(DependencyObject el)
	{
		if (VisualTreeHelper.GetChildrenCount(el) == 0)
		{
			return null;
		}
		for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(el); i++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(el, i);
			if (child is RibbonControl result)
			{
				return result;
			}
			RibbonControl ribbonControl = FindRibbon(child);
			if (ribbonControl != null)
			{
				return ribbonControl;
			}
		}
		return null;
	}
}
