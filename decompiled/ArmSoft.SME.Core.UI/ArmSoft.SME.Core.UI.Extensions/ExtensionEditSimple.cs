using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.DropDowns.MetaData;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.MetaData.DocumentExtension;
using ArmSoft.SME.Core.UI.MetaData.UI_Definition;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Core.UI.Extensions;

public class ExtensionEditSimple : UserControl
{
	public class ExtensionFieldSimple : DocumentGridRow
	{
		[ObservableProperty]
		public string _name;

		[ObservableProperty]
		public string _caption;

		[ObservableProperty]
		public string _type;

		[ObservableProperty]
		public string _dropDownName;

		[ObservableProperty]
		public bool _required;

		[ObservableProperty]
		public bool _saveValueOnCopy;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Name
		{
			get
			{
				return _name;
			}
			[MemberNotNull("_name")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_name, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Name);
					_name = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Name);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Caption
		{
			get
			{
				return _caption;
			}
			[MemberNotNull("_caption")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_caption, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Caption);
					_caption = value;
					OnCaptionChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Caption);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Type
		{
			get
			{
				return _type;
			}
			[MemberNotNull("_type")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_type, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Type);
					_type = value;
					OnTypeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Type);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string DropDownName
		{
			get
			{
				return _dropDownName;
			}
			[MemberNotNull("_dropDownName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_dropDownName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.DropDownName);
					_dropDownName = value;
					OnDropDownNameChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.DropDownName);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool Required
		{
			get
			{
				return _required;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_required, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Required);
					_required = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Required);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool SaveValueOnCopy
		{
			get
			{
				return _saveValueOnCopy;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_saveValueOnCopy, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SaveValueOnCopy);
					_saveValueOnCopy = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SaveValueOnCopy);
				}
			}
		}

		static ExtensionFieldSimple()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(ExtensionFieldSimple), cacheValidationResults: false, new Dictionary<string, Action<DocumentGridRow, ErrorInfo>>
			{
				{
					"Caption",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateCaption((ExtensionFieldSimple)row, info);
					}
				},
				{
					"Type",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateType((ExtensionFieldSimple)row, info);
					}
				},
				{
					"DropDownName",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateDropDownName((ExtensionFieldSimple)row, info);
					}
				}
			});
		}

		private static void ValidateCaption(ExtensionFieldSimple row, ErrorInfo info)
		{
			ArmSoft.SME.Core.UI.Documents.PropertyValidator.EmptyStringValidate(row.Caption, info, ArmSoft.SME.Common.Resources.Name);
		}

		private static void ValidateType(ExtensionFieldSimple row, ErrorInfo info)
		{
			ArmSoft.SME.Core.UI.Documents.PropertyValidator.EmptyStringValidate(row.Type, info, ArmSoft.SME.Common.Resources.Type);
			if (info.ErrorType != ErrorType.Critical && !TypeDropDown.ValuesList().Contains(row.Type))
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, ArmSoft.SME.Common.Resources.Type, row.Type);
			}
		}

		private static void ValidateDropDownName(ExtensionFieldSimple row, ErrorInfo info)
		{
			if (row.Type == FieldTypeExtSimpleModeSE.DropDownType.Caption)
			{
				ArmSoft.SME.Core.UI.Documents.PropertyValidator.EmptyStringValidate(row.DropDownName, info, ArmSoft.SME.Common.Resources.DropDown);
				if (!DropDownTypeDropDown.ValuesList().Contains(row.DropDownName))
				{
					info.ErrorType = ErrorType.Critical;
					info.ErrorText = string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, ArmSoft.SME.Common.Resources.DropDown, row.DropDownName);
				}
			}
			else if (!string.IsNullOrWhiteSpace(row.DropDownName))
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = string.Format(ArmSoft.SME.Common.Resources.FieldMustBeEmpty, ArmSoft.SME.Common.Resources.DropDown, row.DropDownName);
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnCaptionChanged(string value)
		{
			ResetForceValidationBit("Caption");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnTypeChanged(string value)
		{
			ResetForceValidationBit("Type");
			ResetForceValidationBit("DropDownName");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnDropDownNameChanged(string value)
		{
			ResetForceValidationBit("DropDownName");
		}
	}

	public static SingleElements TypeDropDown = new SingleElements(ArmSoft.SME.Common.Resources.FieldTypes, SmartEnum<FieldTypeExtSimpleModeSE, string>.List.Select((FieldTypeExtSimpleModeSE p) => p.Caption));

	public static DropDownType DropDownTypeDropDown = new DropDownType(onlyUsedInExtensions: true, showOnlyCaption: true);

	private readonly Window _owner;

	private readonly List<ExtensionField> _extensionFields = new List<ExtensionField>();

	private readonly ObservableCollection<ExtensionFieldSimple> _extensionFieldsSimple = new ObservableCollection<ExtensionFieldSimple>();

	private DocumentGridColumn _dropDownTypeColumn;

	private bool _isReadOnly;

	private DocumentGridControl _extensionFieldsGrid;

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

	public ExtensionEditSimple(DialogWindow owner)
	{
		_owner = owner;
		DXTabControl tc = owner.AddTabControl(null);
		LayoutGroup container = owner.AddTabedGroup(tc, ArmSoft.SME.Common.Resources.Fields);
		_extensionFieldsGrid = owner.AddGridControl(container, "ExtensionFieldsGrid");
		InitializeGrid();
		PopulateFieldsGridColumns();
	}

	internal void InitializeGrid()
	{
		_extensionFieldsGrid.ItemsSource = _extensionFieldsSimple;
		_extensionFieldsGrid.AllowAdd = true;
		_extensionFieldsGrid.AllowDelete = true;
		_extensionFieldsGrid.ShowButtons = true;
		_extensionFieldsGrid.Height = 350.0;
		_extensionFieldsGrid.AllowDuplicate = false;
		_extensionFieldsGrid.ButtonDuplicate.IsVisible = false;
		_extensionFieldsGrid.AllowMoveUp = true;
		_extensionFieldsGrid.AllowMoveDown = true;
		_extensionFieldsGrid.MovingRowUp += ExtensionGrids_MovingRowUp;
		_extensionFieldsGrid.MovingRowDown += ExtensionGrids_MovingRowDown;
		_extensionFieldsGrid.View.ShowGridMenu += View_ShowGridMenu;
		_extensionFieldsGrid.WriteToStatusBarForCurrentRow += GridControl_WriteToStatusBarForCurrentRow;
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

	private void ConvertExtensionFieldsFromSimple()
	{
		_extensionFields.Clear();
		foreach (ExtensionFieldSimple row in _extensionFieldsSimple)
		{
			ExtensionField extensionField = new ExtensionField
			{
				Caption = row.Caption,
				Name = row.Name,
				Type = (SmartEnum<FieldTypeExtSimpleModeSE, string>.List.FirstOrDefault((FieldTypeExtSimpleModeSE p) => p.Caption == row.Type)?.Value ?? ((string)FieldTypeExtSimpleModeSE.StringType)),
				Tab = ArmSoft.SME.Common.Constants.Extensions.DefaultTabName
			};
			if (!string.IsNullOrWhiteSpace(row.DropDownName))
			{
				string text = SmartEnumHelper.GetSmartEnumNameCaption(typeof(DropDownTypeSE)).FirstOrDefault((CodeName p) => p.Name == row.DropDownName)?.Code;
				extensionField.Type = FieldTypeExtSimpleModeSE.StringType;
				extensionField.DropDownType = "DropDown";
				extensionField.DropDownItem = new DropDown("DropDown")
				{
					Group = text,
					Name = text
				};
			}
			extensionField.UIDataItem = DataItemHelper.GetDataItem((extensionField.Type == FieldTypeExtSimpleModeSE.DropDownType) ? ((string)FieldTypeExtSimpleModeSE.StringType) : extensionField.Type).GetUIDataItem();
			extensionField.UIDataItem.Item.Name = row.Name;
			extensionField.UIDataItem.Item.Caption = row.Caption;
			extensionField.UIDataItem.Item.Required = row.Required;
			extensionField.UIDataItem.Item.SaveValueOnCopy = row.SaveValueOnCopy;
			_extensionFields.Add(extensionField);
		}
	}

	private void ConvertExtensionFieldsToSimple()
	{
		_extensionFieldsSimple.Clear();
		string dropDownName = null;
		foreach (ExtensionField row in _extensionFields)
		{
			if (row.DropDownItem != null)
			{
				dropDownName = SmartEnumHelper.GetSmartEnumNameCaption(typeof(DropDownTypeSE)).FirstOrDefault((CodeName p) => p.Code == row.DropDownItem?.Group)?.Name;
			}
			ExtensionFieldSimple extensionFieldSimple = new ExtensionFieldSimple
			{
				Caption = row.UIDataItem.Item.Caption,
				Name = row.UIDataItem.Item.Name,
				Type = ((row.DropDownItem == null) ? SmartEnum<FieldTypeExtSimpleModeSE, string>.FromName(row.Type).Caption : FieldTypeExtSimpleModeSE.DropDownType.Caption),
				DropDownName = dropDownName,
				Required = row.UIDataItem.Item.Required,
				SaveValueOnCopy = row.UIDataItem.Item.SaveValueOnCopy
			};
			ChangeDropDownView(extensionFieldSimple);
			_extensionFieldsSimple.Add(extensionFieldSimple);
		}
	}

	internal (Dictionary<string, DataItem> DataItems, List<UITab> Tabs, List<ArmSoft.SME.Core.UI.MetaData.UIElement> Elements) CreateDocumentExtension()
	{
		Dictionary<string, DataItem> dictionary = new Dictionary<string, DataItem>();
		List<UITab> list = new List<UITab>();
		List<ArmSoft.SME.Core.UI.MetaData.UIElement> list2 = new List<ArmSoft.SME.Core.UI.MetaData.UIElement>();
		ConvertExtensionFieldsFromSimple();
		foreach (ExtensionField extensionField in _extensionFields)
		{
			if (extensionField.UIDataItem != null)
			{
				DataItem item = extensionField.UIDataItem.Item;
				item.CaptionId = extensionField.CaptionId;
				dictionary.TryAdd(item.Name, item);
			}
		}
		UITab uITab = new UITab
		{
			CaptionId = ArmSoft.SME.Common.Constants.Extensions.DefaultTabName
		};
		foreach (ExtensionField extensionField2 in _extensionFields)
		{
			UIField uIField = CreateUIField(extensionField2, dictionary);
			if (uIField != null)
			{
				uITab.Elements.Add(uIField);
			}
		}
		list.Add(uITab);
		foreach (ExtensionField item2 in _extensionFields.Where((ExtensionField field) => string.IsNullOrWhiteSpace(field.Tab) && string.IsNullOrWhiteSpace(field.Group)).ToList())
		{
			UIField uIField2 = CreateUIField(item2, dictionary);
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
		return !_extensionFieldsGrid.HasValidationError;
	}

	private void PopulateFieldsGridColumns()
	{
		DocumentGridColumn documentGridColumn = _extensionFieldsGrid.AddStringColumn("Name", ArmSoft.SME.Common.Resources.Code, 10);
		documentGridColumn.CellTemplate = (DataTemplate)Settings.GenericResources["ExtensionFieldCodeTemplate"];
		documentGridColumn.Visible = false;
		_extensionFieldsGrid.AddStringColumn("Caption", ArmSoft.SME.Common.Resources.Name, 255);
		_extensionFieldsGrid.AddDropDownColumn("Type", ArmSoft.SME.Common.Resources.Type, TypeDropDown).Width = SizeCalculator.WidthForString(10);
		_dropDownTypeColumn = _extensionFieldsGrid.AddDropDownColumn("DropDownName", ArmSoft.SME.Common.Resources.DropDown, DropDownTypeDropDown);
		_dropDownTypeColumn.Width = SizeCalculator.WidthForString(20);
		_extensionFieldsGrid.AddBooleanColumn("Required", ArmSoft.SME.Common.Resources.IsRequired);
		_extensionFieldsGrid.AddBooleanColumn("SaveValueOnCopy", ArmSoft.SME.Common.Resources.SaveValueOnCopy);
		_extensionFieldsGrid.AddingRow += new SafeEventHandler<DocumentGridAddingRowEventArgs>(ExtensionGrid_AddingRow).Handler;
		_extensionFieldsGrid.DeletingRow += new SafeEventHandler<DocumentGridDeletingRowEventArgs>(ExtensionGrid_DeletingRow).Handler;
		_extensionFieldsGrid.View.CellValueChanged += new SafeEventHandler<CellValueChangedEventArgs>(FieldView_CellValueChanged).Handler;
		_extensionFieldsGrid.View.ShowingEditor += new SafeEventHandler<ShowingEditorEventArgs>(FieldView_ShowingEditor).Handler;
		_extensionFieldsGrid.View.FocusedRowHandleChanged += new SafeEventHandler<FocusedRowHandleChangedEventArgs>(FieldView_FocusedRowHandleChanged).Handler;
	}

	internal void LoadExtensionGrids(List<UITab> tabs, List<ArmSoft.SME.Core.UI.MetaData.UIElement> elements)
	{
		UITab uITab = tabs?.FirstOrDefault((UITab p) => p.CaptionId == ArmSoft.SME.Common.Constants.Extensions.DefaultTabName);
		if (uITab != null && uITab.Elements.Count > 0)
		{
			foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element in uITab.Elements)
			{
				if (element is UIField field)
				{
					_extensionFields.Add(new ExtensionField(field, uITab.CaptionId));
				}
			}
		}
		if (elements != null)
		{
			foreach (ArmSoft.SME.Core.UI.MetaData.UIElement element2 in elements)
			{
				if (element2 is UIField field2)
				{
					_extensionFields.Add(new ExtensionField(field2));
				}
			}
		}
		ConvertExtensionFieldsToSimple();
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

	private void FieldView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		if (_extensionFieldsGrid.Grid.CurrentItem is ExtensionFieldSimple row && e.Column.FieldName == "Type")
		{
			ChangeDropDownView(row);
		}
	}

	private void ChangeDropDownView(ExtensionFieldSimple row)
	{
		if (row.Type == FieldTypeExtSimpleModeSE.DropDownType.Caption)
		{
			_dropDownTypeColumn.DataView = DropDownTypeDropDown;
		}
		else
		{
			_dropDownTypeColumn.DataView = null;
			row.DropDownName = "";
		}
		_dropDownTypeColumn.Width = SizeCalculator.WidthForString(20);
	}

	private void FieldView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _extensionFieldsGrid.View.FocusedRowHandle;
		ExtensionFieldSimple extensionFieldSimple = (ExtensionFieldSimple)_extensionFieldsGrid.Grid.GetRow(focusedRowHandle);
		if (extensionFieldSimple != null)
		{
			ChangeDropDownView(extensionFieldSimple);
		}
	}

	private void FieldView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (e.Row == null || (!(e.Column.Name == "Required") && !(e.Column.Name == "DropDownName")))
		{
			return;
		}
		ExtensionFieldSimple extensionFieldSimple = (ExtensionFieldSimple)e.Row;
		if (e.Column.Name == "Required")
		{
			if (extensionFieldSimple.Type == FieldTypeExtSimpleModeSE.BooleanType.Caption)
			{
				e.Cancel = true;
			}
		}
		else if (e.Column.Name == "DropDownName" && extensionFieldSimple.Type != FieldTypeExtSimpleModeSE.DropDownType.Caption)
		{
			e.Cancel = true;
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

	private void ExtensionGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		ExtensionFieldSimple extensionFieldSimple = (ExtensionFieldSimple)_extensionFieldsGrid.Grid.GetRow(e.RowHandle);
		if (!string.IsNullOrWhiteSpace(extensionFieldSimple.Caption) && !string.IsNullOrWhiteSpace(extensionFieldSimple.Type) && MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ConfUDFieldDeleteDelete, Settings.StringResourceService.GetStringResource(extensionFieldSimple.Caption)), Settings.ProgramName, MessageBoxButton.YesNo) == MessageBoxResult.No)
		{
			e.Cancel = true;
		}
	}

	private void ExtensionGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		_extensionFieldsSimple.Add(new ExtensionFieldSimple
		{
			Name = "UD_" + Settings.CommonCoreHelper.GetRandomString(6)
		});
		e.RowNumber = _extensionFieldsSimple.Count - 1;
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
	}

	private void MoveFieldsRowUpDown(int rowHandle, bool isDirectionUp)
	{
		if (_extensionFieldsGrid.Grid.VisibleRowCount == _extensionFieldsSimple.Count && _extensionFieldsGrid.Grid.GetRow(rowHandle) is ExtensionFieldSimple item)
		{
			_extensionFieldsSimple.RemoveAt(rowHandle);
			_extensionFieldsSimple.Insert(isDirectionUp ? (rowHandle - 1) : (rowHandle + 1), item);
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
		_extensionFieldsGrid.IsReadOnly = isReadOnly;
	}
}
