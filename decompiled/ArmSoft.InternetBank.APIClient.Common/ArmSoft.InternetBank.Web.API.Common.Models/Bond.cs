using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class Bond
{
	public int Id { get; set; }

	public string Isin { get; set; }

	public string ContractNumber { get; set; }

	public string DisplayContract { get; set; }

	public string Currency { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime OriginalDate { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime MaturityDate { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime? ClosingDate { get; set; }

	public string InterestRate { get; set; }

	public string Note { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public decimal? OutstandingPrincipal { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public decimal? CalculatedInterest { get; set; }

	public List<Link> Links { get; set; }
}
