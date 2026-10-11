using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class RP : AccountPayment
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ViolationId { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ViolationTaskId { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ViolationFullName { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverAcc { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public decimal ViolationDebt { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ViolationDetails { get; set; }
}
