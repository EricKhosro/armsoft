using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Validation;

namespace ArmSoft.SME.Core.UI.Controls;

public class MonthYearControl : DateEdit
{
	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(MonthYearControl));

	private readonly PropertyChangeNotifier _isReadOnlyChangeNotifier;

	private readonly PropertyChangeNotifier _isRequiredChangeNotifier;

	private bool _considerRequired;

	private bool _isFirstMonthDay;

	private readonly SpinButtonInfo _spinButtonInfo;

	internal bool SkipValidationIfReadOnly { get; set; }

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

	public bool IsFirstMonthDay
	{
		get
		{
			return _isFirstMonthDay;
		}
		set
		{
			_isFirstMonthDay = value;
			CalculateDay();
		}
	}

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

	public MonthYearControl()
	{
		IsRequired = false;
		Focusable = true;
		AllowNullInput = true;
		InvalidValueBehavior = InvalidValueBehavior.AllowLeaveEditor;
		_isFirstMonthDay = true;
		EditValue = null;
		Mask = "MMMM,yyyy";
		MaskUseAsDisplayFormat = true;
		CausesValidation = true;
		ValidateOnTextInput = false;
		ValidateOnEnterKeyPressed = true;
		_considerRequired = true;
		PopupContentTemplate = (ControlTemplate)ArmSoft.SME.Core.UI.Settings.GenericResources["DateEditCalendarMonth"];
		_spinButtonInfo = new SpinButtonInfo();
		Buttons.Add(_spinButtonInfo);
		_spinButtonInfo.SpinUpClick += new SafeEventHandler<RoutedEventArgs>(SpinButtonInfo_SpinUpClick).Handler;
		_spinButtonInfo.SpinDownClick += new SafeEventHandler<RoutedEventArgs>(SpinButtonInfo_SpinDownClick).Handler;
		_isRequiredChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsRequiredProperty);
		_isRequiredChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isReadOnlyChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, BaseEdit.IsReadOnlyProperty);
		_isReadOnlyChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MonthYearControl_EditValueChanged).Handler;
		KeyDown += new SafeEventHandler<KeyEventArgs>(MonthYearControl_KeyDown).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(MonthYearControl_PreviewKeyDown).Handler;
		PopupOpening += new SafeEventHandler<OpenPopupEventArgs>(MonthYearControl_PopupOpening).Handler;
		Validate += new SafeEventHandler<ValidationEventArgs>(MonthYearControl_Validate).Handler;
	}

	private void IsRequiredChanged(object sender, EventArgs e)
	{
		if (IsRequired && !IsReadOnly)
		{
			Background = ArmSoft.SME.Core.UI.Settings.RequiredBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.BackgroundProperty, typeof(MonthYearControl))).ResetValue((object)this);
		}
		if (IsReadOnly)
		{
			Foreground = ArmSoft.SME.Core.UI.Settings.ReadOnlyBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(MonthYearControl))).ResetValue((object)this);
		}
	}

	private void CalculateDay()
	{
		if (!(EditValue is DateTime dateTime))
		{
			return;
		}
		if (IsFirstMonthDay)
		{
			if (dateTime.Day != 1)
			{
				EditValue = new DateTime(dateTime.Year, dateTime.Month, 1);
			}
		}
		else if (dateTime.AddDays(1.0).Day != 1)
		{
			if (dateTime.Month == 12)
			{
				EditValue = new DateTime(dateTime.Year, 12, 31);
			}
			else
			{
				EditValue = new DateTime(dateTime.Year, dateTime.Month + 1, 1).AddDays(-1.0);
			}
		}
	}

	private void MonthYearControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		CalculateDay();
	}

	private void MonthYearControl_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected Obj, but got Unknown
		if ((int)e.Key == 6)
		{
			MoveFocus(new TraversalRequest((FocusNavigationDirection)0));
		}
	}

	private void MonthYearControl_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		if ((int)e.Key == 20 || (((int)e.Key == 26) & ((int)Keyboard.Modifiers == 2)))
		{
			ShowPopup();
		}
	}

	private void MonthYearControl_PopupOpening(object sender, OpenPopupEventArgs e)
	{
		if (IsReadOnly)
		{
			e.Cancel = true;
		}
	}

	private void MonthYearControl_Validate(object sender, ValidationEventArgs e)
	{
		if ((!SkipValidationIfReadOnly || !IsReadOnly) && IsRequired && _considerRequired && e.Value == null)
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
		}
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

	private void SpinButtonInfo_SpinUpClick(object sender, RoutedEventArgs e)
	{
		if (EditValue is DateTime dateTime)
		{
			EditValue = dateTime.AddMonths(1);
		}
	}

	private void SpinButtonInfo_SpinDownClick(object sender, RoutedEventArgs e)
	{
		if (EditValue is DateTime dateTime)
		{
			EditValue = dateTime.AddMonths(-1);
		}
	}
}
