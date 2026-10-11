using Newtonsoft.Json;

namespace ArmSoft.b2b.V2;

public class ErrorResponse
{
	public string UserMessage { get; set; }

	public string InternalMessage { get; set; }

	[JsonProperty("UserErrorCode")]
	public int UserCode { get; set; }

	public int Severity { get; set; }

	public override string ToString()
	{
		if (!string.IsNullOrEmpty(UserMessage))
		{
			return UserMessage;
		}
		return InternalMessage;
	}
}
