using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class QntUnitRow : CodeCaption
{
	[PrintDataMember("Brief")]
	public string Brief { get; set; }
}
