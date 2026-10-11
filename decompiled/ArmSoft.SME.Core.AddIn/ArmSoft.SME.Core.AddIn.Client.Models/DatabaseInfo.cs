using System;

namespace ArmSoft.SME.Core.AddIn.Client.Models;

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
}
