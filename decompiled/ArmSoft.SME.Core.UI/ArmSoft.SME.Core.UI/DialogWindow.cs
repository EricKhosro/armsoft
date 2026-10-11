using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Dialog;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Converters;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI;

public class DialogWindow : ThemedWindow, IScanable, IMagneticCardReadable
{
	public class StoreValueDescriptor
	{
		public string Name;

		private static readonly List<Type> _supportedTypes;

		public string Caption { get; set; }

		public bool DefaultStoreFlag { get; set; }

		public bool ActualStoreFlag { get; set; }

		public bool ShareStoreFlagWithPreviousControl { get; set; }

		public string DefaultValue { get; set; }

		public string ActualValue { get; set; }

		static StoreValueDescriptor()
		{
			int num = 14;
			List<Type> list = new List<Type>(num);
			CollectionsMarshal.SetCount(list, num);
			Span<Type> span = CollectionsMarshal.AsSpan(list);
			span[0] = typeof(DropDownControl);
			span[1] = typeof(MultiSelectDropDownControl);
			span[2] = typeof(CheckEditExt);
			span[3] = typeof(RadioButtonExt);
			span[4] = typeof(DateEditControl);
			span[5] = typeof(PopupCalcEditExt);
			span[6] = typeof(ButtonEditExt);
			span[7] = typeof(TextEditExt);
			span[8] = typeof(TimeEditControl);
			span[9] = typeof(SpinEditExt);
			span[10] = typeof(ComboBoxExt);
			span[11] = typeof(DropDownCommentControl);
			span[12] = typeof(ComboBoxEditExt);
			span[13] = typeof(MonthYearControl);
			_supportedTypes = list;
		}

		public static string GetValue(Control control)
		{
			string result = "";
			if (control is DropDownControl dropDownControl)
			{
				result = dropDownControl.Code;
			}
			else if (control is MultiSelectDropDownControl multiSelectDropDownControl)
			{
				result = multiSelectDropDownControl.SelectedString;
			}
			else if (control is CheckEditExt checkEditExt)
			{
				if (checkEditExt.IsThreeState)
				{
					if (checkEditExt.IsChecked.HasValue)
					{
						result = (Convert.ToBoolean(checkEditExt.IsChecked) ? "1" : "0");
					}
					else
					{
						result = string.Empty;
					}
				}
				else
				{
					result = (Convert.ToBoolean(checkEditExt.IsChecked) ? "1" : "0");
				}
			}
			else if (control is RadioButtonExt radioButtonExt)
			{
				result = (Convert.ToBoolean(radioButtonExt.IsChecked) ? "1" : "0");
			}
			else if (control is DateEditControl { EditValue: var editValue } dateEditControl)
			{
				result = ((!editValue.HasValue) ? "" : dateEditControl.EditValue.FormatYYYYMMDD());
			}
			else if (control is PopupCalcEditExt { Value: var value })
			{
				result = value.ToString();
			}
			else if (control is TimeEditControl timeEditControl)
			{
				result = timeEditControl.EditValue.ToString();
			}
			else if (control is ButtonEditExt buttonEditExt)
			{
				result = buttonEditExt.Text;
			}
			else if (control is TextEditExt textEditExt)
			{
				result = textEditExt.Text;
			}
			else if (control is SpinEditExt { Value: var value2 })
			{
				result = value2.ToString();
			}
			else if (control is ComboBoxExt { SelectedIndex: var selectedIndex })
			{
				result = selectedIndex.ToString();
			}
			else if (control is DropDownCommentControl dropDownCommentControl)
			{
				result = dropDownCommentControl.Text;
			}
			else if (control is ComboBoxEditExt comboBoxEditExt)
			{
				result = comboBoxEditExt.Text;
			}
			else if (control is MonthYearControl monthYearControl)
			{
				DateTime? dateTime = (DateTime?)monthYearControl.EditValue;
				result = ((!dateTime.HasValue) ? "" : dateTime.Value.FormatYYYYMMDD());
			}
			return result;
		}

		public static void SetValue(Control control, string value)
		{
			if (control is DropDownControl dropDownControl)
			{
				dropDownControl.Code = value;
			}
			else if (control is MultiSelectDropDownControl multiSelectDropDownControl)
			{
				multiSelectDropDownControl.SelectedString = value;
			}
			else if (control is CheckEditExt checkEditExt)
			{
				if (checkEditExt.IsThreeState)
				{
					checkEditExt.IsChecked = value == "1" || (!(value == "0") && false);
				}
				else
				{
					checkEditExt.IsChecked = Convert.ToBoolean(value == "1");
				}
			}
			else if (control is RadioButtonExt radioButtonExt)
			{
				radioButtonExt.IsChecked = Convert.ToBoolean(value == "1");
			}
			else if (control is DateEditControl dateEditControl)
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					dateEditControl.EditValue = null;
				}
				else
				{
					dateEditControl.EditValue = value.YYYYMMDDToDateOnly();
				}
			}
			else if (control is PopupCalcEditExt popupCalcEditExt)
			{
				if (decimal.TryParse(value, out var result))
				{
					popupCalcEditExt.Value = result;
				}
			}
			else if (control is ButtonEditExt buttonEditExt)
			{
				buttonEditExt.Text = value;
			}
			else if (control is TextEditExt textEditExt)
			{
				textEditExt.Text = value;
			}
			else if (control is TimeEditControl timeEditControl)
			{
				timeEditControl.EditValue = value;
			}
			else if (control is SpinEditExt spinEditExt)
			{
				if (decimal.TryParse(value, out var result2))
				{
					spinEditExt.Value = result2;
				}
			}
			else if (control is ComboBoxExt comboBoxExt)
			{
				if (int.TryParse(value, out var result3))
				{
					comboBoxExt.SelectedIndex = result3;
				}
			}
			else if (control is DropDownCommentControl dropDownCommentControl)
			{
				dropDownCommentControl.Text = value;
			}
			else if (control is ComboBoxEditExt comboBoxEditExt)
			{
				comboBoxEditExt.Text = value;
			}
			else if (control is MonthYearControl monthYearControl)
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					monthYearControl.EditValue = null;
				}
				else
				{
					monthYearControl.EditValue = value.YYYYMMDDToDate();
				}
			}
		}

		public static bool Supported(Control control)
		{
			return _supportedTypes.Contains(((object)control).GetType());
		}
	}

	public delegate void WindowActivatedEventHandler(DialogWindow window);

	public delegate void WindowValidationEventHandler(DialogWindow window, DialogWindowValidationArgs args);

	public delegate void NextPressedEventHandler(DialogWindow window, EventArgs e);

	public class ControlDescriptor
	{
		public Control Control { get; set; }

		public LayoutItemWithRequired LayoutItem { get; set; }

		public string Caption { get; set; }

		public string Name { get; set; }

		public string DataItemName { get; set; }

		public string SpecData { get; set; }
	}

	protected SimpleButton _nextButton;

	protected SimpleButton _okButton;

	protected TextBlock _txtStatus;

	protected SimpleButton _cancelButton;

	public LayoutControl _layoutControl;

	protected Button _optionsButton;

	protected PopupMenu _optionsPopupMenu;

	private BarButtonItem _barButtonSaveAs;

	private BarButtonItem _barButtonManageTemplates;

	private BarSubItem _barSubItemAvailableTemplates;

	private BarItemLink _barButtonManageTemplatesItemLink;

	private BarItemLink _barSubItemAvailableTemplatesItemLink;

	private List<BarButtonItem> _availableTemplates;

	private List<DialogValuesTemplate> _dialogValueTemplates;

	protected List<DXTabItem> _tabControls;

	public readonly List<StoreValueDescriptor> StoredValues;

	private const string _storeFlag = "StoreFlag_";

	public Control SetFocusOnActivation;

	private bool _firstActivation;

	private readonly Dictionary<string, object> _properties;

	private DialogWindowMode _mode;

	private string _caption;

	private bool _touchMode;

	private ToggleButton _storeValuesButton;

	private Button _selectFieldsToSaveButton;

	private Button _clearValuesButton;

	protected BarManager _barManager;

	protected string _registryKey = "";

	private static CommandBinding _bnd;

	public Dictionary<string, ControlDescriptor> Controls { get; protected set; }

	protected virtual bool SkipReadOnlyControlsValidation { get; } = true;

	public string HelpId { get; set; }

	public virtual string OkButtonText => ArmSoft.SME.Common.Resources.Ok;

	public virtual ImageSource OkButtonImageSource { get; }

	public virtual string CancelButtonText => ArmSoft.SME.Common.Resources.Cancel;

	public virtual ImageSource CancelButtonImageSource { get; }

	public Dictionary<string, object> Properties => _properties;

	public string Caption
	{
		get
		{
			return _caption;
		}
		set
		{
			_caption = value;
			switch (_mode)
			{
			case DialogWindowMode.None:
				Title = value;
				break;
			case DialogWindowMode.NewMode:
			case DialogWindowMode.NewModeWithNext:
			case DialogWindowMode.CreateCopyMode:
			case DialogWindowMode.CreateCopyModeWithNext:
				Title = value + ArmSoft.SME.Common.Resources.CaptionNew;
				break;
			case DialogWindowMode.EditMode:
				Title = value + ArmSoft.SME.Common.Resources.CaptionEdit;
				break;
			case DialogWindowMode.ViewMode:
				Title = value + ArmSoft.SME.Common.Resources.CaptionView;
				break;
			case DialogWindowMode.DeleteMode:
				break;
			}
		}
	}

	public DialogWindowMode Mode
	{
		get
		{
			return _mode;
		}
		set
		{
			_mode = value;
			SetButtonsContent();
		}
	}

	public bool TouchMode
	{
		get
		{
			return _touchMode;
		}
		set
		{
			_touchMode = value;
		}
	}

	public bool AllowParameterTemplates { get; set; }

	public bool AllowStoreValues => _storeValuesButton != null;

	public bool StoreValuesIsChecked
	{
		get
		{
			if (_storeValuesButton == null)
			{
				return false;
			}
			return _storeValuesButton.IsChecked.Value;
		}
	}

	public object this[string name]
	{
		get
		{
			return Value(name);
		}
		set
		{
			Set_Value(name, value);
		}
	}

	public event WindowActivatedEventHandler WindowActivated;

	public event WindowValidationEventHandler WindowValidation;

	public event NextPressedEventHandler NextPressed;

	public virtual void OnMagneticCardReader(string mcrData)
	{
	}

	public virtual void OnScanner(string scannerData)
	{
	}

	public DialogWindow(Window owner = null)
	{
		WindowStartupLocation = WindowStartupLocation.CenterOwner;
		ShowInTaskbar = false;
		ResizeMode = ResizeMode.NoResize;
		ShowIcon = false;
		SizeToContent = SizeToContent.WidthAndHeight;
		UseLayoutRounding = true;
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		Foreground = Settings.GetForegroundBrush();
		DockPanel dockPanel = new DockPanel
		{
			LastChildFill = true
		};
		Grid grid = new Grid();
		dockPanel.Children.Add(grid);
		DockPanel.SetDock(grid, System.Windows.Controls.Dock.Bottom);
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
			Width = new GridLength(1.0, GridUnitType.Auto)
		};
		System.Windows.Controls.ColumnDefinition value5 = new System.Windows.Controls.ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		};
		grid.ColumnDefinitions.Add(value);
		grid.ColumnDefinitions.Add(value2);
		grid.ColumnDefinitions.Add(value3);
		grid.ColumnDefinitions.Add(value4);
		grid.ColumnDefinitions.Add(value5);
		System.Windows.Controls.RowDefinition value6 = new System.Windows.Controls.RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Auto)
		};
		grid.RowDefinitions.Add(value6);
		_nextButton = new SimpleButton
		{
			HorizontalAlignment = HorizontalAlignment.Center,
			Content = ArmSoft.SME.Common.Resources.NextWord,
			MinWidth = 100.0,
			Margin = new Thickness(15.0, 0.0, 15.0, 5.0),
			UseLayoutRounding = true
		};
		Grid.SetRow(_nextButton, 0);
		Grid.SetColumn(_nextButton, 1);
		_nextButton.Visibility = Visibility.Collapsed;
		grid.Children.Add(_nextButton);
		_okButton = new SimpleButton
		{
			HorizontalAlignment = HorizontalAlignment.Center,
			Content = OkButtonText,
			MinWidth = 100.0,
			Margin = new Thickness(15.0, 0.0, 15.0, 5.0),
			UseLayoutRounding = true
		};
		if (OkButtonImageSource != null)
		{
			_okButton.Glyph = OkButtonImageSource;
		}
		Grid.SetRow(_okButton, 0);
		Grid.SetColumn(_okButton, 2);
		grid.Children.Add(_okButton);
		_cancelButton = new SimpleButton
		{
			HorizontalAlignment = HorizontalAlignment.Center,
			Content = CancelButtonText,
			MinWidth = 100.0,
			IsCancel = true,
			Margin = new Thickness(15.0, 0.0, 15.0, 5.0),
			UseLayoutRounding = true
		};
		if (CancelButtonImageSource != null)
		{
			_cancelButton.Glyph = CancelButtonImageSource;
		}
		Grid.SetRow(_cancelButton, 0);
		Grid.SetColumn(_cancelButton, 3);
		grid.Children.Add(_cancelButton);
		_txtStatus = new TextBlock
		{
			Name = "txtStatus",
			Margin = new Thickness(5.0, 5.0, 5.0, 5.0),
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Center,
			UseLayoutRounding = true,
			Text = ""
		};
		Grid.SetRow(_txtStatus, 0);
		Grid.SetColumn(_txtStatus, 0);
		grid.Children.Add(_txtStatus);
		_layoutControl = new LayoutControl
		{
			VerticalAlignment = VerticalAlignment.Stretch,
			Orientation = Orientation.Vertical,
			UseLayoutRounding = true,
			ScrollBars = ScrollBars.Auto
		};
		dockPanel.Children.Add(_layoutControl);
		_barManager = new BarManager
		{
			Child = dockPanel
		};
		Content = _barManager;
		_firstActivation = true;
		SetFocusOnActivation = null;
		Controls = new Dictionary<string, ControlDescriptor>();
		_tabControls = new List<DXTabItem>();
		_properties = new Dictionary<string, object>();
		StoredValues = new List<StoreValueDescriptor>();
		_mode = DialogWindowMode.None;
		Caption = "";
		if (owner == null)
		{
			Owner = Settings.HostingEnvironment;
		}
		else
		{
			Owner = owner;
		}
		Closed += new SafeEventHandler<EventArgs>(DialogWindow_Closed).Handler;
		Loaded += new SafeEventHandler<RoutedEventArgs>(DialogWindow_Loaded).Handler;
		Activated += new SafeEventHandler<EventArgs>(DialogWindow_Activated).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(DialogWindow_PreviewKeyDown).Handler;
		_okButton.Click += new SafeEventHandler<RoutedEventArgs>(OkButton_Click).Handler;
		_nextButton.Click += new SafeEventHandler<RoutedEventArgs>(NextButton_Click).Handler;
	}

	private void DialogWindow_Closed(object sender, EventArgs e)
	{
		if (Controls == null)
		{
			return;
		}
		foreach (ControlDescriptor value in Controls.Values)
		{
			if (value.LayoutItem != null)
			{
				value.LayoutItem.Content = null;
			}
		}
	}

	private void DialogWindow_Loaded(object sender, RoutedEventArgs e)
	{
		if (TouchMode)
		{
			_nextButton.Height = 40.0;
			_okButton.Height = 40.0;
			_cancelButton.Height = 40.0;
		}
		try
		{
			LoadValues();
			RestoreSettings();
			RegisterDatesCommand();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDialogDefinition))
		{
			CallsRecordingHelper.ExportDialogDefinition(Name, Caption, GetControlsInfoForRecord(withoutValues: true));
		}
	}

	public static void RegisterDatesCommand()
	{
		if (_bnd == null)
		{
			_bnd = new CommandBinding(new RoutedCommand
			{
				InputGestures = { (InputGesture)new KeyGesture((Key)47, (ModifierKeys)1) }
			}, new SafeEventHandler<ExecutedRoutedEventArgs>(ShowSpecialDates).Handler);
			CommandManager.RegisterClassCommandBinding(typeof(DateEditControl), _bnd);
		}
	}

	private void SetButtonsContent()
	{
		if (_mode == DialogWindowMode.ViewMode)
		{
			_nextButton.Visibility = Visibility.Collapsed;
			_okButton.Visibility = Visibility.Collapsed;
			_cancelButton.Content = "OK";
			return;
		}
		_okButton.Content = ((_mode == DialogWindowMode.DeleteMode) ? ArmSoft.SME.Common.Resources.Delete : ArmSoft.SME.Common.Resources.Ok);
		_cancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		_nextButton.Content = ArmSoft.SME.Common.Resources.NextWord;
		if (_mode == DialogWindowMode.NewModeWithNext || _mode == DialogWindowMode.CreateCopyModeWithNext)
		{
			_nextButton.Visibility = Visibility.Visible;
		}
		else
		{
			_nextButton.Visibility = Visibility.Collapsed;
		}
	}

	protected LayoutGroup ButtonsContainer()
	{
		if (_storeValuesButton == null)
		{
			return null;
		}
		return (LayoutGroup)(object)((LayoutItem)(object)_storeValuesButton.Parent).Parent;
	}

	public TextEditExt AddStringControl(string name, string caption, short length, bool isRequired = false, bool storeValue = false, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		return AddStringControl(null, name, caption, length, isRequired, storeValue, captionForStoreValue, HorizontalAlignment.Left, LayoutItemLabelPosition.Left, isNew, helpText);
	}

	public virtual TextEditExt AddStringControl(FieldDefinition definition, bool isNew = false, string helpText = "")
	{
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddStringControl(stringFieldDefinition.Name, stringFieldDefinition.Caption, stringFieldDefinition.Length, stringFieldDefinition.IsRequired, storeValue: false, "", isNew, helpText);
	}

	public virtual TextEditExt AddStringControl(LayoutGroup container, FieldDefinition definition, bool isNew = false, string helpText = "")
	{
		StringFieldDefinition stringFieldDefinition = (StringFieldDefinition)definition;
		return AddStringControl(container, stringFieldDefinition.Name, stringFieldDefinition.Caption, stringFieldDefinition.Length, stringFieldDefinition.IsRequired, storeValue: false, "", HorizontalAlignment.Left, LayoutItemLabelPosition.Left, isNew, helpText);
	}

	public TextEditExt AddStringControl(LayoutGroup container, string name, string caption, short length, bool isRequired = false, bool storeValue = false, string captionForStoreValue = "", HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, LayoutItemLabelPosition labelPosition = LayoutItemLabelPosition.Left, bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, horizontalAlignment, labelPosition, isNew, helpText);
		TextEditExt textEditExt = new TextEditExt
		{
			Name = name.ToUpper(),
			IsRequired = isRequired,
			MaxLength = length,
			MinWidth = 5.0,
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		if (horizontalAlignment != HorizontalAlignment.Stretch)
		{
			textEditExt.Width = SizeCalculator.WidthForString(length);
		}
		CommonEventHandlers.AddIsRequiredBinding(textEditExt, layoutItemWithRequired);
		layoutItemWithRequired.Content = textEditExt;
		AddInCollections(textEditExt, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(textEditExt);
		return textEditExt;
	}

	public Button AddButton(LayoutGroup container, string name, string imageURI = "", string caption = "", string labelCaption = "", string tooltip = "", bool isTabStop = false, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, double imgWidth = 16.0, double imgHeight = 16.0)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		CheckControlExistence(name);
		Button button = new Button();
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, labelCaption);
		layoutItemWithRequired.HorizontalAlignment = horizontalAlignment;
		button.Name = name;
		if (!string.IsNullOrWhiteSpace(tooltip))
		{
			button.ToolTip = tooltip;
		}
		button.IsTabStop = isTabStop;
		if (string.IsNullOrWhiteSpace(imageURI))
		{
			button.Content = caption;
		}
		else
		{
			StackPanel stackPanel = new StackPanel();
			stackPanel.Orientation = Orientation.Horizontal;
			ImageSource image = ImageService.GetImage(imageURI);
			DXImage element = new DXImage
			{
				Source = image,
				Width = imgWidth,
				Height = imgHeight
			};
			stackPanel.Children.Add(element);
			if (!string.IsNullOrWhiteSpace(caption))
			{
				TextBlock element2 = new TextBlock
				{
					Text = caption,
					Margin = new Thickness(3.0, 0.0, 0.0, 0.0)
				};
				stackPanel.Children.Add(element2);
			}
			stackPanel.Margin = new Thickness(-7.0, 0.0, -7.0, 0.0);
			button.Content = stackPanel;
		}
		layoutItemWithRequired.Content = button;
		AddInCollections(button, caption, layoutItemWithRequired, storeValue: false, "", container);
		return button;
	}

	public SplitButton AddSplitButton(LayoutGroup container, string name, string imageURI = "", string caption = "", string labelCaption = "", string tooltip = "", bool isTabStop = false, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, double imgWidth = 16.0, double imgHeight = 16.0, IEnumerable<IBarItem> popupItems = null, bool showGlyphsInPopupMenu = true)
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		CheckControlExistence(name);
		SplitButton splitButton = new SplitButton
		{
			IsTabStop = isTabStop,
			Name = name,
			ArrowPadding = new Thickness(2.0, 0.0, 2.0, 0.0)
		};
		if (!string.IsNullOrWhiteSpace(tooltip))
		{
			splitButton.ToolTip = tooltip;
		}
		if (string.IsNullOrWhiteSpace(imageURI))
		{
			splitButton.Content = caption;
		}
		else
		{
			StackPanel stackPanel = new StackPanel
			{
				Orientation = Orientation.Horizontal,
				Margin = new Thickness(-10.0, 0.0, -3.0, 0.0)
			};
			stackPanel.Children.Add(new DXImage
			{
				Source = ImageService.GetImage(imageURI),
				Width = imgWidth,
				Height = imgHeight
			});
			if (!string.IsNullOrWhiteSpace(caption))
			{
				stackPanel.Children.Add(new TextBlock
				{
					Text = caption,
					Margin = new Thickness(3.0, 0.0, 0.0, 0.0)
				});
			}
			splitButton.Content = stackPanel;
		}
		if (popupItems != null)
		{
			PopupMenu popupMenu = new PopupMenu();
			BarManager.SetShowGlyphsInPopupMenus((DependencyObject)(object)popupMenu, showGlyphsInPopupMenu);
			foreach (IBarItem popupItem in popupItems)
			{
				popupMenu.Items.Add(popupItem);
			}
			splitButton.PopupContent = popupMenu;
		}
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, labelCaption);
		layoutItemWithRequired.HorizontalAlignment = horizontalAlignment;
		layoutItemWithRequired.Content = splitButton;
		AddInCollections(splitButton, caption, layoutItemWithRequired, storeValue: false, "", container);
		return splitButton;
	}

	public DropDownButton AddDropDownButton(LayoutGroup container, string name, string imageURI = "", string caption = "", string labelCaption = "", string tooltip = "", bool isTabStop = false, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, double imgWidth = 16.0, double imgHeight = 16.0, IEnumerable<IBarItem> popupItems = null, bool showGlyphsInPopupMenu = true)
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		CheckControlExistence(name);
		DropDownButton dropDownButton = new DropDownButton
		{
			IsTabStop = isTabStop,
			Name = name,
			ArrowPadding = new Thickness(2.0, 0.0, 2.0, 0.0)
		};
		if (!string.IsNullOrWhiteSpace(tooltip))
		{
			dropDownButton.ToolTip = tooltip;
		}
		if (string.IsNullOrWhiteSpace(imageURI))
		{
			dropDownButton.Content = caption;
		}
		else
		{
			StackPanel stackPanel = new StackPanel
			{
				Orientation = Orientation.Horizontal,
				Margin = new Thickness(-10.0, 0.0, -3.0, 0.0)
			};
			stackPanel.Children.Add(new DXImage
			{
				Source = ImageService.GetImage(imageURI),
				Width = imgWidth,
				Height = imgHeight
			});
			if (!string.IsNullOrWhiteSpace(caption))
			{
				stackPanel.Children.Add(new TextBlock
				{
					Text = caption,
					Margin = new Thickness(3.0, 0.0, 0.0, 0.0)
				});
			}
			dropDownButton.Content = stackPanel;
		}
		if (popupItems != null)
		{
			PopupMenu popupMenu = new PopupMenu();
			BarManager.SetShowGlyphsInPopupMenus((DependencyObject)(object)popupMenu, showGlyphsInPopupMenu);
			foreach (IBarItem popupItem in popupItems)
			{
				popupMenu.Items.Add(popupItem);
			}
			dropDownButton.PopupContent = popupMenu;
		}
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, labelCaption);
		layoutItemWithRequired.HorizontalAlignment = horizontalAlignment;
		layoutItemWithRequired.Content = dropDownButton;
		AddInCollections(dropDownButton, caption, layoutItemWithRequired, storeValue: false, "", container);
		return dropDownButton;
	}

	public ButtonEditExt AddButtonEditControl(string name, string caption, short length, bool isRequired = false, short estimatedButtonsCount = 1, bool storeValue = false, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		return AddButtonEditControl(null, name, caption, length, isRequired, estimatedButtonsCount, storeValue, captionForStoreValue, LayoutItemLabelPosition.Left, isNew, helpText);
	}

	public ButtonEditExt AddButtonEditControl(LayoutGroup container, string name, string caption, short length, bool isRequired = false, short estimatedButtonsCount = 1, bool storeValue = false, string captionForStoreValue = "", LayoutItemLabelPosition labelPosition = LayoutItemLabelPosition.Left, bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, labelPosition, isNew, helpText);
		ButtonEditExt buttonEditExt = new ButtonEditExt
		{
			Name = name.ToUpper(),
			IsRequired = isRequired,
			MaxLength = length,
			MinWidth = 5.0,
			Owner = this,
			Width = SizeCalculator.WidthForString(length) + (double)(20 * estimatedButtonsCount),
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		CommonEventHandlers.AddIsRequiredBinding(buttonEditExt, layoutItemWithRequired);
		layoutItemWithRequired.Content = buttonEditExt;
		AddInCollections(buttonEditExt, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(buttonEditExt);
		return buttonEditExt;
	}

	public PasswordBoxEditExt AddPasswordControl(string name, string caption, short length, bool isRequired = false, bool isNew = false, string helpText = "")
	{
		return AddPasswordControl(null, name, caption, length, isRequired, isNew, helpText);
	}

	public PasswordBoxEditExt AddPasswordControl(LayoutGroup container, string name, string caption, short length, bool isRequired = false, bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, LayoutItemLabelPosition.Left, isNew, helpText);
		PasswordBoxEditExt passwordBoxEditExt = new PasswordBoxEditExt
		{
			Name = name.ToUpper(),
			MaxLength = length,
			MinWidth = 5.0,
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		CommonEventHandlers.AddIsRequiredBinding(passwordBoxEditExt, layoutItemWithRequired);
		passwordBoxEditExt.IsRequired = isRequired;
		passwordBoxEditExt.Width = SizeCalculator.WidthForString(length);
		layoutItemWithRequired.Content = passwordBoxEditExt;
		AddInCollections(passwordBoxEditExt, caption, layoutItemWithRequired, storeValue: false, "", container);
		TabValidationHelper.RegisterControl(passwordBoxEditExt);
		return passwordBoxEditExt;
	}

	public MonthYearControl AddMonthYearControl(string name, string caption, bool isFirstMonthDay, bool isRequired = false, bool storeValue = false, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		return AddMonthYearControl(null, name, caption, isFirstMonthDay, isRequired, storeValue, captionForStoreValue, isNew, helpText);
	}

	public MonthYearControl AddMonthYearControl(LayoutGroup container, string name, string caption, bool isFirstMonthDay, bool isRequired = false, bool storeValue = false, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, LayoutItemLabelPosition.Left, isNew, helpText);
		MonthYearControl monthYearControl = new MonthYearControl
		{
			Name = name.ToUpper(),
			IsRequired = isRequired,
			IsFirstMonthDay = isFirstMonthDay,
			Width = SizeCalculator.WidthForMonthYear(),
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		CommonEventHandlers.AddIsRequiredBinding(monthYearControl, layoutItemWithRequired);
		layoutItemWithRequired.Content = monthYearControl;
		AddInCollections(monthYearControl, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(monthYearControl);
		return monthYearControl;
	}

	public PopupCalcEditExt AddNumericControl(string name, string caption, short length, short precision = 0, bool isRequired = false, bool allowNegativeNumbers = false, bool useCurrencyFormat = true, bool storeValue = false, string captionForStoreValue = "", Type editValueType = null, bool isNew = false, string helpText = "")
	{
		return AddNumericControl(null, name, caption, length, precision, isRequired, allowNegativeNumbers, useCurrencyFormat, storeValue, captionForStoreValue, editValueType, isNew, helpText);
	}

	public PopupCalcEditExt AddNumericControl(LayoutGroup container, string name, string caption, short length, short precision = 0, bool isRequired = false, bool allowNegativeNumbers = false, bool useCurrencyFormat = true, bool storeValue = false, string captionForStoreValue = "", Type editValueType = null, bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, LayoutItemLabelPosition.Left, isNew, helpText);
		PopupCalcEditExt popupCalcEditExt = new PopupCalcEditExt
		{
			Name = name.ToUpper(),
			Mask = (useCurrencyFormat ? DecimalExtension.GetCurrencyFormat(length, precision) : DecimalExtension.GetNumberFormat(length, precision)),
			Precision = precision,
			IsRequired = isRequired,
			AllowNegativeNumbers = allowNegativeNumbers,
			Width = SizeCalculator.WidthForNumber(length),
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		if (editValueType != null)
		{
			popupCalcEditExt.EditValueType = editValueType;
		}
		CommonEventHandlers.AddIsRequiredBinding(popupCalcEditExt, layoutItemWithRequired);
		layoutItemWithRequired.Content = popupCalcEditExt;
		AddInCollections(popupCalcEditExt, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(popupCalcEditExt);
		return popupCalcEditExt;
	}

	public virtual PopupCalcEditExt AddNumericControl(FieldDefinition definition, bool useCurrencyFormat = true, bool isNew = false, string helpText = "")
	{
		NumberFieldDefinition numberFieldDefinition = (NumberFieldDefinition)definition;
		if (numberFieldDefinition.PrecisionDefinedInParam)
		{
			return AddNumericControl(numberFieldDefinition.Name, numberFieldDefinition.Caption, numberFieldDefinition.LengthFromParam(Settings.ParametersService), numberFieldDefinition.PrecisionFromParam(Settings.ParametersService), numberFieldDefinition.IsRequired, !numberFieldDefinition.IsNonNegative, useCurrencyFormat, storeValue: false, "", numberFieldDefinition.Type, isNew, helpText);
		}
		return AddNumericControl(numberFieldDefinition.Name, numberFieldDefinition.Caption, numberFieldDefinition.Length, numberFieldDefinition.Precision, numberFieldDefinition.IsRequired, !numberFieldDefinition.IsNonNegative, useCurrencyFormat, storeValue: false, "", numberFieldDefinition.Type, isNew, helpText);
	}

	public virtual PopupCalcEditExt AddNumericControl(LayoutGroup container, FieldDefinition definition, bool useCurrencyFormat = true, bool isNew = false, string helpText = "")
	{
		NumberFieldDefinition numberFieldDefinition = (NumberFieldDefinition)definition;
		if (numberFieldDefinition.PrecisionDefinedInParam)
		{
			return AddNumericControl(container, numberFieldDefinition.Name, numberFieldDefinition.Caption, numberFieldDefinition.LengthFromParam(Settings.ParametersService), numberFieldDefinition.PrecisionFromParam(Settings.ParametersService), numberFieldDefinition.IsRequired, !numberFieldDefinition.IsNonNegative, useCurrencyFormat, storeValue: false, "", numberFieldDefinition.Type, isNew, helpText);
		}
		return AddNumericControl(container, numberFieldDefinition.Name, numberFieldDefinition.Caption, numberFieldDefinition.Length, numberFieldDefinition.Precision, numberFieldDefinition.IsRequired, !numberFieldDefinition.IsNonNegative, useCurrencyFormat, storeValue: false, "", numberFieldDefinition.Type, isNew, helpText);
	}

	public SpinEditExt AddSpinEdit(string name, string caption, decimal minValue = 0m, decimal maxValue = 2m, double minWidth = 50.0, bool storeValue = false, string captionForStoreValue = "", Type editValueType = null, bool isNew = false, string helpText = "")
	{
		return AddSpinEdit(null, name, caption, minValue, maxValue, minWidth, storeValue, captionForStoreValue, editValueType, LayoutItemLabelPosition.Left, isNew, helpText);
	}

	public SpinEditExt AddSpinEdit(LayoutGroup container, string name, string caption, decimal minValue = 0m, decimal maxValue = 2m, double minWidth = 50.0, bool storeValue = false, string captionForStoreValue = "", Type editValueType = null, LayoutItemLabelPosition labelPosition = LayoutItemLabelPosition.Left, bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, labelPosition, isNew, helpText);
		SpinEditExt spinEditExt = new SpinEditExt
		{
			Name = name.ToUpper(),
			IsFloatValue = false,
			MinValue = minValue,
			MaxValue = maxValue,
			IsReadOnly = false,
			MinWidth = minWidth
		};
		if (editValueType != null)
		{
			spinEditExt.EditValueType = editValueType;
		}
		layoutItemWithRequired.HorizontalContentAlignment = HorizontalAlignment.Left;
		CommonEventHandlers.AddIsRequiredBinding(spinEditExt, layoutItemWithRequired);
		layoutItemWithRequired.Content = spinEditExt;
		AddInCollections(spinEditExt, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(spinEditExt);
		return spinEditExt;
	}

	public DateEditControl AddDateControl(string name, string caption, bool useLongDate = false, bool isRequired = false, bool storeValue = false, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		return AddDateControl(null, name, caption, useLongDate, isRequired, storeValue, captionForStoreValue, isNew, helpText);
	}

	public virtual DateEditControl AddDateControl(FieldDefinition definition, bool useLongDate = false, bool isNew = false, string helpText = "")
	{
		return AddDateControl(definition.Name, definition.Caption, useLongDate, definition.IsRequired, storeValue: false, "", isNew, helpText);
	}

	public DateEditControl AddDateControl(LayoutGroup container, string name, string caption, bool useLongDate = false, bool isRequired = false, bool storeValue = false, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, LayoutItemLabelPosition.Left, isNew, helpText);
		DateEditControl dateEditControl = new DateEditControl
		{
			Name = name.ToUpper(),
			UseLongDate = useLongDate,
			IsRequired = isRequired,
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		CommonEventHandlers.AddIsRequiredBinding(dateEditControl, layoutItemWithRequired);
		dateEditControl.Width = SizeCalculator.WidthForDate(useLongDate);
		layoutItemWithRequired.Content = dateEditControl;
		AddInCollections(dateEditControl, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(dateEditControl);
		return dateEditControl;
	}

	public TimeEditControl AddTimeControl(string name, string caption, bool showSeconds = false, bool isRequired = false, bool storeValue = false, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		return AddTimeControl(null, name, caption, showSeconds, isRequired, storeValue, captionForStoreValue, isNew, helpText);
	}

	public TimeEditControl AddTimeControl(LayoutGroup container, string name, string caption, bool showSeconds = false, bool isRequired = false, bool storeValue = false, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, LayoutItemLabelPosition.Left, isNew, helpText);
		TimeEditControl timeEditControl = new TimeEditControl
		{
			Name = name.ToUpper(),
			ShowSeconds = showSeconds,
			IsRequired = isRequired,
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		CommonEventHandlers.AddIsRequiredBinding(timeEditControl, layoutItemWithRequired);
		timeEditControl.Width = SizeCalculator.WidthForTime(showSeconds);
		layoutItemWithRequired.Content = timeEditControl;
		AddInCollections(timeEditControl, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(timeEditControl);
		return timeEditControl;
	}

	public CheckEditExt AddBooleanControl(string name, string caption, bool storeValue = false, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		return AddBooleanControl(null, name, caption, storeValue, isThreeState: false, captionForStoreValue, isNew, helpText);
	}

	public virtual CheckEditExt AddBooleanControl(FieldDefinition definition, bool isNew = false, string helpText = "")
	{
		return AddBooleanControl(definition.Name, definition.Caption, storeValue: false, "", isNew, helpText);
	}

	public virtual CheckEditExt AddBooleanControl(LayoutGroup container, FieldDefinition definition, bool isNew = false, string helpText = "")
	{
		return AddBooleanControl(container, definition.Name, definition.Caption, storeValue: false, isThreeState: false, "", isNew, helpText);
	}

	public CheckEditExt AddBooleanControl(LayoutGroup container, string name, string caption, bool storeValue = false, bool isThreeState = false, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, LayoutItemLabelPosition.Left, isNew, helpText);
		CheckEditExt checkEditExt = new CheckEditExt();
		CommonEventHandlers.AddIsReadonlyBinding(checkEditExt, layoutItemWithRequired);
		checkEditExt.Name = name.ToUpper();
		checkEditExt.Focusable = true;
		checkEditExt.IsChecked = false;
		checkEditExt.IsThreeState = isThreeState;
		layoutItemWithRequired.Content = checkEditExt;
		AddInCollections(checkEditExt, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		return checkEditExt;
	}

	public RadioButtonExt AddRadioButton(string name, string caption, string groupName, bool storeValue = false, string captionForStoreValue = "")
	{
		return AddRadioButton(null, name, caption, groupName, storeValue, captionForStoreValue);
	}

	public RadioButtonExt AddRadioButton(LayoutGroup container, string name, string caption, string groupName, bool storeValue = false, string captionForStoreValue = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption);
		RadioButtonExt radioButtonExt = new RadioButtonExt();
		Binding binding = new Binding("IsEnabled")
		{
			Source = radioButtonExt,
			Mode = BindingMode.OneWay,
			Converter = new InverseBooleanConverter()
		};
		layoutItemWithRequired.SetBinding(LayoutItemWithRequired.IsReadOnlyProperty, binding);
		radioButtonExt.Name = name.ToUpper();
		radioButtonExt.Focusable = true;
		radioButtonExt.IsChecked = false;
		radioButtonExt.GroupName = groupName;
		layoutItemWithRequired.Content = radioButtonExt;
		AddInCollections(radioButtonExt, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		return radioButtonExt;
	}

	public Rectangle AddLine()
	{
		return AddLine(null);
	}

	public Rectangle AddLine(LayoutGroup container)
	{
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, "");
		Rectangle rectangle = new Rectangle();
		SizeCalculator.ConfigureLine(this, rectangle);
		layoutItemWithRequired.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutItemWithRequired.Content = rectangle;
		return rectangle;
	}

	public TextBlock AddTextBlock(string text, TextBlockImage icon)
	{
		return AddTextBlock(null, text, icon);
	}

	public TextBlock AddTextBlock(LayoutGroup container, string text, TextBlockImage icon)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, "");
		layoutItemWithRequired.HorizontalAlignment = HorizontalAlignment.Stretch;
		TextBlock textBlock = new TextBlock
		{
			Text = text
		};
		if (icon == TextBlockImage.None)
		{
			layoutItemWithRequired.Content = textBlock;
		}
		else
		{
			StackPanel stackPanel = new StackPanel
			{
				Orientation = Orientation.Horizontal
			};
			DXImage dXImage = new DXImage
			{
				Stretch = Stretch.Fill,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};
			string image = "";
			switch (icon)
			{
			case TextBlockImage.Warning:
				image = "warning_32.svg";
				break;
			case TextBlockImage.Error:
				image = "error_32.svg";
				break;
			case TextBlockImage.Information:
				image = "information_32.svg";
				break;
			case TextBlockImage.Question:
				image = "question_16.svg";
				break;
			}
			ImageSource image2 = ImageService.GetImage(image);
			dXImage.Source = image2;
			stackPanel.Children.Add(dXImage);
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
			stackPanel.Children.Add(textBlock);
			layoutItemWithRequired.Content = stackPanel;
		}
		return textBlock;
	}

	public ComboBoxExt AddDiagramKind(LayoutGroup container, string name, bool storeValue = false, bool onlyBarsAndLines = false, string captionForStoreValue = "")
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		CheckControlExistence(name);
		ComboBoxExt comboBoxExt = new ComboBoxExt
		{
			Name = name.ToUpper()
		};
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, "");
		layoutItemWithRequired.Label = ArmSoft.SME.Common.Resources.DiagramKind;
		layoutItemWithRequired.HorizontalAlignment = HorizontalAlignment.Left;
		List<DXImage> list = new List<DXImage>();
		Binding binding = new Binding("IsEnabled")
		{
			Source = comboBoxExt,
			Converter = new BooleanToOpacity()
		};
		ImageSource image = ImageService.GetImage("chart_bar_icon_16.svg");
		DXImage dXImage = new DXImage
		{
			Source = image
		};
		dXImage.SetBinding(UIElement.OpacityProperty, binding);
		list.Add(dXImage);
		image = ImageService.GetImage("chart_stock_icon_16.svg");
		dXImage = new DXImage
		{
			Source = image
		};
		dXImage.SetBinding(UIElement.OpacityProperty, binding);
		list.Add(dXImage);
		if (!onlyBarsAndLines)
		{
			image = ImageService.GetImage("pie_chart_icon_16.svg");
			dXImage = new DXImage
			{
				Source = image
			};
			dXImage.SetBinding(UIElement.OpacityProperty, binding);
			list.Add(dXImage);
			image = ImageService.GetImage("chart_pie_icon_16.svg");
			dXImage = new DXImage
			{
				Source = image
			};
			dXImage.SetBinding(UIElement.OpacityProperty, binding);
			list.Add(dXImage);
		}
		comboBoxExt.ItemsSource = list;
		comboBoxExt.SelectedIndex = 0;
		layoutItemWithRequired.Content = comboBoxExt;
		AddInCollections(comboBoxExt, name, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		return comboBoxExt;
	}

	public DropDownControl AddDropDownControl(string name, string caption, IDropDownView idv, bool isRequired = false, bool allowAdditionButtons = false, bool storeValue = false, bool isShowComment = true, int codeLength = -1, bool isNew = false, string helpText = "")
	{
		return AddDropDownControl(null, name, caption, idv, isRequired, allowAdditionButtons, storeValue, isShowComment, codeLength, "", LayoutItemLabelPosition.Left, isNew, helpText);
	}

	public virtual DropDownControl AddDropDownControl(FieldDefinition definition, IDropDownView idv, bool allowAdditionButtons = false, bool isShowComment = true, int codeWidth = -1, bool isNew = false, string helpText = "")
	{
		return AddDropDownControl(definition.Name, definition.Caption, idv, definition.IsRequired, allowAdditionButtons, storeValue: false, isShowComment, codeWidth, isNew, helpText);
	}

	public virtual DropDownControl AddDropDownControl(LayoutGroup container, FieldDefinition definition, IDropDownView idv, bool allowAdditionButtons = false, bool isShowComment = true, int codeWidth = -1, bool isNew = false, string helpText = "")
	{
		return AddDropDownControl(container, definition.Name, definition.Caption, idv, definition.IsRequired, allowAdditionButtons, storeValue: false, isShowComment, codeWidth, "", LayoutItemLabelPosition.Left, isNew, helpText);
	}

	public virtual DropDownControl AddTreeDropDownControl(LayoutGroup container, FieldDefinition fieldDefinition, bool allowAdditionButtons = false, bool isShowComment = true, int codeWidth = -1, bool isNew = false, string helpText = "")
	{
		TreeNodeFieldDefinition treeNodeFieldDefinition = (TreeNodeFieldDefinition)fieldDefinition;
		return AddDropDownControl(container, treeNodeFieldDefinition.Name, treeNodeFieldDefinition.Caption, new TreeNodes(treeNodeFieldDefinition.TreeId, "", showAsTree: true, showExpanded: true, allowOperationWithAdditionButtons: true, treeNodeFieldDefinition.OnlyLeaf), treeNodeFieldDefinition.IsRequired, allowAdditionButtons, storeValue: false, isShowComment, codeWidth, "", LayoutItemLabelPosition.Left, isNew, helpText);
	}

	public void AddControl(string name, string caption, Control myControl, bool storeValue = false, string captionForStoreValue = "")
	{
		AddControl(null, name, caption, myControl, storeValue, addLayoutItemBinding: false, captionForStoreValue);
	}

	public void AddControl(LayoutGroup container, string name, string caption, Control myControl, bool storeValue = false, bool addLayoutItemBinding = false, string captionForStoreValue = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption);
		myControl.Name = name.ToUpper();
		if (addLayoutItemBinding)
		{
			CommonEventHandlers.AddIsRequiredBinding(myControl, layoutItemWithRequired);
		}
		layoutItemWithRequired.Content = myControl;
		layoutItemWithRequired.IsEnabled = true;
		layoutItemWithRequired.IsReadOnly = false;
		AddInCollections(myControl, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
	}

	public static void AddControlWithoutName(LayoutGroup container, Control myControl)
	{
		container.Children.Add(myControl);
	}

	public DropDownControl AddDropDownControl(LayoutGroup container, string name, string caption, IDropDownView idv, bool isRequired = false, bool allowAdditionButtons = false, bool storeValue = false, bool isShowComment = true, int codeWidth = -1, string captionForStoreValue = "", LayoutItemLabelPosition labelPosition = LayoutItemLabelPosition.Left, bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		DropDownControl dropDownControl = new DropDownControl
		{
			Name = name.ToUpper(),
			IsRequired = isRequired,
			AllowAdditionButtons = allowAdditionButtons,
			CodeWidth = codeWidth,
			DataView = idv,
			Owner = this,
			Code = "",
			ShowComment = isShowComment,
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		dropDownControl.Width = dropDownControl.RecommendedWidth;
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, labelPosition, isNew, helpText);
		CommonEventHandlers.AddIsRequiredBinding(dropDownControl, layoutItemWithRequired);
		layoutItemWithRequired.Content = dropDownControl;
		AddInCollections(dropDownControl, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(dropDownControl.InnerCodeEdit);
		return dropDownControl;
	}

	public DropDownCommentControl AddDropDownCommentControl(string name, string caption, IDropDownView idv, bool isRequired, bool storeValue = false, short length = 50, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		return AddDropDownCommentControl(null, name, caption, idv, isRequired, storeValue, length, horizontalAlignment, captionForStoreValue, LayoutItemLabelPosition.Left, isNew, helpText);
	}

	public DropDownCommentControl AddDropDownCommentControl(LayoutGroup container, string name, string caption, IDropDownView idv, bool isRequired, bool storeValue = false, short length = 50, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, string captionForStoreValue = "", LayoutItemLabelPosition labelPosition = LayoutItemLabelPosition.Left, bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		DropDownCommentControl dropDownCommentControl = new DropDownCommentControl
		{
			Name = name.ToUpper(),
			IsRequired = isRequired,
			DataView = idv,
			Owner = this,
			Text = "",
			MaxLength = length,
			MinWidth = 5.0,
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, labelPosition, isNew, helpText);
		CommonEventHandlers.AddIsRequiredBinding(dropDownCommentControl, layoutItemWithRequired);
		if (horizontalAlignment != HorizontalAlignment.Stretch)
		{
			dropDownCommentControl.Width = SizeCalculator.WidthForString(length);
		}
		layoutItemWithRequired.Content = dropDownCommentControl;
		AddInCollections(dropDownCommentControl, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(dropDownCommentControl.InnerCodeEdit);
		return dropDownCommentControl;
	}

	public DocumentGridControl AddGridControl(string name)
	{
		return AddGridControl(null, name);
	}

	public DocumentGridControl AddGridControl(LayoutGroup container, string name)
	{
		CheckControlExistence(name);
		DocumentGridControl documentGridControl = new DocumentGridControl
		{
			Name = name.ToUpper(),
			Owner = this
		};
		documentGridControl.WriteToStatusBarForCurrentRow += new SafeEventHandler<string>(WriteToStatusBarForCurrentRow).Handler;
		documentGridControl.EscapePressed += new SafeEventHandler<EventArgs>(OnGridEscapePressed).Handler;
		if (container == null)
		{
			_layoutControl.Children.Add(documentGridControl);
		}
		else
		{
			container.Children.Add(documentGridControl);
		}
		AddInCollections(documentGridControl, name, null, storeValue: false, null, container);
		TabValidationHelper.RegisterControl(documentGridControl);
		return documentGridControl;
	}

	public MultiSelectDropDownControl AddMultiSelectDropDownControl(string name, string caption, IDropDownView idv, bool isRequired = false, bool storeValue = false, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		return AddMultiSelectDropDownControl(null, name, caption, idv, isRequired, storeValue, captionForStoreValue, isNew, helpText);
	}

	public MultiSelectDropDownControl AddMultiSelectDropDownControl(LayoutGroup container, string name, string caption, IDropDownView idv, bool isRequired = false, bool storeValue = false, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, LayoutItemLabelPosition.Left, isNew, helpText);
		MultiSelectDropDownControl multiSelectDropDownControl = new MultiSelectDropDownControl
		{
			Name = name.ToUpper(),
			IsRequired = isRequired,
			Owner = this,
			DataView = idv,
			SelectedString = "",
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		CommonEventHandlers.AddIsRequiredBinding(multiSelectDropDownControl, layoutItemWithRequired);
		layoutItemWithRequired.Content = multiSelectDropDownControl;
		AddInCollections(multiSelectDropDownControl, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(multiSelectDropDownControl.InnerCodeEdit);
		return multiSelectDropDownControl;
	}

	public MultiFilterControl AddMultiFilterControl(string name, string caption, MultiFilterTypes supportedFilterTypes, string treeId = "", bool isFullTree = false, IDropDownView idv = null, bool isRequired = false, bool storeValue = true, bool showComment = true, bool isExpanded = true, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		return AddMultiFilterControl(null, name, caption, supportedFilterTypes, treeId, isFullTree, idv, isRequired, storeValue, showComment, isExpanded, captionForStoreValue, isNew, helpText);
	}

	public MultiFilterControl AddMultiFilterControl(LayoutGroup container, string name, string caption, MultiFilterTypes supportedFilterTypes, string treeId = "", bool isFullTree = false, IDropDownView idv = null, bool isRequired = false, bool storeValue = true, bool showComment = true, bool isExpanded = true, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		MultiFilterControl multiFilterControl = new MultiFilterControl(supportedFilterTypes, treeId, isFullTree, idv)
		{
			Name = name,
			Owner = this,
			IsRequired = isRequired,
			ShowComment = showComment,
			IsExpanded = isExpanded,
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		multiFilterControl.Width = multiFilterControl.RecommendedWidth;
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, LayoutItemLabelPosition.Left, isNew, helpText);
		CommonEventHandlers.AddIsRequiredBinding(multiFilterControl, layoutItemWithRequired);
		layoutItemWithRequired.Content = multiFilterControl;
		AddInCollections(multiFilterControl, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(multiFilterControl.InnerCodeEdit);
		return multiFilterControl;
	}

	public DateMultiFilterControl AddDateMultiFilterControl(string name, string caption, MultiFilterTypes supportedFilterTypes, bool useLongDate = false, bool addSpecialDateButton = true, bool isRequired = false, bool storeValue = true, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		return AddDateMultiFilterControl(null, name, caption, supportedFilterTypes, useLongDate, addSpecialDateButton, isRequired, storeValue, captionForStoreValue, isNew, helpText);
	}

	public DateMultiFilterControl AddDateMultiFilterControl(LayoutGroup container, string name, string caption, MultiFilterTypes supportedFilterTypes, bool useLongDate = false, bool addSpecialDateButton = true, bool isRequired = false, bool storeValue = true, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		DateMultiFilterControl dateMultiFilterControl = new DateMultiFilterControl(supportedFilterTypes, useLongDate, addSpecialDateButton)
		{
			Name = name,
			IsRequired = isRequired,
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, LayoutItemLabelPosition.Left, isNew, helpText);
		CommonEventHandlers.AddIsRequiredBinding(dateMultiFilterControl, layoutItemWithRequired);
		layoutItemWithRequired.Content = dateMultiFilterControl;
		AddInCollections(dateMultiFilterControl, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(dateMultiFilterControl);
		return dateMultiFilterControl;
	}

	public MemoEditControl AddMemoControl(string name, string caption, short length = 255, bool isRequired = false, bool storeValue = false, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		return AddMemoControl(null, name, caption, length, isRequired, storeValue, horizontalAlignment, captionForStoreValue, isNew, helpText);
	}

	public MemoEditControl AddMemoControl(LayoutGroup container, string name, string caption, short length = 255, bool isRequired = false, bool storeValue = false, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, LayoutItemLabelPosition.Left, isNew, helpText);
		MemoEditControl memoEditControl = new MemoEditControl
		{
			Name = name.ToUpper(),
			IsRequired = isRequired,
			MaxLength = length,
			Owner = this,
			DropDownCaption = caption,
			MinWidth = 32.0,
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		CommonEventHandlers.AddIsRequiredBinding(memoEditControl, layoutItemWithRequired);
		if (horizontalAlignment != HorizontalAlignment.Stretch)
		{
			memoEditControl.Width = SizeCalculator.WidthForString(length);
		}
		layoutItemWithRequired.Content = memoEditControl;
		AddInCollections(memoEditControl, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(memoEditControl);
		return memoEditControl;
	}

	public ComboBoxEditExt AddComboBoxControl(string name, string caption, short length, IEnumerable<string> itemsSource, bool isRequired = false, bool storeValue = false, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		return AddComboBoxControl(null, name, caption, length, itemsSource, isRequired, storeValue, horizontalAlignment, captionForStoreValue, isNew, helpText);
	}

	public ComboBoxEditExt AddComboBoxControl(LayoutGroup container, string name, string caption, short length, IEnumerable<string> itemsSource, bool isRequired = false, bool storeValue = false, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, string captionForStoreValue = "", bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, LayoutItemLabelPosition.Left, isNew, helpText);
		ComboBoxEditExt comboBoxEditExt = new ComboBoxEditExt
		{
			Name = name.ToUpper(),
			IsRequired = isRequired,
			MaxLength = length,
			MinWidth = 5.0,
			Text = "",
			ItemsSource = itemsSource,
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		CommonEventHandlers.AddIsRequiredBinding(comboBoxEditExt, layoutItemWithRequired);
		if (horizontalAlignment != HorizontalAlignment.Stretch)
		{
			comboBoxEditExt.Width = SizeCalculator.WidthForString(length);
		}
		layoutItemWithRequired.Content = comboBoxEditExt;
		AddInCollections(comboBoxEditExt, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(comboBoxEditExt);
		return comboBoxEditExt;
	}

	public ComboBoxEditExt AddComboBoxControl(LayoutGroup container, string name, string caption, short length, IList itemsSource, string displayMember, string valueMember, bool isRequired = false, bool storeValue = false, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, string captionForStoreValue = "", LayoutItemLabelPosition labelPosition = LayoutItemLabelPosition.Left, bool isNew = false, string helpText = "")
	{
		CheckControlExistence(name);
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, caption, HorizontalAlignment.Left, labelPosition, isNew, helpText);
		ComboBoxEditExt comboBoxEditExt = new ComboBoxEditExt
		{
			Name = name.ToUpper(),
			IsRequired = isRequired,
			MaxLength = length,
			MinWidth = 5.0,
			Text = "",
			ItemsSource = itemsSource,
			DisplayMember = displayMember,
			ValueMember = valueMember,
			SkipValidationIfReadOnly = SkipReadOnlyControlsValidation
		};
		CommonEventHandlers.AddIsRequiredBinding(comboBoxEditExt, layoutItemWithRequired);
		if (horizontalAlignment != HorizontalAlignment.Stretch)
		{
			comboBoxEditExt.Width = SizeCalculator.WidthForString(length);
		}
		layoutItemWithRequired.Content = comboBoxEditExt;
		AddInCollections(comboBoxEditExt, caption, layoutItemWithRequired, storeValue, captionForStoreValue, container);
		TabValidationHelper.RegisterControl(comboBoxEditExt);
		return comboBoxEditExt;
	}

	public void AddStoreValuesButton(LayoutGroup container)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrWhiteSpace(Name))
		{
			throw new InvalidOperationException("Dialog name is empty");
		}
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, "");
		layoutItemWithRequired.HorizontalAlignment = HorizontalAlignment.Right;
		_storeValuesButton = new ToggleButton
		{
			UseLayoutRounding = true,
			IsTabStop = false
		};
		layoutItemWithRequired.Content = _storeValuesButton;
		_storeValuesButton.ToolTip = ArmSoft.SME.Common.Resources.SaveValuesTooltip;
		_storeValuesButton.Click += new SafeEventHandler<RoutedEventArgs>(StoreValuesButton_Click).Handler;
		ImageSource image = ImageService.GetImage("save_16.svg");
		DXImage element = new DXImage
		{
			Source = image,
			Width = 16.0,
			Height = 16.0
		};
		StackPanel stackPanel = new StackPanel
		{
			Orientation = Orientation.Horizontal,
			Margin = new Thickness(-7.0, 0.0, -7.0, 0.0)
		};
		stackPanel.Children.Add(element);
		_storeValuesButton.Content = stackPanel;
		LayoutItemWithRequired layoutItemWithRequired2 = AddLayoutItem(container, "");
		layoutItemWithRequired2.HorizontalAlignment = HorizontalAlignment.Right;
		_selectFieldsToSaveButton = new Button
		{
			UseLayoutRounding = true,
			IsTabStop = false
		};
		layoutItemWithRequired2.Content = _selectFieldsToSaveButton;
		_selectFieldsToSaveButton.ToolTip = ArmSoft.SME.Common.Resources.SelectFieldsToSave;
		_selectFieldsToSaveButton.Click += new SafeEventHandler<RoutedEventArgs>(SelectFieldsToSaveButton_Click).Handler;
		image = ImageService.GetImage("list_check_16.svg");
		element = new DXImage
		{
			Source = image,
			Width = 16.0,
			Height = 16.0
		};
		stackPanel = new StackPanel
		{
			Orientation = Orientation.Horizontal,
			Margin = new Thickness(-7.0, 0.0, -7.0, 0.0)
		};
		stackPanel.Children.Add(element);
		_selectFieldsToSaveButton.Content = stackPanel;
		LayoutItemWithRequired layoutItemWithRequired3 = AddLayoutItem(container, "");
		layoutItemWithRequired3.HorizontalAlignment = HorizontalAlignment.Right;
		_clearValuesButton = new Button
		{
			UseLayoutRounding = true,
			IsTabStop = false
		};
		layoutItemWithRequired3.Content = _clearValuesButton;
		_clearValuesButton.ToolTip = ArmSoft.SME.Common.Resources.ClearValuesTooltip;
		_clearValuesButton.Click += new SafeEventHandler<RoutedEventArgs>(ClearValuesButton_Click).Handler;
		image = ImageService.GetImage("eraser_16.svg");
		element = new DXImage
		{
			Source = image,
			Width = 16.0,
			Height = 16.0
		};
		stackPanel = new StackPanel
		{
			Orientation = Orientation.Horizontal,
			Margin = new Thickness(-7.0, 0.0, -7.0, 0.0)
		};
		stackPanel.Children.Add(element);
		_clearValuesButton.Content = stackPanel;
		AddOptionsButtonIfNeeded();
	}

	private void AddOptionsButtonIfNeeded()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (AllowParameterTemplates)
		{
			InitializeOptionsPopupMenu();
			_barButtonSaveAs = new BarButtonItem
			{
				Content = ArmSoft.SME.Common.Resources.SaveAs,
				Glyph = ImageService.GetImage("save_as_16")
			};
			_barButtonSaveAs.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonSaveAs_ItemClick).Handler;
			_barManager.Items.Add(_barButtonSaveAs);
			_barButtonManageTemplates = new BarButtonItem
			{
				Content = ArmSoft.SME.Common.Resources.Edit
			};
			_barButtonManageTemplates.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonEditDeleteTemplates_ItemClick).Handler;
			_barManager.Items.Add(_barButtonManageTemplates);
			_barSubItemAvailableTemplates = new BarSubItem
			{
				Content = ArmSoft.SME.Common.Resources.Download
			};
			_barManager.Items.Add(_barSubItemAvailableTemplates);
			_availableTemplates = new List<BarButtonItem>();
			_dialogValueTemplates = Settings.DialogService.GetTemplates(Name, forCurrentUser: true);
			_optionsPopupMenu.ItemLinks.Add(_barButtonSaveAs.CreateLink());
			_barButtonManageTemplatesItemLink = _barButtonManageTemplates.CreateLink();
			_optionsPopupMenu.ItemLinks.Add(_barButtonManageTemplatesItemLink);
			_barSubItemAvailableTemplatesItemLink = _barSubItemAvailableTemplates.CreateLink();
			_optionsPopupMenu.ItemLinks.Add(_barSubItemAvailableTemplatesItemLink);
			InnerRefreshOptionsPopupMenu();
		}
	}

	protected void InitializeOptionsPopupMenu()
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		_optionsPopupMenu = new PopupMenu();
		LayoutGroup container = ButtonsContainer();
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, "");
		layoutItemWithRequired.HorizontalAlignment = HorizontalAlignment.Right;
		_optionsButton = new Button
		{
			UseLayoutRounding = true,
			IsTabStop = false,
			ToolTip = ArmSoft.SME.Common.Resources.Options + HotKeysSE.Ctrl_O.CaptionWithBrackets
		};
		_optionsButton.Click += new SafeEventHandler<RoutedEventArgs>(OptionsButton_Click).Handler;
		layoutItemWithRequired.Content = _optionsButton;
		DXImage element = new DXImage
		{
			Source = ImageService.GetImage("options_16.svg"),
			Width = 16.0,
			Height = 16.0
		};
		StackPanel stackPanel = new StackPanel
		{
			Orientation = Orientation.Horizontal,
			Margin = new Thickness(-7.0, 0.0, -7.0, 0.0)
		};
		stackPanel.Children.Add(element);
		_optionsButton.Content = stackPanel;
	}

	private protected virtual void RefreshOptionsPopupMenu()
	{
		if (AllowParameterTemplates)
		{
			InnerRefreshOptionsPopupMenu();
		}
	}

	private void InnerRefreshOptionsPopupMenu()
	{
		_barButtonManageTemplatesItemLink.IsVisible = false;
		_barSubItemAvailableTemplatesItemLink.IsVisible = false;
		if (_dialogValueTemplates.Count == 0)
		{
			return;
		}
		_barButtonManageTemplatesItemLink.IsVisible = true;
		_barSubItemAvailableTemplatesItemLink.IsVisible = true;
		_barSubItemAvailableTemplates.ItemLinks.Clear();
		foreach (BarButtonItem availableTemplate in _availableTemplates)
		{
			availableTemplate.Tag = null;
		}
		int num = 0;
		foreach (DialogValuesTemplate dialogValueTemplate in _dialogValueTemplates)
		{
			if (num < _availableTemplates.Count)
			{
				_availableTemplates[num].Content = dialogValueTemplate.Caption;
				_availableTemplates[num].Tag = dialogValueTemplate.Id;
			}
			else
			{
				BarButtonItem barButtonItem = new BarButtonItem
				{
					Content = dialogValueTemplate.Caption,
					Tag = dialogValueTemplate.Id
				};
				barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ApplyTemplate_ItemClick).Handler;
				_availableTemplates.Add(barButtonItem);
			}
			_barSubItemAvailableTemplates.ItemLinks.Add(_availableTemplates[num].CreateLink());
			num++;
		}
	}

	private void ApplyTemplate_ItemClick(object sender, ItemClickEventArgs e)
	{
		DialogValuesTemplate dialogValuesTemplate = _dialogValueTemplates.FirstOrDefault((DialogValuesTemplate el) => el.Id == (int)e.Item.Tag);
		if (dialogValuesTemplate?.Values != null)
		{
			OnStoreDefaultValues();
			InnerLoadValues(dialogValuesTemplate.Values);
			InnerOnLoadActualValues(notCheckStoreFlag: true);
			if (Mode == DialogWindowMode.None)
			{
				Title = string.Format(ArmSoft.SME.Common.Resources.CaptionWithHotKey, Caption, dialogValuesTemplate.Caption);
			}
		}
	}

	private void BarButtonSaveAs_ItemClick(object sender, ItemClickEventArgs e)
	{
		Dictionary<string, string> valuesToStore = GetValuesToStore(checkStoreFlag: false);
		DialogWindow dialogWindow = new DialogWindow
		{
			Title = ArmSoft.SME.Common.Resources.SaveAs,
			Mode = DialogWindowMode.NewMode
		};
		dialogWindow.AddStringControl("Caption", ArmSoft.SME.Common.Resources.Name, 100, isRequired: true);
		if (dialogWindow.ShowDialog().Value)
		{
			DialogValuesTemplate template = new DialogValuesTemplate
			{
				Caption = dialogWindow["Caption"].ToString(),
				Values = valuesToStore
			};
			DialogValuesTemplate dialogValuesTemplate = _dialogValueTemplates.FirstOrDefault((DialogValuesTemplate el) => el.Caption == template.Caption);
			if (dialogValuesTemplate != null)
			{
				template.Id = dialogValuesTemplate.Id;
				dialogValuesTemplate.Values = template.Values;
			}
			else
			{
				_dialogValueTemplates.Add(template);
			}
			template.Id = Settings.DialogService.StoreTemplate(Name, template);
			RefreshOptionsPopupMenu();
		}
	}

	private void BarButtonEditDeleteTemplates_ItemClick(object sender, ItemClickEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Title = ArmSoft.SME.Common.Resources.DialogValuesGroups
		};
		DocumentGridControl documentGridControl = dialogWindow.AddGridControl("DialogValuesGroups");
		documentGridControl.ButtonAdd.IsVisible = false;
		documentGridControl.ButtonDuplicate.IsVisible = false;
		documentGridControl.AllowDelete = true;
		documentGridControl.AllowAdd = false;
		documentGridControl.AllowDrop = false;
		documentGridControl.AllowDuplicate = false;
		documentGridControl.AllowEdit = true;
		documentGridControl.ItemsSource = _dialogValueTemplates;
		List<DialogValuesTemplate> list = new List<DialogValuesTemplate>();
		foreach (DialogValuesTemplate dialogValueTemplate in _dialogValueTemplates)
		{
			list.Add(dialogValueTemplate.Clone() as DialogValuesTemplate);
		}
		DocumentGridColumn documentGridColumn = documentGridControl.AddStringColumn("Caption", ArmSoft.SME.Common.Resources.Name, 50);
		documentGridColumn.AllowMoving = DefaultBoolean.False;
		documentGridColumn.FieldName = "Caption";
		documentGridColumn.AllowEditing = DefaultBoolean.True;
		documentGridColumn.ShowInColumnChooser = false;
		if (dialogWindow.ShowDialog().Value)
		{
			foreach (DialogValuesTemplate oldTemplate in list)
			{
				DialogValuesTemplate dialogValuesTemplate = _dialogValueTemplates.FirstOrDefault((DialogValuesTemplate el) => el.Id == oldTemplate.Id);
				if (dialogValuesTemplate != null)
				{
					oldTemplate.Caption = dialogValuesTemplate.Caption;
				}
				else
				{
					oldTemplate.Values = null;
				}
			}
			Settings.DialogService.UpdateTemplates(Name, list);
			RefreshOptionsPopupMenu();
		}
		else
		{
			_dialogValueTemplates = list;
		}
	}

	private void OptionsButton_Click(object sender, RoutedEventArgs e)
	{
		OptionsButtonClick();
	}

	private void OptionsButtonClick()
	{
		try
		{
			_optionsPopupMenu.Placement = PlacementMode.Bottom;
			_optionsPopupMenu.PlacementTarget = _optionsButton;
			_optionsPopupMenu.IsOpen = true;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public void AddSpecialDateButton(LayoutGroup container, DateEditControl dateControl, DateEditControl periodEndControl = null)
	{
		if (string.IsNullOrWhiteSpace(Name))
		{
			throw new InvalidOperationException("Dialog name is empty");
		}
		LayoutItemWithRequired layoutItemWithRequired = AddLayoutItem(container, "");
		layoutItemWithRequired.HorizontalContentAlignment = HorizontalAlignment.Left;
		layoutItemWithRequired.Margin = new Thickness(15.0, 0.0, 0.0, 0.0);
		SpecialDateButton specialDateButton = new SpecialDateButton
		{
			Name = "SpecialDate_" + dateControl.Name,
			DateControl = dateControl,
			DatePeriodEndControl = periodEndControl,
			ToolTip = ArmSoft.SME.Common.Resources.Dates + " (Alt+D)"
		};
		dateControl.ToolTip = ArmSoft.SME.Common.Resources.SpecialDateHotKeyMsg;
		if (periodEndControl != null)
		{
			periodEndControl.ToolTip = ArmSoft.SME.Common.Resources.SpecialDateHotKeyMsg;
		}
		specialDateButton.IsTabStop = false;
		layoutItemWithRequired.Content = specialDateButton;
		AddInCollections(specialDateButton, "", layoutItemWithRequired, storeValue: false, null, container);
	}

	public LayoutGroupExt AddHorizontalLayoutGroup(LayoutGroup container = null, LayoutGroupView view = LayoutGroupView.Group, string header = "", double itemSpace = 4.0)
	{
		LayoutGroupExt layoutGroupExt = new LayoutGroupExt
		{
			View = view,
			Orientation = Orientation.Horizontal,
			ItemSpace = itemSpace
		};
		if (!string.IsNullOrWhiteSpace(header))
		{
			layoutGroupExt.Header = header;
		}
		if (container == null)
		{
			_layoutControl.Children.Add(layoutGroupExt);
		}
		else
		{
			container.Children.Add(layoutGroupExt);
		}
		return layoutGroupExt;
	}

	public LayoutGroupExt AddVerticalLayoutGroup(LayoutGroup container = null, LayoutGroupView view = LayoutGroupView.Group, string header = "")
	{
		LayoutGroupExt layoutGroupExt = new LayoutGroupExt
		{
			View = view,
			Orientation = Orientation.Vertical
		};
		if (!string.IsNullOrWhiteSpace(header))
		{
			layoutGroupExt.Header = header;
		}
		if (container == null)
		{
			_layoutControl.Children.Add(layoutGroupExt);
		}
		else
		{
			container.Children.Add(layoutGroupExt);
		}
		return layoutGroupExt;
	}

	public Control GetUIElement(string name)
	{
		if (Controls.ContainsKey(name.ToUpper()))
		{
			return Controls[name.ToUpper()].Control;
		}
		return null;
	}

	protected void RemoveUIElement(string name)
	{
		name = name.ToUpper();
		Controls.Remove(name);
		StoredValues.Remove(name);
	}

	protected internal bool HasControls()
	{
		return Controls.Count > 0;
	}

	protected internal void CheckControlExistence(string name)
	{
		if (Controls.ContainsKey(name.ToUpper()))
		{
			throw new InvalidOperationException("Control \"" + name + "\" already exists.");
		}
	}

	public DXTabControl AddTabControl(LayoutGroup container)
	{
		DXTabControl dXTabControl = new DXTabControl();
		if (container == null)
		{
			_layoutControl.Children.Add(dXTabControl);
		}
		else
		{
			container.Children.Add(dXTabControl);
		}
		return dXTabControl;
	}

	public LayoutGroup AddTabedGroup(DXTabControl tc, string header)
	{
		DXTabItem dXTabItem = new DXTabItem();
		tc.Items.Add(dXTabItem);
		dXTabItem.Header = header;
		if (string.IsNullOrWhiteSpace(dXTabItem.Name))
		{
			string name = Settings.ConvertCaptionToControlName("Tab", header);
			dXTabItem.Name = name;
		}
		dXTabItem.HeaderTemplate = (DataTemplate)Settings.GenericResources["TabHeaderDialog"];
		dXTabItem.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(OnKeyDownEnterMoveNextTabItem).Handler;
		LayoutControl layoutControl = new LayoutControl
		{
			Margin = (Settings.IsWin11Theme ? new Thickness(-8.0, -8.0, -8.0, -8.0) : new Thickness(-15.0, -15.0, -15.0, -15.0)),
			Orientation = Orientation.Vertical
		};
		layoutControl.Name = dXTabItem.Name;
		dXTabItem.Content = layoutControl;
		_tabControls.Add(dXTabItem);
		return layoutControl;
	}

	public DXTabItem AddTabItem(DXTabControl tc, string header)
	{
		DXTabItem dXTabItem = new DXTabItem();
		tc.Items.Add(dXTabItem);
		dXTabItem.Header = header;
		dXTabItem.HeaderTemplate = (DataTemplate)Settings.GenericResources["TabHeaderDialog"];
		dXTabItem.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(OnKeyDownEnterMoveNextTabItem).Handler;
		LayoutControl content = new LayoutControl
		{
			Margin = (Settings.IsWin11Theme ? new Thickness(-8.0, -8.0, -8.0, -8.0) : new Thickness(-15.0, -15.0, -15.0, -15.0)),
			Orientation = Orientation.Vertical
		};
		dXTabItem.Content = content;
		return dXTabItem;
	}

	public static void BindWidth(LayoutGroup lgDestination, LayoutGroup lgSource)
	{
		Binding binding = new Binding("ActualWidth")
		{
			Source = lgSource
		};
		lgDestination.SetBinding(FrameworkElement.MinWidthProperty, binding);
	}

	public static void BindHeight(LayoutGroup lgDestination, LayoutGroup lgSource)
	{
		Binding binding = new Binding("ActualHeight")
		{
			Source = lgSource
		};
		lgDestination.SetBinding(FrameworkElement.MinHeightProperty, binding);
	}

	public void OnKeyDownEnterMoveNextTabItem(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected Obj, but got Unknown
		if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0)
		{
			DXTabItem dXTabItem = (DXTabItem)sender;
			if (!((LayoutGroup)dXTabItem.Content).IsKeyboardFocusWithin)
			{
				dXTabItem.MoveFocus(new TraversalRequest((FocusNavigationDirection)0));
				e.Handled = true;
			}
		}
	}

	public List<CallsRecordingHelper.ControlInfo> GetControlsInfoForRecord(bool withoutValues = false)
	{
		List<CallsRecordingHelper.ControlInfo> list = new List<CallsRecordingHelper.ControlInfo>();
		foreach (DXTabItem tabControl in _tabControls)
		{
			list.Add(new CallsRecordingHelper.ControlInfo(tabControl.Name, ((object)tabControl).GetType().Name, null));
		}
		foreach (ControlDescriptor value in Controls.Values)
		{
			object obj = null;
			if (!withoutValues)
			{
				try
				{
					obj = Value(value.Name);
					if (((object)value.Control).GetType().Name == "DocumentGridControl")
					{
						obj = ((DocumentGridControl)obj).ItemsSource;
					}
				}
				catch
				{
					obj = null;
				}
			}
			list.Add(new CallsRecordingHelper.ControlInfo(value.Control.Name, ((object)value.Control).GetType().Name, value.Caption, null, ControlsHelper.IsControlReadonly(value.Control), obj));
			if (!(((object)value.Control).GetType().Name == "DocumentGridControl"))
			{
				continue;
			}
			foreach (GridColumn column in ((DocumentGridControl)value.Control).Grid.Columns)
			{
				list.Add(new CallsRecordingHelper.ControlInfo(column.Name, ((object)column).GetType().Name, Convert.ToString(column.Header), value.Control.Name, column.ReadOnly || column.AllowEditing == DefaultBoolean.False, null));
			}
		}
		return list;
	}

	private object Value(string name)
	{
		if (Controls.TryGetValue(name.ToUpper(), out var value))
		{
			Control control = value.Control;
			if (control is DropDownControl dropDownControl)
			{
				return dropDownControl.Code;
			}
			if (control is MultiSelectDropDownControl multiSelectDropDownControl)
			{
				return multiSelectDropDownControl.SelectedString;
			}
			if (control is CheckEditExt checkEditExt)
			{
				return checkEditExt.IsChecked;
			}
			if (control is RadioButtonExt radioButtonExt)
			{
				return radioButtonExt.IsChecked;
			}
			if (control is DateEditControl dateEditControl)
			{
				return dateEditControl.EditValue;
			}
			if (control is TimeEditControl timeEditControl)
			{
				return timeEditControl.EditValue;
			}
			if (control is PopupCalcEditExt popupCalcEditExt)
			{
				return popupCalcEditExt.EditValue;
			}
			if (control is PasswordBoxEditExt passwordBoxEditExt)
			{
				return passwordBoxEditExt.Text;
			}
			if (control is ButtonEditExt buttonEditExt)
			{
				return buttonEditExt.Text;
			}
			if (control is TextEditExt textEditExt)
			{
				return textEditExt.Text;
			}
			if (control is SpinEditExt spinEditExt)
			{
				return spinEditExt.Value;
			}
			if (control is ComboBoxExt comboBoxExt)
			{
				return comboBoxExt.SelectedIndex;
			}
			if (control is DropDownCommentControl dropDownCommentControl)
			{
				return dropDownCommentControl.Text;
			}
			if (control is MonthYearControl monthYearControl)
			{
				return monthYearControl.EditValue;
			}
			if (control is ComboBoxEditExt comboBoxEditExt)
			{
				return comboBoxEditExt.Text;
			}
			if (control is MemoEditControl memoEditControl)
			{
				return memoEditControl.EditValue;
			}
			if (control is DocumentGridControl result)
			{
				return result;
			}
			return null;
		}
		return null;
	}

	private void Set_Value(string name, object value)
	{
		if (Controls.TryGetValue(name.ToUpper(), out var value2))
		{
			Control control = value2.Control;
			if (control is DropDownControl dropDownControl)
			{
				dropDownControl.Code = (string)value;
			}
			else if (control is MultiSelectDropDownControl multiSelectDropDownControl)
			{
				multiSelectDropDownControl.SelectedString = (string)value;
			}
			else if (control is CheckEditExt checkEditExt)
			{
				checkEditExt.IsChecked = (checkEditExt.IsThreeState ? ((bool?)value) : new bool?((bool)value));
			}
			else if (control is RadioButtonExt radioButtonExt)
			{
				radioButtonExt.IsChecked = (bool)value;
			}
			else if (control is DateEditControl dateEditControl)
			{
				dateEditControl.EditValue = (DateOnly?)value;
			}
			else if (control is TimeEditControl timeEditControl)
			{
				timeEditControl.EditValue = (string)value;
			}
			else if (control is PopupCalcEditExt popupCalcEditExt)
			{
				popupCalcEditExt.Value = (decimal)value;
			}
			else if (control is PasswordBoxEditExt passwordBoxEditExt)
			{
				passwordBoxEditExt.Text = (string)value;
			}
			else if (control is ButtonEditExt buttonEditExt)
			{
				buttonEditExt.Text = (string)value;
			}
			else if (control is TextEditExt textEditExt)
			{
				textEditExt.Text = (string)value;
			}
			else if (control is SpinEditExt spinEditExt)
			{
				spinEditExt.EditValue = (decimal)value;
			}
			else if (control is ComboBoxExt comboBoxExt)
			{
				comboBoxExt.SelectedIndex = (int)value;
			}
			else if (control is DropDownCommentControl dropDownCommentControl)
			{
				dropDownCommentControl.Text = (string)value;
			}
			else if (control is ComboBoxEditExt comboBoxEditExt)
			{
				comboBoxEditExt.Text = (string)value;
			}
			else if (control is MonthYearControl monthYearControl)
			{
				monthYearControl.EditValue = (DateTime?)value;
			}
			else if (control is MemoEditControl memoEditControl)
			{
				memoEditControl.EditValue = (string)value;
			}
			return;
		}
		throw new InvalidOperationException("Control \"" + name + "\" absent.");
	}

	protected void SetControlsReadonly(bool value)
	{
		foreach (KeyValuePair<string, ControlDescriptor> control in Controls)
		{
			SetControlReadonly(control.Value.Control, value);
		}
	}

	protected virtual void SetControlReadonly(Control control, bool value)
	{
		if (control is TextEditExt textEditExt)
		{
			textEditExt.IsReadOnly = value;
		}
		else if (control is ButtonEditExt buttonEditExt)
		{
			buttonEditExt.IsReadOnly = value;
		}
		else if (control is DropDownControl dropDownControl)
		{
			dropDownControl.IsReadOnly = value;
		}
		else if (control is DateEditControl dateEditControl)
		{
			dateEditControl.IsReadOnly = value;
		}
		else if (control is PopupCalcEditExt popupCalcEditExt)
		{
			popupCalcEditExt.IsReadOnly = value;
		}
		else if (control is CheckEditExt checkEditExt)
		{
			checkEditExt.IsReadOnly = value;
		}
		else if (control is MultiSelectDropDownControl multiSelectDropDownControl)
		{
			multiSelectDropDownControl.IsReadOnly = value;
		}
		else if (control is DropDownCommentControl dropDownCommentControl)
		{
			dropDownCommentControl.IsReadOnly = value;
		}
		else if (control is ComboBoxEditExt comboBoxEditExt)
		{
			comboBoxEditExt.IsReadOnly = value;
		}
		else if (control is MemoEditControl memoEditControl)
		{
			memoEditControl.IsReadOnly = value;
		}
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		OnValidationWindow(okPressed: true);
	}

	private void NextButton_Click(object sender, RoutedEventArgs e)
	{
		OnValidationWindow(okPressed: false);
	}

	private void StoreValuesButton_Click(object sender, RoutedEventArgs e)
	{
		StoreValuesButtonClick();
	}

	private void ClearValuesButton_Click(object sender, RoutedEventArgs e)
	{
		ClearValuesButtonClick();
	}

	private void StoreValuesButtonClick()
	{
		try
		{
			_selectFieldsToSaveButton.IsEnabled = _storeValuesButton.IsChecked.Value;
			Settings.DialogService.StoreValues(Name, new DialogValues
			{
				StoreValues = _storeValuesButton.IsChecked.Value,
				Values = null
			});
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ClearValuesButtonClick()
	{
		try
		{
			OnLoadDefaultValues();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected virtual void AddInCollections(Control control, string caption, LayoutItemWithRequired li, bool storeValue, string captionForStoreValue, LayoutGroup container = null)
	{
		try
		{
			if (SetFocusOnActivation == null)
			{
				SetFocusOnActivation = control;
			}
			ControlDescriptor value = new ControlDescriptor
			{
				Control = control,
				Caption = caption,
				LayoutItem = li,
				Name = control.Name
			};
			Controls.Add(control.Name, value);
			if (StoreValueDescriptor.Supported(control))
			{
				captionForStoreValue = (string.IsNullOrWhiteSpace(captionForStoreValue) ? li.Label.ToString() : captionForStoreValue);
				StoredValues.Add(control.Name, captionForStoreValue, storeValue);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected virtual void OnValidationWindow(bool okPressed, bool showValidationError = false)
	{
		foreach (ControlDescriptor value in Controls.Values)
		{
			try
			{
				if (!ControlsHelper.DoValidateAndSetFocus(value.Control, out var _, validateWithoutRequired: false, showValidationError))
				{
					return;
				}
			}
			catch (ArgumentOutOfRangeException)
			{
			}
		}
		DialogWindowValidationArgs dialogWindowValidationArgs = new DialogWindowValidationArgs();
		DialogValidation(dialogWindowValidationArgs);
		if (dialogWindowValidationArgs.isValid)
		{
			StoreValues();
			StoreSettings();
			if (okPressed)
			{
				DialogResult = true;
			}
			else
			{
				NextPressed?.Invoke(this, new EventArgs());
			}
			return;
		}
		if (!string.IsNullOrEmpty(dialogWindowValidationArgs.ErrorContent))
		{
			MessageBox.Show(dialogWindowValidationArgs.ErrorContent, ArmSoft.SME.Common.Resources.ErrorWord, MessageBoxButton.OK, MessageBoxImage.Hand);
		}
		if (dialogWindowValidationArgs.SetFocusWhenInvalidSave != null)
		{
			ControlsHelper.SetFocus(dialogWindowValidationArgs.SetFocusWhenInvalidSave);
		}
	}

	protected virtual void DialogValidation(DialogWindowValidationArgs args)
	{
		WindowValidation?.Invoke(this, args);
	}

	protected void LoadValues()
	{
		if (AllowStoreValues)
		{
			DialogValues values = Settings.DialogService.GetValues(Name);
			bool storeValues = values.StoreValues;
			Dictionary<string, string> values2 = values.Values;
			_storeValuesButton.IsChecked = storeValues;
			_selectFieldsToSaveButton.IsEnabled = storeValues;
			OnStoreDefaultValues();
			if ((values2 != null) & storeValues)
			{
				InnerLoadValues(values2);
				OnLoadActualValues();
			}
		}
	}

	private void InnerLoadValues(Dictionary<string, string> valuesToLoad)
	{
		if (this is DataViewParametersDialogWindow { ExtendedView: not null } dataViewParametersDialogWindow)
		{
			StoreValueDescriptor item = StoredValues.GetItem(dataViewParametersDialogWindow.ExtendedView.Name);
			if (item != null)
			{
				item.ActualValue = item.DefaultValue;
				if (valuesToLoad.TryGetValue("StoreFlag_" + item.Name, out var value))
				{
					item.ActualStoreFlag = Convert.ToString(value) == "1";
				}
				if (valuesToLoad.TryGetValue(item.Name, out value))
				{
					item.ActualValue = value;
				}
				if (item.ActualStoreFlag)
				{
					StoreValueDescriptor.SetValue(dataViewParametersDialogWindow.ExtendedView, item.ActualValue);
				}
			}
		}
		foreach (StoreValueDescriptor storedValue in StoredValues)
		{
			string name = storedValue.Name;
			storedValue.ActualValue = storedValue.DefaultValue;
			if (valuesToLoad.TryGetValue("StoreFlag_" + name, out var value2))
			{
				storedValue.ActualStoreFlag = Convert.ToString(value2) == "1";
			}
			if (valuesToLoad.TryGetValue(name, out value2))
			{
				storedValue.ActualValue = value2;
			}
		}
	}

	private void StoreValues()
	{
		if (!AllowStoreValues)
		{
			return;
		}
		Dictionary<string, string> valuesToStore = GetValuesToStore(checkStoreFlag: true);
		try
		{
			Settings.DialogService.StoreValues(Name, new DialogValues
			{
				StoreValues = _storeValuesButton.IsChecked.Value,
				Values = valuesToStore
			});
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public virtual void OnLoadActualValues()
	{
		InnerOnLoadActualValues(notCheckStoreFlag: false);
	}

	private void InnerOnLoadActualValues(bool notCheckStoreFlag)
	{
		int i = 0;
		for (int num = StoredValues.Count - 1; i <= num; i++)
		{
			StoreValueDescriptor storeValueDescriptor = StoredValues[i];
			if ((storeValueDescriptor.ActualStoreFlag | notCheckStoreFlag) && Controls.TryGetValue(storeValueDescriptor.Name, out var value) && StoreValueDescriptor.Supported(value.Control))
			{
				StoreValueDescriptor.SetValue(value.Control, storeValueDescriptor.ActualValue);
			}
		}
	}

	private Dictionary<string, string> GetValuesToStore(bool checkStoreFlag)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		OnStoreActualValues();
		foreach (StoreValueDescriptor storedValue in StoredValues)
		{
			string name = storedValue.Name;
			if (checkStoreFlag && storedValue.DefaultStoreFlag != storedValue.ActualStoreFlag)
			{
				dictionary.Add("StoreFlag_" + name, storedValue.ActualStoreFlag ? "1" : "0");
			}
			if (storedValue.ActualStoreFlag || !checkStoreFlag)
			{
				dictionary.Add(name, storedValue.ActualValue);
			}
		}
		return dictionary;
	}

	public virtual void OnLoadDefaultValues()
	{
		foreach (StoreValueDescriptor storedValue in StoredValues)
		{
			if (Controls.TryGetValue(storedValue.Name, out var value) && StoreValueDescriptor.Supported(value.Control))
			{
				StoreValueDescriptor.SetValue(value.Control, storedValue.DefaultValue);
			}
		}
	}

	public virtual void OnStoreActualValues()
	{
		int i = 0;
		for (int num = StoredValues.Count - 1; i <= num; i++)
		{
			StoreValueDescriptor storeValueDescriptor = StoredValues[i];
			if (Controls.TryGetValue(storeValueDescriptor.Name, out var value) && StoreValueDescriptor.Supported(value.Control))
			{
				storeValueDescriptor.ActualValue = StoreValueDescriptor.GetValue(value.Control);
			}
		}
	}

	public virtual void OnStoreDefaultValues()
	{
		int i = 0;
		for (int num = StoredValues.Count - 1; i <= num; i++)
		{
			StoreValueDescriptor storeValueDescriptor = StoredValues[i];
			if (Controls.TryGetValue(storeValueDescriptor.Name, out var value) && StoreValueDescriptor.Supported(value.Control))
			{
				storeValueDescriptor.DefaultValue = StoreValueDescriptor.GetValue(value.Control);
			}
		}
	}

	private void DialogWindow_Activated(object sender, EventArgs e)
	{
		if (_firstActivation)
		{
			ControlsHelper.SetFocus(SetFocusOnActivation);
			WindowActivated?.Invoke(this);
			_firstActivation = false;
		}
	}

	private void DialogWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Invalid comparison between Unknown and I4
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Invalid comparison between Unknown and I4
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.Key == 93 && (int)Keyboard.Modifiers == 2)
			{
				e.Handled = true;
				Close();
			}
			else if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 2)
			{
				e.Handled = true;
				if (Mode == DialogWindowMode.ViewMode)
				{
					DialogResult = true;
				}
				else
				{
					OnValidationWindow(Mode != DialogWindowMode.NewModeWithNext && Mode != DialogWindowMode.CreateCopyModeWithNext);
				}
			}
			else if ((int)e.Key == 62 && (int)Keyboard.Modifiers == 2)
			{
				if (AllowStoreValues)
				{
					_storeValuesButton.IsChecked = !_storeValuesButton.IsChecked;
					StoreValuesButtonClick();
					e.Handled = true;
				}
			}
			else if ((int)e.Key == 47 && (int)Keyboard.Modifiers == 2)
			{
				if (AllowStoreValues)
				{
					ClearValuesButtonClick();
					e.Handled = true;
				}
			}
			else if ((int)e.Key == 90)
			{
				e.Handled = true;
				Settings.HostingEnvironment.ShowHelp(HelpId);
			}
			else if (HotKeys.WithModifier.Control_O.Matches(this, e) && _optionsButton != null)
			{
				OptionsButtonClick();
				e.Handled = true;
			}
			else if (HotKeys.WithModifier.Alt_Shift_J.Matches(this, e))
			{
				string tempFileName = Settings.GetTempFileName(".json");
				CallsRecordingHelper.ExportDialogWindow(this, tempFileName);
				CommonEventHandlers.CopyUnicodeTextToClipboard(tempFileName);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected LayoutItemWithRequired AddLayoutItem(LayoutGroup container, string caption, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, LayoutItemLabelPosition labelPosition = LayoutItemLabelPosition.Left, bool isNew = false, string helpText = "")
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		LayoutItemWithRequired layoutItemWithRequired = new LayoutItemWithRequired(isNew)
		{
			Label = caption,
			LabelPosition = labelPosition,
			HorizontalAlignment = horizontalAlignment
		};
		if (labelPosition == LayoutItemLabelPosition.Top)
		{
			layoutItemWithRequired.LabelTemplate = (DataTemplate)Settings.GenericResources["LayoutItemRequiredTop"];
		}
		LayoutGroup layoutGroup = null;
		if (!string.IsNullOrWhiteSpace(helpText))
		{
			layoutGroup = new LayoutGroup
			{
				Orientation = Orientation.Horizontal
			};
			DXImage element = new DXImage
			{
				Margin = new Thickness(10.0, 0.0, 0.0, 0.0),
				Source = ImageService.GetImage("help_16.svg"),
				Width = 16.0,
				Height = 16.0,
				ToolTip = helpText
			};
			layoutGroup.Children.Add(layoutItemWithRequired);
			layoutGroup.Children.Add(element);
		}
		if (container == null)
		{
			if (layoutGroup == null)
			{
				_layoutControl.Children.Add(layoutItemWithRequired);
			}
			else
			{
				_layoutControl.Children.Add(layoutGroup);
			}
		}
		else if (layoutGroup == null)
		{
			container.Children.Add(layoutItemWithRequired);
		}
		else
		{
			container.Children.Add(layoutGroup);
		}
		return layoutItemWithRequired;
	}

	private void WriteToStatusBarForCurrentRow(object sender, string msg)
	{
		_txtStatus.Text = msg;
	}

	public void WriteToStatusBar(string msg)
	{
		_txtStatus.Text = msg;
	}

	private void OnGridEscapePressed(object sender, EventArgs e)
	{
		DialogResult = false;
	}

	private static void ShowSpecialDates(object sender, ExecutedRoutedEventArgs e)
	{
		if (!(sender is DateEditControl dateEditControl) || !(Window.GetWindow((DependencyObject)(object)dateEditControl) is DialogWindow dialogWindow))
		{
			return;
		}
		foreach (ControlDescriptor value in dialogWindow.Controls.Values)
		{
			if ((object)value.GetType() == typeof(SpecialDateButton))
			{
				SpecialDateButton specialDateButton = (SpecialDateButton)value.Control;
				if (specialDateButton.DateControl.Name == dateEditControl.Name || (specialDateButton.DatePeriodEndControl != null && specialDateButton.DatePeriodEndControl.Name == dateEditControl.Name))
				{
					specialDateButton.ShowDatesMenu();
					break;
				}
			}
		}
	}

	protected void RestoreSettings()
	{
		if (string.IsNullOrWhiteSpace(_registryKey))
		{
			return;
		}
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey($"{Settings.RegistryKey}\\{"Dialog Windows"}\\{_registryKey}", writable: false);
			if (registryKey != null)
			{
				RestoreSettings(registryKey);
			}
		}
		catch (Exception)
		{
		}
	}

	protected virtual void RestoreSettings(RegistryKey rk)
	{
	}

	protected void StoreSettings()
	{
		if (string.IsNullOrWhiteSpace(_registryKey))
		{
			return;
		}
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\Dialog Windows\\" + _registryKey);
			if (registryKey != null)
			{
				StoreSettings(registryKey);
			}
		}
		catch (Exception)
		{
		}
	}

	protected virtual void StoreSettings(RegistryKey rk)
	{
	}

	private void SelectFieldsToSaveButton_Click(object sender, RoutedEventArgs e)
	{
		new SelectDialogFieldsToSaveWindow(this).ShowDialog();
	}

	protected LayoutItemWithRequired GetLayoutItem(Control control)
	{
		if (!Controls.TryGetValue(control.Name, out var value))
		{
			return null;
		}
		return value.LayoutItem;
	}

	protected LayoutItemWithRequired GetLayoutItem(string name)
	{
		if (!Controls.TryGetValue(name, out var value))
		{
			return null;
		}
		return value.LayoutItem;
	}

	protected static void SelectCellInGrid(TableView tableView, DocumentGridControl grid, int rowHnd, string columnToSelect)
	{
		tableView.Grid.CurrentItem = tableView.Grid.GetRow(rowHnd);
		grid.Grid.CurrentColumn = tableView.Grid.Columns[columnToSelect];
		tableView.Focus();
		tableView.ShowEditor();
		((DispatcherObject)tableView).Dispatcher.Invoke((Action)(() =>
		{
			if (tableView.ActiveEditor != null)
			{
				tableView.ActiveEditor.SelectAll();
			}
		}), (DispatcherPriority)6);
	}
}
