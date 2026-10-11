namespace ArmSoft.Cloud.Client.AS7X;

public class UpdateRequest
{
	public string KeySerialNumber { get; set; }

	public string KeyInfo { get; set; }

	public short Localization { get; set; }

	public short Application { get; set; }

	public string ApplicationVersion { get; set; }

	public string CertificateSerialNumber { get; set; }

	public string Mode { get; set; }

	public DBInfo DB { get; set; }

	public SQLServerInfo SQLServer { get; set; }

	public InstanceInfo Instance { get; set; }
}
