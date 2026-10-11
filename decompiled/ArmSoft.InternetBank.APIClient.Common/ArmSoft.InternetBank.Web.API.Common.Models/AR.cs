using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AR : AttachedCardPayment
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string TaxCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string SocialCard { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Account { get; set; }
}
