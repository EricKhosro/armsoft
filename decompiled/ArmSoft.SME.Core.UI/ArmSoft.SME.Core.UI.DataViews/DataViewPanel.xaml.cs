using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.Models.Layout;
using ArmSoft.SME.Common.Models.UnboundColumns;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Converters;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Drawing.Printing;
using DevExpress.Export;
using DevExpress.Spreadsheet;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Docking.Base;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.ExpressionEditor;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.ExpressionEditor;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Ribbon;
using DevExpress.XtraPrinting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Serilog;

namespace ArmSoft.SME.Core.UI.DataViews;

public partial class DataViewPanel : DataViewRibbonSupportedPanel, INotifyPropertyChanged, IDataViewForExtensions, IComponentConnector, IStyleConnector
{
	public enum RefreshType
	{
		None,
		Refresh,
		ChangeParams
	}

	public record ReportHeader(string CompanyName, string Caption)
	{
		public string CompanyName { get; } = CompanyName;

		public string Caption { get; } = Caption;
	}

	public BarButtonItemLink AddFunction;

	public BarButtonItemLink EditFunction;

	public BarButtonItemLink DeleteFunction;

	public BarButtonItemLink CreateCopyFunction;

	public BarButtonItemLink PreviewFunction;

	public BarButtonItemLink DocumentHistoryFunction;

	public bool showTotalsAtRight;

	private static int _idCounter;

	internal HashSet<int> selectedRows;

	private int _visibleSelectedCount;

	private PopupMenu _shownGridMenu;

	private string _defaultFunction;

	private readonly string _idvFilterString;

	public readonly int Id;

	private bool _columnsWidthRestoredFromRegister;

	private List<SelectByConditionGridRow> _selectConditionList;

	public bool ShowTransactionsMode;

	public static readonly DependencyProperty HeaderImageProperty;

	public static readonly DependencyProperty SelectedFilterProperty;

	private bool _treeViewIsDefault;

	private readonly bool _isFilterPopupModeExcel;

	private readonly bool _isAutoFilterRowOldStyle;

	private List<GridLayoutItem> _arrLayouts;

	private List<GridLayoutItem> _arrLayoutsGroupBy;

	private string _defaultLayout = "";

	private string _currentLayout = "";

	private DataViewBase _defaultView;

	private readonly Dictionary<string, string> _unboundExpressions = new Dictionary<string, string>();

	private const string _selectedColumnName = "Selected";

	private const string _isLeafColumnName = "IsLeaf";

	private bool _panelIsLoaded;

	private bool _selectByKeyboard;

	private readonly Dictionary<string, DwpMenuItem> _dwpFunctions = new Dictionary<string, DwpMenuItem>();

	private int _showGridMenu_qnt;

	private object _currentCellValue;

	private string _currentCellDisplayText;

	private string _currentColumnName;

	private bool _isInGrid;

	private SerializableGroupingInfo _columnsGroupingInfo;

	private string _groupingInfoJson = "";

	private List<AsChart> _chartsList;

	private bool _isInFilterPanel;

	private bool _rowSelectedChanged;

	private int _selectedRowHandle;

	public IDataView IDV { get; private set; }

	public AsRibbon CategoryRibbon { get; set; }

	public AsRibbon ContextRibbon { get; set; }

	public MyTableView GridView { get; private set; }

	private CardView CardView { get; set; }

	public MyTreeView TreeView { get; private set; }

	internal RefreshType InRefresh { get; set; }

	public Control TransactionsControl { get; set; }

	public ImageSource HeaderImage
	{
		get
		{
			if ((ImageSource)((DependencyObject)this).GetValue(HeaderImageProperty) == null)
			{
				return IDV.HeaderImage;
			}
			return (ImageSource)((DependencyObject)this).GetValue(HeaderImageProperty);
		}
		set
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource"));
			((DependencyObject)this).SetValue(HeaderImageProperty, (object)value);
		}
	}

	public bool SelectedFilter
	{
		get
		{
			if (((DependencyObject)this).GetValue(SelectedFilterProperty) == null)
			{
				return false;
			}
			return (bool)((DependencyObject)this).GetValue(SelectedFilterProperty);
		}
		set
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedFilter"));
			((DependencyObject)this).SetValue(SelectedFilterProperty, (object)value);
		}
	}

	public string ReportTitle => IDV.ReportTitle();

	public string HelpID => IDV.HelpId;

	public bool DialogSupported => IDV.DialogSupported;

	public bool RefreshSupported => IDV.RefreshSupported;

	public bool AllowAdd => IDV.AllowAdd;

	public bool AllowEdit => IDV.AllowEdit;

	public bool AllowView => IDV.AllowView;

	public bool AllowDelete => IDV.AllowDelete;

	public bool AllowCreateCopy => IDV.AllowCreateCopy;

	public bool AllowPreview => IDV.AllowPreview;

	public bool AllowExport => IDV.AllowExport;

	public bool AllowDocumentHistory => IDV.AllowDocumentHistory;

	public int SearchDelay => IDV.SearchDelay;

	public bool PanelIsLoaded
	{
		get
		{
			return _panelIsLoaded;
		}
		set
		{
			_panelIsLoaded = value;
			if (value)
			{
				ShowTotalsPanel();
			}
		}
	}

	public bool ShowFilterRow
	{
		get
		{
			if (Grid.View is IPanelView panelView)
			{
				return panelView.ShowFilterRow;
			}
			return false;
		}
		set
		{
			if (Grid.View is IPanelView panelView)
			{
				panelView.ShowFilterRow = value;
			}
		}
	}

	public bool ShowGroupPanel
	{
		get
		{
			return GridView.ShowGroupPanel;
		}
		set
		{
			GridView.ShowGroupPanel = value;
		}
	}

	public bool IsSorted
	{
		get
		{
			if (Grid.View is IPanelView panelView)
			{
				foreach (ColumnBase visibleColumn in panelView.GetVisibleColumns())
				{
					if (visibleColumn.SortOrder == ColumnSortOrder.Ascending || visibleColumn.SortOrder == ColumnSortOrder.Descending)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public bool IsGrouped => Grid.IsGrouped;

	public double TotalsPanelRightWidth { get; set; }

	public double TotalsPanelHeight { get; set; }

	public bool ColumnHeadersShowIn2Lines
	{
		get
		{
			if (Grid.Columns.Count > 0)
			{
				return Grid.Columns[0].HeaderTemplate != null;
			}
			return false;
		}
		set
		{
			foreach (GridColumn column in Grid.Columns)
			{
				if (value)
				{
					column.HeaderTemplate = (DataTemplate)Resources["Column2RowHeaderTemplate"];
				}
				else
				{
					column.HeaderTemplate = null;
				}
			}
		}
	}

	public bool IsAutoWidthMode
	{
		get
		{
			if (Grid.View == GridView && GridView.AutoWidth)
			{
				return true;
			}
			if (Grid.View == TreeView && TreeView.AutoWidth)
			{
				return true;
			}
			return false;
		}
		set
		{
			if (GridView != null)
			{
				GridView.AutoWidth = value;
			}
			else if (TreeView != null)
			{
				TreeView.AutoWidth = value;
			}
		}
	}

	public bool IsShowAtRightTotalPanel
	{
		get
		{
			return showTotalsAtRight;
		}
		set
		{
			showTotalsAtRight = value;
		}
	}

	public List<AsChart> ChartsList
	{
		set
		{
			_chartsList = value;
			if (_chartsList != null && _chartsList.Count > 0)
			{
				ChartGroup.Items.Clear();
				foreach (AsChart charts in _chartsList)
				{
					DocumentPanel item = new DocumentPanel
					{
						Caption = charts.ChartTitle,
						Content = charts,
						AllowHide = false,
						AllowClose = false,
						AllowFloat = false,
						AllowMove = false,
						AllowContextMenu = false,
						FloatOnDoubleClick = false,
						ShowCaptionImage = false
					};
					ChartGroup.Items.Add(item);
				}
				ChartGroup.Visibility = Visibility.Visible;
			}
			else
			{
				ChartGroup.Items.Clear();
				ChartGroup.Visibility = Visibility.Collapsed;
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public virtual void OnMagneticCardReader(string mcrData)
	{
		IDV.OnMagneticCardReader(mcrData);
	}

	public virtual void OnScanner(string scannerData)
	{
		IDV.OnScanner(scannerData);
	}

	public virtual void OnF2()
	{
		IDV.OnF2();
	}

	private static void OnListGotFocus(object sender, RoutedEventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected Obj, but got Unknown
		DependencyObject val = (DependencyObject)e.OriginalSource;
		if (((ListBox)sender).Name != "List")
		{
			e.Handled = false;
		}
		else if (val is ListBoxItem listBoxItem)
		{
			listBoxItem.IsSelected = true;
			TextBox textBox = FindChildTextBox((DependencyObject)(object)listBoxItem);
			if (textBox != null)
			{
				textBox.Focus();
				e.Handled = true;
			}
		}
		else if (val is TextBox textBox2)
		{
			textBox2.IsReadOnlyCaretVisible = true;
			e.Handled = true;
		}
	}

	private static TextBox FindChildTextBox(DependencyObject el)
	{
		if (VisualTreeHelper.GetChildrenCount(el) == 0)
		{
			return null;
		}
		for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(el) - 1; i++)
		{
			try
			{
				DependencyObject child = VisualTreeHelper.GetChild(el, i);
				if (child is TextBox result)
				{
					return result;
				}
				TextBox textBox = FindChildTextBox(child);
				if (textBox != null)
				{
					return textBox;
				}
			}
			catch (Exception)
			{
			}
		}
		return null;
	}

	static DataViewPanel()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected Obj, but got Unknown
		_idCounter = 1;
		HeaderImageProperty = DependencyProperty.Register("HeaderImage", typeof(ImageSource), typeof(DataViewPanel));
		SelectedFilterProperty = DependencyProperty.Register("SelectedFilter", typeof(bool), typeof(DataViewPanel));
		BaseColumn.FixedProperty.OverrideMetadata(typeof(GridColumn), (PropertyMetadata)(object)new FrameworkPropertyMetadata(null, (DependencyObject d, object value) => (!(((GridColumn)(object)d).FieldName == "DX$CheckboxSelectorColumn")) ? value : ((object)FixedStyle.Left)));
	}

	public DataViewPanel(IDataView idv)
	{
		string ribbonCommandCaption = "";
		ImageSource smallImage = null;
		ImageSource largeImage = null;
		IDV = idv;
		selectedRows = new HashSet<int>();
		FloatOnDoubleClick = false;
		AllowDrag = true;
		InitializeComponent();
		DataContext = idv;
		InRefresh = RefreshType.None;
		Focusable = true;
		if (!Settings.ShowTransactionsSupport())
		{
			RibbonPageGroupDocument.ItemLinks.Remove(ShowTransactionsCheckItem.CreateLink());
		}
		_columnsWidthRestoredFromRegister = false;
		RibbonControl.SelectedPageOnMerging = SelectedPageOnMerging.ParentSelectedPage;
		EventManager.RegisterClassHandler(typeof(ListBox), System.Windows.UIElement.GotFocusEvent, new RoutedEventHandler(OnListGotFocus));
		foreach (BarItemLinkBase toolbarItemLink in RibbonControl.ToolbarItemLinks)
		{
			toolbarItemLink.MergeType = BarItemMergeType.Replace;
		}
		try
		{
			_isAutoFilterRowOldStyle = Settings.ParametersService.AutoFilterRowOldStyle();
		}
		catch
		{
			_isAutoFilterRowOldStyle = false;
		}
		Settings.HostingEnvironment.CurrentViewPanel = this;
		AllowRestore = false;
		Id = _idCounter;
		_idCounter++;
		CategoryRibbon = new AsRibbon();
		ContextRibbon = new AsRibbon();
		_defaultFunction = "";
		idv.InitPanelRibbonFunctions();
		CategoryRibbon.FillingCompleted = true;
		ContextRibbon.FillingCompleted = true;
		if (CategoryRibbon != null)
		{
			CategoryRibbon.Caption = Caption;
		}
		IDV.TransactionsPanelRibbonSettings(ref ribbonCommandCaption, ref smallImage, ref largeImage);
		if (!string.IsNullOrEmpty(ribbonCommandCaption))
		{
			ShowTransactionsCheckItem.Content = ribbonCommandCaption;
		}
		if (smallImage != null)
		{
			ShowTransactionsCheckItem.Glyph = smallImage;
		}
		if (largeImage != null)
		{
			ShowTransactionsCheckItem.LargeGlyph = largeImage;
		}
		Grid.AutoGenerateColumns = AutoGenerateColumnsMode.None;
		AddSelectionAndLeafColumn();
		if (_cmdCardView.IsChecked.Value && idv.CardViewAllowed())
		{
			InitCardView(idv);
		}
		else if (idv.TreeViewAllowed(ref _treeViewIsDefault) && !string.IsNullOrWhiteSpace(idv.KeyFieldName().Trim()) && !string.IsNullOrWhiteSpace(idv.ParentFieldName().Trim()))
		{
			if (_treeViewIsDefault)
			{
				InitTreeView(idv);
			}
			else
			{
				InitView(idv);
			}
		}
		else
		{
			InitView(idv);
		}
		if (Grid.View == GridView || Grid.View == TreeView)
		{
			Grid.SelectionMode = MultiSelectMode.MultipleRow;
		}
		else
		{
			Grid.SelectionMode = MultiSelectMode.Row;
		}
		try
		{
			_isFilterPopupModeExcel = Settings.ParametersService.FilterPopupModeExcel();
		}
		catch
		{
			_isFilterPopupModeExcel = true;
		}
		PopulateColumns();
		if (idv.ColHeadersIn2Lines)
		{
			ColumnHeadersShowIn2Lines = idv.ColHeadersIn2Lines;
		}
		FlowDocument flowDocument = new FlowDocument
		{
			FontFamily = Settings.FontFamily
		};
		DetailsPanelSc.Document = flowDocument;
		flowDocument.FontSize = 3.0;
		FillDetail();
		SetBarStates();
		HeaderImage = idv.HeaderImage;
		if (!string.IsNullOrEmpty(idv.FilterString))
		{
			_idvFilterString = idv.FilterString;
			Grid.FilterString = _idvFilterString;
		}
		LoadLayouts();
		GotFocus += new SafeEventHandler<RoutedEventArgs>(DataViewPanel_GotFocus).Handler;
		Loaded += new SafeEventHandler<RoutedEventArgs>(DataViewPanel_Loaded).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(DataViewPanel_PreviewKeyDown).Handler;
		PreviewMouseWheel += new SafeEventHandler<MouseWheelEventArgs>(DataViewPanel_PreviewMouseWheel).Handler;
		Unloaded += new SafeEventHandler<RoutedEventArgs>(DataViewPanel_Unloaded).Handler;
		Grid.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(Grid_PreviewKeyDown).Handler;
		Grid.PropertyChanged += new SafeEventHandler<PropertyChangedEventArgs>(Grid_PropertyChanged).Handler;
		Grid.GroupRowCollapsed += new SafeEventHandler<RowEventArgs>(Grid_GroupRowCollapsed).Handler;
		Grid.GroupRowExpanded += new SafeEventHandler<RowEventArgs>(Grid_GroupRowExpanded).Handler;
		Grid.SelectionChanged += new SafeEventHandler<GridSelectionChangedEventArgs>(Grid_SelectionChanged).Handler;
		Grid.PreviewKeyUp += new SafeEventHandler<KeyEventArgs>(Grid_PreviewKeyUp).Handler;
		Grid.PreviewMouseDown += new SafeEventHandler<MouseButtonEventArgs>(Grid_PreviewMouseDown).Handler;
		Grid.StartGrouping += new SafeEventHandler<RoutedEventArgs>(Grid_StartGrouping).Handler;
		Grid.EndGrouping += new SafeEventHandler<RoutedEventArgs>(Grid_EndGrouping).Handler;
		Grid.FilterChanged += new SafeEventHandler<RoutedEventArgs>(Grid_FilterChanged).Handler;
		DetailLayoutManager.DockItemClosing += new SafeEventHandler<ItemCancelEventArgs>(DetailLayoutManager_DockItemClosing).Handler;
		DetailLayoutManager.ShowingMenu += new SafeEventHandler<ShowingMenuEventArgs>(DetailLayoutManager_ShowingMenu).Handler;
		Grid.CustomUnboundColumnData += new SafeEventHandler<GridColumnDataEventArgs>(Grid_CustomUnboundColumnData).Handler;
		DetailsPanel.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(DetailsPanel_PreviewKeyDown).Handler;
		Grid.View.UnboundExpressionEditorCreated += new SafeEventHandler<UnboundExpressionEditorEventArgs>(View_UnboundExpressionEditorCreated).Handler;
		if (Settings.ShowTransactionsSupport())
		{
			if (idv.AllowShowTransactions)
			{
				TransactionsControl = idv.CreateTransactionsControl();
			}
			if (idv.ShowTransactionsOnOpen)
			{
				ShowTransactionsCheckItem.IsChecked = true;
			}
			DataTemplate captionTemplate = (DataTemplate)Resources["PanelsHeader"];
			TotalsPanel.Caption = idv.TransactionsPanelCaption;
			TotalsPanel.CaptionTemplate = captionTemplate;
			TotalsPanelRight.Caption = idv.TransactionsPanelCaption;
			TotalsPanelRight.CaptionTemplate = captionTemplate;
			DetailsPanel.CaptionTemplate = captionTemplate;
		}
		AddSelectorColumnStyle();
	}

	public void AddConditionalFormattingToView()
	{
		List<AsFormatting> conditionalFormattingList = IDV.ConditionalFormattingList;
		if (conditionalFormattingList == null || conditionalFormattingList.Count <= 0)
		{
			return;
		}
		foreach (FormatConditionBase item in ConditionalFormattingHelper.ConvertConditions(IDV.ConditionalFormattingList))
		{
			if (Grid.View == GridView)
			{
				GridView.FormatConditions.Add(item);
			}
			else if (Grid.View == TreeView)
			{
				TreeView.FormatConditions.Add(item);
			}
		}
	}

	private void AddSelectorColumnStyle()
	{
		if (Settings.IsWin11Theme)
		{
			Resources.Add(typeof(GridColumn), (System.Windows.Style)Resources["CheckboxSelectorColumStyle"]);
		}
	}

	private void View_UnboundExpressionEditorCreated(object sender, UnboundExpressionEditorEventArgs e)
	{
		e.ExpressionEditorMode = ExpressionEditorMode.AutoComplete;
		DevExpress.Xpf.ExpressionEditor.ExpressionEditorControl expressionEditorControl = e.AutoCompleteExpressionEditorControl as DevExpress.Xpf.ExpressionEditor.ExpressionEditorControl;
		expressionEditorControl.Tag = e.Column.Name;
		expressionEditorControl.FontFamily = Settings.FontFamily;
		for (int num = expressionEditorControl.Context.Columns.Count - 1; num >= 0; num--)
		{
			if (string.IsNullOrWhiteSpace(expressionEditorControl.Context.Columns[num].Name))
			{
				expressionEditorControl.Context.Columns.RemoveAt(num);
				break;
			}
		}
		if (((MyGridColumn)e.Column).Tag == null)
		{
			expressionEditorControl.Unloaded += ExpressionControl_Unloaded;
		}
	}

	private void ExpressionControl_Unloaded(object sender, RoutedEventArgs e)
	{
		DevExpress.Xpf.ExpressionEditor.ExpressionEditorControl expressionEditorControl = (DevExpress.Xpf.ExpressionEditor.ExpressionEditorControl)sender;
		GridColumn gridColumn = Grid.Columns[expressionEditorControl.Tag.ToString()];
		UnboundColumn unboundColumn = Settings.UnboundColumnsService.Get(IDV.DataViewGroup, gridColumn.Name);
		if (unboundColumn != null && gridColumn.UnboundExpression != unboundColumn.Expression)
		{
			unboundColumn.Expression = gridColumn.UnboundExpression;
			Settings.UnboundColumnsService.Store(unboundColumn);
			if (_unboundExpressions.ContainsKey(unboundColumn.Name))
			{
				_unboundExpressions[unboundColumn.Name] = gridColumn.UnboundExpression;
			}
		}
		expressionEditorControl.Unloaded -= ExpressionControl_Unloaded;
	}

	private void AddSelectionAndLeafColumn()
	{
		MyGridColumn myGridColumn = new MyGridColumn
		{
			Name = "Selected",
			Width = 22.0,
			Header = ArmSoft.SME.Common.Resources.RowSelection,
			FieldName = "Selected",
			AllowPrinting = false,
			FixedWidth = true,
			UnboundType = UnboundColumnType.Boolean,
			AllowSorting = DefaultBoolean.False,
			AllowGrouping = DefaultBoolean.False,
			AllowResizing = DefaultBoolean.False,
			AllowMoving = DefaultBoolean.False,
			FilterPopupMode = FilterPopupMode.CheckedList,
			Fixed = FixedStyle.Left,
			Visible = false,
			ColumnChooserHeaderCaption = ArmSoft.SME.Common.Resources.RowSelection
		};
		Grid.Columns.Add(myGridColumn);
		myGridColumn.ShowInColumnChooser = false;
		myGridColumn = new MyGridColumn
		{
			Name = "IsLeaf",
			Width = 22.0,
			Header = "",
			FieldName = "IsLeaf",
			ReadOnly = true,
			UnboundType = UnboundColumnType.Boolean
		};
		Grid.Columns.Add(myGridColumn);
		myGridColumn.Visible = false;
		myGridColumn.ShowInColumnChooser = false;
	}

	private void InitView(IDataView idv)
	{
		if (GridView == null)
		{
			GridView = new MyTableView
			{
				ShowCriteriaInAutoFilterRow = !_isAutoFilterRowOldStyle,
				GroupValueTemplate = (DataTemplate)Resources["GroupRowHeaderTemplate"],
				Name = "MyTableView",
				SearchPanelParseMode = SearchPanelParseMode.Mixed,
				AllowFixedGroups = DefaultBoolean.True,
				AllowMergedGrouping = true,
				NavigationStyle = GridViewNavigationStyle.Row,
				UseLightweightTemplates = UseLightweightTemplates.All,
				CellStyle = (System.Windows.Style)Resources["CellStyle"],
				AutoFilterRowCellStyle = (System.Windows.Style)Resources["AutoFilterCellStyle"],
				PrintColumnHeaderStyle = (System.Windows.Style)Resources["PrintColumnHeaderStyle"],
				IDV = idv,
				DVP = this,
				IsRowCellMenuEnabled = true,
				IsTotalSummaryMenuEnabled = true,
				IsGroupPanelMenuEnabled = true,
				ShowCheckBoxSelectorColumn = true,
				CheckBoxSelectorColumnWidth = (Settings.IsWin11Theme ? 25 : 22),
				ShowCheckBoxSelectorInGroupRow = true,
				AllowConditionalFormattingMenu = true,
				ShowFocusedRectangle = true,
				ShowGroupPanel = false,
				ShowGroupedColumns = true,
				ShowTotalSummary = true,
				AllowBestFit = true,
				BestFitMaxRowCount = 50,
				SearchPanelPosition = SearchPanelPosition.OverGroupPanel,
				SearchPanelAllowFilter = true,
				GroupSummaryDisplayMode = GroupSummaryDisplayMode.AlignByColumns,
				GroupRowStyle = (System.Windows.Style)Resources["GroupGridRowContentStyle"],
				RowStyle = (System.Windows.Style)Resources["RowStyle"],
				AllowFormatConditionFiltersMenu = false,
				AllowFixedColumnMenu = true
			};
			if (Settings.IsWin11Theme)
			{
				GridView.CheckBoxSelectorColumnHeaderTemplate = (DataTemplate)Resources["SelectorColumnHeaderWin11Template"];
			}
			else
			{
				GridView.CheckBoxSelectorColumnHeaderTemplate = (DataTemplate)Resources["SelectorColumnHeaderTemplate"];
			}
			GridView.ShowFilterPopup += new SafeEventHandler<FilterPopupEventArgs>(TableView_ShowFilterPopup).Handler;
			GridView.RowDoubleClick += new SafeEventHandler<RowDoubleClickEventArgs>(GridRow_MouseDoubleClick).Handler;
			GridView.FocusedRowHandleChanged += new SafeEventHandler<FocusedRowHandleChangedEventArgs>(TableView_FocusedRowHandleChanged).Handler;
			GridView.ShowGridMenu += new SafeEventHandler<GridMenuEventArgs>(ShowTotalGridMenu).Handler;
			GridView.ContextMenuOpening += new SafeEventHandler<ContextMenuEventArgs>(Grid_ContextMenuOpening).Handler;
		}
		Grid.View = GridView;
	}

	private void InitTreeView(IDataView idv)
	{
		if (TreeView == null)
		{
			TreeView = new MyTreeView
			{
				ShowCriteriaInAutoFilterRow = !_isAutoFilterRowOldStyle,
				ExpandNodesOnFiltering = true,
				ShowColumnHeaders = true,
				ShowCheckboxes = true,
				AllowRecursiveNodeChecking = false,
				AllowIndeterminateCheckState = false,
				UseIndicatorForSelection = false,
				UseLightweightTemplates = UseLightweightTemplates.All,
				AllowConditionalFormattingMenu = true,
				AllowColumnFiltering = true,
				AllowColumnMoving = true,
				AllowSorting = true,
				ScrollingMode = ScrollingMode.Smart,
				AutoScrollOnSorting = false,
				NavigationStyle = GridViewNavigationStyle.Row,
				ShowFocusedRectangle = true,
				AllowMoveColumnToDropArea = true,
				IsTotalSummaryMenuEnabled = true,
				ShowTotalSummary = true,
				TreeDerivationMode = TreeDerivationMode.Selfreference,
				AutoFilterRowCellStyle = (System.Windows.Style)Resources["AutoFilterCellStyle"],
				KeyFieldName = idv.KeyFieldName().Trim(),
				ParentFieldName = idv.ParentFieldName().Trim(),
				IsRowCellMenuEnabled = true,
				AllowBestFit = true,
				BestFitMaxRowCount = 50,
				RowStyle = (System.Windows.Style)Resources["SelectedTreeRowStyle"],
				SearchPanelHorizontalAlignment = System.Windows.HorizontalAlignment.Left,
				SearchPanelAllowFilter = true,
				AllowFormatConditionFiltersMenu = false,
				AutoExpandAllNodes = IDV.ShowExpanded()
			};
			AddTreeViewConditionalFormatting();
			TreeView.CustomUnboundColumnData += new SafeEventHandler<TreeListUnboundColumnDataEventArgs>(Tree_CustomUnboundColumnData).Handler;
			TreeView.RowDoubleClick += new SafeEventHandler<RowDoubleClickEventArgs>(GridRow_MouseDoubleClick).Handler;
			TreeView.FocusedRowHandleChanged += new SafeEventHandler<FocusedRowHandleChangedEventArgs>(TableView_FocusedRowHandleChanged).Handler;
			TreeView.ShowGridMenu += new SafeEventHandler<GridMenuEventArgs>(ShowTotalGridMenu).Handler;
			TreeView.ContextMenuOpening += new SafeEventHandler<ContextMenuEventArgs>(Grid_ContextMenuOpening).Handler;
			TreeView.CanSelectRow += new SafeEventHandler<CanSelectRowEventArgs>(TreeView_CanSelectRow).Handler;
			TreeView.CanUnselectRow += new SafeEventHandler<CanUnselectRowEventArgs>(TreeView_CanUnselectRow).Handler;
			TreeView.NodeCheckStateChanged += new SafeEventHandler<TreeListNodeEventArgs>(TreeView_NodeCheckStateChanged).Handler;
		}
		Grid.View = TreeView;
		Grid.SelectionMode = MultiSelectMode.MultipleRow;
		SetTotalSummariesSettings();
	}

	private void InitCardView(IDataView idv)
	{
		if (CardView == null)
		{
			CardView = new CardView
			{
				CardLayout = CardLayout.Rows,
				CardRowTemplateSelector = (DataTemplateSelector)Resources["mySelector"],
				PrintCellStyle = (System.Windows.Style)Resources["MyPrintEditStyle"],
				AllowCardResizing = true,
				ShowGroupPanel = false,
				ShowColumnHeaders = false,
				AllowColumnFiltering = false,
				AllowColumnMoving = false,
				AllowSorting = false,
				AllowGrouping = false,
				ScrollingMode = ScrollingMode.Normal,
				NavigationStyle = GridViewNavigationStyle.Row,
				ShowFocusedRectangle = true,
				AllowMoveColumnToDropArea = false,
				PrintAutoCardWidth = false,
				MaxCardCountInRow = 3,
				PrintCardMargin = new Thickness(5.0),
				SearchPanelPosition = SearchPanelPosition.OverGroupPanel
			};
			DataTemplate cardTemplate = idv.GetCardTemplate();
			if (cardTemplate != null)
			{
				CardView.CardTemplate = cardTemplate;
			}
			cardTemplate = idv.GetCardPrintTemplate();
			if (cardTemplate != null)
			{
				CardView.PrintCardTemplate = cardTemplate;
			}
			cardTemplate = idv.GetCardHeaderTemplate();
			if (cardTemplate != null)
			{
				CardView.CardHeaderTemplate = cardTemplate;
			}
			CardView.MouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(EditCard).Handler;
			CardView.ContextMenuOpening += new SafeEventHandler<ContextMenuEventArgs>(Grid_ContextMenuOpening).Handler;
		}
		Grid.View = CardView;
	}

	internal void SetCaption()
	{
		string text = null;
		string text2 = null;
		if (text == null)
		{
			text = IDV.Caption;
		}
		Caption = text;
		if (text2 == null)
		{
			text2 = IDV.CaptionToolTip();
		}
		ToolTip = text2;
	}

	public void PopulateDynamicColumns()
	{
		Dictionary<string, double> dictionary = new Dictionary<string, double>();
		bool columnHeadersShowIn2Lines = false;
		double totalsPanelHeight = 0.0;
		double totalsPanelRightWidth = 0.0;
		IDV.SetTransactionsPanelsDefaultSize(ref totalsPanelHeight, ref totalsPanelRightWidth);
		TotalsPanelHeight = totalsPanelHeight;
		TotalsPanelRightWidth = totalsPanelRightWidth;
		if (!string.IsNullOrWhiteSpace(IDV.Name))
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey($"{Settings.RegistryKey}\\{"DataView"}\\{IDV.Name}\\{"Columns"}", writable: false);
			if (registryKey != null)
			{
				string[] valueNames = registryKey.GetValueNames();
				foreach (string text in valueNames)
				{
					if (double.TryParse(registryKey.GetValue(text, "0").ToString(), out var result) && result > 0.0)
					{
						dictionary.Add(text, result);
					}
				}
				IsAutoWidthMode = Convert.ToBoolean(registryKey.GetValue("AutoWidth"));
				columnHeadersShowIn2Lines = Convert.ToBoolean(registryKey.GetValue("ColHeadersIn2Lines"));
				_groupingInfoJson = registryKey.GetValue("Grouping")?.ToString();
				double result2 = TotalsPanelRightWidth;
				if (double.TryParse(registryKey.GetValue("TotalsPanelRightWidth", TotalsPanelRightWidth).ToString(), out result2))
				{
					TotalsPanelRightWidth = result2;
				}
				result2 = TotalsPanelHeight;
				if (double.TryParse(registryKey.GetValue("TotalsPanelHeight", TotalsPanelHeight).ToString(), out result2))
				{
					TotalsPanelHeight = result2;
				}
				if (_groupingInfoJson == null)
				{
					_groupingInfoJson = "";
				}
				string text2 = registryKey.GetValue("TransactionsControlDefaultPosition")?.ToString();
				if (text2 == null)
				{
					showTotalsAtRight = IDV.TransactionsControlDefaultPosition != "Bottom";
				}
				else
				{
					showTotalsAtRight = text2 != "Bottom";
				}
			}
		}
		if (TotalsPanelRightWidth > 0.0)
		{
			TotalsPanelRight.ItemWidth = new GridLength(TotalsPanelRightWidth);
		}
		if (TotalsPanelHeight > 0.0)
		{
			TotalsPanel.ItemHeight = new GridLength(TotalsPanelHeight);
		}
		ColumnHeadersShowIn2Lines = columnHeadersShowIn2Lines;
		Grid.Columns.BeginUpdate();
		List<string> list = new List<string>();
		for (int num = Grid.Columns.Count - 1; num >= 0; num--)
		{
			if (((MyGridColumn)Grid.Columns[num]).IsDynamic)
			{
				list.Add(Grid.Columns[num].FieldName);
				Grid.Columns.Remove(Grid.Columns[num]);
			}
		}
		for (int num2 = Grid.TotalSummary.Count - 1; num2 >= 0; num2--)
		{
			if (list.Contains(Grid.TotalSummary[num2].FieldName))
			{
				Grid.TotalSummary.Remove(Grid.TotalSummary[num2]);
			}
		}
		for (int num3 = Grid.GroupSummary.Count - 1; num3 >= 0; num3--)
		{
			if (list.Contains(Grid.GroupSummary[num3].FieldName))
			{
				Grid.GroupSummary.Remove(Grid.GroupSummary[num3]);
			}
		}
		int num4 = Grid.Columns.Count;
		foreach (ViewColumn columnView in IDV.Columns.ViewList.Where((ViewColumn p) => p.IsDynamic))
		{
			GridColumn gridColumn = Grid.Columns.FirstOrDefault((GridColumn p) => p.Name == columnView.Name);
			if (gridColumn != null)
			{
				if (!string.IsNullOrWhiteSpace(columnView.After))
				{
					gridColumn.VisibleIndex = Grid.Columns[columnView.After].VisibleIndex;
				}
				else if (!string.IsNullOrWhiteSpace(columnView.Before))
				{
					gridColumn.VisibleIndex = Grid.Columns[columnView.Before].VisibleIndex - 1;
				}
			}
			else
			{
				CreateGridColumn(columnView.Name, columnView.Name, columnView.Caption, (short)IDV.GetColumnType(columnView.Name), columnView.IsDynamic, columnView.IsDynamic, columnView.IsReference, columnView.IsUnbound, columnView.UnboundExpression, columnView.Visible, columnView.Format, columnView.FooterSum, columnView.Converter, columnView.After, columnView.Before, num4, dictionary);
				num4++;
			}
		}
		int num5 = 0;
		foreach (GridColumn item in from p in Grid.Columns
			orderby p.VisibleIndex, ((MyGridColumn)p).ColumnVisibleIndex
			select p)
		{
			item.VisibleIndex = num5;
			((MyGridColumn)item).ColumnVisibleIndex = num5;
			num5++;
		}
		Grid.Columns.EndUpdate();
	}

	private void PopulateColumns()
	{
		Dictionary<string, double> dictionary = new Dictionary<string, double>();
		bool columnHeadersShowIn2Lines = false;
		double totalsPanelHeight = 0.0;
		double totalsPanelRightWidth = 0.0;
		IDV.SetTransactionsPanelsDefaultSize(ref totalsPanelHeight, ref totalsPanelRightWidth);
		TotalsPanelHeight = totalsPanelHeight;
		TotalsPanelRightWidth = totalsPanelRightWidth;
		if (!string.IsNullOrWhiteSpace(IDV.Name))
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey($"{Settings.RegistryKey}\\{"DataView"}\\{IDV.Name}\\{"Columns"}", writable: false);
			if (registryKey != null)
			{
				string[] valueNames = registryKey.GetValueNames();
				foreach (string text in valueNames)
				{
					if (double.TryParse(registryKey.GetValue(text, "0").ToString(), out var result) && result > 0.0)
					{
						dictionary.Add(text, result);
					}
				}
				IsAutoWidthMode = Convert.ToBoolean(registryKey.GetValue("AutoWidth"));
				columnHeadersShowIn2Lines = Convert.ToBoolean(registryKey.GetValue("ColHeadersIn2Lines"));
				_groupingInfoJson = registryKey.GetValue("Grouping")?.ToString();
				double result2 = TotalsPanelRightWidth;
				if (double.TryParse(registryKey.GetValue("TotalsPanelRightWidth", TotalsPanelRightWidth).ToString(), out result2))
				{
					TotalsPanelRightWidth = result2;
				}
				result2 = TotalsPanelHeight;
				if (double.TryParse(registryKey.GetValue("TotalsPanelHeight", TotalsPanelHeight).ToString(), out result2))
				{
					TotalsPanelHeight = result2;
				}
				if (_groupingInfoJson == null)
				{
					_groupingInfoJson = "";
				}
				string text2 = registryKey.GetValue("TransactionsControlDefaultPosition")?.ToString();
				if (text2 == null)
				{
					showTotalsAtRight = IDV.TransactionsControlDefaultPosition != "Bottom";
				}
				else
				{
					showTotalsAtRight = text2 != "Bottom";
				}
			}
		}
		if (TotalsPanelRightWidth > 0.0)
		{
			TotalsPanelRight.ItemWidth = new GridLength(TotalsPanelRightWidth);
		}
		if (TotalsPanelHeight > 0.0)
		{
			TotalsPanel.ItemHeight = new GridLength(TotalsPanelHeight);
		}
		ColumnHeadersShowIn2Lines = columnHeadersShowIn2Lines;
		Grid.Columns.BeginUpdate();
		int num = 0;
		foreach (MyGridColumn column in Grid.Columns)
		{
			column.ColumnVisibleIndex = num;
			num++;
		}
		foreach (ViewColumn view in IDV.Columns.ViewList)
		{
			CreateGridColumn(view.Name, view.Name, view.Caption, (short)IDV.GetColumnType(view.Name), setColumnDynamic: false, view.IsDynamic, view.IsReference, view.IsUnbound, view.UnboundExpression, view.Visible, view.Format, view.FooterSum, view.Converter, "", "", num, dictionary);
			num++;
		}
		Grid.Columns.EndUpdate();
	}

	public MyGridColumn PopulateColumn(UnboundColumn columnView)
	{
		Grid.Columns.BeginUpdate();
		int visibleIndex = 0;
		MyGridColumn result = CreateGridColumn(columnView.Name, columnView.Name, columnView.Caption, columnView.Type, setColumnDynamic: false, isDynamic: false, isReference: false, isUnbound: true, columnView.Expression, visible: false, DecimalExtension.GetCurrencyFormat(16, columnView.Precision), hasFooterSum: true, null, "", "", visibleIndex, new Dictionary<string, double>());
		Grid.Columns.EndUpdate();
		return result;
	}

	public MyGridColumn CreateGridColumn(string name, string source, string caption, short type, bool setColumnDynamic, bool isDynamic, bool isReference, bool isUnbound, string unboundExpression, bool visible, string format, bool hasFooterSum, IValueConverter converter, string afterColumn, string beforeColumn, int visibleIndex, Dictionary<string, double> colWidths)
	{
		MyGridColumn myGridColumn = new MyGridColumn
		{
			ColumnVisibleIndex = visibleIndex,
			IsSelected = true,
			Name = name,
			FieldName = source,
			Header = caption,
			Visible = visible,
			AllowEditing = DefaultBoolean.False,
			ReadOnly = true
		};
		if (setColumnDynamic)
		{
			myGridColumn.IsDynamic = true;
			myGridColumn.VisibleIndex = visibleIndex;
		}
		if (isUnbound)
		{
			myGridColumn.AllowUnboundExpressionEditor = true;
			myGridColumn.UnboundExpression = unboundExpression;
		}
		if (_isAutoFilterRowOldStyle)
		{
			myGridColumn.AutoFilterCondition = AutoFilterCondition.Contains;
		}
		myGridColumn.AllowColumnFiltering = DefaultBoolean.True;
		if (_isFilterPopupModeExcel)
		{
			myGridColumn.FilterPopupMode = FilterPopupMode.ExcelSmart;
		}
		else
		{
			myGridColumn.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		if (ColumnHeadersShowIn2Lines)
		{
			myGridColumn.HeaderTemplate = (DataTemplate)Resources["Column2RowHeaderTemplate"];
		}
		else
		{
			myGridColumn.HeaderTemplate = null;
		}
		if (format == "Color")
		{
			myGridColumn.CellTemplate = Resources["ColorConverterTemplate"] as DataTemplate;
			myGridColumn.UnboundType = UnboundColumnType.Object;
			myGridColumn.AllowEditing = DefaultBoolean.False;
		}
		if (converter != null)
		{
			Binding binding = new Binding(name)
			{
				Converter = converter,
				Mode = BindingMode.TwoWay
			};
			myGridColumn.Binding = binding;
			if (format == "Image")
			{
				myGridColumn.CellTemplate = Resources["ColumnToImage"] as DataTemplate;
				myGridColumn.UnboundType = UnboundColumnType.Object;
				myGridColumn.AllowEditing = DefaultBoolean.False;
			}
			myGridColumn.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		else
		{
			switch (type)
			{
			case 5:
				if (format == "LargeImage")
				{
					ImageEditSettings editSettings = new ImageEditSettings
					{
						MaxWidth = 100.0
					};
					myGridColumn.EditSettings = editSettings;
				}
				else
				{
					myGridColumn.CellTemplate = Resources["ButtonColumn"] as DataTemplate;
					myGridColumn.AllowEditing = DefaultBoolean.True;
				}
				myGridColumn.UnboundType = UnboundColumnType.Object;
				break;
			case 0:
			{
				if (_isAutoFilterRowOldStyle)
				{
					myGridColumn.AutoFilterCondition = AutoFilterCondition.Contains;
				}
				else
				{
					myGridColumn.AutoFilterCriteria = ClauseType.Contains;
				}
				TextEditSettings textEditSettings = (TextEditSettings)(myGridColumn.EditSettings = new TextEditSettings());
				if (string.IsNullOrEmpty(format))
				{
					myGridColumn.EditSettings.DisplayFormat = format;
				}
				textEditSettings.HorizontalContentAlignment = EditSettingsHorizontalAlignment.Left;
				if (isDynamic | isReference | isUnbound)
				{
					myGridColumn.UnboundType = UnboundColumnType.String;
					myGridColumn.UnboundDataType = typeof(string);
				}
				break;
			}
			case 1:
				myGridColumn.EditSettings = new DateEditSettings();
				if (string.IsNullOrEmpty(format))
				{
					myGridColumn.EditSettings.DisplayFormat = "dd/MM/yy HH:mm:ss";
				}
				else
				{
					myGridColumn.EditSettings.DisplayFormat = format;
				}
				myGridColumn.EditSettings.HorizontalContentAlignment = EditSettingsHorizontalAlignment.Left;
				if (isDynamic | isReference | isUnbound)
				{
					myGridColumn.UnboundType = UnboundColumnType.DateTime;
					myGridColumn.UnboundDataType = typeof(DateTime);
				}
				myGridColumn.Width = SizeCalculator.WidthForDate(useLongDate: false);
				myGridColumn.IsColumnWidthSpecified = true;
				break;
			case 7:
				myGridColumn.EditSettings = new DateEditSettings();
				if (string.IsNullOrEmpty(format))
				{
					myGridColumn.EditSettings.DisplayFormat = "dd/MM/yy";
				}
				else
				{
					myGridColumn.EditSettings.DisplayFormat = format;
				}
				myGridColumn.EditSettings.HorizontalContentAlignment = EditSettingsHorizontalAlignment.Left;
				if (isDynamic | isReference | isUnbound)
				{
					myGridColumn.Name = myGridColumn.FieldName;
					myGridColumn.UnboundType = UnboundColumnType.DateTime;
					myGridColumn.UnboundDataType = typeof(DateOnly);
				}
				myGridColumn.Width = SizeCalculator.WidthForDate(useLongDate: false);
				myGridColumn.IsColumnWidthSpecified = true;
				break;
			case 3:
				myGridColumn.EditSettings = new TextEditSettings();
				IDV.ConditionalFormattingList.Add(new AsCondititionFormat
				{
					Expression = "[" + name + "] < 0",
					ApplyToRow = false,
					FieldName = name,
					FormatName = "Custom",
					CustomFormat = new UserDefinedFormat
					{
						ForegroundColor = ColorsAS.Foreground.EditorError
					},
					IsFixed = true
				});
				if (string.IsNullOrEmpty(format))
				{
					myGridColumn.EditSettings.DisplayFormat = DecimalExtension.GetCurrencyFormat(16, 0);
				}
				else
				{
					myGridColumn.EditSettings.DisplayFormat = format;
				}
				myGridColumn.EditSettings.HorizontalContentAlignment = EditSettingsHorizontalAlignment.Right;
				if (isDynamic | isReference | isUnbound)
				{
					myGridColumn.UnboundType = UnboundColumnType.Integer;
					myGridColumn.UnboundDataType = typeof(int);
				}
				break;
			case 2:
				myGridColumn.EditSettings = new TextEditSettings();
				IDV.ConditionalFormattingList.Add(new AsCondititionFormat
				{
					Expression = "[" + name + "] < 0",
					ApplyToRow = false,
					FieldName = name,
					FormatName = "Custom",
					CustomFormat = new UserDefinedFormat
					{
						ForegroundColor = ColorsAS.Foreground.EditorError
					},
					IsFixed = true
				});
				if (string.IsNullOrEmpty(format))
				{
					myGridColumn.EditSettings.DisplayFormat = DecimalExtension.GetCurrencyFormat(16, 2);
				}
				else
				{
					myGridColumn.EditSettings.DisplayFormat = format;
				}
				myGridColumn.Width = SizeCalculator.WidthForNumber(16);
				myGridColumn.IsColumnWidthSpecified = true;
				myGridColumn.EditSettings.HorizontalContentAlignment = EditSettingsHorizontalAlignment.Right;
				if (isDynamic | isReference | isUnbound)
				{
					myGridColumn.UnboundType = UnboundColumnType.Decimal;
					myGridColumn.UnboundDataType = typeof(decimal);
				}
				break;
			case 4:
				myGridColumn.IsBooleanBased = true;
				if (Settings.IsWin11Theme)
				{
					myGridColumn.CellTemplate = (DataTemplate)Settings.GenericResources["GridBooleanCellWin11Template"];
				}
				else
				{
					myGridColumn.CellTemplate = (DataTemplate)Settings.GenericResources["GridBooleanCellTemplate"];
				}
				if (isDynamic | isReference | isUnbound)
				{
					myGridColumn.UnboundType = UnboundColumnType.Boolean;
					myGridColumn.UnboundDataType = typeof(bool);
				}
				if (!_isAutoFilterRowOldStyle)
				{
					myGridColumn.ShowCriteriaInAutoFilterRow = false;
				}
				break;
			default:
				myGridColumn.UnboundType = UnboundColumnType.Object;
				myGridColumn.UnboundDataType = typeof(object);
				break;
			}
		}
		myGridColumn.HeaderCustomizationAreaTemplate = (DataTemplate)Resources["ColumnTemplate"];
		myGridColumn.TotalSummaryContentStyle = (System.Windows.Style)Resources["SelectedSummaryStyle"];
		((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(ColumnBase.SortIndexProperty, typeof(MyGridColumn))).AddValueChanged((object)myGridColumn, (EventHandler)SortIndexChanged);
		if (colWidths.ContainsKey(myGridColumn.Name.Trim()))
		{
			myGridColumn.Width = colWidths[myGridColumn.Name.Trim()];
			_columnsWidthRestoredFromRegister = true;
		}
		Grid.Columns.Add(myGridColumn);
		if (!string.IsNullOrWhiteSpace(afterColumn))
		{
			myGridColumn.VisibleIndex = Grid.Columns[afterColumn].VisibleIndex;
		}
		else if (!string.IsNullOrWhiteSpace(beforeColumn))
		{
			myGridColumn.VisibleIndex = Grid.Columns[beforeColumn].VisibleIndex - 1;
		}
		if (type == 2 || type == 3)
		{
			if (!FindTotalSummary(myGridColumn.FieldName, selectedSummary: true))
			{
				GridSummaryItem gridSummaryItem = new GridSummaryItem();
				TreeListSummarySettings.SetIsRecursive((DependencyObject)(object)gridSummaryItem, true);
				gridSummaryItem.FieldName = myGridColumn.FieldName;
				gridSummaryItem.DisplayFormat = myGridColumn.EditSettings.DisplayFormat;
				gridSummaryItem.Tag = "SelectedSum";
				gridSummaryItem.SummaryType = SummaryItemType.Sum;
				gridSummaryItem.CalculationMode = GridSummaryCalculationMode.SelectedRows;
				gridSummaryItem.Visible = hasFooterSum;
				Grid.TotalSummary.Add(gridSummaryItem);
			}
			if (!FindTotalSummary(myGridColumn.FieldName, selectedSummary: false))
			{
				GridSummaryItem item = new GridSummaryItem
				{
					FieldName = myGridColumn.FieldName,
					DisplayFormat = myGridColumn.EditSettings.DisplayFormat,
					SummaryType = SummaryItemType.Sum,
					CalculationMode = GridSummaryCalculationMode.AllRows,
					ShowInColumn = myGridColumn.Name,
					Visible = hasFooterSum
				};
				Grid.GroupSummary.Add(item);
				GridSummaryItem gridSummaryItem2 = new GridSummaryItem
				{
					Tag = null
				};
				TreeListSummarySettings.SetIsRecursive((DependencyObject)(object)gridSummaryItem2, false);
				gridSummaryItem2.FieldName = myGridColumn.FieldName;
				gridSummaryItem2.DisplayFormat = myGridColumn.EditSettings.DisplayFormat;
				gridSummaryItem2.SummaryType = SummaryItemType.Sum;
				gridSummaryItem2.CalculationMode = GridSummaryCalculationMode.AllRows;
				Grid.TotalSummary.Add(gridSummaryItem2);
				gridSummaryItem2.Visible = hasFooterSum;
			}
		}
		return myGridColumn;
	}

	private void SetTotalSummariesSettings()
	{
		foreach (GridSummaryItem item in Grid.TotalSummary)
		{
			if (item.CalculationMode == GridSummaryCalculationMode.SelectedRows)
			{
				TreeListSummarySettings.SetIsRecursive((DependencyObject)(object)item, false);
			}
			else
			{
				TreeListSummarySettings.SetIsRecursive((DependencyObject)(object)item, true);
			}
		}
		Grid.UpdateTotalSummary();
	}

	internal void LoadLayouts()
	{
		if (_arrLayouts == null)
		{
			_arrLayouts = new List<GridLayoutItem>();
		}
		else
		{
			_arrLayouts.Clear();
		}
		if (_arrLayoutsGroupBy == null)
		{
			_arrLayoutsGroupBy = new List<GridLayoutItem>();
		}
		else
		{
			_arrLayoutsGroupBy.Clear();
		}
		if (string.IsNullOrWhiteSpace(IDV.Name))
		{
			return;
		}
		try
		{
			List<LayoutRow> list = Settings.LayoutService.GetList(IDV.Name);
			if (list == null)
			{
				return;
			}
			foreach (LayoutRow item in list)
			{
				if (item.LayoutType != 4)
				{
					_arrLayouts.Add(new GridLayoutItem
					{
						Caption = item.Caption,
						Tag = item.Caption,
						Type = (DataViewTypes)item.LayoutType,
						Layout = item.Body
					});
				}
				else
				{
					_arrLayoutsGroupBy.Add(new GridLayoutItem
					{
						Caption = item.Caption,
						Tag = item.Caption,
						Type = (DataViewTypes)item.LayoutType,
						Layout = item.Body
					});
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public void FillDetail()
	{
		DetailsPanelSc.Document.Blocks.Clear();
		Paragraph paragraph = new Paragraph
		{
			FontSize = Settings.GetFontSize()
		};
		paragraph.Inlines.Add(new Run(ReportTitle));
		DetailsPanelSc.Document.Blocks.Add(paragraph);
	}

	public void BindData()
	{
		ClearSelection();
		Grid.ItemsSource = IDV.Data;
	}

	public object Row(int rowHandle)
	{
		try
		{
			return Grid.GetRow(rowHandle);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public object FocusedRow()
	{
		return Grid.GetFocusedRow();
	}

	public void Update(object key)
	{
		List<int> list = null;
		try
		{
			Grid.BeginDataUpdate();
			list = IDV.LoadUpdatedRows(key, this);
			UpdateByRowIndexes(list);
		}
		finally
		{
			Grid.EndDataUpdate();
			if (list != null)
			{
				UpdateRowsFocusAndSelection(list);
			}
		}
	}

	public void Update(List<object> keys)
	{
		List<int> list = null;
		try
		{
			Grid.BeginDataUpdate();
			list = IDV.LoadUpdatedRows(keys, this);
			UpdateByRowIndexes(list);
		}
		finally
		{
			Grid.EndDataUpdate();
			if (list != null)
			{
				UpdateRowsFocusAndSelection(list);
			}
		}
	}

	private void UpdateByRowIndexes(List<int> rowIndexes)
	{
		foreach (int rowIndex in rowIndexes)
		{
			if (rowIndex >= 0)
			{
				try
				{
					int rowHandleByListIndex = Grid.GetRowHandleByListIndex(rowIndex);
					IDV.LoadReferenceColumns(Grid.GetRow(rowHandleByListIndex));
				}
				catch (Exception)
				{
					return;
				}
			}
		}
		ShowTotalsPanel();
		Grid.UpdateTotalSummary();
		Grid.RefreshData();
	}

	private void UpdateRowsFocusAndSelection(List<int> rowIndexes)
	{
		rowIndexes = rowIndexes.Where((int p) => p >= 0).ToList();
		int count = rowIndexes.Count;
		if (count > 0)
		{
			int rowHandleByListIndex = Grid.GetRowHandleByListIndex(rowIndexes[count - 1]);
			Grid.View.FocusedRowHandle = rowHandleByListIndex;
		}
		if (selectedRows.Count <= 0)
		{
			return;
		}
		Grid.BeginSelection();
		foreach (int rowIndex in rowIndexes)
		{
			if (selectedRows.Contains(rowIndex))
			{
				int rowHandleByListIndex2 = Grid.GetRowHandleByListIndex(rowIndex);
				if (!Grid.View.IsRowSelected(rowHandleByListIndex2))
				{
					Grid.SelectItem(rowHandleByListIndex2);
				}
			}
		}
		Grid.EndSelection();
	}

	public List<object> MatchedRows(object key)
	{
		return IDV.MatchedRows(key);
	}

	public void Add()
	{
		if (_shownGridMenu != null)
		{
			_shownGridMenu.ClosePopup();
		}
		IDV.Add();
	}

	public void Edit()
	{
		if (FocusedRow() != null)
		{
			if (_shownGridMenu != null)
			{
				_shownGridMenu.ClosePopup();
			}
			IDV.Edit();
		}
	}

	public void View()
	{
		if (FocusedRow() != null)
		{
			if (_shownGridMenu != null)
			{
				_shownGridMenu.ClosePopup();
			}
			IDV.View();
		}
	}

	public void PrintPreview(PrintMode? mode = null, bool showDialog = false)
	{
		if (FocusedRow() == null)
		{
			return;
		}
		if (Settings.SystemType == SystemTypes.SalesManagement && !mode.HasValue)
		{
			showDialog = true;
		}
		int selectedCount = GetSelectedCount();
		bool insertPageBreaks = true;
		TemplatesCacheRow template = null;
		List<int> rows = GetSelectedOrFocusedRowsHandle();
		IReadOnlyList<TemplatesCacheRow> readOnlyList = IDV.PrintTemplates();
		if (showDialog && readOnlyList != null && (readOnlyList.Count > 1 || selectedCount > 1))
		{
			PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog(readOnlyList, IDV.DefaultPrintTemplate(), selectedCount > 0);
			if (printPreviewDialog.ShowDialog().Value)
			{
				Mouse.OverrideCursor = Cursors.Wait;
				insertPageBreaks = printPreviewDialog.InsertPageBreaks;
				template = printPreviewDialog.PrintTemplate;
				mode = TemplateSubstitutionService.ConvertFileFormatToPrintMode(template.FileFormat);
				if (!printPreviewDialog.PrintSelectedRows)
				{
					int num = 1;
					List<int> list = new List<int>(num);
					CollectionsMarshal.SetCount(list, num);
					CollectionsMarshal.AsSpan(list)[0] = Grid.View.FocusedRowHandle;
					rows = list;
				}
				IDV.PreviewSelected(rows, mode, template, insertPageBreaks);
				Mouse.OverrideCursor = null;
			}
		}
		else
		{
			Mouse.OverrideCursor = Cursors.Wait;
			IDV.PreviewSelected(rows, mode, template, insertPageBreaks);
			Mouse.OverrideCursor = null;
		}
	}

	public bool ExportToXML(string fileName = "")
	{
		if (!GetSelectedAny() && FocusedRow() == null)
		{
			return false;
		}
		if (!IDV.ExistExchangeableRow)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.XMLExportNotSupported);
		}
		try
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				fileName = Settings.CommonCoreHelper.GetFileNameFromExportDialog(".xml", "XML Files (.xml)|*.xml", "Export_" + DateTime.Now.ToString("MMdd_HHmm"));
			}
			Mouse.OverrideCursor = Cursors.Wait;
			if (!string.IsNullOrWhiteSpace(fileName))
			{
				IDV.ExportToXML(fileName, null);
			}
			Mouse.OverrideCursor = null;
			return true;
		}
		catch (Exception ex)
		{
			ex.ThrowStorageException(fileName);
			return false;
		}
	}

	public void Delete()
	{
		if (FocusedRow() != null)
		{
			if (_shownGridMenu != null)
			{
				_shownGridMenu.ClosePopup();
			}
			IDV.Delete();
		}
	}

	public void CreateCopy()
	{
		if (FocusedRow() != null)
		{
			if (_shownGridMenu != null)
			{
				_shownGridMenu.ClosePopup();
			}
			IDV.CreateCopy();
		}
	}

	public void DocumentHistory()
	{
		if (FocusedRow() != null)
		{
			if (_shownGridMenu != null)
			{
				_shownGridMenu.ClosePopup();
			}
			IDV.DocumentHistory();
		}
	}

	public void ShowDialog()
	{
		if (IDV.ShowDialog(useParamValues: true, isRefreshMode: true, null))
		{
			IDV.ApplyDataViewDefinitionParameters();
			RefreshInner(RefreshType.ChangeParams);
			SetBarStates();
		}
	}

	public void Refresh(bool suppressLoadIndication = false)
	{
		RefreshInner(RefreshType.Refresh, suppressLoadIndication);
	}

	private void RefreshInner(RefreshType refreshType, bool suppressLoadIndication = false)
	{
		bool value = ShowTransactionsCheckItem.IsChecked.Value;
		ShowTransactionsMode = false;
		if (InRefresh != RefreshType.None)
		{
			return;
		}
		InRefresh = refreshType;
		PanelIsLoaded = false;
		IDV.BeforeLoadData(this);
		IDV.AddDirectoryLinksToColumns(this);
		if (!suppressLoadIndication && IDV.LoadingIndication == LoadIndication.Always)
		{
			IDV.ShowProgressWindow(this);
		}
		else if (!suppressLoadIndication && IDV.LoadingIndication == LoadIndication.AlwaysInside)
		{
			IDV.ShowProgressWindowInside(this);
		}
		else
		{
			Mouse.OverrideCursor = Cursors.Wait;
			Grid.BeginDataUpdate();
			try
			{
				IDV.LoadData();
				IDV.AfterLoadData(this);
				IDV.LoadReferenceColumns();
				PanelIsLoaded = true;
			}
			finally
			{
				ClearSelection();
				Grid.EndDataUpdate();
			}
			Mouse.OverrideCursor = null;
			InRefresh = RefreshType.None;
		}
		SetCaption();
		BindData();
		FillDetail();
		FitColumns();
		ShowTransactionsMode = value;
		ShowTotalsPanel();
		LoadLayouts();
		string filterString = Grid.FilterString;
		Grid.FilterString = "";
		if (Grid.View == TreeView && IDV.ShowExpanded())
		{
			TreeView.ExpandAllNodes();
		}
		Grid.FilterString = filterString;
		((DispatcherObject)Settings.HostingEnvironment).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
		{
			Mouse.OverrideCursor = null;
		}), (DispatcherPriority)6, (object[])null);
	}

	private void ShowTotalsPanel()
	{
		if (ShowTransactionsMode)
		{
			if (PanelIsLoaded)
			{
				IDV.ShowTransactions(showTotalsAtRight);
				if (!showTotalsAtRight)
				{
					TotalsPanelRight.Content = null;
					TotalsPanel.Content = TransactionsControl;
					TotalsPanel.Visibility = Visibility.Visible;
					TotalsPanelRight.Visibility = Visibility.Collapsed;
				}
				else
				{
					TotalsPanel.Content = null;
					TotalsPanelRight.Content = TransactionsControl;
					TotalsPanelRight.Visibility = Visibility.Visible;
					TotalsPanel.Visibility = Visibility.Collapsed;
				}
			}
			else
			{
				TotalsPanel.Visibility = Visibility.Collapsed;
				TotalsPanelRight.Visibility = Visibility.Collapsed;
			}
		}
		else
		{
			TotalsPanel.Visibility = Visibility.Collapsed;
			TotalsPanelRight.Visibility = Visibility.Collapsed;
		}
	}

	private void Grid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
	{
		GetGridContextMenu();
		if (GridContextMenu != null)
		{
			GridContextMenu.Placement = PlacementMode.MousePoint;
		}
		BarManager.SetDXContextMenu(Grid, GridContextMenu);
		ContextMenuManager.ShowElementContextMenu(Grid);
		e.Handled = true;
	}

	private void Grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
	{
		try
		{
			if (e.Column.Name == "Selected")
			{
				if (e.IsGetData)
				{
					if (selectedRows.Count != 0)
					{
						e.Value = selectedRows.Contains(e.ListSourceRowIndex);
					}
					else
					{
						e.Value = false;
					}
				}
			}
			else if (e.Column.Name == "IsLeaf")
			{
				if (e.IsGetData)
				{
					e.Value = true;
				}
			}
			else if (e.IsGetData)
			{
				if (!e.Column.AllowUnboundExpressionEditor)
				{
					IDynamicValue dynamicValue = (IDynamicValue)IDV.Data[e.ListSourceRowIndex];
					e.Value = dynamicValue.GetDynamicValue(e.Column.FieldName);
				}
				else if (e.Value is UnboundErrorObject)
				{
					e.Value = ArmSoft.SME.Common.Resources.UnboundErrorObjectValue;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void Tree_CustomUnboundColumnData(object sender, TreeListUnboundColumnDataEventArgs e)
	{
		try
		{
			if (e.Node.RowHandle < 0)
			{
				return;
			}
			if (e.Column.Name == "Selected")
			{
				if (e.IsGetData)
				{
					e.Value = TreeView.IsRowSelected(e.Node.RowHandle);
				}
			}
			else if (e.Column.Name == "IsLeaf")
			{
				if (e.IsGetData)
				{
					if (string.IsNullOrWhiteSpace(IDV.IsLeafFieldName()))
					{
						e.Value = !e.Node.HasChildren;
					}
					else
					{
						e.Value = Grid.GetCellValue(e.Node.RowHandle, IDV.IsLeafFieldName());
					}
				}
			}
			else if (e.IsGetData && e.Column.Visible)
			{
				IDynamicValue dynamicValue = (IDynamicValue)IDV.Data[e.Node.RowHandle];
				e.Value = dynamicValue.GetDynamicValue(e.Column.FieldName);
			}
		}
		catch (Exception)
		{
		}
	}

	public void ClearSelection()
	{
		Grid.UnselectAll();
		if (Grid.View == TreeView)
		{
			TreeView.UncheckAllNodes();
		}
		foreach (int dataRowHandle in GetDataRowHandles())
		{
			int listIndexByRowHandle = Grid.GetListIndexByRowHandle(dataRowHandle);
			selectedRows.Remove(listIndexByRowHandle);
		}
	}

	public void RowDeleted(int index)
	{
		if (selectedRows.Count == 0)
		{
			return;
		}
		_rowSelectedChanged = false;
		if (selectedRows.Remove(index))
		{
			_selectedRowHandle = Grid.GetRowHandleByListIndex(index);
		}
		List<int> list = new List<int>();
		foreach (int selectedRow in selectedRows)
		{
			if (selectedRow >= index)
			{
				list.Add(selectedRow - 1);
			}
			else
			{
				list.Add(selectedRow);
			}
		}
		selectedRows.Clear();
		foreach (int item in list)
		{
			selectedRows.Add(item);
		}
		UpdateSelected(rowMode: false);
		ShowOnStatusBar();
	}

	private void Grid_KeyEnter(object sender, KeyEventArgs e)
	{
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Invalid comparison between Unknown and I4
		if (HotKeys.Single.Enter.Matches(this, e))
		{
			int focusedRowHandle = Grid.View.FocusedRowHandle;
			if (focusedRowHandle >= 0)
			{
				bool flag = IsRowSelected(focusedRowHandle);
				ExecuteDefaultFunction();
				if (flag)
				{
					SelectItem(focusedRowHandle);
				}
				Grid.RefreshRow(focusedRowHandle);
			}
			e.Handled = true;
		}
		else if (HotKeys.Functional.F5.Matches(this, e) && AllowPreview)
		{
			PrintPreview();
			e.Handled = true;
		}
		else if (HotKeys.WithModifier.Control_I.Matches(this, e))
		{
			ReduceRows();
			e.Handled = true;
		}
		else if (HotKeys.WithModifier.Shift_F2.Matches(this, e))
		{
			int focusedRowHandle2 = Grid.View.FocusedRowHandle;
			if (Grid.View.NavigationStyle == GridViewNavigationStyle.Cell)
			{
				Grid.View.NavigationStyle = GridViewNavigationStyle.Row;
				Grid.SelectionMode = MultiSelectMode.MultipleRow;
				Grid.ClipboardCopyMode = ClipboardCopyMode.Default;
			}
			else
			{
				Grid.View.NavigationStyle = GridViewNavigationStyle.Cell;
				Grid.SelectionMode = MultiSelectMode.Cell;
				Grid.ClipboardCopyMode = ClipboardCopyMode.ExcludeHeader;
			}
			Grid.View.FocusedRowHandle = focusedRowHandle2;
			if (Grid.View == TreeView)
			{
				Grid.CurrentColumn = TreeView.VisibleColumns[0];
			}
			else if (Grid.View == GridView)
			{
				Grid.CurrentColumn = GridView.VisibleColumns[1];
			}
			SetBarStates();
			e.Handled = true;
		}
		else if ((HotKeys.WithModifier.Control_Up.Matches(this, e) || HotKeys.WithModifier.Control_Down.Matches(this, e)) && GetSelectedAny())
		{
			int focusedRowHandle3 = Grid.View.FocusedRowHandle;
			int rowVisibleIndexByHandle = Grid.GetRowVisibleIndexByHandle(focusedRowHandle3);
			int num;
			int num2;
			if ((int)e.Key == 24)
			{
				num = 0;
				num2 = -1;
				rowVisibleIndexByHandle--;
			}
			else
			{
				num = Grid.VisibleRowCount - 1;
				rowVisibleIndexByHandle++;
				num2 = 1;
			}
			for (int i = rowVisibleIndexByHandle; (num2 >= 0) ? (i <= num) : (i >= num); i += num2)
			{
				int rowHandleByVisibleIndex = Grid.GetRowHandleByVisibleIndex(i);
				if (rowHandleByVisibleIndex >= 0 && Grid.View.IsRowSelected(rowHandleByVisibleIndex))
				{
					Grid.View.FocusedRowHandle = rowHandleByVisibleIndex;
					break;
				}
			}
			e.Handled = true;
		}
		else if (HotKeys.Functional.F2.Matches(this, e))
		{
			OnF2();
			e.Handled = true;
		}
		else
		{
			e.Handled = false;
		}
	}

	public void ReduceRows()
	{
		if (!GetSelectedAny())
		{
			return;
		}
		Mouse.OverrideCursor = Cursors.Wait;
		if (Grid.ItemsSource is IList list)
		{
			Grid.BeginDataUpdate();
			for (int num = list.Count - 1; num >= 0; num--)
			{
				if (!selectedRows.Contains(num))
				{
					list.RemoveAt(num);
				}
			}
			selectedRows.Clear();
			Grid.EndDataUpdate();
			Grid.SelectAll();
			for (int i = 0; i <= list.Count - 1; i++)
			{
				selectedRows.Add(i);
			}
			if (Grid.View == TreeView)
			{
				TreeView.CheckAllNodes();
			}
		}
		else if (Grid.ItemsSource is DataTable dataTable)
		{
			Grid.BeginDataUpdate();
			for (int num2 = dataTable.Rows.Count - 1; num2 >= 0; num2--)
			{
				if (!selectedRows.Contains(num2))
				{
					dataTable.Rows.RemoveAt(num2);
				}
			}
			Grid.EndDataUpdate();
			selectedRows.Clear();
			Grid.SelectAll();
			for (int j = 0; j <= dataTable.Rows.Count - 1; j++)
			{
				selectedRows.Add(j);
			}
		}
		ClearFilter();
		Grid.RefreshData();
		Mouse.OverrideCursor = null;
	}

	private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Invalid comparison between Unknown and I4
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Invalid comparison between Unknown and I4
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Invalid comparison between Unknown and I4
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Invalid comparison between Unknown and I4
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Invalid comparison between Unknown and I4
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Invalid comparison between Unknown and I4
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Invalid comparison between Unknown and I4
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Expected I4, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Invalid comparison between Unknown and I4
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Expected I4, but got Unknown
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Invalid comparison between Unknown and I4
		Grid_KeyEnter(sender, e);
		if (Grid.View.SearchControl != null && Grid.View.SearchControl.GetIsKeyboardFocusWithin())
		{
			return;
		}
		if (Grid.View.FocusedRowHandle == -2147483645 || !Grid.View.IsKeyboardFocusWithinView)
		{
			e.Handled = false;
		}
		else if (Grid.SelectionMode == MultiSelectMode.Cell)
		{
			e.Handled = false;
		}
		else
		{
			if (!(Grid.View is IPanelView))
			{
				return;
			}
			e.Handled = true;
			if (HotKeys.Single.Insert.Matches(this, e) || HotKeys.Single.Space.Matches(this, e) || HotKeys.WithModifier.Shift_Down.Matches(this, e) || HotKeys.WithModifier.Shift_Up.Matches(this, e) || HotKeys.WithModifier.Shift_Insert.Matches(this, e))
			{
				if (!_selectByKeyboard)
				{
					Grid.BeginSelection();
					_selectByKeyboard = true;
				}
				int visibleRowCount = Grid.VisibleRowCount;
				int focusedRowHandle = Grid.View.FocusedRowHandle;
				int listIndexByRowHandle = Grid.GetListIndexByRowHandle(focusedRowHandle);
				if (focusedRowHandle < 0)
				{
					return;
				}
				if (Grid.View == TreeView)
				{
					TreeListNode nodeByRowHandle = TreeView.GetNodeByRowHandle(focusedRowHandle);
					nodeByRowHandle.IsChecked = !nodeByRowHandle.IsChecked;
					if (Convert.ToBoolean(nodeByRowHandle.IsChecked))
					{
						Grid.SelectItem(nodeByRowHandle.RowHandle);
						selectedRows.Add(listIndexByRowHandle);
					}
					else
					{
						selectedRows.Remove(listIndexByRowHandle);
						Grid.UnselectItem(nodeByRowHandle.RowHandle);
					}
				}
				else if (!Grid.View.IsRowSelected(focusedRowHandle))
				{
					selectedRows.Add(listIndexByRowHandle);
					Grid.SelectItem(focusedRowHandle);
				}
				else
				{
					selectedRows.Remove(listIndexByRowHandle);
					Grid.UnselectItem(focusedRowHandle);
				}
				if (Grid.View == GridView)
				{
					Grid.EndSelection();
				}
				if (!string.IsNullOrWhiteSpace(Grid.FilterString))
				{
					Grid.RefreshRow(focusedRowHandle);
				}
				int visibleRowCount2 = Grid.VisibleRowCount;
				if (visibleRowCount == visibleRowCount2)
				{
					if (HotKeys.WithModifier.Shift_Up.Matches(this, e) || HotKeys.WithModifier.Shift_Insert.Matches(this, e))
					{
						Grid.View.MovePrevRow();
					}
					else
					{
						Grid.View.MoveNextRow();
					}
				}
			}
			else if (HotKeys.Single.Multiply.Matches(this, e))
			{
				_selectByKeyboard = true;
				Mouse.OverrideCursor = Cursors.Wait;
				Grid.BeginSelection();
				if (Grid.View == GridView)
				{
					for (int i = 0; i <= Grid.VisibleRowCount - 1; i++)
					{
						int rowHandleByVisibleIndex = Grid.GetRowHandleByVisibleIndex(i);
						if (Grid.IsGroupRowHandle(rowHandleByVisibleIndex))
						{
							continue;
						}
						int listIndexByRowHandle2 = Grid.GetListIndexByRowHandle(rowHandleByVisibleIndex);
						if (!selectedRows.Contains(listIndexByRowHandle2))
						{
							Grid.SelectItem(rowHandleByVisibleIndex);
							if (Grid.View == TreeView)
							{
								TreeView.GetNodeByVisibleIndex(i).IsChecked = true;
							}
							selectedRows.Add(listIndexByRowHandle2);
						}
						else
						{
							Grid.UnselectItem(rowHandleByVisibleIndex);
							if (Grid.View == TreeView)
							{
								TreeView.GetNodeByVisibleIndex(i).IsChecked = false;
							}
							selectedRows.Remove(listIndexByRowHandle2);
						}
					}
				}
				else if (Grid.View == TreeView)
				{
					foreach (TreeListNode node in TreeView.Nodes)
					{
						node.IsChecked = !node.IsChecked;
						if (Convert.ToBoolean(node.IsChecked))
						{
							Grid.SelectItem(node.RowHandle);
						}
						else
						{
							Grid.UnselectItem(node.RowHandle);
						}
						ReverseNodesChecked(node.Nodes);
					}
				}
				Grid.EndSelection();
				if (!string.IsNullOrWhiteSpace(Grid.FilterString))
				{
					Grid.RefreshData();
					Grid.UpdateTotalSummary();
				}
				UpdateSelectedStatus();
				Mouse.OverrideCursor = null;
				_selectByKeyboard = false;
				Mouse.OverrideCursor = null;
			}
			else if (HotKeys.Single.OemPlus.Matches(this, e) || HotKeys.Single.Add.Matches(this, e))
			{
				SelectAll();
			}
			else if (HotKeys.Single.OemMinus.Matches(this, e) || HotKeys.Single.Subtract.Matches(this, e))
			{
				DeselectAll();
			}
			else if (HotKeys.WithModifier.Control_A.Matches(this, e))
			{
				if (Grid.GetSelectedRowHandles().Length == Grid.VisibleRowCount)
				{
					DeselectAll();
				}
				else
				{
					SelectAll();
				}
			}
			else if (Grid.View is IPanelView panelView && panelView.GetVisibleColumns().Any() && (((int)e.Key >= 75 && (int)e.Key <= 83) || ((int)e.Key >= 35 && (int)e.Key <= 43)) && ((int)Keyboard.Modifiers == 2 || (int)Keyboard.Modifiers == 6))
			{
				_selectByKeyboard = false;
				if (Grid.View.FocusedRowHandle == -2147483645)
				{
					e.Handled = false;
					return;
				}
				int num = (((int)e.Key < 75 || (int)e.Key > 83) ? (e.Key - 35) : (e.Key - 75));
				if (panelView.GetVisibleColumns().ElementAt(0).Name == "Selected" || Grid.View == GridView)
				{
					num++;
				}
				if (panelView.GetVisibleColumns().Count() > num)
				{
					GridColumn gridColumn = (GridColumn)panelView.GetVisibleColumns().ElementAt(num);
					ColumnSortOrder sortOrder = gridColumn.SortOrder;
					if ((int)Keyboard.Modifiers == 2)
					{
						RemoveSort();
					}
					gridColumn.SortOrder = sortOrder;
					if (gridColumn.IsSorted && gridColumn.SortOrder == ColumnSortOrder.Ascending)
					{
						gridColumn.SortOrder = ColumnSortOrder.Descending;
					}
					else if (gridColumn.IsSorted && gridColumn.SortOrder == ColumnSortOrder.Descending)
					{
						gridColumn.SortOrder = ColumnSortOrder.None;
					}
					else if (!gridColumn.IsSorted)
					{
						gridColumn.SortOrder = ColumnSortOrder.Ascending;
					}
					Grid.RefreshData();
				}
			}
			else
			{
				e.Handled = false;
			}
		}
	}

	private void SelectAll()
	{
		_selectByKeyboard = true;
		Mouse.OverrideCursor = Cursors.Wait;
		Grid.BeginSelection();
		if (Grid.View == TreeView)
		{
			foreach (TreeListNode node in TreeView.Nodes)
			{
				node.IsChecked = true;
				Grid.SelectItem(node.RowHandle);
				RecursiveNodesCheck(node.Nodes);
				selectedRows.Add(node.RowHandle);
			}
		}
		else
		{
			Grid.SelectAll();
			int[] selectedRowHandles = Grid.GetSelectedRowHandles();
			foreach (int rowHandle in selectedRowHandles)
			{
				int listIndexByRowHandle = Grid.GetListIndexByRowHandle(rowHandle);
				if (!selectedRows.Contains(listIndexByRowHandle) && !Grid.IsGroupRowHandle(rowHandle))
				{
					selectedRows.Add(listIndexByRowHandle);
				}
			}
		}
		Grid.EndSelection();
		if (!string.IsNullOrWhiteSpace(Grid.FilterString))
		{
			Grid.RefreshData();
			Grid.UpdateTotalSummary();
		}
		Mouse.OverrideCursor = null;
		_selectByKeyboard = false;
		UpdateSelectedStatus();
	}

	private void DeselectAll()
	{
		_selectByKeyboard = true;
		Mouse.OverrideCursor = Cursors.Wait;
		Grid.BeginSelection();
		Grid.UnselectAll();
		if (Grid.View == TreeView)
		{
			TreeView.UncheckAllNodes();
		}
		selectedRows.Clear();
		Grid.EndSelection();
		if (!string.IsNullOrWhiteSpace(Grid.FilterString))
		{
			Grid.RefreshData();
		}
		Mouse.OverrideCursor = null;
		_selectByKeyboard = false;
		Grid.UpdateTotalSummary();
		UpdateSelectedStatus();
	}

	private void ReverseNodesChecked(TreeListNodeCollection nodes)
	{
		if (nodes == null || nodes.Count == 0)
		{
			return;
		}
		foreach (TreeListNode node in nodes)
		{
			node.IsChecked = !node.IsChecked;
			if (node.IsChecked.Value)
			{
				Grid.SelectItem(node.RowHandle);
			}
			else
			{
				Grid.UnselectItem(node.RowHandle);
			}
			ReverseNodesChecked(node.Nodes);
		}
	}

	private void RecursiveNodesCheck(TreeListNodeCollection nodes)
	{
		if (nodes == null || nodes.Count == 0)
		{
			return;
		}
		foreach (TreeListNode node in nodes)
		{
			node.IsChecked = true;
			Grid.SelectItem(node.RowHandle);
			RecursiveNodesCheck(node.Nodes);
			selectedRows.Add(node.RowHandle);
		}
	}

	public void AddRibbonMenuGroup(string code, string caption, string smallImage = "")
	{
		CategoryRibbon.AddGroup(code, caption, smallImage);
	}

	public void AddContextMenuGroup(string code, string caption, string smallImage = "")
	{
		ContextRibbon.AddGroup(code, caption, smallImage);
	}

	public bool Get_AllowExecuteContextFunction(string item)
	{
		if (_dwpFunctions.TryGetValue(item, out var value))
		{
			return value.BtnItem.IsEnabled;
		}
		return false;
	}

	public void Set_AllowExecuteContextFunction(string item, bool isEnabled)
	{
		if (_dwpFunctions.TryGetValue(item, out var value) && !value.IsRibbonItem)
		{
			value.BtnItem.IsEnabled = isEnabled;
		}
	}

	private void SetDefaultFunction(string item)
	{
		if (_dwpFunctions.TryGetValue(item, out var value))
		{
			_defaultFunction = value.BtnItem.Name;
		}
	}

	internal void AddRibbonFunction(string group, string code, string caption, ButtonClickFunction funcRef, string smallImage = "", string largeImage = "", bool showInContextMenu = false)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		if (CategoryRibbon.FillingCompleted)
		{
			return;
		}
		BarButtonItem barButtonItem = new BarButtonItem
		{
			Content = caption,
			Name = code
		};
		if (!string.IsNullOrWhiteSpace(smallImage))
		{
			ImageSource image = ImageService.GetImage(smallImage);
			if (image != null)
			{
				barButtonItem.Glyph = image;
			}
		}
		if (!string.IsNullOrWhiteSpace(largeImage))
		{
			ImageSource image2 = ImageService.GetImage(largeImage);
			if (image2 != null)
			{
				barButtonItem.LargeGlyph = image2;
			}
		}
		_dwpFunctions.Add(code, new DwpMenuItem(barButtonItem, isRibbon: true, funcRef, showInContextMenu));
		barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ExecuteUserFunction).Handler;
		CategoryRibbon.AddFunction(group, (BarButtonItemLink)barButtonItem.CreateLink());
	}

	private void AddUserDefinedContextFunction(string name, string image)
	{
		if (string.IsNullOrWhiteSpace(IDV.DataViewGroup))
		{
			return;
		}
		List<string> list = Settings.ServiceProvider.GetRequiredService<IContextFunctionService>().Load(IDV.DataViewGroup);
		if (list == null || list.Count <= 0)
		{
			return;
		}
		AddContextMenuGroup(name, ArmSoft.SME.Common.Resources.MakeUserOperation, image);
		foreach (string item in list)
		{
			ContextFunctionDefinition contextFunction = Settings.ServiceProvider.GetRequiredService<IContextFunctionService>().Load(IDV.DataViewGroup, item);
			BarButtonItem barButtonItem = new BarButtonItem
			{
				Content = contextFunction.Caption,
				Name = contextFunction.Name
			};
			barButtonItem.ItemClick += (object s, ItemClickEventArgs e) =>
			{
				GetUserDefinedContextFunctionItemHandler(contextFunction);
			};
			ContextRibbon.AddFunction(name, (BarButtonItemLink)barButtonItem.CreateLink());
		}
	}

	public List<T> GetSelectedOrFocusedRows<T>()
	{
		List<T> list = new List<T>();
		if (GetSelectedAny())
		{
			list = GetSelectedRows<T>();
		}
		else
		{
			list.Add((T)FocusedRow());
		}
		return list;
	}

	public void AddLinkContextFunction(string group, string code, string caption, ButtonClickFunction funcRef, bool storeOpContext = false, bool isEnabled = true)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (!ContextRibbon.FillingCompleted)
		{
			BarButtonItem barButtonItem = new BarButtonItem
			{
				ContentTemplate = (DataTemplate)Resources["LinkFunctionTemplate"],
				Content = caption,
				Name = code
			};
			ImageSource image = ImageService.GetImage("open_link_16.svg");
			if (image != null)
			{
				barButtonItem.Glyph = image;
			}
			_dwpFunctions.Add(code, new DwpMenuItem(barButtonItem, isRibbon: false, funcRef, showInMenu: true, storeOpContext, isEnabled));
			barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ExecuteUserFunction).Handler;
			ContextRibbon.AddFunction(group, (BarButtonItemLink)barButtonItem.CreateLink());
		}
	}

	public void AddContextFunction(string group, string code, string caption, ButtonClickFunction funcRef, string smallImage = "", string largeImage = "", bool storeOpContext = false, bool isEnabled = true, DataTemplate contentTemplate = null, bool isNew = false)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		if (ContextRibbon.FillingCompleted)
		{
			return;
		}
		BarButtonItem barButtonItem = new BarButtonItem
		{
			Content = caption,
			Name = code
		};
		if (isNew)
		{
			barButtonItem.ContentTemplate = (DataTemplate)Resources["NewFunctionTemplate"];
		}
		if (contentTemplate != null)
		{
			barButtonItem.ContentTemplate = contentTemplate;
		}
		if (!string.IsNullOrWhiteSpace(smallImage))
		{
			ImageSource image = ImageService.GetImage(smallImage);
			if (image != null)
			{
				barButtonItem.Glyph = image;
			}
		}
		if (!string.IsNullOrWhiteSpace(largeImage))
		{
			ImageSource image2 = ImageService.GetImage(largeImage);
			if (image2 != null)
			{
				barButtonItem.LargeGlyph = image2;
			}
		}
		_dwpFunctions.Add(code, new DwpMenuItem(barButtonItem, isRibbon: false, funcRef, showInMenu: true, storeOpContext, isEnabled));
		barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ExecuteUserFunction).Handler;
		ContextRibbon.AddFunction(group, (BarButtonItemLink)barButtonItem.CreateLink());
	}

	public void AddContextSeparator(string group)
	{
		ContextRibbon.AddSeparator(group);
	}

	private void ExecuteUserFunction(object sender, ItemClickEventArgs e)
	{
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		try
		{
			if (!(Grid.ItemsSource is DataTable) && sender is BarButtonItem barButtonItem)
			{
				if (_shownGridMenu != null)
				{
					_shownGridMenu.ClosePopup();
					Grid.View.Focus();
				}
				if (barButtonItem.IsEnabled && _dwpFunctions.TryGetValue(barButtonItem.Name, out var value))
				{
					value.WindowCaption = IDV.Caption;
					CallsRecordingHelper.RecordCallStep(StatisticTypeSE.ContextFunction, IDV.Name + " - " + barButtonItem.Name);
					((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)value.Invoker(), Array.Empty<object>());
				}
			}
		}
		catch (Exception ex)
		{
			Settings.HostingEnvironment.CurrentViewPanel = currentViewPanel;
			Settings.ErrorHandler(ex);
		}
	}

	private void TableView_ShowFilterPopup(object sender, FilterPopupEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		if (e.Column.FieldName == "EntryExists")
		{
			List<object> list = new List<object>();
			DXImage displayValue = new DXImage
			{
				Width = 16.0,
				Height = 16.0,
				Source = ImageService.GetImage("warning_circle_16.svg")
			};
			list.Add(new CustomComboBoxItem
			{
				DisplayValue = null,
				EditValue = ""
			});
			list.Add(new CustomComboBoxItem
			{
				DisplayValue = displayValue,
				EditValue = ArmSoft.SME.Common.Resources.EntriesManual
			});
			displayValue = new DXImage
			{
				Width = 16.0,
				Height = 16.0,
				Source = ImageService.GetImage("bullet_yellow_16.svg")
			};
			list.Add(new CustomComboBoxItem
			{
				DisplayValue = displayValue,
				EditValue = ArmSoft.SME.Common.Resources.EntriesSystemAndDraft
			});
			displayValue = new DXImage
			{
				Width = 16.0,
				Height = 16.0,
				Source = ImageService.GetImage("accept_16.svg")
			};
			list.Add(new CustomComboBoxItem
			{
				DisplayValue = displayValue,
				EditValue = ArmSoft.SME.Common.Resources.System
			});
			e.ComboBoxEdit.ItemsSource = list;
		}
	}

	private void GetGridContextMenu()
	{
		if (Grid.View.FocusedRowHandle < 0 && Grid.VisibleRowCount > 0)
		{
			GridContextMenu = null;
			return;
		}
		try
		{
			GridContextMenu?.ItemLinks.Clear();
			GridContextMenu = new PopupMenu();
			_shownGridMenu = GridContextMenu;
			GridContextMenu.Placement = PlacementMode.MousePoint;
			string defaultFunction = IDV.GetDefaultFunction();
			if (string.IsNullOrWhiteSpace(defaultFunction))
			{
				_defaultFunction = DataViewEditBarButtonItem.Name;
				defaultFunction = _defaultFunction;
			}
			if (Grid.SelectionMode == MultiSelectMode.Cell)
			{
				if (Grid.GetSelectedRowHandles().Length != 0)
				{
					GridContextMenu.ItemLinks.Add(_cmdCopy.CreateLink());
				}
				return;
			}
			BarButtonItemLink barButtonItemLink = (BarButtonItemLink)DataViewAddBarButtonItem.CreateLink();
			if (barButtonItemLink.IsEnabled)
			{
				if (defaultFunction == barButtonItemLink.BarItemName)
				{
					barButtonItemLink.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
				}
				GridContextMenu.ItemLinks.Add(barButtonItemLink);
			}
			barButtonItemLink = (BarButtonItemLink)DataViewEditBarButtonItem.CreateLink();
			if (barButtonItemLink.IsEnabled)
			{
				if (defaultFunction == barButtonItemLink.BarItemName)
				{
					barButtonItemLink.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
				}
				GridContextMenu.ItemLinks.Add(barButtonItemLink);
			}
			barButtonItemLink = (BarButtonItemLink)DataViewDocumentBarButtonItem.CreateLink();
			if (barButtonItemLink.IsEnabled)
			{
				if (defaultFunction == barButtonItemLink.BarItemName)
				{
					barButtonItemLink.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
				}
				GridContextMenu.ItemLinks.Add(barButtonItemLink);
			}
			barButtonItemLink = (BarButtonItemLink)DataViewDeleteBarButtonItem.CreateLink();
			if (barButtonItemLink.IsEnabled)
			{
				if (defaultFunction == barButtonItemLink.BarItemName)
				{
					barButtonItemLink.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
				}
				GridContextMenu.ItemLinks.Add(barButtonItemLink);
			}
			barButtonItemLink = (BarButtonItemLink)DataViewCreateCopyBarButtonItem.CreateLink();
			if (barButtonItemLink.IsEnabled)
			{
				if (defaultFunction == barButtonItemLink.BarItemName)
				{
					barButtonItemLink.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
				}
				GridContextMenu.ItemLinks.Add(barButtonItemLink);
			}
			if (DocumentHistoryBarButtonItem != null)
			{
				barButtonItemLink = (BarButtonItemLink)DocumentHistoryBarButtonItem.CreateLink();
				if (barButtonItemLink.IsEnabled)
				{
					GridContextMenu.ItemLinks.Add(new BarItemLinkSeparator());
					if (defaultFunction == barButtonItemLink.BarItemName)
					{
						barButtonItemLink.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
					}
					GridContextMenu.ItemLinks.Add(barButtonItemLink);
				}
			}
			AsRibbon categoryRibbon = CategoryRibbon;
			if (!categoryRibbon.IsEmpty())
			{
				foreach (RibbonPageGroup group in categoryRibbon.Groups)
				{
					if (!AnyVisibleItem(group))
					{
						continue;
					}
					GridContextMenu.ItemLinks.Add(new BarItemLinkSeparator());
					foreach (BarItemLinkBase itemLink in group.ItemLinks)
					{
						if (itemLink.IsEnabled && _dwpFunctions[itemLink.Item.Name].ShowInContextMenu)
						{
							if (defaultFunction == itemLink.BarItemName)
							{
								itemLink.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
							}
							GridContextMenu.ItemLinks.Add(itemLink);
						}
					}
				}
			}
			ContextRibbon.FillingCompleted = false;
			if (Grid.View != null && Settings.HostingEnvironment.CurrentViewPanel != null)
			{
				ClearContextFunctions();
				IDV.InitContextFunctions();
				IDV.InitCoreContextFunctions();
			}
			AddUserDefinedContextFunction("UserOperation", "action_16.svg");
			ContextRibbon.FillingCompleted = true;
			categoryRibbon = ContextRibbon;
			if (!categoryRibbon.IsEmpty())
			{
				if (categoryRibbon.Groups[0].ItemLinks.Any())
				{
					GridContextMenu.ItemLinks.Add(new BarItemLinkSeparator());
					foreach (BarItemLinkBase itemLink2 in categoryRibbon.Groups[0].ItemLinks)
					{
						if (itemLink2 is BarItemLinkSeparator)
						{
							GridContextMenu.ItemLinks.Add(new BarItemLinkSeparator());
							continue;
						}
						if (defaultFunction == itemLink2.BarItemName)
						{
							itemLink2.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
						}
						GridContextMenu.ItemLinks.Add(itemLink2.Item.CreateLink());
					}
				}
				foreach (RibbonPageGroup group2 in categoryRibbon.Groups)
				{
					if (!(group2.Name != "Root") || !group2.ItemLinks.Any())
					{
						continue;
					}
					GridContextMenu.ItemLinks.Add(new BarItemLinkSeparator());
					BarSubItem barSubItem = new BarSubItem
					{
						Content = group2.Caption,
						Glyph = group2.Glyph
					};
					foreach (BarItemLinkBase itemLink3 in group2.ItemLinks)
					{
						if (itemLink3 is BarItemLinkSeparator)
						{
							barSubItem.ItemLinks.Add(new BarItemLinkSeparator());
						}
						else
						{
							barSubItem.ItemLinks.Add(itemLink3.Item);
						}
					}
					GridContextMenu.ItemLinks.Add(barSubItem.CreateLink());
				}
			}
			if (_isInGrid || _isInFilterPanel)
			{
				GridContextMenu.ItemLinks.Add(new BarItemLinkSeparator());
			}
			if (_isInGrid)
			{
				barButtonItemLink = (BarButtonItemLink)_cmdCopyCurrent.CreateLink();
				barButtonItemLink.Item.Content = string.Format(ArmSoft.SME.Common.Resources.CopyValue, _currentCellDisplayText.Trim());
				barButtonItemLink.Item.ContentTemplate = (DataTemplate)Resources["CopyTemplate"];
				GridContextMenu.ItemLinks.Add(barButtonItemLink);
				barButtonItemLink = (BarButtonItemLink)_cmdFilterCurrent.CreateLink();
				barButtonItemLink.Item.Content = string.Format(ArmSoft.SME.Common.Resources.FilterBy, " = ", _currentCellDisplayText.Trim());
				barButtonItemLink.Item.ContentTemplate = (DataTemplate)Resources["FilterTemplate"];
				GridContextMenu.ItemLinks.Add(barButtonItemLink);
				barButtonItemLink = (BarButtonItemLink)_cmdFilterNotEqualCurrent.CreateLink();
				barButtonItemLink.Item.Content = string.Format(ArmSoft.SME.Common.Resources.FilterBy, " ≠ ", _currentCellDisplayText.Trim());
				barButtonItemLink.Item.ContentTemplate = (DataTemplate)Resources["FilterTemplate"];
				GridContextMenu.ItemLinks.Add(barButtonItemLink);
				if (_showGridMenu_qnt != 0)
				{
					_isInGrid = false;
				}
			}
			if (_isInFilterPanel)
			{
				barButtonItemLink = (BarButtonItemLink)_cmdCopyFilter.CreateLink();
				GridContextMenu.ItemLinks.Add(barButtonItemLink);
				if (_showGridMenu_qnt != 0)
				{
					_isInFilterPanel = false;
				}
			}
			if (_showGridMenu_qnt == 0)
			{
				_showGridMenu_qnt = 1;
			}
			else
			{
				_showGridMenu_qnt = 0;
			}
			BarManager.SetDXContextMenu(Grid, GridContextMenu);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FilterByValue(object sender, ItemClickEventArgs e)
	{
		if (_currentCellValue != null && !string.IsNullOrEmpty(_currentColumnName))
		{
			if (Grid.FilterString == "" && Grid.IsFilterEnabled)
			{
				Grid.FilterString = "[" + _currentColumnName + "] ='" + _currentCellValue.ToString() + "'";
			}
			else
			{
				Grid.FilterString = Grid.FilterString + " And [" + _currentColumnName + "] ='" + _currentCellValue.ToString() + "'";
			}
		}
	}

	private void FilterByNotEqualValue(object sender, ItemClickEventArgs e)
	{
		if (_currentCellValue != null && !string.IsNullOrEmpty(_currentColumnName))
		{
			if (Grid.FilterString == "" && Grid.IsFilterEnabled)
			{
				Grid.FilterString = "[" + _currentColumnName + "] !='" + _currentCellValue.ToString() + "'";
			}
			else
			{
				Grid.FilterString = Grid.FilterString + " And [" + _currentColumnName + "] !='" + _currentCellValue.ToString() + "'";
			}
		}
	}

	private void CopyByValue()
	{
		if (_currentCellValue != null && !string.IsNullOrEmpty(_currentColumnName))
		{
			Clipboard.SetDataObject(_currentCellValue.ToString());
		}
	}

	private bool AnyVisibleItem(RibbonPageGroup grp)
	{
		foreach (BarItemLinkBase itemLink in grp.ItemLinks)
		{
			if (itemLink is BarButtonItemLink && itemLink.IsEnabled && _dwpFunctions[itemLink.Item.Name].ShowInContextMenu)
			{
				return true;
			}
		}
		return false;
	}

	private void ClearContextFunctions()
	{
		ContextRibbon = null;
		ContextRibbon = new AsRibbon();
		if (_dwpFunctions == null)
		{
			return;
		}
		for (int num = _dwpFunctions.Count - 1; num >= 0; num--)
		{
			if (!_dwpFunctions[_dwpFunctions.Keys.ElementAtOrDefault(num)].IsRibbonItem)
			{
				_dwpFunctions.Remove(_dwpFunctions.Keys.ElementAtOrDefault(num));
			}
		}
	}

	public void FitColumns()
	{
		if (InRefresh == RefreshType.Refresh || _columnsWidthRestoredFromRegister || !(Grid.View is IPanelView panelView))
		{
			return;
		}
		foreach (GridColumn column in Grid.Columns)
		{
			if (!(column is MyGridColumn { IsColumnWidthSpecified: not false }))
			{
				panelView.BestFitColumn(column);
			}
		}
	}

	private void ExecuteDefaultFunction()
	{
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		try
		{
			string defaultFunction = IDV.GetDefaultFunction();
			if (string.IsNullOrWhiteSpace(defaultFunction))
			{
				_defaultFunction = DataViewEditBarButtonItem.Name;
				defaultFunction = _defaultFunction;
			}
			else
			{
				_defaultFunction = defaultFunction;
			}
			CallsRecordingHelper.RecordCallStep(StatisticTypeSE.ContextFunction, IDV.Name + " - " + _defaultFunction);
			ContextRibbon.FillingCompleted = false;
			if (Grid.View != null && Settings.HostingEnvironment.CurrentViewPanel != null)
			{
				ClearContextFunctions();
				IDV.InitContextFunctions();
				IDV.InitCoreContextFunctions();
			}
			ContextRibbon.FillingCompleted = true;
			SetDefaultFunction(defaultFunction);
			if (_dwpFunctions.TryGetValue(_defaultFunction, out var value))
			{
				if (value.BtnItem.IsEnabled)
				{
					((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)value.Invoker(), Array.Empty<object>());
				}
			}
			else if (_defaultFunction == AddFunction.Item.Name)
			{
				Add();
			}
			else if (_defaultFunction == EditFunction.Item.Name)
			{
				if (AllowEdit)
				{
					Edit();
				}
				else if (AllowView)
				{
					View();
				}
			}
			else if (_defaultFunction == DeleteFunction.Item.Name)
			{
				Delete();
			}
			else if (_defaultFunction == CreateCopyFunction.Item.Name)
			{
				CreateCopy();
			}
			else if (_defaultFunction == PreviewFunction.Item.Name)
			{
				PrintPreview();
			}
			else if (_defaultFunction == DocumentHistoryFunction.Item.Name)
			{
				DocumentHistory();
			}
		}
		catch (Exception ex)
		{
			Settings.HostingEnvironment.CurrentViewPanel = currentViewPanel;
			Settings.ErrorHandler(ex);
		}
	}

	private void GridRow_MouseDoubleClick(object sender, RowDoubleClickEventArgs e)
	{
		int rowHandle = e.HitInfo.RowHandle;
		if (rowHandle != int.MinValue && rowHandle >= 0)
		{
			e.Handled = true;
			bool flag = IsRowSelected(rowHandle);
			ExecuteDefaultFunction();
			if (flag)
			{
				SelectItem(rowHandle);
			}
		}
	}

	private void TableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		ShowOnStatusBar();
		ShowTotalsPanel();
	}

	public void UpdateSelectedStatus()
	{
		_statusBarSelectedRows.Content = string.Format(ArmSoft.SME.Common.Resources.StatusSelectedRows, GetSelectedCount());
		if (Settings.HostingEnvironment.MainWindowRibbon().IsMergedChild(RibbonControl))
		{
			Settings.HostingEnvironment.StatusBarSelectedRows(_statusBarSelectedRows.Content);
		}
	}

	public void ShowOnStatusBar()
	{
		try
		{
			int focusedRowHandle = Grid.View.FocusedRowHandle;
			_statusBarSelectedRows.Content = string.Format(ArmSoft.SME.Common.Resources.StatusSelectedRows, GetSelectedCount());
			if (Settings.HostingEnvironment.MainWindowRibbon().IsMergedChild(RibbonControl))
			{
				Settings.HostingEnvironment.StatusBarSelectedRows(_statusBarSelectedRows.Content);
			}
			if (-2147483645 == focusedRowHandle || int.MinValue == focusedRowHandle)
			{
				_statusBarCurrentRow.Content = string.Format(ArmSoft.SME.Common.Resources.StatusRows, 0, Grid.VisibleRowCount);
			}
			else if (Grid.VisibleRowCount == 0)
			{
				_statusBarCurrentRow.Content = string.Format(ArmSoft.SME.Common.Resources.StatusRows, 0, 0);
			}
			else
			{
				_statusBarCurrentRow.Content = string.Format(ArmSoft.SME.Common.Resources.StatusRows, Grid.GetRowVisibleIndexByHandle(focusedRowHandle) + 1, Grid.VisibleRowCount);
			}
			if (Settings.HostingEnvironment.MainWindowRibbon().IsMergedChild(RibbonControl))
			{
				Settings.HostingEnvironment.StatusBarCurrentRow(_statusBarCurrentRow.Content);
			}
			AdjustLevelInBarItem();
		}
		catch (Exception)
		{
		}
	}

	public void Set_ColumnGroupSummary(string fieldName, SummaryItemType value)
	{
		bool flag = false;
		foreach (GridSummaryItem item2 in Grid.GroupSummary)
		{
			if (item2.FieldName == fieldName && item2.SummaryType == value)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			GridSummaryItem item = new GridSummaryItem
			{
				FieldName = fieldName,
				Visible = true,
				SummaryType = value
			};
			Grid.GroupSummary.Add(item);
		}
	}

	public void GroupByColumns(string columnsNames)
	{
		columnsNames = columnsNames.Replace(" ", "");
		string[] array = columnsNames.Split(",");
		for (int i = 0; i < array.Length; i++)
		{
			Grid.GroupBy(array[i], mergeWithPreviousGroup: true);
		}
	}

	public void GroupBy(string columnName, bool merge)
	{
		Grid.GroupBy(columnName, merge);
	}

	public void RemoveGrouping()
	{
		if (Grid.View == GridView)
		{
			while (GridView.GroupedColumns.Count > 0)
			{
				int count = GridView.GroupedColumns.Count;
				Grid.UngroupBy(GridView.GroupedColumns[count - 1]);
			}
		}
		else if (Grid.View == CardView)
		{
			while (CardView.GroupedColumns.Count > 0)
			{
				int count2 = CardView.GroupedColumns.Count;
				Grid.UngroupBy(CardView.GroupedColumns[count2 - 1]);
			}
		}
	}

	public bool Get_ColumnVisible(string columnName)
	{
		foreach (GridColumn column in Grid.Columns)
		{
			if (column.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				return column.Visible;
			}
		}
		return false;
	}

	public void Set_ColumnCaption(string columnName, string value)
	{
		if (InRefresh == RefreshType.Refresh)
		{
			return;
		}
		SortedDictionary<int, string> sortedDictionary = new SortedDictionary<int, string>();
		if (!(Grid.View is IPanelView))
		{
			return;
		}
		foreach (GridColumn column in Grid.Columns)
		{
			sortedDictionary.Add(((MyGridColumn)column).ColumnVisibleIndex, column.Name);
			if (column.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				column.Header = value;
			}
		}
	}

	public void Set_ColumnVisible(string columnName, bool value)
	{
		if (InRefresh == RefreshType.Refresh)
		{
			return;
		}
		SortedDictionary<int, string> sortedDictionary = new SortedDictionary<int, string>();
		if (!(Grid.View is IPanelView))
		{
			return;
		}
		foreach (GridColumn column in Grid.Columns)
		{
			sortedDictionary.Add(((MyGridColumn)column).ColumnVisibleIndex, column.Name);
			if (column.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				if ((!column.Visible & value) && column.ShowInColumnChooser)
				{
					column.Visible = false;
				}
				else
				{
					column.Visible = value;
				}
				column.ShowInColumnChooser = value;
			}
		}
		foreach (int key in sortedDictionary.Keys)
		{
			if (Grid.Columns[sortedDictionary[key]].Visible)
			{
				Grid.Columns[sortedDictionary[key]].VisibleIndex = key;
			}
		}
	}

	public bool Get_ShowInColumnChooser(string columnName)
	{
		foreach (GridColumn column in Grid.Columns)
		{
			if (column.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				return column.ShowInColumnChooser;
			}
		}
		return false;
	}

	public void Set_ShowInColumnChooser(string columnName, bool value)
	{
		foreach (GridColumn column in Grid.Columns)
		{
			if (column.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				column.ShowInColumnChooser = value;
				break;
			}
		}
	}

	public void Set_ColumnSortOrder(string columnName, ColumnSortOrder value = ColumnSortOrder.Ascending)
	{
		foreach (GridColumn column in Grid.Columns)
		{
			if (column.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				column.SortOrder = value;
				break;
			}
		}
	}

	public bool Set_ColumnFooterSum(string columnName)
	{
		foreach (GridColumn column in Grid.Columns)
		{
			if (!column.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				continue;
			}
			foreach (GridSummaryItem item in Grid.TotalSummary)
			{
				if (item.FieldName.Equals(column.FieldName, StringComparison.CurrentCultureIgnoreCase) && item.SummaryType == SummaryItemType.Sum)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public void Set_ColumnFooterSum(string columnName, bool value)
	{
		foreach (GridColumn column in Grid.Columns)
		{
			if (!column.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				continue;
			}
			foreach (GridSummaryItem item in Grid.GroupSummary)
			{
				if (item.FieldName.Equals(column.FieldName, StringComparison.CurrentCultureIgnoreCase) && item.SummaryType == SummaryItemType.Sum)
				{
					item.Visible = value;
				}
			}
			foreach (GridSummaryItem item2 in Grid.TotalSummary)
			{
				if (item2.FieldName.Equals(column.FieldName, StringComparison.CurrentCultureIgnoreCase) && item2.SummaryType == SummaryItemType.Sum)
				{
					item2.Visible = value;
				}
			}
		}
	}

	public bool Get_ColumnSelected(string columnName)
	{
		foreach (MyGridColumn item in GridView.VisibleColumns.Cast<MyGridColumn>())
		{
			if (item.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				return item.IsSelected;
			}
		}
		return false;
	}

	public void Set_ColumnSelected(string columnName, bool value)
	{
		foreach (MyGridColumn item in GridView.VisibleColumns.Cast<MyGridColumn>())
		{
			if (item.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
			{
				item.IsSelected = value;
				break;
			}
		}
	}

	public void SetFocus()
	{
		Grid.Focusable = true;
		Keyboard.Focus(Grid.View);
	}

	public ReportGrid CreateRep(bool showReport, bool isPreview = true)
	{
		Mouse.OverrideCursor = Cursors.Wait;
		ReportGrid result = new AsTableReport
		{
			ExportName = IDV.Name
		}.CreateGroupedReport(this, showReport, isPreview);
		Mouse.OverrideCursor = null;
		return result;
	}

	public void SelectRowsByMask(bool isSelectMode = true)
	{
		if (Grid.VisibleRowCount == 0)
		{
			return;
		}
		GridRowSelectionWnd gridRowSelectionWnd = new GridRowSelectionWnd();
		gridRowSelectionWnd.ConstructUI(this, _selectConditionList, isSelectMode);
		if (gridRowSelectionWnd.ShowDialog().Value)
		{
			bool preservePreviousSelection = false;
			bool caseSensitive = false;
			CriteriaOperator filterString = gridRowSelectionWnd.GenerateFilterString2(ref preservePreviousSelection, ref caseSensitive);
			if (isSelectMode)
			{
				SelectByCondition(filterString, preservePreviousSelection);
			}
			else
			{
				SelectByCondition(filterString, preservePreviousSelection, selectMode: false);
			}
			_selectConditionList = gridRowSelectionWnd.ConditionsList();
			ShowOnStatusBar();
		}
	}

	private bool SelectedRowPredicate(int i)
	{
		if (!Grid.IsGrouped || Grid.IsGroupRowHandle(i))
		{
			if (!Grid.IsGrouped)
			{
				return Grid.IsRowVisible(i);
			}
			return false;
		}
		return true;
	}

	private bool SelectedRowInTreePredicate(int i)
	{
		return Grid.GetRowVisibleIndexByHandle(Grid.GetRowHandleByListIndex(i)) >= 0;
	}

	private List<int> GetSelectedOrFocusedRowsHandle()
	{
		if (GetSelectedAny())
		{
			return GetSelectedRows();
		}
		int num = 1;
		List<int> list = new List<int>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = Grid.View.FocusedRowHandle;
		return list;
	}

	private List<int> GetSelectedRows()
	{
		List<int> list = new List<int>();
		if (Grid.SelectionMode == MultiSelectMode.Cell)
		{
			return list;
		}
		if (Grid.View == GridView)
		{
			foreach (int item in Grid.GetSelectedRowHandles().Where(SelectedRowPredicate))
			{
				list.Add(Convert.ToInt32(item));
			}
		}
		else if (Grid.View == TreeView)
		{
			foreach (int item2 in selectedRows.Where(SelectedRowInTreePredicate))
			{
				list.Add(Grid.GetRowHandleByListIndex(item2));
			}
		}
		return list;
	}

	public List<T> GetSelectedRows<T>()
	{
		List<T> list = new List<T>();
		if (Grid.SelectionMode == MultiSelectMode.Cell)
		{
			return list;
		}
		if (Grid.View == GridView)
		{
			foreach (int item in Grid.GetSelectedRowHandles().Where(SelectedRowPredicate))
			{
				list.Add((T)Grid.GetRow(item));
			}
		}
		else if (Grid.View == TreeView)
		{
			foreach (int item2 in selectedRows.Where(SelectedRowInTreePredicate))
			{
				list.Add((T)Grid.GetRowByListIndex(item2));
			}
		}
		return list;
	}

	public int GetSelectedCount()
	{
		_visibleSelectedCount = 0;
		if (Grid.SelectionMode == MultiSelectMode.Cell)
		{
			return 0;
		}
		if (Grid.View == GridView)
		{
			_visibleSelectedCount = Grid.GetSelectedRowHandles().Count(SelectedRowPredicate);
		}
		else if (Grid.View == TreeView)
		{
			_visibleSelectedCount = Grid.GetSelectedRowHandles().Length;
		}
		return _visibleSelectedCount;
	}

	public bool GetSelectedAny()
	{
		if (Grid.SelectionMode == MultiSelectMode.Cell)
		{
			return false;
		}
		if (Grid.View == GridView)
		{
			return Grid.GetSelectedRowHandles().Any(SelectedRowPredicate);
		}
		if (Grid.View == TreeView)
		{
			return selectedRows.Any(SelectedRowInTreePredicate);
		}
		return false;
	}

	public string IdvName()
	{
		return IDV.Name;
	}

	public void ExportToExcelOrPdf(string exportFormat, string fileName = "export.xlsx", string sheetName = "Sheet1", bool showSaveDialog = true)
	{
		if (showSaveDialog && !ShowSaveDialog(exportFormat, ref fileName))
		{
			return;
		}
		ReportGrid reportGrid;
		if (exportFormat.Equals("PDF", StringComparison.CurrentCultureIgnoreCase))
		{
			if (Grid.View is CardView)
			{
				PreviewGrid(exportToPdf: true, fileName);
				return;
			}
			reportGrid = CreateRep(showReport: false);
		}
		else
		{
			reportGrid = CreateRep(showReport: false, isPreview: false);
		}
		if (reportGrid == null)
		{
			return;
		}
		try
		{
			if (exportFormat.Equals("PDF", StringComparison.CurrentCultureIgnoreCase))
			{
				PdfExportOptions options = new PdfExportOptions
				{
					ConvertImagesToJpeg = true
				};
				reportGrid.ExportToPdf(fileName, options);
			}
			else
			{
				XlsxExportOptions options2 = new XlsxExportOptions
				{
					ShowGridLines = false,
					ExportMode = XlsxExportMode.SingleFile,
					SheetName = sheetName,
					TextExportMode = TextExportMode.Value
				};
				reportGrid.ExportToXlsx(fileName, options2);
			}
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = fileName,
					UseShellExecute = true
				});
			}
			catch (Exception)
			{
				MessageBox.Show(ArmSoft.SME.Common.Resources.ExportOk + " " + fileName, ArmSoft.SME.Common.Resources.FileExport);
			}
		}
		catch (Exception ex2)
		{
			ex2.ThrowStorageException(fileName);
		}
	}

	public void ExportToExcelDataAware(string exportFormat, string fileName = "export.xlsx", string sheetName = "Sheet1", bool showSaveDialog = true, bool showFile = true)
	{
		if (showSaveDialog && !ShowSaveDialog(exportFormat, ref fileName))
		{
			return;
		}
		exportFormat = Path.GetExtension(fileName).Substring(1);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		HideSelectedRowsSummary(dictionary);
		string filterString = Grid.FilterString;
		if (GetSelectedAny())
		{
			string text = filterString;
			text = (string.IsNullOrWhiteSpace(text) ? "[Selected]=True" : (text + " and [Selected]=True "));
			Grid.FilterString = text;
		}
		try
		{
			if (exportFormat.Equals("CSV", StringComparison.CurrentCultureIgnoreCase))
			{
				CsvExportOptions options = new CsvExportOptions(";", Encoding.UTF8);
				Grid.View.ExportToCsv(fileName, options);
			}
			else
			{
				XlsxExportOptionsEx options2 = new XlsxExportOptionsEx
				{
					ShowGridLines = false,
					ExportMode = XlsxExportMode.SingleFile,
					SheetName = sheetName,
					TextExportMode = TextExportMode.Value,
					ExportType = ExportType.DataAware
				};
				Grid.View.ExportToXlsx(fileName, options2);
			}
			if (!exportFormat.Equals("CSV", StringComparison.CurrentCultureIgnoreCase))
			{
				AsSpreadSheet asSpreadSheet = new AsSpreadSheet();
				asSpreadSheet.LoadFile(fileName);
				IWorkbook document = asSpreadSheet.GetSpreadSheetControl().Document;
				document.Worksheets[0].Rows.Insert(0);
				document.Worksheets[0].Cells[0, 0].Value = IDV.Caption;
				document.Worksheets[0].Cells[0, 0].Font.Size = 16.0;
				document.Worksheets[0].Cells[0, 0].Font.Bold = true;
				document.SaveDocument(fileName + "." + exportFormat);
				long length = new FileInfo(fileName).Length;
				long length2 = new FileInfo(fileName + "." + exportFormat).Length;
				if ((length2 != 0L && (length2 >= length || length / length2 <= 10)) || 1 == 0)
				{
					File.Delete(fileName);
					File.Move(fileName + "." + exportFormat, fileName);
				}
				else
				{
					File.Delete(fileName + "." + exportFormat);
				}
			}
			if (showFile)
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = fileName,
					UseShellExecute = true
				});
			}
		}
		catch (Exception ex)
		{
			ex.ThrowStorageException(fileName);
		}
		finally
		{
			Grid.FilterString = filterString;
			foreach (GridSummaryItem value in dictionary.Values)
			{
				value.Visible = true;
			}
		}
	}

	private bool ChangeLastColumnWidth(GridColumn column, out double width)
	{
		bool result = false;
		width = column.HeaderCaption.ToString().Length * 8;
		if (width > GridView.VisibleColumns.LastOrDefault().Width.Value)
		{
			result = true;
		}
		for (int i = 0; i < Grid.VisibleRowCount; i++)
		{
			object cellValue = Grid.GetCellValue(i, column);
			if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
			{
				double num = cellValue.ToString().Length * 8;
				if (num > width)
				{
					width = num;
					result = true;
				}
			}
		}
		return result;
	}

	private void HideSelectedRowsSummary(Dictionary<string, object> dctSum)
	{
		foreach (GridSummaryItem item in Grid.TotalSummary)
		{
			if (item.SummaryType == SummaryItemType.Sum && item.CalculationMode == GridSummaryCalculationMode.SelectedRows && item.Visible)
			{
				item.Visible = false;
				dctSum.Add(item.FieldName, item);
			}
		}
	}

	private bool ShowSaveDialog(string exportFormat, ref string fileName)
	{
		SaveFileDialog saveFileDialog = new SaveFileDialog
		{
			DefaultExt = exportFormat
		};
		if (exportFormat.Equals("PDF", StringComparison.CurrentCultureIgnoreCase))
		{
			saveFileDialog.Filter = "PDF files|*.pdf";
		}
		else if (exportFormat.Equals("CSV", StringComparison.CurrentCultureIgnoreCase))
		{
			saveFileDialog.Filter = "CSV UTF-8(Comma delimited)|*.csv";
		}
		else
		{
			saveFileDialog.Filter = "Excel Workbooks|*.xlsx";
		}
		saveFileDialog.AddExtension = true;
		if (string.IsNullOrWhiteSpace(IDV.Name))
		{
			saveFileDialog.FileName = fileName;
		}
		else
		{
			saveFileDialog.FileName = IDV.Name;
		}
		saveFileDialog.OverwritePrompt = true;
		saveFileDialog.Title = "Save file";
		saveFileDialog.ValidateNames = true;
		if (saveFileDialog.ShowDialog().Value)
		{
			fileName = saveFileDialog.FileName;
			return true;
		}
		return false;
	}

	public void PreviewGrid(bool exportToPdf = false, string fileName = "export.pdf")
	{
		if (Grid.View is CardView)
		{
			Window owner = LayoutHelper.FindParentObject<Window>((DependencyObject)(object)this);
			if (IDV.GetCardPrintTemplate() == null)
			{
				CardView.PrintCardTemplate = CardDataTemplate();
			}
			PrintInfo printInfo = new PrintInfo(IdvName());
			PrintableControlLink printableControlLink = new PrintableControlLink(CardView)
			{
				ReportHeaderTemplate = (DataTemplate)Resources["ReportHeader"]
			};
			if (printInfo.PrintPageNumbers)
			{
				printableControlLink.PageFooterTemplate = (DataTemplate)Resources["PageFooter"];
			}
			ReportHeader reportHeaderData = (ReportHeader)(printableControlLink.PageHeaderData = new ReportHeader(printInfo.PrintCompanyName ? Settings.ParametersService.CompanyName() : "", (string)Caption));
			printableControlLink.ReportHeaderData = reportHeaderData;
			if (printInfo.PrintDate)
			{
				printableControlLink.PageFooterData = DateTime.Now.FormatDDMMYY();
			}
			else
			{
				printableControlLink.PageFooterData = "";
			}
			printableControlLink.Landscape = printInfo.PaperOrientation == "Landscape";
			printableControlLink.PaperKind = DXPaperKind.A4;
			if (exportToPdf)
			{
				printableControlLink.ExportToPdf(fileName);
			}
			else
			{
				printableControlLink.ShowPrintPreview(owner);
			}
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = fileName,
					UseShellExecute = true
				});
				return;
			}
			catch (Exception)
			{
				MessageBox.Show(ArmSoft.SME.Common.Resources.ExportOk + " " + fileName, ArmSoft.SME.Common.Resources.FileExport);
				return;
			}
		}
		CreateRep(showReport: true);
	}

	private DataTemplate CardDataTemplate()
	{
		DataTemplate dataTemplate = new DataTemplate();
		double num = 0.0;
		for (int i = 1; i <= Grid.Columns.Count - 1; i++)
		{
			if (Grid.Columns[i].Visible && num < Grid.Columns[i].ActualWidth)
			{
				num = Grid.Columns[i].ActualWidth;
			}
		}
		dataTemplate.DataType = IDV.RowType;
		FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(StackPanel))
		{
			Name = "myFactory"
		};
		frameworkElementFactory.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
		frameworkElementFactory.SetValue(DevExpress.Xpf.Printing.ExportSettings.TargetTypeProperty, TargetType.Panel);
		frameworkElementFactory.SetValue(DevExpress.Xpf.Printing.ExportSettings.BorderThicknessProperty, new Thickness(1.0));
		frameworkElementFactory.SetValue(DevExpress.Xpf.Printing.ExportSettings.BorderColorProperty, Colors.Gray);
		frameworkElementFactory.SetValue(FrameworkElement.MarginProperty, new Thickness(0.0, 5.0, 5.0, 5.0));
		FrameworkElementFactory frameworkElementFactory2 = new FrameworkElementFactory(typeof(Grid));
		frameworkElementFactory2.SetValue(FrameworkElement.MarginProperty, new Thickness(0.0, 5.0, 0.0, 2.0));
		FrameworkElementFactory frameworkElementFactory3 = new FrameworkElementFactory(typeof(System.Windows.Controls.ColumnDefinition));
		frameworkElementFactory3.SetValue(System.Windows.Controls.ColumnDefinition.WidthProperty, new GridLength(180.0));
		frameworkElementFactory2.AppendChild(frameworkElementFactory3);
		FrameworkElementFactory frameworkElementFactory4 = new FrameworkElementFactory(typeof(System.Windows.Controls.ColumnDefinition));
		frameworkElementFactory4.SetValue(System.Windows.Controls.ColumnDefinition.WidthProperty, new GridLength(1.0, GridUnitType.Auto));
		frameworkElementFactory2.AppendChild(frameworkElementFactory4);
		frameworkElementFactory.AppendChild(frameworkElementFactory2);
		if (num > 200.0)
		{
			num -= 200.0;
		}
		if (num < 120.0)
		{
			num = 120.0;
		}
		FontFamily fontFamily = Settings.FontFamily;
		BooleanToString converter = new BooleanToString();
		int num2 = Convert.ToInt32((GridView.VisibleColumns.Count == 0) ? TreeView.VisibleColumns.Count : GridView.VisibleColumns.Count);
		IPanelView panelView = ((GridView.VisibleColumns.Count <= 0) ? ((IPanelView)TreeView) : ((IPanelView)GridView));
		double num3 = Settings.GetFontSize() - 1.0;
		for (int j = 1; j <= num2 - 1; j++)
		{
			if (panelView.GetVisibleColumns().ElementAtOrDefault(j).Name != "Selected")
			{
				FrameworkElementFactory frameworkElementFactory5 = new FrameworkElementFactory(typeof(System.Windows.Controls.RowDefinition));
				frameworkElementFactory5.SetValue(System.Windows.Controls.RowDefinition.HeightProperty, new GridLength(1.0, GridUnitType.Auto));
				frameworkElementFactory2.AppendChild(frameworkElementFactory5);
				FrameworkElementFactory frameworkElementFactory6 = new FrameworkElementFactory(typeof(TextEdit));
				frameworkElementFactory6.SetValue(FrameworkElement.MarginProperty, new Thickness(0.0, 1.0, 1.0, 2.0));
				frameworkElementFactory6.SetValue(System.Windows.Controls.Control.FontFamilyProperty, fontFamily);
				frameworkElementFactory6.SetValue(System.Windows.Controls.Control.FontSizeProperty, num3);
				frameworkElementFactory6.SetValue(FrameworkElement.WidthProperty, 150.0);
				frameworkElementFactory6.SetValue(System.Windows.Controls.Control.VerticalContentAlignmentProperty, VerticalAlignment.Top);
				frameworkElementFactory6.SetValue(FrameworkElement.HorizontalAlignmentProperty, System.Windows.HorizontalAlignment.Stretch);
				frameworkElementFactory6.SetValue(BaseEdit.IsPrintingModeProperty, true);
				frameworkElementFactory6.SetValue(BaseEdit.EditValueProperty, panelView.GetVisibleColumns().ElementAtOrDefault(j).HeaderCaption);
				frameworkElementFactory6.SetValue(System.Windows.Controls.Grid.ColumnProperty, 0);
				frameworkElementFactory6.SetValue(System.Windows.Controls.Grid.RowProperty, j - 1);
				frameworkElementFactory6.SetValue(TextEditBase.PrintTextWrappingProperty, TextWrapping.Wrap);
				frameworkElementFactory2.AppendChild(frameworkElementFactory6);
				MyGridColumn myGridColumn = panelView.GetVisibleColumns().ElementAtOrDefault(j) as MyGridColumn;
				Binding binding = new Binding("Row." + panelView.GetVisibleColumns().ElementAtOrDefault(j).FieldName)
				{
					Mode = BindingMode.OneWay
				};
				frameworkElementFactory6 = new FrameworkElementFactory(typeof(TextEdit));
				frameworkElementFactory6.SetValue(BaseEdit.IsReadOnlyProperty, true);
				frameworkElementFactory6.SetValue(BaseEdit.IsPrintingModeProperty, true);
				frameworkElementFactory6.SetValue(System.Windows.Controls.Control.FontFamilyProperty, fontFamily);
				frameworkElementFactory6.SetValue(FrameworkElement.WidthProperty, num);
				frameworkElementFactory6.SetValue(FrameworkElement.HorizontalAlignmentProperty, System.Windows.HorizontalAlignment.Stretch);
				frameworkElementFactory6.SetValue(System.Windows.Controls.Control.VerticalContentAlignmentProperty, VerticalAlignment.Top);
				frameworkElementFactory6.SetValue(FrameworkElement.MarginProperty, new Thickness(0.0, 1.0, 2.0, 1.0));
				string value = "";
				if (IDV.GetColumnType(myGridColumn.Name) == DataColumnType.Boolean)
				{
					binding.Converter = converter;
				}
				else if (myGridColumn.EditSettings != null)
				{
					value = myGridColumn.EditSettings.DisplayFormat;
				}
				frameworkElementFactory6.SetBinding(BaseEdit.EditValueProperty, binding);
				if (!string.IsNullOrEmpty(value))
				{
					frameworkElementFactory6.SetValue(BaseEdit.DisplayFormatStringProperty, value);
				}
				frameworkElementFactory6.SetValue(System.Windows.Controls.Grid.ColumnProperty, 1);
				frameworkElementFactory6.SetValue(System.Windows.Controls.Grid.RowProperty, j - 1);
				frameworkElementFactory6.SetValue(TextEditBase.PrintTextWrappingProperty, TextWrapping.Wrap);
				frameworkElementFactory2.AppendChild(frameworkElementFactory6);
			}
		}
		dataTemplate.VisualTree = frameworkElementFactory;
		return dataTemplate;
	}

	public void PrintGrid()
	{
		try
		{
			PrintHelper.Print(CreateRep(showReport: false));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ArmSoft.SME.Common.Resources.ErrorWord + " - " + ex.Message, ArmSoft.SME.Common.Resources.Print, MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	public void ExpandGroups()
	{
		if (Grid.View is IPanelView panelView)
		{
			panelView.ExpandGroups();
			Grid.UpdateTotalSummary();
		}
	}

	public void ExpandGroupLevel(int level)
	{
		if (Grid.View is IPanelView panelView)
		{
			if (level == panelView.GroupingLevel)
			{
				panelView.ExpandGroups();
			}
			else
			{
				panelView.ExpandGroupLevel(level);
			}
		}
	}

	public void CollapseGroups()
	{
		if (Grid.View is IPanelView panelView)
		{
			panelView.CollapseGroups();
			Grid.UpdateTotalSummary();
		}
	}

	public void ShowGroupSummaryEditor()
	{
		GridView.ShowGroupSummaryEditor();
	}

	public void RemoveSort()
	{
		Grid.ClearSorting();
	}

	private void SortIndexChanged(object sender, EventArgs args)
	{
		MyGridColumn myGridColumn = (MyGridColumn)sender;
		if (myGridColumn.SortIndex >= 0 && (myGridColumn.SortOrder == ColumnSortOrder.Ascending || myGridColumn.SortOrder == ColumnSortOrder.Descending))
		{
			myGridColumn.ColSortIndex = myGridColumn.SortIndex + 1;
		}
		else
		{
			myGridColumn.ColSortIndex = -1;
		}
	}

	public static void StoreImage(ImageSource imageSource, string fileName)
	{
		if (imageSource != null)
		{
			FileStream fileStream = new FileStream("c:\\\\Large" + fileName + ".png", FileMode.OpenOrCreate, FileAccess.Write);
			PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
			pngBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapImage)imageSource));
			pngBitmapEncoder.Save(fileStream);
			fileStream.Close();
		}
	}

	private void ShowUnboundColumnsDialog()
	{
		new UnboundColumnsDialog(this).ShowDialog();
	}

	public static void ShowColumnChooser(GridControl grd, object parentWindow = null)
	{
		ListBoxEdit listBoxEdit = new ListBoxEdit();
		bool flag = true;
		listBoxEdit.IsReadOnly = false;
		listBoxEdit.Name = "lstColumns";
		CheckedListBoxEditStyleSettings checkedListBoxEditStyleSettings = new CheckedListBoxEditStyleSettings();
		checkedListBoxEditStyleSettings.ApplyToEdit(listBoxEdit);
		listBoxEdit.ShowCustomItems = true;
		listBoxEdit.StyleSettings = checkedListBoxEditStyleSettings;
		foreach (GridColumn item in grd.Columns.OrderBy((GridColumn p) => p.VisibleIndex))
		{
			if (item.ShowInColumnChooser && item.Name != "Selected" && item.AllowMoving != DefaultBoolean.False)
			{
				listBoxEdit.Items.Add(item);
				listBoxEdit.DisplayMember = "HeaderCaption";
				if (item.Visible)
				{
					listBoxEdit.SelectedItems.Add(item);
				}
			}
		}
		DialogWindow dialogWindow = new DialogWindow((Window)parentWindow);
		if (Settings.HostingEnvironment.ActualHeight - 100.0 > 0.0)
		{
			dialogWindow.MaxHeight = Settings.HostingEnvironment.ActualHeight - 100.0;
		}
		else
		{
			dialogWindow.MaxHeight = Settings.HostingEnvironment.ActualHeight;
		}
		dialogWindow.WindowValidation += ValidateColumnChooserDialog;
		dialogWindow.Caption = ArmSoft.SME.Common.Resources.SelectColumns;
		dialogWindow._layoutControl.Children.Add(listBoxEdit);
		if (!dialogWindow.ShowDialog().Value)
		{
			return;
		}
		if (grd.Columns.Count > 0 && grd.Columns[0] is MyGridColumn myGridColumn)
		{
			for (int num = 0; num <= grd.Columns.Count - 1; num++)
			{
				myGridColumn.ColumnIndex = num;
			}
			IPanelView panelView = (IPanelView)grd.View;
			int num2 = ((!(grd.View is MyTableView)) ? 1 : 2);
			List<ColumnBase> list = panelView.GetVisibleColumns().ToList();
			for (int num3 = num2; num3 <= list.Count - 1; num3++)
			{
				MyGridColumn myGridColumn2 = (MyGridColumn)list[num3];
				if (((MyGridColumn)list[num3 - 1]).ColumnIndex > myGridColumn2.ColumnIndex)
				{
					flag = false;
					break;
				}
			}
			for (int num4 = 0; num4 <= grd.Columns.Count - 1; num4++)
			{
				MyGridColumn myGridColumn3 = (MyGridColumn)grd.Columns[num4];
				if (!listBoxEdit.Items.Contains(myGridColumn3))
				{
					continue;
				}
				if (listBoxEdit.SelectedItems.Contains(myGridColumn3))
				{
					if (myGridColumn3.Visible)
					{
						continue;
					}
					myGridColumn3.Visible = true;
					if (!flag)
					{
						myGridColumn3.VisibleIndex = list.Count - 1;
					}
					for (int num5 = num2; num5 <= list.Count - 1; num5++)
					{
						MyGridColumn myGridColumn4 = (MyGridColumn)list[num5];
						if (flag && ((myGridColumn4.VisibleIndex < myGridColumn3.VisibleIndex && myGridColumn4.ColumnIndex > myGridColumn3.ColumnIndex) || (myGridColumn4.VisibleIndex > myGridColumn3.VisibleIndex && myGridColumn4.ColumnIndex < myGridColumn3.ColumnIndex)))
						{
							MyGridColumn myGridColumn5 = myGridColumn4;
							int visibleIndex = myGridColumn4.VisibleIndex;
							int visibleIndex2 = myGridColumn3.VisibleIndex;
							myGridColumn3.VisibleIndex = visibleIndex;
							myGridColumn5.VisibleIndex = visibleIndex2;
						}
					}
				}
				else
				{
					myGridColumn3.Visible = false;
				}
			}
			return;
		}
		for (int num6 = 0; num6 <= grd.Columns.Count - 1; num6++)
		{
			GridColumn gridColumn = grd.Columns[num6];
			if (!listBoxEdit.Items.Contains(gridColumn))
			{
				continue;
			}
			if (listBoxEdit.SelectedItems.Contains(gridColumn))
			{
				if (!gridColumn.Visible)
				{
					gridColumn.Visible = true;
				}
			}
			else
			{
				gridColumn.Visible = false;
			}
		}
	}

	private void ShowTotalGridMenu(object sender, GridMenuEventArgs e)
	{
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		_isInGrid = false;
		if (e.MenuType == GridMenuType.RowCell)
		{
			GridCellMenuInfo gridCellMenuInfo = (GridCellMenuInfo)e.MenuInfo;
			Grid.CurrentItem = gridCellMenuInfo.Row;
			if (GridView != null)
			{
				GridView.FocusedRowHandle = gridCellMenuInfo.Row.RowHandle.Value;
			}
			if (TreeView != null)
			{
				TreeView.FocusedRowHandle = gridCellMenuInfo.Row.RowHandle.Value;
			}
			if (gridCellMenuInfo.Column != null && gridCellMenuInfo.Row != null && !string.IsNullOrWhiteSpace(gridCellMenuInfo.Column.Name))
			{
				_currentCellValue = Grid.GetCellValue(gridCellMenuInfo.Row.RowHandle.Value, gridCellMenuInfo.Column.Name);
				_currentCellDisplayText = Grid.GetCellDisplayText(gridCellMenuInfo.Row.RowHandle.Value, gridCellMenuInfo.Column.Name);
				_currentColumnName = gridCellMenuInfo.Column.Name;
				e.Handled = false;
				_isInGrid = true;
				return;
			}
			_currentCellValue = null;
			_currentCellDisplayText = "";
			_currentColumnName = "";
			_isInGrid = false;
		}
		else if (e.MenuType == GridMenuType.Column)
		{
			bool flag = false;
			for (int i = 0; i <= e.Items.Count - 1; i++)
			{
				if (e.Items[i].Name == "ItemColumnChooser")
				{
					BarButtonItem barButtonItem = (BarButtonItem)e.Items[i];
					barButtonItem.Command = null;
					barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ShowColumnChooser).Handler;
				}
				else if (e.Items[i].Name == "BestFitColumns")
				{
					BarButtonItem barButtonItem2 = (BarButtonItem)e.Items[i];
					barButtonItem2.Command = null;
					barButtonItem2.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ColumnsAutofit).Handler;
				}
				else if (e.Items[i].Name == "UnboundExpressionEditor")
				{
					flag = true;
				}
			}
			BarItemLinkActionBase.SetItemLinkIndex((DependencyObject)(object)_cmdAddUnboundColumns, 11);
			e.Customizations.Add(_cmdAddUnboundColumns);
			if (!flag)
			{
				BarItemLinkSeparator barItemLinkSeparator = new BarItemLinkSeparator();
				BarItemLinkActionBase.SetItemLinkIndex((DependencyObject)(object)barItemLinkSeparator, 12);
				e.Customizations.Add(barItemLinkSeparator);
			}
			e.Customizations.Add(new BarItemLinkSeparator());
			BarButtonItem barButtonItem3 = new BarButtonItem
			{
				Name = "bShowHeaderIn2Lines",
				Content = ArmSoft.SME.Common.Resources.ShowHeaderIn2Lines
			};
			if (ColumnHeadersShowIn2Lines)
			{
				barButtonItem3.Glyph = ImageService.GetImage("check_16.svg");
			}
			else
			{
				barButtonItem3.Glyph = null;
			}
			barButtonItem3.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ShowHeaderIn2Lines).Handler;
			e.Customizations.Add(barButtonItem3);
			barButtonItem3 = new BarButtonItem
			{
				Content = ArmSoft.SME.Common.Resources.AutoWidthColumns,
				Name = "AutoWidthColumns"
			};
			if (IsAutoWidthMode)
			{
				barButtonItem3.Glyph = ImageService.GetImage("check_16.svg");
			}
			else
			{
				barButtonItem3.Glyph = null;
			}
			barButtonItem3.ItemClick += new SafeEventHandler<ItemClickEventArgs>(AutoWidthColumns).Handler;
			e.Customizations.Add(barButtonItem3);
			return;
		}
		if (e.MenuType != GridMenuType.TotalSummary)
		{
			e.Handled = false;
			return;
		}
		BarItem barItem = null;
		foreach (BarItem item in e.Items)
		{
			if (item.Name == "ItemSum")
			{
				barItem = item;
				item.IsVisible = false;
			}
			if (item.Name == "ItemCustomize")
			{
				item.IsVisible = false;
			}
		}
		foreach (GridSummaryItem item2 in Grid.TotalSummary)
		{
			if (item2.SummaryType == SummaryItemType.Sum && item2.CalculationMode == GridSummaryCalculationMode.SelectedRows && e.MenuInfo.Column.FieldName == item2.FieldName)
			{
				BarButtonItem barButtonItem4 = new BarButtonItem
				{
					Content = ArmSoft.SME.Common.Resources.SummarySelectedRowsSum,
					ToolTip = ArmSoft.SME.Common.Resources.SummarySelectedRowsSum
				};
				item2.Tag = "SelectedSum";
				barButtonItem4.Tag = item2;
				barButtonItem4.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ShowCustomSummary).Handler;
				e.Customizations.Add(barButtonItem4);
			}
			if (item2.SummaryType == SummaryItemType.Sum && item2.CalculationMode == GridSummaryCalculationMode.AllRows && e.MenuInfo.Column.FieldName == item2.FieldName)
			{
				BarButtonItem barButtonItem5 = new BarButtonItem
				{
					Content = barItem.Content,
					ToolTip = barItem.ToolTip,
					Glyph = barItem.Glyph,
					LargeGlyph = barItem.LargeGlyph,
					Tag = item2
				};
				barButtonItem5.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ShowSumSummary).Handler;
				e.Customizations.Add(barButtonItem5);
			}
		}
	}

	private void ShowCustomSummary(object sender, ItemClickEventArgs e)
	{
		GridSummaryItem gridSummaryItem = (GridSummaryItem)((BarButtonItem)sender).Tag;
		gridSummaryItem.Visible = !gridSummaryItem.Visible;
		e.Handled = true;
	}

	private void ShowSumSummary(object sender, ItemClickEventArgs e)
	{
		GridSummaryItem gridSummaryItem = (GridSummaryItem)((BarButtonItem)sender).Tag;
		gridSummaryItem.Visible = !gridSummaryItem.Visible;
		e.Handled = true;
	}

	public void ClearFilter()
	{
		Grid.FilterString = "";
	}

	public void SetBarStates()
	{
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Expected Obj, but got Unknown
		bool flag = IDV is GroupByDataView;
		RefreshDataViewBarButtonItem.IsEnabled = RefreshSupported;
		ShowDataViewDialogBarButtonItem.IsEnabled = DialogSupported;
		DataViewAddBarButtonItem.IsEnabled = AllowAdd;
		DataViewEditBarButtonItem.IsEnabled = AllowEdit || AllowView;
		DataViewDeleteBarButtonItem.IsEnabled = AllowDelete;
		DataViewCreateCopyBarButtonItem.IsEnabled = AllowCreateCopy;
		DataViewDocumentBarButtonItem.IsEnabled = AllowPreview;
		ExportDocumentExcelBarButtonItem.IsEnabled = AllowPreview;
		ExportDocumentWordBarButtonItem.IsEnabled = AllowPreview;
		_bExportDocumentXML.IsEnabled = AllowExport;
		ExportDocumentPDFBarButtonItem.IsEnabled = AllowPreview;
		_btnGroupColumns.IsEnabled = Grid.View == GridView && Grid.GroupCount == 0;
		if (Settings.SystemType == SystemTypes.MarketPlace)
		{
			DocumentHistoryBarButtonItem.IsVisible = false;
			DocumentHistoryBarButtonItem.IsEnabled = false;
		}
		else
		{
			DocumentHistoryBarButtonItem.IsEnabled = AllowDocumentHistory;
		}
		_cmdFind.IsEnabled = true;
		_chkReportDetails.IsEnabled = true;
		if (Grid.View == GridView)
		{
			Grid.View.SearchDelay = IDV.SearchDelay;
			_cmdAutoFilter.IsEnabled = true;
			_cmdFilter.IsEnabled = true;
			_cmdGroupPanel.IsEnabled = true;
			_bPrintPreview.IsEnabled = Grid.SelectionMode != MultiSelectMode.Cell;
			_bPrint.IsEnabled = Grid.SelectionMode != MultiSelectMode.Cell;
			_bExportToExcel.IsEnabled = Grid.SelectionMode != MultiSelectMode.Cell;
			_bExportPDF.IsEnabled = Grid.SelectionMode != MultiSelectMode.Cell;
			_bSelect.IsEnabled = true;
			_cmdSelectMarked.IsEnabled = true;
			_cmdSelectAll.IsEnabled = true;
			_cmdSelectByMask.IsEnabled = true;
			_cmdUnSelectByMask.IsEnabled = true;
			_btnFilterSelected.IsEnabled = true;
			_cmdCopy.IsEnabled = true;
			_cmdColumnChooser.IsEnabled = true;
			_cmdAddUnboundColumns.IsEnabled = !string.IsNullOrWhiteSpace(IDV.DataViewGroup);
			ShowFilterRow = _cmdAutoFilter.IsChecked.Value;
			ShowGroupPanel = _cmdGroupPanel.IsChecked.Value;
			ShowTransactionsCheckItem.IsEnabled = IDV.AllowShowTransactions;
			ShowTransactionsMode = ShowTransactionsCheckItem.IsChecked.Value;
			if (_arrLayouts == null || _arrLayouts.Count == 0)
			{
				_bLayoutEdit.IsEnabled = false;
				_bLayoutLoad.IsEnabled = false;
			}
			else
			{
				_bLayoutEdit.IsEnabled = !flag;
				_bLayoutLoad.IsEnabled = !flag;
			}
			_bLayoutSave.IsEnabled = !flag && !string.IsNullOrWhiteSpace(IDV.Name);
			_bLayoutDefault.IsEnabled = !string.IsNullOrWhiteSpace(IDV.Name);
			RibbonPageGroupRecords.IsEnabled = true;
			RibbonPageGroupDocument.IsEnabled = true;
		}
		else if (Grid.View == TreeView)
		{
			_cmdAutoFilter.IsEnabled = true;
			_cmdFilter.IsEnabled = true;
			_cmdGroupPanel.IsEnabled = false;
			_bPrintPreview.IsEnabled = Grid.SelectionMode != MultiSelectMode.Cell;
			_bPrint.IsEnabled = Grid.SelectionMode != MultiSelectMode.Cell;
			_bExportToExcel.IsEnabled = Grid.SelectionMode != MultiSelectMode.Cell;
			_bExportPDF.IsEnabled = Grid.SelectionMode != MultiSelectMode.Cell;
			_bSelect.IsEnabled = true;
			_cmdSelectMarked.IsEnabled = true;
			_cmdSelectAll.IsEnabled = true;
			_cmdSelectByMask.IsEnabled = true;
			_cmdUnSelectByMask.IsEnabled = true;
			_btnFilterSelected.IsEnabled = true;
			_chkShowGroupSummaryEditor.IsEnabled = false;
			_cmdExpandGroups.IsEnabled = true;
			_cmdCollapseGroups.IsEnabled = true;
			_cmdExpandGroupLevel.IsEnabled = true;
			_cmdCopy.IsEnabled = true;
			_cmdColumnChooser.IsEnabled = true;
			_cmdAddUnboundColumns.IsEnabled = false;
			_cmdClearGroup.IsEnabled = false;
			ShowFilterRow = _cmdAutoFilter.IsChecked.Value;
			ShowTransactionsCheckItem.IsEnabled = IDV.AllowShowTransactions;
			ShowTransactionsMode = ShowTransactionsCheckItem.IsChecked.Value;
			if (_arrLayouts == null || _arrLayouts.Count == 0)
			{
				_bLayoutEdit.IsEnabled = false;
				_bLayoutLoad.IsEnabled = false;
			}
			else
			{
				_bLayoutEdit.IsEnabled = !flag;
				_bLayoutLoad.IsEnabled = !flag;
			}
			_bLayoutSave.IsEnabled = !flag && !string.IsNullOrWhiteSpace(IDV.Name);
			_bLayoutDefault.IsEnabled = !string.IsNullOrWhiteSpace(IDV.Name);
		}
		else
		{
			_cmdAutoFilter.IsEnabled = false;
			_cmdFilter.IsEnabled = false;
			_cmdGroupPanel.IsEnabled = false;
			_bPrintPreview.IsEnabled = true;
			_bPrint.IsEnabled = false;
			_bExportToExcel.IsEnabled = false;
			_bExportPDF.IsEnabled = true;
			_bSelect.IsEnabled = false;
			_cmdSelectMarked.IsEnabled = false;
			_cmdSelectByMask.IsEnabled = false;
			_cmdUnSelectByMask.IsEnabled = false;
			_cmdSelectAll.IsEnabled = false;
			_btnFilterSelected.IsEnabled = false;
			_cmdCopy.IsEnabled = false;
			_cmdColumnChooser.IsEnabled = false;
			_cmdAddUnboundColumns.IsEnabled = false;
			ShowFilterRow = false;
			ShowGroupPanel = false;
			ShowTransactionsCheckItem.IsEnabled = false;
			ShowTransactionsMode = false;
			_cmdExpandGroups.IsEnabled = false;
			_cmdCollapseGroups.IsEnabled = false;
			_cmdExpandGroupLevel.IsEnabled = false;
			_chkShowGroupSummaryEditor.IsEnabled = false;
			_cmdClearGroup.IsEnabled = false;
			_cmdClearSort.IsEnabled = false;
			_btnClearFilter.IsEnabled = false;
			_bLayoutEdit.IsEnabled = false;
			_bLayoutLoad.IsEnabled = false;
			_bLayoutSave.IsEnabled = false;
			_bLayoutDefault.IsEnabled = false;
		}
		_cmdCardView.IsEnabled = IDV.CardViewAllowed();
		_cmdTreeView.IsEnabled = IDV.TreeViewAllowed(ref _treeViewIsDefault);
		_cmdTreeView.IsChecked = Grid.View == TreeView;
		_cmdCardView.IsChecked = Grid.View == CardView;
		ShowOnStatusBar();
		if (Grid.View == CardView)
		{
			return;
		}
		Binding binding;
		if (Grid.View == GridView)
		{
			binding = new Binding("GroupedColumns.Count")
			{
				Source = GridView,
				Mode = BindingMode.OneWay,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
				Converter = new GroupedToEnabled()
			};
			_cmdExpandGroups.SetBinding(ContentElement.IsEnabledProperty, binding);
			_cmdExpandGroupLevel.SetBinding(ContentElement.IsEnabledProperty, binding);
			_cmdCollapseGroups.SetBinding(ContentElement.IsEnabledProperty, binding);
			_chkShowGroupSummaryEditor.SetBinding(ContentElement.IsEnabledProperty, binding);
			_cmdClearGroup.SetBinding(ContentElement.IsEnabledProperty, binding);
		}
		_cmdExpandGroupLevel.IsEnabledChanged += new SafeEventHandler<DependencyPropertyChangedEventArgs>(AdjustLevelInBarItem).Handler;
		binding = new Binding("SortInfo.Count")
		{
			Source = Grid,
			Mode = BindingMode.OneWay,
			Converter = new GroupedToEnabled(),
			UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
		};
		_cmdClearSort.SetBinding(ContentElement.IsEnabledProperty, binding);
		binding = new Binding("FilterString")
		{
			Source = Grid,
			Converter = new FilteredToEnabled(),
			UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
		};
		_btnClearFilter.SetBinding(ContentElement.IsEnabledProperty, binding);
		AsRibbon categoryRibbon = CategoryRibbon;
		if (!categoryRibbon.IsEmpty())
		{
			categoryRibbon.DeleteEmptyGroups();
			if (!RibbonDefaultPageCategory.Pages.Contains(categoryRibbon))
			{
				RibbonDefaultPageCategory.Pages.Add(categoryRibbon);
				binding = new Binding("IsActive")
				{
					Source = this
				};
				categoryRibbon.SetBinding(RibbonPage.IsVisibleProperty, binding);
			}
		}
	}

	public void AddToolbarItems()
	{
		AddFunction = DataViewAddBarButtonItem.ConvertForToolbar("t_add_16.svg");
		EditFunction = DataViewEditBarButtonItem.ConvertForToolbar("t_edit_16.svg");
		DeleteFunction = DataViewDeleteBarButtonItem.ConvertForToolbar("t_delete_16.svg");
		CreateCopyFunction = DataViewCreateCopyBarButtonItem.ConvertForToolbar("t_doc_copy_16.svg");
		PreviewFunction = DataViewDocumentBarButtonItem.ConvertForToolbar("t_doc_preview_16.svg");
		RibbonControl.ToolbarItemLinks.Add(AddFunction);
		RibbonControl.ToolbarItemLinks.Add(EditFunction);
		RibbonControl.ToolbarItemLinks.Add(DeleteFunction);
		RibbonControl.ToolbarItemLinks.Add(CreateCopyFunction);
		RibbonControl.ToolbarItemLinks.Add(ShowDataViewDialogBarButtonItem.ConvertForToolbar("t_table_refresh_16.svg"));
		RibbonControl.ToolbarItemLinks.Add(RefreshDataViewBarButtonItem.ConvertForToolbar("t_refresh_16.svg"));
		RibbonControl.ToolbarItemLinks.Add(_bExportToExcel.ConvertForToolbar("t_table_export_excel_16.svg"));
		RibbonControl.ToolbarItemLinks.Add(_bPrintPreview.ConvertForToolbar("t_table_preview_16.svg"));
		RibbonControl.ToolbarItemLinks.Add(_bPrint.ConvertForToolbar("t_print_16.svg"));
		RibbonControl.ToolbarItemLinks.Add(ExportDocumentExcelBarButtonItem.ConvertForToolbar("t_doc_export_excel_16.svg"));
		if (DocumentHistoryBarButtonItem.IsVisible)
		{
			DocumentHistoryFunction = DocumentHistoryBarButtonItem.ConvertForToolbar("t_history_16.svg");
			RibbonControl.ToolbarItemLinks.Add(DocumentHistoryFunction);
		}
		else
		{
			DocumentHistoryFunction = null;
		}
		RibbonControl.ToolbarItemLinks.Add(_bGridLayout.ConvertForToolbar("t_layout_icon16.svg"));
	}

	private void ShowGroupingDialog()
	{
		if (Grid.GroupCount > 0 || Grid.View != GridView || Grid.VisibleRowCount == 0)
		{
			return;
		}
		if (!string.IsNullOrEmpty(_groupingInfoJson))
		{
			_columnsGroupingInfo = JsonSerializer.Deserialize<SerializableGroupingInfo>(_groupingInfoJson);
		}
		DlgGroupByColumns dlgGroupByColumns = new DlgGroupByColumns(this, _columnsGroupingInfo, _arrLayoutsGroupBy);
		if (dlgGroupByColumns.ShowDialog().Value)
		{
			BindingList<GroupingColumn> getSelectedColumns = dlgGroupByColumns.GetSelectedColumns;
			if (getSelectedColumns != null && getSelectedColumns.Count != 0)
			{
				_columnsGroupingInfo = new SerializableGroupingInfo
				{
					ShowQty = dlgGroupByColumns.ShowQTY,
					ColumnsInfo = dlgGroupByColumns.GetSelectedColumns,
					Name = ""
				};
				_groupingInfoJson = JsonSerializer.Serialize(_columnsGroupingInfo);
				ShowGroupedReport(dlgGroupByColumns.ShowQTY, getSelectedColumns);
			}
		}
	}

	private bool FindTotalSummary(string fieldName, bool selectedSummary)
	{
		foreach (GridSummaryItem item in Grid.TotalSummary)
		{
			if (item.FieldName == fieldName && item.SummaryType == SummaryItemType.Sum)
			{
				if (item.CalculationMode == GridSummaryCalculationMode.SelectedRows == selectedSummary)
				{
					return true;
				}
				return false;
			}
		}
		return false;
	}

	private void ShowGroupedReport(bool showQTY, BindingList<GroupingColumn> groupingInfo)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (groupingInfo != null && groupingInfo.Count != 0)
		{
			GroupByDataView groupByDataView = new GroupByDataView(this, groupingInfo, showQTY);
			groupByDataView.Show();
			groupByDataView.DVP.CaptionImage = ImageService.GetImage("grouping_16.svg");
		}
	}

	protected override void BarManager_ItemClick(object sender, ItemClickEventArgs e)
	{
		BarCheckItem barCheckItem = e.Item as BarCheckItem;
		BarButtonItem barButtonItem = e.Item as BarButtonItem;
		MainWindowBase hostingEnvironment = Settings.HostingEnvironment;
		if (hostingEnvironment.CurrentViewPanel == null)
		{
			DataViewPanel dataViewPanel = (hostingEnvironment.CurrentViewPanel = this);
		}
		if (barButtonItem != null && barCheckItem == null)
		{
			CallsRecordingHelper.RecordCallStep(StatisticTypeSE.MainRibbon, IDV.Name + " - " + barButtonItem.Name);
			if (barButtonItem == DataViewAddBarButtonItem)
			{
				Add();
			}
			else if (barButtonItem == DataViewEditBarButtonItem)
			{
				if (AllowEdit)
				{
					Edit();
				}
				else if (AllowView)
				{
					View();
				}
			}
			else if (barButtonItem == DataViewDeleteBarButtonItem)
			{
				Delete();
			}
			else if (barButtonItem == DataViewCreateCopyBarButtonItem)
			{
				CreateCopy();
			}
			else if (barButtonItem == ExportDocumentExcelBarButtonItem)
			{
				PrintPreview(PrintMode.ExportToExcel);
			}
			else if (barButtonItem == ExportDocumentWordBarButtonItem)
			{
				PrintPreview(PrintMode.ExportToWord);
			}
			else if (barButtonItem == ExportDocumentPDFBarButtonItem)
			{
				PrintPreview(PrintMode.ExportToPdf);
			}
			else if (barButtonItem == _bExportDocumentXML)
			{
				ExportToXML();
			}
			else if (barButtonItem == DocumentHistoryBarButtonItem)
			{
				DocumentHistory();
			}
			else if (barButtonItem == _cmdClearSort)
			{
				RemoveSort();
			}
			else if (barButtonItem == _btnClearFilter)
			{
				ClearFilter();
			}
			else if (barButtonItem == _cmdClearGroup)
			{
				RemoveGrouping();
			}
			else if (barButtonItem == ShowDataViewDialogBarButtonItem)
			{
				if (DialogSupported)
				{
					ShowDialog();
				}
			}
			else if (barButtonItem == RefreshDataViewBarButtonItem)
			{
				Refresh();
			}
			else if (barButtonItem == _bPrintPreview)
			{
				if (GetActiveChartIndex() == -1)
				{
					PreviewGrid();
				}
				else
				{
					PreviewChart();
				}
			}
			else if (barButtonItem == _bPrint)
			{
				if (GetActiveChartIndex() == -1)
				{
					PrintGrid();
				}
				else
				{
					PreviewChart();
				}
			}
			else if (barButtonItem == PrintOptionsBarButtonItem)
			{
				string reportKind = IdvName();
				WndPageSetup wndPageSetup = new WndPageSetup();
				wndPageSetup.ReportKind = reportKind;
				wndPageSetup.ShowDialog();
			}
			else if (barButtonItem == _bExportXlsX)
			{
				ExportToExcelOrPdf("xlsx");
			}
			else if (barButtonItem == _bExportXlsXLargeData)
			{
				ExportToExcelDataAware("xlsx");
			}
			else if (barButtonItem == _bExportCsv)
			{
				ExportToExcelDataAware("csv", "export.csv", "Sheet1", showSaveDialog: true, showFile: false);
				MessageBox.Show(ArmSoft.SME.Common.Resources.OperationSucceeded);
			}
			else if (barButtonItem == _bExportPDF)
			{
				ExportToExcelOrPdf("pdf", "export.pdf");
			}
			else if (barButtonItem == _cmdFind || barButtonItem == _cmdFindData)
			{
				Grid.View.ShowSearchPanel(moveFocusInSearchPanel: true);
			}
			else if (barButtonItem == _cmdSelectByMask)
			{
				SelectRowsByMask();
			}
			else if (barButtonItem == _cmdUnSelectByMask)
			{
				SelectRowsByMask(isSelectMode: false);
			}
			else if (barButtonItem == DataViewDocumentBarButtonItem)
			{
				PrintPreview();
			}
			else if (barButtonItem == _cmdFilter)
			{
				Grid.View.ShowFilterEditor(null);
			}
			else if (barButtonItem == _cmdFilterCurrent)
			{
				FilterByValue(null, null);
			}
			else if (barButtonItem == _cmdFilterNotEqualCurrent)
			{
				FilterByNotEqualValue(null, null);
			}
			else if (barButtonItem == _cmdCopyCurrent)
			{
				CopyByValue();
			}
			else if (barButtonItem == _cmdCopyFilter)
			{
				CopyFilterString();
			}
			else if (barButtonItem == _cmdColumnChooser)
			{
				ShowColumnChooser(Grid);
			}
			else if (barButtonItem == _cmdAddUnboundColumns)
			{
				ShowUnboundColumnsDialog();
			}
			else if (barButtonItem == _btnFilterSelected || barButtonItem == _btnFilterSelectedData)
			{
				ReduceRows();
			}
			else if (barButtonItem == _cmdExpandGroups)
			{
				ExpandGroups();
			}
			else if (barButtonItem == _cmdCollapseGroups)
			{
				CollapseGroups();
			}
			else if (barButtonItem == _btnGroupColumns)
			{
				ShowGroupingDialog();
			}
			else if (barButtonItem == HelpBarButtonItem)
			{
				Settings.HostingEnvironment.ShowHelp(HelpID);
			}
			else if (barButtonItem == _bLayoutSave)
			{
				SaveLayoutClick();
			}
			else if (barButtonItem == _bLayoutLoad)
			{
				LoadLayoutClick();
			}
			else if (barButtonItem == _bLayoutDefault)
			{
				RestoreDefaultLayoutClick();
			}
			else if (barButtonItem == _bLayoutEdit)
			{
				EditLayoutClick();
			}
		}
		if (barCheckItem == null)
		{
			return;
		}
		if (barButtonItem == _cmdAutoFilter)
		{
			ShowFilterRow = barCheckItem.IsChecked.Value;
		}
		else if (barButtonItem == _chkShowGroupSummaryEditor)
		{
			ShowGroupSummaryEditor();
		}
		else if (barButtonItem == _chkReportDetails)
		{
			if (barCheckItem.IsChecked.Value)
			{
				FillDetail();
				DetailsPanel.Visibility = Visibility.Visible;
			}
			else
			{
				DetailsPanel.Visibility = Visibility.Collapsed;
			}
		}
		else if (barButtonItem == _cmdGroupPanel)
		{
			ShowGroupPanel = barCheckItem.IsChecked.Value;
		}
		else if (barButtonItem == _cmdCardView && IDV.CardViewAllowed())
		{
			if (_cmdCardView.IsChecked.Value)
			{
				InitCardView(IDV);
				Grid.SelectionMode = MultiSelectMode.Row;
			}
			else
			{
				InitView(IDV);
				Grid.SelectionMode = MultiSelectMode.MultipleRow;
			}
			SetBarStates();
		}
		else if (barButtonItem == _cmdTreeView && IDV.TreeViewAllowed(ref _treeViewIsDefault))
		{
			if (_cmdTreeView.IsChecked.Value)
			{
				if (Grid.View == GridView && (GridView.GroupedColumns == null || GridView.GroupedColumns.Count == 0))
				{
					InitTreeView(IDV);
					Grid.SelectionMode = MultiSelectMode.MultipleRow;
					if (IDV.ShowExpanded())
					{
						TreeView.ExpandAllNodes();
					}
				}
			}
			else if (Grid.View != GridView)
			{
				InitView(IDV);
				Grid.SelectionMode = MultiSelectMode.MultipleRow;
			}
			SetBarStates();
		}
		else if (barButtonItem == ShowTransactionsCheckItem)
		{
			if (ShowTransactionsCheckItem.IsChecked.Value)
			{
				ShowTransactionsMode = true;
				ShowTotalsPanel();
			}
			else
			{
				ShowTransactionsMode = false;
				ShowTotalsPanel();
			}
		}
	}

	private void CopyFilterString()
	{
		Clipboard.SetDataObject(Grid.FilterString);
	}

	private void DataViewPanel_GotFocus(object sender, RoutedEventArgs e)
	{
		if (IsFloating)
		{
			RibbonStatusBar.Visibility = Visibility.Visible;
		}
		ShowOnStatusBar();
	}

	private void DataViewPanel_Loaded(object sender, RoutedEventArgs e)
	{
		_statusBarCompanyName.Content = Settings.HostingEnvironment.StatusBarCompanyName;
		if (string.IsNullOrWhiteSpace(_defaultLayout))
		{
			MemoryStream memoryStream = new MemoryStream();
			Grid.SaveLayoutToStream(memoryStream);
			_defaultLayout = Convert.ToBase64String(memoryStream.ToArray());
			_currentLayout = "Default";
			_defaultView = Grid.View;
			_defaultView.FilterEditorShowOperandTypeIcon = true;
			Grid.SelectedItem = null;
		}
		CollectUnboundExpressions();
	}

	public void CollectUnboundExpressions()
	{
		_unboundExpressions.Clear();
		foreach (GridColumn column in Grid.Columns)
		{
			if (!string.IsNullOrWhiteSpace(column.UnboundExpression))
			{
				_unboundExpressions.Add(column.Name, column.UnboundExpression);
			}
		}
	}

	public static ScrollViewer FindScrollViewer(DependencyObject el)
	{
		if (VisualTreeHelper.GetChildrenCount(el) == 0)
		{
			return null;
		}
		for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(el); i++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(el, i);
			if (child is ScrollViewer result)
			{
				return result;
			}
			ScrollViewer scrollViewer = FindScrollViewer(child);
			if (scrollViewer != null)
			{
				return scrollViewer;
			}
		}
		return null;
	}

	private void DataViewPanel_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (HotKeys.WithModifier.Control_F4.Matches(this, e))
		{
			Settings.HostingEnvironment.ClosePanel(this);
			e.Handled = true;
		}
		else if (HotKeys.WithModifier.Control_OemPlus.Matches(this, e) || HotKeys.WithModifier.Control_Add.Matches(this, e))
		{
			SelectRowsByMask();
			e.Handled = true;
		}
		else if (HotKeys.WithModifier.Control_OemMinus.Matches(this, e) || HotKeys.WithModifier.Control_Subtract.Matches(this, e))
		{
			SelectRowsByMask(isSelectMode: false);
			e.Handled = true;
		}
	}

	private void DetailLayoutManager_DockItemClosing(object sender, ItemCancelEventArgs e)
	{
		if (e.Item.Name == "DetailsPanel")
		{
			DetailsPanel.Visibility = Visibility.Collapsed;
			_chkReportDetails.IsChecked = false;
			e.Cancel = true;
		}
		else if (e.Item == TotalsPanel)
		{
			TotalsPanel.Visibility = Visibility.Collapsed;
			ShowTransactionsCheckItem.IsChecked = false;
			e.Cancel = true;
		}
		else if (e.Item == TotalsPanelRight)
		{
			TotalsPanelRight.Visibility = Visibility.Collapsed;
			ShowTransactionsCheckItem.IsChecked = false;
			e.Cancel = true;
		}
	}

	private void DataViewPanel_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		ScrollViewer scrollViewer = FindScrollViewer((DependencyObject)(object)Grid);
		if ((int)Keyboard.Modifiers == 2)
		{
			scrollViewer?.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - (double)e.Delta);
			e.Handled = true;
		}
		else
		{
			e.Handled = false;
		}
	}

	public void DataViewPanel_Unloaded(object sender, RoutedEventArgs e)
	{
		if (IDV == null)
		{
			Log.Warning(" DataViewPanel_Unloaded ", "IsNothing(IDV) ? ");
			return;
		}
		if (Grid == null)
		{
			Log.Warning(" DataViewPanel_Unloaded " + IDV.Name, "IsNothing(Grid)");
			return;
		}
		if (Grid.Columns.Count == 0)
		{
			Log.Warning(" DataViewPanel_Unloaded " + IDV.Name, "Grid.Columns.Count = 0");
			return;
		}
		try
		{
			foreach (GridColumn column in Grid.Columns)
			{
				if (column is MyGridColumn component)
				{
					((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(ColumnBase.SortIndexProperty, typeof(MyGridColumn))).RemoveValueChanged((object)component, (EventHandler)SortIndexChanged);
					((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(ColumnBase.SortIndexProperty, typeof(MyGridColumn))).RemoveValueChanged((object)component, (EventHandler)SortIndexChanged);
				}
				else
				{
					Log.Warning(" DataViewPanel_Unloaded " + IDV.Name, "IsNothing(Col) " + column.Name);
				}
			}
			SaveSettingsOnClose();
		}
		catch (Exception ex)
		{
			Log.Warning(" DataViewPanel_Unloaded " + IDV.Name, "Failed: " + ex.Message);
		}
	}

	private void SaveSettingsOnClose()
	{
		try
		{
			if (string.IsNullOrWhiteSpace(IDV.Name) || IDV.Data == null || IDV.Data.Count <= 0)
			{
				return;
			}
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey($"{Settings.RegistryKey}\\{"DataView"}\\{IDV.Name}");
			if (registryKey == null)
			{
				return;
			}
			using RegistryKey registryKey2 = registryKey.CreateSubKey("Columns");
			registryKey2.SetValue("AutoWidth", IsAutoWidthMode ? 1 : 0);
			registryKey2.SetValue("ColHeadersIn2Lines", ColumnHeadersShowIn2Lines ? 1 : 0);
			registryKey2.SetValue("Grouping", _groupingInfoJson);
			if (TotalsPanelRight.Visibility == Visibility.Visible && TotalsPanelRight.ActualWidth > 0.0)
			{
				registryKey2.SetValue("TotalsPanelRightWidth", TotalsPanelRight.ActualWidth);
				registryKey2.SetValue("TransactionsControlDefaultPosition", "Right");
			}
			if (TotalsPanel.Visibility == Visibility.Visible && TotalsPanel.ActualHeight > 0.0)
			{
				registryKey2.SetValue("TotalsPanelHeight", TotalsPanel.ActualHeight);
				registryKey2.SetValue("TransactionsControlDefaultPosition", "Bottom");
			}
			foreach (GridColumn column in Grid.Columns)
			{
				if (column.Name != "Selected" && column.Name != "IsLeaf")
				{
					string text = column.ActualWidth.ToString();
					if (text != "NaN")
					{
						registryKey2.SetValue(column.Name.Trim(), text);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Warning(" DataViewPanel_Unloaded " + IDV.Name, "Store in Registry part failed: " + ex.Message);
		}
	}

	public void UpdateStatusAndRecalculateSummaries()
	{
		ShowOnStatusBar();
		if (Grid.View == TreeView && selectedRows.Count > 0)
		{
			Grid.UpdateTotalSummary();
		}
	}

	private void Grid_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (Grid.View != TreeView)
		{
			ShowOnStatusBar();
		}
	}

	private void Grid_GroupRowCollapsed(object sender, RowEventArgs e)
	{
		ShowOnStatusBar();
	}

	private void Grid_GroupRowExpanded(object sender, RowEventArgs e)
	{
		ShowOnStatusBar();
	}

	private void DetailsPanel_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (HotKeys.WithModifier.Control_F.Matches(this, e))
		{
			e.Handled = true;
		}
	}

	private void ShowColumnChooser(object sender, ItemClickEventArgs e)
	{
		ShowColumnChooser(Grid);
	}

	private static void ValidateColumnChooserDialog(DialogWindow dialogWindow, DialogWindowValidationArgs args)
	{
		if (LogicalTreeHelper.FindLogicalNode((DependencyObject)(object)dialogWindow._layoutControl, "lstColumns") is ListBoxEdit listBoxEdit && listBoxEdit.SelectedItems.Count == 0 && dialogWindow.Owner != null && !(dialogWindow.Owner is IPOWindow))
		{
			args.isValid = false;
		}
	}

	private void EditCard(object sender, MouseButtonEventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Point position = e.GetPosition(CardView);
		CardViewHitInfo cardViewHitInfo = CardView.CalcHitInfo(position);
		if (cardViewHitInfo != null && cardViewHitInfo.InCardRow)
		{
			if (AllowEdit)
			{
				Edit();
			}
			else if (AllowView)
			{
				View();
			}
		}
	}

	public void SelectByCondition(CriteriaOperator filterString, bool preservePreviousSelection, bool selectMode = true)
	{
		if ((object)filterString == null || filterString.ToString() == "")
		{
			return;
		}
		string filterString2 = Grid.FilterString;
		int rowHandleByVisibleIndex = Grid.GetRowHandleByVisibleIndex(0);
		if (rowHandleByVisibleIndex < 0)
		{
			return;
		}
		if (!preservePreviousSelection & selectMode)
		{
			ClearSelection();
		}
		_selectByKeyboard = true;
		Mouse.OverrideCursor = Cursors.Wait;
		Grid.FilterCriteria = filterString;
		Grid.BeginSelection();
		int i = 0;
		for (int num = Grid.VisibleRowCount - 1; i <= num; i++)
		{
			rowHandleByVisibleIndex = Grid.GetRowHandleByVisibleIndex(i);
			if (rowHandleByVisibleIndex < 0)
			{
				continue;
			}
			int listIndexByRowHandle = Grid.GetListIndexByRowHandle(rowHandleByVisibleIndex);
			try
			{
				if (selectMode)
				{
					SelectItem(rowHandleByVisibleIndex);
					selectedRows.Add(listIndexByRowHandle);
				}
				else
				{
					UnSelectItem(rowHandleByVisibleIndex);
					selectedRows.Remove(listIndexByRowHandle);
				}
			}
			catch (Exception ex)
			{
				Mouse.OverrideCursor = null;
				DXMessageBox.Show(ex.Message + "\r\nInvalid conditions! Break in rows");
				_selectByKeyboard = false;
				return;
			}
		}
		Grid.EndSelection();
		_selectByKeyboard = false;
		_rowSelectedChanged = true;
		Grid.FilterString = filterString2;
		Mouse.OverrideCursor = null;
	}

	private void SelectItem(int rowHandle)
	{
		Grid.SelectItem(rowHandle);
		if (Grid.View == TreeView)
		{
			TreeView.GetNodeByRowHandle(rowHandle).IsChecked = true;
		}
	}

	private void UnSelectItem(int rowHandle)
	{
		Grid.UnselectItem(rowHandle);
		if (Grid.View == TreeView)
		{
			TreeView.GetNodeByRowHandle(rowHandle).IsChecked = false;
		}
	}

	private bool IsRowSelected(int rowHandle)
	{
		return Grid.View.IsRowSelected(rowHandle);
	}

	private static string GetOriginalColumnName(string columnName)
	{
		columnName = columnName.Replace("SUM_", "");
		columnName = columnName.Replace("GBY_", "");
		columnName = columnName.Replace("AVG_", "");
		columnName = columnName.Replace("MAX_", "");
		columnName = columnName.Replace("MIN_", "");
		return columnName;
	}

	public DataColumnType GetColumnType(string columnName)
	{
		switch (columnName)
		{
		case "Selected":
		case "IsLeaf":
			return DataColumnType.Boolean;
		case "_GroupQuantity":
			return DataColumnType.Integer;
		default:
			if (IDV.Columns.Contains(columnName))
			{
				return IDV.GetColumnType(columnName);
			}
			columnName = GetOriginalColumnName(columnName);
			if (IDV.Columns.Contains(columnName))
			{
				return IDV.GetColumnType(columnName);
			}
			return DataColumnType.Unknown;
		}
	}

	private void ShowTooltip(object sender, MouseEventArgs e)
	{
		ToolTip toolTip = IDV.ToolTipContent(sender);
		if (toolTip != null && sender is Image image)
		{
			image.ToolTip = toolTip;
			((ToolTip)image.ToolTip).StaysOpen = true;
			((ToolTip)image.ToolTip).Placement = PlacementMode.Bottom;
			((ToolTip)image.ToolTip).PlacementTarget = image;
			((ToolTip)image.ToolTip).IsOpen = true;
			e.Handled = true;
		}
	}

	private void CloseToolTip(object sender, MouseEventArgs e)
	{
		FrameworkElement frameworkElement = (FrameworkElement)sender;
		if (frameworkElement.ToolTip != null)
		{
			((ToolTip)frameworkElement.ToolTip).IsOpen = false;
		}
	}

	private void SaveLayoutClick()
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Title = ArmSoft.SME.Common.Resources.LayoutTitle
		};
		dialogWindow.AddButtonEditControl("LayoutCaption", ArmSoft.SME.Common.Resources.Name, 50, isRequired: true, 1);
		dialogWindow["LayoutCaption"] = ((_currentLayout == "Default") ? "" : _currentLayout);
		if (dialogWindow.ShowDialog().Value)
		{
			string key = dialogWindow["LayoutCaption"].ToString().Trim();
			SaveLayout(key);
		}
		SetBarStates();
	}

	private void SaveLayout(string key)
	{
		MemoryStream memoryStream = new MemoryStream();
		Grid.SaveLayoutToStream(memoryStream);
		string text = Convert.ToBase64String(memoryStream.ToArray());
		DataViewTypes dataViewTypes = ((Grid.View == GridView) ? DataViewTypes.TableView : ((Grid.View == TreeView) ? DataViewTypes.TreeView : ((Grid.View != CardView) ? DataViewTypes.TableView : DataViewTypes.CardView)));
		if (StoreLayoutValues(key, text, dataViewTypes))
		{
			int num = FindLayoutByKey(key);
			if (num > -1)
			{
				_arrLayouts[num].Layout = text;
			}
			else
			{
				_arrLayouts.Add(new GridLayoutItem
				{
					Caption = key,
					Tag = key,
					Type = dataViewTypes,
					Layout = text
				});
			}
		}
		RestoreUnboundExpression();
	}

	private void LoadLayoutClick()
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Title = ArmSoft.SME.Common.Resources.LayoutTitle
		};
		DocumentGridControl documentGridControl = dialogWindow.AddGridControl("Layouts");
		FillGrid(documentGridControl);
		documentGridControl.ButtonAdd.IsVisible = false;
		documentGridControl.ButtonDuplicate.IsVisible = false;
		documentGridControl.ButtonDelete.IsVisible = false;
		documentGridControl.Grid.View.NavigationStyle = GridViewNavigationStyle.Row;
		documentGridControl.AllowDelete = false;
		documentGridControl.AllowAdd = false;
		documentGridControl.AllowDuplicate = false;
		documentGridControl.AllowEdit = false;
		documentGridControl.MouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(LoadGridLayout).Handler;
		dialogWindow.Activated += new SafeEventHandler<EventArgs>(FocusGrid).Handler;
		if (dialogWindow.ShowDialog().Value)
		{
			LoadGridLayout(documentGridControl, null);
		}
	}

	private void LoadGridLayout(object sender, MouseButtonEventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected Obj, but got Unknown
		object currentItem = ((DocumentGridControl)sender).Grid.CurrentItem;
		if (currentItem == null)
		{
			return;
		}
		GridLayoutItem gridLayoutItem = (GridLayoutItem)currentItem;
		int num = FindLayoutByKey(gridLayoutItem.Tag);
		if (num <= -1)
		{
			return;
		}
		string layout = _arrLayouts[num].Layout;
		if (Window.GetWindow((DependencyObject)sender) is DialogWindow dialogWindow)
		{
			dialogWindow.Close();
		}
		Grid.RestoreLayoutFromStream(new MemoryStream(Convert.FromBase64String(layout)));
		_currentLayout = _arrLayouts[num].Caption;
		switch (_arrLayouts[num].Type)
		{
		case DataViewTypes.TableView:
			if (Grid.View != GridView)
			{
				Grid.View = GridView;
			}
			break;
		case DataViewTypes.TreeView:
			if (Grid.View != TreeView)
			{
				Grid.View = TreeView;
			}
			break;
		case DataViewTypes.CardView:
			if (Grid.View != CardView)
			{
				Grid.View = CardView;
			}
			break;
		}
		if (Grid.Columns["Selected"].Visible)
		{
			Grid.Columns["Selected"].Visible = false;
		}
		foreach (GridSummaryItem item in Grid.TotalSummary)
		{
			if (item.SummaryType == SummaryItemType.Custom)
			{
				item.SummaryType = SummaryItemType.Sum;
				item.CalculationMode = GridSummaryCalculationMode.SelectedRows;
			}
		}
		foreach (GridSummaryItem item2 in Grid.GroupSummary)
		{
			if (item2.SummaryType == SummaryItemType.Custom)
			{
				item2.SummaryType = SummaryItemType.Sum;
				item2.CalculationMode = GridSummaryCalculationMode.SelectedRows;
			}
		}
		SetBarStates();
		SetTotalSummariesSettings();
		RestoreUnboundExpression();
	}

	private void RestoreDefaultLayoutClick()
	{
		if (!string.IsNullOrEmpty(_defaultLayout.Trim()))
		{
			ClearSelection();
			Grid.RestoreLayoutFromStream(new MemoryStream(Convert.FromBase64String(_defaultLayout)));
			_currentLayout = "Default";
			if (_defaultView != null)
			{
				SetTotalSummariesSettings();
				RestoreUnboundExpression();
				Grid.View = _defaultView;
			}
		}
	}

	private void RestoreUnboundExpression()
	{
		foreach (GridColumn column in Grid.Columns)
		{
			if (_unboundExpressions.TryGetValue(column.Name, out var value))
			{
				column.UnboundExpression = value;
			}
		}
	}

	private void EditLayoutClick()
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Title = ArmSoft.SME.Common.Resources.LayoutTitle
		};
		DocumentGridControl documentGridControl = dialogWindow.AddGridControl("Layouts");
		FillGrid(documentGridControl);
		documentGridControl.ButtonAdd.IsVisible = false;
		documentGridControl.ButtonDuplicate.IsVisible = false;
		documentGridControl.AllowDelete = true;
		documentGridControl.AllowAdd = false;
		documentGridControl.AllowDrop = false;
		documentGridControl.AllowDuplicate = false;
		documentGridControl.AllowEdit = true;
		dialogWindow.WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>((object? d, DialogWindowValidationArgs a) =>
		{
			LayoutWindowEditValidation((DialogWindow)d, a);
		}).Handler;
		if (dialogWindow.ShowDialog().Value)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			for (int num = 0; num <= documentGridControl.Grid.VisibleRowCount - 1; num++)
			{
				int rowHandleByVisibleIndex = documentGridControl.Grid.GetRowHandleByVisibleIndex(num);
				string value = documentGridControl.Grid.GetCellValue(rowHandleByVisibleIndex, documentGridControl.Grid.Columns["Caption"]).ToString().Trim();
				string key = documentGridControl.Grid.GetCellValue(rowHandleByVisibleIndex, documentGridControl.Grid.Columns["Tag"]).ToString().Trim();
				dictionary.Add(key, value);
			}
			try
			{
				foreach (GridLayoutItem arrLayout in _arrLayouts)
				{
					if (!dictionary.ContainsKey(arrLayout.Tag))
					{
						Settings.LayoutService.Delete(new LayoutRow
						{
							Name = IDV.Name,
							Caption = arrLayout.Tag
						});
					}
				}
				List<LayoutRow> list = Settings.LayoutService.GetList(IDV.Name);
				foreach (GridLayoutItem itm in _arrLayouts)
				{
					if (dictionary.TryGetValue(itm.Tag, out var value2) && value2 != itm.Caption && list.Any((LayoutRow p) => p.Caption == itm.Tag) && Settings.LayoutService.Delete(new LayoutRow
					{
						Name = IDV.Name,
						Caption = itm.Tag
					}))
					{
						string caption = dictionary[itm.Caption];
						string layout = itm.Layout;
						StoreLayoutValues(caption, layout, itm.Type);
					}
				}
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
			LoadLayouts();
		}
		SetBarStates();
	}

	private static void LayoutWindowEditValidation(DialogWindow dialog, DialogWindowValidationArgs args)
	{
		DocumentGridControl documentGridControl = (DocumentGridControl)dialog["Layouts"];
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int i = 0; i <= documentGridControl.Grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = documentGridControl.Grid.GetRowHandleByVisibleIndex(i);
			string text = documentGridControl.Grid.GetCellValue(rowHandleByVisibleIndex, documentGridControl.Grid.Columns["Caption"]).ToString().Trim();
			string key = documentGridControl.Grid.GetCellValue(rowHandleByVisibleIndex, documentGridControl.Grid.Columns["Tag"]).ToString().Trim();
			if (string.IsNullOrWhiteSpace(text))
			{
				MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Name));
				args.isValid = false;
			}
			else if (dictionary.ContainsValue(text))
			{
				MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.LayoutDuplicate, text));
				args.isValid = false;
			}
			dictionary.Add(key, text);
		}
	}

	private void FillGrid(DocumentGridControl gridControl)
	{
		List<GridLayoutItem> list = new List<GridLayoutItem>();
		foreach (GridLayoutItem arrLayout in _arrLayouts)
		{
			list.Add(new GridLayoutItem
			{
				Caption = arrLayout.Caption.Trim(),
				Tag = arrLayout.Tag.Trim()
			});
		}
		gridControl.ButtonAdd.IsVisible = false;
		gridControl.ButtonDuplicate.IsVisible = false;
		gridControl.AllowDelete = true;
		gridControl.AllowAdd = false;
		gridControl.AllowDrop = false;
		gridControl.AllowDuplicate = false;
		gridControl.AllowEdit = true;
		gridControl.ItemsSource = list;
		DocumentGridColumn documentGridColumn = gridControl.AddStringColumn("Caption", ArmSoft.SME.Common.Resources.Name, 50);
		documentGridColumn.AllowMoving = DefaultBoolean.False;
		documentGridColumn.FieldName = "Caption";
		documentGridColumn.AllowEditing = DefaultBoolean.True;
		documentGridColumn.ShowInColumnChooser = false;
		DocumentGridColumn documentGridColumn2 = gridControl.AddStringColumn("Tag", ArmSoft.SME.Common.Resources.Name, 50);
		documentGridColumn2.AllowMoving = DefaultBoolean.False;
		documentGridColumn2.FieldName = "Tag";
		documentGridColumn2.AllowEditing = DefaultBoolean.False;
		documentGridColumn2.Visible = false;
	}

	private bool StoreLayoutValues(string caption, string body, DataViewTypes dataViewTypes)
	{
		try
		{
			return Settings.LayoutService.Store(new LayoutRow
			{
				Name = IDV.Name,
				Caption = caption,
				Body = body,
				LayoutType = (byte)dataViewTypes
			});
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			return false;
		}
	}

	private int FindLayoutByKey(string key)
	{
		for (int i = 0; i <= _arrLayouts.Count - 1; i++)
		{
			if (_arrLayouts[i].Caption == key)
			{
				return i;
			}
		}
		return -1;
	}

	private void FocusGrid(object sender, EventArgs e)
	{
		DocumentGridControl documentGridControl = (DocumentGridControl)((DialogWindow)sender)["Layouts"];
		if (documentGridControl.Grid.VisibleRowCount > 0)
		{
			int rowHandleByListIndex = documentGridControl.Grid.GetRowHandleByListIndex(0);
			documentGridControl.View.FocusedRowHandle = rowHandleByListIndex;
			Keyboard.Focus(documentGridControl.Grid.View);
		}
	}

	private static GridColumn FindColumn(DependencyObject sender)
	{
		for (DependencyObject val = ((TextBox)(object)sender).TemplatedParent; val != null; val = VisualTreeHelper.GetParent(val))
		{
			if (val is GridColumnHeader gridColumnHeader)
			{
				return (GridColumn)gridColumnHeader.DataContext;
			}
		}
		return null;
	}

	private void ShowHeaderIn2Lines(object sender, ItemClickEventArgs e)
	{
		ColumnHeadersShowIn2Lines = !ColumnHeadersShowIn2Lines;
	}

	private void AutoWidthColumns(object sender, ItemClickEventArgs e)
	{
		if (GridView != null)
		{
			GridView.AutoWidth = !GridView.AutoWidth;
		}
		else if (TreeView != null)
		{
			TreeView.AutoWidth = !TreeView.AutoWidth;
		}
	}

	private void ColumnsAutofit(object sender, ItemClickEventArgs e)
	{
		if (GridView != null)
		{
			if (GridView.AutoWidth)
			{
				GridView.AutoWidth = false;
			}
			GridView.BestFitColumns();
		}
		else if (TreeView != null)
		{
			if (TreeView.AutoWidth)
			{
				TreeView.AutoWidth = false;
			}
			TreeView.BestFitColumns();
		}
	}

	private void ExpandLevel_ItemClick(object sender, ItemClickEventArgs e)
	{
		int level = (int)((BarButtonItem)sender).Tag;
		_selectByKeyboard = true;
		ExpandGroupLevel(level);
		_selectByKeyboard = false;
		Grid.UpdateTotalSummary();
	}

	private void AdjustLevelInBarItem()
	{
		BarSubItem cmdExpandGroupLevel = _cmdExpandGroupLevel;
		cmdExpandGroupLevel.Items.Clear();
		if (Grid.View is IPanelView { GroupingLevel: var groupingLevel } && groupingLevel > 0)
		{
			for (int i = 1; i <= groupingLevel; i++)
			{
				BarButtonItem barButtonItem = new BarButtonItem
				{
					Tag = i,
					Content = string.Format(ArmSoft.SME.Common.Resources.ExpandToLevel, i.ToString())
				};
				cmdExpandGroupLevel.Items.Add(barButtonItem);
				barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ExpandLevel_ItemClick).Handler;
			}
		}
	}

	private void AdjustLevelInBarItem(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (_cmdExpandGroupLevel.IsEnabled)
		{
			AdjustLevelInBarItem();
		}
	}

	private void AddTreeViewConditionalFormatting()
	{
		IDV.ConditionalFormattingList.Add(new AsCondititionFormat
		{
			FieldName = "Leaf",
			Expression = "[Leaf] = false",
			FormatName = "Custom",
			CustomFormat = new UserDefinedFormat
			{
				FontIsBold = true
			},
			IsFixed = true,
			ApplyToRow = true
		});
	}

	private void PreviewChart()
	{
		SimpleLink simpleLink = new SimpleLink
		{
			DetailCount = 1,
			DetailTemplate = (DataTemplate)Settings.GenericResources["ChartPreviewData"]
		};
		simpleLink.CreateDetail += new SafeEventHandler<DevExpress.Xpf.Printing.CreateAreaEventArgs>(SL_CreateDetail).Handler;
		PrintInfo printInfo = new PrintInfo(IdvName());
		simpleLink.PrintingSystem.PageSettings.PaperKind = DXPaperKind.A4;
		simpleLink.PrintingSystem.PageSettings.Margins.Left = printInfo.MarginLeft * 100f;
		simpleLink.PrintingSystem.PageSettings.Margins.Right = printInfo.MarginRight * 100f;
		simpleLink.PrintingSystem.PageSettings.Margins.Top = printInfo.MarginTop * 100f;
		simpleLink.PrintingSystem.PageSettings.Margins.Bottom = printInfo.MarginBottom * 100f;
		if (printInfo.PaperOrientation == "Landscape")
		{
			simpleLink.Landscape = true;
		}
		else
		{
			simpleLink.Landscape = false;
		}
		simpleLink.CreateDocument(buildForInstantPreview: true);
		simpleLink.ShowPrintPreviewDialog(Settings.HostingEnvironment, Caption.ToString());
	}

	private void SL_CreateDetail(object sender, DevExpress.Xpf.Printing.CreateAreaEventArgs e)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		int activeChartIndex = GetActiveChartIndex();
		if (activeChartIndex != -1)
		{
			VisualBrush brush = new VisualBrush(_chartsList[activeChartIndex]);
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = drawingVisual.RenderOpen();
			SimpleLink simpleLink = (SimpleLink)sender;
			drawingContext.DrawRectangle(brush, null, new Rect(0.0, 0.0, (double)(simpleLink.PrintingSystem.PageSettings.UsablePageSize.Width - 50f), _chartsList[activeChartIndex].ActualHeight));
			drawingContext.Close();
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)simpleLink.PrintingSystem.PageSettings.UsablePageSizeInPixels.Width, (int)simpleLink.PrintingSystem.PageSettings.UsablePageSizeInPixels.Height, 96.0, 96.0, PixelFormats.Pbgra32);
			renderTargetBitmap.Render(drawingVisual);
			e.Data = renderTargetBitmap;
		}
	}

	private int GetActiveChartIndex()
	{
		if (ChartGroup.IsVisible)
		{
			for (int i = 0; i <= ChartGroup.Items.Count - 1; i++)
			{
				if (ChartGroup.Items[i].IsActive)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private void Grid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
	{
		if (Grid.SelectionMode == MultiSelectMode.Cell)
		{
			e.Handled = false;
			return;
		}
		if (Grid.View == TreeView || (e.Action == CollectionChangeAction.Refresh && !Grid.IsGroupRowHandle(Grid.View.FocusedRowHandle)))
		{
			ShowOnStatusBar();
			return;
		}
		bool flag = (Grid.View.SearchControl != null && Grid.View.SearchControl.GetIsKeyboardFocusWithin()) || Grid.View.FocusedRowHandle == -2147483645;
		if (!_selectByKeyboard && !flag)
		{
			int controllerRow = e.ControllerRow;
			if (controllerRow >= 0 && e.Action != CollectionChangeAction.Refresh)
			{
				int listIndexByRowHandle = Grid.GetListIndexByRowHandle(controllerRow);
				if (e.Action == CollectionChangeAction.Add)
				{
					selectedRows.Add(listIndexByRowHandle);
				}
				else if (e.Action == CollectionChangeAction.Remove)
				{
					selectedRows.Remove(listIndexByRowHandle);
					if (Grid.FilterString != "" && Grid.VisibleRowCount > 0)
					{
						Grid.RefreshRow(controllerRow);
						controllerRow = Grid.GetRowHandleByVisibleIndex(Grid.VisibleRowCount - 1);
						Grid.RefreshRow(controllerRow);
					}
				}
				else if (MemoryExtensions.Contains<int>((ReadOnlySpan<int>)Grid.GetSelectedRowHandles(), controllerRow))
				{
					selectedRows.Add(listIndexByRowHandle);
				}
				else
				{
					selectedRows.Remove(listIndexByRowHandle);
				}
			}
		}
		UpdateSelectedStatus();
	}

	private void UpdateSelected(bool rowMode)
	{
		if (GetSelectedRows().Count == 0 && selectedRows.Count == 0)
		{
			return;
		}
		if (Grid.View == GridView)
		{
			Mouse.OverrideCursor = Cursors.Wait;
			if (_rowSelectedChanged)
			{
				List<int> list;
				if (rowMode)
				{
					if (Grid.IsGroupRowHandle(_selectedRowHandle))
					{
						list = GetDataRowHandlesInGroup(_selectedRowHandle);
					}
					else
					{
						int num = 1;
						List<int> list2 = new List<int>(num);
						CollectionsMarshal.SetCount(list2, num);
						CollectionsMarshal.AsSpan(list2)[0] = _selectedRowHandle;
						list = list2;
					}
				}
				else
				{
					list = GetDataRowHandles();
				}
				foreach (int item in list)
				{
					if (!Grid.IsGroupRowHandle(item))
					{
						if (GridView.IsRowSelected(item))
						{
							int listIndexByRowHandle = Grid.GetListIndexByRowHandle(item);
							selectedRows.Add(listIndexByRowHandle);
						}
						else
						{
							int listIndexByRowHandle2 = Grid.GetListIndexByRowHandle(item);
							selectedRows.Remove(listIndexByRowHandle2);
						}
					}
				}
			}
			else
			{
				Grid.BeginSelection();
				List<int> list;
				if (rowMode)
				{
					if (Grid.IsGroupRowHandle(_selectedRowHandle))
					{
						list = GetDataRowHandlesInGroup(_selectedRowHandle);
					}
					else
					{
						int num = 1;
						List<int> list3 = new List<int>(num);
						CollectionsMarshal.SetCount(list3, num);
						CollectionsMarshal.AsSpan(list3)[0] = _selectedRowHandle;
						list = list3;
					}
				}
				else
				{
					list = GetDataRowHandles();
				}
				foreach (int item2 in list)
				{
					int listIndexByRowHandle3 = Grid.GetListIndexByRowHandle(item2);
					if (!selectedRows.Contains(listIndexByRowHandle3) && GridView.IsRowSelected(item2))
					{
						Grid.UnselectItem(item2);
					}
					else if (selectedRows.Contains(listIndexByRowHandle3) && !GridView.IsRowSelected(item2))
					{
						Grid.SelectItem(item2);
					}
				}
			}
			Grid.EndSelection();
			if (Grid.View == GridView)
			{
				try
				{
					Grid.UpdateLayout();
				}
				catch (Exception)
				{
				}
			}
		}
		_selectByKeyboard = false;
		Grid.UpdateTotalSummary();
		ShowOnStatusBar();
		_selectByKeyboard = false;
		Mouse.OverrideCursor = null;
	}

	private List<int> GetDataRowHandles()
	{
		List<int> list = new List<int>();
		for (int i = 0; i <= Grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = Grid.GetRowHandleByVisibleIndex(i);
			if (Grid.IsGroupRowHandle(rowHandleByVisibleIndex))
			{
				if (!Grid.IsGroupRowExpanded(rowHandleByVisibleIndex))
				{
					list.AddRange(GetDataRowHandlesInGroup(rowHandleByVisibleIndex));
				}
			}
			else
			{
				list.Add(rowHandleByVisibleIndex);
			}
		}
		return list;
	}

	private List<int> GetDataRowHandlesInGroup(int groupRowHandle)
	{
		List<int> list = new List<int>();
		for (int i = 0; i <= Grid.GetChildRowCount(groupRowHandle) - 1; i++)
		{
			int childRowHandle = Grid.GetChildRowHandle(groupRowHandle, i);
			if (Grid.IsGroupRowHandle(childRowHandle))
			{
				list.AddRange(GetDataRowHandlesInGroup(childRowHandle));
			}
			else
			{
				list.Add(childRowHandle);
			}
		}
		return list;
	}

	private void Grid_PreviewKeyUp(object sender, KeyEventArgs e)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Invalid comparison between Unknown and I4
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Invalid comparison between Unknown and I4
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Invalid comparison between Unknown and I4
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Invalid comparison between Unknown and I4
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Invalid comparison between Unknown and I4
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Invalid comparison between Unknown and I4
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Invalid comparison between Unknown and I4
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Invalid comparison between Unknown and I4
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Invalid comparison between Unknown and I4
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Invalid comparison between Unknown and I4
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Invalid comparison between Unknown and I4
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Invalid comparison between Unknown and I4
		if (Grid.View.FocusedRowHandle == -2147483645 || Grid.View.ActualShowSearchPanel)
		{
			e.Handled = false;
		}
		else if (_selectByKeyboard && ((int)e.Key == 31 || (int)e.Key == 84 || (int)e.Key == 141 || (int)e.Key == 85 || (int)e.Key == 143 || (int)e.Key == 87 || (int)e.Key == 18 || ((int)e.Key == 26 && (int)Keyboard.Modifiers == 4) || ((int)e.Key == 24 && (int)Keyboard.Modifiers == 4) || ((int)e.Key == 31 && (int)Keyboard.Modifiers == 4) || (int)e.Key == 117 || (int)e.Key == 116))
		{
			Grid.EndSelection();
			_selectByKeyboard = false;
			Mouse.OverrideCursor = null;
			e.Handled = true;
		}
	}

	private void TreeView_CanSelectRow(object sender, CanSelectRowEventArgs e)
	{
		e.CanSelectRow = false;
	}

	private void TreeView_CanUnselectRow(object sender, CanUnselectRowEventArgs e)
	{
		e.CanUnselectRow = false;
	}

	private void TreeView_NodeCheckStateChanged(object sender, TreeListNodeEventArgs e)
	{
		if (_selectByKeyboard)
		{
			return;
		}
		if (e.Node.RowHandle >= 0)
		{
			if (e.Node.IsChecked.Value)
			{
				selectedRows.Add(Grid.GetListIndexByRowHandle(e.Node.RowHandle));
				Grid.SelectItem(e.Node.RowHandle);
			}
			else
			{
				selectedRows.Remove(Grid.GetListIndexByRowHandle(e.Node.RowHandle));
				Grid.UnselectItem(e.Node.RowHandle);
			}
		}
		UpdateSelectedStatus();
	}

	private void FilterSelectedChecked(object sender, RoutedEventArgs e)
	{
		CheckEdit checkEdit = (CheckEdit)sender;
		_selectByKeyboard = true;
		Mouse.OverrideCursor = Cursors.Wait;
		if (checkEdit.IsChecked.Value)
		{
			Grid.Columns["Selected"].AutoFilterValue = "True";
		}
		else
		{
			Grid.Columns["Selected"].AutoFilterValue = "";
		}
		_selectByKeyboard = false;
		Mouse.OverrideCursor = null;
	}

	private void Grid_StartGrouping(object sender, RoutedEventArgs e)
	{
		Mouse.OverrideCursor = Cursors.Wait;
	}

	private void Grid_EndGrouping(object sender, RoutedEventArgs e)
	{
		Mouse.OverrideCursor = null;
	}

	private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (GridView == null)
		{
			return;
		}
		TableViewHitInfo tableViewHitInfo = GridView.CalcHitInfo(e.GetPosition(GridView));
		if (tableViewHitInfo.InColumnHeader)
		{
			if (tableViewHitInfo != null && tableViewHitInfo.Column?.VisibleIndex == 0)
			{
				SelectedFilter = !SelectedFilter;
				e.Handled = true;
			}
		}
		else if (tableViewHitInfo.HitTest == TableViewHitTest.GroupRowCheckBox || (tableViewHitInfo.InRowCell && tableViewHitInfo.Column.VisibleIndex == 0))
		{
			_rowSelectedChanged = true;
			_selectedRowHandle = GridView.FocusedRowHandle;
		}
		_isInFilterPanel = tableViewHitInfo.InFilterPanel && !string.IsNullOrWhiteSpace(Grid.FilterString);
	}

	private void Grid_FilterChanged(object sender, RoutedEventArgs e)
	{
		if (Grid.View != GridView || _selectByKeyboard || selectedRows.Count <= 0)
		{
			return;
		}
		Mouse.OverrideCursor = Cursors.Wait;
		List<int> dataRowHandles = GetDataRowHandles();
		Grid.BeginSelection();
		foreach (int item in dataRowHandles)
		{
			if (!Grid.IsGroupRowHandle(item))
			{
				int listIndexByRowHandle = Grid.GetListIndexByRowHandle(item);
				if (GridView.IsRowSelected(item) && !selectedRows.Contains(listIndexByRowHandle))
				{
					Grid.UnselectItem(item);
				}
				else if (!GridView.IsRowSelected(item) && selectedRows.Contains(listIndexByRowHandle))
				{
					Grid.SelectItem(item);
				}
			}
		}
		Grid.EndSelection();
		Grid.UpdateTotalSummary();
		Mouse.OverrideCursor = null;
		string text = Grid.FilterString.Replace(" ", "");
		if (!string.IsNullOrEmpty(text) && text.Contains("[Selected]='True'"))
		{
			SelectedFilter = true;
		}
		else
		{
			SelectedFilter = false;
		}
	}

	private void DetailLayoutManager_ShowingMenu(object sender, ShowingMenuEventArgs e)
	{
		if (e.TargetElement is LayoutPanel layoutPanel && (!(layoutPanel.Name != "TotalsPanel") || !(layoutPanel.Name != "TotalsPanelRight")))
		{
			_ = (ItemContextMenu)e.Menu;
			BarButtonItem barButtonItem = new BarButtonItem
			{
				Name = "btnDockTotalsPanel"
			};
			if (layoutPanel.Name == "TotalsPanel")
			{
				barButtonItem.Content = ArmSoft.SME.Common.Resources.DockToRight;
			}
			else
			{
				barButtonItem.Content = ArmSoft.SME.Common.Resources.DockToBottom;
			}
			barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(DockTotalsPanel).Handler;
			e.ActionList.Add(barButtonItem);
		}
	}

	private void DockTotalsPanel(object sender, ItemClickEventArgs e)
	{
		showTotalsAtRight = !showTotalsAtRight;
		ShowTotalsPanel();
	}

	public void RefreshDataView()
	{
		Refresh();
	}

	public void UpdateDataView(object key)
	{
		Update(key);
	}

	public bool GetUserDefinedContextFunctionItemHandler(ContextFunctionDefinition item)
	{
		try
		{
			Type type = DefinitionMappingsService.ResolveCF(item.Group, item.Name);
			if (type != null)
			{
				IContextFunctionHandler contextFunctionHandler = (IContextFunctionHandler)ActivatorUtilities.CreateInstance(Settings.ServiceProvider, type);
				if (contextFunctionHandler != null)
				{
					contextFunctionHandler.SetRows(GetSelectedOrFocusedRows<IDynamicValue>());
					contextFunctionHandler.UserDefinedContextFunction(this);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		return true;
	}

	public List<object> GetDataViewSelectedRows()
	{
		return GetSelectedRows<object>();
	}

	public object GetDataViewFocusedRow()
	{
		return FocusedRow();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
