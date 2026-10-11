using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class BaseBudgetaryPayment : AccountPayment
{
	public string ReceiverAccount { get; set; } = string.Empty;

	public string TaxCode { get; set; } = string.Empty;

	public string SocialCard { get; set; } = string.Empty;

	public bool IsSelfEmployed { get; set; }

	public string TaxAuthorityCode { get; set; } = string.Empty;

	public ClientStatus PayerLegalState { get; set; }

	public ResidenceType PayerResidenceType { get; set; }

	public string PayerPassportNumber { get; set; } = string.Empty;

	public PassportType PayerPassportType { get; set; }

	public bool IsThirdParty { get; set; }

	public string ThirdPartyTaxCode { get; set; } = string.Empty;

	public string ThirdPartySocialCard { get; set; } = string.Empty;

	public ResidenceType ThirdPartyResidenceType { get; set; }

	public ClientStatus ThirdPartyLegalState { get; set; }

	public string ThirdPartyName { get; set; } = string.Empty;

	public string ThirdPartyPassportNumber { get; set; } = string.Empty;

	public PassportType ThirdPartyPassportType { get; set; }

	public string ThirdPartyAddress { get; set; } = string.Empty;

	public string ThirdPartyAuthorizedPerson { get; set; } = string.Empty;

	public string ThirdPartyAdditionalInfo { get; set; } = string.Empty;

	public string ReportCode { get; set; } = string.Empty;

	public string AdditionalInfo { get; set; } = string.Empty;

	public string AdditionalInfo2 { get; set; } = string.Empty;

	public string AdditionalInfo3 { get; set; } = string.Empty;

	public string AdditionalInfo4 { get; set; } = string.Empty;

	public string AdditionalInfo5 { get; set; } = string.Empty;

	public string AdditionalInfo6 { get; set; } = string.Empty;

	public bool IsUrgentTransfer { get; set; }
}
