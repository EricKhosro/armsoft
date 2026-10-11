using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Validation;

namespace ArmSoft.SME.Core.UI.Controls;

public class MemoEditControl : ButtonEdit
{
	private bool _considerRequired;

	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(MemoEditControl));

	private readonly PropertyChangeNotifier _isReadOnlyChangeNotifier;

	private readonly PropertyChangeNotifier _isRequiredChangeNotifier;

	private readonly ButtonInfo _dropDownButton;

	internal bool SkipValidationIfReadOnly { get; set; }

	public Window Owner { get; set; }

	public string DropDownCaption { get; set; }

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

	public MemoEditControl()
	{
		IsRequired = false;
		Focusable = true;
		AllowNullInput = true;
		InvalidValueBehavior = InvalidValueBehavior.AllowLeaveEditor;
		EditValue = "";
		DropDownCaption = "";
		MaskUseAsDisplayFormat = true;
		CausesValidation = true;
		ValidateOnTextInput = false;
		AllowDefaultButton = false;
		ValidateOnEnterKeyPressed = true;
		_considerRequired = true;
		_dropDownButton = new ButtonInfo
		{
			GlyphKind = GlyphKind.DropDown
		};
		_dropDownButton.Click += new SafeEventHandler<RoutedEventArgs>(Button_DropDownClick).Handler;
		Buttons.Add(_dropDownButton);
		_isRequiredChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsRequiredProperty);
		_isRequiredChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isReadOnlyChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, BaseEdit.IsReadOnlyProperty);
		_isReadOnlyChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		Validate += new SafeEventHandler<ValidationEventArgs>(MemoEditControl_Validate).Handler;
		KeyDown += new SafeEventHandler<KeyEventArgs>(MemoEditControl_KeyDown).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(MemoEditControl_PreviewKeyDown).Handler;
	}

	private void IsRequiredChanged(object sender, EventArgs e)
	{
		if (IsRequired && !IsReadOnly)
		{
			Background = ArmSoft.SME.Core.UI.Settings.RequiredBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.BackgroundProperty, typeof(MemoEditControl))).ResetValue((object)this);
		}
		if (IsReadOnly)
		{
			Foreground = ArmSoft.SME.Core.UI.Settings.ReadOnlyBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(MemoEditControl))).ResetValue((object)this);
		}
	}

	private void MemoEditControl_Validate(object sender, ValidationEventArgs e)
	{
		if ((!SkipValidationIfReadOnly || !IsReadOnly) && IsRequired && _considerRequired && (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString())))
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
		}
	}

	private void MemoEditControl_KeyDown(object sender, KeyEventArgs e)
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

	private void MemoEditControl_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		if ((int)e.Key == 20 || ((int)e.Key == 26 && (int)Keyboard.Modifiers == 2))
		{
			DropDownClick();
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

	private void Button_DropDownClick(object sender, RoutedEventArgs e)
	{
		DropDownClick();
	}

	private bool GetCoordinatesForPopup(out double top, out double left)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Point val = PointToScreen(new Point(0.0, 0.0));
			Matrix transformFromDevice = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
			Point val2 = transformFromDevice.Transform(val);
			top = val2.Y + ActualHeight;
			left = val2.X;
			return true;
		}
		catch (Exception)
		{
			top = 0.0;
			left = 0.0;
			return false;
		}
	}

	private void DropDownClick()
	{
		try
		{
			DoValidate();
			if (GetCoordinatesForPopup(out var top, out var left))
			{
				MemoWindow memoWindow = new MemoWindow(Owner, MaxLength)
				{
					Top = top,
					Left = left,
					Width = ActualWidth,
					Text = Text,
					IsReadOnly = IsReadOnly,
					Title = DropDownCaption
				};
				if (memoWindow.ShowDialog().Value)
				{
					EditValue = memoWindow.Text;
					Focus();
				}
			}
		}
		catch (Exception ex)
		{
			ArmSoft.SME.Core.UI.Settings.ErrorHandler(ex);
		}
	}
}
