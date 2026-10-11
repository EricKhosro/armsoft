using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class StorageRow
{
	[PrintDataMember("Code")]
	public string Code { get; set; }

	[PrintDataMember("Name")]
	public string Caption { get; set; }

	[PrintDataMember("Stockkeeper")]
	public string Response { get; set; }

	[PrintDataMember("Address")]
	public string Address { get; set; }
}
