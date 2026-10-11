using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ArmSoft.OpenBanking.Api.Client.Core;

public class ValidationProblemDetails
{
	public string? Title { get; set; }

	[JsonPropertyName("errors")]
	public IDictionary<string, string[]> Errors { get; set; }
}
