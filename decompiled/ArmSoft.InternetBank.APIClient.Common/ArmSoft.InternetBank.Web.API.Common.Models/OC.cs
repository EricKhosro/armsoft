using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class OC : Application
{
	public string CreditTemplate { get; set; }

	public string CreditTemplateName { get; set; }

	public string CreditTemplateGroupName { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public decimal Amount { get; set; }

	public string Currency { get; set; }

	public Duration Duration { get; set; }

	public short RepaymentDay { get; set; }

	public CreditReceiveMethod ReceiveMethod { get; set; }

	public string AccountNumber { get; set; }

	public string CardNumber { get; set; }

	public int? AttachedCardId { get; set; }

	public decimal ActualPercent { get; set; }

	public decimal InterestRate { get; set; }

	public OpenCreditApplicationInformation Information { get; set; }

	public string ReceiverClientTaxCode { get; set; }

	public string ReceiverClientSocialCard { get; set; }

	public string ReceiverClientName { get; set; }

	public string ReceiverClientCode { get; set; }

	public string OfferCode { get; set; }

	public decimal RequestedAmount { get; set; }

	public string ProductDescription { get; set; }

	public bool IsClientSignatureRequired { get; set; }

	public bool IsSignedByPartner { get; set; }

	public bool IsEditableByPartner { get; set; }

	public string PartnerClientCode { get; set; }

	public decimal? ServiceFee { get; set; }

	public decimal? ServiceFeeRate { get; set; }

	public ServiceFeeRateDivisorCategory ServiceFeeRateDivisorCategory { get; set; }

	public string ServiceFeeRateDivisorCategoryDescription { get; set; }

	public List<CAField> SubjectiveScores { get; set; }
}
