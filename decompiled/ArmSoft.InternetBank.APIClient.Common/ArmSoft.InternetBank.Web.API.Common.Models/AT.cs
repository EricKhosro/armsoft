using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AT : AccountPayment
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverContactData { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverBankArCaCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverBankName { get; set; }

	public string TransactionRelativeNumber { get; set; }
}
