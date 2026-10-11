using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class Card
{
	public int Id { get; set; }

	public int ExternalId { get; set; }

	public string CardNumber { get; set; }

	public string DisplayCardNumber { get; set; }

	public string AccountNumber { get; set; }

	public string DisplayAccountNumber { get; set; }

	public decimal? InterestRate { get; set; }

	public string Currency { get; set; }

	public string CardType { get; set; }

	public string DisplayCardType { get; set; }

	public string CardTypeColor { get; set; }

	public string Design { get; set; }

	public int? DesignId { get; set; }

	public string ImageUrl { get; set; }

	public string SmallImageUrl { get; set; }

	public byte CardTypeGlobal { get; set; }

	public string Name { get; set; }

	public string EMail { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime FromDate { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime ToDate { get; set; }

	public string Note { get; set; }

	public string MainCardNumber { get; set; }

	public string MainCardNumberWithoutMask { get; set; }

	public bool IsDigital { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public decimal? AccountBalance { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public decimal? ExpectedBalance { get; set; }

	public List<Link> Links { get; set; }
}
