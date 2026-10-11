using System.Runtime.InteropServices;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[Guid("AEC6EFAB-4C34-47C1-9C56-B9FCB414A4BB")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class PF : AccountPayment
{
	public string PayerPartyIdentifierCode { get; set; }

	public string PayerPartyIdentifierValue { get; set; }

	public string PayerOption { get; set; }

	public string PayerNameCode { get; set; }

	public string PayerAddressCode { get; set; }

	public string PayerCityCode { get; set; }

	public string PayerCountryCode { get; set; }

	public string PayerNameValue { get; set; }

	public string PayerAddressValue { get; set; }

	public string PayerCityValue { get; set; }

	public string PayerCountryValue { get; set; }

	public string ReceiverAcc { get; set; }

	public string ReceiverAddress { get; set; }

	public string ReceiverCity { get; set; }

	public string ReceiverCountry { get; set; }

	public string ReceivingCountryCode { get; set; }

	public string ReceiverResidenceCountry { get; set; }

	public ClientStatus ReceiverLegalStatus { get; set; }

	public string TransferPurpose { get; set; }

	public string ReceiverBankSwift { get; set; }

	public string ReceiverBankID { get; set; }

	public string ReceiverBankName { get; set; }

	public string ReceiverBankAddress { get; set; }

	public string ReceiverBankCity { get; set; }

	public string ReceiverBankCountry { get; set; }

	public string IntermediaryBankSwift { get; set; }

	public string IntermediaryBankID { get; set; }

	public string IntermediaryBankName { get; set; }

	public string IntermediaryBankAddress { get; set; }

	public string IntermediaryBankCity { get; set; }

	public string IntermediaryBankCountry { get; set; }

	public string Details2 { get; set; }

	public string Details3 { get; set; }

	public string Details4 { get; set; }

	public bool IsTransferForInvoice { get; set; }

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

	public PF()
	{
		ReceiverLegalStatus = ClientStatus.NotSet;
	}
}
