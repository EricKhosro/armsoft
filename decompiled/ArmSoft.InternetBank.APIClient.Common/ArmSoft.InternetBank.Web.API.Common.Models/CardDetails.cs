using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CardDetails : Card
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public decimal? OnlineBalance { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public AccountOverdraftInfo OverdraftInfo { get; set; }

	public CardStatus CardStatus { get; set; }

	public string DisplayCardStatus { get; set; }
}
