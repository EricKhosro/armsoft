using System.Collections.Generic;
using System.Runtime.InteropServices;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
[Guid("416CCFA9-E67A-4A4C-8F8E-520390926642")]
public class Payment : Document
{
	private string mPayerAccOrCard;

	private string mReceiverAccOrCard;

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ExternalPaymentId { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public bool CreatedByAdmin { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Receiver { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public decimal Amount { get; set; }

	public string Currency { get; set; }

	[ComVisible(false)]
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<Link> Links { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string PayerAccOrCard
	{
		get
		{
			return mPayerAccOrCard;
		}
		internal set
		{
			mPayerAccOrCard = value;
		}
	}

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverAccOrCard
	{
		get
		{
			return mReceiverAccOrCard;
		}
		internal set
		{
			mReceiverAccOrCard = value;
		}
	}

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public string UtilityCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public UtilityType UtilityType { get; set; }
}
