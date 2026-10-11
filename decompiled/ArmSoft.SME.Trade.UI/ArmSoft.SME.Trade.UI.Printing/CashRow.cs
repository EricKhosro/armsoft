using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class CashRow
{
	[PrintDataMember("Code")]
	public string Code { get; set; }

	[PrintDataMember("Name")]
	public string Caption { get; set; }
}
