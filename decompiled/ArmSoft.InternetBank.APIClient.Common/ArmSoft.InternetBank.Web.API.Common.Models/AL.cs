using System.Runtime.InteropServices;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[Guid("E2B510DB-7A37-4E51-9147-E10EAB24A614")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class AL : AccountPayment
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string PayerAddInfo { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string PayerAuthPerson { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverAcc { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverBank { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string TaxCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string SocialCard { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string PayerName { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public ClientStatus PayerLegalState { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public ResidenceType PayerResidenceType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string PayerPassportNo { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public PassportType PayerPassportType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public AdvancedPaymentMethod PaymentMethod { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string InnerType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string CreditPaymentType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string CreditCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string CreditContractOwner { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverTaxCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverSocialCard { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public ClientStatus ReceiverLegalState { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverPassportNo { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public PassportType ReceiverPassportType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverAddress { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverAuthPerson { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ReceiverAddInfo { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string AdditionalInfo1 { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string AdditionalInfo2 { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string AdditionalInfo3 { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string AdditionalInfo4 { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string AdditionalInfo5 { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string AdditionalInfo6 { get; set; }
}
