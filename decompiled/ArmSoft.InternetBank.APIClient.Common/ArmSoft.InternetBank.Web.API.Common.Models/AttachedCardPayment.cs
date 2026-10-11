using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AttachedCardPayment : Payment
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public int AttachedCardId { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string AttachedCardNumber { get; set; }
}
