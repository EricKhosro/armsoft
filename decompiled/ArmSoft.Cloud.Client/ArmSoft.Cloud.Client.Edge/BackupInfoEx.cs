using System;

namespace ArmSoft.Cloud.Client.Edge;

public class BackupInfoEx
{
	public int DbId { get; set; }

	public string DbName { get; set; }

	public string FileName { get; set; }

	public DateTime DateCreated { get; set; }

	public string DbVersion { get; set; }

	public long DbSize { get; set; }

	public string Comment { get; set; }

	public string Creator { get; set; }

	public bool IsAutoBackup { get; set; }

	public bool CreatedByASUser { get; set; }

	public short Appliaction { get; set; }

	public bool PSMode { get; set; }

	public decimal FileLength { get; set; }

	public bool IsDailyBackup { get; set; }

	public bool IsMonthlyBackup { get; set; }

	public bool IsAnnualBackup { get; set; }
}
