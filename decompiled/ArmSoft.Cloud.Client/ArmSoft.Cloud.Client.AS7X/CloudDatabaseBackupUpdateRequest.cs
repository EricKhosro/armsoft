namespace ArmSoft.Cloud.Client.AS7X;

public class CloudDatabaseBackupUpdateRequest
{
	public bool DailyBackupEnabled { get; set; }

	public short DailyBackupCount { get; set; }

	public bool MonthlyBackupEnabled { get; set; }

	public short MonthlyBackupDay { get; set; }

	public short MonthlyBackupCount { get; set; }

	public bool AnnualBackupEnabled { get; set; }

	public short AnnualBackupDay { get; set; }

	public short AnnualBackupMonth { get; set; }

	public short AnnualBackupCount { get; set; }

	public bool CreateOnExistedLogin { get; set; }
}
