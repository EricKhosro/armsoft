using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[Guid("64AA4EFE-52B0-4794-97FC-121728A93D42")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class PO : AccountPayment
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string TaxCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string SocialCard { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverAcc { get; set; }

	public string ReceiverBank { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public ReceiverLegalStatus ReceiverLegalStatus { get; set; }

	public PaymentMode PaymentMode { get; set; }

	public string CurrencyCredit { get; set; }

	public decimal AmountCredit { get; set; }

	public string RateCurrency1 { get; set; }

	public string RateCurrency2 { get; set; }

	public short RateFor { get; set; }

	public decimal RateValue { get; set; }

	public string DisplayRateValue { get; set; }

	public decimal BankRateValue { get; set; }

	public string FixedCurrency { get; set; }

	public bool IsRateChanged { get; set; }

	public bool IsUrgentTransfer { get; set; }

	public string PODetailsDefault { get; set; }

	public string CCDetailsDefault { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	[Obsolete]
	public string CCDocumentInfo { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<DocumentInfo> CCDocumentInfos { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime DefaultDate => DateTime.Now;

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<DocumentLink> CCDocumentLinks { get; set; }

	public PO()
	{
		IsRateChanged = true;
	}
}
