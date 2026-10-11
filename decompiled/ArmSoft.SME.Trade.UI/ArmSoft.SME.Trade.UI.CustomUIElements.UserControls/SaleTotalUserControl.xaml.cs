using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public partial class SaleTotalUserControl : UserControl, IComponentConnector
{
	public SaleTotalUserControl()
	{
		InitializeComponent();
		TB_TotalCaption.Text = ArmSoft.SME.Common.Resources.Total;
		TB_DiscountCaption.Text = ArmSoft.SME.Trade.Common.Resources.DiscountShort;
		TB_CashCaption.Text = ArmSoft.SME.Common.Resources.CashSum;
		TB_NoCashCaption.Text = ArmSoft.SME.Common.Resources.PlCardSum;
		TB_FromBonusCaption.Text = ArmSoft.SME.Trade.Common.Resources.PaidByBonus;
		TB_MustPayCaption.Text = ArmSoft.SME.Trade.Common.Resources.ForPay;
		TB_FromPrepayCaption.Text = ArmSoft.SME.Trade.Common.Resources.PrePayment;
		TB_CreditSaleCaption.Text = ArmSoft.SME.Trade.Common.Resources.CreditSale;
		Totals.RowDefinitions[3].Height = new GridLength(0.0);
		Totals.RowDefinitions[4].Height = new GridLength(0.0);
		Totals.RowDefinitions[5].Height = new GridLength(0.0);
		ASFontHelper.SetFontSettings(this);
	}
}
