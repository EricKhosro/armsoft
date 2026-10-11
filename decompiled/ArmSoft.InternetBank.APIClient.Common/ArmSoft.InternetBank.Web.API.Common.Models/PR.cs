using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class PR : AccountPayment
{
	public string PayerName { get; set; }

	public string PayerAddress { get; set; }

	public string PayerCity { get; set; }

	public string PayerCountry { get; set; }

	public string TaxCode { get; set; }

	public string ReceiverAcc { get; set; }

	public string ReceiverINN { get; set; }

	public string ReceiverKPP { get; set; }

	public string ReceiverAddress { get; set; }

	public string ReceiverCity { get; set; }

	public string ReceiverCountry { get; set; }

	public string ReceivingCountryCode { get; set; }

	public string ReceiverResidenceCountry { get; set; }

	public ClientStatus ReceiverLegalStatus { get; set; }

	public string TransferPurpose { get; set; }

	public string ReceiverBankBIC { get; set; }

	public string ReceiverBankSwift { get; set; }

	public string ReceiverBankID { get; set; }

	public string ReceiverBankName { get; set; }

	public string ReceiverBankAddress { get; set; }

	public string ReceiverBankCity { get; set; }

	public string ReceiverBankCountry { get; set; }

	public string IntermediaryBankBIC { get; set; }

	public string IntermediaryBankSwift { get; set; }

	public string IntermediaryBankID { get; set; }

	public string IntermediaryBankName { get; set; }

	public string IntermediaryBankAddress { get; set; }

	public string IntermediaryBankCity { get; set; }

	public string IntermediaryBankCountry { get; set; }

	public string Details2 { get; set; }

	public string Details3 { get; set; }

	public string Details4 { get; set; }

	public string Charges { get; set; }

	public new string ChargeAcc { get; set; }

	public bool IsSelfCharged { get; set; }

	public bool IgnoreCommissionChange { get; set; }

	public string AdditionalInfo { get; set; }

	public string AdditionalInfo2 { get; set; }

	public string AdditionalInfo3 { get; set; }

	public string AdditionalInfo4 { get; set; }

	public string AdditionalInfo5 { get; set; }

	public string AdditionalInfo6 { get; set; }
}
