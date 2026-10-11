using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.Models.Layout;
using ArmSoft.SME.Core.UI.Controls;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.DragDrop;

namespace ArmSoft.SME.Core.UI.DataViews;

public class DlgGroupByColumns : DialogWindow
{
	private BindingList<GroupingColumn> _allColumns;

	private BindingList<GroupingColumn> _selectedColumns;

	private readonly ListBoxDragDropManager _lstMng = new ListBoxDragDropManager();

	private readonly GridDragDropManager _grdMng = new GridDragDropManager();

	private GridControl _innerGrid;

	private ListBoxEdit _innerList;

	private DataViewPanel _myDvp;

	private List<GridLayoutItem> _arrLayouts;

	private GridControl DataViewGrid { get; set; }

	public bool ShowQTY => Convert.ToBoolean(base["ShowQTY"]);

	public BindingList<GroupingColumn> GetSelectedColumns
	{
		get
		{
			int num = 0;
			if (_selectedColumns == null || _selectedColumns.Count == 0)
			{
				return null;
			}
			while (num < _selectedColumns.Count)
			{
				if (!_selectedColumns[num].UsedInGrouping)
				{
					_selectedColumns.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
			return _selectedColumns;
		}
	}

	private void ConstructUI(DataViewPanel dvp, SerializableGroupingInfo selectedOptions = null, List<GridLayoutItem> arrSavedViews = null)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		DataViewGrid = dvp.Grid;
		_myDvp = dvp;
		_allColumns = new BindingList<GroupingColumn>();
		_selectedColumns = new BindingList<GroupingColumn>();
		_arrLayouts = arrSavedViews;
		if (selectedOptions != null && selectedOptions.ColumnsInfo != null)
		{
			_selectedColumns = selectedOptions.ColumnsInfo;
			flag = selectedOptions.ShowQty;
		}
		LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		AddTextBlock(layoutGroupExt, ArmSoft.SME.Common.Resources.GrpDlgText, TextBlockImage.None);
		List<BarButtonItem> list = new List<BarButtonItem>();
		BarButtonItem barButtonItem = new BarButtonItem
		{
			Name = "bLayoutLoad",
			Content = ArmSoft.SME.Common.Resources.LayoutLoad,
			LargeGlyph = ImageService.GetImage("layout_icon32.svg"),
			Glyph = ImageService.GetImage("layout_icon16.svg")
		};
		barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(LoadLayoutClick).Handler;
		list.Add(barButtonItem);
		barButtonItem = new BarButtonItem
		{
			Name = "bLayoutClear",
			Content = ArmSoft.SME.Common.Resources.ClearWord,
			LargeGlyph = ImageService.GetImage("delete_16.svg"),
			Glyph = ImageService.GetImage("delete_16.svg")
		};
		barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ClearSelectedColumns).Handler;
		list.Add(barButtonItem);
		barButtonItem = new BarButtonItem
		{
			Name = "bLayoutSave",
			Content = ArmSoft.SME.Common.Resources.Save,
			LargeGlyph = ImageService.GetImage("layout_add32.svg"),
			Glyph = ImageService.GetImage("layout_add16.svg")
		};
		barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(SaveLayoutClick).Handler;
		list.Add(barButtonItem);
		barButtonItem = new BarButtonItem
		{
			Name = "bLayoutEdit",
			Content = ArmSoft.SME.Common.Resources.Edit,
			LargeGlyph = ImageService.GetImage("layout_edit32.svg"),
			Glyph = ImageService.GetImage("layout_edit16.svg")
		};
		barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(EditLayoutClick).Handler;
		list.Add(barButtonItem);
		AddDropDownButton(layoutGroupExt, "mnuGroupByType", "", ArmSoft.SME.Common.Resources.GroupingView, "", "", isTabStop: true, HorizontalAlignment.Right, 16.0, 16.0, list);
		LayoutGroupExt layoutGroupExt2 = AddHorizontalLayoutGroup();
		PreviewMouseLeftButtonDown += new SafeEventHandler<MouseButtonEventArgs>(ChangeViewEditorMode).Handler;
		Title = ArmSoft.SME.Common.Resources.GroupByColumns;
		ListBoxEdit listBoxEdit = new ListBoxEdit
		{
			Name = "AllColumns",
			DisplayMember = "Caption",
			Width = 150.0,
			AllowDrop = true
		};
		layoutGroupExt2.Children.Add(listBoxEdit);
		_lstMng.Attach((DependencyObject)(object)listBoxEdit);
		_innerList = listBoxEdit;
		_lstMng.Drop += new SafeEventHandler<ListBoxDropEventArgs>(AddColumnToLst).Handler;
		_innerList.MouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(MoveListItem).Handler;
		_lstMng.DragElementTemplate = DraggedRowTemplate();
		_lstMng.AllowDrag = true;
		_lstMng.AllowDrop = true;
		DocumentGridControl documentGridControl = AddGridControl(layoutGroupExt2, "Columns");
		_innerGrid = documentGridControl.InnerGrid;
		_innerGrid.Name = "Columns";
		_grdMng.Attach((DependencyObject)(object)_innerGrid);
		_grdMng.DragElementTemplate = DraggedRowTemplate();
		_grdMng.Drop += new SafeEventHandler<GridDropEventArgs>(AddColumnToGrd).Handler;
		_innerGrid.MouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(MoveGridItem).Handler;
		documentGridControl.InnerView.EditorShowMode = EditorShowMode.MouseDownFocused;
		_grdMng.AllowDrag = true;
		_grdMng.AllowDrop = true;
		documentGridControl.ShowButtons = false;
		DocumentGridColumn documentGridColumn = documentGridControl.AddStringColumn("Caption", ArmSoft.SME.Common.Resources.Column, 50);
		documentGridColumn.ReadOnly = true;
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Width = 100.0;
		DocumentGridColumn documentGridColumn2 = documentGridControl.AddBooleanColumn("GroupBy", ArmSoft.SME.Common.Resources.GrpColGroupBy);
		documentGridColumn2.Width = 50.0;
		documentGridColumn2.ReadOnly = false;
		documentGridColumn2.AllowEditing = DefaultBoolean.True;
		documentGridColumn2.FieldName = "GroupBy";
		DocumentGridColumn documentGridColumn3 = documentGridControl.AddBooleanColumn("CalcSum", ArmSoft.SME.Common.Resources.GrpColGroupSum);
		documentGridColumn3.Width = 50.0;
		documentGridColumn3.ReadOnly = false;
		documentGridColumn3.AllowEditing = DefaultBoolean.True;
		documentGridColumn3.FieldName = "CalcSum";
		documentGridColumn3.CellTemplate = AggrColTemplate(onlyGroup: false, "CalcSum");
		DocumentGridColumn documentGridColumn4 = documentGridControl.AddBooleanColumn("CalcAvg", ArmSoft.SME.Common.Resources.GrpColGroupAvg);
		documentGridColumn4.Width = 50.0;
		documentGridColumn4.ReadOnly = false;
		documentGridColumn4.AllowEditing = DefaultBoolean.True;
		documentGridColumn4.FieldName = "CalcAvg";
		documentGridColumn4.CellTemplate = AggrColTemplate(onlyGroup: false, "CalcAvg");
		DocumentGridColumn documentGridColumn5 = documentGridControl.AddBooleanColumn("CalcMax", ArmSoft.SME.Common.Resources.GrpColGroupMax);
		documentGridColumn5.Width = 50.0;
		documentGridColumn5.ReadOnly = false;
		documentGridColumn5.AllowEditing = DefaultBoolean.True;
		documentGridColumn5.FieldName = "CalcMax";
		documentGridColumn5.CellTemplate = AggrColTemplate(onlyGroup: true, "CalcMax");
		DocumentGridColumn documentGridColumn6 = documentGridControl.AddBooleanColumn("CalcMin", ArmSoft.SME.Common.Resources.GrpColGroupMin);
		documentGridColumn6.ReadOnly = false;
		documentGridColumn6.Width = 50.0;
		documentGridColumn6.AllowEditing = DefaultBoolean.True;
		documentGridColumn6.FieldName = "CalcMin";
		documentGridColumn6.CellTemplate = AggrColTemplate(onlyGroup: true, "CalcMin");
		AddBooleanControl("ShowQTY", ArmSoft.SME.Common.Resources.GrpShowQty).EditValue = flag;
		MinHeight = 400.0;
		if (Settings.HostingEnvironment.ActualHeight - 100.0 > 0.0)
		{
			MaxHeight = Settings.HostingEnvironment.ActualHeight - 100.0;
		}
		else
		{
			MaxHeight = 400.0;
		}
		BindingList<GroupingColumn> bindingList = new BindingList<GroupingColumn>();
		foreach (GridColumn column in DataViewGrid.Columns)
		{
			if (column.Visible && column.Name != "Selected" && column.Name != "_GroupQuantity" && column.AllowMoving != DefaultBoolean.False)
			{
				GroupingColumn groupingColumn = new GroupingColumn
				{
					Name = column.Name,
					Caption = column.Header.ToString(),
					EnableAggregates = true,
					AllowSum = false
				};
				if (column.FieldType == typeof(DateTime) || column.FieldType == typeof(DateTime?))
				{
					groupingColumn.TypeGroup = DataColumnType.DateTime;
				}
				else if (column.FieldType == typeof(DateOnly) || column.FieldType == typeof(DateOnly?))
				{
					groupingColumn.TypeGroup = DataColumnType.DateOnly;
				}
				else if (column.FieldType == typeof(string))
				{
					groupingColumn.TypeGroup = DataColumnType.String;
				}
				else if (column.FieldType == typeof(bool))
				{
					groupingColumn.TypeGroup = DataColumnType.Boolean;
					groupingColumn.AllowSum = false;
					groupingColumn.EnableAggregates = false;
				}
				else if (column.FieldType == typeof(int) || column.FieldType == typeof(int?))
				{
					groupingColumn.TypeGroup = DataColumnType.Integer;
					groupingColumn.AllowSum = true;
				}
				else if (column.FieldType == typeof(decimal) || column.FieldType == typeof(decimal?))
				{
					groupingColumn.TypeGroup = DataColumnType.Decimal;
					groupingColumn.AllowSum = true;
				}
				else
				{
					groupingColumn.TypeGroup = DataColumnType.Unknown;
					groupingColumn.AllowSum = false;
					groupingColumn.EnableAggregates = false;
				}
				bindingList.Add(groupingColumn);
			}
		}
		if (_selectedColumns != null)
		{
			List<GroupingColumn> list2 = new List<GroupingColumn>();
			foreach (GroupingColumn selectedColumn in _selectedColumns)
			{
				if (!RowBelongsToList(selectedColumn, bindingList))
				{
					list2.Add(selectedColumn);
				}
			}
			if (list2.Count > 0)
			{
				foreach (GroupingColumn item in list2)
				{
					_selectedColumns.Remove(item);
				}
				list2 = null;
			}
			list2 = new List<GroupingColumn>();
			foreach (GroupingColumn item2 in bindingList)
			{
				if (RowBelongsToList(item2, _selectedColumns))
				{
					list2.Add(item2);
				}
			}
			if (list2.Count > 0)
			{
				foreach (GroupingColumn item3 in list2)
				{
					bindingList.Remove(item3);
				}
				list2 = null;
			}
		}
		_allColumns = bindingList;
		listBoxEdit.ItemsSource = _allColumns;
		documentGridControl.ItemsSource = _selectedColumns;
	}

	private void LoadLayoutClick(object sender, ItemClickEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Title = ArmSoft.SME.Common.Resources.GroupByColumns
		};
		DocumentGridControl documentGridControl = dialogWindow.AddGridControl("Layouts");
		FillGrid(documentGridControl);
		documentGridControl.ShowButtons = false;
		documentGridControl.ButtonDuplicate.IsVisible = false;
		documentGridControl.ButtonDelete.IsVisible = false;
		documentGridControl.Grid.View.NavigationStyle = GridViewNavigationStyle.Row;
		documentGridControl.AllowDelete = false;
		documentGridControl.AllowAdd = false;
		documentGridControl.AllowDuplicate = false;
		documentGridControl.AllowEdit = false;
		dialogWindow.Activated += new SafeEventHandler<EventArgs>(FocusGrid).Handler;
		if (dialogWindow.ShowDialog().Value)
		{
			LoadGroupByLayout(documentGridControl);
		}
	}

	private void LoadGroupByLayout(object sender)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected Obj, but got Unknown
		object currentItem = ((DocumentGridControl)sender).Grid.CurrentItem;
		if (currentItem == null)
		{
			return;
		}
		GridLayoutItem gridLayoutItem = (GridLayoutItem)currentItem;
		int num = FindLayoutByKey(gridLayoutItem.Tag.ToString());
		if (num <= -1)
		{
			return;
		}
		string layout = _arrLayouts[num].Layout;
		if (Window.GetWindow((DependencyObject)sender) is DialogWindow dialogWindow)
		{
			dialogWindow.Close();
		}
		SerializableGroupingInfo serializableGroupingInfo = JsonSerializer.Deserialize<SerializableGroupingInfo>(layout);
		ClearSelectedColumns(this, null);
		foreach (GroupingColumn item in serializableGroupingInfo.ColumnsInfo)
		{
			MoveListItemToGrid(item, bCreateNew: false);
		}
		base["ShowQTY"] = serializableGroupingInfo.ShowQty;
	}

	private int FindLayoutByKey(string sKey)
	{
		for (int i = 0; i <= _arrLayouts.Count - 1; i++)
		{
			if (_arrLayouts[i].Caption == sKey)
			{
				return i;
			}
		}
		return -1;
	}

	private void FillGrid(DocumentGridControl grd)
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
		grd.ButtonAdd.IsVisible = false;
		grd.ButtonDuplicate.IsVisible = false;
		grd.AllowDelete = true;
		grd.AllowAdd = false;
		grd.AllowDrop = false;
		grd.AllowDuplicate = false;
		grd.AllowEdit = true;
		grd.ItemsSource = list;
		DocumentGridColumn documentGridColumn = grd.AddStringColumn("Caption", ArmSoft.SME.Common.Resources.Name, 50);
		documentGridColumn.AllowMoving = DefaultBoolean.False;
		documentGridColumn.FieldName = "Caption";
		documentGridColumn.AllowEditing = DefaultBoolean.True;
		documentGridColumn.ShowInColumnChooser = false;
		DocumentGridColumn documentGridColumn2 = grd.AddStringColumn("Tag", ArmSoft.SME.Common.Resources.Name, 30);
		documentGridColumn2.AllowMoving = DefaultBoolean.False;
		documentGridColumn2.FieldName = "Tag";
		documentGridColumn2.AllowEditing = DefaultBoolean.False;
		documentGridColumn2.Visible = false;
	}

	private void EditLayoutClick(object sender, ItemClickEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Title = ArmSoft.SME.Common.Resources.GroupByColumns
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
		dialogWindow.WindowValidation += LayoutWindowEditValidation;
		if (!dialogWindow.ShowDialog().Value)
		{
			return;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int i = 0; i <= documentGridControl.Grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = documentGridControl.Grid.GetRowHandleByVisibleIndex(i);
			string value = Convert.ToString(documentGridControl.Grid.GetCellValue(rowHandleByVisibleIndex, documentGridControl.Grid.Columns["Caption"])).Trim();
			string key = Convert.ToString(documentGridControl.Grid.GetCellValue(rowHandleByVisibleIndex, documentGridControl.Grid.Columns["Tag"])).Trim();
			dictionary.Add(key, value);
		}
		try
		{
			foreach (GridLayoutItem arrLayout in _arrLayouts)
			{
				if (!dictionary.ContainsKey(arrLayout.Tag))
				{
					Settings.LayoutService.IsDeleteLayoutRow(_myDvp.IdvName(), arrLayout.Tag);
				}
			}
			foreach (GridLayoutItem arrLayout2 in _arrLayouts)
			{
				if (dictionary.TryGetValue(arrLayout2.Tag, out var value2) && value2 != arrLayout2.Caption && Settings.LayoutService.ExistsLayoutRow(_myDvp.IdvName(), arrLayout2.Tag) && Settings.LayoutService.IsDeleteLayoutRow(_myDvp.IdvName(), arrLayout2.Tag))
				{
					string caption = dictionary[arrLayout2.Caption];
					StoreLayoutValues(caption, arrLayout2.Layout, arrLayout2.Type);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		_myDvp.LoadLayouts();
	}

	private bool StoreLayoutValues(string caption, string body, DataViewTypes dvType)
	{
		try
		{
			Settings.LayoutService.Store(new LayoutRow
			{
				Body = body,
				Caption = caption,
				LayoutType = (byte)dvType,
				Name = _myDvp.IdvName()
			});
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			return false;
		}
		return true;
	}

	private void LayoutWindowEditValidation(DialogWindow dialog, DialogWindowValidationArgs args)
	{
		DocumentGridControl documentGridControl = (DocumentGridControl)dialog["Layouts"];
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int i = 0; i <= documentGridControl.Grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = documentGridControl.Grid.GetRowHandleByVisibleIndex(i);
			string text = Convert.ToString(documentGridControl.Grid.GetCellValue(rowHandleByVisibleIndex, documentGridControl.Grid.Columns["Caption"])).Trim();
			string key = Convert.ToString(documentGridControl.Grid.GetCellValue(rowHandleByVisibleIndex, documentGridControl.Grid.Columns["Tag"])).Trim();
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

	private void FocusGrid(object sender, EventArgs e)
	{
		DocumentGridControl documentGridControl = (DocumentGridControl)((DialogWindow)sender)["Layouts"];
		if (documentGridControl.Grid.VisibleRowCount > 0)
		{
			int focusedRowHandle = Convert.ToInt32(documentGridControl.Grid.GetRowHandleByListIndex(0));
			documentGridControl.View.FocusedRowHandle = focusedRowHandle;
			Keyboard.Focus(documentGridControl.Grid.View);
		}
	}

	private void ClearSelectedColumns(object sender, ItemClickEventArgs e)
	{
		while (_selectedColumns.Count > 0)
		{
			MoveGridRowToList(_selectedColumns[0]);
		}
	}

	private void SaveLayoutClick(object sender, ItemClickEventArgs e)
	{
		if (GetSelectedColumns == null || GetSelectedColumns.Count == 0)
		{
			return;
		}
		DialogWindow dialogWindow = new DialogWindow
		{
			Title = ArmSoft.SME.Common.Resources.GroupByColumns
		};
		dialogWindow.AddButtonEditControl("LayoutCaption", ArmSoft.SME.Common.Resources.Name, 50, isRequired: true, 1);
		dialogWindow["LayoutCaption"] = "";
		if (!dialogWindow.ShowDialog().Value)
		{
			return;
		}
		string text = Convert.ToString(dialogWindow["LayoutCaption"]).Trim();
		string text2 = JsonSerializer.Serialize(new SerializableGroupingInfo
		{
			ShowQty = ShowQTY,
			ColumnsInfo = GetSelectedColumns
		});
		if (StoreLayoutValues(text, text2, DataViewTypes.GroupByReport))
		{
			int num = FindLayoutByKey(text);
			if (num > -1)
			{
				_arrLayouts[num].Layout = text2;
				return;
			}
			_arrLayouts.Add(new GridLayoutItem
			{
				Caption = text,
				Tag = text,
				Type = DataViewTypes.GroupByReport,
				Layout = text2
			});
		}
	}

	private static DataTemplate DraggedRowTemplate()
	{
		DataTemplate dataTemplate = new DataTemplate();
		Binding binding = new Binding("Caption");
		FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(TextBlock));
		frameworkElementFactory.SetBinding(TextBlock.TextProperty, binding);
		dataTemplate.VisualTree = frameworkElementFactory;
		DataTemplate dataTemplate2 = new DataTemplate
		{
			DataType = typeof(GroupingColumn)
		};
		binding = new Binding("FirstDraggingObject")
		{
			Mode = BindingMode.OneWay,
			UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
		};
		FrameworkElementFactory frameworkElementFactory2 = new FrameworkElementFactory(typeof(ContentPresenter));
		frameworkElementFactory2.SetBinding(ContentPresenter.ContentProperty, binding);
		frameworkElementFactory2.SetValue(ContentPresenter.ContentTemplateProperty, dataTemplate);
		dataTemplate2.VisualTree = frameworkElementFactory2;
		return dataTemplate2;
	}

	private static DataTemplate AggrColTemplate(bool onlyGroup, string fieldName)
	{
		DataTemplate dataTemplate = new DataTemplate
		{
			DataType = typeof(GroupingColumn)
		};
		Binding binding = ((!onlyGroup) ? new Binding("Data.AllowSum") : new Binding("Data.EnableAggregates"));
		binding.Mode = BindingMode.OneWay;
		binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
		FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(CheckEdit));
		frameworkElementFactory.SetBinding(UIElement.IsEnabledProperty, binding);
		frameworkElementFactory.SetValue(CheckEdit.IsThreeStateProperty, false);
		frameworkElementFactory.SetBinding(binding: new Binding("Data." + fieldName)
		{
			Mode = BindingMode.TwoWay
		}, dp: BaseEdit.EditValueProperty);
		frameworkElementFactory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
		frameworkElementFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
		frameworkElementFactory.SetValue(FrameworkElement.NameProperty, "PART_Editor");
		dataTemplate.VisualTree = frameworkElementFactory;
		return dataTemplate;
	}

	private void AddColumnToGrd(object sender, GridDropEventArgs e)
	{
		foreach (GroupingColumn draggedRow in e.DraggedRows)
		{
			if (!RowBelongsToList(draggedRow, _selectedColumns))
			{
				MoveListItemToGrid(draggedRow);
				e.Handled = true;
			}
			else
			{
				e.Handled = false;
			}
		}
	}

	private void AddColumnToLst(object sender, ListBoxDropEventArgs e)
	{
		foreach (GroupingColumn draggedRow in e.DraggedRows)
		{
			if (!RowBelongsToList(draggedRow, _allColumns))
			{
				MoveGridRowToList(draggedRow);
				e.Handled = true;
			}
			else
			{
				e.Handled = false;
			}
		}
	}

	internal DlgGroupByColumns(DataViewPanel dvp, SerializableGroupingInfo selectedOptions, List<GridLayoutItem> arrSavedLayouts)
	{
		PreviewMouseLeftButtonDown += new SafeEventHandler<MouseButtonEventArgs>(ChangeViewEditorMode).Handler;
		Title = ArmSoft.SME.Common.Resources.GroupByColumns;
		ConstructUI(dvp, selectedOptions, arrSavedLayouts);
	}

	private void ChangeViewEditorMode(object sender, MouseButtonEventArgs e)
	{
		GridControl gridControl = (GridControl)(object)_grdMng.AssociatedObject;
		if (gridControl.CurrentColumn != null)
		{
			if (gridControl.CurrentColumn.Name == "Caption")
			{
				((TableView)gridControl.View).EditorShowMode = EditorShowMode.MouseDownFocused;
			}
			else
			{
				((TableView)gridControl.View).EditorShowMode = EditorShowMode.MouseDown;
			}
		}
	}

	private static bool RowBelongsToList(GroupingColumn row, BindingList<GroupingColumn> columns)
	{
		if (row != null && columns != null)
		{
			foreach (GroupingColumn column in columns)
			{
				if (column.Name == row.Name)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void MoveListItem(object sender, MouseButtonEventArgs e)
	{
		if (_innerList.SelectedItem != null)
		{
			GroupingColumn row = (GroupingColumn)_innerList.SelectedItem;
			if (!RowBelongsToList(row, _selectedColumns))
			{
				MoveListItemToGrid(row);
				e.Handled = true;
			}
			else
			{
				e.Handled = false;
			}
		}
	}

	private void MoveListItemToGrid(GroupingColumn row, bool bCreateNew = true)
	{
		GroupingColumn groupingColumn = new GroupingColumn
		{
			Caption = row.Caption,
			Name = row.Name,
			TypeGroup = row.TypeGroup
		};
		groupingColumn.AllowSum = groupingColumn.TypeGroup == DataColumnType.Decimal || groupingColumn.TypeGroup == DataColumnType.Integer;
		groupingColumn.EnableAggregates = row.EnableAggregates;
		if (bCreateNew)
		{
			if (groupingColumn.AllowSum)
			{
				groupingColumn.CalcSum = true;
				groupingColumn.GroupBy = false;
			}
			else
			{
				groupingColumn.GroupBy = true;
				groupingColumn.CalcSum = false;
			}
		}
		else
		{
			groupingColumn.CalcSum = row.CalcSum;
			groupingColumn.GroupBy = row.GroupBy;
		}
		groupingColumn.CalcAvg = row.CalcAvg;
		groupingColumn.CalcMin = row.CalcMin;
		groupingColumn.CalcMax = row.CalcMax;
		_selectedColumns.Add(groupingColumn);
		if (!bCreateNew)
		{
			if (_allColumns.Count > 0)
			{
				for (int i = 0; i < _allColumns.Count; i++)
				{
					if (_allColumns[i].Name == row.Name)
					{
						_allColumns.RemoveAt(i);
						break;
					}
				}
			}
		}
		else
		{
			_allColumns.Remove(row);
		}
		_innerGrid.RefreshData();
		_innerList.UpdateLayout();
	}

	private void MoveGridRowToList(GroupingColumn row)
	{
		GroupingColumn item = new GroupingColumn
		{
			Caption = row.Caption,
			Name = row.Name,
			TypeGroup = row.TypeGroup,
			GroupBy = row.GroupBy,
			CalcSum = row.CalcSum,
			CalcAvg = row.CalcAvg,
			CalcMin = row.CalcMin,
			CalcMax = row.CalcMax,
			EnableAggregates = row.EnableAggregates
		};
		_allColumns.Add(item);
		_selectedColumns.Remove(row);
	}

	private void MoveGridItem(object sender, MouseButtonEventArgs e)
	{
		if (_innerGrid.CurrentItem != null)
		{
			GroupingColumn row = (GroupingColumn)_innerGrid.CurrentItem;
			if (!RowBelongsToList(row, _allColumns))
			{
				MoveGridRowToList(row);
				e.Handled = true;
			}
			else
			{
				e.Handled = false;
			}
		}
	}
}
