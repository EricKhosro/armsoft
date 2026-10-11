using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Common.Models.Documents;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(4)]
public class BankInWindow : BankInOutWindow<BankIn>
{
	protected override double WidthValue => 630.0;

	protected override double HeightValue => 490.0;

	protected override double MinWidthValue => 600.0;

	protected override double MinHeightValue => 250.0;

	public BankInWindow()
	{
	}

	public BankInWindow(BankIn doc)
		: base(doc)
	{
	}
}
