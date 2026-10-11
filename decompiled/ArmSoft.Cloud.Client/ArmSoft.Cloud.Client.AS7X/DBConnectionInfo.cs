using System;
using System.Collections.Generic;

namespace ArmSoft.Cloud.Client.AS7X;

public class DBConnectionInfo
{
	public int DBId { get; set; }

	public string Database { get; set; }

	public string Version { get; set; }

	public string TaxCode { get; set; }

	public string CompanyName { get; set; }

	public string Server { get; set; }

	public string ServerInnerAddress { get; set; }

	public Guid ServerId { get; set; }

	public string Login { get; set; }

	public string Password { get; set; }

	public List<Role> Roles { get; set; }

	public string ServiceAddress { get; set; }

	public bool PSMode { get; set; }
}
