using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public partial class SaleBonusInfoUserControl : UserControl, IComponentConnector
{
	public SaleBonusInfoUserControl()
	{
		InitializeComponent();
		TB_DiscountCaption.Text = ArmSoft.SME.Trade.Common.Resources.DiscountShort + " (";
		TB_BonusCaption.Text = ArmSoft.SME.Trade.Common.Resources.AccumulatedBonusShort + ": ";
		TB_AccumulatedBonusCaption.Text = ArmSoft.SME.Trade.Common.Resources.CalcBonusesShort + ": ";
		TB_AddedBonusCaption.Text = ArmSoft.SME.Trade.Common.Resources.AddToBonusShort + ": ";
		ASFontHelper.SetFontSettings(this);
	}
}
