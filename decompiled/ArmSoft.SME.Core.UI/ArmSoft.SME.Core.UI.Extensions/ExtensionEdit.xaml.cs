using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.DropDowns.MetaData;
using ArmSoft.SME.Core.UI.DropDownsData;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.MetaData.DocumentExtension;
using ArmSoft.SME.Core.UI.MetaData.UI_Definition;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.SmartEnums;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Extensions;

public partial class ExtensionEdit : UserControl, IComponentConnector
{
	private const short _extensionFieldGridColumnLength = 10;

	private readonly StringResources _stringResourcesView;

	private Dictionary<string, string> _systemPages;

	private readonly Window _owner;

	private readonly bool _allowAddTabs;

	private readonly ObservableCollection<ExtensionField> _extensionFields = new ObservableCollection<ExtensionField>();

	private readonly ObservableCollection<TabDescription> _extensionTabs = new ObservableCollection<TabDescription>();

	private readonly ObservableCollection<UIGroupDescription> _extensionGroups = new ObservableCollection<UIGroupDescription>();

	private readonly SingleElements _pagesSingleElements;

	private bool _isReadOnly;

	internal bool IsReadOnly
	{
		get
		{
			return _isReadOnly;
		}
		set
		{
			SetReadOnlyState(value);
			_isReadOnly = value;
		}
	}

	public ExtensionEdit(bool allowAddPages, Window owner = null)
	{
		_owner = owner;
		_allowAddTabs = allowAddPages;
		InitializeComponent();
		_stringResourcesView = new StringResources();
		InitializeGrid(_extensionGroupsGrid, _extensionGroups);
		InitializeGrid(_extensionFieldsGrid, _extensionFields);
		if (allowAddPages)
		{
			_pagesSingleElements = new SingleElements(ArmSoft.SME.Common.Resources.Pages, _extensionTabs.Select((TabDescription el) => el.CaptionId));
			InitializeGrid(_extensionTabsGrid, _extensionTabs);
			PopulateTabGridColumns();
		}
		else
		{
			_extensionTabsLayout.Visibility = Visibility.Collapsed;
		}
		PopulateGroupGridColumns();
		PopulateFieldsGridColumns();
	}

	internal void InitializeGrid(DocumentGridControl gridControl, object itemSource)
	{
		gridControl.ItemsSource = itemSource;
		gridControl.MinHeight = 150.0;
		gridControl.AllowDuplicate = false;
		gridControl.ButtonDuplicate.IsVisible = false;
		gridControl.AllowMoveUp = true;
		gridControl.AllowMoveDown = true;
		gridControl.MovingRowUp += ExtensionGrids_MovingRowUp;
		gridControl.MovingRowDown += ExtensionGrids_MovingRowDown;
		gridControl.View.ShowGridMenu += View_ShowGridMenu;
		gridControl.WriteToStatusBarForCurrentRow += GridControl_WriteToStatusBarForCurrentRow;
	}

	private void View_ShowGridMenu(object sender, GridMenuEventArgs e)
	{
		if (e.MenuType != GridMenuType.Column)
		{
			return;
		}
		e.Items.Where((BarItem el) => el.Name != "SearchPanel").ForEach((BarItem row) =>
		{
			row.IsVisible = false;
		});
		foreach (IBarManagerControllerAction customization in e.Customizations)
		{
			if (customization is BarButtonItem barButtonItem)
			{
				barButtonItem.IsVisible = false;
			}
			else if (customization is BarItemLinkSeparator barItemLinkSeparator)
			{
				barItemLinkSeparator.IsVisible = false;
			}
		}
	}

	internal (Dictionary<string, DataItem> DataItems, List<UITab> Tabs, List<ArmSoft.SME.Core.UI.MetaData.UIElement> Elements) CreateDocumentExtension()
	{
		Dictionary<string, DataItem> dictionary = new Dictionary<string, DataItem>();
		List<UITab> list = new List<UITab>();
		List<ArmSoft.SME.Core.UI.MetaData.UIElement> list2 = new List<ArmSoft.SME.Core.UI.MetaData.UIElement>();
		foreach (ExtensionField extensionField in _extensionFields)
		{
			if (extensionField.UIDataItem != null)
			{
				DataItem item = extensionField.UIDataItem.Item;
				item.CaptionId = extensionField.CaptionId;
				dictionary.TryAdd(item.Name, item);
			}
		}
		foreach (TabDescription tabDesc in _extensionTabs)
		{
			if (string.IsNullOrWhiteSpace(tabDesc.CaptionId))
			{
				continue;
			}
			UITab uITab = new UITab
			{
				CaptionId = tabDesc.CaptionId
			};
			List<UIGroupDescription> list3 = _extensionGroups.Where((UIGroupDescription g) => g.Tab == tabDesc.CaptionId).ToList();
			List<ExtensionField> list4 = _extensionFields.Where((ExtensionField f) => f.Tab == tabDesc.CaptionId).ToList();
			foreach (UIGroupDescription item2 in list3)
			{
				UIGroup uIGroup = CreateUIGroup(item2, dictionary);
				if (uIGroup != null)
				{
					uITab.Elements.Add(uIGroup);
				}
			}
			foreach (ExtensionField item3 in list4)
			{
				UIField uIField = CreateUIField(item3, dictionary);
				if (uIField != null)
				{
					uITab.Elements.Add(uIField);
				}
			}
			if (uITab.Elements.Count > 0 || !tabDesc.IsSystem)
			{
				list.Add(uITab);
			}
		}
		List<UIGroupDescription> list5 = _extensionGroups.Where((UIGroupDescription group) => string.IsNullOrWhiteSpace(group.Tab) && string.IsNullOrWhiteSpace(group.Group)).ToList();
		List<ExtensionField> list6 = _extensionFields.Where((ExtensionField field) => string.IsNullOrWhiteSpace(field.Tab) && string.IsNullOrWhiteSpace(field.Group)).ToList();
		foreach (UIGroupDescription item4 in list5)
		{
			UIGroup uIGroup2 = CreateUIGroup(item4, dictionary);
			if (uIGroup2 != null)
			{
				list2.Add(uIGroup2);
			}
		}
		foreach (ExtensionField item5 in list6)
		{
			UIField uIField2 = CreateUIField(item5, dictionary);
			if (uIField2 != null)
			{
				list2.Add(uIField2);
			}
		}
		return (DataItems: dictionary, Tabs: list, Elements: list2);
	}

	internal bool Validate(out string errorMessage, out Control errorControl)
	{
		errorMessage = null;
		errorControl = null;
		foreach (TabDescription tab in _extensionTabs)
		{
			if (string.IsNullOrWhiteSpace(tab.CaptionId))
			{
				errorMessage = string.Format(ArmSoft.SME.Common.Resources.GridHasRowsWithEmptyCode, ArmSoft.SME.Common.Resources.Pages);
				errorControl = _extensionTabsGrid;
				return false;
			}
			if (_extensionTabs.Count((TabDescription item) => item.CaptionId == tab.CaptionId) > 1)
			{
				string colNumRepeatedInTable = ArmSoft.SME.Common.Resources.ColNumRepeatedInTable;
				InlineArray4<object> buffer = default;
				buffer[0] = ArmSoft.SME.Common.Resources.Name;
				buffer[1] = ArmSoft.SME.Common.Resources.Pages;
				buffer[2] = tab.CaptionId;
				buffer[3] = tab.CaptionId;
				errorMessage = string.Format(colNumRepeatedInTable, (ReadOnlySpan<object?>)buffer);
				errorControl = _extensionTabsGrid;
				return false;
			}
		}
		foreach (UIGroupDescription group in _extensionGroups)
		{
			if (string.IsNullOrWhiteSpace(group.Name))
			{
				errorMessage = string.Format(ArmSoft.SME.Common.Resources.GridHasRowsWithEmptyCode, ArmSoft.SME.Common.Resources.Groups);
				errorControl = _extensionGroupsGrid;
				return false;
			}
			if (_extensionGroups.Count((UIGroupDescription item) => item.Name == group.Name) > 1)
			{
				string colNumRepeatedInTable2 = ArmSoft.SME.Common.Resources.ColNumRepeatedInTable;
				InlineArray4<object> buffer2 = default;
				buffer2[0] = ArmSoft.SME.Common.Resources.Name;
				buffer2[1] = ArmSoft.SME.Common.Resources.Groups;
				buffer2[2] = group.Name;
				buffer2[3] = group.Name;
				errorMessage = string.Format(colNumRepeatedInTable2, (ReadOnlySpan<object?>)buffer2);
				errorControl = _extensionGroupsGrid;
				return false;
			}
			if (!IsValueParentValid(group.Name, ArmSoft.SME.Common.Resources.Groups, ArmSoft.SME.Common.Resources.Page, group.Tab, ArmSoft.SME.Common.Resources.Pages, _extensionTabs, out errorMessage))
			{
				errorControl = _extensionGroupsGrid;
				return false;
			}
			if (!IsValueParentValid(group.Name, ArmSoft.SME.Common.Resources.Groups, ArmSoft.SME.Common.Resources.Group, group.Group, ArmSoft.SME.Common.Resources.Groups, _extensionGroups, out errorMessage))
			{
				errorControl = _extensionGroupsGrid;
				return false;
			}
		}
		foreach (ExtensionField field in _extensionFields)
		{
			if (field.UIDataItem == null || !field.UIDataItem.IsConfigured)
			{
				int num = _extensionFields.IndexOf(field) + 1;
				errorMessage = string.Format(ArmSoft.SME.Common.Resources.DocExtensionFieldTypeNotConfigured, ArmSoft.SME.Common.Resources.Fields, field.Name, num);
				errorControl = _extensionFieldsGrid;
				return false;
			}
			if (string.IsNullOrWhiteSpace(field.Name))
			{
				errorMessage = string.Format(ArmSoft.SME.Common.Resources.GridHasRowsWithEmptyCode, ArmSoft.SME.Common.Resources.Fields);
				errorControl = _extensionFieldsGrid;
				return false;
			}
			if (_extensionFields.Count((ExtensionField item) => item.Name == field.Name) > 1)
			{
				string colNumRepeatedInTable3 = ArmSoft.SME.Common.Resources.ColNumRepeatedInTable;
				InlineArray4<object> buffer3 = default;
				buffer3[0] = ArmSoft.SME.Common.Resources.Name;
				buffer3[1] = ArmSoft.SME.Common.Resources.Fields;
				buffer3[2] = field.Name;
				buffer3[3] = field.Name;
				errorMessage = string.Format(colNumRepeatedInTable3, (ReadOnlySpan<object?>)buffer3);
				errorControl = _extensionFieldsGrid;
				return false;
			}
			if (!IsValueParentValid(field.Name, ArmSoft.SME.Common.Resources.Fields, ArmSoft.SME.Common.Resources.Group, field.Group, ArmSoft.SME.Common.Resources.Groups, _extensionGroups, out errorMessage))
			{
				errorControl = _extensionFieldsGrid;
				return false;
			}
			if (!IsValueParentValid(field.Name, ArmSoft.SME.Common.Resources.Fields, ArmSoft.SME.Common.Resources.Page, field.Tab, ArmSoft.SME.Common.Resources.Pages, _extensionTabs, out errorMessage))
			{
				errorControl = _extensionFieldsGrid;
				return false;
			}
		}
		return true;
	}

	private void PopulateTabGridColumns()
	{
		_extensionTabsGrid.AddDropDownColumn("CaptionId", ArmSoft.SME.Common.Resources.CaptionId, _stringResourcesView, allowAdditionButtons: true);
		_extensionTabsGrid.View.CellValueChanged += View_CellValueChanged;
		_extensionTabsGrid.AddStringColumn("Header", ArmSoft.SME.Common.Resources.Name, 50).ReadOnly = true;
		_extensionTabsGrid.AddBooleanColumn("IsSystem", ArmSoft.SME.Common.Resources.System).ReadOnly = true;
		_extensionTabsGrid.AddingRow += ExtensionTabsGrid_AddingRow;
		_extensionTabsGrid.Grid.CurrentItemChanged += Grid_CurrentItemChanged;
	}

	private void Grid_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
	{
		if (e.NewItem is TabDescription tabDescription)
		{
			if (tabDescription.IsSystem)
			{
				_extensionTabsGrid.ButtonDelete.IsEnabled = false;
			}
			else
			{
				_extensionTabsGrid.ButtonDelete.IsEnabled = true;
			}
		}
	}

	private void SetTabCaption(TabDescription tabDescription)
	{
		if (tabDescription.IsSystem)
		{
			if (_systemPages.TryGetValue(tabDescription.CaptionId, out var value))
			{
				tabDescription.Header = value;
			}
		}
		else
		{
			UserResourcesDataProvider.DataRow rowByValue = _stringResourcesView.GetRowByValue(tabDescription.CaptionId);
			tabDescription.Header = ((rowByValue == null) ? "" : rowByValue.Caption);
		}
	}

	private void PopulateGroupGridColumns()
	{
		double num = SizeCalculator.WidthForString(10);
		DocumentGridColumn documentGridColumn = _extensionGroupsGrid.AddStringColumn("Name", ArmSoft.SME.Common.Resources.Code, 50);
		documentGridColumn.Width = num;
		documentGridColumn.CellTemplate = (DataTemplate)Settings.GenericResources["ExtensionFieldCodeTemplate"];
		_extensionGroupsGrid.AddDropDownColumn("CaptionId", ArmSoft.SME.Common.Resources.CaptionId, _stringResourcesView, allowAdditionButtons: true).Width = num;
		if (_allowAddTabs)
		{
			_extensionGroupsGrid.AddDropDownColumn("Tab", ArmSoft.SME.Common.Resources.Page, _pagesSingleElements).Width = num;
		}
		DocumentGridColumn documentGridColumn2 = _extensionGroupsGrid.AddDropDownColumn("Group", ArmSoft.SME.Common.Resources.ParentGroup, 10);
		documentGridColumn2.Width = num;
		documentGridColumn2.IsOwnProcessingClickDropDown = true;
		_extensionGroupsGrid.AddingRow += ExtensionGroupsGrid_AddingRow;
		_extensionGroupsGrid.ClickDropDown += ExtensionGroupsGrid_ClickDropDown;
		_extensionGroupsGrid.View.CellValueChanged += GroupView_CellValueChanged;
	}

	private void PopulateFieldsGridColumns()
	{
		double num = SizeCalculator.WidthForString(10);
		_extensionFieldsGrid.AddStringColumn("Name", ArmSoft.SME.Common.Resources.Code, 10).CellTemplate = (DataTemplate)Settings.GenericResources["ExtensionFieldCodeTemplate"];
		_extensionFieldsGrid.AddDropDownColumn("CaptionId", ArmSoft.SME.Common.Resources.CaptionId, _stringResourcesView, allowAdditionButtons: true).Width = SizeCalculator.WidthForString(10);
		DocumentGridColumn documentGridColumn = _extensionFieldsGrid.AddDropDownColumn("Type", ArmSoft.SME.Common.Resources.Type, new SmartEnum(typeof(FieldTypeSE)));
		documentGridColumn.AddAdditionalButton("t_edit_16.svg", ArmSoft.SME.Common.Resources.EditAttributes).Click += new SafeEventHandler<RoutedEventArgs>(EditAttributesButton_Click).Handler;
		documentGridColumn.Width = num;
		DocumentGridColumn documentGridColumn2 = _extensionFieldsGrid.AddDropDownColumn("DropDownType", ArmSoft.SME.Common.Resources.DropDown, new SmartEnum(typeof(DropDownControlTypeSE)));
		documentGridColumn2.AddAdditionalButton("t_edit_16.svg", ArmSoft.SME.Common.Resources.EditAttributes).Click += new SafeEventHandler<RoutedEventArgs>(FieldDropDownEdit_Click).Handler;
		documentGridColumn2.Width = SizeCalculator.WidthForString(10);
		_extensionFieldsGrid.AddDropDownColumn("HorizontalAlignmentName", ArmSoft.SME.Common.Resources.HorizontalAlignment, new SingleElements(ArmSoft.SME.Common.Resources.HorizontalAlignment, DataItemHelper.DataItemHorizontalAlignmentList)).Width = num;
		_extensionFieldsGrid.AddBooleanColumn("ShowInMobile", ArmSoft.SME.Common.Resources.ShowInMobile);
		_extensionFieldsGrid.AddBooleanColumn("HideCaption", ArmSoft.SME.Common.Resources.HideCaption);
		_extensionFieldsGrid.AddBooleanColumn("StoreValue", ArmSoft.SME.Common.Resources.StoreValue);
		if (_allowAddTabs)
		{
			_extensionFieldsGrid.AddDropDownColumn("Tab", ArmSoft.SME.Common.Resources.Page, _pagesSingleElements).Width = num;
		}
		_extensionFieldsGrid.AddDropDownColumn("Group", ArmSoft.SME.Common.Resources.Group, new SingleElements(ArmSoft.SME.Common.Resources.Groups, _extensionGroups.Select((UIGroupDescription el) => el.Name))).Width = num;
		_extensionFieldsGrid.AddingRow += ExtensionGrid_AddingRow;
		_extensionFieldsGrid.View.ShowingEditor += new SafeEventHandler<ShowingEditorEventArgs>(ExtensionFieldsView_ShowingEditor).Handler;
		_extensionFieldsGrid.View.CellValueChanged += FieldView_CellValueChanged;
	}

	internal void LoadExtensionGrids(List<UITab> tabs, List<ArmSoft.SME.Core.UI.MetaData.UIElement> elements)
	{
		if (tabs != null)
		{
			foreach (UITab tab in tabs)
			{
				_extensionTabs.Add(new TabDescription
				{
					CaptionId = tab.CaptionId
				});
				if (tab.Elements.Count <= 0)
				{
					continue;
				}
				foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element in tab.Elements)
				{
					if (element is UIGroup uIGroup)
					{
						InitGroupElements(uIGroup, tab.CaptionId);
					}
					else if (element is UIField field)
					{
						_extensionFields.Add(new ExtensionField(field, tab.CaptionId));
					}
				}
			}
		}
		if (elements == null)
		{
			return;
		}
		foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element2 in elements)
		{
			if (element2 is UIGroup uIGroup2)
			{
				InitGroupElements(uIGroup2);
			}
			else if (element2 is UIField field2)
			{
				_extensionFields.Add(new ExtensionField(field2));
			}
		}
	}

	private void InitGroupElements(UIGroup group, string parentTab = "", string parentGroup = "")
	{
		_extensionGroups.Add(new UIGroupDescription
		{
			Name = group.Name,
			CaptionId = ((!string.IsNullOrWhiteSpace(group.CaptionId)) ? group.CaptionId : group.Caption),
			IsCollapsed = group.IsCollapsed,
			IsCollapsible = group.IsCollapsible,
			IsHorizontal = group.IsHorizontal,
			Tab = parentTab,
			Group = parentGroup
		});
		if (group.Elements.Count <= 0)
		{
			return;
		}
		foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element in group.Elements)
		{
			if (element is UIField field)
			{
				_extensionFields.Add(new ExtensionField(field, null, group.Name));
			}
			else if (element is UIGroup uIGroup)
			{
				InitGroupElements(uIGroup, "", group.Name);
			}
		}
	}

	private UIGroup CreateUIGroup(UIGroupDescription groupDesc, Dictionary<string, DataItem> dataItems)
	{
		if (string.IsNullOrWhiteSpace(groupDesc.Name))
		{
			return null;
		}
		UIGroup uIGroup = new UIGroup
		{
			Name = groupDesc.Name,
			CaptionId = groupDesc.CaptionId,
			IsHorizontal = groupDesc.IsHorizontal,
			IsCollapsible = groupDesc.IsCollapsible,
			IsCollapsed = groupDesc.IsCollapsed
		};
		List<UIGroupDescription> list = _extensionGroups.Where((UIGroupDescription group) => group.Group == groupDesc.Name).ToList();
		foreach (ExtensionField item in _extensionFields.Where((ExtensionField field) => field.Group == groupDesc.Name).ToList())
		{
			UIField uIField = CreateUIField(item, dataItems);
			if (uIField != null)
			{
				uIGroup.Elements.Add(uIField);
			}
		}
		foreach (UIGroupDescription item2 in list)
		{
			UIGroup uIGroup2 = CreateUIGroup(item2, dataItems);
			if (uIGroup2 != null)
			{
				uIGroup.Elements.Add(uIGroup2);
			}
		}
		return uIGroup;
	}

	private UIField CreateUIField(ExtensionField fieldDesc, Dictionary<string, DataItem> dataItems)
	{
		if (string.IsNullOrWhiteSpace(fieldDesc.Name) || fieldDesc.UIDataItem?.Item == null)
		{
			return null;
		}
		if (!dataItems.TryGetValue(fieldDesc.Name, out var value))
		{
			return null;
		}
		return new UIField
		{
			Name = fieldDesc.Name,
			DataItem = value,
			DropDownItem = fieldDesc.DropDownItem,
			CaptionId = fieldDesc.CaptionId,
			HideCaption = fieldDesc.HideCaption,
			HorizontalAlignment = fieldDesc.HorizontalAlignment,
			ShowInMobile = fieldDesc.ShowInMobile,
			StoreValue = fieldDesc.StoreValue
		};
	}

	public void UpdateGridSources(short poTypeDef, short poType, bool updateExtendedItemsGrids = true)
	{
		_systemPages?.Clear();
		if (updateExtendedItemsGrids)
		{
			_extensionTabs.Clear();
			_extensionFields.Clear();
			_extensionGroups.Clear();
		}
		IPOWindow iPOWindow = ((poTypeDef != POTypeSE.Directory.Value) ? Settings.ServiceProvider.GetRequiredService<IDocumentService>().CreateWindow(poType) : Settings.ServiceProvider.GetRequiredService<IDirectoryService>().CreatePOWindow(poType));
		if (iPOWindow == null)
		{
			return;
		}
		_systemPages = iPOWindow.GetTabs();
		if (_systemPages != null)
		{
			foreach (KeyValuePair<string, string> item in _systemPages.Reverse())
			{
				TabDescription tabDescription = _extensionTabs.FirstOrDefault((TabDescription el) => el.CaptionId == item.Key);
				if (tabDescription == null)
				{
					_extensionTabs.Insert(0, new TabDescription
					{
						CaptionId = item.Key,
						Header = item.Value,
						IsSystem = true
					});
				}
				else
				{
					_extensionTabs.Move(_extensionTabs.IndexOf(tabDescription), 0);
					tabDescription.IsSystem = true;
				}
			}
		}
		_extensionTabs.ForEach(SetTabCaption);
		_pagesSingleElements.DataProvider = new SingleElementsDataProvider(ArmSoft.SME.Common.Resources.Pages, _extensionTabs.Select((TabDescription el) => el.CaptionId));
		_pagesSingleElements.ReloadRows();
	}

	private void ExtensionFieldsView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (_extensionFieldsGrid.Grid.CurrentItem is ExtensionField extensionField)
		{
			if (e.Column.FieldName == "Type")
			{
				e.Cancel = true;
			}
			else if (e.Column.FieldName == "DropDownType")
			{
				_extensionFieldsGrid.Grid.Columns["DropDownType"].ReadOnly = extensionField.Type != "String";
			}
			else if (e.Column.FieldName == "HorizontalAlignmentName")
			{
				e.Cancel = true;
			}
			if (e.Column.FieldName == "Group")
			{
				_extensionFieldsGrid.Grid.Columns["Group"].ReadOnly = !string.IsNullOrWhiteSpace(extensionField.Tab);
			}
			else if (e.Column.FieldName == "Tab")
			{
				_extensionFieldsGrid.Grid.Columns["Tab"].ReadOnly = !string.IsNullOrWhiteSpace(extensionField.Group);
			}
		}
	}

	private void FieldView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		if (!(_extensionFieldsGrid.Grid.CurrentItem is ExtensionField extensionField))
		{
			return;
		}
		string text = e.Value?.ToString() ?? "";
		string value = e.OldValue?.ToString() ?? "";
		if (e.Column.FieldName == "Name" && extensionField.UIDataItem != null)
		{
			extensionField.UIDataItem.Item.Name = text;
		}
		else if (e.Column.FieldName == "CaptionId" && extensionField.UIDataItem != null)
		{
			extensionField.UIDataItem.Item.CaptionId = extensionField.CaptionId;
		}
		else if (e.Column.FieldName == "Type")
		{
			if (!text.Equals(value))
			{
				extensionField.UIDataItem = DataItemHelper.GetDataItem(text).GetUIDataItem();
				if (!text.Equals("String"))
				{
					extensionField.DropDownItem = null;
					extensionField.DropDownType = "";
				}
			}
		}
		else if (e.Column.FieldName == "DropDownType")
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				extensionField.DropDownItem = null;
			}
			else if (!text.Equals(value))
			{
				extensionField.DropDownItem = new DropDown(text);
				switch (text)
				{
				case "DropDownComment":
					extensionField.DropDownItem.IsDropDownComment = true;
					break;
				case "MultiSelect":
					extensionField.DropDownItem.IsMultiSelect = true;
					break;
				case "FileDialog":
					extensionField.DropDownItem.IsFileDialog = true;
					break;
				}
			}
		}
		else if (e.Column.FieldName == "Group" && _allowAddTabs)
		{
			GridColumn gridColumn = _extensionFieldsGrid.Grid.Columns["Tab"];
			if (!string.IsNullOrWhiteSpace(text))
			{
				gridColumn.ReadOnly = true;
			}
			else
			{
				gridColumn.ReadOnly = false;
			}
		}
		else if (e.Column.FieldName == "Tab")
		{
			GridColumn gridColumn2 = _extensionFieldsGrid.Grid.Columns["Group"];
			if (!string.IsNullOrWhiteSpace(text))
			{
				gridColumn2.ReadOnly = true;
			}
			else
			{
				gridColumn2.ReadOnly = false;
			}
		}
	}

	private void GroupView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		if (_extensionGroupsGrid.Grid.CurrentItem is UIGroupDescription && e.Column.FieldName == "Group" && _allowAddTabs)
		{
			GridColumn gridColumn = _extensionGroupsGrid.Grid.Columns["Tab"];
			if (!string.IsNullOrWhiteSpace(e.Value?.ToString()))
			{
				gridColumn.ReadOnly = true;
			}
			else
			{
				gridColumn.ReadOnly = false;
			}
		}
	}

	private void EditAttributesButton_Click(object sender, RoutedEventArgs e)
	{
		if (_extensionFieldsGrid.Grid.CurrentItem is ExtensionField { UIDataItem: not null } extensionField)
		{
			extensionField.UIDataItem.ShowAttributeDialog(_owner);
		}
	}

	private void FieldDropDownEdit_Click(object sender, RoutedEventArgs e)
	{
		if (_extensionFieldsGrid.Grid.CurrentItem is ExtensionField { DropDownItem: not null } extensionField)
		{
			new DropDownItemDialogWindow(extensionField.DropDownItem, _owner).ShowDialog();
		}
	}

	private void ExtensionGroupsGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		if (!(e.Column.FieldName == "Group"))
		{
			return;
		}
		object currentItem = _extensionGroupsGrid.Grid.CurrentItem;
		UIGroupDescription groupRow = currentItem as UIGroupDescription;
		if (groupRow != null)
		{
			SingleElements singleElements = new SingleElements(ArmSoft.SME.Common.Resources.Groups, _extensionGroups.Where((UIGroupDescription el) => el.Name != null && !el.Name.Equals(groupRow.Name))?.Select((UIGroupDescription el) => el.Name));
			ModalBrowser modalBrowser = new ModalBrowser(singleElements)
			{
				Top = e.Top,
				Left = e.Left,
				Owner = _owner
			};
			if (modalBrowser.Show())
			{
				groupRow.Group = singleElements.GetRowByIndex(modalBrowser.SelectedRowIndex).Name;
				_extensionGroupsGrid.Grid.RefreshRow(_extensionGroupsGrid.View.FocusedRowHandle);
			}
		}
	}

	private void ExtensionTabsGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		_extensionTabs.Add(new TabDescription());
		e.RowNumber = _extensionTabs.Count - 1;
	}

	private void ExtensionGroupsGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		_extensionGroups.Add(new UIGroupDescription
		{
			Name = "UD_"
		});
		e.RowNumber = _extensionGroups.Count - 1;
	}

	private void ExtensionGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		_extensionFields.Add(new ExtensionField
		{
			Name = "UD_"
		});
		e.RowNumber = _extensionFields.Count - 1;
	}

	private void ExtensionGrids_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		MoveRowsUpDown(e.DocumentGrid.Name, e.RowHandle, isUp: true);
	}

	private void ExtensionGrids_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		MoveRowsUpDown(e.DocumentGrid.Name, e.RowHandle, isUp: false);
	}

	private void MoveRowsUpDown(string gridName, int rowHandle, bool isUp)
	{
		if (gridName.Equals(_extensionFieldsGrid.Name))
		{
			MoveFieldsRowUpDown(rowHandle, isUp);
		}
		else if (gridName.Equals(_extensionGroupsGrid.Name))
		{
			MoveGroupsRowUpDown(rowHandle, isUp);
		}
		else if (gridName.Equals(_extensionTabsGrid.Name))
		{
			MoveTabsRowUpDown(rowHandle, isUp);
		}
	}

	private void MoveFieldsRowUpDown(int rowHandle, bool isDirectionUp)
	{
		if (_extensionFieldsGrid.Grid.VisibleRowCount == _extensionFields.Count && _extensionFieldsGrid.Grid.GetRow(rowHandle) is ExtensionField item)
		{
			_extensionFields.RemoveAt(rowHandle);
			_extensionFields.Insert(isDirectionUp ? (rowHandle - 1) : (rowHandle + 1), item);
		}
	}

	private void MoveGroupsRowUpDown(int rowHandle, bool isUp)
	{
		if (_extensionGroupsGrid.Grid.VisibleRowCount == _extensionGroups.Count && _extensionGroupsGrid.Grid.GetRow(rowHandle) is UIGroupDescription item)
		{
			_extensionGroups.RemoveAt(rowHandle);
			_extensionGroups.Insert(isUp ? (rowHandle - 1) : (rowHandle + 1), item);
		}
	}

	private void MoveTabsRowUpDown(int rowHandle, bool isDirectionUp)
	{
		if (_extensionTabsGrid.Grid.VisibleRowCount == _extensionTabs.Count && _extensionTabsGrid.Grid.GetRow(rowHandle) is TabDescription item)
		{
			_extensionTabs.RemoveAt(rowHandle);
			_extensionTabs.Insert(isDirectionUp ? (rowHandle - 1) : (rowHandle + 1), item);
		}
	}

	private void View_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		if (_extensionTabsGrid.Grid.CurrentItem is TabDescription tabCaption && e.Column.FieldName == "CaptionId")
		{
			SetTabCaption(tabCaption);
		}
	}

	private void GridControl_WriteToStatusBarForCurrentRow(object sender, string msg)
	{
		if (_owner is DialogWindow dialogWindow)
		{
			dialogWindow.WriteToStatusBar(msg);
		}
	}

	private void SetReadOnlyState(bool isReadOnly)
	{
		_extensionTabsGrid.IsReadOnly = isReadOnly;
		_extensionGroupsGrid.IsReadOnly = isReadOnly;
		_extensionFieldsGrid.IsReadOnly = isReadOnly;
	}

	private bool IsValueParentValid(string childCode, string childGridCaption, string columnName, string parentValue, string parentGridCaption, IEnumerable<UIElementDescription> rows, out string errorMessage)
	{
		if (!string.IsNullOrWhiteSpace(parentValue) && !rows.Any((UIElementDescription item) => (!(item is UIGroupDescription uIGroupDescription)) ? item.CaptionId.Equals(parentValue) : uIGroupDescription.Name.Equals(parentValue)))
		{
			string valueNotExistsInGrid = ArmSoft.SME.Common.Resources.ValueNotExistsInGrid;
			InlineArray5<object> buffer = default;
			buffer[0] = parentGridCaption;
			buffer[1] = childGridCaption;
			buffer[2] = childCode;
			buffer[3] = columnName;
			buffer[4] = parentValue;
			errorMessage = string.Format(valueNotExistsInGrid, (ReadOnlySpan<object?>)buffer);
			return false;
		}
		errorMessage = null;
		return true;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
