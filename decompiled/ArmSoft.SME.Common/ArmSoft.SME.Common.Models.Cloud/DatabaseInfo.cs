using System;

namespace ArmSoft.SME.Common.Models.Cloud;

public class DatabaseInfo
{
	public int DbId { get; set; }

	public string DbName { get; set; }

	public string Caption { get; set; }

	public int ServerId { get; set; }

	public string ServerName { get; set; }

	public DateTime? LastBackup { get; set; }

	public string Version { get; set; }

	public string TaxCode { get; set; }

	public string CompanyName { get; set; }

	public string ServiceAddress { get; set; }

	public string Host { get; set; }

	public bool PSMode { get; set; }

	public int? KeyId { get; set; }

	public string KeyCode { get; set; }

	public DateTime? LastLogin { get; set; }

	public string DbServiceAddress { get; set; }

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
