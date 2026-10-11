namespace ArmSoft.Cloud.Client.Edge;

public class DatabaseBackupRequest
{
	public string Database { get; set; }

	public string Filename { get; set; }

	public string Comment { get; set; }

	public bool IsAutoBackup { get; set; }

	public bool IsDailyBackup { get; set; }

	public bool IsMonthlyBackup { get; set; }

	public bool IsAnnualBackup { get; set; }
}
