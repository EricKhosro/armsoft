using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AccountDetails : Account
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public AccountOverdraftInfo OverdraftInfo { get; set; }
}
