using System;
using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class DocumentListItem
{
	public int? CreatorUserId { get; set; }

	public Guid DocId { get; set; }

	public string DocIdString => DocId.ToString();

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public byte[] TimeStamp { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Type { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string DisplayType { get; set; }

	public DocumentState State { get; set; }

	public string PayerAccOrCard { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string DisplayState { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string DisplaySubState { get; set; }

	public string DocNum { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime Date { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Details { get; set; }

	public bool CanBeSigned { get; set; }

	public bool CanBeDeleted { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Receiver { get; set; }

	public string ReceiverAcc { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public string UtilityCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public UtilityType UtilityType { get; set; }

	public string Category { get; set; }

	public decimal? Amount { get; set; }

	public string Currency { get; set; }

	public bool AreFirstSignaturesCompleted { get; set; }

	public bool AreSecondSignaturesCompleted { get; set; }

	public bool AreThirdSignaturesCompleted { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<Link> Links { get; set; }

	public string CreatorFirstName { get; set; }

	public string CreatorLastName { get; set; }
}
