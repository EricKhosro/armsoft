using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.SM;

public class CustomersSynchronizationRequest
{
	[JsonProperty("dbts")]
	public ulong? DBTS { get; set; }

	[JsonProperty("entities", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public List<Customer> Entities { get; set; }
}
