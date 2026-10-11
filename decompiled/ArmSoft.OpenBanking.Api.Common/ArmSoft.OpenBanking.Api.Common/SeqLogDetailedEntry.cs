namespace ArmSoft.OpenBanking.Api.Common;

public class SeqLogDetailedEntry : SeqLogEntry
{
	public string RenderedMessage { get; set; } = string.Empty;

	public string Exception { get; set; } = string.Empty;
}
