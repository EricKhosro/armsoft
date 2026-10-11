using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class BP : AccountPayment
{
	public string TaxCode { get; set; }

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
	public string ReceiverAcc { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string TaxAuthorityCode { get; set; }

	public bool IsSelfEmployed { get; set; }

	public bool IsThirdParty { get; set; }

	public string ThirdPartyTaxCode { get; set; }

	public string ThirdPartySocialCard { get; set; }

	public ResidenceType ThirdPartyResidenceType { get; set; }

	public ClientStatus ThirdPartyLegalState { get; set; }

	public string ThirdPartyName { get; set; }

	public string ThirdPartyPassportNo { get; set; }

	public PassportType ThirdPartyPassportType { get; set; }

	public string ThirdPartyAddress { get; set; }

	public string ThirdPartyAuthPerson { get; set; }

	public string ThirdPartyAddInfo { get; set; }

	public string ReportCode { get; set; }

	public string AdditionalInfo { get; set; }

	public string AdditionalInfo2 { get; set; }

	public string AdditionalInfo3 { get; set; }

	public string AdditionalInfo4 { get; set; }

	public string AdditionalInfo5 { get; set; }

	public string AdditionalInfo6 { get; set; }

	public bool IsUrgentTransfer { get; set; }
}
