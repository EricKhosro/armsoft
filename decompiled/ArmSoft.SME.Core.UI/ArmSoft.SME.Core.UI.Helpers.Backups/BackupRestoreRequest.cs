namespace ArmSoft.SME.Core.UI.Helpers.Backups;

public class BackupRestoreRequest
{
	public OperationTypes Type { get; set; }

	public string ServerName { get; set; }

	public string ServiceAddress { get; set; }

	public string DatabaseName { get; set; }

	public string Comment { get; set; }

	public string FileName { get; set; }

	public bool IsAutoBackup { get; set; }

	public bool DoNotArchiveBackup { get; set; }
}
