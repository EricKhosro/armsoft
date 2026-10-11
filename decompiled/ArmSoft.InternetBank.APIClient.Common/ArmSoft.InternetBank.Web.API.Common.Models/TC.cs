using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class TC : AccountPayment
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverCardNumber { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverCountryCode { get; set; }
}
