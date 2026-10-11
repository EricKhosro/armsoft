using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Validation;
using DevExpress.Xpf.Editors.Validation.Native;

namespace ArmSoft.SME.Core.UI.Controls;

public class PopupCalcEditExt : PopupCalcEdit
{
	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(PopupCalcEditExt));

	public static readonly DependencyProperty AllowNegativeNumbersProperty = DependencyProperty.Register("AllowNegativeNumbers", typeof(bool), typeof(PopupCalcEditExt));

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

	public bool AllowNegativeNumbers
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(AllowNegativeNumbersProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(AllowNegativeNumbersProperty, (object)value);
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

	public PopupCalcEditExt()
	{
		IsRequired = false;
		AllowNegativeNumbers = false;
		Focusable = true;
		AllowNullInput = false;
		InvalidValueBehavior = InvalidValueBehavior.AllowLeaveEditor;
		EditValue = 0m;
		MaskUseAsDisplayFormat = true;
		CausesValidation = true;
		ShowPopupIfReadOnly = false;
		ValidateOnTextInput = false;
		_considerRequired = true;
		SelectAllOnMouseUp = true;
		KeyDown += new SafeEventHandler<KeyEventArgs>(PopupCalcEditExt_KeyDown).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(PopupCalcEditExt_PreviewKeyDown).Handler;
		PopupOpening += new SafeEventHandler<OpenPopupEventArgs>(PopupCalcEditExt_PopupOpening).Handler;
		Validate += new SafeEventHandler<ValidationEventArgs>(PopupCalcEditExt_Validate).Handler;
		Style = (Style)ArmSoft.SME.Core.UI.Settings.GenericResources["PopupCalcEditStyle"];
	}

	private void PopupCalcEditExt_KeyDown(object sender, KeyEventArgs e)
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

	private void PopupCalcEditExt_PreviewKeyDown(object sender, KeyEventArgs e)
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

	private void PopupCalcEditExt_PopupOpening(object sender, OpenPopupEventArgs e)
	{
		if (IsReadOnly)
		{
			e.Cancel = true;
		}
	}

	private void PopupCalcEditExt_Validate(object sender, ValidationEventArgs e)
	{
		if (SkipValidationIfReadOnly && IsReadOnly)
		{
			return;
		}
		if (IsRequired && _considerRequired)
		{
			if (e.Value == null)
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
			}
			else if (Convert.ToDecimal(e.Value) == 0m && e.UpdateSource != UpdateEditorSource.ValueChanging)
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
			}
		}
		if (AllowNegativeNumbers)
		{
			return;
		}
		if (e.Value == null)
		{
			if (!AllowNullInput)
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
			}
		}
		else if (Convert.ToDecimal(e.Value) < 0m && e.UpdateSource != UpdateEditorSource.ValueChanging)
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.ValueIsNegative;
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
