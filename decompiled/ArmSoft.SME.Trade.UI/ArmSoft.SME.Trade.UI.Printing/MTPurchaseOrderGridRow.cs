using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTPurchaseOrderGridRow : MTGridRow
{
	[PrintDataMember("Image")]
	public byte[] Image { get; set; }
}
