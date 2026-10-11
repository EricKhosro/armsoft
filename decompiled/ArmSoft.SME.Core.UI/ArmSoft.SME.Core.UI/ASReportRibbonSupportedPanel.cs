using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Ribbon;

namespace ArmSoft.SME.Core.UI;

public class ASReportRibbonSupportedPanel : RibbonSupportedPanelBase
{
	protected BarButtonItem _cmdColors;

	public override MergingType BarItemMergingType => MergingType.WithName;

	protected LayoutPanel SearchPanel { get; private set; }

	protected LayoutPanel MainReportPanel { get; private set; }

	protected LayoutGroup MainLayoutGroup { get; private set; }

	protected LayoutPanel ChartPanel { get; private set; }

	protected FlowDocumentScrollViewer ScrollViewHeader { get; private set; }

	protected FlowDocumentScrollViewer ScrollViewFooter { get; private set; }

	protected FlowDocumentScrollViewer ScrollView { get; private set; }

	protected ScrollViewer MainScrollViewer { get; private set; }

	protected Button FindNextButton { get; private set; }

	protected ComboBoxEdit FindTextComboBox { get; private set; }

	protected CheckEdit SearchUpCheckEdit { get; private set; }

	protected CheckEdit IgnoreCaseCheckEdit { get; private set; }

	protected CheckEdit MatchWholeWordCheckEdit { get; private set; }

	protected ScrollBar ScrollBar { get; private set; }

	public PopupMenu ReportContextMenu { get; set; }

	protected GridControl TotalsGrid { get; private set; }

	public ASReportRibbonSupportedPanel()
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
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		base.BuildBarManagerItems();
		ExportDocumentWordBarButtonItem = AddBarButtonItem("bExportDocumentWord", ArmSoft.SME.Common.Resources.ExportToWord, ImageService.GetImage("doc_export_word_16.svg"), ImageService.GetImage("doc_export_word_32.svg"), new KeyGesture((Key)96, (ModifierKeys)2));
		_bExportDocumentXML = AddBarButtonItem("bExportDocumentXML", ArmSoft.SME.Common.Resources.ExportToXML, ImageService.GetImage("doc_export_xml_16.svg"), ImageService.GetImage("doc_export_xml_32.svg"));
		ExportDocumentPDFBarButtonItem = AddBarButtonItem("bExportDocumentPDF", ArmSoft.SME.Common.Resources.ExportToPDF, ImageService.GetImage("doc_export_pdf_16.svg"), ImageService.GetImage("doc_export_pdf_32.svg"));
		_cmdColors = AddBarButtonItem("cmdColors", ArmSoft.SME.Common.Resources.Colors, null, null, new KeyGesture((Key)60, (ModifierKeys)2));
		_bExportPDF = AddBarButtonItem("bExportPDF", ArmSoft.SME.Common.Resources.ExportToPDF, ImageService.GetImage("table_export_pdf_16.svg"), ImageService.GetImage("table_export_pdf_32.svg"));
		BarItemLink barItemLink = _bExportXlsX.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_bExportToExcel.ItemLinks.Add(barItemLink);
		_cmdCopy = AddBarButtonItem("cmdCopy", ArmSoft.SME.Common.Resources.Copy, ImageService.GetImage("copy_16.svg"), ImageService.GetImage("copy_32.svg"), new KeyGesture((Key)46, (ModifierKeys)2));
		_cmdCopy.Command = ApplicationCommands.Copy;
		_chkReportDetails = AddBarCheckItem("chkReportDetails", ArmSoft.SME.Common.Resources.ReportDetails, ImageService.GetImage("table_show_details_16.svg"), ImageService.GetImage("table_show_details_32.svg"));
		_cmdSelectAll = AddBarButtonItem("cmdSelectAll", ArmSoft.SME.Common.Resources.SelectAll, null, null, new KeyGesture((Key)44, (ModifierKeys)2));
		_cmdSelectAll.Command = ApplicationCommands.SelectAll;
		_cmdSelectMarked = AddBarButtonItem("cmdSelectMarked", ArmSoft.SME.Common.Resources.SelectMarked, null, null);
		_bSelect = AddBarSubItem("bSelect", ArmSoft.SME.Common.Resources.SelectRow, ImageService.GetImage("select_all_16.svg"), ImageService.GetImage("select_all_32.svg"));
		barItemLink = _cmdSelectAll.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_bSelect.ItemLinks.Add(barItemLink);
		barItemLink = _cmdSelectMarked.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_bSelect.ItemLinks.Add(barItemLink);
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
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		base.BuildBarManagerUI();
		DockPanel dockPanel = new DockPanel
		{
			LastChildFill = true,
			UseLayoutRounding = true
		};
		BarManager.Child = dockPanel;
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
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup3, "cmdColors");
		RibbonPageGroup ribbonPageGroup4 = RibbonSupportedPanelBase.CreateRibbonPageGroup(page2, "RibbonPageGroupSelect", ArmSoft.SME.Common.Resources.SelectingWord, ImageService.GetImage("group_panel_16.svg"));
		ribbonPageGroup4.MergeType = RibbonMergeType.Replace;
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup4, "cmdCopy");
		RibbonSupportedPanelBase.AddSubLink(ribbonPageGroup4, "bSelect");
		RibbonPage ribbonPage = CreateRibbonPage("RibbonPageExport", ArmSoft.SME.Common.Resources.Export);
		ribbonPage.MergeType = RibbonMergeType.Replace;
		RibbonPageGroup ribbonPageGroup5 = RibbonSupportedPanelBase.CreateRibbonPageGroup(ribbonPage, "RibbonPageGroupExport", ArmSoft.SME.Common.Resources.Export, null);
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup5, "bExportDocumentWord");
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup5, "bExportDocumentExcel");
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup5, "bExportDocumentXML");
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup5, "bExportDocumentPDF");
		RibbonSupportedPanelBase.AddSubLink(ribbonPageGroup5, "bExportToExcel");
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup5, "bExportPDF");
		RibbonPage ribbonPage2 = CreateRibbonPage("RibbonPageHelp", ArmSoft.SME.Common.Resources.Help);
		ribbonPage2.MergeOrder = 4;
		RibbonPageGroup ribbonPageGroup6 = RibbonSupportedPanelBase.CreateRibbonPageGroup(ribbonPage2, "RibbonPageGroupHelp", ArmSoft.SME.Common.Resources.Help, null);
		ribbonPageGroup6.AllowCollapse = false;
		RibbonSupportedPanelBase.AddLink(ribbonPageGroup6, "barButtonHelp");
		RibbonStatusBar = new RibbonStatusBarControl
		{
			IsSizeGripVisible = false,
			MinHeight = 10.0,
			Visibility = Visibility.Visible
		};
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
		DockPanel.SetDock(RibbonStatusBar, Dock.Bottom);
		dockPanel.Children.Add(RibbonStatusBar);
		DetailLayoutManager = new DockLayoutManager
		{
			ViewStyle = DockingViewStyle.Light
		};
		dockPanel.Children.Add(DetailLayoutManager);
		MainLayoutGroup = new LayoutGroup
		{
			VerticalAlignment = VerticalAlignment.Stretch,
			Orientation = Orientation.Vertical
		};
		MainReportPanel = new LayoutPanel
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			AllowClose = false,
			AllowFloat = false,
			AllowHide = false,
			AllowMove = false,
			FloatOnDoubleClick = false,
			ItemHeight = new GridLength(1.0, GridUnitType.Star),
			ItemWidth = new GridLength(1.0, GridUnitType.Star),
			ShowCaption = false
		};
		Grid grid = new Grid
		{
			VerticalAlignment = VerticalAlignment.Stretch
		};
		grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
		{
			Height = GridLength.Auto
		});
		grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition
		{
			Width = GridLength.Auto
		});
		SearchPanel = new LayoutPanel
		{
			AllowActivate = true,
			AllowClose = true,
			AllowDock = false,
			AllowFloat = false,
			AllowHide = false,
			FloatOnDoubleClick = false,
			ItemHeight = GridLength.Auto,
			Visibility = Visibility.Collapsed
		};
		Grid.SetRow(SearchPanel, 0);
		Grid.SetColumn(SearchPanel, 0);
		Grid.SetColumnSpan(SearchPanel, 2);
		Grid grid2 = new Grid();
		grid2.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
		{
			Height = GridLength.Auto
		});
		grid2.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid2.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition
		{
			Width = GridLength.Auto
		});
		grid2.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition
		{
			Width = GridLength.Auto
		});
		FindTextComboBox = new ComboBoxEdit
		{
			Margin = new Thickness(5.0),
			TabIndex = 0
		};
		Grid.SetRow(FindTextComboBox, 0);
		Grid.SetColumn(FindTextComboBox, 0);
		grid2.Children.Add(FindTextComboBox);
		StackPanel stackPanel = new StackPanel
		{
			Orientation = Orientation.Horizontal
		};
		Grid.SetRow(stackPanel, 0);
		Grid.SetColumn(stackPanel, 1);
		FindNextButton = new Button
		{
			Content = ArmSoft.SME.Common.Resources.FindNext,
			Margin = new Thickness(5.0),
			HorizontalContentAlignment = HorizontalAlignment.Center,
			TabIndex = 1
		};
		stackPanel.Children.Add(FindNextButton);
		SearchUpCheckEdit = new CheckEdit
		{
			Content = ArmSoft.SME.Common.Resources.SearchUp,
			Margin = new Thickness(5.0),
			TabIndex = 3
		};
		stackPanel.Children.Add(SearchUpCheckEdit);
		IgnoreCaseCheckEdit = new CheckEdit
		{
			Content = ArmSoft.SME.Common.Resources.IgnoreCase,
			Margin = new Thickness(5.0),
			TabIndex = 3
		};
		stackPanel.Children.Add(IgnoreCaseCheckEdit);
		MatchWholeWordCheckEdit = new CheckEdit
		{
			Content = ArmSoft.SME.Common.Resources.FindWholeWord,
			Margin = new Thickness(5.0, 5.0, 10.0, 5.0),
			TabIndex = 4
		};
		stackPanel.Children.Add(MatchWholeWordCheckEdit);
		grid2.Children.Add(stackPanel);
		SearchPanel.Content = grid2;
		MainScrollViewer = new ScrollViewer
		{
			HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
			VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
			Margin = new Thickness(0.0)
		};
		Grid.SetRow(MainScrollViewer, 1);
		Grid.SetColumn(MainScrollViewer, 0);
		DockPanel dockPanel2 = new DockPanel();
		ScrollViewHeader = new FlowDocumentScrollViewer
		{
			MinHeight = 0.0,
			Margin = new Thickness(0.0),
			BorderBrush = Brushes.Black,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
			VerticalScrollBarVisibility = ScrollBarVisibility.Auto
		};
		DockPanel.SetDock(ScrollViewHeader, Dock.Top);
		dockPanel2.Children.Add(ScrollViewHeader);
		ScrollViewFooter = new FlowDocumentScrollViewer
		{
			MinHeight = 0.0,
			Margin = new Thickness(0.0),
			BorderBrush = Brushes.Black,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
			VerticalScrollBarVisibility = ScrollBarVisibility.Auto
		};
		DockPanel.SetDock(ScrollViewFooter, Dock.Bottom);
		dockPanel2.Children.Add(ScrollViewFooter);
		ScrollView = new FlowDocumentScrollViewer
		{
			Margin = new Thickness(0.0),
			BorderBrush = Brushes.Transparent,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
			IsSelectionEnabled = true,
			VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
		};
		ScrollView.PreviewMouseDoubleClick += ScV_PreviewMouseDoubleClick;
		ReportContextMenu = new PopupMenu();
		BarManager.SetDXContextMenu(ScrollView, ReportContextMenu);
		dockPanel2.Children.Add(ScrollView);
		MainScrollViewer.Content = dockPanel2;
		ScrollBar = new ScrollBar
		{
			Orientation = Orientation.Vertical
		};
		Grid.SetRow(ScrollBar, 1);
		Grid.SetColumn(ScrollBar, 1);
		grid.Children.Add(SearchPanel);
		grid.Children.Add(MainScrollViewer);
		grid.Children.Add(ScrollBar);
		MainReportPanel.Content = grid;
		ChartPanel = new LayoutPanel
		{
			VerticalAlignment = VerticalAlignment.Bottom,
			AllowClose = false,
			AllowFloat = false,
			AllowHide = false,
			AllowMove = false,
			FloatOnDoubleClick = false,
			ItemHeight = GridLength.Auto,
			Visibility = Visibility.Collapsed
		};
		TotalsPanel = new LayoutPanel
		{
			AllowClose = false,
			AllowFloat = false,
			AllowHide = false,
			AllowMove = false,
			FloatOnDoubleClick = false,
			ItemHeight = GridLength.Auto,
			Visibility = Visibility.Collapsed
		};
		TotalsGrid = new GridControl();
		TotalsPanel.Content = TotalsGrid;
		DetailsPanel = new LayoutPanel
		{
			VerticalAlignment = VerticalAlignment.Bottom,
			AllowFloat = false,
			AllowHide = false,
			AllowMove = false,
			FloatOnDoubleClick = false,
			ItemHeight = GridLength.Auto,
			ItemWidth = new GridLength(1.0, GridUnitType.Star),
			Visibility = Visibility.Collapsed
		};
		DetailsPanelSc = new FlowDocumentScrollViewer
		{
			MinHeight = 0.0,
			Margin = new Thickness(0.0),
			BorderBrush = Brushes.Black,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
			VerticalScrollBarVisibility = ScrollBarVisibility.Auto
		};
		DetailsPanel.Content = DetailsPanelSc;
		MainLayoutGroup.Items.Add(MainReportPanel);
		MainLayoutGroup.Items.Add(ChartPanel);
		MainLayoutGroup.Items.Add(TotalsPanel);
		MainLayoutGroup.Items.Add(DetailsPanel);
		DetailLayoutManager.LayoutRoot = MainLayoutGroup;
	}

	protected virtual void ScV_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
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
}
