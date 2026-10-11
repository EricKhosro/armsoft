namespace ArmSoft.SME.Core.UI.Helpers.Backups;

public class DropBoxImportExportRequest(DropBoxRequestModes mode)
{
	public string DBName { get; set; } = string.Empty;

	public string DBCaption { get; set; } = string.Empty;

	public string FileName { get; set; } = string.Empty;

	public decimal CompSize { get; set; }

	public DropBoxRequestModes Mode { get; private set; } = mode;
}
