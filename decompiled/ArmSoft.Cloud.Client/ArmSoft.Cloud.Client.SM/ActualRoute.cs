using System;
using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.SM;

public class ActualRoute
{
	[JsonProperty("date")]
	public DateTime Date { get; set; }

	[JsonProperty("customerId")]
	public int CustomerId { get; set; }

	[JsonProperty("location", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public GeoLocation Location { get; set; }

	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("dbInnerId")]
	public int DBId { get; set; }
}
