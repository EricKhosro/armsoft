using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class IBPayment
{
	public DateTime Date { get; set; }

	public decimal Amount { get; set; }

	public decimal AmountAMD { get; set; }

	public string Currency { get; set; }

	public string Details { get; set; }

	public Guid DocId { get; set; }

	public byte DocState { get; set; }

	public short DocType { get; set; }

	public string DocNum { get; set; }

	public string PayerAcc { get; set; }

	public string PayerTaxCode { get; set; }

	public string TaxAuthorityCode { get; set; }

	public string PayerName { get; set; }

	public string ReceiverAcc { get; set; }

	public string ReceiverName { get; set; }

	public ReceiverLegalStatus ReceiverLegalStatus { get; set; }

	public string ReceiverPassportNum { get; set; }

	public bool IsManuallyEntered { get; set; }

	public bool IsThirdParty { get; set; }

	public ResidenceType ThirdPartyResidenceType { get; set; }

	public ClientStatus ThirdPartyLegalState { get; set; }

	public PassportType ThirdPartyPassportType { get; set; }

	public string ThirdPartyPassportNo { get; set; }

	public string ThirdPartyAddress { get; set; }

	public string ThirdPartyTaxCode { get; set; }

	public string ThirdPartySocialCard { get; set; }

	public string ThirdPartyName { get; set; }

	public string ThirdPartyAdditionalInfo { get; set; }
}
