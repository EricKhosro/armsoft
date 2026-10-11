using System;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CW : Application
{
	public string PayerAcc { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public decimal Amount { get; set; }

	public string Currency { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime WithdrawalDate { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string BranchCode { get; set; }

	public bool IsReceiverAnotherClient { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Receiver { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverLastName { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverPassportNum { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverPassportType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverPassportAuthority { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	[JsonConverter(typeof(DateConverter))]
	public DateTime? ReceiverPassportDateOfIssue { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	[JsonConverter(typeof(DateConverter))]
	public DateTime? ReceiverPassportExpirationDate { get; set; }
}
