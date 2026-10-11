using System;
using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.SM;

public class SalesPointInfoRegisteredInB2BMobile
{
	[JsonProperty("SalesPointId")]
	public int SalesPointId { get; set; }

	[JsonProperty("LastLoginTime")]
	public DateTime? LastLoginTime { get; set; }
}
