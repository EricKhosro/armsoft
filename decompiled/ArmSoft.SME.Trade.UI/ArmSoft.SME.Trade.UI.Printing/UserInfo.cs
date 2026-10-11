using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class UserInfo
{
	[PrintDataMember("Name")]
	public string Name { get; set; }

	[PrintDataMember("Description")]
	public string UserDesc { get; set; }
}
