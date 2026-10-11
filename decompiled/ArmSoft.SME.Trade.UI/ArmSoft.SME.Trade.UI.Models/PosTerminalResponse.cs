namespace ArmSoft.SME.Trade.UI.Models;

public class PosTerminalResponse
{
	public string RRN { get; set; } = "";

	public string TerminalId { get; set; } = "";

	public PosTerminalResponse(string rrn, string terminalId)
	{
		RRN = rrn;
		TerminalId = terminalId;
	}

	public PosTerminalResponse()
	{
	}
}
