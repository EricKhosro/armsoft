using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Validation;

namespace ArmSoft.SME.Core.UI.Controls;

public class TextEditExt : TextEdit
{
	private bool _considerRequired;

	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(TextEditExt));

	private readonly PropertyChangeNotifier _isReadOnlyChangeNotifier;

	private readonly PropertyChangeNotifier _isRequiredChangeNotifier;

	private readonly PropertyChangeNotifier _isEnabledChangeNotifier;

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

	public TextEditExt()
	{
		IsRequired = false;
		Focusable = true;
		AllowNullInput = true;
		InvalidValueBehavior = InvalidValueBehavior.AllowLeaveEditor;
		EditValue = "";
		MaskUseAsDisplayFormat = true;
		CausesValidation = true;
		ValidateOnTextInput = false;
		ValidateOnEnterKeyPressed = true;
		_considerRequired = true;
		_isRequiredChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsRequiredProperty);
		_isRequiredChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isReadOnlyChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, BaseEdit.IsReadOnlyProperty);
		_isReadOnlyChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isEnabledChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, UIElement.IsEnabledProperty);
		_isEnabledChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		KeyDown += new SafeEventHandler<KeyEventArgs>(TextEditExt_KeyDown).Handler;
		Validate += new SafeEventHandler<ValidationEventArgs>(TextEditExt_Validate).Handler;
	}

	private void IsRequiredChanged(object sender, EventArgs e)
	{
		if (IsRequired && !IsReadOnly)
		{
			Background = ArmSoft.SME.Core.UI.Settings.RequiredBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.BackgroundProperty, typeof(TextEditExt))).ResetValue((object)this);
		}
		if (IsReadOnly && IsEnabled)
		{
			Foreground = ArmSoft.SME.Core.UI.Settings.ReadOnlyBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(TextEditExt))).ResetValue((object)this);
		}
	}

	private void TextEditExt_Validate(object sender, ValidationEventArgs e)
	{
		if ((!SkipValidationIfReadOnly || !IsReadOnly) && IsRequired && _considerRequired && (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString())))
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
			EditValue = "";
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
}
