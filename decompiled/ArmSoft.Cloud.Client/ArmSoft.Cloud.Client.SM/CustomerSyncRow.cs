using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.SM;

public class CustomerSyncRow
{
	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("ts")]
	public ulong TS { get; set; }
}
