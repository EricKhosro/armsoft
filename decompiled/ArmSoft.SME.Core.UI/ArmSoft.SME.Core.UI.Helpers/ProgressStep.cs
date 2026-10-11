namespace ArmSoft.SME.Core.UI.Helpers;

public enum ProgressStep
{
	PrepareFileForUpload,
	UploadToDropBox,
	DownloadFromDropBox,
	ImportBackupFile,
	CreateBackup,
	ZIPBackupFile,
	CopyToLocalFile,
	RestoreBackup,
	ExtractBackupFile,
	CreateDatabaseFromTemplate
}
