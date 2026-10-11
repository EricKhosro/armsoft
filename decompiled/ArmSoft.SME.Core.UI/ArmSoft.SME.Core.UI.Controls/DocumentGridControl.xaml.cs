using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class DocumentGridControl : UserControl, INotifyPropertyChanged, IComponentConnector, IStyleConnector
{
	public delegate void AddingRowEventHandler(object sender, DocumentGridAddingRowEventArgs e);

	public delegate void AddingRowWithDialogEventHandler(object sender, DocumentGridAddingRowEventArgs e);

	public delegate void EditingRowWithDialogEventHandler(object sender, DocumentGridEditingRowEventArgs e);

	public delegate void DuplicatingRowEventHandler(object sender, DocumentGridDuplicatingRowEventArgs e);

	public delegate void DuplicatingRowWithDialogEventHandler(object sender, DocumentGridDuplicatingRowEventArgs e);

	public delegate void DeletingRowEventHandler(object sender, DocumentGridDeletingRowEventArgs e);

	public delegate void DeletedRowEventHandler(object sender, EventArgs e);

	public delegate void WriteToStatusBarForCurrentRowEventHandler(object sender, string msg);

	public delegate void EscapePressedEventHandler(object sender, EventArgs e);

	public delegate void ControlEnterPressedEventHandler(object sender, EventArgs e);

	public delegate void ClickDropDownEventHandler(object sender, DocumentGridClickDropDownEventArgs e);

	public delegate void DropDownEditedEventHandler(object sender, DocumentGridDropDownEditedEventArgs e);

	public delegate void BeforeAddNewDropDownEventHandler(object sender, DocumentGridClickDropDownEventArgs e);

	public delegate void RefreshButtonsEventHandler(object sender, EventArgs e);

	public delegate void InsertingRowEventHandler(object sender, DocumentGridInsertingRowEventArgs e);

	public delegate void MovingRowUpEventHandler(object sender, DocumentGridMovingRowUpOrDownEventArgs e);

	public delegate void MovingRowDownEventHandler(object sender, DocumentGridMovingRowUpOrDownEventArgs e);

	public delegate void PrepareDropDownClick(int rowHandle, DocumentGridColumn column);

	private bool _isReadOnly;

	private bool _isGridMode = true;

	private bool _allowAdd = true;

	private bool _allowEdit = true;

	private bool _allowDelete = true;

	private bool _allowDuplicate = true;

	private bool _allowInsert;

	private bool _allowMoveUp;

	private bool _allowMoveDown;

	private bool _hasRowIndicator;

	private readonly List<BarEditItemExt> _addedButtons;

	private bool _isDocumentGridRowBound;

	private bool _showColHeaderIn2Lines;

	public static readonly DependencyProperty IsReadOnlyProperty;

	public static readonly DependencyProperty IsRequiredProperty;

	public static readonly DependencyProperty HasValidationErrorProperty;

	private bool _hasRowDeletedColumn;

	public static RoutedEvent VisibleRowsCountChanged { get; }

	public bool ShowColHeaderIn2Lines
	{
		get
		{
			return _showColHeaderIn2Lines;
		}
		set
		{
			_showColHeaderIn2Lines = value;
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

	public string Caption { get; set; } = "";

	public Window Owner { get; set; }

	public bool HasRowDeletedColumn
	{
		get
		{
			return _hasRowDeletedColumn;
		}
		set
		{
			_hasRowDeletedColumn = value;
		}
	}

	public PrepareDropDownClick BeforeClickDropDown { get; set; }

	public bool IsRequired
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsRequiredProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsRequiredProperty, (object)value);
			CheckHasValidationError();
		}
	}

	public GridControl Grid => InnerGrid;

	public ToolBarControl InnerButtonsPanel => ButtonsToolbar;

	public TableView View => InnerView;

	public bool IsVisibleViewEditButton
	{
		get
		{
			return ButtonViewEdit.IsVisible;
		}
		set
		{
			ButtonViewEdit.IsVisible = value;
			AllowEdit = value;
		}
	}

	public bool IsVisibleDuplicateButton
	{
		get
		{
			return ButtonDuplicate.IsVisible;
		}
		set
		{
			ButtonDuplicate.IsVisible = value;
			AllowDuplicate = value;
		}
	}

	public bool IsVisibleAddButton
	{
		get
		{
			return ButtonAdd.IsVisible;
		}
		set
		{
			ButtonAdd.IsVisible = value;
			AllowAdd = value;
		}
	}

	public bool IsVisibleDeleteButton
	{
		get
		{
			return ButtonDelete.IsVisible;
		}
		set
		{
			ButtonDelete.IsVisible = value;
			AllowDelete = value;
		}
	}

	public bool IsGridMode
	{
		get
		{
			return _isGridMode;
		}
		set
		{
			_isGridMode = value;
		}
	}

	public bool HasValidationError => (bool)((DependencyObject)this).GetValue(HasValidationErrorProperty);

	public bool IsReadOnly
	{
		get
		{
			return _isReadOnly;
		}
		set
		{
			_isReadOnly = value;
			((DependencyObject)this).SetValue(IsReadOnlyProperty, (object)value);
			View.AllowEditing = true;
			foreach (GridColumn visibleColumn in View.VisibleColumns)
			{
				visibleColumn.ReadOnly = value;
				if (visibleColumn.AllowEditing != DefaultBoolean.False)
				{
					visibleColumn.AllowEditing = DefaultBoolean.True;
				}
			}
			ConfigureButtons();
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReadOnlyChanged"));
		}
	}

	public bool AllowAdd
	{
		get
		{
			return _allowAdd;
		}
		set
		{
			_allowAdd = value;
			ConfigureButtons();
		}
	}

	public bool AllowEdit
	{
		get
		{
			return _allowEdit;
		}
		set
		{
			_allowEdit = value;
			ConfigureButtons();
		}
	}

	public bool IsOpenedInOfflineModeDoc { get; set; }

	public bool AllowDelete
	{
		get
		{
			return _allowDelete;
		}
		set
		{
			_allowDelete = value;
			ConfigureButtons();
		}
	}

	public bool AllowDuplicate
	{
		get
		{
			return _allowDuplicate;
		}
		set
		{
			_allowDuplicate = value;
			ConfigureButtons();
		}
	}

	public bool AllowInsert
	{
		get
		{
			return _allowInsert;
		}
		set
		{
			_allowInsert = value;
			ConfigureButtons();
		}
	}

	public bool AllowMoveUp
	{
		get
		{
			return _allowMoveUp;
		}
		set
		{
			_allowMoveUp = value;
			ConfigureButtons();
		}
	}

	public bool AllowMoveDown
	{
		get
		{
			return _allowMoveDown;
		}
		set
		{
			_allowMoveDown = value;
			ConfigureButtons();
		}
	}

	public object ItemsSource
	{
		get
		{
			return Grid.ItemsSource;
		}
		set
		{
			Grid.ItemsSource = value;
			Type type = value.GetType();
			if (type.IsGenericType)
			{
				Type type2 = type.GetGenericArguments()[0];
				_isDocumentGridRowBound = type2.IsSubclassOf(typeof(DocumentGridRow));
			}
			else
			{
				_isDocumentGridRowBound = false;
			}
			CheckHasValidationError();
		}
	}

	public bool ShowButtons
	{
		get
		{
			return ButtonsToolbar.Visibility == Visibility.Visible;
		}
		set
		{
			ButtonsToolbar.Visibility = ((!value) ? Visibility.Collapsed : Visibility.Visible);
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public event AddingRowEventHandler AddingRow;

	public event AddingRowWithDialogEventHandler AddingRowWithDialog;

	public event EditingRowWithDialogEventHandler EditingRowWithDialog;

	public event DuplicatingRowEventHandler DuplicatingRow;

	public event DuplicatingRowWithDialogEventHandler DuplicatingRowWithDialog;

	public event DeletingRowEventHandler DeletingRow;

	public event DeletedRowEventHandler DeletedRow;

	public event WriteToStatusBarForCurrentRowEventHandler WriteToStatusBarForCurrentRow;

	public event EscapePressedEventHandler EscapePressed;

	public event ControlEnterPressedEventHandler ControlEnterPressed;

	public event ClickDropDownEventHandler ClickDropDown;

	public event DropDownEditedEventHandler DropDownEdited;

	public event BeforeAddNewDropDownEventHandler BeforeAddNewDropDown;

	public event RefreshButtonsEventHandler RefreshButtons;

	public event InsertingRowEventHandler InsertingRow;

	public event MovingRowUpEventHandler MovingRowUp;

	public event MovingRowDownEventHandler MovingRowDown;

	static DocumentGridControl()
	{
		IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DocumentGridControl));
		IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(DocumentGridControl));
		HasValidationErrorProperty = DependencyProperty.Register("HasValidationError", typeof(bool), typeof(DocumentGridControl));
		VisibleRowsCountChanged = EventManager.RegisterRoutedEvent("VisibleRowsCountChangedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DocumentGridControl));
	}

	public DocumentGridColumn AddStringColumn(FieldDefinition fieldDefinition, bool trimValue = true)
	{
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)fieldDefinition;
		return AddStringColumn(stringFieldDefinition.Name, stringFieldDefinition.Caption, stringFieldDefinition.Length, trimValue);
	}

	public DocumentGridColumn AddStringColumn(string propertyName, string caption, short length, bool trimValue = true)
	{
		DocumentGridColumn documentGridColumn = CreateGridColumn(propertyName, caption, trimValue);
		documentGridColumn.AutoFilterCondition = AutoFilterCondition.Contains;
		TextEditSettings editSettings = new TextEditSettings
		{
			MaskType = MaskType.None,
			MaxLength = length,
			AllowNullInput = false
		};
		documentGridColumn.EditSettings = editSettings;
		documentGridColumn.Width = SizeCalculator.WidthForString(length);
		if (HasRowDeletedColumn)
		{
			documentGridColumn.DisplayTemplate = (ControlTemplate)Resources["TextColumnTemplate"];
		}
		Grid.Columns.Add(documentGridColumn);
		return documentGridColumn;
	}

	public DocumentGridColumn AddNumberColumn(FieldDefinition fieldDefinition, bool showTotal = false, bool useCurrencyFormat = true, bool isOwnProcessingClickDropDown = false)
	{
		NumberFieldDefinition numberFieldDefinition = (NumberFieldDefinition)fieldDefinition;
		if (numberFieldDefinition.PrecisionDefinedInParam)
		{
			return AddNumberColumn(numberFieldDefinition.Name, numberFieldDefinition.Caption, numberFieldDefinition.LengthFromParam(Settings.ParametersService), numberFieldDefinition.PrecisionFromParam(Settings.ParametersService), !numberFieldDefinition.IsNonNegative, showTotal, useCurrencyFormat, isOwnProcessingClickDropDown);
		}
		return AddNumberColumn(numberFieldDefinition.Name, numberFieldDefinition.Caption, numberFieldDefinition.Length, numberFieldDefinition.Precision, !numberFieldDefinition.IsNonNegative, showTotal, useCurrencyFormat, isOwnProcessingClickDropDown);
	}

	public DocumentGridColumn AddNumberColumn(string propertyName, string caption, short length, short precision = 0, bool allowNegativeNumbers = false, bool showTotal = false, bool useCurrencyFormat = true, bool isOwnProcessingClickDropDown = false)
	{
		DocumentGridColumn documentGridColumn = CreateGridColumn(propertyName, caption, trimValue: false);
		documentGridColumn.AllowNegativeNumbers = allowNegativeNumbers;
		documentGridColumn.AutoFilterCondition = AutoFilterCondition.Contains;
		documentGridColumn.Precision = precision;
		FormatCondition item = new FormatCondition
		{
			ValueRule = ConditionRule.Less,
			FieldName = propertyName,
			Value1 = 0m,
			PredefinedFormatName = "RedText"
		};
		InnerView.FormatConditions.Add(item);
		if (HasRowDeletedColumn)
		{
			documentGridColumn.DisplayTemplate = (ControlTemplate)Resources["NumericColumnTemplate"];
		}
		else
		{
			documentGridColumn.DisplayTemplate = (ControlTemplate)Settings.GenericResources["ColumnDisplayTemplate"];
		}
		CalcEditSettings calcEditSettings = new CalcEditSettings
		{
			MaskType = MaskType.Numeric,
			Precision = precision,
			Mask = (useCurrencyFormat ? DecimalExtension.GetCurrencyFormat(length, precision) : DecimalExtension.GetNumberFormat(length, precision)),
			MaskUseAsDisplayFormat = true,
			ShowPopupIfReadOnly = false,
			AllowNullInput = false,
			AllowSpinOnMouseWheel = false,
			ValidateOnTextInput = false,
			SelectAllOnMouseUp = true
		};
		if (isOwnProcessingClickDropDown)
		{
			calcEditSettings.AllowDefaultButton = false;
			ButtonInfo buttonInfo = new ButtonInfo
			{
				GlyphKind = GlyphKind.DropDown
			};
			buttonInfo.Click += new SafeEventHandler<RoutedEventArgs>(ColumnDropDownClick).Handler;
			calcEditSettings.Buttons.Add(buttonInfo);
		}
		documentGridColumn.EditSettings = calcEditSettings;
		documentGridColumn.Width = SizeCalculator.WidthForNumber(length);
		Grid.Columns.Add(documentGridColumn);
		if (showTotal)
		{
			Grid.TotalSummary.Add(new GridSummaryItem
			{
				SummaryType = SummaryItemType.Sum,
				FieldName = propertyName,
				DisplayFormat = "{0:" + calcEditSettings.Mask + "}"
			});
			View.ShowTotalSummary = true;
		}
		documentGridColumn.DropDownEnabled = true;
		documentGridColumn.IsOwnProcessingClickDropDown = isOwnProcessingClickDropDown;
		return documentGridColumn;
	}

	public DocumentGridColumn AddRowIndicator(short width = 35)
	{
		DocumentGridColumn documentGridColumn = CreateGridColumn("RowIndicator", ArmSoft.SME.Common.Resources.RowNumber, trimValue: false);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.UnboundType = UnboundColumnType.Integer;
		documentGridColumn.Width = width;
		Grid.Columns.Add(documentGridColumn);
		_hasRowIndicator = true;
		return documentGridColumn;
	}

	public DocumentGridColumn AddDateColumn(FieldDefinition fieldDefinition, bool useLongDate = false)
	{
		return AddDateColumn(fieldDefinition.Name, fieldDefinition.Caption, useLongDate);
	}

	public DocumentGridColumn AddDateColumn(string propertyName, string caption, bool useLongDate = false)
	{
		DocumentGridColumn documentGridColumn = CreateGridColumn(propertyName, caption, trimValue: false);
		if (useLongDate)
		{
			documentGridColumn.CellTemplate = (DataTemplate)Resources["DateLongEditTemplate"];
		}
		else
		{
			documentGridColumn.CellTemplate = (DataTemplate)Resources["DateEditTemplate"];
		}
		documentGridColumn.Width = SizeCalculator.WidthForDate(useLongDate);
		Grid.Columns.Add(documentGridColumn);
		return documentGridColumn;
	}

	public DocumentGridColumn AddBooleanColumn(FieldDefinition fieldDefinition)
	{
		return AddBooleanColumn(fieldDefinition.Name, fieldDefinition.Caption);
	}

	public DocumentGridColumn AddBooleanColumn(string propertyName, string caption)
	{
		DocumentGridColumn documentGridColumn = CreateGridColumn(propertyName, caption, trimValue: false);
		CheckEditSettings editSettings = new CheckEditSettings
		{
			AllowNullInput = false
		};
		documentGridColumn.EditSettings = editSettings;
		if (Settings.IsWin11Theme)
		{
			documentGridColumn.CellTemplate = (DataTemplate)Settings.GenericResources["GridBooleanCellWin11Template"];
		}
		else
		{
			documentGridColumn.CellTemplate = (DataTemplate)Settings.GenericResources["GridBooleanCellTemplate"];
		}
		Grid.Columns.Add(documentGridColumn);
		return documentGridColumn;
	}

	public DocumentGridColumn AddTreeDropDownColumn(FieldDefinition fieldDefinition, bool allowAdditionButtons = false, bool isOwnProcessingClickDropDown = false)
	{
		TreeNodeFieldDefinition treeNodeFieldDefinition = (TreeNodeFieldDefinition)fieldDefinition;
		return AddDropDownColumn(treeNodeFieldDefinition.Name, treeNodeFieldDefinition.Caption, new TreeNodes(treeNodeFieldDefinition.TreeId, "", showAsTree: true, showExpanded: true, allowOperationWithAdditionButtons: true, treeNodeFieldDefinition.OnlyLeaf), allowAdditionButtons, isOwnProcessingClickDropDown);
	}

	public DocumentGridColumn AddSmartEnumDropDownColumn(FieldDefinition fieldDefinition, bool allowAdditionButtons = false, bool isOwnProcessingClickDropDown = false)
	{
		SmartEnumFieldDefinition smartEnumFieldDefinition = (SmartEnumFieldDefinition)fieldDefinition;
		return AddDropDownColumn(smartEnumFieldDefinition.Name, smartEnumFieldDefinition.Caption, new SmartEnum(smartEnumFieldDefinition.SmartEnumType, smartEnumFieldDefinition.Values, padLeft: false, '0', allSubTypes: false, smartEnumFieldDefinition.ValueMember), allowAdditionButtons, isOwnProcessingClickDropDown);
	}

	public DocumentGridColumn AddDropDownColumn(FieldDefinition fieldDefinition, IDropDownView idv, bool allowAdditionButtons = false, bool isOwnProcessingClickDropDown = false, bool clearDefaultButton = false)
	{
		return AddDropDownColumn(fieldDefinition.Name, fieldDefinition.Caption, idv, allowAdditionButtons, isOwnProcessingClickDropDown, clearDefaultButton);
	}

	public DocumentGridColumn AddDropDownColumn(string propertyName, string caption, IDropDownView idv, bool allowAdditionButtons = false, bool isOwnProcessingClickDropDown = false, bool clearDefaultButton = false)
	{
		DocumentGridColumn documentGridColumn = CreateGridColumn(propertyName, caption, trimValue: true);
		documentGridColumn.DropDownEnabled = !clearDefaultButton;
		documentGridColumn.AllowAdditionButtons = allowAdditionButtons;
		documentGridColumn.DataView = idv;
		documentGridColumn.AutoFilterCondition = AutoFilterCondition.Contains;
		documentGridColumn.IsOwnProcessingClickDropDown = isOwnProcessingClickDropDown;
		ButtonEditSettings buttonEditSettings = new ButtonEditSettings
		{
			MaskType = MaskType.None,
			MaxLength = idv.ValueMemberLength(),
			AllowNullInput = false,
			AllowDefaultButton = false
		};
		if (!clearDefaultButton)
		{
			ButtonInfo buttonInfo = new ButtonInfo
			{
				GlyphKind = GlyphKind.DropDown
			};
			buttonInfo.Click += new SafeEventHandler<RoutedEventArgs>(ColumnDropDownClick).Handler;
			buttonEditSettings.Buttons.Add(buttonInfo);
			if (idv.AddNewSupported && documentGridColumn.AllowAdditionButtons)
			{
				ButtonInfo buttonInfo2 = new ButtonInfo
				{
					GlyphKind = GlyphKind.Plus,
					ToolTip = ArmSoft.SME.Common.Resources.NewAdd + " " + HotKeysSE.Ctrl_N.CaptionWithBrackets
				};
				buttonInfo2.Click += new SafeEventHandler<RoutedEventArgs>(ColumnDropDownAddNew).Handler;
				buttonEditSettings.Buttons.Add(buttonInfo2);
			}
			if (idv.EditSupported && documentGridColumn.AllowAdditionButtons)
			{
				ButtonInfo buttonInfo3 = new ButtonInfo
				{
					GlyphKind = GlyphKind.Edit,
					ToolTip = ArmSoft.SME.Common.Resources.Edit + " " + HotKeysSE.Ctrl_E.CaptionWithBrackets
				};
				buttonInfo3.Click += new SafeEventHandler<RoutedEventArgs>(ColumnDropDownEdit).Handler;
				buttonEditSettings.Buttons.Add(buttonInfo3);
			}
		}
		documentGridColumn.EditSettings = buttonEditSettings;
		if (HasRowDeletedColumn)
		{
			documentGridColumn.DisplayTemplate = (ControlTemplate)Resources["TextColumnTemplate"];
		}
		Grid.Columns.Add(documentGridColumn);
		return documentGridColumn;
	}

	public DocumentGridColumn AddDropDownColumn(FieldDefinition fieldDefinition, bool trimValue = true, GlyphKind glyphKind = GlyphKind.DropDown, bool allowAdditionButtons = false, bool isOwnProcessingClickDropDown = false)
	{
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)fieldDefinition;
		return AddDropDownColumn(stringFieldDefinition.Name, stringFieldDefinition.Caption, stringFieldDefinition.Length, trimValue, glyphKind, allowAdditionButtons, isOwnProcessingClickDropDown);
	}

	public DocumentGridColumn AddDropDownColumn(string propertyName, string caption, short length, bool trimValue = true, GlyphKind glyphKind = GlyphKind.DropDown, bool allowAdditionButtons = false, bool isOwnProcessingClickDropDown = false)
	{
		DocumentGridColumn documentGridColumn = CreateGridColumn(propertyName, caption, trimValue);
		documentGridColumn.DropDownEnabled = true;
		documentGridColumn.AutoFilterCondition = AutoFilterCondition.Contains;
		documentGridColumn.AllowAdditionButtons = allowAdditionButtons;
		documentGridColumn.IsOwnProcessingClickDropDown = isOwnProcessingClickDropDown;
		ButtonEditSettings buttonEditSettings = new ButtonEditSettings
		{
			MaskType = MaskType.None,
			MaxLength = length,
			AllowNullInput = false,
			AllowDefaultButton = false
		};
		ButtonInfo buttonInfo = new ButtonInfo
		{
			GlyphKind = glyphKind
		};
		buttonInfo.Click += new SafeEventHandler<RoutedEventArgs>(ColumnDropDownClick).Handler;
		buttonEditSettings.Buttons.Add(buttonInfo);
		if (documentGridColumn.AllowAdditionButtons)
		{
			ButtonInfo buttonInfo2 = new ButtonInfo
			{
				GlyphKind = GlyphKind.Plus,
				ToolTip = ArmSoft.SME.Common.Resources.NewAdd + " " + HotKeysSE.Ctrl_N.CaptionWithBrackets
			};
			buttonInfo2.Click += new SafeEventHandler<RoutedEventArgs>(ColumnDropDownAddNew).Handler;
			buttonEditSettings.Buttons.Add(buttonInfo2);
		}
		if (documentGridColumn.AllowAdditionButtons)
		{
			ButtonInfo buttonInfo3 = new ButtonInfo
			{
				GlyphKind = GlyphKind.Edit,
				ToolTip = ArmSoft.SME.Common.Resources.Edit + " " + HotKeysSE.Ctrl_E.CaptionWithBrackets
			};
			buttonInfo3.Click += new SafeEventHandler<RoutedEventArgs>(ColumnDropDownEdit).Handler;
			buttonEditSettings.Buttons.Add(buttonInfo3);
		}
		documentGridColumn.EditSettings = buttonEditSettings;
		documentGridColumn.Width = SizeCalculator.WidthForString(length);
		if (HasRowDeletedColumn)
		{
			documentGridColumn.DisplayTemplate = (ControlTemplate)Resources["TextColumnTemplate"];
		}
		Grid.Columns.Add(documentGridColumn);
		return documentGridColumn;
	}

	public DocumentGridColumn AddDropDownCommentColumn(FieldDefinition fieldDefinition, IDropDownView idv, short length = 50, bool trimValue = true, GlyphKind glyphKind = GlyphKind.DropDown)
	{
		return AddDropDownCommentColumn(fieldDefinition.Name, fieldDefinition.Caption, idv, length, trimValue, glyphKind);
	}

	public DocumentGridColumn AddDropDownCommentColumn(string propertyName, string caption, IDropDownView idv, short length = 50, bool trimValue = true, GlyphKind glyphKind = GlyphKind.DropDown)
	{
		DocumentGridColumn documentGridColumn = CreateGridColumn(propertyName, caption, trimValue);
		documentGridColumn.DataView = idv;
		documentGridColumn.DropDownEnabled = true;
		documentGridColumn.IsCommentClickDropDown = true;
		documentGridColumn.AutoFilterCondition = AutoFilterCondition.Contains;
		ButtonEditSettings buttonEditSettings = new ButtonEditSettings
		{
			MaskType = MaskType.None,
			MaxLength = length,
			AllowNullInput = false,
			AllowDefaultButton = false
		};
		ButtonInfo buttonInfo = new ButtonInfo
		{
			GlyphKind = glyphKind
		};
		buttonInfo.Click += new SafeEventHandler<RoutedEventArgs>(CommentColumnDropDownClick).Handler;
		buttonEditSettings.Buttons.Add(buttonInfo);
		documentGridColumn.EditSettings = buttonEditSettings;
		documentGridColumn.Width = SizeCalculator.WidthForString((length < 50) ? length : 50);
		if (HasRowDeletedColumn)
		{
			documentGridColumn.DisplayTemplate = (ControlTemplate)Resources["TextColumnTemplate"];
		}
		Grid.Columns.Add(documentGridColumn);
		return documentGridColumn;
	}

	public BarEditItemExt AddButton(string imageURI = "", string caption = "", string tooltip = "", double width = 0.0)
	{
		BarEditItemExt barEditItemExt = new BarEditItemExt();
		barEditItemExt.AddButton(imageURI, caption, tooltip, width);
		_addedButtons.Add(barEditItemExt);
		ButtonsToolbar.Items.Add(barEditItemExt);
		ToolTipService.SetShowOnDisabled((DependencyObject)(object)barEditItemExt, value: true);
		barEditItemExt.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(ButtonPreviewKeyDown).Handler;
		return barEditItemExt;
	}

	public BarEditItemExt AddSplitButton(string imageURI = "", string caption = "", string tooltip = "", bool isTabStop = false, double imgWidth = 16.0, double imgHeight = 16.0, IEnumerable<IBarItem> popupItems = null, bool showGlyphsInPopupMenu = true, double width = 0.0)
	{
		BarEditItemExt barEditItemExt = new BarEditItemExt();
		barEditItemExt.AddSplitButton(imageURI, caption, tooltip, isTabStop, imgWidth, imgHeight, popupItems, showGlyphsInPopupMenu, width);
		_addedButtons.Add(barEditItemExt);
		ButtonsToolbar.Items.Add(barEditItemExt);
		ToolTipService.SetShowOnDisabled((DependencyObject)(object)barEditItemExt, value: true);
		barEditItemExt.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(ButtonPreviewKeyDown).Handler;
		return barEditItemExt;
	}

	public BarEditItemExt AddDropDown(LayoutItemWithRequired sp, DropDownControl dropDown, string caption = "", string tooltip = "")
	{
		BarEditItemExt barEditItemExt = new BarEditItemExt();
		barEditItemExt.AddDropDown(sp, dropDown, caption, tooltip);
		_addedButtons.Add(barEditItemExt);
		ButtonsToolbar.Items.Add(barEditItemExt);
		ToolTipService.SetShowOnDisabled((DependencyObject)(object)barEditItemExt, value: true);
		return barEditItemExt;
	}

	public bool CalculateHasValidationError(bool validateWithoutRequired = false)
	{
		string errorMessage;
		return CalculateHasValidationError(out errorMessage, validateWithoutRequired);
	}

	public bool CalculateHasValidationError(out string errorMessage, bool validateWithoutRequired = false, bool validateAgain = false)
	{
		errorMessage = "";
		if (View.HasValidationError)
		{
			return true;
		}
		int num = ((Grid.ItemsSource is ICollection collection) ? collection.Count : 0);
		if (IsRequired && num == 0 && !validateWithoutRequired)
		{
			return true;
		}
		if (_isDocumentGridRowBound)
		{
			for (int i = 0; i < num; i++)
			{
				DocumentGridRow documentGridRow = (DocumentGridRow)Grid.GetRowByListIndex(i);
				if (documentGridRow != null)
				{
					if (validateAgain || !documentGridRow.HasValidationError.HasValue)
					{
						documentGridRow.ValidateWithoutResponse();
					}
					if (Convert.ToBoolean(documentGridRow.HasValidationError))
					{
						errorMessage = documentGridRow.ValidationErrorMessage;
						return true;
					}
				}
			}
		}
		return false;
	}

	public void ConfigureButtons()
	{
		if (IsOpenedInOfflineModeDoc)
		{
			ConfigureButtonsInOffineReadOnlyModeDoc();
		}
		else
		{
			ConfigureButtonsInOnlineModeDoc();
		}
	}

	private void ConfigureButtonsInOffineReadOnlyModeDoc()
	{
		ButtonAdd.IsEnabled = false;
		ButtonViewEdit.IsEnabled = false;
		ButtonDelete.IsEnabled = false;
		ButtonDuplicate.IsEnabled = false;
		ButtonInsert.IsVisible = false;
		ButtonMoveUp.IsVisible = false;
		ButtonMoveDown.IsVisible = false;
		ButtonInsert.IsEnabled = false;
		ButtonMoveUp.IsEnabled = false;
		ButtonMoveDown.IsEnabled = false;
		if (_addedButtons == null)
		{
			return;
		}
		foreach (BarEditItemExt addedButton in _addedButtons)
		{
			addedButton.IsEnabled = false;
		}
		RefreshButtons?.Invoke(this, new EventArgs());
	}

	private void ConfigureButtonsInOnlineModeDoc()
	{
		ButtonAdd.IsEnabled = !IsReadOnly && AllowAdd;
		ButtonViewEdit.ToolTip = ((AllowEdit && !IsReadOnly) ? ArmSoft.SME.Common.Resources.Edit : ArmSoft.SME.Common.Resources.View) + " " + HotKeysSE.Ctrl_E.CaptionWithBrackets;
		ButtonViewEdit.IsEnabled = IsVisibleViewEditButton && Grid.VisibleRowCount > 0;
		ButtonDelete.IsEnabled = !IsReadOnly && AllowDelete && Grid.VisibleRowCount > 0;
		ButtonDuplicate.IsEnabled = ButtonAdd.IsEnabled && AllowDuplicate && Grid.VisibleRowCount > 0;
		ButtonInsert.IsVisible = AllowAdd && AllowInsert && IsGridMode;
		ButtonMoveUp.IsVisible = AllowMoveUp;
		ButtonMoveDown.IsVisible = AllowMoveDown;
		ButtonInsert.IsEnabled = ButtonAdd.IsEnabled && AllowInsert && IsGridMode && Grid.VisibleRowCount > 0;
		ButtonInsert.ToolTip = ArmSoft.SME.Common.Resources.Insert + " " + HotKeysSE.Ctrl_I.CaptionWithBrackets + (AllowDuplicate ? (", " + HotKeysSE.Ctrl_Alt_I.CaptionWithBrackets) : "");
		ButtonMoveUp.IsEnabled = !IsReadOnly && AllowMoveUp && Grid.VisibleRowCount > 0;
		ButtonMoveDown.IsEnabled = !IsReadOnly && AllowMoveDown && Grid.VisibleRowCount > 0;
		if (_addedButtons == null)
		{
			return;
		}
		foreach (BarEditItemExt addedButton in _addedButtons)
		{
			addedButton.IsEnabled = !IsReadOnly;
		}
		RefreshButtons?.Invoke(this, new EventArgs());
	}

	private void CheckHasValidationError()
	{
		bool flag = CalculateHasValidationError();
		if (HasValidationError != flag)
		{
			SetValidationErrorProperty(flag);
		}
	}

	private void SetValidationErrorProperty(bool value)
	{
		((DependencyObject)this).SetValue(HasValidationErrorProperty, (object)value);
	}

	private DocumentGridColumn CreateGridColumn(string propertyName, string caption, bool trimValue)
	{
		DocumentGridColumn documentGridColumn = new DocumentGridColumn
		{
			Name = propertyName,
			FieldName = propertyName,
			Header = caption,
			Visible = true,
			TrimValue = trimValue,
			AllowEditing = DefaultBoolean.Default,
			AllowColumnFiltering = DefaultBoolean.False,
			AllowSorting = DefaultBoolean.False
		};
		if (_showColHeaderIn2Lines)
		{
			documentGridColumn.HeaderTemplate = (DataTemplate)Resources["Column2RowHeaderTemplate"];
		}
		return documentGridColumn;
	}

	private void DeleteCurrentRow()
	{
		if (IsReadOnly || !AllowDelete)
		{
			return;
		}
		View.CommitEditing();
		View.HideEditor();
		int focusedRowHandle = View.FocusedRowHandle;
		if (CommonEventHandlers.IsDataRow(focusedRowHandle, Grid))
		{
			DocumentGridDeletingRowEventArgs e = new DocumentGridDeletingRowEventArgs(this, focusedRowHandle);
			DeletingRow?.Invoke(this, e);
			if (!e.Cancel)
			{
				View.DeleteRow(focusedRowHandle);
				DeletedRow?.Invoke(this, new EventArgs());
			}
			if (_hasRowIndicator)
			{
				Grid.RefreshData();
			}
		}
	}

	private void AddRow()
	{
		if (IsReadOnly || !AllowAdd)
		{
			return;
		}
		Grid.Focus();
		DocumentGridAddingRowEventArgs e = new DocumentGridAddingRowEventArgs(this);
		if (IsGridMode)
		{
			AddingRow?.Invoke(this, e);
			if (e.RowNumber != -1)
			{
				InnerAddRow(e);
			}
		}
		else
		{
			AddingRowWithDialog?.Invoke(this, e);
		}
	}

	public void InnerAddRow(DocumentGridAddingRowEventArgs args)
	{
		int rowHandleByVisibleIndex = Grid.GetRowHandleByVisibleIndex(args.RowNumber);
		View.MoveFocusedRow(Grid.GetRowVisibleIndexByHandle(rowHandleByVisibleIndex));
		if (_isDocumentGridRowBound)
		{
			((DocumentGridRow)Grid.GetRow(rowHandleByVisibleIndex))?.ValidateWithoutResponse();
		}
		if (args.FocusFirstColumn)
		{
			View.Grid.CurrentColumn = View.VisibleColumns[_hasRowIndicator ? 1 : 0];
			((DispatcherObject)this).Dispatcher.Invoke((Action)MyShowEditor, (DispatcherPriority)6);
		}
		else
		{
			View.Grid.CurrentColumn = View.VisibleColumns[args.FocusColumnIndex];
			((DispatcherObject)this).Dispatcher.Invoke((Action)MyShowEditor, (DispatcherPriority)6);
		}
	}

	private void MoveRowUp()
	{
		if (IsReadOnly || !AllowMoveUp)
		{
			return;
		}
		Grid.Focus();
		View.CommitEditing();
		View.HideEditor();
		int focusedRowHandle = View.FocusedRowHandle;
		if (CommonEventHandlers.IsDataRow(focusedRowHandle, Grid) && focusedRowHandle > 0)
		{
			DocumentGridMovingRowUpOrDownEventArgs e = new DocumentGridMovingRowUpOrDownEventArgs(this, focusedRowHandle);
			MovingRowUp?.Invoke(this, e);
			if (_hasRowIndicator)
			{
				Grid.RefreshRow(focusedRowHandle);
			}
			View.FocusedRowHandle = focusedRowHandle - 1;
			View.Grid.SelectedItem = View.Grid.CurrentItem;
		}
	}

	private void MoveRowDown()
	{
		if (IsReadOnly || !AllowMoveDown)
		{
			return;
		}
		Grid.Focus();
		View.CommitEditing();
		View.HideEditor();
		int focusedRowHandle = View.FocusedRowHandle;
		if (CommonEventHandlers.IsDataRow(focusedRowHandle, Grid) && focusedRowHandle < Grid.VisibleRowCount - 1)
		{
			DocumentGridMovingRowUpOrDownEventArgs e = new DocumentGridMovingRowUpOrDownEventArgs(this, focusedRowHandle);
			MovingRowDown?.Invoke(this, e);
			if (_hasRowIndicator)
			{
				Grid.RefreshRow(focusedRowHandle);
			}
			View.FocusedRowHandle = focusedRowHandle + 1;
			View.Grid.SelectedItem = View.Grid.CurrentItem;
		}
	}

	private void EditRowWithDialog()
	{
		if (!IsVisibleViewEditButton)
		{
			return;
		}
		Grid.Focus();
		View.CommitEditing();
		View.HideEditor();
		int focusedRowHandle = View.FocusedRowHandle;
		if (!CommonEventHandlers.IsDataRow(focusedRowHandle, Grid))
		{
			return;
		}
		DocumentGridEditingRowEventArgs e = new DocumentGridEditingRowEventArgs(this, focusedRowHandle, AllowEdit && !IsReadOnly);
		EditingRowWithDialog?.Invoke(this, e);
		if (AllowEdit && !IsReadOnly && e.RowEdited)
		{
			if (_isDocumentGridRowBound)
			{
				((DocumentGridRow)Grid.GetRow(focusedRowHandle))?.ValidateWithoutResponse();
			}
			Grid.UpdateTotalSummary();
		}
	}

	private void DuplicateRow()
	{
		if (IsReadOnly || !AllowDuplicate)
		{
			return;
		}
		Grid.Focus();
		View.CommitEditing();
		View.HideEditor();
		int focusedRowHandle = View.FocusedRowHandle;
		if (!CommonEventHandlers.IsDataRow(focusedRowHandle, Grid))
		{
			return;
		}
		DocumentGridDuplicatingRowEventArgs e = new DocumentGridDuplicatingRowEventArgs(this, focusedRowHandle);
		if (IsGridMode)
		{
			DuplicatingRow?.Invoke(this, e);
			if (e.NewRowNumber != -1)
			{
				View.MoveFocusedRow(Grid.GetRowVisibleIndexByHandle(Grid.GetRowHandleByVisibleIndex(e.NewRowNumber)));
				Grid.UpdateTotalSummary();
			}
		}
		else
		{
			DuplicatingRowWithDialog?.Invoke(this, e);
		}
	}

	private void InsertRow(bool isDuplicate = false)
	{
		if (IsReadOnly || !AllowInsert || !IsGridMode)
		{
			return;
		}
		Grid.Focus();
		View.CommitEditing();
		View.HideEditor();
		int focusedRowHandle = View.FocusedRowHandle;
		if (CommonEventHandlers.IsDataRow(focusedRowHandle, Grid))
		{
			DocumentGridInsertingRowEventArgs e = new DocumentGridInsertingRowEventArgs(this, focusedRowHandle, isDuplicate);
			InsertingRow?.Invoke(this, e);
			View.FocusedRowHandle = e.RowHandle;
			View.Grid.SelectedItem = View.Grid.CurrentItem;
			if (e.IsDuplicate)
			{
				Grid.UpdateTotalSummary();
			}
			if (_isDocumentGridRowBound)
			{
				((DocumentGridRow)Grid.GetRow(e.RowHandle))?.ValidateWithoutResponse();
			}
			if (e.FocusFirstColumn)
			{
				View.Grid.CurrentColumn = View.VisibleColumns[_hasRowIndicator ? 1 : 0];
				((DispatcherObject)this).Dispatcher.Invoke((Action)MyShowEditor, (DispatcherPriority)6);
			}
			if (_hasRowIndicator)
			{
				Grid.RefreshData();
			}
		}
	}

	private void MyShowEditor()
	{
		View.ShowEditor();
		if (View.ActiveEditor != null)
		{
			View.ActiveEditor.SelectAll();
		}
	}

	private void RowChanged()
	{
		int focusedRowHandle = View.FocusedRowHandle;
		if (Visibility == Visibility.Visible)
		{
			string msg;
			if (Grid.VisibleRowCount > 0 && CommonEventHandlers.IsDataRow(focusedRowHandle, Grid))
			{
				msg = Grid.GetRowVisibleIndexByHandle(focusedRowHandle) + 1 + "/" + Grid.VisibleRowCount;
			}
			else
			{
				msg = ((Grid.VisibleRowCount != 0) ? ("0/" + Grid.VisibleRowCount) : "");
			}
			WriteToStatusBarForCurrentRow?.Invoke(this, msg);
		}
		CheckHasValidationError();
	}

	private void InnerClickDropDown(int rowHandle, DocumentGridColumn column, bool upClick = false)
	{
		if (BeforeClickDropDown != null)
		{
			BeforeClickDropDown(rowHandle, column);
		}
		if (((IsReadOnly || column.ReadOnly) && !column.IsRaiseClickDropDownEventInReadOnlyMode) || (column.AllowEditing == DefaultBoolean.False && !column.IsOwnProcessingClickDropDown))
		{
			return;
		}
		try
		{
			if (column.DataView == null || column.IsOwnProcessingClickDropDown)
			{
				View.CommitEditing();
				View.HideEditor();
				DocumentGridClickDropDownEventArgs e = new DocumentGridClickDropDownEventArgs
				{
					Grid = this,
					RowHandle = rowHandle,
					Column = column,
					Value = View.Grid.GetFocusedValue()?.ToString(),
					NewValue = null
				};
				double top = e.Top;
				double left = e.Left;
				GetCoordinatesForPopup(rowHandle, column, ref top, ref left);
				e.Top = top;
				e.Left = left;
				e.Owner = Owner;
				e.RowChangedDelegate = ColumnDropDownBrowserRowChanged;
				e.UpClick = upClick;
				ClickDropDown?.Invoke(this, e);
				if (e.NewValue != null)
				{
					View.Grid.SetFocusedRowCellValue(column, e.NewValue);
					View.CommitEditing();
					View.ShowEditor();
				}
			}
			else
			{
				ModalBrowser modalBrowser = new ModalBrowser(column.DataView);
				double top2 = 0.0;
				double left2 = 0.0;
				GetCoordinatesForPopup(rowHandle, column, ref top2, ref left2);
				modalBrowser.Top = top2;
				modalBrowser.Left = left2;
				View.CommitEditing();
				View.HideEditor();
				modalBrowser.LookUpValue = View.Grid.GetFocusedValue()?.ToString();
				modalBrowser.Owner = Owner;
				modalBrowser.SearchString = View.Grid.GetFocusedValue()?.ToString();
				if (modalBrowser.Show(upClick))
				{
					View.Grid.SetFocusedRowCellValue(column, column.DataView.ValueMember(modalBrowser.SelectedRowIndex));
					View.CommitEditing();
					View.ShowEditor();
				}
			}
			RowChanged();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void InnerCommentClickDropDown(int rowHandle, DocumentGridColumn column, bool upClick = false)
	{
		if (BeforeClickDropDown != null)
		{
			BeforeClickDropDown(rowHandle, column);
		}
		if (((IsReadOnly || column.ReadOnly) && !column.IsRaiseClickDropDownEventInReadOnlyMode) || (column.AllowEditing == DefaultBoolean.False && !column.IsOwnProcessingClickDropDown))
		{
			return;
		}
		try
		{
			if (column.DataView == null || column.IsOwnProcessingClickDropDown)
			{
				View.CommitEditing();
				View.HideEditor();
				DocumentGridClickDropDownEventArgs e = new DocumentGridClickDropDownEventArgs
				{
					Grid = this,
					RowHandle = rowHandle,
					Column = column,
					Value = View.Grid.GetFocusedValue()?.ToString(),
					NewValue = null,
					UpClick = upClick
				};
				double top = e.Top;
				double left = e.Left;
				GetCoordinatesForPopup(rowHandle, column, ref top, ref left);
				e.Top = top;
				e.Left = left;
				e.Owner = Owner;
				e.RowChangedDelegate = ColumnDropDownBrowserRowChanged;
				ClickDropDown?.Invoke(this, e);
				if (e.NewValue != null)
				{
					View.Grid.SetFocusedRowCellValue(column, e.NewValue);
					View.CommitEditing();
					View.ShowEditor();
				}
			}
			else
			{
				ModalBrowser modalBrowser = new ModalBrowser(column.DataView);
				double top2 = 0.0;
				double left2 = 0.0;
				GetCoordinatesForPopup(rowHandle, column, ref top2, ref left2);
				modalBrowser.Top = top2;
				modalBrowser.Left = left2;
				View.CommitEditing();
				View.HideEditor();
				modalBrowser.LookUpValue = "";
				modalBrowser.Owner = Owner;
				modalBrowser.SearchString = View.Grid.GetFocusedValue()?.ToString();
				if (modalBrowser.Show(upClick))
				{
					View.Grid.SetFocusedRowCellValue(column, column.DataView.CommentMember(modalBrowser.SelectedRowIndex));
					View.CommitEditing();
					View.ShowEditor();
				}
			}
			RowChanged();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddNewDropDown(int rowHandle, DocumentGridColumn column)
	{
		if (IsReadOnly || column.AllowEditing == DefaultBoolean.False || column.ReadOnly || column.DataView == null || !column.DataView.AddNewEnabled)
		{
			return;
		}
		try
		{
			View.CommitEditing();
			View.HideEditor();
			string value = "";
			string comment = "";
			double top = 0.0;
			double left = 0.0;
			GetCoordinatesForPopup(rowHandle, column, ref top, ref left);
			if (column.DataView.AddNew(ref value, ref comment, Owner, top, left))
			{
				View.Grid.SetFocusedRowCellValue(column, value);
				View.CommitEditing();
				View.ShowEditor();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void EditDropDown(int rowHandle, DocumentGridColumn column)
	{
		if (IsReadOnly || column.AllowEditing == DefaultBoolean.False || column.ReadOnly || column.DataView == null || !column.DataView.EditEnabled)
		{
			return;
		}
		try
		{
			View.CommitEditing();
			View.HideEditor();
			string value = View.Grid.GetFocusedValue()?.ToString();
			string comment = "";
			double top = 0.0;
			double left = 0.0;
			if (!string.IsNullOrWhiteSpace(value))
			{
				GetCoordinatesForPopup(rowHandle, column, ref top, ref left);
				if (column.DataView.Edit(ref value, ref comment, Owner, top, left, View.Grid.GetFocusedRow()))
				{
					View.Grid.SetFocusedRowCellValue(column, value);
					View.CommitEditing();
					View.ShowEditor();
					DocumentGridDropDownEditedEventArgs e = new DocumentGridDropDownEditedEventArgs
					{
						Grid = this,
						RowHandle = rowHandle,
						Column = column,
						Value = value,
						Comment = comment,
						Owner = Owner,
						RowChangedDelegate = ColumnDropDownBrowserRowChanged
					};
					DropDownEdited?.Invoke(this, e);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void GetCoordinatesForPopup(int rowHandle, DocumentGridColumn column, ref double top, ref double left)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		FrameworkElement cellElementByRowHandleAndColumn = View.GetCellElementByRowHandleAndColumn(rowHandle, column);
		if (cellElementByRowHandleAndColumn != null)
		{
			Point val = cellElementByRowHandleAndColumn.PointToScreen(new Point(0.0, 0.0));
			Matrix transformFromDevice = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
			Point val2 = transformFromDevice.Transform(val);
			top = val2.Y + cellElementByRowHandleAndColumn.ActualHeight;
			left = val2.X;
		}
	}

	private void Grid_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "VisibleRowCount")
		{
			RaiseEvent(new RoutedEventArgs(VisibleRowsCountChanged));
			RowChanged();
			ConfigureButtons();
		}
	}

	private void DocumentGridControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
	{
	}

	private void ColumnDropDownClick(object sender, RoutedEventArgs e)
	{
		try
		{
			int focusedRowHandle = View.FocusedRowHandle;
			DocumentGridColumn documentGridColumn = (DocumentGridColumn)Grid.CurrentColumn;
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action<int, DocumentGridColumn, bool>(InnerClickDropDown), new object[3] { focusedRowHandle, documentGridColumn, false });
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CommentColumnDropDownClick(object sender, RoutedEventArgs e)
	{
		try
		{
			int focusedRowHandle = View.FocusedRowHandle;
			DocumentGridColumn documentGridColumn = (DocumentGridColumn)Grid.CurrentColumn;
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action<int, DocumentGridColumn, bool>(InnerCommentClickDropDown), new object[3] { focusedRowHandle, documentGridColumn, false });
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ColumnDropDownBrowserRowChanged(int visibleRowNumber, int totalVisibleRows)
	{
		string msg = visibleRowNumber + "/" + totalVisibleRows;
		if (Visibility == Visibility.Visible)
		{
			WriteToStatusBarForCurrentRow?.Invoke(this, msg);
		}
	}

	private void ColumnDropDownAddNew(object sender, RoutedEventArgs e)
	{
		try
		{
			int focusedRowHandle = View.FocusedRowHandle;
			DocumentGridColumn documentGridColumn = (DocumentGridColumn)Grid.CurrentColumn;
			DocumentGridClickDropDownEventArgs e2 = new DocumentGridClickDropDownEventArgs
			{
				Grid = this,
				RowHandle = focusedRowHandle,
				Column = documentGridColumn,
				Value = View.Grid.GetFocusedValue()?.ToString(),
				NewValue = null
			};
			double top = e2.Top;
			double left = e2.Left;
			GetCoordinatesForPopup(focusedRowHandle, documentGridColumn, ref top, ref left);
			e2.Top = top;
			e2.Left = left;
			e2.Owner = Owner;
			e2.RowChangedDelegate = ColumnDropDownBrowserRowChanged;
			BeforeAddNewDropDown?.Invoke(sender, e2);
			if (e2.RowHandle != -1)
			{
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action<int, DocumentGridColumn>(AddNewDropDown), new object[2] { focusedRowHandle, documentGridColumn });
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ColumnDropDownEdit(object sender, RoutedEventArgs e)
	{
		try
		{
			int focusedRowHandle = View.FocusedRowHandle;
			DocumentGridColumn documentGridColumn = (DocumentGridColumn)Grid.CurrentColumn;
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action<int, DocumentGridColumn>(EditDropDown), new object[2] { focusedRowHandle, documentGridColumn });
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void View_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		DocumentGridColumn documentGridColumn = (DocumentGridColumn)e.Column;
		if (documentGridColumn.TrimValue)
		{
			string text = (string)e.Value;
			if (text == null)
			{
				e.Handled = true;
				InnerGrid.SetCellValue(e.RowHandle, e.Column, " ");
			}
			else if (text != text.Trim())
			{
				e.Handled = true;
				InnerGrid.SetCellValue(e.RowHandle, e.Column, text.Trim());
			}
		}
		else if (e.Value == null && documentGridColumn.EditSettings is TextEditSettings)
		{
			e.Handled = true;
			InnerGrid.SetCellValue(e.RowHandle, e.Column, " ");
			InnerGrid.SetCellValue(e.RowHandle, e.Column, "");
		}
	}

	private void View_CellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		if (e.Column.EditSettings is CheckEditSettings)
		{
			View.CommitEditing();
		}
	}

	private void InnerView_ValidateCell(object sender, GridCellValidationEventArgs e)
	{
		if (InnerView.ActiveEditor is DateEditControl dateEditControl)
		{
			e.Handled = true;
			string obj = (dateEditControl.UseLongDate ? "  /  /    " : "  /  /  ");
			if (e.Value.Equals(obj))
			{
				bool suppressValidation = dateEditControl.SuppressValidation;
				dateEditControl.SuppressValidation = true;
				dateEditControl.ClearBaseEditValue();
				dateEditControl.SuppressValidation = suppressValidation;
			}
		}
	}

	private void View_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		RowChanged();
	}

	private void View_GotFocus(object sender, RoutedEventArgs e)
	{
		RowChanged();
	}

	private void View_LostFocus(object sender, RoutedEventArgs e)
	{
		WriteToStatusBarForCurrentRow?.Invoke(this, "");
	}

	private void View_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Invalid comparison between Unknown and I4
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Invalid comparison between Unknown and I4
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Invalid comparison between Unknown and I4
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Invalid comparison between Unknown and I4
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Invalid comparison between Unknown and I4
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Invalid comparison between Unknown and I4
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Invalid comparison between Unknown and I4
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Invalid comparison between Unknown and I4
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Invalid comparison between Unknown and I4
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Invalid comparison between Unknown and I4
		if ((int)e.Key == 47 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			DeleteCurrentRow();
		}
		else if ((int)e.Key == 31)
		{
			e.Handled = true;
			AddRow();
		}
		else if ((int)e.Key == 32 && !View.IsEditing)
		{
			e.Handled = true;
			DeleteCurrentRow();
		}
		else if ((int)e.Key == 57 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			AddRow();
		}
		else if ((int)e.Key == 48 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			EditRowWithDialog();
		}
		else if ((int)e.Key == 68 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			DuplicateRow();
		}
		else if ((int)e.Key == 52 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			InsertRow();
		}
		else if ((int)e.Key == 52 && (int)Keyboard.Modifiers == 3 && AllowDuplicate)
		{
			e.Handled = true;
			InsertRow(AllowDuplicate);
		}
	}

	private void ButtonAdd_Click(object sender, RoutedEventArgs e)
	{
		AddRow();
	}

	private void ButtonDuplicate_Click(object sender, RoutedEventArgs e)
	{
		DuplicateRow();
	}

	private void ButtonInsert_Click(object sender, RoutedEventArgs e)
	{
		InsertRow();
	}

	private void ButtonViewEdit_Click(object sender, RoutedEventArgs e)
	{
		EditRowWithDialog();
	}

	private void ButtonDelete_Click(object sender, RoutedEventArgs e)
	{
		DeleteCurrentRow();
	}

	private void ButtonMoveUp_Click(object sender, RoutedEventArgs e)
	{
		MoveRowUp();
	}

	private void ButtonMoveDown_Click(object sender, RoutedEventArgs e)
	{
		MoveRowDown();
	}

	private void View_HiddenEditor(object sender, EditorEventArgs e)
	{
		View.CommitEditing();
	}

	private void View_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Invalid comparison between Unknown and I4
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Invalid comparison between Unknown and I4
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Invalid comparison between Unknown and I4
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Invalid comparison between Unknown and I4
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Invalid comparison between Unknown and I4
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Invalid comparison between Unknown and I4
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Invalid comparison between Unknown and I4
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Invalid comparison between Unknown and I4
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Invalid comparison between Unknown and I4
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Invalid comparison between Unknown and I4
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Invalid comparison between Unknown and I4
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Invalid comparison between Unknown and I4
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Invalid comparison between Unknown and I4
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Invalid comparison between Unknown and I4
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Invalid comparison between Unknown and I4
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Invalid comparison between Unknown and I4
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Invalid comparison between Unknown and I4
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Invalid comparison between Unknown and I4
		if ((int)e.Key == 26 && (int)Keyboard.Modifiers == 2)
		{
			DocumentGridColumn documentGridColumn = (DocumentGridColumn)View.Grid.CurrentColumn;
			if (documentGridColumn == null || !documentGridColumn.DropDownEnabled)
			{
				return;
			}
			int focusedRowHandle = View.FocusedRowHandle;
			if (View.ActiveEditor is PopupCalcEdit && !documentGridColumn.IsOwnProcessingClickDropDown)
			{
				((PopupCalcEdit)View.ActiveEditor).ShowPopup();
				e.Handled = true;
			}
			else if (CommonEventHandlers.IsDataRow(focusedRowHandle, Grid))
			{
				if (documentGridColumn.IsCommentClickDropDown)
				{
					InnerCommentClickDropDown(focusedRowHandle, documentGridColumn);
				}
				else
				{
					InnerClickDropDown(focusedRowHandle, documentGridColumn);
				}
				e.Handled = true;
			}
		}
		else if ((int)e.Key == 24 && (int)Keyboard.Modifiers == 2)
		{
			DocumentGridColumn documentGridColumn2 = (DocumentGridColumn)View.Grid.CurrentColumn;
			if (documentGridColumn2 == null || !documentGridColumn2.DropDownEnabled)
			{
				return;
			}
			int focusedRowHandle2 = View.FocusedRowHandle;
			if (View.ActiveEditor is PopupCalcEdit && !documentGridColumn2.IsOwnProcessingClickDropDown)
			{
				((PopupCalcEdit)View.ActiveEditor).ShowPopup();
				e.Handled = true;
			}
			else if (CommonEventHandlers.IsDataRow(focusedRowHandle2, Grid))
			{
				if (documentGridColumn2.IsCommentClickDropDown)
				{
					InnerCommentClickDropDown(focusedRowHandle2, documentGridColumn2, upClick: true);
				}
				else
				{
					InnerClickDropDown(focusedRowHandle2, documentGridColumn2, upClick: true);
				}
				e.Handled = true;
			}
		}
		else if (((int)e.Key == 48 || (int)e.Key == 57) && (int)Keyboard.Modifiers == 6)
		{
			DocumentGridColumn documentGridColumn3 = (DocumentGridColumn)View.Grid.CurrentColumn;
			if (documentGridColumn3 == null || documentGridColumn3.DataView == null)
			{
				return;
			}
			int focusedRowHandle3 = View.FocusedRowHandle;
			if (!CommonEventHandlers.IsDataRow(focusedRowHandle3, Grid))
			{
				return;
			}
			if ((int)e.Key == 48 && documentGridColumn3.AllowAdditionButtons && documentGridColumn3.DataView.EditSupported)
			{
				EditDropDown(focusedRowHandle3, documentGridColumn3);
				e.Handled = true;
			}
			else if ((int)e.Key == 57 && documentGridColumn3.AllowAdditionButtons && documentGridColumn3.DataView.AddNewSupported)
			{
				DocumentGridClickDropDownEventArgs e2 = new DocumentGridClickDropDownEventArgs
				{
					Grid = this,
					RowHandle = focusedRowHandle3,
					Column = documentGridColumn3,
					Value = View.Grid.GetFocusedValue()?.ToString(),
					NewValue = null
				};
				double top = e2.Top;
				double left = e2.Left;
				GetCoordinatesForPopup(focusedRowHandle3, documentGridColumn3, ref top, ref left);
				e2.Top = top;
				e2.Left = left;
				e2.Owner = Owner;
				e2.RowChangedDelegate = ColumnDropDownBrowserRowChanged;
				BeforeAddNewDropDown?.Invoke(sender, e2);
				if (e2.RowHandle != -1)
				{
					AddNewDropDown(focusedRowHandle3, documentGridColumn3);
				}
				e.Handled = true;
			}
		}
		else if ((int)Keyboard.Modifiers == 0 && (int)e.Key == 13 && !View.IsEditing)
		{
			e.Handled = true;
			EscapePressed?.Invoke(this, new EventArgs());
		}
		else if ((int)e.Key == 57 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			AddRow();
		}
		else if ((int)e.Key == 48 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			EditRowWithDialog();
		}
		else if ((int)e.Key == 68 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			DuplicateRow();
		}
		else if ((int)e.Key == 52 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			InsertRow();
		}
		else if ((int)e.Key == 52 && (int)Keyboard.Modifiers == 3 && AllowDuplicate)
		{
			e.Handled = true;
			InsertRow(AllowDuplicate);
		}
	}

	private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			View.CommitEditing();
			ControlEnterPressed?.Invoke(this, new EventArgs());
		}
		else if ((int)e.Key == 6)
		{
			View.UnselectCell(View.FocusedRowHandle, (GridColumn)Grid.CurrentColumn);
		}
	}

	private void ButtonPreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			ControlEnterPressed?.Invoke(this, new EventArgs());
		}
	}

	public DocumentGridControl()
		: this(false)
	{
	}

	public DocumentGridControl(bool isSupportDialogMode = false)
	{
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Expected Obj, but got Unknown
		InitializeComponent();
		InnerGrid.ClipboardCopyMode = ClipboardCopyMode.ExcludeHeader;
		IsRequired = false;
		_addedButtons = new List<BarEditItemExt>();
		InnerView.RowStyle = (Style)Resources["RowStyle"];
		SetValidationErrorProperty(value: false);
		DataControlBase.AllowInfiniteGridSize = true;
		ButtonAdd.ToolTip = ArmSoft.SME.Common.Resources.NewAdd + " " + HotKeysSE.Ctrl_N.CaptionWithBrackets;
		ButtonAdd.ItemClick += new SafeEventHandler<RoutedEventArgs>(ButtonAdd_Click).Handler;
		ButtonViewEdit.IsVisible = IsVisibleViewEditButton;
		ButtonViewEdit.ToolTip = ArmSoft.SME.Common.Resources.Edit + " " + HotKeysSE.Ctrl_E.CaptionWithBrackets;
		ButtonViewEdit.ItemClick += new SafeEventHandler<RoutedEventArgs>(ButtonViewEdit_Click).Handler;
		ButtonViewEdit.IsVisible = isSupportDialogMode;
		ButtonDelete.ToolTip = ArmSoft.SME.Common.Resources.Delete + " " + HotKeysSE.Ctrl_D.CaptionWithBrackets;
		ButtonDelete.ItemClick += new SafeEventHandler<RoutedEventArgs>(ButtonDelete_Click).Handler;
		ButtonDuplicate.ToolTip = ArmSoft.SME.Common.Resources.Copy + " " + HotKeysSE.Ctrl_Y.CaptionWithBrackets;
		ButtonDuplicate.ItemClick += new SafeEventHandler<RoutedEventArgs>(ButtonDuplicate_Click).Handler;
		ButtonMoveUp.ToolTip = ArmSoft.SME.Common.Resources.Up;
		ButtonMoveUp.ItemClick += new SafeEventHandler<RoutedEventArgs>(ButtonMoveUp_Click).Handler;
		ButtonMoveDown.ToolTip = ArmSoft.SME.Common.Resources.Down;
		ButtonMoveDown.ItemClick += new SafeEventHandler<RoutedEventArgs>(ButtonMoveDown_Click).Handler;
		InnerGrid.PropertyChanged += new SafeEventHandler<PropertyChangedEventArgs>(Grid_PropertyChanged).Handler;
		PreviewMouseWheel += new SafeEventHandler<MouseWheelEventArgs>(DocumentGridControl_PreviewMouseWheel).Handler;
		InnerView.CellValueChanged += new SafeEventHandler<CellValueChangedEventArgs>(View_CellValueChanged).Handler;
		InnerView.CellValueChanging += new SafeEventHandler<CellValueChangedEventArgs>(View_CellValueChanging).Handler;
		InnerView.ValidateCell += new SafeEventHandler<GridCellValidationEventArgs>(InnerView_ValidateCell).Handler;
		InnerView.FocusedRowHandleChanged += new SafeEventHandler<FocusedRowHandleChangedEventArgs>(View_FocusedRowHandleChanged).Handler;
		InnerView.GotFocus += new SafeEventHandler<RoutedEventArgs>(View_GotFocus).Handler;
		InnerView.LostFocus += new SafeEventHandler<RoutedEventArgs>(View_LostFocus).Handler;
		InnerView.KeyDown += new SafeEventHandler<KeyEventArgs>(View_KeyDown).Handler;
		ButtonInsert.ItemClick += new SafeEventHandler<RoutedEventArgs>(ButtonInsert_Click).Handler;
		InnerView.HiddenEditor += new SafeEventHandler<EditorEventArgs>(View_HiddenEditor).Handler;
		InnerView.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(View_PreviewKeyDown).Handler;
		InnerGrid.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(Grid_PreviewKeyDown).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(ButtonPreviewKeyDown).Handler;
		InnerView.ShowGridMenu += new SafeEventHandler<GridMenuEventArgs>(ShowGridMenu).Handler;
		IsVisibleChanged += new SafeEventHandler<DependencyPropertyChangedEventArgs>(DocumentGridControl_IsVisibleChanged).Handler;
		InnerGrid.CustomUnboundColumnData += new SafeEventHandler<GridColumnDataEventArgs>(Grid_CustomUnboundColumnData).Handler;
		ConfigureButtons();
		InnerView.UseLightweightTemplates = UseLightweightTemplates.All;
		InnerView.SearchPanelPosition = SearchPanelPosition.OverGroupPanel;
	}

	private void ShowGridMenu(object sender, GridMenuEventArgs e)
	{
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		if (e.MenuType == GridMenuType.Column)
		{
			for (int i = 0; i <= e.Items.Count - 1; i++)
			{
				if (((TableView)e.OriginalSource).Grid.CurrentColumn == null)
				{
					continue;
				}
				if (e.Items[i].Name == "ItemGroupColumn" || e.Items[i].Name == "ItemGroupBox")
				{
					e.Items[i].IsVisible = true;
					continue;
				}
				if (e.Items[i].Name == "ItemSortAscending" || e.Items[i].Name == "ItemSortDescending" || e.Items[i].Name == "ItemClearSorting")
				{
					e.Items[i].IsVisible = ((TableView)e.OriginalSource).Grid.CurrentColumn.AllowSorting == DefaultBoolean.True;
					continue;
				}
				e.Items[i].IsVisible = true;
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
					barButtonItem2.ItemClick += ColumnsAutofit;
				}
			}
			e.Customizations.Add(new BarItemLinkSeparator());
			BarButtonItem barButtonItem3 = new BarButtonItem
			{
				Name = "bShowHeaderIn2Lines",
				Content = ArmSoft.SME.Common.Resources.ShowHeaderIn2Lines
			};
			if (ColHeadersShownIn2lines())
			{
				barButtonItem3.Glyph = ImageService.GetImage("check_16.svg");
			}
			else
			{
				barButtonItem3.Glyph = null;
			}
			barButtonItem3.ItemClick += ShowHeaderIn2Lines;
			e.Customizations.Add(barButtonItem3);
			barButtonItem3 = new BarButtonItem
			{
				Content = ArmSoft.SME.Common.Resources.AutoWidthColumns,
				Name = "AutoWidthColumns"
			};
			if (IsAutoWidthMode())
			{
				barButtonItem3.Glyph = ImageService.GetImage("check_16.svg");
			}
			else
			{
				barButtonItem3.Glyph = null;
			}
			barButtonItem3.ItemClick += new SafeEventHandler<ItemClickEventArgs>(AutoWidthColumns).Handler;
			e.Customizations.Add(barButtonItem3);
		}
		e.Handled = false;
	}

	private void ShowColumnChooser(object sender, ItemClickEventArgs e)
	{
		DependencyObject parent = VisualTreeHelper.GetParent((DependencyObject)(object)this);
		while (parent != null && !(parent is Window))
		{
			parent = VisualTreeHelper.GetParent(parent);
		}
		if (parent != null)
		{
			DataViewPanel.ShowColumnChooser(Grid, parent);
		}
	}

	private void PreventMouseClick(object sender, MouseButtonEventArgs e)
	{
		TextBox textBox = (TextBox)sender;
		textBox.SelectionStart = 0;
		textBox.SelectionLength = 0;
		e.Handled = true;
	}

	private void ShowHeaderIn2Lines(object sender, ItemClickEventArgs e)
	{
		_showColHeaderIn2Lines = !_showColHeaderIn2Lines;
		foreach (GridColumn column in Grid.Columns)
		{
			if (column.HeaderTemplate == null)
			{
				column.HeaderTemplate = (DataTemplate)Resources["Column2RowHeaderTemplate"];
			}
			else
			{
				column.HeaderTemplate = null;
			}
		}
	}

	private bool ColHeadersShownIn2lines()
	{
		if (Grid.Columns.Count > 0)
		{
			return Grid.Columns[0].HeaderTemplate != null;
		}
		return false;
	}

	private void AutoWidthColumns(object sender, ItemClickEventArgs e)
	{
		InnerView.AutoWidth = !InnerView.AutoWidth;
	}

	private bool IsAutoWidthMode()
	{
		return InnerView.AutoWidth;
	}

	private void ColumnsAutofit(object sender, ItemClickEventArgs e)
	{
		InnerView.AutoWidth = false;
		InnerView.BestFitColumns();
	}

	public void RestoreSettings(RegistryKey rk)
	{
		if (string.IsNullOrWhiteSpace(Name))
		{
			throw new InvalidOperationException("Name is empty");
		}
		if (rk == null)
		{
			return;
		}
		RegistryKey registryKey = rk.OpenSubKey("Grid\\" + Name, writable: false);
		if (registryKey == null)
		{
			return;
		}
		ShowColHeaderIn2Lines = Convert.ToBoolean(registryKey.GetValue("HeadersIn2Lines"));
		InnerView.AutoWidth = Convert.ToBoolean(registryKey.GetValue("AutoWidth"));
		Dictionary<string, double> dictionary = new Dictionary<string, double>();
		RegistryKey registryKey2 = registryKey.OpenSubKey("Columns");
		if (registryKey2 != null)
		{
			string[] valueNames = registryKey2.GetValueNames();
			foreach (string text in valueNames)
			{
				if (double.TryParse(registryKey2.GetValue(text, "0").ToString(), out var result) && result > 0.0)
				{
					dictionary.Add(text, result);
				}
			}
			registryKey2.Close();
		}
		registryKey.Close();
		foreach (GridColumn column in Grid.Columns)
		{
			if (dictionary.TryGetValue(column.Name.Trim(), out var value))
			{
				column.Width = value;
			}
		}
	}

	public void StoreSettings(RegistryKey rk)
	{
		if (string.IsNullOrWhiteSpace(Name))
		{
			throw new InvalidOperationException("Name is empty");
		}
		if (rk == null)
		{
			return;
		}
		using RegistryKey registryKey = rk.CreateSubKey("Grid\\" + Name);
		registryKey.SetValue("HeadersIn2Lines", ShowColHeaderIn2Lines);
		registryKey.SetValue("AutoWidth", IsAutoWidthMode());
		using RegistryKey registryKey2 = registryKey.CreateSubKey("Columns");
		foreach (GridColumn column in Grid.Columns)
		{
			if (!string.IsNullOrWhiteSpace(column.Name))
			{
				string text = column.ActualWidth.ToString();
				if (text != "NaN")
				{
					registryKey2.SetValue(column.Name.Trim(), text);
				}
			}
		}
	}

	private void DocumentGridControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (!IsVisible)
		{
			WriteToStatusBarForCurrentRow?.Invoke(this, "");
		}
		else
		{
			RowChanged();
		}
	}

	private void Grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
	{
		if (e.IsGetData)
		{
			e.Value = e.ListSourceRowIndex + 1;
		}
	}
}
