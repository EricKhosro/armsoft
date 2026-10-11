using System;
using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.SM;

public class CustomerSyncState
{
	[JsonProperty("timeSpan")]
	public TimeSpan? TimeSpan { get; set; }

	[JsonProperty("ts")]
	public ulong? TS { get; set; }
}
