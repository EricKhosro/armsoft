using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArmSoft.b2b.V2;

public class Suggestion
{
	public int Id { get; set; }

	[JsonProperty("property")]
	public string Property { get; set; }

	[JsonProperty("value")]
	public string Value { get; set; }

	[JsonProperty("status")]
	[JsonConverter(typeof(StringEnumConverter))]
	public SuggestionStatus Status { get; set; }

	[JsonProperty("state")]
	public AppStateEnum State { get; set; }

	[JsonProperty("creationDate")]
	public DateTime CreationDate { get; set; }
}
