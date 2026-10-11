using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Validation;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.Controls;

public class ButtonEditExt : ButtonEdit
{
	public delegate void MagicClickEventHandler(object sender, MagicClickEventArgs e);

	private ButtonInfo _magicButton;

	private string _magicToolTip;

	private bool _considerRequired;

	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(ButtonEditExt));

	private readonly PropertyChangeNotifier _isReadOnlyChangeNotifier;

	private readonly PropertyChangeNotifier _isRequiredChangeNotifier;

	private const string _magicToolTipHotKeyConst = " (Ctrl+M)";

	private const string _magicButtonNameConst = "MagicButton";

	internal bool SkipValidationIfReadOnly { get; set; }

	public Window Owner { get; set; }

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

	public string MagicToolTip
	{
		get
		{
			return _magicToolTip;
		}
		set
		{
			_magicToolTip = value;
			MakeMagicToolTip();
		}
	}

	public bool AllowMagicButton
	{
		get
		{
			return _magicButton != null;
		}
		set
		{
			if (value)
			{
				if (_magicButton == null)
				{
					Path content = new Path
					{
						Style = (Style)ArmSoft.SME.Core.UI.Settings.GenericResources["WizardPath"],
						Fill = ArmSoft.SME.Core.UI.Settings.GetArrowBrush()
					};
					_magicButton = new ButtonInfo
					{
						GlyphKind = GlyphKind.User,
						Content = content,
						Name = "MagicButton"
					};
					_magicButton.Click += new SafeEventHandler<RoutedEventArgs>(MagicButton_Click).Handler;
					Binding binding = new Binding("IsReadOnly")
					{
						Source = this,
						Mode = BindingMode.OneWay,
						Converter = new InverseBooleanConverter()
					};
					_magicButton.SetBinding(ContentElement.IsEnabledProperty, binding);
					MakeMagicToolTip();
					Buttons.Add(_magicButton);
				}
			}
			else if (_magicButton != null)
			{
				Buttons.Remove(_magicButton);
				_magicButton = null;
			}
		}
	}

	public bool IsEnabledMagicButton
	{
		get
		{
			if (_magicButton != null)
			{
				return _magicButton.IsEnabled;
			}
			return false;
		}
		set
		{
			if (_magicButton != null)
			{
				_magicButton.IsEnabled = value;
			}
		}
	}

	public event MagicClickEventHandler MagicClick;

	private void MakeMagicToolTip()
	{
		if (_magicButton != null && !string.IsNullOrWhiteSpace(MagicToolTip))
		{
			_magicButton.ToolTip = MagicToolTip + " (Ctrl+M)";
		}
	}

	public ButtonEditExt()
	{
		IsRequired = false;
		Focusable = true;
		AllowNullInput = true;
		InvalidValueBehavior = InvalidValueBehavior.AllowLeaveEditor;
		EditValue = "";
		MaskUseAsDisplayFormat = true;
		CausesValidation = true;
		ValidateOnTextInput = false;
		AllowDefaultButton = false;
		ValidateOnEnterKeyPressed = true;
		_considerRequired = true;
		_magicButton = null;
		_isRequiredChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsRequiredProperty);
		_isRequiredChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isReadOnlyChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, BaseEdit.IsReadOnlyProperty);
		_isReadOnlyChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		Validate += new SafeEventHandler<ValidationEventArgs>(ButtonEditExt_Validate).Handler;
		KeyDown += new SafeEventHandler<KeyEventArgs>(ButtonEditExt_KeyDown).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(ButtonEditExt_PreviewKeyDown).Handler;
	}

	private void IsRequiredChanged(object sender, EventArgs e)
	{
		if (IsRequired && !IsReadOnly)
		{
			Background = ArmSoft.SME.Core.UI.Settings.RequiredBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.BackgroundProperty, typeof(ButtonEditExt))).ResetValue((object)this);
		}
		if (IsReadOnly)
		{
			Foreground = ArmSoft.SME.Core.UI.Settings.ReadOnlyBrush;
		}
		else
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(ButtonEditExt))).ResetValue((object)this);
		}
	}

	private void ButtonEditExt_Validate(object sender, ValidationEventArgs e)
	{
		if ((!SkipValidationIfReadOnly || !IsReadOnly) && IsRequired && _considerRequired && (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString())))
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
			EditValue = "";
		}
	}

	private void ButtonEditExt_KeyDown(object sender, KeyEventArgs e)
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

	private void ButtonEditExt_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		if (_magicButton != null && _magicButton.IsEnabled && (int)e.Key == 56 && (int)Keyboard.Modifiers == 2 && GetCoordinatesForPopup(out var top, out var left))
		{
			MagicClickEventArgs e2 = new MagicClickEventArgs(Owner, top, left);
			MagicClick?.Invoke(this, e2);
			e.Handled = true;
		}
	}

	private void MagicButton_Click(object sender, RoutedEventArgs e)
	{
		if (GetCoordinatesForPopup(out var top, out var left))
		{
			MagicClickEventArgs e2 = new MagicClickEventArgs(Owner, top, left);
			MagicClick?.Invoke(this, e2);
		}
	}

	public bool GetCoordinatesForPopup(out double top, out double left)
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
