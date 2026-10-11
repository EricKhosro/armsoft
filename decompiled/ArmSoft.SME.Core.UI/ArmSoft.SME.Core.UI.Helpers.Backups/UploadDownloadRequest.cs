namespace ArmSoft.SME.Core.UI.Helpers.Backups;

public class UploadDownloadRequest
{
	public ProgressStep ProgressStep { get; set; }

	public string ServerName { get; set; }

	public string ServiceAddress { get; set; }

	public string SourceFileName { get; set; }

	public string DestinationFileName { get; set; }
}
