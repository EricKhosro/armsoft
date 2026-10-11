using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Validation;

namespace ArmSoft.SME.Core.UI.Controls;

public class ComboBoxEditExt : ComboBoxEdit
{
	public delegate void ClickDropDownEventHandler(object sender, ComboBoxEditExtEventArgs e);

	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(ComboBoxEditExt));

	private readonly PropertyChangeNotifier _isReadOnlyChangeNotifier;

	private readonly PropertyChangeNotifier _isRequiredChangeNotifier;

	private bool _considerRequired;

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

	public event ClickDropDownEventHandler ClickDropDown;

	public ComboBoxEditExt()
	{
		IsRequired = false;
		Focusable = true;
		AllowNullInput = true;
		InvalidValueBehavior = InvalidValueBehavior.AllowLeaveEditor;
		AutoComplete = true;
		ImmediatePopup = false;
		EditValue = "";
		MaskUseAsDisplayFormat = true;
		CausesValidation = true;
		ValidateOnTextInput = false;
		_considerRequired = true;
		_isRequiredChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsRequiredProperty);
		_isRequiredChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isReadOnlyChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, BaseEdit.IsReadOnlyProperty);
		_isReadOnlyChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		Validate += new SafeEventHandler<ValidationEventArgs>(ComboBoxEditExt_Validate).Handler;
		KeyDown += new SafeEventHandler<KeyEventArgs>(ComboBoxEditExt_KeyDown).Handler;
		PopupOpening += new SafeEventHandler<OpenPopupEventArgs>(ComboBoxEditExt_PopupOpening).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(ComboBoxEditExt_PreviewKeyDown).Handler;
	}

	private void IsRequiredChanged(object sender, EventArgs e)
	{
		if (IsRequired && !IsReadOnly)
		{
			Background = ArmSoft.SME.Core.UI.Settings.RequiredBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.BackgroundProperty, typeof(ComboBoxEditExt))).ResetValue((object)this);
		}
		if (IsReadOnly)
		{
			Foreground = ArmSoft.SME.Core.UI.Settings.ReadOnlyBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(ComboBoxEditExt))).ResetValue((object)this);
		}
	}

	private void ComboBoxEditExt_Validate(object sender, ValidationEventArgs e)
	{
		if ((!SkipValidationIfReadOnly || !IsReadOnly) && IsRequired && _considerRequired && (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString())))
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
		}
	}

	private void ComboBoxEditExt_KeyDown(object sender, KeyEventArgs e)
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

	private void ComboBoxEditExt_PopupOpening(object sender, OpenPopupEventArgs e)
	{
		if (IsReadOnly)
		{
			e.Cancel = true;
			return;
		}
		ComboBoxEditExtEventArgs e2 = new ComboBoxEditExtEventArgs(this);
		ClickDropDown?.Invoke(this, e2);
		if (e2.Cancel)
		{
			e.Cancel = true;
		}
	}

	private void ComboBoxEditExt_PreviewKeyDown(object sender, KeyEventArgs e)
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
