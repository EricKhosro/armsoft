using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.SM;

public class CustomerSuggestion
{
	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("customerInnerId")]
	public int CustomerInnerId { get; set; }

	[JsonProperty("suggestions", NullValueHandling = NullValueHandling.Ignore)]
	public List<Suggestion> Suggestions { get; set; }
}
