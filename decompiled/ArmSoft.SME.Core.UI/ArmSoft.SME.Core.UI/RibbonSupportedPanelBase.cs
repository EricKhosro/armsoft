using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Extensions;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Ribbon;

namespace ArmSoft.SME.Core.UI;

public class RibbonSupportedPanelBase : DocumentPanel
{
	public enum MergingType
	{
		WithName,
		WithMergingProperties
	}

	protected BarButtonItem _bExportXlsX;

	protected BarSubItem _bExportToExcel;

	protected BarCheckItem _cmdGroupPanel;

	protected BarCheckItem _cmdAutoFilter;

	protected BarButtonItem _cmdFind;

	protected BarButtonItem _cmdClearSort;

	protected BarButtonItem _btnFilterSelected;

	protected BarButtonItem _bPrintPreview;

	protected BarButtonItem _bPrint;

	protected BarButtonItem _bExportDocumentXML;

	protected BarButtonItem _bExportCsv;

	protected BarButtonItem _bExportPDF;

	protected BarButtonItem _cmdCopy;

	protected BarCheckItem _chkReportDetails;

	protected BarButtonItem _cmdSelectAll;

	protected BarButtonItem _cmdSelectMarked;

	protected BarSubItem _bSelect;

	protected BarStaticItem _statusBarCurrentRow;

	protected BarStaticItem _statusBarSelectedRows;

	protected BarStaticItem _statusBarCompanyName;

	protected readonly bool _allowHomePanel;

	public BarManager BarManager { get; protected set; }

	public RibbonControl RibbonControl { get; protected set; }

	public RibbonDefaultPageCategory RibbonDefaultPageCategory { get; protected set; }

	protected RibbonPageGroup RibbonPageGroupRecords { get; set; }

	protected RibbonPageGroup RibbonPageGroupDocument { get; set; }

	protected RibbonStatusBarControl RibbonStatusBar { get; set; }

	protected DockLayoutManager DetailLayoutManager { get; set; }

	public LayoutPanel DetailsPanel { get; set; }

	protected LayoutPanel TotalsPanel { get; set; }

	protected FlowDocumentScrollViewer DetailsPanelSc { get; set; }

	public virtual MergingType BarItemMergingType => MergingType.WithMergingProperties;

	public BarButtonItem DataViewAddBarButtonItem { get; private set; }

	public BarButtonItem DataViewEditBarButtonItem { get; private set; }

	public BarButtonItem DataViewDeleteBarButtonItem { get; private set; }

	public BarButtonItem DataViewCreateCopyBarButtonItem { get; private set; }

	public BarButtonItem DataViewDocumentBarButtonItem { get; private set; }

	public BarButtonItem DocumentHistoryBarButtonItem { get; private set; }

	public BarButtonItem ShowDataViewDialogBarButtonItem { get; private set; }

	public BarButtonItem RefreshDataViewBarButtonItem { get; private set; }

	public BarButtonItem PrintOptionsBarButtonItem { get; private set; }

	public BarButtonItem ExportDocumentExcelBarButtonItem { get; private set; }

	public BarButtonItem ExportDocumentWordBarButtonItem { get; set; }

	public BarButtonItem ExportDocumentPDFBarButtonItem { get; set; }

	public BarCheckItem ShowTransactionsCheckItem { get; set; }

	public BarButtonItem HelpBarButtonItem { get; protected set; }

	public RibbonSupportedPanelBase(bool allowHomePanel)
	{
		BarManager = new BarManager
		{
			Focusable = false
		};
		_allowHomePanel = allowHomePanel;
		BarManager.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarManager_ItemClick).Handler;
		BuildBarManagerItems();
		BuildBarManagerUI();
		Content = BarManager;
	}

	protected virtual void BuildBarManagerItems()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		DataViewAddBarButtonItem = AddBarButtonItem("bDataViewAdd", ArmSoft.SME.Common.Resources.NewAdd, ImageService.GetImage("add_16.svg"), ImageService.GetImage("add_32.svg"), new KeyGesture((Key)57, (ModifierKeys)2));
		DataViewEditBarButtonItem = AddBarButtonItem("bDataViewEdit", ArmSoft.SME.Common.Resources.Edit, ImageService.GetImage("edit_16.svg"), ImageService.GetImage("edit_32.svg"), new KeyGesture((Key)48, (ModifierKeys)2));
		DataViewDeleteBarButtonItem = AddBarButtonItem("bDataViewDelete", ArmSoft.SME.Common.Resources.Delete, ImageService.GetImage("delete_16.svg"), ImageService.GetImage("delete_32.svg"), new KeyGesture((Key)47, (ModifierKeys)2));
		DataViewCreateCopyBarButtonItem = AddBarButtonItem("bDataViewCreateCopy", ArmSoft.SME.Common.Resources.Copy, ImageService.GetImage("doc_copy_16.svg"), ImageService.GetImage("doc_copy_32.svg"), new KeyGesture((Key)68, (ModifierKeys)2));
		ShowDataViewDialogBarButtonItem = AddBarButtonItem("bShowDataViewDialog", ArmSoft.SME.Common.Resources.ShowDialog, ImageService.GetImage("table_refresh_16.svg"), ImageService.GetImage("table_refresh_32.svg"), new KeyGesture((Key)50, (ModifierKeys)2));
		RefreshDataViewBarButtonItem = AddBarButtonItem("bRefreshDataView", ArmSoft.SME.Common.Resources.Refresh, ImageService.GetImage("refresh_16.svg"), ImageService.GetImage("refresh_32.svg"), new KeyGesture((Key)61, (ModifierKeys)2));
		_bExportXlsX = AddBarButtonItem("bExportXlsX", ArmSoft.SME.Common.Resources.ExportToExcel, null, null, new KeyGesture((Key)91, (ModifierKeys)2));
		_bExportToExcel = AddBarSubItem("bExportToExcel", ArmSoft.SME.Common.Resources.ExportToExcel, ImageService.GetImage("table_export_excel_16.svg"), ImageService.GetImage("table_export_excel_32.svg"));
		_cmdGroupPanel = AddBarCheckItem("cmdGroupPanel", ArmSoft.SME.Common.Resources.Grouping, ImageService.GetImage("group_panel_16.svg"), ImageService.GetImage("group_panel_32.svg"));
		_cmdAutoFilter = AddBarCheckItem("cmdAutoFilter", ArmSoft.SME.Common.Resources.ShowFilterRow, ImageService.GetImage("table_filter_row_16.svg"), ImageService.GetImage("table_filter_row_32.svg"), new KeyGesture((Key)60, (ModifierKeys)2));
		_cmdFind = AddBarButtonItem("cmdFind", ArmSoft.SME.Common.Resources.Find, ImageService.GetImage("find_16.svg"), ImageService.GetImage("find_32.svg"), new KeyGesture((Key)49, (ModifierKeys)2));
		_cmdClearSort = AddBarButtonItem("cmdClearSort", ArmSoft.SME.Common.Resources.ClearSortShort, ImageService.GetImage("sort_clear_all_16.svg"), ImageService.GetImage("sort_clear_all_32.svg"));
		_btnFilterSelected = AddBarButtonItem("btnFilterSelected", ArmSoft.SME.Common.Resources.FilterSelected, ImageService.GetImage("Filter_selected_16.svg"), ImageService.GetImage("Filter_selected_32.svg"), new KeyGesture((Key)52, (ModifierKeys)2));
		_bPrintPreview = AddBarButtonItem("bPrintPreview", ArmSoft.SME.Common.Resources.PrintPreview, ImageService.GetImage("table_preview_16.svg"), ImageService.GetImage("table_preview_32.svg"), new KeyGesture((Key)94, (ModifierKeys)4));
		_bPrint = AddBarButtonItem("bPrint", ArmSoft.SME.Common.Resources.Print, ImageService.GetImage("print_16.svg"), ImageService.GetImage("print_32.svg"), new KeyGesture((Key)59, (ModifierKeys)2));
		PrintOptionsBarButtonItem = AddBarButtonItem("bPrintOptions", ArmSoft.SME.Common.Resources.PreviewOptions, ImageService.GetImage("page_setup_16.svg"), ImageService.GetImage("page_setup_32.svg"));
		DataViewDocumentBarButtonItem = AddBarButtonItem("bDataViewDocument", ArmSoft.SME.Common.Resources.PrintPreview, ImageService.GetImage("doc_preview_16.svg"), ImageService.GetImage("doc_preview_32.svg"), new KeyGesture((Key)94));
		ExportDocumentExcelBarButtonItem = AddBarButtonItem("bExportDocumentExcel", ArmSoft.SME.Common.Resources.ExportToExcel, ImageService.GetImage("doc_export_excel_16.svg"), ImageService.GetImage("doc_export_excel_32.svg"), new KeyGesture((Key)97, (ModifierKeys)2));
		DocumentHistoryBarButtonItem = AddBarButtonItem("bDocumentHistory", ArmSoft.SME.Common.Resources.History, ImageService.GetImage("history_16.svg"), ImageService.GetImage("history_32.svg"), new KeyGesture((Key)51, (ModifierKeys)2));
		ShowTransactionsCheckItem = AddBarCheckItem("cmdShowTransactions", (Settings.SystemType == SystemTypes.SalesManagement) ? ArmSoft.SME.Common.Resources.ViewMoreDetails : ArmSoft.SME.Common.Resources.ViewTrans, null, null, new KeyGesture((Key)63, (ModifierKeys)6));
		if (Settings.SystemType == SystemTypes.Trade || Settings.SystemType == SystemTypes.SalesManagement)
		{
			ShowTransactionsCheckItem.Glyph = ImageService.GetImage("show_row_details_16.svg");
			ShowTransactionsCheckItem.LargeGlyph = ImageService.GetImage("show_row_details_32.svg");
		}
		else
		{
			ShowTransactionsCheckItem.Glyph = ImageService.GetImage("show_transactions_16.svg");
			ShowTransactionsCheckItem.LargeGlyph = ImageService.GetImage("show_transactions_32.svg");
		}
		HelpBarButtonItem = AddBarButtonItem("barButtonHelp", ArmSoft.SME.Common.Resources.Help, ImageService.GetImage("help_16.svg"), ImageService.GetImage("help_32.svg"), new KeyGesture((Key)90));
	}

	protected virtual void BuildBarManagerUI()
	{
	}

	protected BarButtonItem AddBarButtonItem(string name, string content, ImageSource glyph, ImageSource largeGlyph, KeyGesture keyGesture = null)
	{
		BarButtonItem barButtonItem = new BarButtonItem
		{
			Content = content,
			Glyph = glyph,
			LargeGlyph = largeGlyph,
			KeyGesture = keyGesture
		};
		if (BarItemMergingType == MergingType.WithName)
		{
			BarManager.Items.AddWithName(barButtonItem, name);
		}
		else
		{
			BarManager.Items.AddWithMergingProperties(barButtonItem, name);
		}
		return barButtonItem;
	}

	protected BarSubItem AddBarSubItem(string name, string content, ImageSource glyph, ImageSource largeGlyph, KeyGesture keyGesture = null)
	{
		BarSubItem barSubItem = new BarSubItem
		{
			Content = content,
			Glyph = glyph,
			LargeGlyph = largeGlyph,
			KeyGesture = keyGesture
		};
		if (BarItemMergingType == MergingType.WithName)
		{
			BarManager.Items.AddWithName(barSubItem, name);
		}
		else
		{
			BarManager.Items.AddWithMergingProperties(barSubItem, name);
		}
		return barSubItem;
	}

	protected BarCheckItem AddBarCheckItem(string name, string content, ImageSource glyph, ImageSource largeGlyph, KeyGesture keyGesture = null)
	{
		BarCheckItem barCheckItem = new BarCheckItem
		{
			Content = content,
			Glyph = glyph,
			LargeGlyph = largeGlyph,
			KeyGesture = keyGesture
		};
		if (BarItemMergingType == MergingType.WithName)
		{
			BarManager.Items.AddWithName(barCheckItem, name);
		}
		else
		{
			BarManager.Items.AddWithMergingProperties(barCheckItem, name);
		}
		return barCheckItem;
	}

	protected BarStaticItem AddBarStaticItem(string name, string content, ImageSource glyph, ImageSource largeGlyph, KeyGesture keyGesture = null)
	{
		BarStaticItem barStaticItem = new BarStaticItem
		{
			Content = content,
			Glyph = glyph,
			LargeGlyph = largeGlyph,
			KeyGesture = keyGesture
		};
		if (BarItemMergingType == MergingType.WithName)
		{
			BarManager.Items.AddWithName(barStaticItem, name);
		}
		else
		{
			BarManager.Items.AddWithMergingProperties(barStaticItem, name);
		}
		return barStaticItem;
	}

	protected virtual void BarManager_ItemClick(object sender, ItemClickEventArgs e)
	{
	}

	protected RibbonPage CreateRibbonPage(string name, string caption)
	{
		RibbonPage ribbonPage = new RibbonPage
		{
			Name = name,
			Caption = caption,
			MergeType = RibbonMergeType.MergeItems,
			CaptionTemplate = (DataTemplate)Settings.GenericResources["RibbonPageHeaderTemplate"]
		};
		RibbonDefaultPageCategory.Pages.Add(ribbonPage);
		return ribbonPage;
	}

	protected static RibbonPageGroup CreateRibbonPageGroup(RibbonPage page, string name, string caption, ImageSource glyph, bool allowCollapse = false)
	{
		RibbonPageGroup ribbonPageGroup = new RibbonPageGroup
		{
			Name = name,
			Caption = caption,
			AllowCollapse = allowCollapse,
			Glyph = glyph
		};
		page.Groups.Add(ribbonPageGroup);
		return ribbonPageGroup;
	}

	protected static void AddLink(RibbonPageGroup group, string barItemName, RibbonItemStyles style = RibbonItemStyles.Default, BarItemMergeType mergeType = BarItemMergeType.Replace)
	{
		BarButtonItemLink barButtonItemLink = new BarButtonItemLink
		{
			BarItemName = barItemName,
			MergeType = mergeType
		};
		if (style != RibbonItemStyles.Default)
		{
			barButtonItemLink.RibbonStyle = style;
		}
		group.ItemLinks.Add(barButtonItemLink);
	}

	protected static void AddCheckItemLink(RibbonPageGroup group, string barItemName, RibbonItemStyles style = RibbonItemStyles.Default, BarItemMergeType mergeType = BarItemMergeType.Replace)
	{
		BarCheckItemLink barCheckItemLink = new BarCheckItemLink
		{
			BarItemName = barItemName,
			MergeType = mergeType
		};
		if (style != RibbonItemStyles.Default)
		{
			barCheckItemLink.RibbonStyle = style;
		}
		group.ItemLinks.Add(barCheckItemLink);
	}

	protected static void AddSeparator(RibbonPageGroup group)
	{
		group.ItemLinks.Add(new BarItemLinkSeparator());
	}

	protected static void AddSubLink(RibbonPageGroup group, string barItemName, RibbonItemStyles style = RibbonItemStyles.Default, BarItemMergeType mergeType = BarItemMergeType.Replace)
	{
		BarSubItemLink barSubItemLink = new BarSubItemLink
		{
			BarItemName = barItemName,
			MergeType = mergeType
		};
		if (style != RibbonItemStyles.Default)
		{
			barSubItemLink.RibbonStyle = style;
		}
		group.ItemLinks.Add(barSubItemLink);
	}
}
