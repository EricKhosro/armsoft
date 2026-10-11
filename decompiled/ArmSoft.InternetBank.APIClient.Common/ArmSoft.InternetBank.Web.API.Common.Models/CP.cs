using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CP : AccountPayment
{
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

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public decimal Debt { get; set; }

	public bool AbonentType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ComDetails { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Special { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<UtilitySpecialSearchFieldValue> UtilitySpecialSearchFields { get; set; }
}
