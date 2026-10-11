using System;

namespace ArmSoft.Cloud.Client.AS7X;

public class CloudDBInfo
{
	public int DBId { get; set; }

	public string Database { get; set; }

	public string Version { get; set; }

	public string TaxCode { get; set; }

	public string CompanyName { get; set; }

	public string ServerName { get; set; }

	public Guid ServerId { get; set; }

	public bool PSMode { get; set; }
}
