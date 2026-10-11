namespace ArmSoft.Arcus2Client;

public class TerminalResponse
{
	public string RRN { get; set; }

	public string TerminalId { get; set; }

	public TerminalResponse(string rrn, string terminalId)
	{
		RRN = rrn;
		TerminalId = terminalId;
	}

	public TerminalResponse()
	{
	}
}
