using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public partial class SamplePOSSaleableGoodInfoUserControl : UserControl, IComponentConnector
{
	public delegate void ButtonClickEventHandler(object sender, SaleWindowHelper.ButtonClickEventArgs e);

	private MTInvoiceBase.MTInvoiceSpecificationRow _mTInvoiceSpecificationRow;

	public MTInvoiceBase.MTInvoiceSpecificationRow MTInvoiceSpecificationRow
	{
		get
		{
			return _mTInvoiceSpecificationRow;
		}
		set
		{
			_mTInvoiceSpecificationRow = value;
			TB_GoodName.Text = _mTInvoiceSpecificationRow.FullName;
		}
	}

	public event ButtonClickEventHandler ButtonClick;

	public SamplePOSSaleableGoodInfoUserControl()
	{
		InitializeComponent();
		DiscountName.Text = ArmSoft.SME.Trade.Common.Resources.Discount + " (%, ∑)";
		ASFontHelper.SetFontSettings(this);
	}

	private void BE_Discount_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Discount));
	}

	private void BE_QtyPlus_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.QtyPlus));
	}

	private void BE_QtyMinus_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.QtyMinus));
	}

	private void ButtonQTY_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.QTY));
	}
}
