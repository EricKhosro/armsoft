using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class EB : Application
{
	public string PayerAcc { get; set; }

	public string Currency { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public decimal Amount { get; set; }

	public string BondTypeId { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public int BondsCount { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public decimal NominalAmount { get; set; }

	public string Email { get; set; }
}
