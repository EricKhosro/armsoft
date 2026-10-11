using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.DateNavigator;
using DevExpress.Xpf.Editors.Popups;
using DevExpress.Xpf.Editors.Validation;

namespace ArmSoft.SME.Core.UI.Controls;

public class DateEditControl : PopupBaseEdit
{
	public delegate void ValidateEventHandler(object sender, DateEditValidationEventArgs e);

	public delegate void EditValueChangedEventHandler(object sender, EditValueChangedEventArgs e);

	private const string _shortDateMask = "[0123456789 ]{2}/[0123456789 ]{2}/[0123456789 ]{2}";

	private const string _longDateMask = "[0123456789 ]{2}/[0123456789 ]{2}/[0123456789 ]{4}";

	public const string ShortEmptyValue = "  /  /  ";

	public const string LongEmptyValue = "  /  /    ";

	private const int _shortFormatBeginYear = 50;

	private static readonly DateOnly _minShortDate = new DateOnly(1950, 1, 1);

	private static readonly DateOnly _maxShortDate = new DateOnly(2049, 12, 31);

	private static readonly DateOnly _minLongDate = new DateOnly(1900, 1, 1);

	private static readonly DateOnly _maxLongDate = new DateOnly(2079, 6, 6);

	private bool _considerRequired;

	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(DateEditControl));

	private bool _allowChangeValue;

	private DateNavigator _popupDateNavigator;

	private readonly PropertyChangeNotifier _isReadOnlyChangeNotifier;

	private readonly PropertyChangeNotifier _isRequiredChangeNotifier;

	private PropertyChangeNotifier _selectedDatesChangeNotifier;

	private DateOnly _selectedDate;

	internal bool SkipValidationIfReadOnly { get; set; }

	internal bool SuppressValidation { get; set; }

	public Visibility ControlVisibility
	{
		get
		{
			return ((Control)(object)Parent).Visibility;
		}
		set
		{
			((Control)(object)Parent).Visibility = value;
		}
	}

	public bool IsRequired
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsRequiredProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsRequiredProperty, (object)value);
		}
	}

	public virtual bool UseLongDate
	{
		get
		{
			return Mask == "[0123456789 ]{2}/[0123456789 ]{2}/[0123456789 ]{4}";
		}
		set
		{
			DateOnly? editValue = EditValue;
			Mask = (value ? "[0123456789 ]{2}/[0123456789 ]{2}/[0123456789 ]{4}" : "[0123456789 ]{2}/[0123456789 ]{2}/[0123456789 ]{2}");
			EditValue = editValue;
		}
	}

	public new DateOnly? EditValue
	{
		get
		{
			string value = base.EditValue?.ToString();
			if (IsAcceptable(value))
			{
				return StringToDate(value);
			}
			return null;
		}
		set
		{
			base.EditValue = DateToString(value);
		}
	}

	public bool IsCalledFromGrid { get; set; }

	public new event EditValueChangedEventHandler EditValueChanged;

	public new event ValidateEventHandler Validate;

	internal event ValidateEventHandler AfterValidate;

	public void ClearBaseEditValue()
	{
		base.EditValue = null;
	}

	private void DateEdit_EditValueChanging(object sender, EditValueChangingEventArgs e)
	{
		if (IsPopupOpen && !_allowChangeValue)
		{
			if (IsAcceptable(e.NewValue.ToString()))
			{
				_selectedDate = StringToDate(e.NewValue.ToString()) ?? DateTime.Today.ToDateOnly();
			}
			else
			{
				_selectedDate = DateTime.Today.ToDateOnly();
			}
			e.IsCancel = true;
			e.Handled = true;
		}
	}

	private void CodeEdit_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected Obj, but got Unknown
		if ((int)e.Key == 6)
		{
			((UIElement)sender).MoveFocus(new TraversalRequest((FocusNavigationDirection)0));
		}
	}

	private void CodeEdit_PopupOpening(object sender, OpenPopupEventArgs e)
	{
		if (IsReadOnly)
		{
			e.Cancel = true;
		}
	}

	private void DateEdit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		EditValueChanged?.Invoke(this, e);
	}

	private void DateEdit_Validate(object sender, ValidationEventArgs e)
	{
		if (SkipValidationIfReadOnly && IsReadOnly)
		{
			return;
		}
		DateOnly? dateOnly = e.Value as DateOnly?;
		bool flag;
		string text;
		if (e.Value == null)
		{
			flag = true;
			text = (UseLongDate ? "  /  /    " : "  /  /  ");
		}
		else
		{
			text = e.Value.ToString();
			flag = ((!UseLongDate) ? (text == "  /  /  ") : (text == "  /  /    "));
		}
		if (flag)
		{
			if (IsRequired && _considerRequired)
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
			}
			_allowChangeValue = true;
			SetEditValue(text);
			_allowChangeValue = false;
		}
		else
		{
			if (SuppressValidation)
			{
				return;
			}
			if (!IsAcceptable(text))
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.InvalidDate;
			}
			else
			{
				DateOnly? dateOnly2 = dateOnly;
				if (!dateOnly2.HasValue)
				{
					dateOnly = StringToDate(text);
				}
				string text2 = DateToString(dateOnly);
				if (text2 != text)
				{
					SetEditValue(text2);
				}
			}
		}
		if (!e.IsValid && AfterValidate == null)
		{
			return;
		}
		DateEditValidationEventArgs e2 = new DateEditValidationEventArgs(e.RoutedEvent, e.Source, dateOnly, e.Culture, e.UpdateSource)
		{
			IsValid = e.IsValid
		};
		if (e.IsValid && Validate != null)
		{
			Validate?.Invoke(this, e2);
			e.IsValid = e2.IsValid;
			e.ErrorContent = e2.ErrorContent;
			e.RoutedEvent = e2.RoutedEvent;
			e.Source = e2.Source;
		}
		if (!e.IsValid)
		{
			DateOnly? dateOnly2 = dateOnly;
			if (!dateOnly2.HasValue)
			{
				dateOnly = StringToDate(text);
			}
			SetEditValue(DateToString(dateOnly));
		}
		AfterValidate?.Invoke(this, e2);
	}

	private void SetEditValue(string value)
	{
		if (!SuppressValidation)
		{
			try
			{
				SuppressValidation = true;
				base.EditValue = value;
			}
			finally
			{
				SuppressValidation = false;
			}
		}
	}

	private bool ShiftDate(int unit, bool increment)
	{
		DoValidate();
		string text = base.EditValue.ToString();
		string text2 = null;
		if (string.IsNullOrWhiteSpace(text))
		{
			text2 = DateToString(DateTime.Today.ToDateOnly());
		}
		else if (text == (UseLongDate ? "  /  /    " : "  /  /  "))
		{
			text2 = DateToString(DateTime.Today.ToDateOnly());
		}
		else
		{
			if (HasValidationError)
			{
				return false;
			}
			if (IsAcceptable(text))
			{
				DateOnly tmpDate = StringToDate(text).Value;
				switch (unit)
				{
				case 1:
					tmpDate = tmpDate.AddDays(increment ? 1 : (-1));
					break;
				case 2:
					tmpDate = tmpDate.AddMonths(increment ? 1 : (-1));
					break;
				case 3:
					tmpDate = tmpDate.AddYears(increment ? 1 : (-1));
					break;
				}
				CheckMinMaxDate(ref tmpDate);
				text2 = DateToString(tmpDate);
			}
		}
		if (text2 == null)
		{
			return false;
		}
		try
		{
			SuppressValidation = true;
			base.EditValue = text2;
			if (Convert.ToString(base.EditValue) != text2)
			{
				Clear();
				base.EditValue = text2;
			}
		}
		finally
		{
			SuppressValidation = false;
		}
		return text2 == base.EditValue.ToString();
	}

	private bool IsAcceptable(string value)
	{
		try
		{
			StringToDate(value);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private DateOnly? StringToDate(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return null;
		}
		if (value == "  /  /  " || value == "  /  /    ")
		{
			return null;
		}
		int year;
		string text;
		if (UseLongDate)
		{
			text = value.Substring(6, 4).Trim();
			if (string.IsNullOrWhiteSpace(text))
			{
				year = DateTime.Today.Year;
			}
			else if (text.Length <= 2)
			{
				year = Convert.ToInt32(text);
				year = ((year < 50) ? (year + 2000) : (year + 1900));
			}
			else
			{
				year = Convert.ToInt32(text);
			}
		}
		else
		{
			text = value.Substring(6, 2);
			if (string.IsNullOrWhiteSpace(text))
			{
				year = DateTime.Today.Year;
			}
			else
			{
				year = Convert.ToInt32(text);
				year = ((year < 50) ? (year + 2000) : (year + 1900));
			}
		}
		text = value.Substring(3, 2);
		int num = ((!string.IsNullOrWhiteSpace(text)) ? Convert.ToInt32(text) : DateTime.Today.Month);
		int num2 = Convert.ToInt32(value.Substring(0, 2));
		if (num >= 1 && num <= 12)
		{
			int num3 = DateTime.DaysInMonth(year, num);
			if (num2 > num3)
			{
				num2 = num3;
			}
		}
		DateOnly tmpDate = new DateOnly(year, num, num2);
		CheckMinMaxDate(ref tmpDate);
		return tmpDate;
	}

	private string DateToString(DateOnly value)
	{
		if (UseLongDate)
		{
			return value.FormatDDMMYYYY();
		}
		return value.FormatDDMMYY();
	}

	private string DateToString(DateOnly? value)
	{
		if (value.HasValue)
		{
			return DateToString(value.Value);
		}
		if (!UseLongDate)
		{
			return "  /  /  ";
		}
		return "  /  /    ";
	}

	private void PopupDateNavigator_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		try
		{
			if ((int)Keyboard.Modifiers == 0 && (int)e.Key == 6)
			{
				_allowChangeValue = true;
				EditValue = _selectedDate;
				ClosePopup();
				_allowChangeValue = false;
				e.Handled = true;
			}
			else if ((int)Keyboard.Modifiers == 0 && (int)e.Key == 13)
			{
				CancelPopup();
			}
		}
		catch (Exception ex)
		{
			ArmSoft.SME.Core.UI.Settings.ErrorHandler(ex);
		}
	}

	private void PopupDateNavigator_Loaded(object sender, RoutedEventArgs e)
	{
		_popupDateNavigator = (DateNavigator)sender;
		_selectedDatesChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)_popupDateNavigator, DateNavigator.SelectedDatesProperty);
		_selectedDatesChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(SelectedDatesChanged).Handler;
		_popupDateNavigator.MouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(SetDateValue).Handler;
		_popupDateNavigator.KeyDown += new SafeEventHandler<KeyEventArgs>(PopupDateNavigator_KeyDown).Handler;
		if (DoValidate())
		{
			DateOnly? editValue = EditValue;
			if (editValue.HasValue)
			{
				_popupDateNavigator.FocusedDate = editValue.Value.ToDateTime();
				_selectedDate = editValue.Value;
			}
			else
			{
				_popupDateNavigator.FocusedDate = DateTime.Today;
				EditValue = _popupDateNavigator.FocusedDate.ToDateOnly();
			}
		}
		else
		{
			_popupDateNavigator.FocusedDate = DateTime.Today;
			EditValue = _popupDateNavigator.FocusedDate.ToDateOnly();
		}
		_popupDateNavigator.Focus();
	}

	private void SetPopupContent()
	{
		PopupContentTemplate = new ControlTemplate(typeof(PopupContentControl))
		{
			VisualTree = new FrameworkElementFactory(typeof(DateNavigator))
		};
		PopupContentTemplate.VisualTree.SetValue(DateNavigator.ShowWeekNumbersProperty, false);
		PopupContentTemplate.VisualTree.SetValue(DateNavigator.CalendarViewProperty, DateNavigatorCalendarView.Month);
		PopupContentTemplate.VisualTree.SetValue(DateNavigator.IsMultiSelectProperty, false);
		PopupContentTemplate.VisualTree.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(PopupDateNavigator_Loaded));
	}

	private void SelectedDatesChanged(object sender, EventArgs e)
	{
		DateOnly tmpDate = _popupDateNavigator.FocusedDate.ToDateOnly();
		CheckMinMaxDate(ref tmpDate);
		EditValue = tmpDate;
	}

	private void CheckMinMaxDate(ref DateOnly tmpDate)
	{
		if (!UseLongDate)
		{
			if (tmpDate < _minShortDate)
			{
				tmpDate = _minShortDate;
			}
			else if (tmpDate > _maxShortDate)
			{
				tmpDate = _maxShortDate;
			}
		}
		else if (tmpDate < _minLongDate)
		{
			tmpDate = _minLongDate;
		}
		else if (tmpDate > _maxLongDate)
		{
			tmpDate = _maxLongDate;
		}
	}

	private void CodeEdit_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		if ((int)e.Key == 20 || ((int)e.Key == 26 && (int)Keyboard.Modifiers == 2))
		{
			ShowPopup();
			e.Handled = true;
		}
	}

	public DateEditControl()
	{
		ShowSizeGrip = true;
		InvalidValueBehavior = InvalidValueBehavior.AllowLeaveEditor;
		Focusable = true;
		AllowNullInput = true;
		base.EditValue = "  /  /  ";
		CausesValidation = true;
		AllowSpinOnMouseWheel = false;
		MaskAutoComplete = AutoCompleteType.None;
		MaskIgnoreBlank = false;
		MaskPlaceHolder = ' ';
		MaskType = MaskType.Regular;
		Mask = "[0123456789 ]{2}/[0123456789 ]{2}/[0123456789 ]{2}";
		MaskUseAsDisplayFormat = true;
		ValidateOnEnterKeyPressed = true;
		ValidateOnTextInput = false;
		IsRequired = false;
		IsReadOnly = false;
		_considerRequired = true;
		FontFamily = ArmSoft.SME.Core.UI.Settings.FontFamily;
		FontSize = ArmSoft.SME.Core.UI.Settings.GetFontSize();
		SetPopupContent();
		_isRequiredChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsRequiredProperty);
		_isRequiredChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isReadOnlyChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, BaseEdit.IsReadOnlyProperty);
		_isReadOnlyChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		MouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(SelectAllValue).Handler;
		KeyDown += new SafeEventHandler<KeyEventArgs>(CodeEdit_KeyDown).Handler;
		base.Validate += new SafeEventHandler<ValidationEventArgs>(DateEdit_Validate).Handler;
		PopupOpening += new SafeEventHandler<OpenPopupEventArgs>(CodeEdit_PopupOpening).Handler;
		base.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DateEdit_EditValueChanged).Handler;
		EditValueChanging += new SafeEventHandler<EditValueChangingEventArgs>(DateEdit_EditValueChanging).Handler;
		Spin += new SafeEventHandler<SpinEventArgs>(CodeEdit_Spin).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(CodeEdit_PreviewKeyDown).Handler;
	}

	private void IsRequiredChanged(object sender, EventArgs e)
	{
		if (IsRequired && !IsReadOnly)
		{
			Background = ArmSoft.SME.Core.UI.Settings.RequiredBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.BackgroundProperty, typeof(PopupBaseEdit))).ResetValue((object)this);
		}
		if (IsReadOnly && !IsCalledFromGrid)
		{
			Foreground = ArmSoft.SME.Core.UI.Settings.ReadOnlyBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(PopupBaseEdit))).ResetValue((object)this);
		}
	}

	protected override void BeforePreviewKeyDown(KeyEventArgs e)
	{
		DateEditControl_PreviewKeyDown(this, e);
	}

	private void DateEditControl_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		if ((int)e.Key == 23 && (int)Keyboard.Modifiers == 2)
		{
			int caretIndex = CaretIndex;
			if (caretIndex <= 3)
			{
				CaretIndex = 0;
			}
			else if (caretIndex <= 6)
			{
				CaretIndex = 3;
			}
			else
			{
				CaretIndex = 6;
			}
			Select(CaretIndex, 1);
			e.Handled = true;
		}
		else if ((int)e.Key == 25 && (int)Keyboard.Modifiers == 2)
		{
			int caretIndex2 = CaretIndex;
			if (caretIndex2 <= 2)
			{
				CaretIndex = 3;
			}
			else if (caretIndex2 <= 5)
			{
				CaretIndex = 6;
			}
			else
			{
				CaretIndex = (UseLongDate ? 9 : 7);
			}
			Select(CaretIndex, 1);
			e.Handled = true;
		}
	}

	private void CodeEdit_Spin(object sender, SpinEventArgs e)
	{
		int caretIndex = CaretIndex;
		if (caretIndex >= 0 && caretIndex <= 2)
		{
			if (ShiftDate(1, e.IsSpinUp))
			{
				CaretIndex = 1;
				Select(CaretIndex, 1);
			}
		}
		else if (caretIndex >= 3 && caretIndex <= 5)
		{
			if (ShiftDate(2, e.IsSpinUp))
			{
				CaretIndex = 4;
				Select(CaretIndex, 1);
			}
		}
		else if (caretIndex > 5 && ShiftDate(3, e.IsSpinUp))
		{
			CaretIndex = (UseLongDate ? 9 : 7);
			Select(CaretIndex, 1);
		}
		e.Handled = true;
	}

	public bool DoValidateWithoutRequired()
	{
		bool flag = false;
		try
		{
			_considerRequired = false;
			return DoValidate();
		}
		finally
		{
			_considerRequired = true;
		}
	}

	private void SetDateValue(object sender, MouseButtonEventArgs e)
	{
		_allowChangeValue = true;
		EditValue = _selectedDate;
		ClosePopup();
		_allowChangeValue = false;
	}

	private void DateChanged(object sender, SelectionChangedEventArgs e)
	{
		string text = ((DatePicker)sender).Text;
		if (IsAcceptable(text))
		{
			DateOnly? dateOnly = StringToDate(text);
			if (dateOnly.HasValue)
			{
				_selectedDate = dateOnly.Value;
			}
		}
	}

	private void SelectAllValue(object sender, MouseButtonEventArgs e)
	{
		SelectAll();
	}
}
