using ArmSoft.SME.Common.Models.PrintableElements;

namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class PosTerminalReceipt : PrintableElement
{
	public string Caption { get; set; }

	public byte[] HeaderImage { get; set; }

	public string Receipt { get; set; }
}
