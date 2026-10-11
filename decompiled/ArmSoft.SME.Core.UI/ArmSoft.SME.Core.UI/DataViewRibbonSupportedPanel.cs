using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Ribbon;

namespace ArmSoft.SME.Core.UI;

public class DataViewRibbonSupportedPanel : RibbonSupportedPanelBase
{
	protected BarButtonItem _bExportXlsXLargeData;

	protected BarButtonItem _cmdPaste;

	protected BarButtonItem _cmdExpandGroups;

	protected BarSubItem _cmdExpandGroupLevel;

	protected BarButtonItem _cmdCollapseGroups;

	protected BarCheckItem _chkShowGroupSummaryEditor;

	protected BarButtonItem _btnGroupColumns;

	protected BarButtonItem _cmdSelectByMask;

	protected BarButtonItem _cmdUnSelectByMask;

	protected BarCheckItem _cmdCardView;

	protected BarCheckItem _cmdTreeView;

	protected BarButtonItem _cmdFindData;

	protected BarButtonItem _cmdFilter;

	protected BarButtonItem _cmdClearGroup;

	protected BarButtonItem _cmdFilterCurrent;

	protected BarButtonItem _cmdFilterNotEqualCurrent;

	protected BarButtonItem _cmdCopyCurrent;

	protected BarButtonItem _btnFilterSelectedData;

	protected BarButtonItem _cmdCopyFilter;

	protected BarButtonItem _btnClearFilter;

	protected BarButtonItem _cmdColumnChooser;

	protected BarButtonItem _cmdAddUnboundColumns;

	protected BarButtonItem _bLayoutSave;

	protected BarButtonItem _bLayoutEdit;

	protected BarButtonItem _bLayoutLoad;

	protected BarButtonItem _bLayoutDefault;

	protected BarSubItem _bGridLayout;

	private readonly Dictionary<string, bool> _barButtonStates = new Dictionary<string, bool>();

	public override MergingType BarItemMergingType => MergingType.WithName;

	public Grid LoadingDecoratorGrid { get; private set; }

	public GridControl Grid { get; private set; }

	protected PopupMenu GridContextMenu { get; set; }

	protected LayoutPanel TotalsPanelRight { get; private set; }

	protected DocumentGroup ChartGroup { get; private set; }

	public DataViewRibbonSupportedPanel()
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
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Unknown result type (might be due to invalid IL or missing references)
		//IL_0774: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0800: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Unknown result type (might be due to invalid IL or missing references)
		//IL_0816: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0843: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Unknown result type (might be due to invalid IL or missing references)
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08df: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_090c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0912: Unknown result type (might be due to invalid IL or missing references)
		//IL_0922: Unknown result type (might be due to invalid IL or missing references)
		//IL_0928: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0955: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Unknown result type (might be due to invalid IL or missing references)
		//IL_096b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
		base.BuildBarManagerItems();
		ExportDocumentWordBarButtonItem = AddBarButtonItem("bExportDocumentWord", ArmSoft.SME.Common.Resources.ExportToWord, ImageService.GetImage("doc_export_word_16.svg"), ImageService.GetImage("doc_export_word_32.svg"), new KeyGesture((Key)96, (ModifierKeys)2));
		_bExportDocumentXML = AddBarButtonItem("bExportDocumentXML", ArmSoft.SME.Common.Resources.ExportToXML, ImageService.GetImage("doc_export_xml_16.svg"), ImageService.GetImage("doc_export_xml_32.svg"));
		ExportDocumentPDFBarButtonItem = AddBarButtonItem("bExportDocumentPDF", ArmSoft.SME.Common.Resources.ExportToPDF, ImageService.GetImage("doc_export_pdf_16.svg"), ImageService.GetImage("doc_export_pdf_32.svg"));
		_bExportXlsXLargeData = AddBarButtonItem("bExportXlsXLargeData", ArmSoft.SME.Common.Resources.ExportToExcel + " " + ArmSoft.SME.Common.Resources.LargeData, null, null, new KeyGesture((Key)91, (ModifierKeys)6));
		_bExportPDF = AddBarButtonItem("bExportPDF", ArmSoft.SME.Common.Resources.ExportToPDF, ImageService.GetImage("table_export_pdf_16.svg"), ImageService.GetImage("table_export_pdf_32.svg"));
		BarItemLink barItemLink = _bExportXlsX.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_bExportToExcel.ItemLinks.Add(barItemLink);
		barItemLink = _bExportXlsXLargeData.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_bExportToExcel.ItemLinks.Add(barItemLink);
		_cmdCopy = AddBarButtonItem("cmdCopy", ArmSoft.SME.Common.Resources.Copy, ImageService.GetImage("copy_16.svg"), ImageService.GetImage("copy_32.svg"), new KeyGesture((Key)46, (ModifierKeys)2));
		_cmdCopy.Command = ApplicationCommands.Copy;
		_cmdPaste = AddBarButtonItem("cmdPaste", ArmSoft.SME.Common.Resources.Paste, ImageService.GetImage("paste_16.svg"), ImageService.GetImage("paste_32.svg"), new KeyGesture((Key)65, (ModifierKeys)2));
		_cmdPaste.Command = ApplicationCommands.Paste;
		_chkReportDetails = AddBarCheckItem("chkReportDetails", ArmSoft.SME.Common.Resources.ReportDetails, ImageService.GetImage("table_show_details_16.svg"), ImageService.GetImage("table_show_details_32.svg"));
		_cmdExpandGroups = AddBarButtonItem("cmdExpandGroups", ArmSoft.SME.Common.Resources.ExpandGroups, ImageService.GetImage("plus_16.svg"), ImageService.GetImage("plus_32.svg"), new KeyGesture((Key)144, (ModifierKeys)2, "Ctrl + >"));
		_cmdExpandGroupLevel = AddBarSubItem("cmdExpandGroupLevel", ArmSoft.SME.Common.Resources.ExpandLevel, ImageService.GetImage("plus_minus_16.svg"), ImageService.GetImage("plus_minus_32.svg"));
		_cmdCollapseGroups = AddBarButtonItem("cmdCollapseGroups", ArmSoft.SME.Common.Resources.CollapseGroups, ImageService.GetImage("minus_16.svg"), ImageService.GetImage("minus_32.svg"), new KeyGesture((Key)142, (ModifierKeys)2, "Ctrl + <"));
		_chkShowGroupSummaryEditor = AddBarCheckItem("chkShowGroupSummaryEditor", ArmSoft.SME.Common.Resources.ShowSubTotals, ImageService.GetImage("report_16.svg"), ImageService.GetImage("report_32.svg"));
		_btnGroupColumns = AddBarButtonItem("btnGroupColumns", ArmSoft.SME.Common.Resources.GroupByColumns, ImageService.GetImage("grouping_16.svg"), ImageService.GetImage("grouping_32.svg"), new KeyGesture((Key)50, (ModifierKeys)6));
		_cmdSelectAll = AddBarButtonItem("cmdSelectAll", ArmSoft.SME.Common.Resources.SelectAll, null, null, new KeyGesture((Key)44, (ModifierKeys)2));
		_cmdSelectAll.Command = ApplicationCommands.SelectAll;
		_cmdSelectMarked = AddBarButtonItem("cmdSelectMarked", ArmSoft.SME.Common.Resources.SelectMarked, null, null);
		_cmdSelectByMask = AddBarButtonItem("cmdSelectByMask", ArmSoft.SME.Common.Resources.SelectByCondition, ImageService.GetImage("select_row_condition_16.svg"), ImageService.GetImage("select_row_condition_32.svg"));
		_cmdSelectByMask.Hint = "(Ctrl + \"+\")";
		_cmdUnSelectByMask = AddBarButtonItem("cmdUnSelectByMask", ArmSoft.SME.Common.Resources.UnSelectByCondition, ImageService.GetImage("select_row_condition_16.svg"), ImageService.GetImage("select_row_condition_32.svg"));
		_cmdUnSelectByMask.Hint = "(Ctrl + \"-\")";
		_cmdCardView = AddBarCheckItem("cmdCardView", ArmSoft.SME.Common.Resources.CardView, ImageService.GetImage("Card_View_16.svg"), ImageService.GetImage("Card_View_32.svg"));
		_cmdTreeView = AddBarCheckItem("cmdTreeView", ArmSoft.SME.Common.Resources.TreeView, ImageService.GetImage("TreeView16.svg"), ImageService.GetImage("TreeView32.svg"));
		_bSelect = AddBarSubItem("bSelect", ArmSoft.SME.Common.Resources.SelectRow, ImageService.GetImage("select_all_16.svg"), ImageService.GetImage("select_all_32.svg"));
		barItemLink = _cmdSelectAll.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_bSelect.ItemLinks.Add(barItemLink);
		barItemLink = _cmdSelectMarked.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_bSelect.ItemLinks.Add(barItemLink);
		_cmdFindData = AddBarButtonItem("cmdFindData", ArmSoft.SME.Common.Resources.Find, ImageService.GetImage("find_16.svg"), ImageService.GetImage("find_32.svg"), new KeyGesture((Key)49, (ModifierKeys)2));
		_cmdFilter = AddBarButtonItem("cmdFilter", ArmSoft.SME.Common.Resources.Filter, ImageService.GetImage("filter_16.svg"), ImageService.GetImage("filter_32.svg"));
		_cmdClearGroup = AddBarButtonItem("cmdClearGroup", ArmSoft.SME.Common.Resources.ClearGrouping, ImageService.GetImage("ungroup_16.svg"), ImageService.GetImage("ungroup_32.svg"));
		_cmdFilterCurrent = AddBarButtonItem("cmdFilterCurrent", ArmSoft.SME.Common.Resources.FilterByCurrentValue, ImageService.GetImage("filter_equal_16.svg"), ImageService.GetImage("filter_equal_32.svg"));
		_cmdFilterNotEqualCurrent = AddBarButtonItem("cmdFilterNotEqualCurrent", null, ImageService.GetImage("filter_not_equal_16.svg"), ImageService.GetImage("filter_not_equal_32.svg"));
		_cmdCopyCurrent = AddBarButtonItem("cmdCopyCurrent", null, ImageService.GetImage("copy_16.svg"), ImageService.GetImage("copy_32.svg"));
		_btnFilterSelectedData = AddBarButtonItem("btnFilterSelectedData", ArmSoft.SME.Common.Resources.FilterSelected, ImageService.GetImage("Filter_selected_16.svg"), ImageService.GetImage("Filter_selected_32.svg"), new KeyGesture((Key)52, (ModifierKeys)2));
		_cmdCopyFilter = AddBarButtonItem("cmdCopyFilter", null, null, null);
		_btnClearFilter = AddBarButtonItem("btnClearFilter", ArmSoft.SME.Common.Resources.ClearFilter, ImageService.GetImage("Filter_clear_16.svg"), ImageService.GetImage("Filter_clear_32.svg"));
		_cmdColumnChooser = AddBarButtonItem("cmdColumnChooser", ArmSoft.SME.Common.Resources.SelectColumns, ImageService.GetImage("select_column_16.svg"), ImageService.GetImage("select_column_32.svg"));
		_cmdAddUnboundColumns = AddBarButtonItem("cmdAddUnboundColumns", ArmSoft.SME.Common.Resources.UnboundColumns, ImageService.GetImage("table_functions_16.svg"), ImageService.GetImage("table_functions_32.svg"));
		_bLayoutSave = AddBarButtonItem("bLayoutSave", ArmSoft.SME.Common.Resources.Save, ImageService.GetImage("layout_add16.svg"), ImageService.GetImage("layout_add32.svg"));
		_bLayoutEdit = AddBarButtonItem("bLayoutEdit", ArmSoft.SME.Common.Resources.Edit, ImageService.GetImage("layout_edit16.svg"), ImageService.GetImage("layout_edit32.svg"));
		_bLayoutLoad = AddBarButtonItem("bLayoutLoad", ArmSoft.SME.Common.Resources.LayoutLoad, ImageService.GetImage("layout_icon16.svg"), ImageService.GetImage("layout_icon32.svg"), new KeyGesture((Key)60, (ModifierKeys)6));
		_bLayoutLoad.MergeType = BarItemMergeType.Add;
		_bLayoutDefault = AddBarButtonItem("bLayoutDefault", ArmSoft.SME.Common.Resources.LayoutRestoreDefault, ImageService.GetImage("layout_green_16.svg"), ImageService.GetImage("layout_green_32.svg"));
		_bLayoutDefault.MergeType = BarItemMergeType.Add;
		_bGridLayout = AddBarSubItem("bGridLayout", ArmSoft.SME.Common.Resources.LayoutTitle, ImageService.GetImage("layout_icon16.svg"), ImageService.GetImage("layout_icon32.svg"));
		barItemLink = _bLayoutLoad.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_bGridLayout.ItemLinks.Add(barItemLink);
		barItemLink = _bLayoutDefault.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_bGridLayout.ItemLinks.Add(barItemLink);
		barItemLink = _bLayoutSave.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_bGridLayout.ItemLinks.Add(barItemLink);
		barItemLink = _bLayoutEdit.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_bGridLayout.ItemLinks.Add(barItemLink);
		_statusBarCurrentRow = AddBarStaticItem("statusBarCurrentRow", "", null, null);
		_statusBarCurrentRow.ItemMinWidth = 150.0;
		_statusBarCurrentRow.AutoSizeMode = BarItemAutoSizeMode.Fill;
		_statusBarCurrentRow.ContentAlignment = HorizontalAlignment.Center;
		_statusBarCurrentRow.Focusable = false;
		_statusBarSelectedRows = AddBarStaticItem("statusBarSelectedRows", "", null, null);
		_statusBarSelectedRows.ItemMinWidth = 50.0;
		_statusBarSelectedRows.AutoSizeMode = BarItemAutoSizeMode.Fill;
		_statusBarSelectedRows.ContentAlignment = HorizontalAlignment.Center;
		_statusBarSelectedRows.Focusable = false;
		_statusBarCompanyName = AddBarStaticItem("statusBarCompanyName", "", null, null);
		_statusBarCompanyName.ItemMinWidth = 100.0;
	}

	protected override void BuildBarManagerUI()
	{
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		Grid grid = new Grid();
		BarManager.Child = grid;
		grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
		{
			Height = GridLength.Auto,
			MinHeight = 0.0
		});
		grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star),
			MinHeight = 0.0
		});
		grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
		{
			Height = GridLength.Auto,
			MinHeight = 0.0
		});
		grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		DockPanel dockPanel = new DockPanel
		{
			LastChildFill = true,
			UseLayoutRounding = true
		};
		System.Windows.Controls.Grid.SetRow(dockPanel, 0);
		System.Windows.Controls.Grid.SetColumn(dockPanel, 0);
		grid.Children.Add(dockPanel);
		RibbonControl = new RibbonControl
		{
			AllowCustomization = false,
			AllowKeyTips = true,
			RibbonStyle = RibbonStyle.Office2019,
			ShowApplicationButton = false,
			ToolbarShowMode = RibbonQuickAccessToolbarShowMode.Default,
			UseLayoutRounding = true
		};
		DockPanel.SetDock(RibbonControl, Dock.Top);
		dockPanel.Children.Add(RibbonControl);
		RibbonDefaultPageCategory = new RibbonDefaultPageCategory();
		RibbonControl.Categories.Add(RibbonDefaultPageCategory);
		RibbonPage page = CreateRibbonPage("RibbonPageHome", ArmSoft.SME.Common.Resources.Home);
		RibbonPageGroupRecords = RibbonSupportedPanelBase.CreateRibbonPageGroup(page, "RibbonPageGroupRecords", ArmSoft.SME.Common.Resources.Records, ImageService.GetImage("add_small_32.svg"));
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupRecords, "bDataViewAdd", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupRecords, "bDataViewEdit", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupRecords, "bDataViewDelete", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupRecords, "bDataViewCreateCopy", RibbonItemStyles.SmallWithText);
		RibbonPageGroup ribbonPageGroup = RibbonSupportedPanelBase.CreateRibbonPageGroup(page, "RibbonPageGroupDataView", ArmSoft.SME.Common.Resources.DataView, ImageService.GetImage("table_16.svg"));
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup, "bShowDataViewDialog", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup, "bRefreshDataView", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddSubLink(ribbonPageGroup, "bExportToExcel", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddCheckItemLink(ribbonPageGroup, "cmdGroupPanel", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup, "cmdFind", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddCheckItemLink(ribbonPageGroup, "cmdAutoFilter", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup, "btnFilterSelected", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup, "cmdClearSort", RibbonItemStyles.SmallWithText);
		RibbonPageGroup ribbonPageGroup2 = RibbonSupportedPanelBase.CreateRibbonPageGroup(page, "RibbonPagePrint", ArmSoft.SME.Common.Resources.Print, ImageService.GetImage("print_16.svg"));
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup2, "bPrintPreview", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup2, "bPrint", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup2, "bPrintOptions", RibbonItemStyles.SmallWithText);
		RibbonPageGroupDocument = RibbonSupportedPanelBase.CreateRibbonPageGroup(page, "RibbonPageGroupDocument", ArmSoft.SME.Common.Resources.Documents, ImageService.GetImage("print_16.svg"));
		RibbonSupportedPanelBase.AddCheckItemLink(RibbonPageGroupDocument, "cmdShowTransactions", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupDocument, "bDataViewDocument", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupDocument, "bExportDocumentExcel", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(RibbonPageGroupDocument, "bDocumentHistory", RibbonItemStyles.SmallWithText);
		RibbonPage page2 = CreateRibbonPage("RibbonPageData", ArmSoft.SME.Common.Resources.DataView);
		RibbonPageGroup ribbonPageGroup3 = RibbonSupportedPanelBase.CreateRibbonPageGroup(page2, "RibbonPageGroupView", ArmSoft.SME.Common.Resources.Face, ImageService.GetImage("table_16.svg"));
		RibbonSupportedPanelBase.AddCheckItemLink(ribbonPageGroup3, "chkReportDetails", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup3, "cmdClearSort", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup3, "cmdColumnChooser", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup3, "cmdAddUnboundColumns", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddSubLink(ribbonPageGroup3, "bGridLayout", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddCheckItemLink(ribbonPageGroup3, "cmdCardView");
		RibbonSupportedPanelBase.AddCheckItemLink(ribbonPageGroup3, "cmdTreeView");
		RibbonPageGroup ribbonPageGroup4 = RibbonSupportedPanelBase.CreateRibbonPageGroup(page2, "RibbonPageGroupDataFilter", ArmSoft.SME.Common.Resources.Filtering, ImageService.GetImage("filter_small_32.svg"));
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup4, "cmdFindData", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddCheckItemLink(ribbonPageGroup4, "cmdAutoFilter", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup4, "cmdFilter", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup4, "btnFilterSelectedData", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup4, "btnClearFilter", RibbonItemStyles.SmallWithText);
		RibbonPageGroup ribbonPageGroup5 = RibbonSupportedPanelBase.CreateRibbonPageGroup(page2, "RibbonPageGroupDataGroup", ArmSoft.SME.Common.Resources.Grouping, ImageService.GetImage("group_panel_16.svg"));
		RibbonSupportedPanelBase.AddCheckItemLink(ribbonPageGroup5, "cmdGroupPanel", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup5, "btnGroupColumns", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup5, "cmdClearGroup", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddSeparator(ribbonPageGroup5);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup5, "cmdExpandGroups", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddSubLink(ribbonPageGroup5, "cmdExpandGroupLevel", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup5, "cmdCollapseGroups", RibbonItemStyles.SmallWithText);
		RibbonSupportedPanelBase.AddCheckItemLink(ribbonPageGroup5, "chkShowGroupSummaryEditor", RibbonItemStyles.SmallWithText);
		RibbonPageGroup ribbonPageGroup6 = RibbonSupportedPanelBase.CreateRibbonPageGroup(page2, "RibbonPageGroupSelect", ArmSoft.SME.Common.Resources.SelectingWord, ImageService.GetImage("group_panel_16.svg"));
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup6, "cmdCopy");
		RibbonSupportedPanelBase.AddSubLink(ribbonPageGroup6, "bSelect");
		RibbonSupportedPanelBase.AddSeparator(ribbonPageGroup6);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup6, "cmdSelectByMask", RibbonItemStyles.Large);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup6, "cmdUnSelectByMask", RibbonItemStyles.Large);
		RibbonPageGroup ribbonPageGroup7 = RibbonSupportedPanelBase.CreateRibbonPageGroup(CreateRibbonPage("RibbonPageExport", ArmSoft.SME.Common.Resources.Export), "RibbonPageGroupExport", ArmSoft.SME.Common.Resources.Export, null);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup7, "bExportDocumentWord");
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup7, "bExportDocumentExcel");
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup7, "bExportDocumentXML");
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup7, "bExportDocumentPDF");
		RibbonSupportedPanelBase.AddSubLink(ribbonPageGroup7, "bExportToExcel");
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup7, "bExportPDF");
		RibbonPage ribbonPage = CreateRibbonPage("RibbonPageHelp", ArmSoft.SME.Common.Resources.Help);
		ribbonPage.MergeOrder = 4;
		RibbonPageGroup ribbonPageGroup8 = RibbonSupportedPanelBase.CreateRibbonPageGroup(ribbonPage, "RibbonPageGroupHelp", ArmSoft.SME.Common.Resources.Help, null);
		ribbonPageGroup8.AllowCollapse = false;
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup8, "barButtonHelp");
		DetailLayoutManager = new DockLayoutManager
		{
			ViewStyle = DockingViewStyle.Light
		};
		System.Windows.Controls.Grid.SetRow(DetailLayoutManager, 1);
		System.Windows.Controls.Grid.SetColumn(DetailLayoutManager, 0);
		grid.Children.Add(DetailLayoutManager);
		LayoutGroup layoutGroup = new LayoutGroup
		{
			Orientation = Orientation.Horizontal,
			VerticalAlignment = VerticalAlignment.Stretch,
			ClosingBehavior = ClosingBehavior.ImmediatelyRemove
		};
		DetailLayoutManager.LayoutRoot = layoutGroup;
		LayoutGroup layoutGroup2 = new LayoutGroup
		{
			Orientation = Orientation.Vertical,
			VerticalAlignment = VerticalAlignment.Stretch,
			ClosingBehavior = ClosingBehavior.ImmediatelyRemove
		};
		layoutGroup.Items.Add(layoutGroup2);
		LayoutPanel layoutPanel = new LayoutPanel
		{
			AllowClose = false,
			AllowFloat = false,
			AllowHide = false,
			AllowMove = false,
			FloatOnDoubleClick = false,
			ShowCaption = false,
			ItemHeight = new GridLength(1.0, GridUnitType.Star),
			ItemWidth = new GridLength(1.0, GridUnitType.Star)
		};
		layoutGroup2.Items.Add(layoutPanel);
		LoadingDecoratorGrid = new Grid();
		layoutPanel.Content = LoadingDecoratorGrid;
		Grid = new GridControl
		{
			Margin = (Settings.IsWin11Theme ? new Thickness(-1.0, 4.0, -1.0, -1.0) : new Thickness(0.0, 4.0, 0.0, 0.0)),
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			ClipboardCopyMode = ClipboardCopyMode.Default
		};
		LoadingDecoratorGrid.Children.Add(Grid);
		GridContextMenu = new PopupMenu();
		BarManager.SetDXContextMenu(Grid, GridContextMenu);
		TotalsPanel = new LayoutPanel
		{
			VerticalAlignment = VerticalAlignment.Stretch,
			VerticalContentAlignment = VerticalAlignment.Top,
			AllowClose = true,
			AllowFloat = false,
			AllowHide = false,
			AllowMove = false,
			AllowSizing = true,
			ClosingBehavior = ClosingBehavior.ImmediatelyRemove,
			FloatOnDoubleClick = false,
			ItemHeight = new GridLength(180.0),
			ItemWidth = new GridLength(1.0, GridUnitType.Star),
			Visibility = Visibility.Collapsed
		};
		layoutGroup2.Items.Add(TotalsPanel);
		ChartGroup = new DocumentGroup
		{
			AllowClose = false,
			AllowDock = false,
			AllowFloat = false,
			AllowHide = false,
			AllowMove = false,
			FloatOnDoubleClick = false,
			ShowTabForSinglePage = false,
			Visibility = Visibility.Collapsed
		};
		layoutGroup2.Items.Add(ChartGroup);
		DetailsPanel = new LayoutPanel
		{
			AllowFloat = false,
			AllowHide = false,
			ClosingBehavior = ClosingBehavior.ImmediatelyRemove,
			ItemHeight = GridLength.Auto,
			Caption = ArmSoft.SME.Common.Resources.ReportDetails,
			ItemWidth = new GridLength(1.0, GridUnitType.Star),
			Visibility = Visibility.Collapsed
		};
		layoutGroup2.Items.Add(DetailsPanel);
		DetailsPanelSc = new FlowDocumentScrollViewer
		{
			Height = double.NaN,
			MinHeight = 0.0,
			Margin = new Thickness(0.0),
			BorderBrush = Brushes.Black,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
			VerticalScrollBarVisibility = ScrollBarVisibility.Auto
		};
		DetailsPanel.Content = DetailsPanelSc;
		RibbonStatusBar = new RibbonStatusBarControl
		{
			Height = double.NaN,
			IsSizeGripVisible = false,
			Visibility = Visibility.Visible
		};
		System.Windows.Controls.Grid.SetRow(RibbonStatusBar, 2);
		System.Windows.Controls.Grid.SetColumn(RibbonStatusBar, 0);
		grid.Children.Add(RibbonStatusBar);
		RibbonStatusBar.LeftItemLinks.Add(new BarStaticItemLink
		{
			BarItemName = "statusBarCurrentRow",
			MergeType = BarItemMergeType.Remove
		});
		RibbonStatusBar.LeftItemLinks.Add(new BarStaticItemLink
		{
			BarItemName = "statusBarSelectedRows",
			MergeType = BarItemMergeType.Remove
		});
		RibbonStatusBar.LeftItemLinks.Add(new BarStaticItemLink
		{
			BarItemName = "statusBarCompanyName",
			MergeType = BarItemMergeType.Remove
		});
		TotalsPanelRight = new LayoutPanel
		{
			HorizontalAlignment = HorizontalAlignment.Right,
			AllowClose = true,
			AllowFloat = false,
			AllowHide = false,
			AllowMove = false,
			AllowSizing = true,
			ClosingBehavior = ClosingBehavior.ImmediatelyRemove,
			FloatOnDoubleClick = false,
			ItemWidth = new GridLength(150.0),
			Visibility = Visibility.Collapsed
		};
		layoutGroup.Items.Add(TotalsPanelRight);
	}

	public void SetBarStatesDisabled()
	{
		_barButtonStates.Clear();
		foreach (BarItem item in BarManager.Items)
		{
			_barButtonStates.Add(item.Name, item.IsEnabled);
			item.IsEnabled = false;
		}
	}

	public void SetBarStatesEnabled()
	{
		foreach (BarItem item in BarManager.Items)
		{
			item.IsEnabled = _barButtonStates[item.Name];
		}
	}

	public GridControl InnerGrid()
	{
		return Grid;
	}

	public RibbonControl InnerRibbonControl()
	{
		return RibbonControl;
	}

	public LayoutPanel InnerDetailsPanel()
	{
		return DetailsPanel;
	}

	public object StatusBarSelectedRowsContent()
	{
		return _statusBarSelectedRows.Content;
	}

	public object StatusBarCurrentRowContent()
	{
		return _statusBarCurrentRow.Content;
	}

	public RibbonDefaultPageCategory InnerRibbonDefaultPageCategory()
	{
		return RibbonDefaultPageCategory;
	}

	public BarButtonItem InnerDataViewDeleteButtonItem()
	{
		return DataViewDeleteBarButtonItem;
	}

	public BarCheckItem InnerReportDetailCheckItem()
	{
		return _chkReportDetails;
	}

	public BarCheckItem InnerCmdTreeView()
	{
		return _cmdTreeView;
	}
}
