using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Converters;
using ArmSoft.SME.Core.UI.HelpClasses;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Microsoft.Win32;
using Serilog;

namespace ArmSoft.SME.Core.UI.DataViews;

public class ModalBrowserWindow : ThemedWindow
{
	public delegate void ModalBrowserRowChangedDelegate(int visibleRowNumber, int totalVisibleRows);

	public delegate void ModalBrowserClosedDelegate();

	protected readonly ModalBrowser _mb;

	private readonly List<bool> _selectedRows;

	private readonly HeaderItemControl _exportToCsvHeaderItem;

	private readonly HeaderItemControl _reloadHeaderItem;

	private readonly HeaderItemControl _closedFilterHeaderItem;

	private bool _closedFilterIsChecked;

	private readonly DXImage _closedFilterIsCheckedImage;

	private readonly DXImage _closedFilterIsUncheckedImage;

	private readonly bool _isMultiSelect;

	private bool _firstActivateCall;

	private int _selectedRowIndex;

	private int _onlyOneSelectedRowIndex = -1;

	private bool _useBestFits;

	private readonly Button _okButton;

	private readonly Button _cancelButton;

	protected readonly TextBlock _txtStatus;

	protected readonly GridControl _grid;

	private readonly TableView _view;

	protected readonly Grid _buttonsPanel;

	private readonly MyTreeView _treeView;

	private bool _changeFilterAuto;

	private string _lookUpValue;

	public List<int> SelectedRowsIndexes { get; set; }

	public ModalBrowserClosedDelegate ClosedDelegate { get; set; }

	public bool MultiSelect => _isMultiSelect;

	public int SelectedRowIndex => _selectedRowIndex;

	public string LookUpValue
	{
		get
		{
			return _lookUpValue;
		}
		set
		{
			_lookUpValue = (string.IsNullOrWhiteSpace(value) ? string.Empty : value.TrimEnd());
		}
	}

	public ModalBrowserWindow(ModalBrowser mb, bool multiSelect, Window owner, bool modeSearchPanel = false)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		Title = "ModalBrowser";
		Height = 257.0;
		Width = 393.0;
		WindowStyle = WindowStyle.SingleBorderWindow;
		ShowIcon = false;
		SizeToContent = SizeToContent.Manual;
		WindowState = WindowState.Normal;
		TitleAlignment = WindowTitleAlignment.Center;
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		ControlBoxButtonSet = ControlBoxButtons.Close | ControlBoxButtons.MaximizeRestore;
		if (mb.DataView.ReloadMode != ReloadMode.None)
		{
			DXImage content = new DXImage
			{
				Source = ImageService.GetImage("refresh_small_16.svg"),
				Width = 18.0,
				Height = 18.0,
				SnapsToDevicePixels = true,
				UseLayoutRounding = true,
				Stretch = Stretch.Uniform
			};
			_reloadHeaderItem = new HeaderItemControl
			{
				Name = "Reload",
				Content = content,
				ToolTip = ArmSoft.SME.Common.Resources.RefreshTooltip
			};
			_reloadHeaderItem.Click += new SafeEventHandler<RoutedEventArgs>(ReloadHeaderItem_Click).Handler;
			KeyDown += new SafeEventHandler<KeyEventArgs>(ModalBrowserWindow_KeyDown).Handler;
			HeaderItems.Add(_reloadHeaderItem);
		}
		DockPanel dockPanel = (DockPanel)(Content = new DockPanel
		{
			LastChildFill = true
		});
		_buttonsPanel = new Grid
		{
			Name = "ButtonsPanel"
		};
		dockPanel.Children.Add(_buttonsPanel);
		DockPanel.SetDock(_buttonsPanel, Dock.Bottom);
		System.Windows.Controls.ColumnDefinition value = new System.Windows.Controls.ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		};
		System.Windows.Controls.ColumnDefinition value2 = new System.Windows.Controls.ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Auto)
		};
		System.Windows.Controls.ColumnDefinition value3 = new System.Windows.Controls.ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Auto)
		};
		System.Windows.Controls.ColumnDefinition value4 = new System.Windows.Controls.ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		};
		_buttonsPanel.ColumnDefinitions.Add(value);
		_buttonsPanel.ColumnDefinitions.Add(value2);
		_buttonsPanel.ColumnDefinitions.Add(value3);
		_buttonsPanel.ColumnDefinitions.Add(value4);
		System.Windows.Controls.RowDefinition value5 = new System.Windows.Controls.RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Auto)
		};
		_buttonsPanel.RowDefinitions.Add(value5);
		_txtStatus = new TextBlock
		{
			Name = "txtStatus",
			Margin = new Thickness(5.0, 5.0, 5.0, 5.0),
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Center,
			MinWidth = 150.0,
			UseLayoutRounding = true,
			Text = ""
		};
		Grid.SetRow(_txtStatus, 0);
		Grid.SetColumn(_txtStatus, 0);
		_buttonsPanel.Children.Add(_txtStatus);
		_okButton = new Button
		{
			Name = "OkButton",
			Margin = new Thickness(15.0, 5.0, 15.0, 5.0),
			HorizontalContentAlignment = HorizontalAlignment.Center,
			MinWidth = 100.0,
			TabIndex = 2,
			IsDefault = true,
			UseLayoutRounding = true,
			Content = "OK"
		};
		Grid.SetRow(_okButton, 0);
		Grid.SetColumn(_okButton, 1);
		_buttonsPanel.Children.Add(_okButton);
		_cancelButton = new Button
		{
			Name = "CancelButton",
			Margin = new Thickness(15.0, 5.0, 15.0, 5.0),
			HorizontalContentAlignment = HorizontalAlignment.Center,
			MinWidth = 100.0,
			TabIndex = 3,
			IsCancel = true,
			UseLayoutRounding = true,
			Content = "Cancel"
		};
		Grid.SetRow(_cancelButton, 0);
		Grid.SetColumn(_cancelButton, 2);
		_buttonsPanel.Children.Add(_cancelButton);
		_grid = new GridControl
		{
			ShowBorder = true,
			TabIndex = 1,
			Name = "Grid"
		};
		_view = new TableView
		{
			VerticalScrollbarVisibility = ScrollBarVisibility.Auto,
			SearchPanelParseMode = SearchPanelParseMode.Mixed,
			Name = "View",
			SearchPanelPosition = SearchPanelPosition.OverGroupPanel
		};
		_grid.View = _view;
		dockPanel.Children.Add(_grid);
		_mb = mb;
		if (owner == null)
		{
			Owner = Settings.HostingEnvironment;
		}
		else
		{
			Owner = owner;
		}
		_firstActivateCall = true;
		_selectedRowIndex = -1;
		_lookUpValue = "";
		_isMultiSelect = multiSelect;
		SelectedRowsIndexes = null;
		if (modeSearchPanel)
		{
			_view.SearchString = mb.SearchString;
			_view.ShowSearchPanel(moveFocusInSearchPanel: true);
		}
		_grid.AutoGenerateColumns = AutoGenerateColumnsMode.None;
		_grid.ClipboardCopyMode = ClipboardCopyMode.ExcludeHeader;
		Title = mb.Caption;
		_view.IsRowCellMenuEnabled = true;
		_view.Grid.SelectionMode = MultiSelectMode.None;
		_view.NavigationStyle = GridViewNavigationStyle.Cell;
		_view.AllowConditionalFormattingMenu = true;
		_view.ShowFocusedRectangle = true;
		_view.ShowAutoFilterRow = true;
		_view.ShowGroupPanel = false;
		_view.ShowGroupedColumns = true;
		_view.UseLightweightTemplates = UseLightweightTemplates.All;
		_view.BestFitMaxRowCount = 50;
		_useBestFits = true;
		_view.ShowIndicator = false;
		_view.AllowGrouping = false;
		_view.AutoFilterRowCellStyle = (Style)Settings.GenericResources["AutoFilterRowCellStyle2"];
		if (mb.IsTree)
		{
			_treeView = new MyTreeView
			{
				ExpandNodesOnFiltering = true,
				ShowCheckboxes = false,
				ShowColumnHeaders = true,
				IsRowCellMenuEnabled = true,
				ShowAutoFilterRow = true,
				AllowRecursiveNodeChecking = false,
				AllowIndeterminateCheckState = false,
				UseLightweightTemplates = UseLightweightTemplates.All,
				AllowConditionalFormattingMenu = true,
				AllowColumnFiltering = true,
				AllowColumnMoving = true,
				AllowSorting = true,
				ScrollingMode = ScrollingMode.Smart,
				VerticalScrollbarVisibility = ScrollBarVisibility.Auto,
				NavigationStyle = GridViewNavigationStyle.Cell,
				AllowMoveColumnToDropArea = false,
				IsTotalSummaryMenuEnabled = false,
				ShowTotalSummary = false,
				TreeDerivationMode = TreeDerivationMode.Selfreference,
				KeyFieldName = mb.KeyFieldName,
				ParentFieldName = mb.ParentFieldName,
				SearchPanelHorizontalAlignment = HorizontalAlignment.Left,
				AllowFormatConditionFiltersMenu = false
			};
			_grid.View = _treeView;
		}
		GridColumn gridColumn = new MyGridColumn
		{
			Name = "IsLeaf",
			Width = (Settings.IsWin11Theme ? 25 : 22),
			Header = "",
			FieldName = mb.IdvIsLeafFieldName,
			ReadOnly = true,
			UnboundType = UnboundColumnType.Boolean
		};
		_grid.Columns.Add(gridColumn);
		gridColumn.Visible = false;
		gridColumn.ShowInColumnChooser = false;
		if (_mb.DataView.ReloadMode == ReloadMode.ReloadAlways || (_mb.DataView.ReloadMode == ReloadMode.Reloadable && !_mb.DataView.DataLoadedAtLeastOnce))
		{
			_mb.DataView.ReloadRows();
		}
		if (mb.MultiSelect)
		{
			gridColumn = new GridColumn
			{
				Name = "Selected",
				Width = (Settings.IsWin11Theme ? 25 : 21),
				Header = "",
				FieldName = "Selected",
				UnboundType = UnboundColumnType.Boolean,
				AllowSorting = DefaultBoolean.False,
				AllowGrouping = DefaultBoolean.False,
				AllowResizing = DefaultBoolean.False,
				FilterPopupMode = FilterPopupMode.List,
				Fixed = FixedStyle.Left,
				ColumnChooserHeaderCaption = ArmSoft.SME.Common.Resources.RowSelection
			};
			if (Settings.IsWin11Theme)
			{
				if (mb.IsTree)
				{
					gridColumn.CellTemplate = (DataTemplate)Settings.GenericResources["GridSelectingColumnCellWin11Template"];
				}
				else
				{
					gridColumn.CellTemplate = (DataTemplate)Settings.GenericResources["GridBooleanCellWin11Template"];
				}
			}
			else if (mb.IsTree)
			{
				gridColumn.CellTemplate = (DataTemplate)Settings.GenericResources["GridSelectingColumnCellTemplate"];
			}
			_grid.Columns.Add(gridColumn);
		}
		foreach (ViewColumn view in mb.DataView.Columns.ViewList)
		{
			MyGridColumn myGridColumn = Mapper.Convert(view, mb.DataView.GetColumnType);
			Binding binding = new Binding("Column.ColSortIndex")
			{
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
				Converter = new IntegerToString(),
				Mode = BindingMode.TwoWay
			};
			FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(TextBlock));
			frameworkElementFactory.SetBinding(TextBlock.TextProperty, binding);
			frameworkElementFactory.SetValue(TextBlock.FontStyleProperty, FontStyles.Italic);
			frameworkElementFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
			frameworkElementFactory.SetValue(TextBlock.FontStretchProperty, FontStretches.Condensed);
			frameworkElementFactory.SetValue(TextBlock.ForegroundProperty, Brushes.Red);
			frameworkElementFactory.SetValue(TextBlock.FontSizeProperty, 10.0);
			DataTemplate headerCustomizationAreaTemplate = new DataTemplate
			{
				VisualTree = frameworkElementFactory
			};
			myGridColumn.HeaderCustomizationAreaTemplate = headerCustomizationAreaTemplate;
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(ColumnBase.SortIndexProperty, typeof(MyGridColumn))).AddValueChanged((object)myGridColumn, (EventHandler)SortIndexChanged);
			if (Settings.IsWin11Theme && view.Type == DataColumnType.Boolean)
			{
				myGridColumn.CellTemplate = (DataTemplate)Settings.GenericResources["GridBooleanCellWin11Template"];
			}
			_grid.Columns.Add(myGridColumn);
		}
		List<AsFormatting> conditionalFormattingList = mb.DataView.ConditionalFormattingList;
		if (conditionalFormattingList != null && conditionalFormattingList.Count > 0)
		{
			foreach (FormatConditionBase item in ConditionalFormattingHelper.ConvertConditions(mb.DataView.ConditionalFormattingList))
			{
				if (mb.IsTree)
				{
					_treeView.FormatConditions.Add(item);
				}
				else
				{
					_view.FormatConditions.Add(item);
				}
			}
		}
		mb.DataView.BeforeLoadData(_grid);
		mb.DataView.LoadData();
		mb.DataView.AfterLoadData(_grid);
		if (mb.MultiSelect)
		{
			_selectedRows = new List<bool>(mb.DataView.Data.Count);
			ClearSelection();
			_okButton.Content = ArmSoft.SME.Common.Resources.Ok;
			_cancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		}
		else
		{
			_okButton.Visibility = Visibility.Collapsed;
			_cancelButton.Visibility = Visibility.Collapsed;
		}
		_grid.ItemsSource = mb.DataView.Data;
		if (_mb.DataView.ClosedFilterSupported)
		{
			SetClosedFilter();
		}
		Activated += new SafeEventHandler<EventArgs>(ModalBrowserWindow_Activated).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(ModalBrowserWindow_Closing).Handler;
		Loaded += new SafeEventHandler<RoutedEventArgs>(ModalBrowserWindow_Loaded).Handler;
		Unloaded += new SafeEventHandler<RoutedEventArgs>(ModalBrowserWindow_Unloaded).Handler;
		_grid.CustomUnboundColumnData += new SafeEventHandler<GridColumnDataEventArgs>(Grid_CustomUnboundColumnData).Handler;
		_grid.PropertyChanged += new SafeEventHandler<PropertyChangedEventArgs>(Grid_PropertyChanged).Handler;
		_okButton.Click += new SafeEventHandler<RoutedEventArgs>(OkButton_Click).Handler;
		_cancelButton.Click += new SafeEventHandler<RoutedEventArgs>(CancelButton_Click).Handler;
		_view.PreviewMouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(View_MouseDoubleClick).Handler;
		_view.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(View_PreviewKeyDown).Handler;
		_view.KeyDown += new SafeEventHandler<KeyEventArgs>(View_KeyDown).Handler;
		_view.FocusedRowHandleChanged += new SafeEventHandler<FocusedRowHandleChangedEventArgs>(View_FocusedRowHandleChanged).Handler;
		if (_treeView != null)
		{
			_treeView.CustomUnboundColumnData += new SafeEventHandler<TreeListUnboundColumnDataEventArgs>(Tree_CustomUnboundColumnData).Handler;
			_treeView.PreviewMouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(View_MouseDoubleClick).Handler;
			_treeView.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(View_PreviewKeyDown).Handler;
			_treeView.KeyDown += new SafeEventHandler<KeyEventArgs>(View_KeyDown).Handler;
			_treeView.FocusedRowHandleChanged += new SafeEventHandler<FocusedRowHandleChangedEventArgs>(View_FocusedRowHandleChanged).Handler;
		}
	}

	private void SortIndexChanged(object sender, EventArgs args)
	{
		MyGridColumn myGridColumn = (MyGridColumn)sender;
		if (myGridColumn.SortIndex >= 0 && myGridColumn.SortOrder != ColumnSortOrder.None)
		{
			myGridColumn.ColSortIndex = myGridColumn.SortIndex + 1;
		}
		else
		{
			myGridColumn.ColSortIndex = -1;
		}
	}

	protected void Grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
	{
		if (e.Column.Name == "Selected")
		{
			if (e.IsGetData && _selectedRows.Count != 0)
			{
				e.Value = _selectedRows[e.ListSourceRowIndex];
			}
			if (e.IsSetData)
			{
				_selectedRows[e.ListSourceRowIndex] = (bool)e.Value;
			}
		}
		else if (e.IsGetData)
		{
			IDynamicValue dynamicValue = (IDynamicValue)_mb.DataView.Data[e.ListSourceRowIndex];
			e.Value = dynamicValue.GetDynamicValue(e.Column.FieldName);
		}
	}

	protected void Tree_CustomUnboundColumnData(object sender, TreeListUnboundColumnDataEventArgs e)
	{
		if (e.Node.RowHandle < 0)
		{
			return;
		}
		int listIndexByRowHandle = _grid.GetListIndexByRowHandle(e.Node.RowHandle);
		if (listIndexByRowHandle < 0)
		{
			return;
		}
		if (e.Column.Name == "Selected")
		{
			if (e.IsGetData && _selectedRows.Count != 0)
			{
				e.Value = _selectedRows[listIndexByRowHandle];
			}
			if (e.IsSetData)
			{
				bool flag = true;
				if (_mb.SelectLeavesOnly)
				{
					flag = ((!string.IsNullOrWhiteSpace(_mb.IdvIsLeafFieldName)) ? Convert.ToBoolean(_grid.GetCellValue(e.Node.RowHandle, _mb.IdvIsLeafFieldName)) : (!e.Node.HasChildren));
				}
				if (flag)
				{
					_selectedRows[listIndexByRowHandle] = Convert.ToBoolean(e.Value);
				}
				else
				{
					_selectedRows[listIndexByRowHandle] = false;
				}
			}
		}
		else if (e.Column.Name == "IsLeaf")
		{
			if (e.IsGetData)
			{
				if (_mb.IdvIsLeafFieldName == "")
				{
					e.Value = !e.Node.HasChildren;
				}
				else
				{
					e.Value = _grid.GetCellValue(e.Node.RowHandle, _mb.IdvIsLeafFieldName);
				}
			}
		}
		else if (e.IsGetData)
		{
			IDynamicValue dynamicValue = (IDynamicValue)_mb.DataView.Data[listIndexByRowHandle];
			e.Value = dynamicValue.GetDynamicValue(e.Column.FieldName);
		}
	}

	public void ClearSelection()
	{
		for (int i = 0; i <= _mb.DataView.Data.Count - 1; i++)
		{
			_selectedRows.Add(item: false);
		}
	}

	protected void ModalBrowserWindow_Activated(object sender, EventArgs e)
	{
		if (_firstActivateCall)
		{
			_firstActivateCall = false;
			if (_useBestFits)
			{
				if (_grid.View == _view)
				{
					_view.BestFitColumns();
				}
				else if (_grid.View == _treeView)
				{
					_treeView.BestFitColumns();
				}
			}
			if (MultiSelect)
			{
				foreach (int selectedRowsIndex in SelectedRowsIndexes)
				{
					if (SelectedRowsIndexes.Count == 1)
					{
						_onlyOneSelectedRowIndex = selectedRowsIndex;
					}
					_selectedRows[selectedRowsIndex] = true;
					_grid.RefreshRow(_grid.GetRowHandleByListIndex(selectedRowsIndex));
				}
			}
			int lookUpIndex = GetLookUpIndex();
			if (lookUpIndex >= 0)
			{
				int rowHandleByListIndex = _grid.GetRowHandleByListIndex(lookUpIndex);
				_grid.View.TopRowIndex = lookUpIndex;
				_grid.View.FocusedRowHandle = rowHandleByListIndex;
			}
		}
		RowChanged();
		_grid.Focus();
	}

	private void Grid_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "VisibleRowCount")
		{
			RowChanged();
		}
	}

	private void Grid_FilterChanged(object sender, RoutedEventArgs e)
	{
		if (!_changeFilterAuto && !_closedFilterIsChecked)
		{
			_closedFilterIsChecked = !_closedFilterIsChecked;
			_closedFilterHeaderItem.Content = (_closedFilterIsChecked ? _closedFilterIsCheckedImage : _closedFilterIsUncheckedImage);
		}
		_changeFilterAuto = false;
	}

	protected void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		int rowHandleByMouseEventArgs = _grid.View.GetRowHandleByMouseEventArgs(e);
		if (rowHandleByMouseEventArgs != int.MinValue && rowHandleByMouseEventArgs != -2147483645)
		{
			if (MultiSelect)
			{
				ProcessCurrentRowForMultiSelect();
			}
			else
			{
				ProcessEnter();
			}
		}
	}

	private int GetLookUpIndex()
	{
		int result = -1;
		if (!string.IsNullOrEmpty(_lookUpValue))
		{
			int length = _lookUpValue.Length;
			int num = 1;
			int num2 = 0;
			while (num2 < _mb.DataView.Data.Count)
			{
				string text = _mb.DataView.ValueMember(num2);
				if (text != null)
				{
					if (text.Length >= num && string.Compare(text.Substring(0, num), _lookUpValue.Substring(0, num), ignoreCase: true) == 0)
					{
						result = num2;
						num++;
						if (num > length)
						{
							return result;
						}
					}
					else
					{
						num2++;
					}
					continue;
				}
				return result;
			}
		}
		return result;
	}

	private void ProcessEnter()
	{
		if (MultiSelect)
		{
			if (SelectedRowsIndexes == null)
			{
				List<int> list = (SelectedRowsIndexes = new List<int>());
			}
			SelectedRowsIndexes.Clear();
			for (int i = 0; i <= _selectedRows.Count - 1; i++)
			{
				if (_selectedRows[i])
				{
					SelectedRowsIndexes.Add(i);
				}
			}
			if (SelectedRowsIndexes.Count == 0)
			{
				int focusedRowHandle = _grid.View.FocusedRowHandle;
				if (_grid.GetListIndexByRowHandle(focusedRowHandle) != _onlyOneSelectedRowIndex && CommonEventHandlers.IsDataRow(focusedRowHandle, _grid))
				{
					SelectedRowsIndexes.Add(_grid.GetListIndexByRowHandle(focusedRowHandle));
				}
			}
			DialogResult = true;
			RaiseClosed();
		}
		else
		{
			int focusedRowHandle2 = _grid.View.FocusedRowHandle;
			if (CommonEventHandlers.IsDataRow(focusedRowHandle2, _grid))
			{
				_selectedRowIndex = _grid.GetListIndexByRowHandle(focusedRowHandle2);
				DialogResult = true;
				RaiseClosed();
			}
		}
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		ProcessEnter();
	}

	private void ProcessCurrentRowForMultiSelect()
	{
		int focusedRowHandle = _grid.View.FocusedRowHandle;
		if (CommonEventHandlers.IsDataRow(focusedRowHandle, _grid))
		{
			int listIndexByRowHandle = _grid.GetListIndexByRowHandle(focusedRowHandle);
			if (listIndexByRowHandle >= 0)
			{
				_selectedRows[listIndexByRowHandle] = !_selectedRows[listIndexByRowHandle];
				_grid.RefreshRow(focusedRowHandle);
			}
		}
	}

	private void CancelButton_Click(object sender, RoutedEventArgs e)
	{
		RaiseClosed();
	}

	private void RowChanged()
	{
		int focusedRowHandle = _grid.View.FocusedRowHandle;
		if (-2147483645 == focusedRowHandle || int.MinValue == focusedRowHandle)
		{
			_txtStatus.Text = "0/" + _grid.VisibleRowCount;
			return;
		}
		int rowVisibleIndexByHandle = _grid.GetRowVisibleIndexByHandle(focusedRowHandle);
		if (rowVisibleIndexByHandle >= 0)
		{
			_txtStatus.Text = rowVisibleIndexByHandle + 1 + "/" + _grid.VisibleRowCount;
		}
		else
		{
			_txtStatus.Text = "0/" + _grid.VisibleRowCount;
		}
	}

	protected void RaiseClosed()
	{
		if (ClosedDelegate != null)
		{
			ClosedDelegate();
		}
	}

	protected void View_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected I4, but got Unknown
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Invalid comparison between Unknown and I4
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Invalid comparison between Unknown and I4
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected I4, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Invalid comparison between Unknown and I4
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Invalid comparison between Unknown and I4
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Invalid comparison between Unknown and I4
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Invalid comparison between Unknown and I4
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Invalid comparison between Unknown and I4
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Invalid comparison between Unknown and I4
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected I4, but got Unknown
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Invalid comparison between Unknown and I4
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Expected I4, but got Unknown
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Invalid comparison between Unknown and I4
		Key key = e.Key;
		if ((int)key <= 43)
		{
			if ((int)key == 3)
			{
				if (MultiSelect)
				{
					if ((int)Keyboard.Modifiers == 0)
					{
						_okButton.Focus();
					}
					else if ((int)Keyboard.Modifiers == 4)
					{
						_cancelButton.Focus();
					}
					e.Handled = true;
				}
				return;
			}
			if ((int)key == 6)
			{
				_grid.View.CommitEditing();
				ProcessEnter();
				e.Handled = true;
				return;
			}
			switch (key - 13)
			{
			default:
				return;
			case 0:
				e.Handled = true;
				DialogResult = false;
				RaiseClosed();
				return;
			case 11:
				if ((int)Keyboard.Modifiers == 2)
				{
					ColumnBase currentColumn = _grid.CurrentColumn;
					if (currentColumn != null)
					{
						_grid.ClearSorting();
						_grid.SortBy((GridColumn)currentColumn, ColumnSortOrder.Ascending);
						e.Handled = true;
					}
				}
				return;
			case 13:
				if ((int)Keyboard.Modifiers == 2)
				{
					ColumnBase currentColumn2 = _grid.CurrentColumn;
					if (currentColumn2 != null)
					{
						_grid.ClearSorting();
						_grid.SortBy((GridColumn)currentColumn2, ColumnSortOrder.Descending);
						e.Handled = true;
					}
				}
				return;
			case 9:
				_grid.View.MoveFirstRow();
				e.Handled = true;
				return;
			case 8:
				_grid.View.MoveLastRow();
				e.Handled = true;
				return;
			case 18:
				if (MultiSelect && CommonEventHandlers.IsDataRow(_grid.View.FocusedRowHandle, _grid))
				{
					ProcessCurrentRowForMultiSelect();
					_grid.View.MoveNextRow();
				}
				e.Handled = true;
				return;
			case 5:
				if (!MultiSelect || !CommonEventHandlers.IsDataRow(_grid.View.FocusedRowHandle, _grid))
				{
					return;
				}
				if (_mb.IsTree && _grid.CurrentColumn.Name == "Selected")
				{
					if (!_grid.View.IsEditing)
					{
						ProcessCurrentRowForMultiSelect();
					}
				}
				else
				{
					ProcessCurrentRowForMultiSelect();
				}
				e.Handled = true;
				return;
			case 22:
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
			case 29:
			case 30:
				break;
			case 1:
			case 2:
			case 3:
			case 4:
			case 6:
			case 7:
			case 10:
			case 12:
			case 14:
			case 15:
			case 16:
			case 17:
			case 19:
			case 20:
			case 21:
				return;
			}
		}
		else
		{
			switch (key - 75)
			{
			default:
				if ((int)key == 141)
				{
					goto case 10;
				}
				if ((int)key != 143)
				{
					return;
				}
				goto case 12;
			case 9:
			{
				if (!MultiSelect || !CommonEventHandlers.IsDataRow(_grid.View.FocusedRowHandle, _grid))
				{
					return;
				}
				if (_grid.View.IsEditing)
				{
					_grid.View.CloseEditor();
				}
				for (int i = 0; i <= _grid.VisibleRowCount - 1; i++)
				{
					int rowHandleByVisibleIndex = _grid.GetRowHandleByVisibleIndex(i);
					if (CommonEventHandlers.IsDataRow(rowHandleByVisibleIndex, _grid))
					{
						int listIndexByRowHandle = _grid.GetListIndexByRowHandle(rowHandleByVisibleIndex);
						_selectedRows[listIndexByRowHandle] = !_selectedRows[listIndexByRowHandle];
						_grid.RefreshRow(rowHandleByVisibleIndex);
					}
				}
				e.Handled = true;
				return;
			}
			case 10:
			{
				if (!MultiSelect || !CommonEventHandlers.IsDataRow(_grid.View.FocusedRowHandle, _grid))
				{
					return;
				}
				for (int k = 0; k <= _grid.VisibleRowCount - 1; k++)
				{
					int rowHandleByVisibleIndex3 = _grid.GetRowHandleByVisibleIndex(k);
					if (CommonEventHandlers.IsDataRow(rowHandleByVisibleIndex3, _grid))
					{
						int listIndexByRowHandle3 = _grid.GetListIndexByRowHandle(rowHandleByVisibleIndex3);
						_selectedRows[listIndexByRowHandle3] = true;
						_grid.RefreshRow(rowHandleByVisibleIndex3);
					}
				}
				e.Handled = true;
				return;
			}
			case 12:
			{
				if (!MultiSelect || !CommonEventHandlers.IsDataRow(_grid.View.FocusedRowHandle, _grid))
				{
					return;
				}
				for (int j = 0; j <= _grid.VisibleRowCount - 1; j++)
				{
					int rowHandleByVisibleIndex2 = _grid.GetRowHandleByVisibleIndex(j);
					if (CommonEventHandlers.IsDataRow(rowHandleByVisibleIndex2, _grid))
					{
						int listIndexByRowHandle2 = _grid.GetListIndexByRowHandle(rowHandleByVisibleIndex2);
						_selectedRows[listIndexByRowHandle2] = false;
						_grid.RefreshRow(rowHandleByVisibleIndex2);
					}
				}
				e.Handled = true;
				return;
			}
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				break;
			case 11:
				return;
			}
		}
		if (_grid.View == null || _grid.Columns.Count <= 0 || ((int)Keyboard.Modifiers != 2 && (int)Keyboard.Modifiers != 6))
		{
			return;
		}
		int num = (((int)e.Key < 75 || (int)e.Key > 83) ? (e.Key - 35) : (e.Key - 75));
		for (int l = 0; l <= Math.Min(_grid.Columns.Count, 1); l++)
		{
			GridColumn gridColumn = _grid.Columns[l];
			if (gridColumn != null && gridColumn.Name.InList("Selected", "IsLeaf"))
			{
				num++;
			}
		}
		if (_grid.View == null || _grid.Columns.Count <= num)
		{
			return;
		}
		GridColumn gridColumn2 = _grid.Columns[num];
		if (gridColumn2 != null && gridColumn2.Visible)
		{
			ColumnSortOrder sortOrder = gridColumn2.SortOrder;
			if ((int)Keyboard.Modifiers == 2)
			{
				_grid.ClearSorting();
			}
			gridColumn2.SortOrder = sortOrder;
			if (gridColumn2.IsSorted && gridColumn2.SortOrder == ColumnSortOrder.Ascending)
			{
				gridColumn2.SortOrder = ColumnSortOrder.Descending;
			}
			else if (gridColumn2.IsSorted && gridColumn2.SortOrder == ColumnSortOrder.Descending)
			{
				gridColumn2.SortOrder = ColumnSortOrder.None;
			}
			else if (!gridColumn2.IsSorted)
			{
				gridColumn2.SortOrder = ColumnSortOrder.Ascending;
			}
			_grid.RefreshData();
		}
	}

	protected void View_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		if ((int)e.Key != 116 && (int)e.Key != 117 && (int)e.Key != 118 && (int)e.Key != 119 && (int)e.Key != 120 && (int)e.Key != 121 && _grid.View.FocusedRowHandle != -2147483645)
		{
			if (_grid.CurrentColumn != null)
			{
				_grid.CurrentColumn.AutoFilterValue = "";
			}
			_grid.View.FocusedRowHandle = -2147483645;
		}
	}

	protected void View_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		RowChanged();
	}

	protected void ModalBrowserWindow_Closing(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(_mb.DataView.Name) || WindowState != WindowState.Normal)
		{
			return;
		}
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey($"{Settings.RegistryKey}\\{"MB"}\\{_mb.DataView.Name}");
		if (registryKey == null)
		{
			return;
		}
		registryKey.SetValue("Width", Width);
		registryKey.SetValue("Height", Height);
		using RegistryKey registryKey2 = registryKey.CreateSubKey("Columns");
		foreach (GridColumn column in _grid.Columns)
		{
			if (column.Name != "Selected")
			{
				string text = column.ActualWidth.ToString();
				if (text != "NaN")
				{
					registryKey2.SetValue(column.Name.Trim(), text);
				}
			}
		}
	}

	private void ModalBrowserWindow_Loaded(object sender, RoutedEventArgs e)
	{
		LoadRegistrySettings();
	}

	private void ModalBrowserWindow_Unloaded(object sender, RoutedEventArgs e)
	{
		try
		{
			foreach (GridColumn column in _grid.Columns)
			{
				if (!(column.Name == "Selected") && !(column.Name == "IsLeaf"))
				{
					if (column is MyGridColumn component)
					{
						((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(ColumnBase.SortIndexProperty, typeof(MyGridColumn))).RemoveValueChanged((object)component, (EventHandler)SortIndexChanged);
						((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(ColumnBase.SortIndexProperty, typeof(MyGridColumn))).RemoveValueChanged((object)component, (EventHandler)SortIndexChanged);
					}
					else
					{
						Log.Warning(" ModalBrowserWindow_Unloaded " + _mb.DataView.Name, "IsNothing(Col) " + column.Name);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Warning(" ModalBrowserWindow_Unloaded " + _mb.DataView.Name, "Failed: " + ex.Message);
		}
	}

	public void LoadRegistrySettings()
	{
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey($"{Settings.RegistryKey}\\{"MB"}\\{_mb.DataView.Name}", writable: false);
		if (registryKey == null)
		{
			return;
		}
		if (double.TryParse(registryKey.GetValue("Width", "0").ToString(), out var result) && result > 0.0)
		{
			Width = result;
		}
		if (double.TryParse(registryKey.GetValue("Height", "0").ToString(), out var result2) && result2 > 0.0)
		{
			Height = result2;
		}
		using RegistryKey registryKey2 = registryKey.OpenSubKey("Columns", writable: false);
		if (registryKey2 == null)
		{
			return;
		}
		foreach (GridColumn column in _grid.Columns)
		{
			if (column.Name != "Selected" && double.TryParse(registryKey2.GetValue(column.Name.Trim(), "0").ToString(), out result) && result > 0.0)
			{
				column.Width = result;
				_useBestFits = false;
			}
		}
	}

	private void ReloadHeaderItem_Click(object sender, RoutedEventArgs e)
	{
		Reload();
	}

	private void ModalBrowserWindow_KeyDown(object sender, KeyEventArgs e)
	{
		if (HotKeys.WithModifier.Control_R.Matches(this, e))
		{
			Reload();
		}
	}

	private void Reload()
	{
		_mb.DataView.ReloadRows(LoadSource.ReloadButton);
		_grid.RefreshData();
		if (MultiSelect)
		{
			ClearSelection();
		}
	}

	private void ClosedFilterHeaderItem_Click(object sender, RoutedEventArgs e)
	{
		_closedFilterIsChecked = !_closedFilterIsChecked;
		SetClosedFilter();
		_closedFilterHeaderItem.Content = (_closedFilterIsChecked ? _closedFilterIsCheckedImage : _closedFilterIsUncheckedImage);
	}

	private void SetClosedFilter()
	{
		_changeFilterAuto = true;
		if (_treeView != null)
		{
			_treeView.ExpandNodesOnFiltering = false;
		}
		if (_mb.DataView.ClosedFilterSupported)
		{
			if (_closedFilterIsChecked)
			{
				_mb.DataView.FilterString = "";
			}
			else
			{
				_mb.DataView.FilterString = _mb.DataView.ClosedColumnName + "=false";
			}
			_grid.FilterString = _mb.DataView.FilterString;
			if (_treeView != null)
			{
				_treeView.ExpandNodesOnFiltering = true;
			}
		}
	}
}
