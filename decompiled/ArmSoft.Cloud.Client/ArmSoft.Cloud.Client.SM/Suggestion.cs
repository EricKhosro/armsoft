using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArmSoft.Cloud.Client.SM;

public class Suggestion
{
	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("property")]
	public string Property { get; set; }

	[JsonProperty("value")]
	public string Value { get; set; }

	[JsonProperty("status")]
	[JsonConverter(typeof(StringEnumConverter))]
	public SuggestionStatus Status { get; set; }

	[JsonProperty("state")]
	[JsonConverter(typeof(StringEnumConverter))]
	public AppStateEnum State { get; set; }
}
