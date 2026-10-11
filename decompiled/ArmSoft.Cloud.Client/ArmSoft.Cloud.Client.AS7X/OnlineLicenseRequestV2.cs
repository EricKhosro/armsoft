namespace ArmSoft.Cloud.Client.AS7X;

internal class OnlineLicenseRequestV2
{
	public string Key { get; set; }

	public string Fingerprint { get; set; }

	public string Database { get; set; }

	public string Server { get; set; }

	public string WorkStaition { get; set; }

	public int TerminalId { get; set; }

	public int ProcessId { get; set; }

	public string Version { get; set; }
}
