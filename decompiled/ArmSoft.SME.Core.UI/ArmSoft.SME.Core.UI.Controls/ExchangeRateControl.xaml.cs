using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class ExchangeRateControl : UserControl, IComponentConnector
{
	public delegate void EditValueChangedEventHandler(object sender, RateEditValueChangedEventArgs e);

	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(ExchangeRateControl));

	public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(ExchangeRateControl));

	private bool _considerRequired;

	private readonly PropertyChangeNotifier _isReadOnlyChangeNotifier;

	private readonly PropertyChangeNotifier _isRequiredChangeNotifier;

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

	public bool IsReadOnly
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsReadOnlyProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsReadOnlyProperty, (object)value);
			PopupCalcRate.IsReadOnly = value;
			PopupCalcBase.IsReadOnly = value;
			if (value)
			{
				LabelAt.Foreground = Settings.ReadOnlyBrush;
			}
			else
			{
				LabelAt.Foreground = PopupCalcRate.Foreground;
			}
		}
	}

	public virtual decimal Rate
	{
		get
		{
			return (decimal)PopupCalcRate.EditValue;
		}
		set
		{
			PopupCalcRate.EditValue = value;
		}
	}

	public virtual decimal Base
	{
		get
		{
			return (decimal)PopupCalcBase.EditValue;
		}
		set
		{
			PopupCalcBase.EditValue = value;
		}
	}

	public bool HasValidationError
	{
		get
		{
			if (!PopupCalcBase.HasValidationError)
			{
				return PopupCalcRate.HasValidationError;
			}
			return true;
		}
	}

	public PopupCalcEdit InnerRateControl => PopupCalcRate;

	public PopupCalcEdit InnerBaseControl => PopupCalcBase;

	public event EditValueChangedEventHandler EditValueChanged;

	public bool DoValidate()
	{
		bool flag = PopupCalcRate.DoValidate();
		if (flag)
		{
			flag = PopupCalcBase.DoValidate();
		}
		return flag;
	}

	private void ExchangeRateControl_Initialized(object sender, EventArgs e)
	{
		LabelAt.Content = ArmSoft.SME.Common.Resources.At;
		PopupCalcRate.Mask = DecimalExtension.GetCurrencyFormat(11, 4);
		PopupCalcRate.MaskUseAsDisplayFormat = true;
		PopupCalcRate.Precision = 4;
		PopupCalcRate.EditValue = 1m;
		PopupCalcRate.Width = SizeCalculator.WidthForNumber(11);
		PopupCalcRate.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PopupCalcRate_EditValueChanged).Handler;
		PopupCalcRate.KeyDown += new SafeEventHandler<KeyEventArgs>(OnKeyDownEnter).Handler;
		PopupCalcRate.Validate += new SafeEventHandler<ValidationEventArgs>(OnValidateRate).Handler;
		PopupCalcRate.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(PreviewKeyDownEH).Handler;
		PopupCalcRate.PopupOpening += new SafeEventHandler<OpenPopupEventArgs>(OnPopupOpening).Handler;
		PopupCalcBase.Mask = DecimalExtension.GetCurrencyFormat(5, 0);
		PopupCalcBase.MaskUseAsDisplayFormat = true;
		PopupCalcBase.Precision = 0;
		PopupCalcBase.EditValue = 1m;
		PopupCalcBase.Width = SizeCalculator.WidthForNumber(5);
		PopupCalcBase.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PopupCalcBase_EditValueChanged).Handler;
		PopupCalcBase.KeyDown += new SafeEventHandler<KeyEventArgs>(OnKeyDownEnter).Handler;
		PopupCalcBase.Validate += new SafeEventHandler<ValidationEventArgs>(OnValidateBase).Handler;
		PopupCalcBase.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(PreviewKeyDownEH).Handler;
		PopupCalcBase.PopupOpening += new SafeEventHandler<OpenPopupEventArgs>(OnPopupOpening).Handler;
	}

	private void PopupCalcRate_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		RateEditValueChangedEventArgs e2 = new RateEditValueChangedEventArgs(e)
		{
			IsRateChanged = true
		};
		EditValueChanged?.Invoke(this, e2);
	}

	private void PopupCalcBase_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		RateEditValueChangedEventArgs e2 = new RateEditValueChangedEventArgs(e);
		EditValueChanged?.Invoke(this, e2);
	}

	private void OnValidateRate(object sender, ValidationEventArgs e)
	{
		if (e.Value == null)
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
		}
		else if (Convert.ToDecimal(e.Value) < 0m)
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.ValueIsNegative;
		}
		else if (IsRequired && _considerRequired && Convert.ToDecimal(e.Value) == 0m)
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
		}
	}

	private void OnValidateBase(object sender, ValidationEventArgs e)
	{
		if (e.Value == null)
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
		}
		else if (Convert.ToDecimal(e.Value) < 0m)
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.ValueIsNegative;
		}
		else if (Convert.ToDecimal(e.Value) == 0m)
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
		}
	}

	private void PreviewKeyDownEH(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		if ((int)e.Key == 20 || (((int)e.Key == 26) & ((int)Keyboard.Modifiers == 2)))
		{
			((PopupCalcEdit)sender).ShowPopup();
		}
	}

	private void OnKeyDownEnter(object sender, KeyEventArgs e)
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

	private void OnPopupOpening(object sender, OpenPopupEventArgs e)
	{
		if (IsReadOnly)
		{
			e.Cancel = true;
		}
	}

	public ExchangeRateControl()
	{
		Initialized += new SafeEventHandler<EventArgs>(ExchangeRateControl_Initialized).Handler;
		InitializeComponent();
		IsRequired = false;
		IsReadOnly = false;
		_considerRequired = true;
		PopupCalcBase.Style = (Style)Settings.GenericResources["PopupCalcEditStyle"];
		PopupCalcRate.Style = PopupCalcBase.Style;
		_isRequiredChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsRequiredProperty);
		_isRequiredChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
		_isReadOnlyChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsReadOnlyProperty);
		_isReadOnlyChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredChanged).Handler;
	}

	private void IsRequiredChanged(object sender, EventArgs e)
	{
		if (IsRequired && !IsReadOnly)
		{
			PopupCalcRate.Background = Settings.RequiredBrush;
			PopupCalcBase.Background = Settings.RequiredBrush;
		}
		else
		{
			DependencyPropertyDescriptor val = DependencyPropertyDescriptor.FromProperty(Control.BackgroundProperty, typeof(PopupCalcEdit));
			((PropertyDescriptor)(object)val).ResetValue((object)PopupCalcRate);
			((PropertyDescriptor)(object)val).ResetValue((object)PopupCalcBase);
		}
		if (IsReadOnly)
		{
			PopupCalcRate.Foreground = Settings.ReadOnlyBrush;
			PopupCalcBase.Foreground = Settings.ReadOnlyBrush;
		}
		else
		{
			DependencyPropertyDescriptor val2 = DependencyPropertyDescriptor.FromProperty(Control.ForegroundProperty, typeof(PopupCalcEdit));
			((PropertyDescriptor)(object)val2).ResetValue((object)PopupCalcRate);
			((PropertyDescriptor)(object)val2).ResetValue((object)PopupCalcBase);
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
