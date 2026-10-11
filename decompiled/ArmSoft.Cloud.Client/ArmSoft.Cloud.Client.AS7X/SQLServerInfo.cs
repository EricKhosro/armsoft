using System;

namespace ArmSoft.Cloud.Client.AS7X;

public class SQLServerInfo
{
	public string Name { get; set; }

	public Guid msdbId { get; set; }

	public string ObjectVersion { get; set; }

	public string ServerVersion { get; set; }

	public string Edition { get; set; }
}
