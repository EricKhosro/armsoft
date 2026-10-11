using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CT : AccountPayment
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverContactData { get; set; }
}
