using System;
using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.Edge;

public class DBConnectionInfo
{
	public string ProgramName { get; set; }

	public string HostName { get; set; }

	public int UserId { get; set; }

	public string UserName { get; set; }

	public string User { get; set; }

	public string Window { get; set; }

	public string Operation { get; set; }

	[JsonConverter(typeof(LocalDateConverter))]
	public DateTime LoginTime { get; set; }

	[JsonConverter(typeof(LocalDateConverter))]
	public DateTime LastTime { get; set; }

	public short SPID { get; set; }

	public short Blocked { get; set; }

	public bool Locked { get; set; }
}
