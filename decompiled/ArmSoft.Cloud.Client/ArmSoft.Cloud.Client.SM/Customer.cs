using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.SM;

public class Customer
{
	[JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public int Id { get; set; }

	[JsonProperty("dbInnerId", DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public int DBInnerId { get; set; }

	[JsonProperty("itId")]
	public int ITId { get; set; }

	[JsonProperty("actorId")]
	public int ActorId { get; set; }

	[JsonProperty("innerId", DefaultValueHandling = DefaultValueHandling.Ignore)]
	public int InnerId { get; set; }

	[JsonProperty("code")]
	public string Code { get; set; }

	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("taxCode")]
	public string TaxCode { get; set; }

	[JsonProperty("businessAddress")]
	public string BusinessAddress { get; set; }

	[JsonProperty("location", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public GeoLocation Location { get; set; }

	[JsonProperty("isClosed")]
	public bool IsClosed { get; set; }

	[JsonProperty("ts")]
	public ulong TS { get; set; }
}
