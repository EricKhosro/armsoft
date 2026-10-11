using ArmSoft.SME.Core.UI;

namespace ArmSoft.SME.Trade.UI;

public class TradeDialogWindow : DialogWindow
{
	public PartnerContractGroupControls AddPartnerContractGroupControls()
	{
		return new PartnerContractGroupControls(this);
	}
}
