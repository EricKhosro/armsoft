using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.Documents.Goods;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Data;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class FilteredSearchWindow : DialogWindow
{
	private bool firstActivateCall = true;

	private LayoutGroup filterSearchGroup;

	private LayoutGroup filterGroup;

	private GridControl filterGrid;

	private TreeListView filterGridView;

	private GridControl searchGrid;

	private DXTabItem filteredSearchTab;

	private GridControl filteredSearchGrid;

	private TableView filteredSearchTableView;

	private LayoutGroup resultsLayoutGroup;

	private BarButtonItem addToResults;

	private LayoutGroup аdditionalInfoLayoutGroup;

	private GridControl resultsGrid;

	private TableView resultsTableView;

	private BarButtonItem deleteRow;

	private DXTabControl searchTabControl;

	private readonly FilteredSearch filteredSearch;

	private CheckEditExt requestQuantity;

	private readonly bool isBasketMode;

	public object SimpleResult { get; set; }

	public List<DocumentGridRow> BasketResult { get; set; }

	public FilteredSearchWindow(FilteredSearch filteredSearch, Window owner, bool isBasketMode)
		: base(owner)
	{
		this.filteredSearch = filteredSearch;
		this.isBasketMode = isBasketMode;
		ConstructUI();
		if (isBasketMode)
		{
			BasketResult = new List<DocumentGridRow>();
		}
		WindowStartupLocation = filteredSearch.WindowStartupLocation;
		Title = filteredSearch.Caption;
		Icon = Settings.HostingEnvironment.Icon;
		filterGrid.FontWeight = FontWeights.Bold;
		filterGridView.KeyFieldName = filteredSearch.FilterKeyFieldName;
		filterGridView.ParentFieldName = filteredSearch.FilterParentFieldName;
		ConfigureGridColumnsAndData(filterGrid, filteredSearch.FilterDataView);
		ConfigureGridColumnsAndData(searchGrid, filteredSearch.SearchDataView);
		if (filteredSearch.FilteredSearchDataView != null)
		{
			filteredSearchTab.Visibility = Visibility.Visible;
			filteredSearchTab.Header = filteredSearch.FilteredSearchDataView.Caption;
			ConfigureGridColumnsAndData(filteredSearchGrid, filteredSearch.FilteredSearchDataView);
		}
		if (!isBasketMode)
		{
			resultsLayoutGroup.Visibility = Visibility.Collapsed;
			_okButton.Visibility = Visibility.Collapsed;
			_cancelButton.Visibility = Visibility.Collapsed;
			аdditionalInfoLayoutGroup.Visibility = Visibility.Collapsed;
			addToResults.IsVisible = false;
		}
		else if (filteredSearch.ResultsList != null)
		{
			ConfigureResultListColumns();
			resultsGrid.ItemsSource = filteredSearch.ResultsList.ItemsSource;
			deleteRow.Content = ArmSoft.SME.Common.Resources.Delete + " (Ctrl+D)";
		}
		Activated += new SafeEventHandler<EventArgs>(FilteredSearchWindow_Activated).Handler;
		filterGrid.SelectedItemChanged += new SafeEventHandler<SelectedItemChangedEventArgs>(FilterGrid_SelectedItemChanged).Handler;
		filterGridView.ShowGridMenu += new SafeEventHandler<GridMenuEventArgs>(ShowGridMenu).Handler;
		filterGridView.PreviewMouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(FilterGridView_PreviewMouseDoubleClick).Handler;
		SafeEventHandler<MouseButtonEventArgs> safeEventHandler = new SafeEventHandler<MouseButtonEventArgs>(SourceGridView_PreviewMouseDoubleClick);
		searchGrid.View.PreviewMouseDoubleClick += safeEventHandler.Handler;
		filteredSearchTableView.PreviewMouseDoubleClick += safeEventHandler.Handler;
		SafeEventHandler<KeyEventArgs> safeEventHandler2 = new SafeEventHandler<KeyEventArgs>(SearchGridView_PreviewKeyDown);
		searchGrid.View.PreviewKeyDown += safeEventHandler2.Handler;
		filteredSearchTableView.PreviewKeyDown += safeEventHandler2.Handler;
		resultsTableView.KeyDown += new SafeEventHandler<KeyEventArgs>(ResultsGridView_PreviewKeyDown).Handler;
		filterGrid.CustomUnboundColumnData += new SafeEventHandler<GridColumnDataEventArgs>(FilterGrid_CustomUnboundColumnData).Handler;
		searchGrid.CustomUnboundColumnData += new SafeEventHandler<GridColumnDataEventArgs>(SearchGrid_CustomUnboundColumnData).Handler;
		filteredSearchGrid.CustomUnboundColumnData += new SafeEventHandler<GridColumnDataEventArgs>(FilteredSearchGrid_CustomUnboundColumnData).Handler;
		resultsGrid.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(ResultsGrid_PreviewKeyDown).Handler;
		searchGrid.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(SearchGrid_PreviewKeyDown).Handler;
		searchTabControl.SelectionChanged += new SafeEventHandler<TabControlSelectionChangedEventArgs>(SearchTabControl_SelectionChanged).Handler;
		resultsTableView.CellValueChanged += new SafeEventHandler<CellValueChangedEventArgs>(ResultsGridView_CellValueChanged).Handler;
	}

	private void FilteredSearchWindow_Activated(object sender, EventArgs e)
	{
		if (firstActivateCall)
		{
			firstActivateCall = false;
			filterGrid.Focus();
			FilterChanged();
		}
	}

	private void FilterGrid_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
	{
		FilterChanged();
	}

	private void FilterChanged()
	{
		object focusedRow = filterGrid.GetFocusedRow();
		if (focusedRow == null)
		{
			return;
		}
		IFilterKey filterKey = (IFilterKey)focusedRow;
		object filterKey2 = filterKey.FilterKey;
		object filterPath = filterKey.FilterPath;
		List<object> list = new List<object>();
		foreach (IMatchedFilterKey datum in GetDataViewFromActiveTab().Data)
		{
			if (datum.FilterMatched(filterKey2, filterPath))
			{
				list.Add(datum);
			}
		}
		GetGridFromActiveTab().ItemsSource = list;
		GetGridFromActiveTab().RefreshData();
	}

	private void ShowGridMenu(object sender, GridMenuEventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		e.Customizations.Add(new BarItemLinkSeparator());
		BarButtonItem barButtonItem = new BarButtonItem
		{
			Name = "ExpandGroups",
			Content = ArmSoft.SME.Common.Resources.ExpandGroups,
			Glyph = ImageService.GetImage("plus_16.svg")
		};
		barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ExpandGroups_ItemClick).Handler;
		e.Customizations.Add(barButtonItem);
		barButtonItem = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.CollapseGroups,
			Name = "CollapseGroups",
			Glyph = ImageService.GetImage("minus_16.svg")
		};
		barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(CollapseGroups_ItemClick).Handler;
		e.Customizations.Add(barButtonItem);
		e.Handled = false;
	}

	private void ExpandGroups_ItemClick(object sender, ItemClickEventArgs e)
	{
		filterGridView.ExpandAllNodes();
	}

	private void CollapseGroups_ItemClick(object sender, ItemClickEventArgs e)
	{
		filterGridView.CollapseAllNodes();
	}

	private void SourceGridView_PreviewMouseDoubleClick(object sender, MouseEventArgs e)
	{
		int rowHandleByMouseEventArgs = GetTableViewFromActiveTab().GetRowHandleByMouseEventArgs(e);
		if (rowHandleByMouseEventArgs != int.MinValue && rowHandleByMouseEventArgs != -2147483645)
		{
			ProcessEnter();
		}
	}

	private void FilterGridView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		if (!(sender is TreeListView { FocusedNode: { HasChildren: not false } focusedNode } treeListView))
		{
			return;
		}
		if (focusedNode.IsExpanded)
		{
			treeListView.CollapseNode(focusedNode.RowHandle);
		}
		else
		{
			treeListView.ExpandNode(focusedNode.RowHandle);
			foreach (TreeListNode node in focusedNode.Nodes)
			{
				treeListView.CollapseNode(node.RowHandle);
			}
		}
		e.Handled = true;
	}

	private void SearchGridView_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0)
		{
			GetTableViewFromActiveTab().CommitEditing();
			ProcessEnter();
			e.Handled = true;
		}
	}

	private void ProcessEnter()
	{
		object focusedRow = GetGridFromActiveTab().GetFocusedRow();
		if (focusedRow == null)
		{
			return;
		}
		if (!isBasketMode)
		{
			SimpleResult = ((IMatchedFilterKey)focusedRow).FilterKey;
			DialogResult = true;
			return;
		}
		object[] array = PrepareArgsBeforeProcessingEnter();
		if (array != null)
		{
			int num = filteredSearch.ResultsList.AddToItemsSource(((IMatchedFilterKey)focusedRow).FilterKey, array);
			resultsGrid.RefreshData();
			if (num != -1)
			{
				int rowHandleByVisibleIndex = resultsGrid.GetRowHandleByVisibleIndex(num);
				resultsTableView.MoveFocusedRow(resultsGrid.GetRowVisibleIndexByHandle(rowHandleByVisibleIndex));
			}
		}
	}

	private object[] PrepareArgsBeforeProcessingEnter()
	{
		if (requestQuantity.IsChecked.Value)
		{
			DialogWindow dialogWindow = new DialogWindow(this);
			LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
			dialogWindow.AddNumericControl(container, "Quantity", ArmSoft.SME.Common.Resources.Quantity, 13, 3).Value = 1m;
			if (dialogWindow.ShowDialog().Value)
			{
				return new object[1] { dialogWindow["Quantity"] };
			}
			return null;
		}
		return new object[1] { 1m };
	}

	private void AddToResults_ItemClick(object sender, ItemClickEventArgs e)
	{
		ProcessEnter();
	}

	private void ResultsGridView_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if (((int)e.Key == 32 && !resultsTableView.IsEditing) || ((int)e.Key == 47 && (int)Keyboard.Modifiers == 2))
		{
			e.Handled = true;
			DeleteRowFromResultsGrid();
		}
		else if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0)
		{
			resultsTableView.CommitEditing();
			resultsTableView.HideEditor();
		}
	}

	private IView GetDataViewFromActiveTab()
	{
		if (!((object)searchTabControl.SelectedContainer).Equals((object?)filteredSearchTab))
		{
			return filteredSearch.SearchDataView;
		}
		return filteredSearch.FilteredSearchDataView;
	}

	private GridControl GetGridFromActiveTab()
	{
		if (!((object)searchTabControl.SelectedContainer).Equals((object?)filteredSearchTab))
		{
			return searchGrid;
		}
		return filteredSearchGrid;
	}

	private TableView GetTableViewFromActiveTab()
	{
		if (!((object)searchTabControl.SelectedContainer).Equals((object?)filteredSearchTab))
		{
			return (TableView)searchGrid.View;
		}
		return filteredSearchTableView;
	}

	private void ConfigureGridColumnsAndData(GridControl grid, IView sourceDataView)
	{
		grid.AutoGenerateColumns = AutoGenerateColumnsMode.None;
		foreach (ViewColumn view in sourceDataView.Columns.ViewList)
		{
			grid.Columns.Add(GenerateGridColumn(view, sourceDataView.GetColumnType(view.Source)));
		}
		sourceDataView.LoadData();
		grid.ItemsSource = sourceDataView.Data;
	}

	private GridColumn GenerateGridColumn(ViewColumn viewColumn, DataColumnType dataType)
	{
		GridColumn gridColumn = new GridColumn
		{
			Name = viewColumn.Name,
			FieldName = viewColumn.Source,
			Header = viewColumn.Caption,
			Visible = viewColumn.Visible,
			AllowGrouping = DefaultBoolean.False,
			AllowEditing = DefaultBoolean.False,
			ReadOnly = true,
			AllowColumnFiltering = DefaultBoolean.True,
			FilterPopupMode = FilterPopupMode.CheckedList
		};
		if (viewColumn.Converter != null)
		{
			Binding binding = new Binding(viewColumn.Name)
			{
				Converter = viewColumn.Converter,
				Mode = BindingMode.TwoWay
			};
			gridColumn.Binding = binding;
			if (viewColumn.Format == "Image")
			{
				gridColumn.CellTemplate = Resources["ColumnToImage"] as DataTemplate;
				gridColumn.UnboundType = UnboundColumnType.Object;
				gridColumn.AllowEditing = DefaultBoolean.False;
			}
			return gridColumn;
		}
		switch (dataType)
		{
		case DataColumnType.String:
			if (viewColumn.IsDynamic)
			{
				gridColumn.UnboundType = UnboundColumnType.String;
			}
			gridColumn.AutoFilterCriteria = ClauseType.Contains;
			break;
		case DataColumnType.DateTime:
			if (viewColumn.IsDynamic)
			{
				gridColumn.UnboundType = UnboundColumnType.DateTime;
			}
			gridColumn.EditSettings = new DateEditSettings();
			if (viewColumn.Format == string.Empty)
			{
				gridColumn.EditSettings.DisplayFormat = "dd/MM/yy HH:mm:ss";
			}
			else
			{
				gridColumn.EditSettings.DisplayFormat = viewColumn.Format;
			}
			break;
		case DataColumnType.DateOnly:
			if (viewColumn.IsDynamic)
			{
				gridColumn.UnboundType = UnboundColumnType.DateTime;
			}
			gridColumn.EditSettings = new DateEditSettings();
			if (viewColumn.Format == string.Empty)
			{
				gridColumn.EditSettings.DisplayFormat = "dd/MM/yy";
			}
			else
			{
				gridColumn.EditSettings.DisplayFormat = viewColumn.Format;
			}
			break;
		case DataColumnType.Decimal:
			if (viewColumn.IsDynamic)
			{
				gridColumn.UnboundType = UnboundColumnType.Decimal;
			}
			gridColumn.EditSettings = new TextEditSettings();
			gridColumn.DisplayTemplate = (ControlTemplate)Settings.GenericResources["ColumnDisplayTemplate"];
			if (viewColumn.Format == string.Empty)
			{
				gridColumn.EditSettings.DisplayFormat = DecimalExtension.GetCurrencyFormat(16, 2);
			}
			else
			{
				gridColumn.EditSettings.DisplayFormat = viewColumn.Format;
			}
			break;
		case DataColumnType.Integer:
			if (viewColumn.IsDynamic)
			{
				gridColumn.UnboundType = UnboundColumnType.Integer;
			}
			if (!string.IsNullOrWhiteSpace(viewColumn.Format))
			{
				gridColumn.EditSettings = new TextEditSettings();
				gridColumn.DisplayTemplate = (ControlTemplate)Settings.GenericResources["ColumnDisplayTemplate"];
				gridColumn.EditSettings.DisplayFormat = viewColumn.Format;
			}
			break;
		case DataColumnType.Boolean:
			if (viewColumn.IsDynamic)
			{
				gridColumn.UnboundType = UnboundColumnType.Boolean;
			}
			gridColumn.ShowCriteriaInAutoFilterRow = false;
			break;
		}
		return gridColumn;
	}

	protected void FilterGrid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
	{
		if (e.IsGetData)
		{
			IDynamicValue dynamicValue = (IDynamicValue)filteredSearch.FilterDataView.Data[e.ListSourceRowIndex];
			e.Value = dynamicValue.GetDynamicValue(e.Column.FieldName);
		}
	}

	protected void SearchGrid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
	{
		if (e.IsGetData)
		{
			IDynamicValue dynamicValue = (IDynamicValue)filteredSearch.SearchDataView.Data[e.ListSourceRowIndex];
			e.Value = dynamicValue.GetDynamicValue(e.Column.FieldName);
		}
	}

	protected void FilteredSearchGrid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
	{
		if (e.IsGetData)
		{
			IDynamicValue dynamicValue = (IDynamicValue)filteredSearch.FilteredSearchDataView.Data[e.ListSourceRowIndex];
			e.Value = dynamicValue.GetDynamicValue(e.Column.FieldName);
		}
	}

	private void ResultsGrid_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		if (isBasketMode && (int)e.Key == 6 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			resultsTableView.CommitEditing();
		}
	}

	private void SearchGrid_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if ((int)e.Key == 60 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			((TableView)searchGrid.View).ShowAutoFilterRow = !((TableView)searchGrid.View).ShowAutoFilterRow;
		}
	}

	private void DeleteRow_ItemClick(object sender, ItemClickEventArgs e)
	{
		DeleteRowFromResultsGrid();
	}

	private void DeleteRowFromResultsGrid()
	{
		resultsTableView.CommitEditing();
		resultsTableView.HideEditor();
		int focusedRowHandle = resultsTableView.FocusedRowHandle;
		resultsTableView.DeleteRow(focusedRowHandle);
	}

	private void SearchTabControl_SelectionChanged(object sender, TabControlSelectionChangedEventArgs e)
	{
		FilterChanged();
	}

	private void ResultsGridView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		if (!(resultsGrid.GetRow(e.RowHandle) is MTInvoiceWindow.MTInvoiceSpecificationRow mTInvoiceSpecificationRow))
		{
			return;
		}
		string name = e.Column.Name;
		if (!(name == "Quantity"))
		{
			if (name == "Price")
			{
				mTInvoiceSpecificationRow.Summa = PriceHelper.CalculateSum(Convert.ToDecimal(e.Value), mTInvoiceSpecificationRow.Quantity);
			}
		}
		else
		{
			mTInvoiceSpecificationRow.Summa = PriceHelper.CalculateSum(mTInvoiceSpecificationRow.Price, Convert.ToDecimal(e.Value));
		}
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		try
		{
			if (DialogResult.HasValue && DialogResult.Value && isBasketMode)
			{
				BasketResult = filteredSearch.ResultsList.ItemsSource.ToList();
			}
			if (isBasketMode && (!DialogResult.HasValue || !DialogResult.Value) && filteredSearch.ResultsList.ItemsSource.Any() && Settings.ParametersService.QuestWhenCloseWind() && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.QuestionWhenCloseWindow, filteredSearch.Caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, this) == MessageBoxResult.No)
			{
				e.Cancel = true;
			}
			base.OnClosing(e);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ConfigureResultListColumns()
	{
		foreach (FilteredSearchGridColumn column in filteredSearch.ResultsList.Columns)
		{
			GridColumn gridColumn = new GridColumn();
			gridColumn.Name = column.FieldName;
			gridColumn.FieldName = column.FieldName;
			gridColumn.Header = column.Header;
			gridColumn.AllowGrouping = DefaultBoolean.False;
			if (!column.AllowEditing)
			{
				gridColumn.AllowEditing = DefaultBoolean.False;
				gridColumn.ReadOnly = true;
				gridColumn.AllowFocus = false;
			}
			if (column.CustomWidth > 0.0)
			{
				gridColumn.Width = column.CustomWidth;
			}
			gridColumn.AllowColumnFiltering = DefaultBoolean.True;
			gridColumn.FilterPopupMode = FilterPopupMode.CheckedList;
			if (!string.IsNullOrEmpty(column.NumericFormat))
			{
				gridColumn.DisplayTemplate = (ControlTemplate)Resources["NumericColumnTemplate"];
				CalcEditSettings calcEditSettings = (CalcEditSettings)(gridColumn.EditSettings = new CalcEditSettings
				{
					MaskType = MaskType.Numeric,
					Mask = column.NumericFormat,
					MaskUseAsDisplayFormat = true,
					AllowNullInput = false,
					AllowSpinOnMouseWheel = false,
					ValidateOnTextInput = false
				});
				if (column.ShowTotalSummary)
				{
					resultsGrid.TotalSummary.Add(new GridSummaryItem
					{
						SummaryType = SummaryItemType.Sum,
						FieldName = gridColumn.FieldName,
						DisplayFormat = "{0:" + calcEditSettings.Mask + "}"
					});
					resultsTableView.ShowTotalSummary = true;
				}
			}
			resultsGrid.Columns.Add(gridColumn);
		}
	}

	private void ConstructUI()
	{
		ResizeMode = ResizeMode.CanResize;
		SizeToContent = SizeToContent.Manual;
		ShowInTaskbar = true;
		ShowIcon = true;
		Height = 600.0;
		Width = 900.0;
		if (!string.IsNullOrWhiteSpace(filteredSearch.Name))
		{
			_registryKey = filteredSearch.Name;
		}
		_layoutControl.Padding = new Thickness(8.0);
		_layoutControl.ScrollBars = ScrollBars.None;
		filterSearchGroup = AddHorizontalLayoutGroup();
		LayoutControl.SetAllowVerticalSizing(filterSearchGroup, value: true);
		AddFilterGroup(filterSearchGroup);
		LayoutGroupExt container = AddVerticalLayoutGroup(filterSearchGroup);
		searchTabControl = AddTabControl(container);
		AddSearchTab(searchTabControl);
		AddFilteredSerachTab(searchTabControl);
		аdditionalInfoLayoutGroup = AddHorizontalLayoutGroup(container);
		аdditionalInfoLayoutGroup.Height = 40.0;
		аdditionalInfoLayoutGroup.HorizontalAlignment = HorizontalAlignment.Right;
		requestQuantity = AddBooleanControl(аdditionalInfoLayoutGroup, "RequestQuantity", ArmSoft.SME.Trade.Common.Resources.RequestQuantity);
		AddResultsLayout();
	}

	private void AddResultsLayout()
	{
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		resultsLayoutGroup = AddHorizontalLayoutGroup();
		resultsLayoutGroup.HorizontalAlignment = HorizontalAlignment.Stretch;
		resultsLayoutGroup.VerticalAlignment = VerticalAlignment.Stretch;
		resultsLayoutGroup.Name = "ResultsLayoutGroup";
		resultsGrid = new GridControl
		{
			Name = "resultsGrid",
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			AllowLiveDataShaping = true,
			AutoGenerateColumns = AutoGenerateColumnsMode.None
		};
		resultsTableView = new TableView
		{
			Name = "resultsTableView",
			AllowColumnFiltering = false,
			AllowColumnMoving = true,
			AllowEditing = true,
			AllowGrouping = false,
			AllowLeaveFocusOnTab = true,
			AllowMoveColumnToDropArea = false,
			AllowSorting = true,
			EditorButtonShowMode = EditorButtonShowMode.ShowOnlyInEditor,
			EnterMoveNextColumn = true,
			IsGroupPanelMenuEnabled = false,
			IsRowCellMenuEnabled = true,
			IsTotalSummaryMenuEnabled = false,
			ItemsSourceErrorInfoShowMode = ItemsSourceErrorInfoShowMode.None,
			NewItemRowPosition = NewItemRowPosition.None,
			ScrollingMode = ScrollingMode.Smart,
			ShowColumnHeaders = true,
			ShowFocusedRectangle = false,
			ShowGroupPanel = false,
			ShowGroupedColumns = false,
			ShowTotalSummary = false,
			VerticalScrollbarVisibility = ScrollBarVisibility.Auto
		};
		resultsGrid.View = resultsTableView;
		PopupMenu popupMenu = new PopupMenu();
		deleteRow = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.Delete,
			Glyph = ImageService.GetImage("delete_16.svg")
		};
		deleteRow.ItemClick += new SafeEventHandler<ItemClickEventArgs>(DeleteRow_ItemClick).Handler;
		popupMenu.Items.Add(deleteRow);
		BarManager.SetDXContextMenu(resultsGrid, popupMenu);
		DialogWindow.AddControlWithoutName(resultsLayoutGroup, resultsGrid);
	}

	private void AddFilteredSerachTab(DXTabControl tabControl)
	{
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		filteredSearchTab = AddTabItem(tabControl, string.Empty);
		filteredSearchTab.Visibility = Visibility.Collapsed;
		filteredSearchGrid = new GridControl
		{
			Name = "filteredSearchGrid",
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			SelectionMode = MultiSelectMode.Cell
		};
		filteredSearchTableView = new TableView
		{
			Name = "filteredSearchTableView",
			AllowBestFit = true,
			AllowColumnFiltering = false,
			AllowColumnMoving = true,
			AllowGrouping = false,
			AllowLeaveFocusOnTab = true,
			AllowMoveColumnToDropArea = false,
			AllowSorting = true,
			EditorButtonShowMode = EditorButtonShowMode.ShowOnlyInEditor,
			EnterMoveNextColumn = true,
			IsGroupPanelMenuEnabled = false,
			IsRowCellMenuEnabled = true,
			IsTotalSummaryMenuEnabled = false,
			NavigationStyle = GridViewNavigationStyle.Row,
			NewItemRowPosition = NewItemRowPosition.None,
			ScrollingMode = ScrollingMode.Smart,
			ShowColumnHeaders = true,
			ShowFocusedRectangle = false,
			ShowGroupPanel = false,
			ShowGroupedColumns = false,
			ShowTotalSummary = false,
			VerticalScrollbarVisibility = ScrollBarVisibility.Auto
		};
		filteredSearchGrid.View = filteredSearchTableView;
		PopupMenu popupMenu = new PopupMenu();
		BarButtonItem barButtonItem = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.NewAdd,
			Glyph = ImageService.GetImage("add_16.svg")
		};
		barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(AddToResults_ItemClick).Handler;
		popupMenu.Items.Add(barButtonItem);
		BarManager.SetDXContextMenu(filteredSearchGrid, popupMenu);
		DialogWindow.AddControlWithoutName((LayoutGroup)filteredSearchTab.Content, filteredSearchGrid);
	}

	private void AddFilterGroup(LayoutGroup lg)
	{
		filterGroup = AddVerticalLayoutGroup(lg);
		filterGroup.Width = 320.0;
		filterGroup.HorizontalAlignment = HorizontalAlignment.Stretch;
		filterGroup.VerticalAlignment = VerticalAlignment.Stretch;
		filterGroup.Name = "FilterGroup";
		LayoutControl.SetAllowHorizontalSizing(filterGroup, value: true);
		filterGrid = new GridControl
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			SelectionMode = MultiSelectMode.Cell,
			Name = "FilterGrid"
		};
		filterGridView = new TreeListView
		{
			AllowColumnFiltering = false,
			AllowColumnMoving = true,
			AllowIndeterminateCheckState = false,
			AllowLeaveFocusOnTab = true,
			AllowMoveColumnToDropArea = false,
			AllowRecursiveNodeChecking = false,
			AllowSorting = true,
			AutoExpandAllNodes = true,
			AutoWidth = true,
			EditorButtonShowMode = EditorButtonShowMode.ShowOnlyInEditor,
			EditorShowMode = EditorShowMode.MouseDown,
			EnterMoveNextColumn = true,
			IsRowCellMenuEnabled = false,
			IsTotalSummaryMenuEnabled = false,
			NavigationStyle = GridViewNavigationStyle.Row,
			ScrollingMode = ScrollingMode.Smart,
			ShowCheckboxes = false,
			ShowColumnHeaders = true,
			ShowFocusedRectangle = false,
			ShowTotalSummary = false,
			TreeDerivationMode = TreeDerivationMode.Selfreference,
			VerticalScrollbarVisibility = ScrollBarVisibility.Auto
		};
		filterGrid.View = filterGridView;
		DialogWindow.AddControlWithoutName(filterGroup, filterGrid);
	}

	private void AddSearchTab(DXTabControl tabControl)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		LayoutGroup layoutGroup = AddTabedGroup(tabControl, ArmSoft.SME.Common.Resources.All);
		layoutGroup.Name = "SearchTab";
		searchGrid = new GridControl
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			SelectionMode = MultiSelectMode.Cell,
			Name = "SearchGrid"
		};
		PopupMenu popupMenu = new PopupMenu();
		addToResults = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.NewAdd,
			Glyph = ImageService.GetImage("add_16.svg")
		};
		addToResults.ItemClick += new SafeEventHandler<ItemClickEventArgs>(AddToResults_ItemClick).Handler;
		popupMenu.Items.Add(addToResults);
		BarManager.SetDXContextMenu(searchGrid, popupMenu);
		TableView view = new TableView
		{
			AllowBestFit = true,
			AllowColumnFiltering = false,
			AllowColumnMoving = true,
			AllowGrouping = false,
			AllowLeaveFocusOnTab = true,
			AllowMoveColumnToDropArea = false,
			AllowSorting = true,
			EditorButtonShowMode = EditorButtonShowMode.ShowOnlyInEditor,
			EditorShowMode = EditorShowMode.MouseDown,
			EnterMoveNextColumn = true,
			IsGroupPanelMenuEnabled = false,
			IsRowCellMenuEnabled = true,
			IsTotalSummaryMenuEnabled = false,
			NavigationStyle = GridViewNavigationStyle.Row,
			NewItemRowPosition = NewItemRowPosition.None,
			ScrollingMode = ScrollingMode.Smart,
			ShowColumnHeaders = true,
			ShowCriteriaInAutoFilterRow = true,
			ShowFocusedRectangle = false,
			ShowGroupPanel = false,
			ShowGroupedColumns = false,
			ShowTotalSummary = false,
			VerticalScrollbarVisibility = ScrollBarVisibility.Auto
		};
		searchGrid.View = view;
		DialogWindow.AddControlWithoutName(layoutGroup, searchGrid);
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		if ((isBasketMode && WindowState != WindowState.Minimized) || (!isBasketMode && WindowState == WindowState.Normal))
		{
			rk.SetValue("Width", Width);
			rk.SetValue("Height", Height);
			rk.SetValue("FilterWidth", filterGroup.Width);
			if (isBasketMode)
			{
				rk.SetValue("WindowState", WindowState);
				rk.SetValue("FilterSearchHeight", filterSearchGroup.Height);
				rk.SetValue("RequestQuantity", requestQuantity.IsChecked);
			}
			SaveColumnWidthsInRegistry(filterGrid.Columns, rk.CreateSubKey("FilterColumns"));
			SaveColumnWidthsInRegistry(searchGrid.Columns, rk.CreateSubKey("SearchColumns"));
			SaveColumnWidthsInRegistry(filteredSearchGrid.Columns, rk.CreateSubKey("FilteredSearchColumns"));
		}
	}

	protected override void RestoreSettings(RegistryKey rk)
	{
		if (double.TryParse(rk.GetValue("Width", "0").ToString(), out var result) && result > 0.0)
		{
			Width = result;
		}
		if (double.TryParse(rk.GetValue("Height", "0").ToString(), out var result2) && result2 > 0.0)
		{
			Height = result2;
		}
		if (double.TryParse(rk.GetValue("FilterWidth", "0").ToString(), out var result3) && result3 > 0.0)
		{
			filterGroup.Width = result3;
		}
		if (isBasketMode)
		{
			if (double.TryParse(rk.GetValue("FilterSearchHeight", "0").ToString(), out var result4))
			{
				if (result4 > 0.0)
				{
					filterSearchGroup.Height = result4;
				}
				else
				{
					filterSearchGroup.Height = 400.0;
				}
			}
			if (Enum.TryParse<WindowState>(rk.GetValue("WindowState", WindowState.Normal).ToString(), out var result5))
			{
				WindowState = ((result5 != WindowState.Minimized) ? result5 : WindowState.Normal);
			}
			requestQuantity.IsChecked = bool.Parse(rk.GetValue("RequestQuantity", "False").ToString());
		}
		else
		{
			WindowState = WindowState.Normal;
		}
		LoadColumnWidthsFromRegistry(filterGrid.Columns, rk.OpenSubKey("FilterColumns", writable: false));
		LoadColumnWidthsFromRegistry(searchGrid.Columns, rk.OpenSubKey("SearchColumns", writable: false));
		LoadColumnWidthsFromRegistry(filteredSearchGrid.Columns, rk.OpenSubKey("FilteredSearchColumns", writable: false));
	}

	private static void SaveColumnWidthsInRegistry(GridColumnCollection columns, RegistryKey registryKey)
	{
		if (registryKey == null)
		{
			return;
		}
		foreach (GridColumn column in columns)
		{
			string text = column.ActualWidth.ToString();
			if (text != "NaN")
			{
				registryKey.SetValue(column.Name.Trim(), text);
			}
		}
		registryKey.Close();
	}

	private static void LoadColumnWidthsFromRegistry(GridColumnCollection columns, RegistryKey registryKey)
	{
		if (registryKey == null)
		{
			return;
		}
		foreach (GridColumn column in columns)
		{
			if (double.TryParse(registryKey.GetValue(column.Name.Trim(), "0").ToString(), out var result) && result > 0.0)
			{
				column.Width = result;
			}
		}
		registryKey.Close();
	}
}
