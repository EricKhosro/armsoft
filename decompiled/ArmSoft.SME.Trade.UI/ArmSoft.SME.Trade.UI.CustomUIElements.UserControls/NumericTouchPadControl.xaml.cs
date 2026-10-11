using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public partial class NumericTouchPadControl : UserControl, IComponentConnector
{
	public delegate void ButtonClickEventHandler(object sender, SaleWindowHelper.ButtonClickEventArgs e);

	public static readonly DependencyProperty RedesignedHideLastRowProperty = DependencyProperty.Register("RedesignedHideLastRow", typeof(bool), typeof(NumericTouchPadControl), (PropertyMetadata)(object)new FrameworkPropertyMetadata((object)false, (PropertyChangedCallback)RedesignedHideLastRowPropertyChanged));

	public bool RedesignedHideLastRow
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(RedesignedHideLastRowProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(RedesignedHideLastRowProperty, (object)value);
		}
	}

	public event ButtonClickEventHandler ButtonClick;

	public NumericTouchPadControl()
	{
		InitializeComponent();
		ASFontHelper.SetFontSettings(this);
	}

	private void Button1_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs("1"));
	}

	private void Button2_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs("2"));
	}

	private void Button3_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs("3"));
	}

	private void Button4_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs("4"));
	}

	private void Button5_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs("5"));
	}

	private void Button6_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs("6"));
	}

	private void Button7_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs("7"));
	}

	private void Button8_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs("8"));
	}

	private void Button9_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs("9"));
	}

	private void Button0_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs("0"));
	}

	private void Button00_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs("00"));
	}

	private void Button000_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs("000"));
	}

	private void ButtonDot_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs("."));
	}

	private void ButtonBack_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.BackSpace));
	}

	private void ButtonC_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.C));
	}

	public static void RedesignedHideLastRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		NumericTouchPadControl numericTouchPadControl = (NumericTouchPadControl)(object)d;
		if ((bool)e.NewValue)
		{
			numericTouchPadControl.GridLastRow.Height = new GridLength(0.0);
			numericTouchPadControl.GridLastSpacingRowRow.Height = new GridLength(0.0);
		}
		else
		{
			numericTouchPadControl.GridLastSpacingRowRow.Style = (Style)Application.Current.FindResource("TouchModePrimaryButtonsSeparatorRowDefintionStyle");
			numericTouchPadControl.GridLastRow.Style = (Style)Application.Current.FindResource("TouchModePrimaryButtonRowDefintionStyle");
		}
	}

	static NumericTouchPadControl()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected Obj, but got Unknown
	}
}
