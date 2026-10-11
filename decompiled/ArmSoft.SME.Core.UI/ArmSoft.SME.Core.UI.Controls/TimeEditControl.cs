using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Validation;

namespace ArmSoft.SME.Core.UI.Controls;

public class TimeEditControl : TextEdit
{
	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(TimeEditControl));

	private readonly PropertyChangeNotifier _isReadOnlyChangeNotifier;

	private readonly PropertyChangeNotifier _isRequiredChangeNotifier;

	private bool _considerRequired;

	private bool _suppressValidation;

	public const string ShortEmptyValue = "  :  ";

	private const string _longEmptyValue = "  :  :  ";

	private const string _shortTimeMask = "[0123456789 ]{2}:[0123456789 ]{2}";

	private const string _longTimeMask = "[0123456789 ]{2}:[0123456789 ]{2}:[0123456789 ]{2}";

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

	public virtual bool ShowSeconds
	{
		get
		{
			return Mask == "[0123456789 ]{2}:[0123456789 ]{2}:[0123456789 ]{2}";
		}
		set
		{
			object editValue = EditValue;
			Mask = (value ? "[0123456789 ]{2}:[0123456789 ]{2}:[0123456789 ]{2}" : "[0123456789 ]{2}:[0123456789 ]{2}");
			EditValue = editValue;
		}
	}

	public TimeSpan? TimeSpanValue => StringToTimeSpan((string)EditValue);

	public TimeEditControl()
	{
		IsRequired = false;
		Focusable = true;
		AllowNullInput = true;
		_considerRequired = true;
		InvalidValueBehavior = InvalidValueBehavior.AllowLeaveEditor;
		MaskUseAsDisplayFormat = true;
		CausesValidation = true;
		ValidateOnTextInput = false;
		ValidateOnEnterKeyPressed = true;
		MaskAutoComplete = AutoCompleteType.None;
		MaskType = MaskType.Regular;
		Mask = "[0123456789 ]{2}:[0123456789 ]{2}";
		AllowSpinOnMouseWheel = false;
		MaskIgnoreBlank = false;
		MaskPlaceHolder = ' ';
		MaskUseAsDisplayFormat = true;
		EditValue = (ShowSeconds ? "  :  :  " : "  :  ");
		_isRequiredChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsRequiredProperty);
		_isRequiredChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isReadOnlyChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, BaseEdit.IsReadOnlyProperty);
		_isReadOnlyChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		KeyDown += new SafeEventHandler<KeyEventArgs>(TextEditExt_KeyDown).Handler;
		Validate += new SafeEventHandler<ValidationEventArgs>(TextEditExt_Validate).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(PreviewKeyDownHandler).Handler;
		Spin += new SafeEventHandler<SpinEventArgs>(SpinHandler).Handler;
	}

	private void IsRequiredChanged(object sender, EventArgs e)
	{
		if (IsRequired && !IsReadOnly)
		{
			Background = ArmSoft.SME.Core.UI.Settings.RequiredBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.BackgroundProperty, typeof(TimeEditControl))).ResetValue((object)this);
		}
		if (IsReadOnly)
		{
			Foreground = ArmSoft.SME.Core.UI.Settings.ReadOnlyBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(TimeEditControl))).ResetValue((object)this);
		}
	}

	private void TextEditExt_KeyDown(object sender, KeyEventArgs e)
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

	private void TextEditExt_Validate(object sender, ValidationEventArgs e)
	{
		if (SkipValidationIfReadOnly && IsReadOnly)
		{
			return;
		}
		bool flag;
		string text;
		if (e.Value == null)
		{
			flag = true;
			text = (string)(EditValue = (ShowSeconds ? "  :  :  " : "  :  "));
		}
		else
		{
			text = ((!(e.Value is TimeSpan value)) ? e.Value.ToString() : TimeSpanToString(value));
			flag = ((!ShowSeconds) ? (text == "  :  ") : (text == "  :  :  "));
			if (string.IsNullOrWhiteSpace(text))
			{
				flag = true;
			}
		}
		if (_suppressValidation)
		{
			return;
		}
		if (flag)
		{
			if (IsRequired && _considerRequired)
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				EditValue = (ShowSeconds ? "  :  :  " : "  :  ");
			}
			else if (Convert.ToString(EditValue) != text)
			{
				_suppressValidation = true;
				EditValue = text;
				_suppressValidation = false;
			}
		}
		else if (!IsAcceptable(text))
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.InvalidTime;
		}
		else
		{
			string text3 = TimeSpanToString(StringToTimeSpan(text));
			if (text3 != text)
			{
				EditValue = text3;
			}
		}
	}

	private bool IsAcceptable(string value)
	{
		try
		{
			StringToTimeSpan(value);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private TimeSpan? StringToTimeSpan(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return null;
		}
		if (ShowSeconds && value == "  :  :  ")
		{
			return null;
		}
		if (!ShowSeconds && value == "  :  ")
		{
			return null;
		}
		int num = Convert.ToInt32(value.Left(2));
		if (num < 0)
		{
			num = 0;
		}
		if (num > 23)
		{
			num = 23;
		}
		string value2 = value.Substring(3, 2);
		int num2 = ((!string.IsNullOrWhiteSpace(value2)) ? Convert.ToInt32(value2) : 0);
		if (num2 < 0)
		{
			num2 = 0;
		}
		if (num2 > 59)
		{
			num2 = 59;
		}
		int num3;
		if (ShowSeconds)
		{
			value2 = value.Substring(6, 2);
			num3 = ((!string.IsNullOrWhiteSpace(value2)) ? Convert.ToInt32(value2) : 0);
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num3 > 59)
			{
				num3 = 59;
			}
		}
		else
		{
			num3 = 0;
		}
		return new TimeSpan(num, num2, num3);
	}

	public string TimeSpanToString(TimeSpan value)
	{
		if (ShowSeconds)
		{
			return value.Hours.ToString("00") + ":" + value.Minutes.ToString("00") + ":" + value.Seconds.ToString("00");
		}
		return value.Hours.ToString("00") + ":" + value.Minutes.ToString("00");
	}

	private string TimeSpanToString(TimeSpan? value)
	{
		if (value.HasValue)
		{
			return TimeSpanToString(value.Value);
		}
		if (!ShowSeconds)
		{
			return "  :  ";
		}
		return "  :  :  ";
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

	private void SpinHandler(object sender, SpinEventArgs e)
	{
		int caretIndex = CaretIndex;
		if (caretIndex >= 0 && caretIndex <= 2)
		{
			if (ShiftTime(1, e.IsSpinUp))
			{
				CaretIndex = 1;
				Select(CaretIndex, 1);
			}
		}
		else if (caretIndex >= 3 && caretIndex <= 5)
		{
			if (ShiftTime(2, e.IsSpinUp))
			{
				CaretIndex = 4;
				Select(CaretIndex, 1);
			}
		}
		else if (caretIndex > 5 && ShiftTime(3, e.IsSpinUp))
		{
			CaretIndex = 7;
			Select(CaretIndex, 1);
		}
		e.Handled = true;
	}

	private bool ShiftTime(int unit, bool increment)
	{
		DoValidate();
		string text = (string)EditValue;
		string text2 = null;
		if (string.IsNullOrWhiteSpace(text))
		{
			text2 = (ShowSeconds ? DateTime.Now.ToString("HH:mm:ss") : DateTime.Now.ToString("HH:mm"));
		}
		else if (ShowSeconds && text == "  :  :  ")
		{
			text2 = DateTime.Now.ToString("HH:mm:ss");
		}
		else if (!ShowSeconds && text == "  :  ")
		{
			text2 = DateTime.Now.ToString("HH:mm");
		}
		else
		{
			if (HasValidationError)
			{
				return false;
			}
			if (IsAcceptable(text))
			{
				if (ShowSeconds)
				{
					int num = Convert.ToInt32(text.Left(2));
					int num2 = Convert.ToInt32(text.Substring(3, 2));
					int num3 = Convert.ToInt32(text.Substring(6, 2));
					switch (unit)
					{
					case 1:
						if (increment && num < 23)
						{
							num++;
						}
						else if (!increment && num > 0)
						{
							num--;
						}
						break;
					case 2:
						if (increment && num2 < 59)
						{
							num2++;
						}
						else if (!increment && num2 > 0)
						{
							num2--;
						}
						break;
					case 3:
						if (increment && num3 < 59)
						{
							num3++;
						}
						else if (!increment && num3 > 0)
						{
							num3--;
						}
						break;
					}
					text2 = num.ToString("00") + ":" + num2.ToString("00") + ":" + num3.ToString("00");
				}
				else
				{
					int num4 = Convert.ToInt32(text.Left(2));
					int num5 = Convert.ToInt32(text.Substring(3, 2));
					switch (unit)
					{
					case 1:
						if (increment && num4 < 23)
						{
							num4++;
						}
						else if (!increment && num4 > 0)
						{
							num4--;
						}
						break;
					case 2:
						if (increment && num5 < 59)
						{
							num5++;
						}
						else if (!increment && num5 > 0)
						{
							num5--;
						}
						break;
					}
					text2 = num4.ToString("00") + ":" + num5.ToString("00");
				}
			}
		}
		if (text2 == null)
		{
			return false;
		}
		try
		{
			_suppressValidation = true;
			EditValue = text2;
			if (Convert.ToString(EditValue) != text2)
			{
				Clear();
				EditValue = text2;
			}
		}
		finally
		{
			_suppressValidation = false;
		}
		return text2 == (string)EditValue;
	}

	private void PreviewKeyDownHandler(object sender, KeyEventArgs e)
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
				CaretIndex = 7;
			}
			Select(CaretIndex, 1);
			e.Handled = true;
		}
	}
}
