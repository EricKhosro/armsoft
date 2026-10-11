using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class MCRow
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string UtilityCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public UtilityType UtilityType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string UtilityName { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string LocationCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string AbonentCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string AbonentID { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string AbonentName { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string AbonentAddress { get; set; }

	public bool AbonentType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public decimal Debt { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public decimal Amount { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ComDetails { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Special { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string DebtDate { get; set; }

	public AbonentSearchResult SearchResult { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<UtilitySpecialSearchFieldValue> UtilitySpecialSearchFields { get; set; }

	public decimal CommissionAmount { get; set; }
}
