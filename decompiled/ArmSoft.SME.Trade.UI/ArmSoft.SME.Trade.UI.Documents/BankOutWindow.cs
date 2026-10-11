using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Common.Models.Documents;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(5)]
public class BankOutWindow : BankInOutWindow<BankOut>
{
	public BankOutWindow()
	{
	}

	public BankOutWindow(BankOut doc)
		: base(doc)
	{
	}
}
