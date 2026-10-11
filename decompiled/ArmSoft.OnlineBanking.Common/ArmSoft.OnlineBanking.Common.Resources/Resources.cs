using System.Reflection;
using System.Threading;

namespace ArmSoft.OnlineBanking.Common.Resources;

internal static class Resources
{
	private static string language = string.Empty;

	internal static string Language
	{
		get
		{
			if (!string.IsNullOrEmpty(language))
			{
				return language;
			}
			return Thread.CurrentThread.CurrentUICulture.Name;
		}
		set
		{
			language = value;
		}
	}

	internal static string Field_Date => GetField_Date(Language);

	internal static string Field_Id => GetField_Id(Language);

	internal static string Field_PayerAcc => GetField_PayerAcc(Language);

	internal static string Field_PayerName => GetField_PayerName(Language);

	internal static string Field_ContractNo => GetField_ContractNo(Language);

	internal static string Field_Receiver => GetField_Receiver(Language);

	internal static string Field_DepositAttraction_Receiver => GetField_DepositAttraction_Receiver(Language);

	internal static string Field_ReceiverBank => GetField_ReceiverBank(Language);

	internal static string Field_PayerBank => GetField_PayerBank(Language);

	internal static string Field_AmountInWords => GetField_AmountInWords(Language);

	internal static string Field_Details => GetField_Details(Language);

	internal static string Field_Currency => GetField_Currency(Language);

	internal static string Field_Amount => GetField_Amount(Language);

	internal static string Field_ReceiverPassportNumber => GetField_ReceiverPassportNumber(Language);

	internal static string Zero => GetZero(Language);

	internal static string One => GetOne(Language);

	internal static string Two => GetTwo(Language);

	internal static string Three => GetThree(Language);

	internal static string Four => GetFour(Language);

	internal static string Five => GetFive(Language);

	internal static string Six => GetSix(Language);

	internal static string Seven => GetSeven(Language);

	internal static string Eight => GetEight(Language);

	internal static string Nine => GetNine(Language);

	internal static string Ten => GetTen(Language);

	internal static string Eleven => GetEleven(Language);

	internal static string Twelve => GetTwelve(Language);

	internal static string Thirteen => GetThirteen(Language);

	internal static string Fourteen => GetFourteen(Language);

	internal static string Fifteen => GetFifteen(Language);

	internal static string Sixteen => GetSixteen(Language);

	internal static string Seventeen => GetSeventeen(Language);

	internal static string Eighteen => GetEighteen(Language);

	internal static string Nineteen => GetNineteen(Language);

	internal static string Twenty => GetTwenty(Language);

	internal static string Thirty => GetThirty(Language);

	internal static string Forty => GetForty(Language);

	internal static string Fifty => GetFifty(Language);

	internal static string Sixty => GetSixty(Language);

	internal static string Seventy => GetSeventy(Language);

	internal static string Eighty => GetEighty(Language);

	internal static string Ninety => GetNinety(Language);

	internal static string Hundred => GetHundred(Language);

	internal static string Thousand => GetThousand(Language);

	internal static string Million => GetMillion(Language);

	internal static string Milliard => GetMilliard(Language);

	internal static string Trillion => GetTrillion(Language);

	internal static string Yes => GetYes(Language);

	internal static string No => GetNo(Language);

	internal static string DocumentStateCanceled => GetDocumentStateCanceled(Language);

	internal static string DocumentStateConfirmed => GetDocumentStateConfirmed(Language);

	internal static string DocumentStateEditable => GetDocumentStateEditable(Language);

	internal static string DocumentStateAccepted => GetDocumentStateAccepted(Language);

	internal static string DocumentStateInProcessBank => GetDocumentStateInProcessBank(Language);

	internal static string DocumentStateNotSet => GetDocumentStateNotSet(Language);

	internal static string DocumentStateReady => GetDocumentStateReady(Language);

	internal static string DocumentStateReadyToBeSent => GetDocumentStateReadyToBeSent(Language);

	internal static string DocumentStateRefused => GetDocumentStateRefused(Language);

	internal static string DocumentStateSent => GetDocumentStateSent(Language);

	internal static string DocumentStateInProcessOnline => GetDocumentStateInProcessOnline(Language);

	internal static string DocumentStateTemplate => GetDocumentStateTemplate(Language);

	internal static string DocumentStatePartiallySigned => GetDocumentStatePartiallySigned(Language);

	internal static string DocumentStateClientSignature => GetDocumentStateClientSignature(Language);

	internal static string SecurityProviderDigipassGoSeries => GetSecurityProviderDigipassGoSeries(Language);

	internal static string SecurityProviderDigipass250300Series => GetSecurityProviderDigipass250300Series(Language);

	internal static string SecurityProviderDigipassMobile => GetSecurityProviderDigipassMobile(Language);

	internal static string SecurityProviderMobileSMS => GetSecurityProviderMobileSMS(Language);

	internal static string SecurityProviderGoogleAuth => GetSecurityProviderGoogleAuth(Language);

	internal static string SecurityProviderDigipass => GetSecurityProviderDigipass(Language);

	internal static string SecurityProviderDeactivate => GetSecurityProviderDeactivate(Language);

	internal static string SecurityProviderNone => GetSecurityProviderNone(Language);

	internal static string SystemTypeAccountant => GetSystemTypeAccountant(Language);

	internal static string SystemTypeImportService => GetSystemTypeImportService(Language);

	internal static string SystemTypeMobileSite => GetSystemTypeMobileSite(Language);

	internal static string SystemTypeWebSite => GetSystemTypeWebSite(Language);

	internal static string SystemTypeMobileApplication => GetSystemTypeMobileApplication(Language);

	internal static string SystemTypeAdminWebSite => GetSystemTypeAdminWebSite(Language);

	internal static string SystemTypeWorkerService => GetSystemTypeWorkerService(Language);

	internal static string SystemTypeWeb => GetSystemTypeWeb(Language);

	internal static string SystemTypeCorporateApi => GetSystemTypeCorporateApi(Language);

	internal static string SystemTypeAndroid => GetSystemTypeAndroid(Language);

	internal static string SystemTypeIOS => GetSystemTypeIOS(Language);

	internal static string SystemTypeAll => GetSystemTypeAll(Language);

	internal static string SystemTypeOther => GetSystemTypeOther(Language);

	internal static string Field_AdditionalInfo => GetField_AdditionalInfo(Language);

	internal static string Field_AdditionalInfo2 => GetField_AdditionalInfo2(Language);

	internal static string Field_AdditionalInfo3 => GetField_AdditionalInfo3(Language);

	internal static string Field_AdditionalInfo4 => GetField_AdditionalInfo4(Language);

	internal static string Field_AdditionalInfo5 => GetField_AdditionalInfo5(Language);

	internal static string Field_AdditionalInfo6 => GetField_AdditionalInfo6(Language);

	internal static string Field_TaxAuthorityCode => GetField_TaxAuthorityCode(Language);

	internal static string Field_IsThirdParty => GetField_IsThirdParty(Language);

	internal static string Field_ReportCode => GetField_ReportCode(Language);

	internal static string Field_ThirdPartyAddInfo => GetField_ThirdPartyAddInfo(Language);

	internal static string Field_ThirdPartyAdditionalInfo => GetField_ThirdPartyAdditionalInfo(Language);

	internal static string Field_ThirdPartyAddress => GetField_ThirdPartyAddress(Language);

	internal static string Field_ThirdPartyAuthPerson => GetField_ThirdPartyAuthPerson(Language);

	internal static string Field_ThirdPartyAuthorizedPerson => GetField_ThirdPartyAuthorizedPerson(Language);

	internal static string Field_ThirdPartyLegalState => GetField_ThirdPartyLegalState(Language);

	internal static string Field_ThirdPartyPassportNo => GetField_ThirdPartyPassportNo(Language);

	internal static string Field_ThirdPartyPassportNumber => GetField_ThirdPartyPassportNumber(Language);

	internal static string Field_ThirdPartyResidenceType => GetField_ThirdPartyResidenceType(Language);

	internal static string Field_ThirdPartySocialCard => GetField_ThirdPartySocialCard(Language);

	internal static string Field_ThirdPartyTaxCode => GetField_ThirdPartyTaxCode(Language);

	internal static string Field_ThirdPartyPassportType => GetField_ThirdPartyPassportType(Language);

	internal static string Field_IsUrgentTransfer => GetField_IsUrgentTransfer(Language);

	internal static string SectionTitlePayerBank => GetSectionTitlePayerBank(Language);

	internal static string SectionTitleBeneficiaryBank => GetSectionTitleBeneficiaryBank(Language);

	internal static string Field_ReceiverAcc => GetField_ReceiverAcc(Language);

	internal static string Field_TaxCode => GetField_TaxCode(Language);

	internal static string Field_SocialCard => GetField_SocialCard(Language);

	internal static string Field_PayerLegalState => GetField_PayerLegalState(Language);

	internal static string Field_PayerPassportNo => GetField_PayerPassportNo(Language);

	internal static string Field_PayerPassportNumber => GetField_PayerPassportNumber(Language);

	internal static string Field_PayerPassportType => GetField_PayerPassportType(Language);

	internal static string Field_PayerResidenceType => GetField_PayerResidenceType(Language);

	internal static string Field_CommissionAmount => GetField_CommissionAmount(Language);

	internal static string Field_CommissionAmountInWords => GetField_CommissionAmountInWords(Language);

	internal static string PassportDeathCertificate => GetPassportDeathCertificate(Language);

	internal static string PassportForeign => GetPassportForeign(Language);

	internal static string PassportRA => GetPassportRA(Language);

	internal static string ClientStatusLegal => GetClientStatusLegal(Language);

	internal static string ClientStatusNatural => GetClientStatusNatural(Language);

	internal static string ClientStatusEntrepreneur => GetClientStatusEntrepreneur(Language);

	internal static string ReceiverLegalStatusLegal => GetReceiverLegalStatusLegal(Language);

	internal static string ClientStatusAll => GetClientStatusAll(Language);

	internal static string NonResident => GetNonResident(Language);

	internal static string Resident => GetResident(Language);

	internal static string Field_RoadPolicePayment_ReceiverAcc => GetField_RoadPolicePayment_ReceiverAcc(Language);

	internal static string Field_RoadPolicePayment_ViolationDebt => GetField_RoadPolicePayment_ViolationDebt(Language);

	internal static string Field_RoadPolicePayment_ViolationDetails => GetField_RoadPolicePayment_ViolationDetails(Language);

	internal static string Field_RoadPolicePayment_ViolationFullName => GetField_RoadPolicePayment_ViolationFullName(Language);

	internal static string Field_RoadPolicePayment_ViolationId => GetField_RoadPolicePayment_ViolationId(Language);

	internal static string DocumentTypePassportPayment => GetDocumentTypePassportPayment(Language);

	internal static string DocumentTypeDepositAttraction => GetDocumentTypeDepositAttraction(Language);

	internal static string DocumentTypeOpenAccountApplication => GetDocumentTypeOpenAccountApplication(Language);

	internal static string DocumentTypeCloseAccountApplication => GetDocumentTypeCloseAccountApplication(Language);

	internal static string DocumentTypeBudgetaryPayment => GetDocumentTypeBudgetaryPayment(Language);

	internal static string DocumentTypeArmSoftApplication => GetDocumentTypeArmSoftApplication(Language);

	internal static string DocumentTypeCreditRepaymentViaPartner => GetDocumentTypeCreditRepaymentViaPartner(Language);

	internal static string DocumentTypeCashWithdrawalApplication => GetDocumentTypeCashWithdrawalApplication(Language);

	internal static string DocumentTypeCommunalPayment => GetDocumentTypeCommunalPayment(Language);

	internal static string DocumentTypeContactTransfer => GetDocumentTypeContactTransfer(Language);

	internal static string DocumentTypeArCaP2PTransfer => GetDocumentTypeArCaP2PTransfer(Language);

	internal static string DocumentTypeCreditRepayment => GetDocumentTypeCreditRepayment(Language);

	internal static string DocumentTypeCreditRepaymentWithAttachedCard => GetDocumentTypeCreditRepaymentWithAttachedCard(Language);

	internal static string DocumentTypeCurrencyConversion => GetDocumentTypeCurrencyConversion(Language);

	internal static string DocumentTypeCustomizedApplication => GetDocumentTypeCustomizedApplication(Language);

	internal static string DocumentTypeForeignPayment => GetDocumentTypeForeignPayment(Language);

	internal static string DocumentTypeForeignPaymentRus => GetDocumentTypeForeignPaymentRus(Language);

	internal static string DocumentTypeLocalPayment => GetDocumentTypeLocalPayment(Language);

	internal static string DocumentTypeGarbageCollectionTaxPayment => GetDocumentTypeGarbageCollectionTaxPayment(Language);

	internal static string DocumentTypeGoalAccountClose => GetDocumentTypeGoalAccountClose(Language);

	internal static string DocumentTypeMultipleCommunal => GetDocumentTypeMultipleCommunal(Language);

	internal static string DocumentTypeMultiplePayment => GetDocumentTypeMultiplePayment(Language);

	internal static string DocumentTypeOpenDepositApplication => GetDocumentTypeOpenDepositApplication(Language);

	internal static string DocumentTypeOpenCreditApplication => GetDocumentTypeOpenCreditApplication(Language);

	internal static string DocumentTypeOpenDigitalCardApplication => GetDocumentTypeOpenDigitalCardApplication(Language);

	internal static string DocumentTypePaymentsPackage => GetDocumentTypePaymentsPackage(Language);

	internal static string DocumentTypeRequestCancel => GetDocumentTypeRequestCancel(Language);

	internal static string DocumentTypeRoadPolicePayment => GetDocumentTypeRoadPolicePayment(Language);

	internal static string DocumentTypeSoldierPayment => GetDocumentTypeSoldierPayment(Language);

	internal static string DocumentTypeTaxPayment => GetDocumentTypeTaxPayment(Language);

	internal static string DocumentTypeGarbageCollectionPayment => GetDocumentTypeGarbageCollectionPayment(Language);

	internal static string DocumentTypeEvocaGiftCardApplication => GetDocumentTypeEvocaGiftCardApplication(Language);

	internal static string DocumentTypeEvocaDigitalGiftCardApplication => GetDocumentTypeEvocaDigitalGiftCardApplication(Language);

	internal static string DocumentTypeEvocaGetBondApplication => GetDocumentTypeEvocaGetBondApplication(Language);

	internal static string DocumentTypePropertyTaxPayment => GetDocumentTypePropertyTaxPayment(Language);

	internal static string DocumentTypeVehicleTaxPayment => GetDocumentTypeVehicleTaxPayment(Language);

	internal static string DocumentTypeRealEstateTaxPayment => GetDocumentTypeRealEstateTaxPayment(Language);

	internal static string OtherTransfer => GetOtherTransfer(Language);

	internal static string DocumentTypeAccountReplenishment => GetDocumentTypeAccountReplenishment(Language);

	internal static string AccountReplenishment => GetAccountReplenishment(Language);

	internal static string DocumentTypeCardTransfer => GetDocumentTypeCardTransfer(Language);

	internal static string DocumentTypeFriendCreditRepayment => GetDocumentTypeFriendCreditRepayment(Language);

	internal static string DocumentTypeCashOutByPhone => GetDocumentTypeCashOutByPhone(Language);

	internal static string DocumentTypeCompulsoryEnforcementPayment => GetDocumentTypeCompulsoryEnforcementPayment(Language);

	internal static string DocumentTypeASWAPayment => GetDocumentTypeASWAPayment(Language);

	internal static string DocumentTypeEvocaGetLoanApplication => GetDocumentTypeEvocaGetLoanApplication(Language);

	internal static string DocumentTypeAmioLoanApplication => GetDocumentTypeAmioLoanApplication(Language);

	internal static string AdvancedPaymentCreditRepayment => GetAdvancedPaymentCreditRepayment(Language);

	internal static string DocumentTypeAdvancedTransfer => GetDocumentTypeAdvancedTransfer(Language);

	internal static string RateSuggestionStateNew => GetRateSuggestionStateNew(Language);

	internal static string RateSuggestionOperationCreated => GetRateSuggestionOperationCreated(Language);

	internal static string RateSuggestionStateAssigned => GetRateSuggestionStateAssigned(Language);

	internal static string RateSuggestionOperationAssigned => GetRateSuggestionOperationAssigned(Language);

	internal static string RateSuggestionOperationStarted => GetRateSuggestionOperationStarted(Language);

	internal static string RateSuggestionStateStarted => GetRateSuggestionStateStarted(Language);

	internal static string RateSuggestionStateAcceptedByClient => GetRateSuggestionStateAcceptedByClient(Language);

	internal static string RateSuggestionOperationAcceptedByClient => GetRateSuggestionOperationAcceptedByClient(Language);

	internal static string RateSuggestionStateSigned => GetRateSuggestionStateSigned(Language);

	internal static string RateSuggestionOperationSigned => GetRateSuggestionOperationSigned(Language);

	internal static string RateSuggestionStateConfirmedByBank => GetRateSuggestionStateConfirmedByBank(Language);

	internal static string RateSuggestionOperationConfirmedByBank => GetRateSuggestionOperationConfirmedByBank(Language);

	internal static string RateSuggestionStateRefusedByClient => GetRateSuggestionStateRefusedByClient(Language);

	internal static string RateSuggestionOperationRefusedByClient => GetRateSuggestionOperationRefusedByClient(Language);

	internal static string RateSuggestionStateRefusedByOperator => GetRateSuggestionStateRefusedByOperator(Language);

	internal static string RateSuggestionOperationRefusedByOperator => GetRateSuggestionOperationRefusedByOperator(Language);

	internal static string RateSuggestionStateRefusedByBank => GetRateSuggestionStateRefusedByBank(Language);

	internal static string RateSuggestionOperationRefusedByBank => GetRateSuggestionOperationRefusedByBank(Language);

	internal static string RateSuggestionStateExpired => GetRateSuggestionStateExpired(Language);

	internal static string RateSuggestionOperationExpired => GetRateSuggestionOperationExpired(Language);

	internal static string RateSuggestionStateGroupInProcess => GetRateSuggestionStateGroupInProcess(Language);

	internal static string RateSuggestionStateGroupWaitingToBeSigned => GetRateSuggestionStateGroupWaitingToBeSigned(Language);

	internal static string RateSuggestionStateGroupCompleted => GetRateSuggestionStateGroupCompleted(Language);

	internal static string RateSuggestionStateGroupAssigned => GetRateSuggestionStateGroupAssigned(Language);

	internal static string RateSuggestionStateGroupNew => GetRateSuggestionStateGroupNew(Language);

	internal static string RateSuggestionStateGroupAssignedToAnotherOperator => GetRateSuggestionStateGroupAssignedToAnotherOperator(Language);

	internal static string Field_CardTransfer_ReceiverCountryCode => GetField_CardTransfer_ReceiverCountryCode(Language);

	internal static string Field_ReceiverCountryCode => GetField_ReceiverCountryCode(Language);

	internal static string Field_ReceiverCardNumber => GetField_ReceiverCardNumber(Language);

	internal static string Field_ReceiverCardAccountNumber => GetField_ReceiverCardAccountNumber(Language);

	internal static string Field_DocNum => GetField_DocNum(Language);

	internal static string Field_Number => GetField_Number(Language);

	internal static string AccountTypeSettlement => GetAccountTypeSettlement(Language);

	internal static string AccountTypeCard => GetAccountTypeCard(Language);

	internal static string AccountTypeOther => GetAccountTypeOther(Language);

	internal static string Field_ReceiverId => GetField_ReceiverId(Language);

	internal static string Field_Attachment => GetField_Attachment(Language);

	internal static string CardStatusActive => GetCardStatusActive(Language);

	internal static string CardStatusBlocked => GetCardStatusBlocked(Language);

	internal static string CardStatusBlockedByBank => GetCardStatusBlockedByBank(Language);

	internal static string CardStatusExpired => GetCardStatusExpired(Language);

	internal static string CardStatusInactive => GetCardStatusInactive(Language);

	internal static string CardTypeVisa => GetCardTypeVisa(Language);

	internal static string CardTypeMasterCard => GetCardTypeMasterCard(Language);

	internal static string CardTypeArCa => GetCardTypeArCa(Language);

	internal static string CardTypeAmex => GetCardTypeAmex(Language);

	internal static string CardTypeDinersClub => GetCardTypeDinersClub(Language);

	internal static string CardTypeUnionPay => GetCardTypeUnionPay(Language);

	internal static string CardTypeJcb => GetCardTypeJcb(Language);

	internal static string CardTypeDiscoverCard => GetCardTypeDiscoverCard(Language);

	internal static string CardTypeMaestro => GetCardTypeMaestro(Language);

	internal static string CardTypeMir => GetCardTypeMir(Language);

	internal static string Field_AttachedCardPayment_CardNumber => GetField_AttachedCardPayment_CardNumber(Language);

	internal static string Field_AttachedCardPayment_AttachedCardId => GetField_AttachedCardPayment_AttachedCardId(Language);

	internal static string Field_OpenAccountApplication_AccountTypeId => GetField_OpenAccountApplication_AccountTypeId(Language);

	internal static string Field_Rate => GetField_Rate(Language);

	internal static string Field_CurrencyConversion_PayerAcc => GetField_CurrencyConversion_PayerAcc(Language);

	internal static string Field_CurrencyConversion_Amount => GetField_CurrencyConversion_Amount(Language);

	internal static string Field_CurrencyConversion_ReceiverAcc => GetField_CurrencyConversion_ReceiverAcc(Language);

	internal static string Field_AmountCredit => GetField_AmountCredit(Language);

	internal static string Field_UseBankRate => GetField_UseBankRate(Language);

	internal static string IdentificationDocumentTypeBirthCertificate => GetIdentificationDocumentTypeBirthCertificate(Language);

	internal static string IdentificationDocumentTypeForeignPassport => GetIdentificationDocumentTypeForeignPassport(Language);

	internal static string IdentificationDocumentTypeInternationallyRecognizedIdentityDocuments => GetIdentificationDocumentTypeInternationallyRecognizedIdentityDocuments(Language);

	internal static string IdentificationDocumentTypeMilitaryCard => GetIdentificationDocumentTypeMilitaryCard(Language);

	internal static string IdentificationDocumentTypeOther => GetIdentificationDocumentTypeOther(Language);

	internal static string IdentificationDocumentTypeRABiometricPassport => GetIdentificationDocumentTypeRABiometricPassport(Language);

	internal static string IdentificationDocumentTypeRAIDCard => GetIdentificationDocumentTypeRAIDCard(Language);

	internal static string IdentificationDocumentTypeRANonBiometricPassport => GetIdentificationDocumentTypeRANonBiometricPassport(Language);

	internal static string IdentificationDocumentTypeRAReturnCertificate => GetIdentificationDocumentTypeRAReturnCertificate(Language);

	internal static string IdentificationDocumentTypeRASpecialPassport => GetIdentificationDocumentTypeRASpecialPassport(Language);

	internal static string IdentificationDocumentTypeRefugeeCertificate => GetIdentificationDocumentTypeRefugeeCertificate(Language);

	internal static string IdentificationDocumentTypeResidenceCard => GetIdentificationDocumentTypeResidenceCard(Language);

	internal static string IdentificationDocumentTypeTemporaryAsylumCertificate => GetIdentificationDocumentTypeTemporaryAsylumCertificate(Language);

	internal static string IdentificationDocumentTypeTemporaryIdentityDocument => GetIdentificationDocumentTypeTemporaryIdentityDocument(Language);

	internal static string IdentificationDocumentTypeTemporaryResidenceCard => GetIdentificationDocumentTypeTemporaryResidenceCard(Language);

	internal static string IdentificationDocumentTypeTravelDocument => GetIdentificationDocumentTypeTravelDocument(Language);

	internal static string TransactionDirection_Credit => GetTransactionDirection_Credit(Language);

	internal static string TransactionDirection_Debit => GetTransactionDirection_Debit(Language);

	internal static string Field_ForeignPayment_PayerAddress => GetField_ForeignPayment_PayerAddress(Language);

	internal static string Field_ForeignPayment_PayerCity => GetField_ForeignPayment_PayerCity(Language);

	internal static string Field_ForeignPayment_PayerCountry => GetField_ForeignPayment_PayerCountry(Language);

	internal static string Field_ForeignPayment_PayerName => GetField_ForeignPayment_PayerName(Language);

	internal static string Field_ForeignPayment_PayerOption => GetField_ForeignPayment_PayerOption(Language);

	internal static string Field_ForeignPayment_ReferenceNo => GetField_ForeignPayment_ReferenceNo(Language);

	internal static string Field_IgnoreCommissionChange => GetField_IgnoreCommissionChange(Language);

	internal static string Field_IsSelfCharged => GetField_IsSelfCharged(Language);

	internal static string Field_Charges => GetField_Charges(Language);

	internal static string Field_ForeignPayment_AdditionalInfo => GetField_ForeignPayment_AdditionalInfo(Language);

	internal static string Field_ForeignPayment_AdditionalInfo2 => GetField_ForeignPayment_AdditionalInfo2(Language);

	internal static string Field_ForeignPayment_AdditionalInfo3 => GetField_ForeignPayment_AdditionalInfo3(Language);

	internal static string Field_ForeignPayment_AdditionalInfo4 => GetField_ForeignPayment_AdditionalInfo4(Language);

	internal static string Field_ForeignPayment_AdditionalInfo5 => GetField_ForeignPayment_AdditionalInfo5(Language);

	internal static string Field_ForeignPayment_AdditionalInfo6 => GetField_ForeignPayment_AdditionalInfo6(Language);

	internal static string Field_ReceivingCountryCode => GetField_ReceivingCountryCode(Language);

	internal static string Field_ReceiverResidenceCountry => GetField_ReceiverResidenceCountry(Language);

	internal static string Field_ReceiverLegalStatus => GetField_ReceiverLegalStatus(Language);

	internal static string Field_LocalPayment_ReceiverLegalStatus => GetField_LocalPayment_ReceiverLegalStatus(Language);

	internal static string Field_MultiplePayment_ReceiverLegalStatus => GetField_MultiplePayment_ReceiverLegalStatus(Language);

	internal static string Field_TransferPurpose => GetField_TransferPurpose(Language);

	internal static string Field_IntermediaryBankAddress => GetField_IntermediaryBankAddress(Language);

	internal static string Field_IntermediaryBankCity => GetField_IntermediaryBankCity(Language);

	internal static string Field_IntermediaryBankCountry => GetField_IntermediaryBankCountry(Language);

	internal static string Field_IntermediaryBankID => GetField_IntermediaryBankID(Language);

	internal static string Field_IntermediaryBankName => GetField_IntermediaryBankName(Language);

	internal static string Field_IntermediaryBankSwift => GetField_IntermediaryBankSwift(Language);

	internal static string Field_ReceiverAddress => GetField_ReceiverAddress(Language);

	internal static string Field_ReceiverBankAddress => GetField_ReceiverBankAddress(Language);

	internal static string Field_ReceiverBankCity => GetField_ReceiverBankCity(Language);

	internal static string Field_ReceiverBankCountry => GetField_ReceiverBankCountry(Language);

	internal static string Field_ReceiverBankID => GetField_ReceiverBankID(Language);

	internal static string Field_ReceiverBankName => GetField_ReceiverBankName(Language);

	internal static string Field_ReceiverBankSwift => GetField_ReceiverBankSwift(Language);

	internal static string Field_ForeignPayment_ReceiverAcc => GetField_ForeignPayment_ReceiverAcc(Language);

	internal static string Field_ReceiverCity => GetField_ReceiverCity(Language);

	internal static string Field_ReceiverCountry => GetField_ReceiverCountry(Language);

	internal static string Field_PayerBankCity => GetField_PayerBankCity(Language);

	internal static string Field_PayerBankCountry => GetField_PayerBankCountry(Language);

	internal static string Field_PayerBankID => GetField_PayerBankID(Language);

	internal static string Field_PayerBankName => GetField_PayerBankName(Language);

	internal static string Field_PayerBankAddress => GetField_PayerBankAddress(Language);

	internal static string Field_PayerBankSwift => GetField_PayerBankSwift(Language);

	internal static string ChargesBEN => GetChargesBEN(Language);

	internal static string ChargesOUR => GetChargesOUR(Language);

	internal static string ChargesOURG => GetChargesOURG(Language);

	internal static string ChargesSHA => GetChargesSHA(Language);

	internal static string Field_OptionF => GetField_OptionF(Language);

	internal static string Field_DefaultOption => GetField_DefaultOption(Language);

	internal static string NameAndAddressCodesAdditionalInformation => GetNameAndAddressCodesAdditionalInformation(Language);

	internal static string NameAndAddressCodesAddress => GetNameAndAddressCodesAddress(Language);

	internal static string NameAndAddressCodesBirthDate => GetNameAndAddressCodesBirthDate(Language);

	internal static string NameAndAddressCodesBirthPlace => GetNameAndAddressCodesBirthPlace(Language);

	internal static string NameAndAddressCodesCountryTown => GetNameAndAddressCodesCountryTown(Language);

	internal static string NameAndAddressCodesCustomerIdentificationNumber => GetNameAndAddressCodesCustomerIdentificationNumber(Language);

	internal static string NameAndAddressCodesName => GetNameAndAddressCodesName(Language);

	internal static string NameAndAddressCodesNationalIdentityNumber => GetNameAndAddressCodesNationalIdentityNumber(Language);

	internal static string Field_ChargeAcc => GetField_ChargeAcc(Language);

	internal static string EvocaMobileLoanType_Action => GetEvocaMobileLoanType_Action(Language);

	internal static string EvocaMobileLoanType_Overdraft => GetEvocaMobileLoanType_Overdraft(Language);

	internal static string PushStatusIsEnabled => GetPushStatusIsEnabled(Language);

	internal static string PushStatusIsDisabled => GetPushStatusIsDisabled(Language);

	internal static string PushStatusIsInactive => GetPushStatusIsInactive(Language);

	internal static string PushStatusIsTurnedOff => GetPushStatusIsTurnedOff(Language);

	internal static string AccountTransactionTypeTransfer => GetAccountTransactionTypeTransfer(Language);

	internal static string AccountTransactionTypeMiscellaneous => GetAccountTransactionTypeMiscellaneous(Language);

	internal static string AccountTransactionTypeRepayment => GetAccountTransactionTypeRepayment(Language);

	internal static string AccountTransactionTypeRevaluation => GetAccountTransactionTypeRevaluation(Language);

	internal static string AccountTransactionTypeInterestCalculation => GetAccountTransactionTypeInterestCalculation(Language);

	internal static string AccountTransactionTypeInterestCapitalization => GetAccountTransactionTypeInterestCapitalization(Language);

	internal static string AccountTransactionTypeCurrencyExchange => GetAccountTransactionTypeCurrencyExchange(Language);

	internal static string AccountTransactionTypeProvision => GetAccountTransactionTypeProvision(Language);

	internal static string AccountTransactionTypeCommission => GetAccountTransactionTypeCommission(Language);

	internal static string AccountTransactionTypeCommissionWithCurrencyExchange => GetAccountTransactionTypeCommissionWithCurrencyExchange(Language);

	internal static string AccountTransactionTypePenalty => GetAccountTransactionTypePenalty(Language);

	internal static string AccountTransactionTypePenaltyWithCurrencyExchange => GetAccountTransactionTypePenaltyWithCurrencyExchange(Language);

	internal static string AccountTransactionTypeInterest => GetAccountTransactionTypeInterest(Language);

	internal static string AccountTransactionTypeInterestWithCurrencyExchange => GetAccountTransactionTypeInterestWithCurrencyExchange(Language);

	internal static string AccountTransactionTypeOverdraftRepayment => GetAccountTransactionTypeOverdraftRepayment(Language);

	internal static string AccountTransactionTypeOverdraftRepaymentWithOtherAccount => GetAccountTransactionTypeOverdraftRepaymentWithOtherAccount(Language);

	internal static string AccountTransactionTypeOverdraftRepaymentWithCurrencyExchange => GetAccountTransactionTypeOverdraftRepaymentWithCurrencyExchange(Language);

	internal static string AccountTransactionTypeBalanceAdjustment => GetAccountTransactionTypeBalanceAdjustment(Language);

	internal static string CreditReceiveMethodCash => GetCreditReceiveMethodCash(Language);

	internal static string CreditReceiveMethodWithCard => GetCreditReceiveMethodWithCard(Language);

	internal static string CreditReceiveMethodWithAccount => GetCreditReceiveMethodWithAccount(Language);

	internal static string CreditReceiveMethodWithAttachedCard => GetCreditReceiveMethodWithAttachedCard(Language);

	internal static string OpenCreditFileTypeContract => GetOpenCreditFileTypeContract(Language);

	internal static string OpenCreditFileTypeArbitrage => GetOpenCreditFileTypeArbitrage(Language);

	internal static string OpenCreditFileTypeIndividualSheet => GetOpenCreditFileTypeIndividualSheet(Language);

	internal static string Field_IsSelfEmployed => GetField_IsSelfEmployed(Language);

	internal static string Field_CreditRepayment_RepaymentType => GetField_CreditRepayment_RepaymentType(Language);

	internal static string Field_CreditRepayment_Receiver => GetField_CreditRepayment_Receiver(Language);

	internal static string Field_CreditRepayment_IsFullRepayment => GetField_CreditRepayment_IsFullRepayment(Language);

	internal static string Field_CreditRepayment_OverpaymentPrincipal => GetField_CreditRepayment_OverpaymentPrincipal(Language);

	internal static string Field_OverpaymentFee => GetField_OverpaymentFee(Language);

	internal static string Field_OverpaymentPrincipalInWords => GetField_OverpaymentPrincipalInWords(Language);

	internal static string Field_OverpaymentFeeInWords => GetField_OverpaymentFeeInWords(Language);

	internal static string Field_AgreementScheduleRow_Total => GetField_AgreementScheduleRow_Total(Language);

	internal static string LoanScheduledPayment => GetLoanScheduledPayment(Language);

	internal static string PrepaymentOfPrincipal => GetPrepaymentOfPrincipal(Language);

	internal static string Field_MultiplePayment_Rows => GetField_MultiplePayment_Rows(Language);

	internal static string Field_MultiplePayment_Amount => GetField_MultiplePayment_Amount(Language);

	internal static string TaxPropertyTypeRealty => GetTaxPropertyTypeRealty(Language);

	internal static string TaxPropertyTypeTransport => GetTaxPropertyTypeTransport(Language);

	internal static string PropertyTaxDebtPeriodIncludingCurrentMonth => GetPropertyTaxDebtPeriodIncludingCurrentMonth(Language);

	internal static string PropertyTaxDebtPeriodIncludingCurrentYear => GetPropertyTaxDebtPeriodIncludingCurrentYear(Language);

	internal static string AdvancedPaymentCreditPaymentCombined => GetAdvancedPaymentCreditPaymentCombined(Language);

	internal static string AdvancedPaymentCreditPaymentInterest => GetAdvancedPaymentCreditPaymentInterest(Language);

	internal static string AdvancedPaymentCreditPaymentPenalty => GetAdvancedPaymentCreditPaymentPenalty(Language);

	internal static string AdvancedPaymentCreditPaymentPrincipal => GetAdvancedPaymentCreditPaymentPrincipal(Language);

	internal static string AdvancedPaymentMethodNoAccount => GetAdvancedPaymentMethodNoAccount(Language);

	internal static string AdvancedPaymentMethodToAccount => GetAdvancedPaymentMethodToAccount(Language);

	internal static string AnonymousPayment => GetAnonymousPayment(Language);

	internal static string PublicPayment => GetPublicPayment(Language);

	internal static string Field_GlobalCardType => GetField_GlobalCardType(Language);

	internal static string Field_ThemeId => GetField_ThemeId(Language);

	internal static string Field_Image => GetField_Image(Language);

	internal static string Field_SmallImage => GetField_SmallImage(Language);

	internal static string Field_CardDesign_ProfileId => GetField_CardDesign_ProfileId(Language);

	internal static string Field_CardDesign_Design => GetField_CardDesign_Design(Language);

	internal static string Field_Name => GetField_Name(Language);

	internal static string Field_CardDesignTheme_Title => GetField_CardDesignTheme_Title(Language);

	internal static string Field_CardTypes => GetField_CardTypes(Language);

	internal static string Field_IsDigital => GetField_IsDigital(Language);

	internal static string Field_State => GetField_State(Language);

	internal static string BondIssuanceTypeNonInterestBearing => GetBondIssuanceTypeNonInterestBearing(Language);

	internal static string BondIssuanceTypeInterestBearing => GetBondIssuanceTypeInterestBearing(Language);

	internal static string DocumentTypePurchaseBondApplication => GetDocumentTypePurchaseBondApplication(Language);

	internal static string Email => GetEmail(Language);

	internal static string PhoneNumber => GetPhoneNumber(Language);

	internal static string Address => GetAddress(Language);

	internal static string StatementDeliveryMethod => GetStatementDeliveryMethod(Language);

	internal static string Mail => GetMail(Language);

	internal static string Manually => GetManually(Language);

	internal static string Field_OpenAccountApplication_BranchCode => GetField_OpenAccountApplication_BranchCode(Language);

	internal static string Field_AccountStatement_Currency => GetField_AccountStatement_Currency(Language);

	internal static string Field_AccountStatement_OpeningBalance => GetField_AccountStatement_OpeningBalance(Language);

	internal static string Field_AccountStatement_Type => GetField_AccountStatement_Type(Language);

	internal static string Field_AccountStatement_Number => GetField_AccountStatement_Number(Language);

	internal static string Field_AccountStatement_TaxCode => GetField_AccountStatement_TaxCode(Language);

	internal static string Field_AccountStatement_OperationDate => GetField_AccountStatement_OperationDate(Language);

	internal static string Field_AccountStatement_DocumentNumber => GetField_AccountStatement_DocumentNumber(Language);

	internal static string Field_AccountStatement_OperationType => GetField_AccountStatement_OperationType(Language);

	internal static string Field_AccountStatement_CorrespondentAccount => GetField_AccountStatement_CorrespondentAccount(Language);

	internal static string Field_AccountStatement_CorrespondentName => GetField_AccountStatement_CorrespondentName(Language);

	internal static string Field_AccountStatement_CreditAmount => GetField_AccountStatement_CreditAmount(Language);

	internal static string Field_AccountStatement_DebitAmount => GetField_AccountStatement_DebitAmount(Language);

	internal static string Field_AccountStatement_CreditAmountAMD => GetField_AccountStatement_CreditAmountAMD(Language);

	internal static string Field_AccountStatement_DebitAmountAMD => GetField_AccountStatement_DebitAmountAMD(Language);

	internal static string Field_AccountStatement_Comment => GetField_AccountStatement_Comment(Language);

	internal static string Field_AccountStatement_DailyTurnover => GetField_AccountStatement_DailyTurnover(Language);

	internal static string Field_AccountStatement_DailyBalance => GetField_AccountStatement_DailyBalance(Language);

	internal static string Field_AccountStatement_Turnover => GetField_AccountStatement_Turnover(Language);

	internal static string Field_AccountStatement_ClosingBalance => GetField_AccountStatement_ClosingBalance(Language);

	internal static string Field_AccountStatement_AvailableBalance => GetField_AccountStatement_AvailableBalance(Language);

	internal static string Field_AccountStatement_ContractAmount => GetField_AccountStatement_ContractAmount(Language);

	internal static string Field_AccountStatement_UsedAmount => GetField_AccountStatement_UsedAmount(Language);

	internal static string Field_AccountStatement_SigningDate => GetField_AccountStatement_SigningDate(Language);

	internal static string Field_AccountStatement_RepaymentDate => GetField_AccountStatement_RepaymentDate(Language);

	internal static string Field_AccountStatement_DebtAmount => GetField_AccountStatement_DebtAmount(Language);

	internal static string Field_AccountStatement_DebtInterestAmount => GetField_AccountStatement_DebtInterestAmount(Language);

	internal static string Field_AccountStatement_Amount => GetField_AccountStatement_Amount(Language);

	internal static string Field_AccountStatement_Fine => GetField_AccountStatement_Fine(Language);

	internal static string Field_AccountStatement_TotalDebt => GetField_AccountStatement_TotalDebt(Language);

	internal static string Field_AccountStatement_AccountLimit => GetField_AccountStatement_AccountLimit(Language);

	internal static string Field_AccountStatement_APR => GetField_AccountStatement_APR(Language);

	internal static string Field_AccountStatement_Interest => GetField_AccountStatement_Interest(Language);

	internal static string Field_AccountStatement_InterestAmount => GetField_AccountStatement_InterestAmount(Language);

	internal static string Field_AccountStatement_FromBeginningOfYearInterestAmount => GetField_AccountStatement_FromBeginningOfYearInterestAmount(Language);

	internal static string Field_AccountStatement_DayCount => GetField_AccountStatement_DayCount(Language);

	internal static string Field_AccountStatement_OverviewAccountStatement => GetField_AccountStatement_OverviewAccountStatement(Language);

	internal static string Field_CardStatement_OverviewCardStatement => GetField_CardStatement_OverviewCardStatement(Language);

	internal static string Field_CardStatement_Number => GetField_CardStatement_Number(Language);

	internal static string Field_CardStatement_AccountNumber => GetField_CardStatement_AccountNumber(Language);

	internal static string Field_CardStatement_Currency => GetField_CardStatement_Currency(Language);

	internal static string Field_CardStatement_TaxCode => GetField_CardStatement_TaxCode(Language);

	internal static string Field_CardStatement_SubCards => GetField_CardStatement_SubCards(Language);

	internal static string Field_CardStatement_OpeningBalance => GetField_CardStatement_OpeningBalance(Language);

	internal static string Field_CardStatement_PostingDate => GetField_CardStatement_PostingDate(Language);

	internal static string Field_CardStatement_TransactionDate => GetField_CardStatement_TransactionDate(Language);

	internal static string Field_CardStatement_BillingAmount => GetField_CardStatement_BillingAmount(Language);

	internal static string Field_CardStatement_TransactionAmount => GetField_CardStatement_TransactionAmount(Language);

	internal static string Field_CardStatement_Description => GetField_CardStatement_Description(Language);

	internal static string Field_CardStatement_InTransactions => GetField_CardStatement_InTransactions(Language);

	internal static string Field_CardStatement_InTurnover => GetField_CardStatement_InTurnover(Language);

	internal static string Field_CardStatement_OutTransactions => GetField_CardStatement_OutTransactions(Language);

	internal static string Field_CardStatement_OutTurnover => GetField_CardStatement_OutTurnover(Language);

	internal static string Field_CardStatement_ClosingBalance => GetField_CardStatement_ClosingBalance(Language);

	internal static string Field_CardStatement_AvailableBalance => GetField_CardStatement_AvailableBalance(Language);

	internal static string Field_AgreementStatement_OverviewDepositStatement => GetField_AgreementStatement_OverviewDepositStatement(Language);

	internal static string Field_AgreementStatement_ContractNo => GetField_AgreementStatement_ContractNo(Language);

	internal static string Field_AgreementStatement_Currency => GetField_AgreementStatement_Currency(Language);

	internal static string Field_AgreementStatement_OpeningBalance => GetField_AgreementStatement_OpeningBalance(Language);

	internal static string Field_AgreementStatement_Principal => GetField_AgreementStatement_Principal(Language);

	internal static string Field_AgreementStatement_Interest => GetField_AgreementStatement_Interest(Language);

	internal static string Field_AgreementStatement_CommitmentFee => GetField_AgreementStatement_CommitmentFee(Language);

	internal static string Field_AgreementStatement_BankRateInterest => GetField_AgreementStatement_BankRateInterest(Language);

	internal static string Field_AgreementStatement_Penalty => GetField_AgreementStatement_Penalty(Language);

	internal static string Field_AgreementStatement_Penalty2 => GetField_AgreementStatement_Penalty2(Language);

	internal static string Field_AgreementStatement_Loss => GetField_AgreementStatement_Loss(Language);

	internal static string Field_AgreementStatement_Transactions => GetField_AgreementStatement_Transactions(Language);

	internal static string Field_AgreementStatement_OperationDate => GetField_AgreementStatement_OperationDate(Language);

	internal static string Field_AgreementStatement_Amount => GetField_AgreementStatement_Amount(Language);

	internal static string Field_AgreementStatement_Description => GetField_AgreementStatement_Description(Language);

	internal static string Field_AgreementStatement_Totals => GetField_AgreementStatement_Totals(Language);

	internal static string Field_AgreementStatement_AgreementTotalDescription => GetField_AgreementStatement_AgreementTotalDescription(Language);

	internal static string Field_AgreementStatement_ClosingBalance => GetField_AgreementStatement_ClosingBalance(Language);

	internal static string Field_AgreementStatement_OverviewCreditStatement => GetField_AgreementStatement_OverviewCreditStatement(Language);

	internal static string Field_MerchantTransaction_OverviewMerchantStatement => GetField_MerchantTransaction_OverviewMerchantStatement(Language);

	internal static string Field_MerchantTransaction_OperationDate => GetField_MerchantTransaction_OperationDate(Language);

	internal static string Field_MerchantTransaction_TransactionDate => GetField_MerchantTransaction_TransactionDate(Language);

	internal static string Field_MerchantTransaction_TransactionDirection => GetField_MerchantTransaction_TransactionDirection(Language);

	internal static string Field_MerchantTransaction_Currency => GetField_MerchantTransaction_Currency(Language);

	internal static string Field_MerchantTransaction_Amount => GetField_MerchantTransaction_Amount(Language);

	internal static string Field_MerchantTransaction_MerchantFee => GetField_MerchantTransaction_MerchantFee(Language);

	internal static string Field_MerchantTransaction_AmountWithoutMerchantFee => GetField_MerchantTransaction_AmountWithoutMerchantFee(Language);

	internal static string Field_MerchantTransaction_MerchantName => GetField_MerchantTransaction_MerchantName(Language);

	internal static string Field_MerchantTransaction_CardNumber => GetField_MerchantTransaction_CardNumber(Language);

	internal static string Field_MerchantTransaction_CardTypeName => GetField_MerchantTransaction_CardTypeName(Language);

	internal static string Field_MerchantTransaction_CardBelongingType => GetField_MerchantTransaction_CardBelongingType(Language);

	internal static string Field_MerchantTransaction_TransactionReferenceNumber => GetField_MerchantTransaction_TransactionReferenceNumber(Language);

	internal static string Field_MerchantTransaction_TerminalId => GetField_MerchantTransaction_TerminalId(Language);

	internal static string Field_MerchantTransaction_AuthorizationCode => GetField_MerchantTransaction_AuthorizationCode(Language);

	internal static string Field_MerchantIds => GetField_MerchantIds(Language);

	internal static string Field_TerminalIds => GetField_TerminalIds(Language);

	internal static string Field_Transactions => GetField_Transactions(Language);

	internal static string ReceiveTransactionInfoOff => GetReceiveTransactionInfoOff(Language);

	internal static string ReceiveTransactionInfoViaPushNotification => GetReceiveTransactionInfoViaPushNotification(Language);

	internal static string ReceiveTransactionInfoViaSms => GetReceiveTransactionInfoViaSms(Language);

	internal static string SmsLanguageArmenian => GetSmsLanguageArmenian(Language);

	internal static string SmsLanguageArmenianLatinized => GetSmsLanguageArmenianLatinized(Language);

	internal static string SmsLanguageEnglish => GetSmsLanguageEnglish(Language);

	internal static string SmsFormatWithBalance => GetSmsFormatWithBalance(Language);

	internal static string SmsFormatWithoutBalance => GetSmsFormatWithoutBalance(Language);

	internal static string Field_AbonentName => GetField_AbonentName(Language);

	internal static string Field_AbonentAddress => GetField_AbonentAddress(Language);

	internal static string Field_Debt => GetField_Debt(Language);

	internal static string Field_Prepayment => GetField_Prepayment(Language);

	internal static string Field_AbonentCode => GetField_AbonentCode(Language);

	internal static string Field_LocationCode => GetField_LocationCode(Language);

	internal static string Field_ComDetails => GetField_ComDetails(Language);

	internal static string Field_MemorialOrder_Base => GetField_MemorialOrder_Base(Language);

	internal static string Field_MemorialOrder_ReceiverAccount => GetField_MemorialOrder_ReceiverAccount(Language);

	internal static string Field_MemorialOrder_PayerAcc => GetField_MemorialOrder_PayerAcc(Language);

	internal static string Field_MemorialOrder_PayerAddress => GetField_MemorialOrder_PayerAddress(Language);

	internal static string Field_MemorialOrder_PayerId => GetField_MemorialOrder_PayerId(Language);

	internal static string Field_MemorialOrder_PayerName => GetField_MemorialOrder_PayerName(Language);

	internal static string Field_MemorialOrder_ReceiverAddress => GetField_MemorialOrder_ReceiverAddress(Language);

	internal static string Field_MemorialOrder_ReceiverId => GetField_MemorialOrder_ReceiverId(Language);

	internal static string Field_MemorialOrder_ReceiverName => GetField_MemorialOrder_ReceiverName(Language);

	internal static string DocumentTypeMemorialOrder => GetDocumentTypeMemorialOrder(Language);

	internal static string DocumentTypeDepositPartialReductionApplication => GetDocumentTypeDepositPartialReductionApplication(Language);

	internal static string DocumentTypeDepositTerminationApplication => GetDocumentTypeDepositTerminationApplication(Language);

	internal static string DocumentTypeSignableFileDocument => GetDocumentTypeSignableFileDocument(Language);

	internal static string UserSignatureModeFingerprint => GetUserSignatureModeFingerprint(Language);

	internal static string UserSignatureModePin => GetUserSignatureModePin(Language);

	internal static string UserSignatureModeFaceId => GetUserSignatureModeFaceId(Language);

	internal static string Field_AccountNumber => GetField_AccountNumber(Language);

	internal static string SystemTypeCreditPlatform => GetSystemTypeCreditPlatform(Language);

	internal static string SystemTypeCustomApplication => GetSystemTypeCustomApplication(Language);

	internal static string DataUpdateTypeDisabled => GetDataUpdateTypeDisabled(Language);

	internal static string DataUpdateTypeRemind => GetDataUpdateTypeRemind(Language);

	internal static string DataUpdateTypeRequired => GetDataUpdateTypeRequired(Language);

	internal static string SignableFileStateNew => GetSignableFileStateNew(Language);

	internal static string SignableFileStateSentToClientForClient => GetSignableFileStateSentToClientForClient(Language);

	internal static string SignableFileStateSentToClientForOperator => GetSignableFileStateSentToClientForOperator(Language);

	internal static string SignableFileStateSentToOperator => GetSignableFileStateSentToOperator(Language);

	internal static string SignableFileStateApproved => GetSignableFileStateApproved(Language);

	internal static string SignableFileStateRejectedByOperator => GetSignableFileStateRejectedByOperator(Language);

	internal static string SignableFileStateRejectedByClient => GetSignableFileStateRejectedByClient(Language);

	internal static string SignableFileStateExpired => GetSignableFileStateExpired(Language);

	internal static string SignableFileStateProblematic => GetSignableFileStateProblematic(Language);

	internal static string Field_SignableFile_InitialFileName => GetField_SignableFile_InitialFileName(Language);

	internal static string Field_FileSignature_Identity => GetField_FileSignature_Identity(Language);

	internal static string Field_FileSignature_Date => GetField_FileSignature_Date(Language);

	internal static string Field_FileSignature_SignatureIsValid => GetField_FileSignature_SignatureIsValid(Language);

	internal static string Field_FileSignature_TimeStampIsValid => GetField_FileSignature_TimeStampIsValid(Language);

	internal static string Field_FileSignature_CertificateIsValid => GetField_FileSignature_CertificateIsValid(Language);

	internal static string Field_SignableFile_InitialUploadDate => GetField_SignableFile_InitialUploadDate(Language);

	internal static string Field_SignableFile_SignedUploadDate => GetField_SignableFile_SignedUploadDate(Language);

	internal static string Field_SignableFile_InitialContent => GetField_SignableFile_InitialContent(Language);

	internal static string Field_SignableFile_SignedContent => GetField_SignableFile_SignedContent(Language);

	internal static string Field_AmeriaSignableFileType_Description => GetField_AmeriaSignableFileType_Description(Language);

	internal static string Field_AmeriaSignableFileType_QueryParams => GetField_AmeriaSignableFileType_QueryParams(Language);

	internal static string Field_SignableFile_TypeId => GetField_SignableFile_TypeId(Language);

	internal static string Field_AmeriaSignableFileType_ClassId => GetField_AmeriaSignableFileType_ClassId(Language);

	internal static string BankQuestionAccount => GetBankQuestionAccount(Language);

	internal static string BankQuestionCreditAgreement => GetBankQuestionCreditAgreement(Language);

	internal static string BankQuestionDepositAgreement => GetBankQuestionDepositAgreement(Language);

	internal static string BankQuestionCard => GetBankQuestionCard(Language);

	internal static string BankQuestionSocialCard => GetBankQuestionSocialCard(Language);

	internal static string BankQuestionTaxCode => GetBankQuestionTaxCode(Language);

	internal static string UsernamePolicy => GetUsernamePolicy(Language);

	internal static string UsernamePolicySimple => GetUsernamePolicySimple(Language);

	internal static string Field_ClientCode => GetField_ClientCode(Language);

	internal static string Field_ClientName => GetField_ClientName(Language);

	internal static string Field_SignableFile_OperatorName => GetField_SignableFile_OperatorName(Language);

	internal static string InterestPayPeriodHalfYearly => GetInterestPayPeriodHalfYearly(Language);

	internal static string InterestPayPeriodMonthly => GetInterestPayPeriodMonthly(Language);

	internal static string InterestPayPeriodQuarterly => GetInterestPayPeriodQuarterly(Language);

	internal static string InterestPayPeriodStartOfTerm => GetInterestPayPeriodStartOfTerm(Language);

	internal static string InterestPayPeriodEndOfTerm => GetInterestPayPeriodEndOfTerm(Language);

	internal static string InterestPayPeriodYearly => GetInterestPayPeriodYearly(Language);

	internal static string Field_MultipleCommunal_Rows => GetField_MultipleCommunal_Rows(Language);

	internal static string Field_CreditRepaymentWithAttachedCard_RepaymentType => GetField_CreditRepaymentWithAttachedCard_RepaymentType(Language);

	internal static string Field_CreditRepaymentWithAttachedCard_Receiver => GetField_CreditRepaymentWithAttachedCard_Receiver(Language);

	internal static string Field_CreditRepaymentWithAttachedCard_IsFullRepayment => GetField_CreditRepaymentWithAttachedCard_IsFullRepayment(Language);

	internal static string Field_CreditRepaymentWithAttachedCard_OverpaymentPrincipal => GetField_CreditRepaymentWithAttachedCard_OverpaymentPrincipal(Language);

	internal static string Field_CreditRepaymentWithAttachedCard_CardNumber => GetField_CreditRepaymentWithAttachedCard_CardNumber(Language);

	internal static string Field_ForeignPaymentRus_PayerAddress => GetField_ForeignPaymentRus_PayerAddress(Language);

	internal static string Field_ForeignPaymentRus_PayerCity => GetField_ForeignPaymentRus_PayerCity(Language);

	internal static string Field_ForeignPaymentRus_PayerCountry => GetField_ForeignPaymentRus_PayerCountry(Language);

	internal static string Field_ForeignPaymentRus_ReceiverAddress => GetField_ForeignPaymentRus_ReceiverAddress(Language);

	internal static string Field_ForeignPaymentRus_ReceiverCountry => GetField_ForeignPaymentRus_ReceiverCountry(Language);

	internal static string Field_ForeignPaymentRus_ReceiverINN => GetField_ForeignPaymentRus_ReceiverINN(Language);

	internal static string Field_ForeignPaymentRus_ReceiverKPP => GetField_ForeignPaymentRus_ReceiverKPP(Language);

	internal static string Field_ForeignPaymentRus_ReceiverBankBIC => GetField_ForeignPaymentRus_ReceiverBankBIC(Language);

	internal static string Field_ForeignPaymentRus_IntermediaryBankBIC => GetField_ForeignPaymentRus_IntermediaryBankBIC(Language);

	internal static string Field_ForeignPaymentRus_AdditionalInfo => GetField_ForeignPaymentRus_AdditionalInfo(Language);

	internal static string Field_ForeignPaymentRus_AdditionalInfo2 => GetField_ForeignPaymentRus_AdditionalInfo2(Language);

	internal static string Field_ForeignPaymentRus_AdditionalInfo3 => GetField_ForeignPaymentRus_AdditionalInfo3(Language);

	internal static string Field_ForeignPaymentRus_AdditionalInfo4 => GetField_ForeignPaymentRus_AdditionalInfo4(Language);

	internal static string Field_ForeignPaymentRus_AdditionalInfo5 => GetField_ForeignPaymentRus_AdditionalInfo5(Language);

	internal static string Field_ForeignPaymentRus_AdditionalInfo6 => GetField_ForeignPaymentRus_AdditionalInfo6(Language);

	internal static string DebitAccountPositiveBalance => GetDebitAccountPositiveBalance(Language);

	internal static string DebitAccountNegativeBalance => GetDebitAccountNegativeBalance(Language);

	internal static string CreditAccountPositiveBalance => GetCreditAccountPositiveBalance(Language);

	internal static string CreditAccountNegativeBalance => GetCreditAccountNegativeBalance(Language);

	internal static string MinimumOfPositiveBalances => GetMinimumOfPositiveBalances(Language);

	internal static string CreditTurnoverOfDebitAccount => GetCreditTurnoverOfDebitAccount(Language);

	internal static string CreditTurnoverOfDebitAccountMinusDebitTurnover => GetCreditTurnoverOfDebitAccountMinusDebitTurnover(Language);

	internal static string DebitTurnoverOfDebitAccount => GetDebitTurnoverOfDebitAccount(Language);

	internal static string DebitTurnoverOfDebitAccountMinusCreditTurnover => GetDebitTurnoverOfDebitAccountMinusCreditTurnover(Language);

	internal static string FixedAmount => GetFixedAmount(Language);

	internal static string DebtAmount => GetDebtAmount(Language);

	internal static string Field_UtilityCode => GetField_UtilityCode(Language);

	internal static string Field_Comments => GetField_Comments(Language);

	internal static string Field_FileName => GetField_FileName(Language);

	internal static string DocumentOperationNotSet => GetDocumentOperationNotSet(Language);

	internal static string DocumentOperationCreated => GetDocumentOperationCreated(Language);

	internal static string DocumentOperationEdited => GetDocumentOperationEdited(Language);

	internal static string DocumentOperationSigned => GetDocumentOperationSigned(Language);

	internal static string DocumentOperationSignedByFirst => GetDocumentOperationSignedByFirst(Language);

	internal static string DocumentOperationSignedBySecond => GetDocumentOperationSignedBySecond(Language);

	internal static string DocumentOperationSignedByThird => GetDocumentOperationSignedByThird(Language);

	internal static string DocumentOperationSent => GetDocumentOperationSent(Language);

	internal static string DocumentOperationSentToEdit => GetDocumentOperationSentToEdit(Language);

	internal static string DocumentOperationConfirmed => GetDocumentOperationConfirmed(Language);

	internal static string DocumentOperationRefused => GetDocumentOperationRefused(Language);

	internal static string DocumentOperationCanceled => GetDocumentOperationCanceled(Language);

	internal static string DocumentOperationConfirmedCommissionChange => GetDocumentOperationConfirmedCommissionChange(Language);

	internal static string DocumentOperationSignatureQueued => GetDocumentOperationSignatureQueued(Language);

	internal static string DocumentOperationSignatureFailed => GetDocumentOperationSignatureFailed(Language);

	internal static string DocumentOperationSignatureIgnored => GetDocumentOperationSignatureIgnored(Language);

	internal static string Field_CurrencyCredit => GetField_CurrencyCredit(Language);

	internal static string Field_AbonentID => GetField_AbonentID(Language);

	internal static string Field_AbonentType => GetField_AbonentType(Language);

	internal static string Field_AccountType => GetField_AccountType(Language);

	internal static string Field_Goal => GetField_Goal(Language);

	internal static string Field_CardDesign => GetField_CardDesign(Language);

	internal static string Field_CardTypeId => GetField_CardTypeId(Language);

	internal static string Field_CardDesignId => GetField_CardDesignId(Language);

	internal static string TaxPropertyTypeGarbageCollection => GetTaxPropertyTypeGarbageCollection(Language);

	internal static string TaxPropertyTypeLand => GetTaxPropertyTypeLand(Language);

	internal static string TaxPropertyTypeWaterTransport => GetTaxPropertyTypeWaterTransport(Language);

	internal static string Field_RoadPolicePayment_ViolationTaskId => GetField_RoadPolicePayment_ViolationTaskId(Language);

	internal static string PeriodicityDays => GetPeriodicityDays(Language);

	internal static string PeriodicityMonths => GetPeriodicityMonths(Language);

	internal static string PeriodicityMonthsDays => GetPeriodicityMonthsDays(Language);

	internal static string DocumentSignQueueRowStatusNew => GetDocumentSignQueueRowStatusNew(Language);

	internal static string DocumentSignQueueRowStatusStarted => GetDocumentSignQueueRowStatusStarted(Language);

	internal static string DocumentSignQueueRowStatusSigned => GetDocumentSignQueueRowStatusSigned(Language);

	internal static string DocumentSignQueueRowStatusSignFailed => GetDocumentSignQueueRowStatusSignFailed(Language);

	internal static string DocumentSignQueueRowStatusIgnored => GetDocumentSignQueueRowStatusIgnored(Language);

	internal static string DocumentSignQueueStatusCreated => GetDocumentSignQueueStatusCreated(Language);

	internal static string DocumentSignQueueStatusStarted => GetDocumentSignQueueStatusStarted(Language);

	internal static string DocumentSignQueueStatusSucceeded => GetDocumentSignQueueStatusSucceeded(Language);

	internal static string DocumentSignQueueStatusFailed => GetDocumentSignQueueStatusFailed(Language);

	internal static string Field_OpenDepositApplication_IsDepositTermExtendable => GetField_OpenDepositApplication_IsDepositTermExtendable(Language);

	internal static string Field_OpenDepositApplication_IsDepositCapitalizable => GetField_OpenDepositApplication_IsDepositCapitalizable(Language);

	internal static string Field_OpenDepositApplication_Date => GetField_OpenDepositApplication_Date(Language);

	internal static string Field_OpenDepositApplication_Duration => GetField_OpenDepositApplication_Duration(Language);

	internal static string Field_OpenDepositApplication_DurationDays => GetField_OpenDepositApplication_DurationDays(Language);

	internal static string Field_OpenDepositApplication_DurationMonths => GetField_OpenDepositApplication_DurationMonths(Language);

	internal static string Field_OpenDepositApplication_MaturityDate => GetField_OpenDepositApplication_MaturityDate(Language);

	internal static string Field_DepositTemplateCode => GetField_DepositTemplateCode(Language);

	internal static string Field_DepositTemplateName => GetField_DepositTemplateName(Language);

	internal static string Field_FixedRate => GetField_FixedRate(Language);

	internal static string Field_PercentAccount => GetField_PercentAccount(Language);

	internal static string Field_TaxAmount => GetField_TaxAmount(Language);

	internal static string Field_Info => GetField_Info(Language);

	internal static string Field_LastUpdateDate => GetField_LastUpdateDate(Language);

	internal static string Field_Year => GetField_Year(Language);

	internal static string Field_TaxDebtId => GetField_TaxDebtId(Language);

	internal static string Field_IsJoint => GetField_IsJoint(Language);

	internal static string Field_PropertyOwner => GetField_PropertyOwner(Language);

	internal static string Field_BankAccount => GetField_BankAccount(Language);

	internal static string Field_PropertyTaxPayment_Rows => GetField_PropertyTaxPayment_Rows(Language);

	internal static string Field_RealEstateTaxPayment_Rows => GetField_RealEstateTaxPayment_Rows(Language);

	internal static string Field_VehicleTaxPayment_Rows => GetField_VehicleTaxPayment_Rows(Language);

	internal static string Field_PropertyTaxDebtPeriod => GetField_PropertyTaxDebtPeriod(Language);

	internal static string Field_PropertyTaxOwnerLegalStatus => GetField_PropertyTaxOwnerLegalStatus(Language);

	internal static string Field_PropertyType => GetField_PropertyType(Language);

	internal static string Field_PropertySearchMethod => GetField_PropertySearchMethod(Language);

	internal static string Field_VehicleLicensePlateNumber => GetField_VehicleLicensePlateNumber(Language);

	internal static string Field_VehicleRegistrationCertificateNumber => GetField_VehicleRegistrationCertificateNumber(Language);

	internal static string Field_PropertyTaxCode => GetField_PropertyTaxCode(Language);

	internal static string Field_Share => GetField_Share(Language);

	internal static string Field_Overpay => GetField_Overpay(Language);

	internal static string Field_TaxPaymentType => GetField_TaxPaymentType(Language);

	internal static string TaxPaymentTypeDebt => GetTaxPaymentTypeDebt(Language);

	internal static string TaxPaymentTypeFine => GetTaxPaymentTypeFine(Language);

	internal static string Field_IsDepositReplenishable => GetField_IsDepositReplenishable(Language);

	internal static string Field_CompulsoryEnforcementPayment_IdentificationDocumentNumber => GetField_CompulsoryEnforcementPayment_IdentificationDocumentNumber(Language);

	internal static string Field_CompulsoryEnforcementPayment_DebtorName => GetField_CompulsoryEnforcementPayment_DebtorName(Language);

	internal static string Field_CompulsoryEnforcementPayment_PlaintiffName => GetField_CompulsoryEnforcementPayment_PlaintiffName(Language);

	internal static string Field_CompulsoryEnforcementPayment_RequestedAmount => GetField_CompulsoryEnforcementPayment_RequestedAmount(Language);

	internal static string Field_CompulsoryEnforcementPayment_ChargeAmount => GetField_CompulsoryEnforcementPayment_ChargeAmount(Language);

	internal static string Field_ReceiverContactData => GetField_ReceiverContactData(Language);

	internal static string Field_ContactTransfer_Receiver => GetField_ContactTransfer_Receiver(Language);

	internal static string Field_ContactTransfer_ReceiverBank => GetField_ContactTransfer_ReceiverBank(Language);

	internal static string Field_TaxPayment_Rows => GetField_TaxPayment_Rows(Language);

	internal static string Field_GarbageCollectionPayment_Rows => GetField_GarbageCollectionPayment_Rows(Language);

	internal static string Field_TaxPropertyType => GetField_TaxPropertyType(Language);

	internal static string Field_TaxPropertyId => GetField_TaxPropertyId(Language);

	internal static string Field_TaxPayerName => GetField_TaxPayerName(Language);

	internal static string Field_TaxPayerAddress => GetField_TaxPayerAddress(Language);

	internal static string Field_TaxPayment_Amount => GetField_TaxPayment_Amount(Language);

	internal static string Field_GarbageCollectionPayment_Amount => GetField_GarbageCollectionPayment_Amount(Language);

	internal static string Field_CashOutByPhone_ReceiverMobileNumber => GetField_CashOutByPhone_ReceiverMobileNumber(Language);

	internal static string Field_CashOutByPhone_PayerAcc => GetField_CashOutByPhone_PayerAcc(Language);

	internal static string Field_CashOutByPhone_Amount => GetField_CashOutByPhone_Amount(Language);

	internal static string Field_CashOutByPhone_WithdrawalCode => GetField_CashOutByPhone_WithdrawalCode(Language);

	internal static string Field_CashOutByPhone_ExpirationDate => GetField_CashOutByPhone_ExpirationDate(Language);

	internal static string Field_PurchaseBondApplication_PayerName => GetField_PurchaseBondApplication_PayerName(Language);

	internal static string Field_PurchaseBondApplication_PayerBank => GetField_PurchaseBondApplication_PayerBank(Language);

	internal static string Field_PurchaseBondApplication_IssuanceNumber => GetField_PurchaseBondApplication_IssuanceNumber(Language);

	internal static string Field_PurchaseBondApplication_BondsCount => GetField_PurchaseBondApplication_BondsCount(Language);

	internal static string Field_MerchantTransaction_TokenPan => GetField_MerchantTransaction_TokenPan(Language);

	internal static string Field_ClosingBalance => GetField_ClosingBalance(Language);

	internal static string Field_ClosingBalanceDate => GetField_ClosingBalanceDate(Language);

	internal static string Field_OpeningBalance => GetField_OpeningBalance(Language);

	internal static string Field_OpeningBalanceDate => GetField_OpeningBalanceDate(Language);

	internal static string Field_AccountStatement_CreditTurnover => GetField_AccountStatement_CreditTurnover(Language);

	internal static string Field_AccountStatement_CreditTurnoverAMD => GetField_AccountStatement_CreditTurnoverAMD(Language);

	internal static string Field_AccountStatement_DebitTurnover => GetField_AccountStatement_DebitTurnover(Language);

	internal static string Field_AccountStatement_DebitTurnoverAMD => GetField_AccountStatement_DebitTurnoverAMD(Language);

	internal static string Field_AccountStatement_OpeningBalanceAMD => GetField_AccountStatement_OpeningBalanceAMD(Language);

	internal static string Field_CashWithdrawalApplication_WithdrawalDate => GetField_CashWithdrawalApplication_WithdrawalDate(Language);

	internal static string Field_CashWithdrawalApplication_BranchName => GetField_CashWithdrawalApplication_BranchName(Language);

	internal static string Field_CashWithdrawalApplication_IsReceiverAnotherClient => GetField_CashWithdrawalApplication_IsReceiverAnotherClient(Language);

	internal static string Field_CashWithdrawalApplication_Receiver => GetField_CashWithdrawalApplication_Receiver(Language);

	internal static string Field_CashWithdrawalApplication_ReceiverLastName => GetField_CashWithdrawalApplication_ReceiverLastName(Language);

	internal static string Field_CashWithdrawalApplication_ReceiverPassportNum => GetField_CashWithdrawalApplication_ReceiverPassportNum(Language);

	internal static string Field_CashWithdrawalApplication_ReceiverPassportAuthority => GetField_CashWithdrawalApplication_ReceiverPassportAuthority(Language);

	internal static string Field_CashWithdrawalApplication_ReceiverIdentificationDocumentType => GetField_CashWithdrawalApplication_ReceiverIdentificationDocumentType(Language);

	internal static string Field_CashWithdrawalApplication_ReceiverPassportDateOfIssue => GetField_CashWithdrawalApplication_ReceiverPassportDateOfIssue(Language);

	internal static string Field_CashWithdrawalApplication_ReceiverPassportExpirationDate => GetField_CashWithdrawalApplication_ReceiverPassportExpirationDate(Language);

	internal static string Field_ForeignPayment_TransferAdditionalInfo => GetField_ForeignPayment_TransferAdditionalInfo(Language);

	internal static string Field_ForeignPayment_ReceivingAmount => GetField_ForeignPayment_ReceivingAmount(Language);

	internal static string Field_ForeignPayment_ReceivingCurrency => GetField_ForeignPayment_ReceivingCurrency(Language);

	internal static string Field_DocumentName => GetField_DocumentName(Language);

	internal static string SecurityProviderRefreshTypeWithOtp => GetSecurityProviderRefreshTypeWithOtp(Language);

	internal static string SecurityProviderRefreshTypeWithBiometric => GetSecurityProviderRefreshTypeWithBiometric(Language);

	internal static string Field_ThirdPartyPassportOrTaxCode => GetField_ThirdPartyPassportOrTaxCode(Language);

	internal static string Field_VehicleUseTypeId => GetField_VehicleUseTypeId(Language);

	internal static string Field_InsuranceStartDate => GetField_InsuranceStartDate(Language);

	internal static string Field_InsuranceDuration => GetField_InsuranceDuration(Language);

	internal static string Field_AswaContractMobile => GetField_AswaContractMobile(Language);

	internal static string Field_AswaContractEmail => GetField_AswaContractEmail(Language);

	internal static string Field_AswaContractDefaultAccount => GetField_AswaContractDefaultAccount(Language);

	internal static string Field_InsuranceCompanyId => GetField_InsuranceCompanyId(Language);

	internal static string Field_AmeriaSignableFileType_OperatorRoles => GetField_AmeriaSignableFileType_OperatorRoles(Language);

	internal static string WorkerInstanceStatusRunning => GetWorkerInstanceStatusRunning(Language);

	internal static string WorkerInstanceStatusStoppedUnexpectedly => GetWorkerInstanceStatusStoppedUnexpectedly(Language);

	internal static string WorkerInstanceStatusStoppedGracefully => GetWorkerInstanceStatusStoppedGracefully(Language);

	internal static string WorkerInstanceStatusUnknown => GetWorkerInstanceStatusUnknown(Language);

	internal static string DocumentTypeCreditLineDisbursementApplication => GetDocumentTypeCreditLineDisbursementApplication(Language);

	internal static string Field_AdvancedLocalPayment_CreditPaymentType => GetField_AdvancedLocalPayment_CreditPaymentType(Language);

	internal static string Field_AdvancedLocalPayment_AdditionalInfo => GetField_AdvancedLocalPayment_AdditionalInfo(Language);

	internal static string Field_AdvancedLocalPayment_AdditionalInfo2 => GetField_AdvancedLocalPayment_AdditionalInfo2(Language);

	internal static string Field_AdvancedLocalPayment_AdditionalInfo3 => GetField_AdvancedLocalPayment_AdditionalInfo3(Language);

	internal static string Field_AdvancedLocalPayment_AdditionalInfo4 => GetField_AdvancedLocalPayment_AdditionalInfo4(Language);

	internal static string Field_AdvancedLocalPayment_AdditionalInfo5 => GetField_AdvancedLocalPayment_AdditionalInfo5(Language);

	internal static string Field_AdvancedLocalPayment_AdditionalInfo6 => GetField_AdvancedLocalPayment_AdditionalInfo6(Language);

	internal static string Field_AdvancedLocalPayment_CreditContractOwner => GetField_AdvancedLocalPayment_CreditContractOwner(Language);

	internal static string Field_AdvancedLocalPayment_CreditCode => GetField_AdvancedLocalPayment_CreditCode(Language);

	internal static string Field_AdvancedLocalPayment_PaymentMethod => GetField_AdvancedLocalPayment_PaymentMethod(Language);

	internal static string Field_AdvancedLocalPayment_ReceiverLegalState => GetField_AdvancedLocalPayment_ReceiverLegalState(Language);

	internal static string Field_AdvancedLocalPayment_ReceiverTaxCode => GetField_AdvancedLocalPayment_ReceiverTaxCode(Language);

	internal static string Field_AdvancedLocalPayment_ReceiverSocialCard => GetField_AdvancedLocalPayment_ReceiverSocialCard(Language);

	internal static string Field_AdvancedLocalPayment_ReceiverPassportType => GetField_AdvancedLocalPayment_ReceiverPassportType(Language);

	internal static string Field_AdvancedLocalPayment_ReceiverPassportNo => GetField_AdvancedLocalPayment_ReceiverPassportNo(Language);

	internal static string Field_AdvancedLocalPayment_ReceiverPassportNumber => GetField_AdvancedLocalPayment_ReceiverPassportNumber(Language);

	internal static string Field_AdvancedLocalPayment_ReceiverAuthorizedPerson => GetField_AdvancedLocalPayment_ReceiverAuthorizedPerson(Language);

	internal static string Field_AdvancedLocalPayment_PayerAuthorizedPerson => GetField_AdvancedLocalPayment_PayerAuthorizedPerson(Language);

	internal static string Field_AdvancedLocalPayment_ReceiverBankName => GetField_AdvancedLocalPayment_ReceiverBankName(Language);

	internal static string Field_AdvancedLocalPayment_PayerAdditionalInfo => GetField_AdvancedLocalPayment_PayerAdditionalInfo(Language);

	internal static string Field_AdvancedLocalPayment_ReceiverAdditionalInfo => GetField_AdvancedLocalPayment_ReceiverAdditionalInfo(Language);

	internal static string DocumentTypeQrPayment => GetDocumentTypeQrPayment(Language);

	internal static string RegistrationTypeNewClient => GetRegistrationTypeNewClient(Language);

	internal static string RegistrationTypeExistingClient => GetRegistrationTypeExistingClient(Language);

	internal static string CreditOverpaymentTypeUpcomingAmounts => GetCreditOverpaymentTypeUpcomingAmounts(Language);

	internal static string CreditOverpaymentTypeLastAmounts => GetCreditOverpaymentTypeLastAmounts(Language);

	internal static string CreditOverpaymentTypeUpcomingAmountThenLast => GetCreditOverpaymentTypeUpcomingAmountThenLast(Language);

	internal static string CreditOverpaymentTypeWithScheduleRedistribution => GetCreditOverpaymentTypeWithScheduleRedistribution(Language);

	internal static string CreditOverpaymentTypeUpcomingAmountsDescription => GetCreditOverpaymentTypeUpcomingAmountsDescription(Language);

	internal static string CreditOverpaymentTypeLastAmountsDescription => GetCreditOverpaymentTypeLastAmountsDescription(Language);

	internal static string CreditOverpaymentTypeUpcomingAmountThenLastDescription => GetCreditOverpaymentTypeUpcomingAmountThenLastDescription(Language);

	internal static string CreditOverpaymentTypeWithScheduleRedistributionDescription => GetCreditOverpaymentTypeWithScheduleRedistributionDescription(Language);

	internal static string LinkUrl => GetLinkUrl(Language);

	internal static string LinkDocumentType => GetLinkDocumentType(Language);

	internal static string LinkModule => GetLinkModule(Language);

	internal static string Field_QrPayment_MerchantName => GetField_QrPayment_MerchantName(Language);

	internal static string Field_QrPayment_ExternalCode => GetField_QrPayment_ExternalCode(Language);

	internal static string Field_OverpaymentType => GetField_OverpaymentType(Language);

	internal static string Field_FriendCreditRepayment_PayerBank => GetField_FriendCreditRepayment_PayerBank(Language);

	internal static string Field_FriendCreditRepayment_FriendCreditSearchType => GetField_FriendCreditRepayment_FriendCreditSearchType(Language);

	internal static string Field_FriendCreditRepayment_ContractNumber => GetField_FriendCreditRepayment_ContractNumber(Language);

	internal static string Field_FriendCreditRepayment_ContractCreditCode => GetField_FriendCreditRepayment_ContractCreditCode(Language);

	internal static string Field_FriendCreditRepayment_PassportNumber => GetField_FriendCreditRepayment_PassportNumber(Language);

	internal static string Field_FriendCreditRepayment_ContractDetails => GetField_FriendCreditRepayment_ContractDetails(Language);

	internal static string Field_FriendCreditRepayment_BankPartner => GetField_FriendCreditRepayment_BankPartner(Language);

	internal static string Field_ArCaPayTransfer_ReceiverContactData => GetField_ArCaPayTransfer_ReceiverContactData(Language);

	internal static string Field_ArCaPayTransfer_Receiver => GetField_ArCaPayTransfer_Receiver(Language);

	internal static string Field_ArCaPayTransfer_ReceiverBankArCaCode => GetField_ArCaPayTransfer_ReceiverBankArCaCode(Language);

	internal static string Field_AgreementStatement_OverviewBondStatement => GetField_AgreementStatement_OverviewBondStatement(Language);

	internal static string ServiceFeeRateDivisorCategoryDescription_Daily => GetServiceFeeRateDivisorCategoryDescription_Daily(Language);

	internal static string ServiceFeeRateDivisorCategoryDescription_Weekly => GetServiceFeeRateDivisorCategoryDescription_Weekly(Language);

	internal static string ServiceFeeRateDivisorCategoryDescription_Monthly => GetServiceFeeRateDivisorCategoryDescription_Monthly(Language);

	internal static string ServiceFeeRateDivisorCategoryDescription_Yearly => GetServiceFeeRateDivisorCategoryDescription_Yearly(Language);

	internal static string Field_CompulsoryEnforcementPayment_Receiver => GetField_CompulsoryEnforcementPayment_Receiver(Language);

	internal static string Field_Message_Subject => GetField_Message_Subject(Language);

	internal static string Field_Message_Body => GetField_Message_Body(Language);

	internal static string MerchantCategoryCodeEatingPlacesAndRestaurants => GetMerchantCategoryCodeEatingPlacesAndRestaurants(Language);

	internal static string MerchantCategoryCodeGroceryStoresAndSupermarkets => GetMerchantCategoryCodeGroceryStoresAndSupermarkets(Language);

	internal static string MerchantCategoryCodeLodgingHotelsMotelsResorts => GetMerchantCategoryCodeLodgingHotelsMotelsResorts(Language);

	internal static string MerchantCategoryCodeFastFoodRestaurants => GetMerchantCategoryCodeFastFoodRestaurants(Language);

	internal static string MerchantCategoryCodeMiscellaneousFoodStores => GetMerchantCategoryCodeMiscellaneousFoodStores(Language);

	internal static string MerchantCategoryCodeFamilyClothingStores => GetMerchantCategoryCodeFamilyClothingStores(Language);

	internal static string MerchantCategoryCodeTravelAgenciesAndTourOperators => GetMerchantCategoryCodeTravelAgenciesAndTourOperators(Language);

	internal static string MerchantCategoryCodeLumberAndBuildingMaterialsStores => GetMerchantCategoryCodeLumberAndBuildingMaterialsStores(Language);

	internal static string MerchantCategoryCodeMiscellaneousGeneralMerchandise => GetMerchantCategoryCodeMiscellaneousGeneralMerchandise(Language);

	internal static string MerchantCategoryCodeMensAndWomensClothingStores => GetMerchantCategoryCodeMensAndWomensClothingStores(Language);

	internal static string MerchantCategoryCodeJewelryStores => GetMerchantCategoryCodeJewelryStores(Language);

	internal static string MerchantCategoryCodeBusinessServicesNotElsewhereClassified => GetMerchantCategoryCodeBusinessServicesNotElsewhereClassified(Language);

	internal static string MerchantCategoryCodeAirlines => GetMerchantCategoryCodeAirlines(Language);

	internal static string MerchantCategoryCodeCarRental => GetMerchantCategoryCodeCarRental(Language);

	internal static string MerchantCategoryCodeTaxisAndLimousines => GetMerchantCategoryCodeTaxisAndLimousines(Language);

	internal static string MerchantCategoryCodeAutomatedFuelDispensers => GetMerchantCategoryCodeAutomatedFuelDispensers(Language);

	internal static string MerchantCategoryCodeServiceStations => GetMerchantCategoryCodeServiceStations(Language);

	internal static string MerchantCategoryCodeDoctors => GetMerchantCategoryCodeDoctors(Language);

	internal static string MerchantCategoryCodeDentists => GetMerchantCategoryCodeDentists(Language);

	internal static string MerchantCategoryCodeHospitals => GetMerchantCategoryCodeHospitals(Language);

	internal static string MerchantCategoryCodeFoodStore => GetMerchantCategoryCodeFoodStore(Language);

	internal static string MerchantCategoryCodeClothingStore => GetMerchantCategoryCodeClothingStore(Language);

	internal static string MerchantCategoryCodeTravelAgency => GetMerchantCategoryCodeTravelAgency(Language);

	internal static string MerchantCategoryCodeBuildingMaterialsStore => GetMerchantCategoryCodeBuildingMaterialsStore(Language);

	internal static string MerchantCategoryCodeGeneralStore => GetMerchantCategoryCodeGeneralStore(Language);

	internal static string MerchantCategoryCodeApparelAndAccessoriesStore => GetMerchantCategoryCodeApparelAndAccessoriesStore(Language);

	internal static string MerchantCategoryCodeJewelryStore => GetMerchantCategoryCodeJewelryStore(Language);

	internal static string MerchantCategoryCodeService => GetMerchantCategoryCodeService(Language);

	internal static string MerchantCategoryCodeRestaurant => GetMerchantCategoryCodeRestaurant(Language);

	internal static string MerchantCategoryCodeSupermarket => GetMerchantCategoryCodeSupermarket(Language);

	internal static string MerchantCategoryCodeHotel => GetMerchantCategoryCodeHotel(Language);

	internal static string MerchantCategoryCodeCafeFastFood => GetMerchantCategoryCodeCafeFastFood(Language);

	internal static string MerchantCategoryCodeOther => GetMerchantCategoryCodeOther(Language);

	internal static string GetField_Date(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата";
			}
			return "Date";
		}
		return "Ամսաթիվ";
	}

	internal static string GetField_Id(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Идентификатор";
			}
			return "ID";
		}
		return "Նույնացուցիչ";
	}

	internal static string GetField_PayerAcc(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет";
			}
			return "Account";
		}
		return "Հաշիվ";
	}

	internal static string GetField_PayerName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название плательщика";
			}
			return "Payer Name";
		}
		return "Վճարողի անվանում";
	}

	internal static string GetField_ContractNo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Договор №";
			}
			return "Contract No.";
		}
		return "Պայմանագրի N";
	}

	internal static string GetField_Receiver(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название договора";
			}
			return "Beneficiary Name";
		}
		return "Շահառուի անվանում";
	}

	internal static string GetField_DepositAttraction_Receiver(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название контракта";
			}
			return "Contract Name";
		}
		return "Պայմանագրի անվանում";
	}

	internal static string GetField_ReceiverBank(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Банк получателя";
			}
			return "Beneficiary Bank";
		}
		return "Շահառուի բանկ";
	}

	internal static string GetField_PayerBank(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Банк плательщика";
			}
			return "Remitter Bank";
		}
		return "Վճարողի բանկ";
	}

	internal static string GetField_AmountInWords(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма прописью";
			}
			return "Amount in Words";
		}
		return "Գումարը բառերով";
	}

	internal static string GetField_Details(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Детали";
			}
			return "Details";
		}
		return "Նպատակ";
	}

	internal static string GetField_Currency(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Валюта";
			}
			return "Currency";
		}
		return "Արժույթ";
	}

	internal static string GetField_Amount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма";
			}
			return "Amount";
		}
		return "Գումար";
	}

	internal static string GetField_ReceiverPassportNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Паспортные данные";
			}
			return "Beneficiary Passport No.";
		}
		return "Անձնագրի տվյալներ";
	}

	internal static string GetZero(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "ноль";
			}
			return "zero";
		}
		return "զրո";
	}

	internal static string GetOne(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "одна";
			}
			return "one";
		}
		return "մեկ";
	}

	internal static string GetTwo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "две";
			}
			return "two";
		}
		return "երկու";
	}

	internal static string GetThree(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "три";
			}
			return "three";
		}
		return "երեք";
	}

	internal static string GetFour(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "четыре";
			}
			return "four";
		}
		return "չորս";
	}

	internal static string GetFive(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "пять";
			}
			return "five";
		}
		return "հինգ";
	}

	internal static string GetSix(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "шесть";
			}
			return "six";
		}
		return "վեց";
	}

	internal static string GetSeven(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "семь";
			}
			return "seven";
		}
		return "յոթ";
	}

	internal static string GetEight(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "восемь";
			}
			return "eight";
		}
		return "ութ";
	}

	internal static string GetNine(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "девять";
			}
			return "nine";
		}
		return "ինը";
	}

	internal static string GetTen(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "десять";
			}
			return "ten";
		}
		return "տասը";
	}

	internal static string GetEleven(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "одиннадцать";
			}
			return "eleven";
		}
		return "տասնմեկ";
	}

	internal static string GetTwelve(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "двенадцать";
			}
			return "twelve";
		}
		return "տասներկու";
	}

	internal static string GetThirteen(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "тринадцать";
			}
			return "thirteen";
		}
		return "տասներեք";
	}

	internal static string GetFourteen(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "четырнадцать";
			}
			return "fourteen";
		}
		return "տասնչորս";
	}

	internal static string GetFifteen(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "пятнадцать";
			}
			return "fifteen";
		}
		return "տասնհինգ";
	}

	internal static string GetSixteen(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "шестнадцать";
			}
			return "sixteen";
		}
		return "տասնվեց";
	}

	internal static string GetSeventeen(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "семнадцать";
			}
			return "seventeen";
		}
		return "տասնյոթ";
	}

	internal static string GetEighteen(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "восемнадцать";
			}
			return "eighteen";
		}
		return "տասնութ";
	}

	internal static string GetNineteen(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "девятнадцать";
			}
			return "nineteen";
		}
		return "տասնինը";
	}

	internal static string GetTwenty(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "двадцать";
			}
			return "twenty";
		}
		return "քսան";
	}

	internal static string GetThirty(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "тридцать";
			}
			return "thirty";
		}
		return "երեսուն";
	}

	internal static string GetForty(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "сорок";
			}
			return "forty";
		}
		return "քառասուն";
	}

	internal static string GetFifty(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "пятьдесят";
			}
			return "fifty";
		}
		return "հիսուն";
	}

	internal static string GetSixty(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "шестьдесят";
			}
			return "sixty";
		}
		return "վաթսուն";
	}

	internal static string GetSeventy(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "семьдесят";
			}
			return "seventy";
		}
		return "յոթանասուն";
	}

	internal static string GetEighty(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "восемьдесят";
			}
			return "eighty";
		}
		return "ութսուն";
	}

	internal static string GetNinety(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "девяносто";
			}
			return "ninety";
		}
		return "իննսուն";
	}

	internal static string GetHundred(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "сто";
			}
			return "hundred";
		}
		return "հարյուր";
	}

	internal static string GetThousand(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "тысяча";
			}
			return "thousand";
		}
		return "հազար";
	}

	internal static string GetMillion(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "миллион";
			}
			return "million";
		}
		return "միլիոն";
	}

	internal static string GetMilliard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "миллиард";
			}
			return "billion";
		}
		return "միլիարդ";
	}

	internal static string GetTrillion(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "триллион";
			}
			return "trillion";
		}
		return "տրիլիոն";
	}

	internal static string GetYes(string language)
	{
		if (language == "hy-AM")
		{
			return "Այո";
		}
		return "Yes";
	}

	internal static string GetNo(string language)
	{
		if (language == "hy-AM")
		{
			return "Ոչ";
		}
		return "No";
	}

	internal static string GetDocumentStateCanceled(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отменен";
			}
			return "Canceled";
		}
		return "Չեղարկված է";
	}

	internal static string GetDocumentStateConfirmed(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Исполнен";
			}
			return "Confirmed";
		}
		return "Կատարված է";
	}

	internal static string GetDocumentStateEditable(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Редактируемый";
			}
			return "Editable";
		}
		return "Խմբագրվող է";
	}

	internal static string GetDocumentStateAccepted(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Принятый";
			}
			return "Accepted";
		}
		return "Ընդունված է";
	}

	internal static string GetDocumentStateInProcessBank(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "В процессе";
			}
			return "In Process";
		}
		return "Մշակվում է";
	}

	internal static string GetDocumentStateNotSet(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Неизвестный";
			}
			return "Unknown";
		}
		return "Անորոշ է";
	}

	internal static string GetDocumentStateReady(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Готов";
			}
			return "Ready";
		}
		return "Պատրաստ է";
	}

	internal static string GetDocumentStateReadyToBeSent(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Готов к отправке";
			}
			return "Ready to be Sent";
		}
		return "Պատրաստ է ուղարկման";
	}

	internal static string GetDocumentStateRefused(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отклонен";
			}
			return "Refused";
		}
		return "Մերժված է";
	}

	internal static string GetDocumentStateSent(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отправлен в банк";
			}
			return "Sent to Bank";
		}
		return "Ուղարկված է բանկ";
	}

	internal static string GetDocumentStateInProcessOnline(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "В онлайн обработке";
			}
			return "In Online Process";
		}
		return "Մշակվում է օնլայն";
	}

	internal static string GetDocumentStateTemplate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Шаблон";
			}
			return "Template";
		}
		return "Ձևանմուշ";
	}

	internal static string GetDocumentStatePartiallySigned(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подписываемый";
			}
			return "Signable";
		}
		return "Ստորագրվող է";
	}

	internal static string GetDocumentStateClientSignature(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подпись клиента";
			}
			return "Client Signature";
		}
		return "Հաճախորդի ստորագրում";
	}

	internal static string GetSecurityProviderDigipassGoSeries(string language)
	{
		return "Digipass GO Series Token";
	}

	internal static string GetSecurityProviderDigipass250300Series(string language)
	{
		return "Digipass 250-300 Series Token";
	}

	internal static string GetSecurityProviderDigipassMobile(string language)
	{
		return "Digipass for Mobile";
	}

	internal static string GetSecurityProviderMobileSMS(string language)
	{
		return "SMS";
	}

	internal static string GetSecurityProviderGoogleAuth(string language)
	{
		return "Google Authenticator";
	}

	internal static string GetSecurityProviderDigipass(string language)
	{
		return "Digipass";
	}

	internal static string GetSecurityProviderDeactivate(string language)
	{
		return "Without security provider";
	}

	internal static string GetSecurityProviderNone(string language)
	{
		return "NONE (Only Username/Password)";
	}

	internal static string GetSystemTypeAccountant(string language)
	{
		if (language == "hy-AM")
		{
			return "ՀԾ-Հաշվապահ";
		}
		return "AS-Accountant";
	}

	internal static string GetSystemTypeImportService(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сервис импорта";
			}
			return "Import Service";
		}
		return "Ներմուծման ծառայություն";
	}

	internal static string GetSystemTypeMobileSite(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Мобильный сайт";
			}
			return "Mobile-Site";
		}
		return "Մոբայլ կայք";
	}

	internal static string GetSystemTypeWebSite(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Веб-сайт";
			}
			return "Web-Site";
		}
		return "Web կայք";
	}

	internal static string GetSystemTypeMobileApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Мобильное приложение";
			}
			return "Mobile Application";
		}
		return "Մոբայլ հավելված";
	}

	internal static string GetSystemTypeAdminWebSite(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Администратор";
			}
			return "Administrator";
		}
		return "Ադմինիստրատոր";
	}

	internal static string GetSystemTypeWorkerService(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Система";
			}
			return "System";
		}
		return "Համակարգ";
	}

	internal static string GetSystemTypeWeb(string language)
	{
		if (language == "ru-RU")
		{
			return "Веб";
		}
		return "Web";
	}

	internal static string GetSystemTypeCorporateApi(string language)
	{
		return "Corporate API";
	}

	internal static string GetSystemTypeAndroid(string language)
	{
		return "Android";
	}

	internal static string GetSystemTypeIOS(string language)
	{
		return "IOS";
	}

	internal static string GetSystemTypeAll(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Общее";
			}
			return "Overall";
		}
		return "Ընդհանուր";
	}

	internal static string GetSystemTypeOther(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Другое";
			}
			return "Other";
		}
		return "Այլ";
	}

	internal static string GetField_AdditionalInfo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сообщение (строка 1)";
			}
			return "Message (line 1)";
		}
		return "Հաղորդագրություն (տող 1)";
	}

	internal static string GetField_AdditionalInfo2(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сообщение (строка 2)";
			}
			return "Message (line 2)";
		}
		return "Հաղորդագրություն (տող 2)";
	}

	internal static string GetField_AdditionalInfo3(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сообщение (строка 3)";
			}
			return "Message (line 3)";
		}
		return "Հաղորդագրություն (տող 3)";
	}

	internal static string GetField_AdditionalInfo4(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сообщение (строка 4)";
			}
			return "Message (line 4)";
		}
		return "Հաղորդագրություն (տող 4)";
	}

	internal static string GetField_AdditionalInfo5(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сообщение (строка 5)";
			}
			return "Message (line 5)";
		}
		return "Հաղորդագրություն (տող 5)";
	}

	internal static string GetField_AdditionalInfo6(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сообщение (строка 6)";
			}
			return "Message (line 6)";
		}
		return "Հաղորդագրություն (տող 6)";
	}

	internal static string GetField_TaxAuthorityCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Код региона";
			}
			return "Regional Code";
		}
		return "Տարածաշրջանի/ՀՏ կոդ";
	}

	internal static string GetField_IsThirdParty(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Данные другого клиента";
			}
			return "Payment for a third party";
		}
		return "Վճարում այլ հաճախորդի համար";
	}

	internal static string GetField_ReportCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Код отчетности";
			}
			return "Report code";
		}
		return "Հաշվետվողականության կոդ";
	}

	internal static string GetField_ThirdPartyAddInfo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Прочие данные";
			}
			return "Third Party Other Information";
		}
		return "Լրացուցիչ տվյալներ";
	}

	internal static string GetField_ThirdPartyAdditionalInfo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Прочие данные";
			}
			return "Third Party Other Information";
		}
		return "Լրացուցիչ տվյալներ";
	}

	internal static string GetField_ThirdPartyAddress(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Адрес";
			}
			return "Third Party Address";
		}
		return "Հասցե";
	}

	internal static string GetField_ThirdPartyAuthPerson(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Уполномоченное лицо";
			}
			return "Third Party Authorized Person";
		}
		return "Լիազորված անձ";
	}

	internal static string GetField_ThirdPartyAuthorizedPerson(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Уполномоченное лицо";
			}
			return "Third Party Authorized Person";
		}
		return "Լիազորված անձ";
	}

	internal static string GetField_ThirdPartyLegalState(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Юридический статус";
			}
			return "Third Party Legal Status";
		}
		return "Իրավաբանական կարգավիճակ";
	}

	internal static string GetField_ThirdPartyPassportNo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Паспортные данные";
			}
			return "Third Party Passport Data";
		}
		return "Անձնագրի տվյալներ";
	}

	internal static string GetField_ThirdPartyPassportNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Паспортные данные";
			}
			return "Third Party Passport Data";
		}
		return "Անձնագրի տվյալներ";
	}

	internal static string GetField_ThirdPartyResidenceType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Резидентность";
			}
			return "Third Party Residence";
		}
		return "Ռեզիդենտություն";
	}

	internal static string GetField_ThirdPartySocialCard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Социальная карта №";
			}
			return "Third Party Social Card No.";
		}
		return "Սոց. քարտի N/ՀԾՀ";
	}

	internal static string GetField_ThirdPartyTaxCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "ИНН";
			}
			return "Third Party TIN";
		}
		return "ՀՎՀՀ";
	}

	internal static string GetField_ThirdPartyPassportType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип паспорта";
			}
			return "Third Party Passport Type";
		}
		return "Անձնագրի տեսակ";
	}

	internal static string GetField_IsUrgentTransfer(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Срочный перевод";
			}
			return "Urgent transfer";
		}
		return "Շտապ փոխանցում";
	}

	internal static string GetSectionTitlePayerBank(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Банк плательщика";
			}
			return "Remitter Bank";
		}
		return "Վճարողի բանկ";
	}

	internal static string GetSectionTitleBeneficiaryBank(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Банк получателя";
			}
			return "Beneficiary Bank";
		}
		return "Շահառուի բանկ";
	}

	internal static string GetField_ReceiverAcc(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет получателя";
			}
			return "Beneficiary Account";
		}
		return "Շահառուի հաշիվ";
	}

	internal static string GetField_TaxCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "ИНН";
			}
			return "TIN";
		}
		return "ՀՎՀՀ";
	}

	internal static string GetField_SocialCard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Социальная карта №";
			}
			return "Social Card No.";
		}
		return "Սոց. քարտի N/ՀԾՀ";
	}

	internal static string GetField_PayerLegalState(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Юридический статус плательщика";
			}
			return "Payer Legal Status";
		}
		return "Վճարողի իրավաբանական կարգավիճակ";
	}

	internal static string GetField_PayerPassportNo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Паспортные данные плательщика";
			}
			return "Payer Passport Data";
		}
		return "Վճարողի անձնագրի տվյալներ";
	}

	internal static string GetField_PayerPassportNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Паспортные данные плательщика";
			}
			return "Payer Passport Data";
		}
		return "Վճարողի անձնագրի տվյալներ";
	}

	internal static string GetField_PayerPassportType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип паспорта плательщика";
			}
			return "Payer Passport Type";
		}
		return "Վճարողի անձնագրի տեսակ";
	}

	internal static string GetField_PayerResidenceType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Резидентность плательщика";
			}
			return "Payer Residence";
		}
		return "Վճարողի ռեզիդենտություն";
	}

	internal static string GetField_CommissionAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Комиссия";
			}
			return "Commission";
		}
		return "Միջնորդավճար";
	}

	internal static string GetField_CommissionAmountInWords(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Комиссия прописью";
			}
			return "Commission In Words";
		}
		return "Միջնորդավճարը բառերով";
	}

	internal static string GetPassportDeathCertificate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Свидетельство о смерти";
			}
			return "Death certificate";
		}
		return "Մահվան վկայական";
	}

	internal static string GetPassportForeign(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Выдан иностранным государством";
			}
			return "Issued by foreign country";
		}
		return "Օտարերկրյա պետության կողմից տրված";
	}

	internal static string GetPassportRA(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Выдан в РА";
			}
			return "Issued by RA";
		}
		return "ՀՀ-ի կողմից տրված";
	}

	internal static string GetClientStatusLegal(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Юридическое лицо";
			}
			return "Legal Entity";
		}
		return "Իրավաբանական անձ";
	}

	internal static string GetClientStatusNatural(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Физическое лицо";
			}
			return "Natural Person";
		}
		return "Ֆիզիկական անձ";
	}

	internal static string GetClientStatusEntrepreneur(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Предприниматель";
			}
			return "Private Entrepreneur";
		}
		return "Անհատ ձեռներեց";
	}

	internal static string GetReceiverLegalStatusLegal(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Юридическое лицо / ИП";
			}
			return "Legal entity / SP";
		}
		return "Իրավաբանական անձ / ԱՁ";
	}

	internal static string GetClientStatusAll(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Общее";
			}
			return "Overall";
		}
		return "Ընդհանուր";
	}

	internal static string GetNonResident(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Нерезидент";
			}
			return "Non-Resident";
		}
		return "Ոչ ռեզիդենտ";
	}

	internal static string GetResident(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Резидент";
			}
			return "Resident";
		}
		return "Ռեզիդենտ";
	}

	internal static string GetField_RoadPolicePayment_ReceiverAcc(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет для оплаты";
			}
			return "Account for Payment";
		}
		return "Վճարման հաշվեհամար";
	}

	internal static string GetField_RoadPolicePayment_ViolationDebt(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Задолженность";
			}
			return "Debt";
		}
		return "Պարտք";
	}

	internal static string GetField_RoadPolicePayment_ViolationDetails(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Детали штрафа";
			}
			return "Violation Details";
		}
		return "Տուգանքի մանրամասներ";
	}

	internal static string GetField_RoadPolicePayment_ViolationFullName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Полное имя";
			}
			return "Full Name";
		}
		return "Տուգանվածի անվանումը/ԱԱՀ";
	}

	internal static string GetField_RoadPolicePayment_ViolationId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер постановления";
			}
			return "Decree No.";
		}
		return "Ոստիկանության վարչական տուգանքի որոշման համար";
	}

	internal static string GetDocumentTypePassportPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Перевод по паспортным данным";
			}
			return "Transfer via Passport Data";
		}
		return "Փոխանցում անձնագր. տվյալներով";
	}

	internal static string GetDocumentTypeDepositAttraction(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Пополнение депозита";
			}
			return "Deposit Replenishment";
		}
		return "Ավանդի համալրում";
	}

	internal static string GetDocumentTypeOpenAccountApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заявление на открытие счета";
			}
			return "Account Opening Application";
		}
		return "Հաշվի բացման հայտ";
	}

	internal static string GetDocumentTypeCloseAccountApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заявление на закрытии счета";
			}
			return "Account Closing Application";
		}
		return "Հաշվի փակման հայտ";
	}

	internal static string GetDocumentTypeBudgetaryPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Перевод в бюджет";
			}
			return "Budgetary Transfer";
		}
		return "Բյուջետային փոխանցում";
	}

	internal static string GetDocumentTypeArmSoftApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заявление для ArmSoft";
			}
			return "Application for ArmSoft";
		}
		return "Դիմում ՀԾ-ին";
	}

	internal static string GetDocumentTypeCreditRepaymentViaPartner(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Погашение кредита у партнера банка";
			}
			return "Loan Repayment at Bank's Partner";
		}
		return "Վարկի մարում բանկի գործընկերոջ մոտ";
	}

	internal static string GetDocumentTypeCashWithdrawalApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заявление на снятие наличных";
			}
			return "Cash Withdrawal Application";
		}
		return "Կանխիկացման հայտ";
	}

	internal static string GetDocumentTypeCommunalPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплата услуг";
			}
			return "Payment for Services";
		}
		return "Ծառայությունների վճարում";
	}

	internal static string GetDocumentTypeContactTransfer(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Перевод по контактными данными (тел., эл. почта)";
			}
			return "Transfer via Contact Data (phone, Email)";
		}
		return "Փոխանցում կոնտ. տվյալներով (հեռ., էլ. փոստ)";
	}

	internal static string GetDocumentTypeArCaP2PTransfer(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Перевод через систему ArCaPay";
			}
			return "Transfer via ArCaPay system";
		}
		return "Փոխանցում ArCaPay համակարգով";
	}

	internal static string GetDocumentTypeCreditRepayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Погашение кредита";
			}
			return "Loan Repayment";
		}
		return "Վարկի մարում";
	}

	internal static string GetDocumentTypeCreditRepaymentWithAttachedCard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Погашение кредита";
			}
			return "Loan Repayment";
		}
		return "Վարկի մարում";
	}

	internal static string GetDocumentTypeCurrencyConversion(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Обмен валюты";
			}
			return "Currency Exchange";
		}
		return "Արտարժույթի փոխանակում";
	}

	internal static string GetDocumentTypeCustomizedApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Описываемое заявление";
			}
			return "Customized Application";
		}
		return "Նկարագրվող հայտ";
	}

	internal static string GetDocumentTypeForeignPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Международный перевод";
			}
			return "International Transfer";
		}
		return "Միջազգային փոխանցում";
	}

	internal static string GetDocumentTypeForeignPaymentRus(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Перевод RUB";
			}
			return "Transfer in RUB";
		}
		return "RUB-ով փոխանցում";
	}

	internal static string GetDocumentTypeLocalPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Перевод на счет";
			}
			return "Transfer to Account";
		}
		return "Փոխանցում հաշվին";
	}

	internal static string GetDocumentTypeGarbageCollectionTaxPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплата за сбор мусора";
			}
			return "Garbage Collection Payment";
		}
		return "Աղբահանության վճարում";
	}

	internal static string GetDocumentTypeGoalAccountClose(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Закрытие целевого счета";
			}
			return "Goal Account Closure";
		}
		return "Նպատակային հաշվի փակում";
	}

	internal static string GetDocumentTypeMultipleCommunal(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Групповая оплата услуг";
			}
			return "Service Group Payment";
		}
		return "Ծառայությունների խմբային վճարում";
	}

	internal static string GetDocumentTypeMultiplePayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Групповой перевод";
			}
			return "Multiple Transfer";
		}
		return "Խմբային փոխանցում";
	}

	internal static string GetDocumentTypeOpenDepositApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заявление на открытие депозита";
			}
			return "Deposit Opening Application";
		}
		return "Ավանդի բացման հայտ";
	}

	internal static string GetDocumentTypeOpenCreditApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заявка на кредит";
			}
			return "Loan Application";
		}
		return "Վարկի ստացման հայտ";
	}

	internal static string GetDocumentTypeOpenDigitalCardApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заявление на открытие цифровой карты";
			}
			return "Digital Card Opening Application";
		}
		return "Թվային քարտի բացման հայտ";
	}

	internal static string GetDocumentTypePaymentsPackage(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Пакет платежей";
			}
			return "Payments Package";
		}
		return "Վճարումների փաթեթ";
	}

	internal static string GetDocumentTypeRequestCancel(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заявка на отмену";
			}
			return "Cancellation Request";
		}
		return "Չեղարկման հայտ";
	}

	internal static string GetDocumentTypeRoadPolicePayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплата административных штрафов полиции";
			}
			return "Police Administrative Fine Payment";
		}
		return "Ոստիկանության վարչական տուգանքի վճարում";
	}

	internal static string GetDocumentTypeSoldierPayment(string language)
	{
		if (language == "hy-AM")
		{
			return "Նվիրատվություն ԶԻՆԱՊԱՀ հիմնադրամին";
		}
		return "Donation to ZINAPAH Foundation";
	}

	internal static string GetDocumentTypeTaxPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплата налога на имущество (Ереван)";
			}
			return "Property Tax Payment (Yerevan)";
		}
		return "Գույքահարկի վճարում (Երևան)";
	}

	internal static string GetDocumentTypeGarbageCollectionPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплата вывоза мусора (Ереван) до 2024 года";
			}
			return "Garbage Collection Payment (Yerevan) until 2024";
		}
		return "Աղբահանության վճարում (Երևան) մինչև 2024";
	}

	internal static string GetDocumentTypeEvocaGiftCardApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заявление на открытие подарочной карты";
			}
			return "Gift Card Opening Application";
		}
		return "Նվեր քարտի բացման հայտ";
	}

	internal static string GetDocumentTypeEvocaDigitalGiftCardApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заявление на открытие цифровой подарочной карты";
			}
			return "Digital Gift Card Opening Application";
		}
		return "Թվային նվեր քարտի բացման հայտ";
	}

	internal static string GetDocumentTypeEvocaGetBondApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заявление на покупку облигации";
			}
			return "Bond Purchasing Application";
		}
		return "Պարտատոմսի գնման հայտ";
	}

	internal static string GetDocumentTypePropertyTaxPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплата налога на имущество";
			}
			return "Property Tax Payment";
		}
		return "Գույքահարկի վճարում";
	}

	internal static string GetDocumentTypeVehicleTaxPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплата налога на транспортное средство";
			}
			return "Vehicle Tax Payment";
		}
		return "Մեքենայի գույքահարկի վճարում";
	}

	internal static string GetDocumentTypeRealEstateTaxPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплата налога на недвижимость";
			}
			return "Real Estate Tax Payment";
		}
		return "Անշարժ գույքի գույքահարկի վճարում";
	}

	internal static string GetOtherTransfer(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Перевод (другое)";
			}
			return "Transfer (other)";
		}
		return "Փոխանցում (այլ)";
	}

	internal static string GetDocumentTypeAccountReplenishment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Пополнение счета";
			}
			return "Account Replenishment";
		}
		return "Հաշվի համալրում";
	}

	internal static string GetAccountReplenishment(string language)
	{
		return "Account Replenishment";
	}

	internal static string GetDocumentTypeCardTransfer(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Перевод на карту";
			}
			return "Transfer to Card";
		}
		return "Փոխանցում քարտին";
	}

	internal static string GetDocumentTypeFriendCreditRepayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Погашение кредита друга";
			}
			return "Friend's Loan Repayment";
		}
		return "Ընկերոջ վարկի մարում";
	}

	internal static string GetDocumentTypeCashOutByPhone(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Снятие наличных номером мобильного телефона";
			}
			return "Cash Withdrawal By Phone Number";
		}
		return "Կանխիկացում հեռախոսահամարով";
	}

	internal static string GetDocumentTypeCompulsoryEnforcementPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "СПИСА платеж";
			}
			return "CES Payment";
		}
		return "ՀԿԱԾ վճարում";
	}

	internal static string GetDocumentTypeASWAPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подписание договора ОСАГО";
			}
			return "MTPL Contract Signing";
		}
		return "ԱՊՊԱ պայմանագրի կնքում";
	}

	internal static string GetDocumentTypeEvocaGetLoanApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заявление на получение кредита";
			}
			return "Get loan application";
		}
		return "Վարկի ստացման հայտ";
	}

	internal static string GetDocumentTypeAmioLoanApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Онлайн кредит";
			}
			return "Online loan";
		}
		return "Օնլայն վարկ";
	}

	internal static string GetAdvancedPaymentCreditRepayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Перевод для выплаты кредита";
			}
			return "Transfer for Credit Repayment";
		}
		return "Փոխանցում վարկի մարման համար";
	}

	internal static string GetDocumentTypeAdvancedTransfer(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Расширенный перевод";
			}
			return "Advanced Transfer";
		}
		return "Ընդլայնված փոխանցում";
	}

	internal static string GetRateSuggestionStateNew(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Новый";
			}
			return "New";
		}
		return "Նոր";
	}

	internal static string GetRateSuggestionOperationCreated(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Созданный";
			}
			return "Created";
		}
		return "Ստեղծվել է";
	}

	internal static string GetRateSuggestionStateAssigned(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "На рассмотрении";
			}
			return "Under Review";
		}
		return "Դիտարկվում է";
	}

	internal static string GetRateSuggestionOperationAssigned(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Назначенный";
			}
			return "Assigned";
		}
		return "Կցվել է";
	}

	internal static string GetRateSuggestionOperationStarted(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Предложенный";
			}
			return "Suggested";
		}
		return "Առաջարկվել է";
	}

	internal static string GetRateSuggestionStateStarted(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "В ожидании ответа клиента";
			}
			return "Waiting For Client Response";
		}
		return "Սպասում է հաճախորդի արձագանքին";
	}

	internal static string GetRateSuggestionStateAcceptedByClient(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Принято";
			}
			return "Accepted";
		}
		return "Ընդունված է";
	}

	internal static string GetRateSuggestionOperationAcceptedByClient(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Принято";
			}
			return "Accepted";
		}
		return "Ընդունվել է";
	}

	internal static string GetRateSuggestionStateSigned(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подписано";
			}
			return "Signed";
		}
		return "Ստորագրված է";
	}

	internal static string GetRateSuggestionOperationSigned(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подписано";
			}
			return "Signed";
		}
		return "Ստորագրվել է";
	}

	internal static string GetRateSuggestionStateConfirmedByBank(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Исполнено";
			}
			return "Confirmed";
		}
		return "Կատարված է";
	}

	internal static string GetRateSuggestionOperationConfirmedByBank(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Исполнено";
			}
			return "Confirmed";
		}
		return "Կատարվել է";
	}

	internal static string GetRateSuggestionStateRefusedByClient(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отменено";
			}
			return "Canceled";
		}
		return "Չեղարկված է";
	}

	internal static string GetRateSuggestionOperationRefusedByClient(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отменено";
			}
			return "Canceled";
		}
		return "Չեղարկվել է";
	}

	internal static string GetRateSuggestionStateRefusedByOperator(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отклонено";
			}
			return "Rejected";
		}
		return "Մերժվել է";
	}

	internal static string GetRateSuggestionOperationRefusedByOperator(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отклонено";
			}
			return "Rejected";
		}
		return "Մերժված է";
	}

	internal static string GetRateSuggestionStateRefusedByBank(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отклонено банком";
			}
			return "Rejected By Bank";
		}
		return "Մերժված է բանկի կողմից";
	}

	internal static string GetRateSuggestionOperationRefusedByBank(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return ">Отклонено банком";
			}
			return "Rejected By Bank";
		}
		return "Մերժվել է բանկի կողմից";
	}

	internal static string GetRateSuggestionStateExpired(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Просроченный";
			}
			return "Expired";
		}
		return "Ժամկետանց է";
	}

	internal static string GetRateSuggestionOperationExpired(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Просроченный";
			}
			return "Expired";
		}
		return "Ժամկետանց է";
	}

	internal static string GetRateSuggestionStateGroupInProcess(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "В процессе";
			}
			return "In Process";
		}
		return "Ընթացիկ";
	}

	internal static string GetRateSuggestionStateGroupWaitingToBeSigned(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "В ожидании подписи";
			}
			return "Waiting To Be Signed";
		}
		return "Ստորագրման ենթակա";
	}

	internal static string GetRateSuggestionStateGroupCompleted(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Завершенные";
			}
			return "Completed";
		}
		return "Ավարտված";
	}

	internal static string GetRateSuggestionStateGroupAssigned(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Назначенные мне";
			}
			return "Assigned To Me";
		}
		return "Կցված ինձ";
	}

	internal static string GetRateSuggestionStateGroupNew(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Новые";
			}
			return "New";
		}
		return "Նոր";
	}

	internal static string GetRateSuggestionStateGroupAssignedToAnotherOperator(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Назначено другому оператору";
			}
			return "Assigned to another operator";
		}
		return "Կցված է այլ օպերատորի";
	}

	internal static string GetField_CardTransfer_ReceiverCountryCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Страна карты";
			}
			return "Card Country";
		}
		return "Քարտի երկիր";
	}

	internal static string GetField_ReceiverCountryCode(string language)
	{
		if (language == "ru-RU")
		{
			return "Страна карты";
		}
		return "Card Country";
	}

	internal static string GetField_ReceiverCardNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер карты получателя";
			}
			return "Beneficiary Card No.";
		}
		return "Շահառուի քարտի N";
	}

	internal static string GetField_ReceiverCardAccountNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер счета получателя";
			}
			return ">Beneficiary Account No.";
		}
		return "Շահառուի հաշվի N";
	}

	internal static string GetField_DocNum(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер транзакции";
			}
			return "Transaction Number";
		}
		return "Գործարքի համար";
	}

	internal static string GetField_Number(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер транзакции";
			}
			return "Transaction Number";
		}
		return "Գործարքի համար";
	}

	internal static string GetAccountTypeSettlement(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Расчетный счет";
			}
			return "Settlement Account";
		}
		return "Հաշվարկային հաշիվ";
	}

	internal static string GetAccountTypeCard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Карточный счет";
			}
			return "Card Account";
		}
		return "Քարտային հաշիվ";
	}

	internal static string GetAccountTypeOther(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Другой счет";
			}
			return "Other Account";
		}
		return "Այլ հաշիվ";
	}

	internal static string GetField_ReceiverId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет получателя/Карта получателя";
			}
			return "Beneficiary Account/Beneficiary Card";
		}
		return "Շահառուի հաշվիվ/Շահառուի քարտ";
	}

	internal static string GetField_Attachment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Прикрепленный файл";
			}
			return "Attached File";
		}
		return "Կցված ֆայլ";
	}

	internal static string GetCardStatusActive(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Активный";
			}
			return "Active";
		}
		return "Ակտիվ";
	}

	internal static string GetCardStatusBlocked(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заблокированный";
			}
			return "Blocked";
		}
		return "Բլոկավորված";
	}

	internal static string GetCardStatusBlockedByBank(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заблокирован банком";
			}
			return "Blocked by bank";
		}
		return "Բլոկավորված բանկի կողմից";
	}

	internal static string GetCardStatusExpired(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Просроченный";
			}
			return "Expired";
		}
		return "Ժամկետանց";
	}

	internal static string GetCardStatusInactive(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Карта не активирована";
			}
			return "Card is not activated";
		}
		return "Քարտն ակտիվացված չէ";
	}

	internal static string GetCardTypeVisa(string language)
	{
		return "Visa";
	}

	internal static string GetCardTypeMasterCard(string language)
	{
		return "MasterCard";
	}

	internal static string GetCardTypeArCa(string language)
	{
		return "ArCa";
	}

	internal static string GetCardTypeAmex(string language)
	{
		return "Amex";
	}

	internal static string GetCardTypeDinersClub(string language)
	{
		return "DinersClub";
	}

	internal static string GetCardTypeUnionPay(string language)
	{
		return "UnionPay";
	}

	internal static string GetCardTypeJcb(string language)
	{
		return "Jcb";
	}

	internal static string GetCardTypeDiscoverCard(string language)
	{
		return "DiscoverCard";
	}

	internal static string GetCardTypeMaestro(string language)
	{
		return "Maestro";
	}

	internal static string GetCardTypeMir(string language)
	{
		return "Mir";
	}

	internal static string GetField_AttachedCardPayment_CardNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Прикрепленная карта";
			}
			return "Attached Card";
		}
		return "Կցված քարտ";
	}

	internal static string GetField_AttachedCardPayment_AttachedCardId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Карта";
			}
			return "Card";
		}
		return "Քարտ";
	}

	internal static string GetField_OpenAccountApplication_AccountTypeId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип счета";
			}
			return "Account Type";
		}
		return "Հաշվի տիպ";
	}

	internal static string GetField_Rate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Курс обмена";
			}
			return "Exchange Rate";
		}
		return "Փոխարժեք";
	}

	internal static string GetField_CurrencyConversion_PayerAcc(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет дебет";
			}
			return "Debit Account";
		}
		return "Հաշիվ դեբետ";
	}

	internal static string GetField_CurrencyConversion_Amount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма дебет";
			}
			return "Debit Amount";
		}
		return "Դեբետ գումար";
	}

	internal static string GetField_CurrencyConversion_ReceiverAcc(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет кредит";
			}
			return "Credit Account";
		}
		return "Հաշիվ կրեդիտ";
	}

	internal static string GetField_AmountCredit(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма кредит";
			}
			return "Credit Amount";
		}
		return "Կրեդիտ գումար";
	}

	internal static string GetField_UseBankRate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Применить текущий курс банка";
			}
			return "Apply The Bank's Current Rate";
		}
		return "Կիրառել բանկի ընթացիկ փոխարժեքը";
	}

	internal static string GetIdentificationDocumentTypeBirthCertificate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Свидетельство о рождении";
			}
			return "Birth certificate";
		}
		return "Ծննդյան վկայական";
	}

	internal static string GetIdentificationDocumentTypeForeignPassport(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Заграничный паспорт";
			}
			return "Foreign passport";
		}
		return "Օտարերկրյա անձնագիր";
	}

	internal static string GetIdentificationDocumentTypeInternationallyRecognizedIdentityDocuments(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Международно признанные документы, удостоверяющие личность";
			}
			return "Internationally recognized identity documents";
		}
		return "Անձը հաստատող միջազգայնորեն ճանաչված փաստաթղթեր";
	}

	internal static string GetIdentificationDocumentTypeMilitaryCard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Военный билет";
			}
			return "Military card";
		}
		return "Զինվորական գրքույկ";
	}

	internal static string GetIdentificationDocumentTypeOther(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Другое";
			}
			return "Other";
		}
		return "Այլ";
	}

	internal static string GetIdentificationDocumentTypeRABiometricPassport(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Биометрический паспорт РА";
			}
			return "Biometric Passport of RA";
		}
		return "ՀՀ բիոմետրիկ անձնագիր";
	}

	internal static string GetIdentificationDocumentTypeRAIDCard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Идентификационная карта РА";
			}
			return "RA ID Card";
		}
		return "ՀՀ նույնականացման քարտ";
	}

	internal static string GetIdentificationDocumentTypeRANonBiometricPassport(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Небиометрический паспорт РА";
			}
			return "Non-Biometric Passport of RA";
		}
		return "ՀՀ ոչ բիոմետրիկ անձնագիր";
	}

	internal static string GetIdentificationDocumentTypeRAReturnCertificate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сертификат на возвращение в РА";
			}
			return "Return certificate to RA";
		}
		return "Վերադարձի վկայական՝ միայն ՀՀ վերադառնալու համար";
	}

	internal static string GetIdentificationDocumentTypeRASpecialPassport(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Специальный паспорт РА";
			}
			return "RA special passport";
		}
		return "ՀՀ հատուկ անձնագիր";
	}

	internal static string GetIdentificationDocumentTypeRefugeeCertificate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Свидетельство беженца";
			}
			return "Refugee certificate";
		}
		return "Փախստականի վկայական";
	}

	internal static string GetIdentificationDocumentTypeResidenceCard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Вид на жительство";
			}
			return "Residence card";
		}
		return "Կացության վկայական";
	}

	internal static string GetIdentificationDocumentTypeTemporaryAsylumCertificate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Временное свидетельство о предоставлении убежища";
			}
			return "Temporary asylum certificate";
		}
		return "Ժամանակավոր ապաստանի վկայական";
	}

	internal static string GetIdentificationDocumentTypeTemporaryIdentityDocument(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Временное удостоверение личности, выданное Полицией РА";
			}
			return "Temporary identity document given by RA Police";
		}
		return "ՀՀ Ոստիկանության կողմից ժամանակավորապեu տրվող անձը հաստատող փաստաթուղթ";
	}

	internal static string GetIdentificationDocumentTypeTemporaryResidenceCard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Временный вид на жительство";
			}
			return "Temporary residence card";
		}
		return "Ժամանակավոր կացության քարտ";
	}

	internal static string GetIdentificationDocumentTypeTravelDocument(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Проездной документ";
			}
			return "Travel document";
		}
		return "Ճամփորդական փաստաթուղթ";
	}

	internal static string GetTransactionDirection_Credit(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Кредит";
			}
			return "Credit";
		}
		return "Կրեդիտային";
	}

	internal static string GetTransactionDirection_Debit(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дебет";
			}
			return "Debit";
		}
		return "Դեբետային";
	}

	internal static string GetField_ForeignPayment_PayerAddress(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 2";
			}
			return "Line 2";
		}
		return "Տող 2";
	}

	internal static string GetField_ForeignPayment_PayerCity(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 3";
			}
			return "Line 3";
		}
		return "Տող 3";
	}

	internal static string GetField_ForeignPayment_PayerCountry(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 4";
			}
			return "Line 4";
		}
		return "Տող 4";
	}

	internal static string GetField_ForeignPayment_PayerName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 1";
			}
			return "Line 1";
		}
		return "Տող 1";
	}

	internal static string GetField_ForeignPayment_PayerOption(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Детали плательщика";
			}
			return "Payer Option";
		}
		return "Վճարողի տվյալներ";
	}

	internal static string GetField_ForeignPayment_ReferenceNo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Ссылка №";
			}
			return "Reference No.";
		}
		return "Հղում N";
	}

	internal static string GetField_IgnoreCommissionChange(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Совершить перевод несмотря на изменение комиссии";
			}
			return "Make the transfer despite the commission change";
		}
		return "Կատարել փոխանցումը միջնորդավճարի փոփոխության դեպքում";
	}

	internal static string GetField_IsSelfCharged(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Включить комиссию банка-отправителя в переводимую сумму";
			}
			return "Include charges of sender bank in payment amount";
		}
		return "Ուղարկող բանկի գանձումը ներառել փոխանցվող գումարում";
	}

	internal static string GetField_Charges(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Расходы";
			}
			return "Charges";
		}
		return "Գանձում";
	}

	internal static string GetField_ForeignPayment_AdditionalInfo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 1";
			}
			return "Line 1";
		}
		return "Տող 1";
	}

	internal static string GetField_ForeignPayment_AdditionalInfo2(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 2";
			}
			return "Line 2";
		}
		return "Տող 2";
	}

	internal static string GetField_ForeignPayment_AdditionalInfo3(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 3";
			}
			return "Line 3";
		}
		return "Տող 3";
	}

	internal static string GetField_ForeignPayment_AdditionalInfo4(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 4";
			}
			return "Line 4";
		}
		return "Տող 4";
	}

	internal static string GetField_ForeignPayment_AdditionalInfo5(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 5";
			}
			return "Line 5";
		}
		return "Տող 5";
	}

	internal static string GetField_ForeignPayment_AdditionalInfo6(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 6";
			}
			return "Line 6";
		}
		return "Տող 6";
	}

	internal static string GetField_ReceivingCountryCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Страна получения";
			}
			return "Receiving country";
		}
		return "Ստացման երկիր";
	}

	internal static string GetField_ReceiverResidenceCountry(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Страна резидентства получателя";
			}
			return "Receiver Residence Country";
		}
		return "Ստացողի ռեզիդենտության երկիր";
	}

	internal static string GetField_ReceiverLegalStatus(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Юридический статус получателя";
			}
			return "Receiver Legal Status";
		}
		return "Ստացողի իրավաբանական կարգավիճակ";
	}

	internal static string GetField_LocalPayment_ReceiverLegalStatus(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Юридический статус получателя";
			}
			return "Beneficiary Legal Status";
		}
		return "Շահառուի իրավաբանական կարգավիճակ";
	}

	internal static string GetField_MultiplePayment_ReceiverLegalStatus(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Юридический статус получателя";
			}
			return "Beneficiary Legal Status";
		}
		return "Շահառուի իրավաբանական կարգավիճակ";
	}

	internal static string GetField_TransferPurpose(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Цель перевода";
			}
			return "Transfer Purpose";
		}
		return "Փոխանցման նպատակ";
	}

	internal static string GetField_IntermediaryBankAddress(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Адрес";
			}
			return "Address";
		}
		return "Հասցե";
	}

	internal static string GetField_IntermediaryBankCity(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Город";
			}
			return "City";
		}
		return "Քաղաք";
	}

	internal static string GetField_IntermediaryBankCountry(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Страна";
			}
			return "Country";
		}
		return "Երկիր";
	}

	internal static string GetField_IntermediaryBankID(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет Банка";
			}
			return "Bank Account";
		}
		return "Բանկի հաշիվ";
	}

	internal static string GetField_IntermediaryBankName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название";
			}
			return "Name";
		}
		return "Անվանում";
	}

	internal static string GetField_IntermediaryBankSwift(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "SWIFT код";
			}
			return "SWIFT code";
		}
		return "SWIFT կոդ";
	}

	internal static string GetField_ReceiverAddress(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Адрес";
			}
			return "Beneficiary Address";
		}
		return "Հասցե";
	}

	internal static string GetField_ReceiverBankAddress(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Адрес";
			}
			return "Address";
		}
		return "Հասցե";
	}

	internal static string GetField_ReceiverBankCity(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Город";
			}
			return "City";
		}
		return "Քաղաք";
	}

	internal static string GetField_ReceiverBankCountry(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Страна";
			}
			return "Country";
		}
		return "Երկիր";
	}

	internal static string GetField_ReceiverBankID(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет Банка";
			}
			return "Bank Account";
		}
		return "Բանկի հաշիվ";
	}

	internal static string GetField_ReceiverBankName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название";
			}
			return "Name";
		}
		return "Անվանում";
	}

	internal static string GetField_ReceiverBankSwift(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "SWIFT код";
			}
			return "SWIFT code";
		}
		return "SWIFT կոդ";
	}

	internal static string GetField_ForeignPayment_ReceiverAcc(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет";
			}
			return "Account";
		}
		return "Հաշիվ";
	}

	internal static string GetField_ReceiverCity(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Город";
			}
			return "City";
		}
		return "Քաղաք";
	}

	internal static string GetField_ReceiverCountry(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Страна";
			}
			return "Beneficiary Country";
		}
		return "Երկիր";
	}

	internal static string GetField_PayerBankCity(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Город";
			}
			return "City";
		}
		return "Քաղաք";
	}

	internal static string GetField_PayerBankCountry(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Страна";
			}
			return "Country";
		}
		return "Երկիր";
	}

	internal static string GetField_PayerBankID(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет банка";
			}
			return "Bank Account";
		}
		return "Բանկի հաշիվ";
	}

	internal static string GetField_PayerBankName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название";
			}
			return "Name";
		}
		return "Անվանում";
	}

	internal static string GetField_PayerBankAddress(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Адрес";
			}
			return "Address";
		}
		return "Հասցե";
	}

	internal static string GetField_PayerBankSwift(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "SWIFT код";
			}
			return "SWIFT Code";
		}
		return "SWIFT կոդ";
	}

	internal static string GetChargesBEN(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "BEN: Все расходы банков-посредников и корреспондентов взимаются с суммы перевода";
			}
			return "BEN: All costs of intermediary and correspondent banks are charged from the transferred amount";
		}
		return "BEN: Թղթակից և բոլոր միջնորդ բանկերի ծախսերը գանձվում են փոխանցվող գումարից";
	}

	internal static string GetChargesOUR(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "OUR: Плательщик несет только расходы {0}а и банка-корреспондента";
			}
			return "OUR: The payer bears only the costs of {0} and the correspondent bank";
		}
		return "OUR: Վճարողը կրում է միայն {0}ի և թղթակից բանկի ծախսերը";
	}

	internal static string GetChargesOURG(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "OURG: Плательщик несет все расходы, в том числе расходы банков-посредников";
			}
			return "OURG: The payer bears all costs, including those of the intermediary banks";
		}
		return "OURG: Վճարողը կրում է բոլոր ծախսերը` ներառյալ միջնորդ բանկերի ծախսերը";
	}

	internal static string GetChargesSHA(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "SHA: Расходы отправляющей стороны оплачивает плательщик,расходы получающей стороны оплачивает получатель";
			}
			return "SHA: Charges on the Sender's side are borne by the Payer,charges on the Receiver's side are borne by Beneficiary";
		}
		return "SHA: Ուղարկող կողմի ծախսերը հոգում է վճարողը, ստացող կողմի ծախսերը հոգում է շահառուն";
	}

	internal static string GetField_OptionF(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Использовать опцию F для деталей плательщика";
			}
			return "Use option F for payer details";
		}
		return "Օգտագործել վճարողի տվյալների F տարբերակը";
	}

	internal static string GetField_DefaultOption(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Опция по умолчанию, утвержденная банком";
			}
			return "Default option chosen by the bank";
		}
		return "Լռելյայն տարբերակը` որոշված բանկի կողմից";
	}

	internal static string GetNameAndAddressCodesAdditionalInformation(string language)
	{
		return "Additional Information";
	}

	internal static string GetNameAndAddressCodesAddress(string language)
	{
		return "Address Line";
	}

	internal static string GetNameAndAddressCodesBirthDate(string language)
	{
		return "Date of Birth";
	}

	internal static string GetNameAndAddressCodesBirthPlace(string language)
	{
		return "Place of Birth";
	}

	internal static string GetNameAndAddressCodesCountryTown(string language)
	{
		return "Country and Town";
	}

	internal static string GetNameAndAddressCodesCustomerIdentificationNumber(string language)
	{
		return "Customer Identification Number";
	}

	internal static string GetNameAndAddressCodesName(string language)
	{
		return "Name of the ordering customer";
	}

	internal static string GetNameAndAddressCodesNationalIdentityNumber(string language)
	{
		return "National Identity Number";
	}

	internal static string GetField_ChargeAcc(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет комиссии";
			}
			return "Commission Account";
		}
		return "Միջնորդավճարի հաշիվ";
	}

	internal static string GetEvocaMobileLoanType_Action(string language)
	{
		return "Action";
	}

	internal static string GetEvocaMobileLoanType_Overdraft(string language)
	{
		return "Overdraft";
	}

	internal static string GetPushStatusIsEnabled(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Активный";
			}
			return "Enabled";
		}
		return "Միացված է";
	}

	internal static string GetPushStatusIsDisabled(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Деактивирован";
			}
			return "Disabled";
		}
		return "Անջատված է";
	}

	internal static string GetPushStatusIsInactive(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Неактивный";
			}
			return "Inactive";
		}
		return "Ակտիվ չէ";
	}

	internal static string GetPushStatusIsTurnedOff(string language)
	{
		if (language == "hy-AM")
		{
			return "Անջատված է (ադմինի կողմից)";
		}
		return "Disabled (by admin)";
	}

	internal static string GetAccountTransactionTypeTransfer(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Перевод";
			}
			return "Transfer";
		}
		return "Փոխանցում";
	}

	internal static string GetAccountTransactionTypeMiscellaneous(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Прочее";
			}
			return "Other";
		}
		return "Այլ";
	}

	internal static string GetAccountTransactionTypeRepayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Погашение";
			}
			return "Repayment";
		}
		return "Մարում";
	}

	internal static string GetAccountTransactionTypeRevaluation(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Переоценка";
			}
			return "Revaluation";
		}
		return "Վերագնահատում";
	}

	internal static string GetAccountTransactionTypeInterestCalculation(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Начисление процентов";
			}
			return "Interest Calculation";
		}
		return "Տոկոսի հաշվարկում";
	}

	internal static string GetAccountTransactionTypeInterestCapitalization(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Капитализация процентов";
			}
			return "Interest Capitalization";
		}
		return "Տոկոսի կապիտալացում";
	}

	internal static string GetAccountTransactionTypeCurrencyExchange(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Конвертация валюты";
			}
			return "Currency Exchange";
		}
		return "Արտարժույթի փոխանակում";
	}

	internal static string GetAccountTransactionTypeProvision(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Резервирование";
			}
			return "Provision/De-provision";
		}
		return "Պահուստավորում/Ապապահուստավորում";
	}

	internal static string GetAccountTransactionTypeCommission(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Комиссия";
			}
			return "Commission";
		}
		return "Միջնորդավճար";
	}

	internal static string GetAccountTransactionTypeCommissionWithCurrencyExchange(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Комиссия с конвертацией";
			}
			return "Commission With Currency Exchange";
		}
		return "Միջնորդավճար արտարժույթի փոխանակումով";
	}

	internal static string GetAccountTransactionTypePenalty(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Штраф";
			}
			return "Penalty";
		}
		return "Տույժ";
	}

	internal static string GetAccountTransactionTypePenaltyWithCurrencyExchange(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Штраф с конвертацией";
			}
			return "Penalty With Currency Exchange";
		}
		return "Տույժ արտարժույթի փոխանակումով";
	}

	internal static string GetAccountTransactionTypeInterest(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Проценты";
			}
			return "Interest";
		}
		return "Տոկոսագումար";
	}

	internal static string GetAccountTransactionTypeInterestWithCurrencyExchange(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Проценты с конвертацией";
			}
			return "Interest With Currency Exchange";
		}
		return "Տոկոսագումար արտարժույթի փոխանակումով";
	}

	internal static string GetAccountTransactionTypeOverdraftRepayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Выдача/Погашение овердрафта";
			}
			return "Overdraft Disbursement/Repayment";
		}
		return "Օվերդրաֆտի տրամադրում/մարում";
	}

	internal static string GetAccountTransactionTypeOverdraftRepaymentWithOtherAccount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Выдача/Погашение овердрафта другим счетом";
			}
			return "Overdraft Disbursement/Repayment With Other Account";
		}
		return "Օվերդրաֆտի տրամադրում/մարում այլ հաշվով";
	}

	internal static string GetAccountTransactionTypeOverdraftRepaymentWithCurrencyExchange(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Выдача/Погашение овердрафта с обменом валюты";
			}
			return "Overdraft Disbursement/Repayment With Currency Exchange";
		}
		return "Օվերդրաֆտի տրամադրում/մարում արտարժույթի փոխանակումով";
	}

	internal static string GetAccountTransactionTypeBalanceAdjustment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Регулировка баланса";
			}
			return "Balance Adjustment";
		}
		return "Մնացորդի ճշտում";
	}

	internal static string GetCreditReceiveMethodCash(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "В филиале";
			}
			return "In Branch";
		}
		return "Մասնաճյուղում";
	}

	internal static string GetCreditReceiveMethodWithCard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "С картой";
			}
			return "With Card";
		}
		return "Քարտով";
	}

	internal static string GetCreditReceiveMethodWithAccount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "С аккаунтом";
			}
			return "With Account";
		}
		return "Հաշվով";
	}

	internal static string GetCreditReceiveMethodWithAttachedCard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return " С прикреплённой картой";
			}
			return "With Attached Card";
		}
		return "Կցված քարտով";
	}

	internal static string GetOpenCreditFileTypeContract(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Соглашение";
			}
			return "Agreement";
		}
		return "Պայմանագիր";
	}

	internal static string GetOpenCreditFileTypeArbitrage(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Арбитражное соглашение";
			}
			return "Arbitration agreement";
		}
		return "Արբիտրաժային համաձայնագիր";
	}

	internal static string GetOpenCreditFileTypeIndividualSheet(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Индивидуальный лист";
			}
			return "Individual sheet";
		}
		return "Անհատական թերթիկ";
	}

	internal static string GetField_IsSelfEmployed(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Самозанятое лицо";
			}
			return "Self Employed";
		}
		return "Ինքնազբաղ անձ";
	}

	internal static string GetField_CreditRepayment_RepaymentType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип погашения";
			}
			return "Repayment Type";
		}
		return "Մարման եղանակ";
	}

	internal static string GetField_CreditRepayment_Receiver(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название договора";
			}
			return "Contract Name";
		}
		return "Պայմանագրի անվանում";
	}

	internal static string GetField_CreditRepayment_IsFullRepayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Полное погашение";
			}
			return "Full Repayment";
		}
		return "Ամբողջական մարում";
	}

	internal static string GetField_CreditRepayment_OverpaymentPrincipal(string language)
	{
		if (language == "hy-AM")
		{
			return "Մայր գումար";
		}
		return "Principal";
	}

	internal static string GetField_OverpaymentFee(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Комиссия за досрочное погашение";
			}
			return "Fee for premature payment";
		}
		return "Վաղաժամկետ մարումից գանձում";
	}

	internal static string GetField_OverpaymentPrincipalInWords(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Основная сумма прописью";
			}
			return "Principal in Words";
		}
		return "Մայր գումարը բառերով";
	}

	internal static string GetField_OverpaymentFeeInWords(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Комиссия за досрочное погашение прописью";
			}
			return "Fee for premature payment in words";
		}
		return "Վաղաժամկետ մարումից գանձումը բառերով";
	}

	internal static string GetField_AgreementScheduleRow_Total(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Итого";
			}
			return "Total";
		}
		return "Ընդամենը";
	}

	internal static string GetLoanScheduledPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Очередное погашение кредита";
			}
			return "Loan Scheduled payment";
		}
		return "Վարկի հերթական մարում";
	}

	internal static string GetPrepaymentOfPrincipal(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Досрочное погашение основной суммы";
			}
			return "Prepayment of Principal";
		}
		return "Մայր գումարի վաղաժամկետ մարում";
	}

	internal static string GetField_MultiplePayment_Rows(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Получатели";
			}
			return "Beneficiaries";
		}
		return "Շահառուներ";
	}

	internal static string GetField_MultiplePayment_Amount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Общая сумма";
			}
			return "Total Amount";
		}
		return "Ընդհանուր գումար";
	}

	internal static string GetTaxPropertyTypeRealty(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Недвижимость";
			}
			return "Real estate";
		}
		return "Անշարժ գույք";
	}

	internal static string GetTaxPropertyTypeTransport(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Транспортное средство";
			}
			return "Vehicle";
		}
		return "Տրանսպորտային միջոց";
	}

	internal static string GetPropertyTaxDebtPeriodIncludingCurrentMonth(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "До текущего месяца (включительно)";
			}
			return "Up to current month (inclusive)";
		}
		return "Մինչև ընթացիկ ամիս (ներառյալ)";
	}

	internal static string GetPropertyTaxDebtPeriodIncludingCurrentYear(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "До текущего года (включительно)";
			}
			return "Up to current year (inclusive)";
		}
		return "Մինչև ընթացիկ տարի (ներառյալ)";
	}

	internal static string GetAdvancedPaymentCreditPaymentCombined(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплата основного долга, процентов, штрафов и пеней  кредита";
			}
			return "Loan Principal, Interest, Penalty Payment";
		}
		return "Վարկի մայր գումարի, տոկոսի, տույժի(տուգանքի) վճարում";
	}

	internal static string GetAdvancedPaymentCreditPaymentInterest(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплата процентов кредита";
			}
			return "Loan Interest Payment";
		}
		return "Վարկի տոկոսի վճարում";
	}

	internal static string GetAdvancedPaymentCreditPaymentPenalty(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплата штрафов и пеней кредита";
			}
			return "Loan Penalty Payment";
		}
		return "Վարկի տույժի, տուգանքի վճարում";
	}

	internal static string GetAdvancedPaymentCreditPaymentPrincipal(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплата основного долга кредита";
			}
			return "Loan Principal Payment";
		}
		return "Վարկի մայր գումարի վճարում";
	}

	internal static string GetAdvancedPaymentMethodNoAccount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Перевод без счета";
			}
			return "Non Account Transfer";
		}
		return "Անհաշիվ փոխանցում";
	}

	internal static string GetAdvancedPaymentMethodToAccount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Перевод на счет";
			}
			return "Transfer to Account";
		}
		return "Փոխանցում հաշվին";
	}

	internal static string GetAnonymousPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Анонимный";
			}
			return "Anonymous";
		}
		return "Գաղտնի";
	}

	internal static string GetPublicPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Публичный";
			}
			return "Public";
		}
		return "Հրապարակային";
	}

	internal static string GetField_GlobalCardType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Основной тип карты";
			}
			return "Card Main Type";
		}
		return "Քարտի հիմնական տիպ";
	}

	internal static string GetField_ThemeId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тема";
			}
			return "Theme";
		}
		return "Թեմա";
	}

	internal static string GetField_Image(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Изображение";
			}
			return "Image";
		}
		return "Նկար";
	}

	internal static string GetField_SmallImage(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Маленькое изображение";
			}
			return "Small Image";
		}
		return "Փոքր նկար";
	}

	internal static string GetField_CardDesign_ProfileId(string language)
	{
		return "Visa/Master Profile";
	}

	internal static string GetField_CardDesign_Design(string language)
	{
		return "Design";
	}

	internal static string GetField_Name(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название";
			}
			return "Name";
		}
		return "Անվանում";
	}

	internal static string GetField_CardDesignTheme_Title(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название";
			}
			return "Name";
		}
		return "Անվանում";
	}

	internal static string GetField_CardTypes(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Типы карт";
			}
			return "Card Types";
		}
		return "Քարտի տեսակներ";
	}

	internal static string GetField_IsDigital(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Цифровая";
			}
			return "Digital";
		}
		return "Թվային";
	}

	internal static string GetField_State(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Статус";
			}
			return "State";
		}
		return "Վիճակ";
	}

	internal static string GetBondIssuanceTypeNonInterestBearing(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Беспроцентный";
			}
			return "Non-interest bearing";
		}
		return "Տոկոսային եկամուտ չբերող";
	}

	internal static string GetBondIssuanceTypeInterestBearing(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "С получением процентов";
			}
			return "Interest bearing";
		}
		return "Տոկոսային եկամուտ բերող";
	}

	internal static string GetDocumentTypePurchaseBondApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Приобретение облигации";
			}
			return "Bond Purchase";
		}
		return "Պարտատոմսի ձեռքբերում";
	}

	internal static string GetEmail(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Эл. почта";
			}
			return "Email";
		}
		return "Էլ. փոստ";
	}

	internal static string GetPhoneNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Телефонный номер";
			}
			return "Phone Number";
		}
		return "Հեռախոսահամար";
	}

	internal static string GetAddress(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Адрес";
			}
			return "Address";
		}
		return "Հասցե";
	}

	internal static string GetStatementDeliveryMethod(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Метод доставки выписки";
			}
			return "Statement Delivery Method";
		}
		return "Քաղվացքի ստացման տարբերակ";
	}

	internal static string GetMail(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Почта";
			}
			return "Mail";
		}
		return "Փոստ";
	}

	internal static string GetManually(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Вручную";
			}
			return "Manually";
		}
		return "Առձեռն";
	}

	internal static string GetField_OpenAccountApplication_BranchCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Филиал";
			}
			return "Branch";
		}
		return "Մասնաճյուղ";
	}

	internal static string GetField_AccountStatement_Currency(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Валюта";
			}
			return "Currency";
		}
		return "Արժույթ";
	}

	internal static string GetField_AccountStatement_OpeningBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Начальный остаток на {0}";
			}
			return "Opening Balance on {0}";
		}
		return "Սկզբնական մնացորդ {0}-ով";
	}

	internal static string GetField_AccountStatement_Type(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип счета";
			}
			return "Account Type";
		}
		return "Հաշվի տիպ";
	}

	internal static string GetField_AccountStatement_Number(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет №";
			}
			return "Account No.";
		}
		return "Հաշիվ N";
	}

	internal static string GetField_AccountStatement_TaxCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "ИНН";
			}
			return "TIN";
		}
		return "ՀՎՀՀ";
	}

	internal static string GetField_AccountStatement_OperationDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата";
			}
			return "Date";
		}
		return "Ամսաթիվ";
	}

	internal static string GetField_AccountStatement_DocumentNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Док.№";
			}
			return "Doc.No.";
		}
		return "Փաստ.N";
	}

	internal static string GetField_AccountStatement_OperationType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип";
			}
			return "Type";
		}
		return "ԳՏ";
	}

	internal static string GetField_AccountStatement_CorrespondentAccount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет";
			}
			return "Account";
		}
		return "Հաշիվ";
	}

	internal static string GetField_AccountStatement_CorrespondentName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Плательщик/Получатель";
			}
			return "Remitter/Beneficiary";
		}
		return "Վճարող/Շահառու";
	}

	internal static string GetField_AccountStatement_CreditAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Кредит";
			}
			return "Credit";
		}
		return "Մուտք";
	}

	internal static string GetField_AccountStatement_DebitAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дебет";
			}
			return "Debit";
		}
		return "Ելք";
	}

	internal static string GetField_AccountStatement_CreditAmountAMD(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Кредит (AMD)";
			}
			return "Credit(AMD)";
		}
		return "Մուտք (AMD)";
	}

	internal static string GetField_AccountStatement_DebitAmountAMD(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дебет (AMD)";
			}
			return "Debit(AMD)";
		}
		return "Ելք (AMD)";
	}

	internal static string GetField_AccountStatement_Comment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Детали";
			}
			return "Details";
		}
		return "Նպատակ";
	}

	internal static string GetField_AccountStatement_DailyTurnover(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оборот на {0}";
			}
			return "Turnover on {0}";
		}
		return "Շարժ {0}-ով";
	}

	internal static string GetField_AccountStatement_DailyBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Остаток на {0}";
			}
			return "Balance on {0}";
		}
		return "Մնացորդ {0}-ով";
	}

	internal static string GetField_AccountStatement_Turnover(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оборот по счету";
			}
			return "Overall Turnover";
		}
		return "Ընդհանուր շարժ";
	}

	internal static string GetField_AccountStatement_ClosingBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Конечный остаток на {0}";
			}
			return "Closing Balance on {0}";
		}
		return "Վերջնական մնացորդ {0}-ով";
	}

	internal static string GetField_AccountStatement_AvailableBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Конечный доступный остаток";
			}
			return "Closing Available Balance";
		}
		return "Վերջնական հասանելի մնացորդ";
	}

	internal static string GetField_AccountStatement_ContractAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма овердрафта";
			}
			return "Overdraft Amount";
		}
		return "Օվերդրաֆտի գումար";
	}

	internal static string GetField_AccountStatement_UsedAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Использованная часть";
			}
			return "Used Amount";
		}
		return "Օգտագործված մաս";
	}

	internal static string GetField_AccountStatement_SigningDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата подписания";
			}
			return "Signing Date";
		}
		return "Կնքման ամսաթիվ";
	}

	internal static string GetField_AccountStatement_RepaymentDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата погашения";
			}
			return "Maturity Date";
		}
		return "Մարման վերջնաժամկետ";
	}

	internal static string GetField_AccountStatement_DebtAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма, которая должна быть погашена к {0}";
			}
			return "Total Amount to be paid by {0}";
		}
		return "Մինչև {0}-ը մարման ենթակա գումար";
	}

	internal static string GetField_AccountStatement_DebtInterestAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "включая начисленные проценты";
			}
			return "which includes accrued interest of";
		}
		return "որից հաշվարկված տոկոսագումար";
	}

	internal static string GetField_AccountStatement_Amount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма оверлимита";
			}
			return "Overlimit Amount";
		}
		return "Գերածախսի գումար";
	}

	internal static string GetField_AccountStatement_Fine(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Пеня за оверлимит";
			}
			return "Overlimit Fee";
		}
		return "Գերածախսի տույժ";
	}

	internal static string GetField_AccountStatement_TotalDebt(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Общая сумма к погашению";
			}
			return "Total amount to be paid";
		}
		return "Ընդհանուր մարման ենթակա գումար";
	}

	internal static string GetField_AccountStatement_AccountLimit(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Допустимый минимальный остаток";
			}
			return "Permissible Minimum of Balance";
		}
		return "Թույլատրելի նվազագույն մնացորդ";
	}

	internal static string GetField_AccountStatement_APR(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Процентная ставка";
			}
			return "Interest Rate";
		}
		return "Տարեկան տոկոսադրույք";
	}

	internal static string GetField_AccountStatement_Interest(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Годовая процентная доходность";
			}
			return "Annual % Yield";
		}
		return "Տարեկան տոկոսային եկամտաբերություն";
	}

	internal static string GetField_AccountStatement_InterestAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Выплаченные проценты";
			}
			return "Paid Interest";
		}
		return "Վճարված տոկոսագումար";
	}

	internal static string GetField_AccountStatement_FromBeginningOfYearInterestAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Выплаченные с начала года проценты";
			}
			return "Paid Interest from the Beginning of the Year";
		}
		return "Տարվա սկզբից վճարված տոկոսագումար";
	}

	internal static string GetField_AccountStatement_DayCount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Количество дней";
			}
			return "Days Count";
		}
		return "Օրերի քանակ";
	}

	internal static string GetField_AccountStatement_OverviewAccountStatement(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Выписка со счета";
			}
			return "Account Statement";
		}
		return "Հաշվի քաղվածք";
	}

	internal static string GetField_CardStatement_OverviewCardStatement(string language)
	{
		if (language == "hy-AM")
		{
			return "Քարտի քաղվածք";
		}
		return "Card Statement";
	}

	internal static string GetField_CardStatement_Number(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Карта №";
			}
			return "Card No.";
		}
		return "Քարտ N";
	}

	internal static string GetField_CardStatement_AccountNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет №";
			}
			return "Account No.";
		}
		return "Հաշիվ N";
	}

	internal static string GetField_CardStatement_Currency(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Валюта";
			}
			return "Currency";
		}
		return "Արժույթ";
	}

	internal static string GetField_CardStatement_TaxCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "ИНН";
			}
			return "TIN";
		}
		return "ՀՎՀՀ";
	}

	internal static string GetField_CardStatement_SubCards(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подкарты";
			}
			return "Sub-Cards";
		}
		return "Ենթաքարտեր";
	}

	internal static string GetField_CardStatement_OpeningBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Начальный остаток на {0}";
			}
			return "Opening Balance on {0}";
		}
		return "Սկզբնական մնացորդ {0}-ով";
	}

	internal static string GetField_CardStatement_PostingDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата проводки";
			}
			return "Posting Date";
		}
		return "Հաշվանցման ամսաթիվ";
	}

	internal static string GetField_CardStatement_TransactionDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата операции";
			}
			return "Transaction Date";
		}
		return "Գործառնության ամսաթիվ";
	}

	internal static string GetField_CardStatement_BillingAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплаченная сумма";
			}
			return "Billing Amount";
		}
		return "Հաշվարկված գումար";
	}

	internal static string GetField_CardStatement_TransactionAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма операции";
			}
			return "Transaction Amount";
		}
		return "Գործարքի գումար";
	}

	internal static string GetField_CardStatement_Description(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Описание транзакции";
			}
			return "Transaction Description";
		}
		return "Մեկնաբանություն";
	}

	internal static string GetField_CardStatement_InTransactions(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Кредиты";
			}
			return "Credits";
		}
		return "Մուտքեր";
	}

	internal static string GetField_CardStatement_InTurnover(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Итого кредитов";
			}
			return "Total Credits";
		}
		return "Ընդամենը մուտքեր";
	}

	internal static string GetField_CardStatement_OutTransactions(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дебеты";
			}
			return "Debits";
		}
		return "Ելքեր";
	}

	internal static string GetField_CardStatement_OutTurnover(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Итого дебетов";
			}
			return "Total Debits";
		}
		return "Ընդամենը ելքեր";
	}

	internal static string GetField_CardStatement_ClosingBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Конечный остаток на {0}";
			}
			return "Closing Balance on {0}";
		}
		return "Վերջնական մնացորդ {0}-ով";
	}

	internal static string GetField_CardStatement_AvailableBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Конечный доступный остаток";
			}
			return "Closing Available Balance";
		}
		return "Վերջնական հասանելի մնացորդ";
	}

	internal static string GetField_AgreementStatement_OverviewDepositStatement(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Выписка депозита";
			}
			return "Deposit statement";
		}
		return "Ավանդի քաղվածք";
	}

	internal static string GetField_AgreementStatement_ContractNo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Договор №";
			}
			return "Contract No.";
		}
		return "Պայմանագրի N";
	}

	internal static string GetField_AgreementStatement_Currency(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Валюта";
			}
			return "Currency";
		}
		return "Արժույթ";
	}

	internal static string GetField_AgreementStatement_OpeningBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Начальный остаток на {0}";
			}
			return "Opening Balance on {0}";
		}
		return "Սկզբնական մնացորդ {0}-ով";
	}

	internal static string GetField_AgreementStatement_Principal(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Остаток";
			}
			return "Principal";
		}
		return "Մնացորդ";
	}

	internal static string GetField_AgreementStatement_Interest(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Процент";
			}
			return "Interest";
		}
		return "Տոկոս";
	}

	internal static string GetField_AgreementStatement_CommitmentFee(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Процент неиспользованной части";
			}
			return "Unused Part Interest";
		}
		return "Չօգտագործված մասի տոկոս";
	}

	internal static string GetField_AgreementStatement_BankRateInterest(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Процент рефинансирования";
			}
			return "Refinancing Rate";
		}
		return "Վերաֆինանսավորման տոկոս";
	}

	internal static string GetField_AgreementStatement_Penalty(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Пеня с суммы";
			}
			return "Rate on Overdue Principal";
		}
		return "Գումարային տույժ";
	}

	internal static string GetField_AgreementStatement_Penalty2(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Пеня с процента";
			}
			return "Rate on Overdue Interest";
		}
		return "Տոկոսային տույժ";
	}

	internal static string GetField_AgreementStatement_Loss(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Процент убытка";
			}
			return "Expense Interest";
		}
		return "Վնասի տոկոս";
	}

	internal static string GetField_AgreementStatement_Transactions(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Транзакции";
			}
			return "Transactions";
		}
		return "Գործառնություններ";
	}

	internal static string GetField_AgreementStatement_OperationDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата";
			}
			return "Date";
		}
		return "Ամսաթիվ";
	}

	internal static string GetField_AgreementStatement_Amount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма";
			}
			return "Amount";
		}
		return "Գումար";
	}

	internal static string GetField_AgreementStatement_Description(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Детали";
			}
			return "Details";
		}
		return "Նպատակ";
	}

	internal static string GetField_AgreementStatement_Totals(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Итог";
			}
			return "Total";
		}
		return "Խմբավորված";
	}

	internal static string GetField_AgreementStatement_AgreementTotalDescription(string language)
	{
		if (language == "hy-AM")
		{
			return "Նպատակ";
		}
		return "Details";
	}

	internal static string GetField_AgreementStatement_ClosingBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Конечный остаток на {0}";
			}
			return "Closing Balance on {0}";
		}
		return "Վերջնական մնացորդ {0}-ով";
	}

	internal static string GetField_AgreementStatement_OverviewCreditStatement(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Выписка кредита";
			}
			return "Credit statement";
		}
		return "Վարկի քաղվածք";
	}

	internal static string GetField_MerchantTransaction_OverviewMerchantStatement(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Выписка торгового эквайринга";
			}
			return "Merchant Statement";
		}
		return "Սպասարկման կետի քաղվածք";
	}

	internal static string GetField_MerchantTransaction_OperationDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата проводки";
			}
			return "Posting Date";
		}
		return "Հաշվանցման ամսաթիվ";
	}

	internal static string GetField_MerchantTransaction_TransactionDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата сделки";
			}
			return "Transaction Date";
		}
		return "Գործարքի ամսաթիվ";
	}

	internal static string GetField_MerchantTransaction_TransactionDirection(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Направление сделки";
			}
			return "Transaction Direction";
		}
		return "Գործարքի տիպ";
	}

	internal static string GetField_MerchantTransaction_Currency(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Валюта транзакции";
			}
			return "Transaction Currency";
		}
		return "Գործարքի արժույթ";
	}

	internal static string GetField_MerchantTransaction_Amount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма транзакции";
			}
			return "Transaction Amount";
		}
		return "Գործարքի գումար";
	}

	internal static string GetField_MerchantTransaction_MerchantFee(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Комиссия";
			}
			return "Commission";
		}
		return "Միջնորդավճար";
	}

	internal static string GetField_MerchantTransaction_AmountWithoutMerchantFee(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма без комиссии";
			}
			return "Amount Without Commission";
		}
		return "Գումարն առանց միջնորդավճարի";
	}

	internal static string GetField_MerchantTransaction_MerchantName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Имя торгового эквайринга";
			}
			return "Merchant Name";
		}
		return "Սպասարկման կետի անվանում";
	}

	internal static string GetField_MerchantTransaction_CardNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер карты";
			}
			return "Card Number";
		}
		return "Քարտի համար";
	}

	internal static string GetField_MerchantTransaction_CardTypeName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип карты";
			}
			return "Card Type";
		}
		return "Քարտի տիպ";
	}

	internal static string GetField_MerchantTransaction_CardBelongingType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Эмитент карты";
			}
			return "Card Belonging Type";
		}
		return "Քարտի թողարկող";
	}

	internal static string GetField_MerchantTransaction_TransactionReferenceNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "RRN";
			}
			return "RRN";
		}
		return "RRN";
	}

	internal static string GetField_MerchantTransaction_TerminalId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Терминал №";
			}
			return "Terminal No.";
		}
		return "Տերմինալի N";
	}

	internal static string GetField_MerchantTransaction_AuthorizationCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Код авторизации";
			}
			return "Authorization Code";
		}
		return "Ավտորիզացիոն կոդ";
	}

	internal static string GetField_MerchantIds(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Торговые эквайринги";
			}
			return "Merchants";
		}
		return "Սպասարկման կետեր";
	}

	internal static string GetField_TerminalIds(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Терминалы";
			}
			return "Terminals";
		}
		return "Տերմինալներ";
	}

	internal static string GetField_Transactions(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Транзакции";
			}
			return "Transactions";
		}
		return "Գործարքներ";
	}

	internal static string GetReceiveTransactionInfoOff(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Выкл.";
			}
			return "Off";
		}
		return "Չստանալ";
	}

	internal static string GetReceiveTransactionInfoViaPushNotification(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Получать через Push уведомления";
			}
			return "Receive via Push notification";
		}
		return "Ստանալ Push տեղեկացումներով";
	}

	internal static string GetReceiveTransactionInfoViaSms(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Получать через SMS";
			}
			return "Receive via SMS";
		}
		return "Ստանալ SMS-ով";
	}

	internal static string GetSmsLanguageArmenian(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Армянский";
			}
			return "Armenian";
		}
		return "Հայատառ";
	}

	internal static string GetSmsLanguageArmenianLatinized(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Армянский, латинскими буквами";
			}
			return "Armenian Latinized";
		}
		return "Հայերեն լատինատառ";
	}

	internal static string GetSmsLanguageEnglish(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Английский";
			}
			return "English";
		}
		return "Անգլերեն";
	}

	internal static string GetSmsFormatWithBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "С балансом";
			}
			return "With balance";
		}
		return "Մնացորդով";
	}

	internal static string GetSmsFormatWithoutBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Без баланса";
			}
			return "Without balance";
		}
		return "Առանց մնացորդի";
	}

	internal static string GetField_AbonentName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Наименование";
			}
			return "Name";
		}
		return "Անվանում";
	}

	internal static string GetField_AbonentAddress(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Адрес";
			}
			return "Address";
		}
		return "Հասցե";
	}

	internal static string GetField_Debt(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Долг";
			}
			return "Debt to";
		}
		return "Պարտք առ";
	}

	internal static string GetField_Prepayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Предоплата";
			}
			return "Prepayment";
		}
		return "Կանխավճար";
	}

	internal static string GetField_AbonentCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Абонент";
			}
			return "Abonent";
		}
		return "Բաժանորդ";
	}

	internal static string GetField_LocationCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Регион";
			}
			return "Location";
		}
		return "Տարածք";
	}

	internal static string GetField_ComDetails(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Детали";
			}
			return "Details";
		}
		return "Մանրամասներ";
	}

	internal static string GetField_MemorialOrder_Base(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Основание";
			}
			return "Base";
		}
		return "Հիմք";
	}

	internal static string GetField_MemorialOrder_ReceiverAccount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Кредит";
			}
			return "Credit";
		}
		return "Կրեդիտ";
	}

	internal static string GetField_MemorialOrder_PayerAcc(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дебет";
			}
			return "Debit";
		}
		return "Դեբետ";
	}

	internal static string GetField_MemorialOrder_PayerAddress(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Адрес плательщика";
			}
			return "Payer Address";
		}
		return "Վճարողի հասցե";
	}

	internal static string GetField_MemorialOrder_PayerId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Док. удост. личность плательщика";
			}
			return "Payer Id";
		}
		return "Վճարողի անձը հաստատող փաստ.";
	}

	internal static string GetField_MemorialOrder_PayerName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Плательщик/Наименование счета";
			}
			return "Payer/Account Name";
		}
		return "Վճարող/Հաշվի անվանում";
	}

	internal static string GetField_MemorialOrder_ReceiverAddress(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Адрес получателя";
			}
			return "Receiver Address";
		}
		return "Ստացողի հասցե";
	}

	internal static string GetField_MemorialOrder_ReceiverId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Док. удост. личность получателя";
			}
			return "Receiver Id";
		}
		return "Ստացողի անձը հաստատող փաստ.";
	}

	internal static string GetField_MemorialOrder_ReceiverName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Получатель/Наименование счета";
			}
			return "Receiver/Account Name";
		}
		return "Ստացող/Հաշվի անվանում";
	}

	internal static string GetDocumentTypeMemorialOrder(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Мемориальный ордер";
			}
			return "Memorial Order";
		}
		return "Հիշարար օրդեր";
	}

	internal static string GetDocumentTypeDepositPartialReductionApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Частичное снятие депозита";
			}
			return "Deposit Partial Reduction";
		}
		return "Ավանդի մասնակի նվազեցում";
	}

	internal static string GetDocumentTypeDepositTerminationApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Расторжение договора";
			}
			return "Deposit Termination";
		}
		return "Ավանդի դադարեցում";
	}

	internal static string GetDocumentTypeSignableFileDocument(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подписываемый файл";
			}
			return "Signable File";
		}
		return "Ստորագրվող ֆայլ";
	}

	internal static string GetUserSignatureModeFingerprint(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отпечаток пальца";
			}
			return "Fingerprint";
		}
		return "Մատնահետք";
	}

	internal static string GetUserSignatureModePin(string language)
	{
		return "PIN";
	}

	internal static string GetUserSignatureModeFaceId(string language)
	{
		return "Face ID";
	}

	internal static string GetField_AccountNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет";
			}
			return "Account";
		}
		return "Հաշիվ";
	}

	internal static string GetSystemTypeCreditPlatform(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Кредитная платформа";
			}
			return "Credit platform";
		}
		return "Վարկային հարթակ";
	}

	internal static string GetSystemTypeCustomApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Собственная система";
			}
			return "Custom application";
		}
		return "Սեփական համակարգ";
	}

	internal static string GetDataUpdateTypeDisabled(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отключен";
			}
			return "Disabled";
		}
		return "Անջատված";
	}

	internal static string GetDataUpdateTypeRemind(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Напоминать";
			}
			return "Remind";
		}
		return "Հիշեցնել";
	}

	internal static string GetDataUpdateTypeRequired(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Обязательный";
			}
			return "Required";
		}
		return "Պարտադիր";
	}

	internal static string GetSignableFileStateNew(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Новый";
			}
			return "New";
		}
		return "Նոր";
	}

	internal static string GetSignableFileStateSentToClientForClient(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подписываемый";
			}
			return "Signable";
		}
		return "Ստորագրվող է";
	}

	internal static string GetSignableFileStateSentToClientForOperator(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отправлен клиенту";
			}
			return "Sent to Client";
		}
		return "Ուղարկված է հաճախորդին";
	}

	internal static string GetSignableFileStateSentToOperator(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отправлен для утверждения";
			}
			return "Sent for Approval";
		}
		return "Ուղարկված է հաստատման";
	}

	internal static string GetSignableFileStateApproved(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Утвержден";
			}
			return "Approved";
		}
		return "Հաստատված է";
	}

	internal static string GetSignableFileStateRejectedByOperator(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отклонен банком";
			}
			return "Rejected by Bank";
		}
		return "Մերժված է բանկի կողմից";
	}

	internal static string GetSignableFileStateRejectedByClient(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отклонен клиентом";
			}
			return "Rejected by Client";
		}
		return "Մերժված է հաճախորդի կողմից";
	}

	internal static string GetSignableFileStateExpired(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Истекший";
			}
			return "Expired";
		}
		return "Ժամկետանց";
	}

	internal static string GetSignableFileStateProblematic(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Проблемный";
			}
			return "Problematic";
		}
		return "Խնդրահարույց";
	}

	internal static string GetField_SignableFile_InitialFileName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Имя файла";
			}
			return "File Name";
		}
		return "Ֆայլի անվանում";
	}

	internal static string GetField_FileSignature_Identity(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подписавший";
			}
			return "Signer";
		}
		return "Ստորագրող";
	}

	internal static string GetField_FileSignature_Date(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата подписи";
			}
			return "Sign Date";
		}
		return "Ստորագրության ամսաթիվ";
	}

	internal static string GetField_FileSignature_SignatureIsValid(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подпись действительна";
			}
			return "Signature is Valid";
		}
		return "Ստորագրությունը վավեր է";
	}

	internal static string GetField_FileSignature_TimeStampIsValid(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Временная метка действительна";
			}
			return "Time Stamp is Valid";
		}
		return "Ժամանակի դրոշմը վավեր է";
	}

	internal static string GetField_FileSignature_CertificateIsValid(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сертификат верен";
			}
			return "Certificate is Valid";
		}
		return "Սերտիֆիկատը վավեր է";
	}

	internal static string GetField_SignableFile_InitialUploadDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата загрузки";
			}
			return "Upload Date";
		}
		return "Վերբեռնման ամսաթիվ";
	}

	internal static string GetField_SignableFile_SignedUploadDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата загрузки подписанного файла";
			}
			return "Signed File Upload Date";
		}
		return "Ստորագրված ֆայլի վերբեռնման ամսաթիվ";
	}

	internal static string GetField_SignableFile_InitialContent(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Изначальный файл";
			}
			return "Initial File";
		}
		return "Սկզբնական ֆայլ";
	}

	internal static string GetField_SignableFile_SignedContent(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подписанный файл";
			}
			return "Signed File";
		}
		return "Ստորագրված ֆայլ";
	}

	internal static string GetField_AmeriaSignableFileType_Description(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Описание";
			}
			return "Description";
		}
		return "Նկարագրություն";
	}

	internal static string GetField_AmeriaSignableFileType_QueryParams(string language)
	{
		return "Query Parameters";
	}

	internal static string GetField_SignableFile_TypeId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип файла";
			}
			return "File Type";
		}
		return "Ֆայլի տեսակ";
	}

	internal static string GetField_AmeriaSignableFileType_ClassId(string language)
	{
		return "Class ID";
	}

	internal static string GetBankQuestionAccount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Один из ваших банковских счетов";
			}
			return "One of your bank accounts";
		}
		return "Ձեր բանկային հաշիվներից որևէ մեկի համարը";
	}

	internal static string GetBankQuestionCreditAgreement(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер одного из ваших кредитных договоров";
			}
			return "One of your credit agreement numbers";
		}
		return "Ձեր վարկային պայմանագրերից որևէ մեկի համարը";
	}

	internal static string GetBankQuestionDepositAgreement(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер одного из ваших депозитных договоров";
			}
			return "One of your deposit agreement numbers";
		}
		return "Ձեր ավանդի պայմանագրերից որևէ մեկի համարը";
	}

	internal static string GetBankQuestionCard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер одного из ваших карт";
			}
			return "Оne of your card numbers";
		}
		return "Ձեր քարտերից որևէ մեկի համարը";
	}

	internal static string GetBankQuestionSocialCard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер социальной карты";
			}
			return "Social card number";
		}
		return "Սոցիալական քարտի համարը";
	}

	internal static string GetBankQuestionTaxCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "ИНН";
			}
			return "TIN";
		}
		return "ՀՎՀՀ";
	}

	internal static string GetUsernamePolicy(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Имя пользователя должно содержать не менее {0} символов, включая {1} цифры.";
			}
			return "The username should contain at least {0} character(s) including {1} numeric symbol(s).";
		}
		return "Մուտքանունը պետք է պարունակի առնվազն {0} նիշ, այդ թվում {1} թվային";
	}

	internal static string GetUsernamePolicySimple(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Имя пользователя должно содержать не менее {0} символов.";
			}
			return "The username should contain at least {0} character(s).";
		}
		return "Մուտքանունը պետք է պարունակի առնվազն {0} նիշ";
	}

	internal static string GetField_ClientCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Код клиента";
			}
			return "Client Code";
		}
		return "Հաճախորդի կոդ";
	}

	internal static string GetField_ClientName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Имя клиента";
			}
			return "Client Name";
		}
		return "Հաճախորդի անվանում";
	}

	internal static string GetField_SignableFile_OperatorName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Польз. банка";
			}
			return "Bank User";
		}
		return "Բանկի օգտ.";
	}

	internal static string GetInterestPayPeriodHalfYearly(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Полугодовой";
			}
			return "HalfYearly";
		}
		return "Կիսամյակային";
	}

	internal static string GetInterestPayPeriodMonthly(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Ежемесячно";
			}
			return "Monthly";
		}
		return "Ամսական";
	}

	internal static string GetInterestPayPeriodQuarterly(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Квартальный";
			}
			return "Quarterly";
		}
		return "Եռամսյակային";
	}

	internal static string GetInterestPayPeriodStartOfTerm(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "В начале срока";
			}
			return "At the start of the term";
		}
		return "Ժամկետի սկզբում";
	}

	internal static string GetInterestPayPeriodEndOfTerm(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "В конце срока";
			}
			return "At the end of the term";
		}
		return "Ժամկետի վերջում";
	}

	internal static string GetInterestPayPeriodYearly(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Ежегодный";
			}
			return "Yearly";
		}
		return "Տարեկան";
	}

	internal static string GetField_MultipleCommunal_Rows(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Услуги";
			}
			return "Utilities";
		}
		return "Ծառայություններ";
	}

	internal static string GetField_CreditRepaymentWithAttachedCard_RepaymentType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип погашения";
			}
			return "Repayment Type";
		}
		return "Մարման եղանակ";
	}

	internal static string GetField_CreditRepaymentWithAttachedCard_Receiver(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название договора";
			}
			return "Contract Name";
		}
		return "Պայմանագրի անվանում";
	}

	internal static string GetField_CreditRepaymentWithAttachedCard_IsFullRepayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Полное погашение";
			}
			return "Full Repayment";
		}
		return "Ամբողջական մարում";
	}

	internal static string GetField_CreditRepaymentWithAttachedCard_OverpaymentPrincipal(string language)
	{
		if (language == "hy-AM")
		{
			return "Մայր գումար";
		}
		return "Principal";
	}

	internal static string GetField_CreditRepaymentWithAttachedCard_CardNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Карта №";
			}
			return "Card No.";
		}
		return "Քարտ N";
	}

	internal static string GetField_ForeignPaymentRus_PayerAddress(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Адрес";
			}
			return "Address";
		}
		return "Հասցե";
	}

	internal static string GetField_ForeignPaymentRus_PayerCity(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Город";
			}
			return "City";
		}
		return "Քաղաք";
	}

	internal static string GetField_ForeignPaymentRus_PayerCountry(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Страна";
			}
			return "Country";
		}
		return "Երկիր";
	}

	internal static string GetField_ForeignPaymentRus_ReceiverAddress(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Адрес";
			}
			return "Address";
		}
		return "Հասցե";
	}

	internal static string GetField_ForeignPaymentRus_ReceiverCountry(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Страна";
			}
			return "Country";
		}
		return "Երկիր";
	}

	internal static string GetField_ForeignPaymentRus_ReceiverINN(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "ИНН получателя";
			}
			return "Beneficiary INN";
		}
		return "Շահառուի ԻՆՆ";
	}

	internal static string GetField_ForeignPaymentRus_ReceiverKPP(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "КПП получателя";
			}
			return "Beneficiary KPP";
		}
		return "Շահառուի ԿՊՊ";
	}

	internal static string GetField_ForeignPaymentRus_ReceiverBankBIC(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "БИК";
			}
			return "BIC";
		}
		return "ԲԻԿ";
	}

	internal static string GetField_ForeignPaymentRus_IntermediaryBankBIC(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "БИК";
			}
			return "BIC";
		}
		return "ԲԻԿ";
	}

	internal static string GetField_ForeignPaymentRus_AdditionalInfo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 1";
			}
			return "Line 1";
		}
		return "Տող 1";
	}

	internal static string GetField_ForeignPaymentRus_AdditionalInfo2(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 2";
			}
			return "Line 2";
		}
		return "Տող 2";
	}

	internal static string GetField_ForeignPaymentRus_AdditionalInfo3(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 3";
			}
			return "Line 3";
		}
		return "Տող 3";
	}

	internal static string GetField_ForeignPaymentRus_AdditionalInfo4(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 4";
			}
			return "Line 4";
		}
		return "Տող 4";
	}

	internal static string GetField_ForeignPaymentRus_AdditionalInfo5(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 5";
			}
			return "Line 5";
		}
		return "Տող 5";
	}

	internal static string GetField_ForeignPaymentRus_AdditionalInfo6(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Строка 6";
			}
			return "Line 6";
		}
		return "Տող 6";
	}

	internal static string GetDebitAccountPositiveBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Положительный остаток дебетового счета․";
			}
			return "Debit account positive balance․";
		}
		return "Դեբետ հաշվի դրական մնացորդ";
	}

	internal static string GetDebitAccountNegativeBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отрицательный остаток дебетового счета․";
			}
			return "Debit account negative balance․";
		}
		return "Դեբետ հաշվի բացասական մնացորդ";
	}

	internal static string GetCreditAccountPositiveBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Положительный остаток кредитного счета․";
			}
			return "Credit account positive balance․";
		}
		return "Կրեդիտ հաշվի դրական մնացորդ";
	}

	internal static string GetCreditAccountNegativeBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отрицательный остаток кредитного счета․";
			}
			return "Credit account negative balance․";
		}
		return "Կրեդիտ հաշվի բացասական մնացորդ";
	}

	internal static string GetMinimumOfPositiveBalances(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Минимальный из положительных остатков․";
			}
			return "Minimum of positive balances․";
		}
		return "Դրական մնացորդներից փոքրագույնը";
	}

	internal static string GetCreditTurnoverOfDebitAccount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Кредитовый оборот по дебетовому счету․";
			}
			return "Credit turnover of debit account․";
		}
		return "Դեբետ հաշվի կրեդիտային շարժ";
	}

	internal static string GetCreditTurnoverOfDebitAccountMinusDebitTurnover(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Кредитовый оборот по дебетовому счету минус дебетовый оборот․";
			}
			return "Credit turnover of debit account minus debit turnover․";
		}
		return "Դեբետ հաշվի կրեդիտային շարժ հանած դեբետային շարժ";
	}

	internal static string GetDebitTurnoverOfDebitAccount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дебетовый оборот по дебетовому счету․";
			}
			return "Debit turnover of debit account․";
		}
		return "Դեբետ հաշվի դեբետային շարժ";
	}

	internal static string GetDebitTurnoverOfDebitAccountMinusCreditTurnover(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дебетовый оборот по дебетовому счету минус кредитовый оборот.";
			}
			return "Debit turnover of debit account minus credit turnover.";
		}
		return "Դեբետ հաշվի դեբետային շարժ հանած կրեդիտային շարժ";
	}

	internal static string GetFixedAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Фиксированная сумма.";
			}
			return "Fixed amount";
		}
		return "Ֆիքսած գումար";
	}

	internal static string GetDebtAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма долга";
			}
			return "Debt amount";
		}
		return "Պարտքի գումար";
	}

	internal static string GetField_UtilityCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Услуга";
			}
			return "Utility";
		}
		return "Ծառայություն";
	}

	internal static string GetField_Comments(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Комментарии";
			}
			return "Comments";
		}
		return "Մեկնաբանություն";
	}

	internal static string GetField_FileName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Файл";
			}
			return "Attachment";
		}
		return "Կցված ֆայլ";
	}

	internal static string GetDocumentOperationNotSet(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Неизвестный";
			}
			return "Not Set";
		}
		return "Անորոշ է";
	}

	internal static string GetDocumentOperationCreated(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Создан";
			}
			return "Created";
		}
		return "Ստեղծվել է";
	}

	internal static string GetDocumentOperationEdited(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Редактирован";
			}
			return "Edited";
		}
		return "Խմբագրվել է";
	}

	internal static string GetDocumentOperationSigned(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подписан";
			}
			return "Signed";
		}
		return "Ստորագրվել է";
	}

	internal static string GetDocumentOperationSignedByFirst(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Поставлена подпись первого ранга";
			}
			return "Signed by First Rank Signatory";
		}
		return "Դրվել է առաջին կարգի ստորագրություն";
	}

	internal static string GetDocumentOperationSignedBySecond(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Поставлена подпись второго ранга";
			}
			return "Signed by Second Rank Signatory";
		}
		return "Դրվել է երկրորդ կարգի ստորագրություն";
	}

	internal static string GetDocumentOperationSignedByThird(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Поставлена подпись третьего ранга";
			}
			return "Signed by Third Rank Signatory";
		}
		return "Դրվել է երրորդ կարգի ստորագրություն";
	}

	internal static string GetDocumentOperationSent(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отправлен в банк";
			}
			return "Sent to Bank";
		}
		return "Ուղարկվել է բանկ";
	}

	internal static string GetDocumentOperationSentToEdit(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отправлен на редактирование";
			}
			return "Sent to edit";
		}
		return "Ուղարկվել է խմբագրման";
	}

	internal static string GetDocumentOperationConfirmed(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Исполнен банком";
			}
			return "Confirmed by Bank";
		}
		return "Կատարվել է բանկի կողմից";
	}

	internal static string GetDocumentOperationRefused(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отклонен банком";
			}
			return "Refused by Bank";
		}
		return "Մերժվել է բանկի կողմից";
	}

	internal static string GetDocumentOperationCanceled(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отменен";
			}
			return "Canceled";
		}
		return "Չեղարկվել է";
	}

	internal static string GetDocumentOperationConfirmedCommissionChange(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Одобрено изменение комиссии";
			}
			return "Confirmed commission change";
		}
		return "Հաստատվել է միջնորդավճարի փոփոխությունը";
	}

	internal static string GetDocumentOperationSignatureQueued(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Вынесен на групповую подпись";
			}
			return "Putted for group signature";
		}
		return "Դրվել է խմբային ստորագրության";
	}

	internal static string GetDocumentOperationSignatureFailed(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Не удалось подписать";
			}
			return "Signature failed";
		}
		return "Ստորագրությունը ձախողվել է";
	}

	internal static string GetDocumentOperationSignatureIgnored(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подпись проигнорирована";
			}
			return "Signature ignored";
		}
		return "Ստորագրությունը բաց է թողնվել";
	}

	internal static string GetField_CurrencyCredit(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Валюта кредит";
			}
			return "Credit Currency";
		}
		return "Կրեդիտ արժույթ";
	}

	internal static string GetField_AbonentID(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Идентификатор";
			}
			return "Identifier";
		}
		return "Որոշիչ";
	}

	internal static string GetField_AbonentType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип Абонента";
			}
			return "Abonent Type";
		}
		return "Բաժանորդի տեսակ";
	}

	internal static string GetField_AccountType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип счета";
			}
			return "Account type";
		}
		return "Հաշվի տեսակը";
	}

	internal static string GetField_Goal(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Цель";
			}
			return "Goal";
		}
		return "Նպատակ";
	}

	internal static string GetField_CardDesign(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дизайн карты";
			}
			return "Card Design";
		}
		return "Քարտի տեսք";
	}

	internal static string GetField_CardTypeId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип карты";
			}
			return "Card Type";
		}
		return "Քարտի տիպ";
	}

	internal static string GetField_CardDesignId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дизайн";
			}
			return "Design";
		}
		return "Տեսք";
	}

	internal static string GetTaxPropertyTypeGarbageCollection(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оплата за вывоз мусора";
			}
			return "Garbage Collection Payment";
		}
		return "Աղբահանության վճարում";
	}

	internal static string GetTaxPropertyTypeLand(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Земля";
			}
			return "Land";
		}
		return "Հող";
	}

	internal static string GetTaxPropertyTypeWaterTransport(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Водный транспорт";
			}
			return "Water Transport";
		}
		return "Ջրային տրանսպորտ";
	}

	internal static string GetField_RoadPolicePayment_ViolationTaskId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Внутренний номер штрафа";
			}
			return "Violation Task ID";
		}
		return "Ոստիկանության վարչական տուգանքի ներքին համար";
	}

	internal static string GetPeriodicityDays(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "{0} день(ей)";
			}
			return "{0} day(s)";
		}
		return "{0} օր";
	}

	internal static string GetPeriodicityMonths(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "{0} месяц(ев)";
			}
			return "{0} month(s)";
		}
		return "{0} ամիս";
	}

	internal static string GetPeriodicityMonthsDays(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "{0} месяц(ев) {1} день(ей)";
			}
			return "{0} month(s) {1} day(s)";
		}
		return "{0} ամիս {1} օր";
	}

	internal static string GetDocumentSignQueueRowStatusNew(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "В ожидании";
			}
			return "Pending";
		}
		return "Սպասվում է";
	}

	internal static string GetDocumentSignQueueRowStatusStarted(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "В ожидании";
			}
			return "Pending";
		}
		return "Սպասվում է";
	}

	internal static string GetDocumentSignQueueRowStatusSigned(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Подписан";
			}
			return "Signed";
		}
		return "Ստորագրվել է";
	}

	internal static string GetDocumentSignQueueRowStatusSignFailed(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Не удалось";
			}
			return "Failed";
		}
		return "Ձախողվել է";
	}

	internal static string GetDocumentSignQueueRowStatusIgnored(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Проигнорирован";
			}
			return "Ignored";
		}
		return "Բաց է թողնվել";
	}

	internal static string GetDocumentSignQueueStatusCreated(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "В ожидании";
			}
			return "Pending";
		}
		return "Սպասվում է";
	}

	internal static string GetDocumentSignQueueStatusStarted(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "в процессе";
			}
			return "In Process";
		}
		return "Մշակվում է";
	}

	internal static string GetDocumentSignQueueStatusSucceeded(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Завершен";
			}
			return "Completed";
		}
		return "Կատարված է";
	}

	internal static string GetDocumentSignQueueStatusFailed(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Не удалось";
			}
			return "Failed";
		}
		return "Ձախողված է";
	}

	internal static string GetField_OpenDepositApplication_IsDepositTermExtendable(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "С возможностью продления";
			}
			return "Extendable";
		}
		return "Երկարաձգվող";
	}

	internal static string GetField_OpenDepositApplication_IsDepositCapitalizable(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Капитализируемый";
			}
			return "Capitalizable";
		}
		return "Կապիտալացվող";
	}

	internal static string GetField_OpenDepositApplication_Date(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата открытия";
			}
			return "Opening Date";
		}
		return "Ներդրման ա/թ";
	}

	internal static string GetField_OpenDepositApplication_Duration(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Длительность вклада";
			}
			return "Deposit Term";
		}
		return "Ներդրման տևողություն";
	}

	internal static string GetField_OpenDepositApplication_DurationDays(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Длительность вклада (дн.)";
			}
			return "Deposit Term (days)";
		}
		return "Ներդրման տևողություն (օր)";
	}

	internal static string GetField_OpenDepositApplication_DurationMonths(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Длительность вклада (мес.)";
			}
			return "Deposit Term (months)";
		}
		return "Ներդրման տևողություն (ամիս)";
	}

	internal static string GetField_OpenDepositApplication_MaturityDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата погашения";
			}
			return "Maturity Date";
		}
		return "Մարման ա/թ";
	}

	internal static string GetField_DepositTemplateCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип депозита";
			}
			return "Deposit Type";
		}
		return "Ավանդատեսակ";
	}

	internal static string GetField_DepositTemplateName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название";
			}
			return "Name";
		}
		return "Անվանում";
	}

	internal static string GetField_FixedRate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Процентная ставка";
			}
			return "Interest Rate";
		}
		return "Տոկոսադրույք";
	}

	internal static string GetField_PercentAccount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет выплаты процентов";
			}
			return "Interest Payment Account";
		}
		return "Տոկոսագումարի ստացման հաշիվ";
	}

	internal static string GetField_TaxAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Баланс";
			}
			return "Balance";
		}
		return "Մնացորդ";
	}

	internal static string GetField_Info(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Информация";
			}
			return "Information";
		}
		return "Տեղեկություն";
	}

	internal static string GetField_LastUpdateDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата обновления";
			}
			return "Update Date";
		}
		return "Թարմացման ա/թ";
	}

	internal static string GetField_Year(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата";
			}
			return "Year";
		}
		return "Տարեթիվ";
	}

	internal static string GetField_TaxDebtId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Код налога на имущество";
			}
			return "Property Tax Code";
		}
		return "Գույքահարկի կոդ";
	}

	internal static string GetField_IsJoint(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Совместный";
			}
			return "Joint";
		}
		return "Համատեղ";
	}

	internal static string GetField_PropertyOwner(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Владелец";
			}
			return "Owner";
		}
		return "Սեփականատեր";
	}

	internal static string GetField_BankAccount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер счета получателя";
			}
			return "Receiver account number";
		}
		return "Ստացողի հաշվեհամար";
	}

	internal static string GetField_PropertyTaxPayment_Rows(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Список налогов на имущество";
			}
			return "Property Taxes List";
		}
		return "Գույքահարկերի ցուցակ";
	}

	internal static string GetField_RealEstateTaxPayment_Rows(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Список налогов на имущество";
			}
			return "Property Taxes List";
		}
		return "Գույքահարկերի ցուցակ";
	}

	internal static string GetField_VehicleTaxPayment_Rows(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Список налогов на имущество";
			}
			return "Property Taxes List";
		}
		return "Գույքահարկերի ցուցակ";
	}

	internal static string GetField_PropertyTaxDebtPeriod(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Период";
			}
			return "Period";
		}
		return "ժամանակահատված";
	}

	internal static string GetField_PropertyTaxOwnerLegalStatus(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Юридический статус";
			}
			return "Legal Status";
		}
		return "Իրավաբանական կարգավիճակ";
	}

	internal static string GetField_PropertyType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип имущества";
			}
			return "Property Type";
		}
		return "Գույքի տեսակ";
	}

	internal static string GetField_PropertySearchMethod(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Метод поиска собственности";
			}
			return "Property Search Method";
		}
		return "Գույքի փնտրման եղանակ";
	}

	internal static string GetField_VehicleLicensePlateNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Госномерной знак";
			}
			return "Plate Number";
		}
		return "Համարանիշ";
	}

	internal static string GetField_VehicleRegistrationCertificateNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Свидетельство о регистрации";
			}
			return "Registration Number";
		}
		return "Տեխնիկական անձնագիր";
	}

	internal static string GetField_PropertyTaxCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Код налога на имущество";
			}
			return "Property Tax Code";
		}
		return "Գույքահարկի կոդ";
	}

	internal static string GetField_Share(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Доля";
			}
			return "Share";
		}
		return "Մասնաբաժին";
	}

	internal static string GetField_Overpay(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Переплата";
			}
			return "Overpay";
		}
		return "Գերավճար";
	}

	internal static string GetField_TaxPaymentType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип";
			}
			return "Type";
		}
		return "Տեսակ";
	}

	internal static string GetTaxPaymentTypeDebt(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Долг";
			}
			return "Debt";
		}
		return "Պարտք";
	}

	internal static string GetTaxPaymentTypeFine(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Штраф";
			}
			return "Fine";
		}
		return "Տույժ";
	}

	internal static string GetField_IsDepositReplenishable(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Пополняемый";
			}
			return "Replenishable";
		}
		return "Համալրվող";
	}

	internal static string GetField_CompulsoryEnforcementPayment_IdentificationDocumentNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Документ, удостоверяющий личность";
			}
			return "Identification document";
		}
		return "Անձը հաստատող փաստաթուղթ";
	}

	internal static string GetField_CompulsoryEnforcementPayment_DebtorName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Должник";
			}
			return "Debtor";
		}
		return "Պարտապան";
	}

	internal static string GetField_CompulsoryEnforcementPayment_PlaintiffName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Истец";
			}
			return "Plaintiff";
		}
		return "Հայցվոր";
	}

	internal static string GetField_CompulsoryEnforcementPayment_RequestedAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Запрошенная сумма";
			}
			return "Requested Amount";
		}
		return "Հայցվող գումար";
	}

	internal static string GetField_CompulsoryEnforcementPayment_ChargeAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Штраф";
			}
			return "Charge Amount";
		}
		return "Տույժ";
	}

	internal static string GetField_ReceiverContactData(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер телефона получателя/эл. адрес";
			}
			return "Receiver Phone Number/ Email";
		}
		return "Ստացողի հեռախոս/Էլ. փոստ";
	}

	internal static string GetField_ContactTransfer_Receiver(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название получателя";
			}
			return "Receiver Name";
		}
		return "Ստացողի անվանում";
	}

	internal static string GetField_ContactTransfer_ReceiverBank(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Банк получателя";
			}
			return "Receiver Bank";
		}
		return "Ստացողի բանկ";
	}

	internal static string GetField_TaxPayment_Rows(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Список долгов";
			}
			return "Debts list";
		}
		return "Պարտքերի ցուցակ";
	}

	internal static string GetField_GarbageCollectionPayment_Rows(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Список долгов";
			}
			return "Debts list";
		}
		return "Պարտքերի ցուցակ";
	}

	internal static string GetField_TaxPropertyType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип";
			}
			return "Type";
		}
		return "Տեսակ";
	}

	internal static string GetField_TaxPropertyId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Идентификационный номер недвижимости";
			}
			return "Property Identification Number";
		}
		return "Գույքի հաշվառման համար";
	}

	internal static string GetField_TaxPayerName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название";
			}
			return "Name";
		}
		return "Անվանում";
	}

	internal static string GetField_TaxPayerAddress(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Адрес";
			}
			return "Address";
		}
		return "Հասցե";
	}

	internal static string GetField_TaxPayment_Amount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Общая сумма";
			}
			return "Total amount";
		}
		return "Ընդհանուր գումար";
	}

	internal static string GetField_GarbageCollectionPayment_Amount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Общая сумма";
			}
			return "Total amount";
		}
		return "Ընդհանուր գումար";
	}

	internal static string GetField_CashOutByPhone_ReceiverMobileNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Мобильный номер получателя";
			}
			return "Receiver Phone Number";
		}
		return "Ստացողի հեռախոսահամար";
	}

	internal static string GetField_CashOutByPhone_PayerAcc(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет";
			}
			return "Account";
		}
		return "Հաշիվ";
	}

	internal static string GetField_CashOutByPhone_Amount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма вывода наличных";
			}
			return "Withdrawal Amount";
		}
		return "Կանխիկացվող գումար";
	}

	internal static string GetField_CashOutByPhone_WithdrawalCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Код выдачи";
			}
			return "Withdrawal Code";
		}
		return "Կանխիկացման կոդ";
	}

	internal static string GetField_CashOutByPhone_ExpirationDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Активно до";
			}
			return "Active Until";
		}
		return "Ակտիվ է մինչև";
	}

	internal static string GetField_PurchaseBondApplication_PayerName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название плательщика";
			}
			return "Payer Name";
		}
		return "Վճարողի անվանում";
	}

	internal static string GetField_PurchaseBondApplication_PayerBank(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Банк плательщика";
			}
			return "Payer Bank";
		}
		return "Վճարողի բանկ";
	}

	internal static string GetField_PurchaseBondApplication_IssuanceNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер выпуска";
			}
			return "Issuance Number";
		}
		return "Թողարկման համար";
	}

	internal static string GetField_PurchaseBondApplication_BondsCount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Количество";
			}
			return "Quantity";
		}
		return "Քանակ";
	}

	internal static string GetField_MerchantTransaction_TokenPan(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер Apple Pay/Google Pay";
			}
			return "Apple Pay/Google Pay Number";
		}
		return "Apple Pay/Google Pay համար";
	}

	internal static string GetField_ClosingBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Конечный остаток на {0}";
			}
			return "Closing Balance on {0}";
		}
		return "Վերջնական մնացորդ {0}-ով";
	}

	internal static string GetField_ClosingBalanceDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Конец периода";
			}
			return "End of Period";
		}
		return "Ժամանակահատվածի վերջ";
	}

	internal static string GetField_OpeningBalance(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Начальный остаток на {0}";
			}
			return "Opening Balance on {0}";
		}
		return "Սկզբնական մնացորդ {0}-ով";
	}

	internal static string GetField_OpeningBalanceDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Начала периода";
			}
			return "Start of Period";
		}
		return "Ժամանակահատվածի սկիզբ";
	}

	internal static string GetField_AccountStatement_CreditTurnover(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оборот по кредиту";
			}
			return "Credit Turnover";
		}
		return "Ընդհանուր շարժ կրեդիտով";
	}

	internal static string GetField_AccountStatement_CreditTurnoverAMD(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оборот по кредиту (AMD)";
			}
			return "Credit Turnover (AMD)";
		}
		return "Ընդհանուր շարժ կրեդիտով (AMD)";
	}

	internal static string GetField_AccountStatement_DebitTurnover(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оборот по дебету";
			}
			return "Debit Turnover";
		}
		return "Ընդհանուր շարժ դեբետով";
	}

	internal static string GetField_AccountStatement_DebitTurnoverAMD(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Оборот по дебету (AMD)";
			}
			return "Debit Turnover (AMD)";
		}
		return "Ընդհանուր շարժ դեբետով (AMD)";
	}

	internal static string GetField_AccountStatement_OpeningBalanceAMD(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Начальный остаток на {0} (AMD)";
			}
			return "Opening Balance on {0} (AMD)";
		}
		return "Սկզբնական մնացորդ {0}-ով (AMD)";
	}

	internal static string GetField_CashWithdrawalApplication_WithdrawalDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата обналичивания";
			}
			return "Withdrawal Date";
		}
		return "Կանխիկացման ամսաթիվ";
	}

	internal static string GetField_CashWithdrawalApplication_BranchName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Филиал обналичивания";
			}
			return "Withdrawal Branch";
		}
		return "Կանխիկացման մասնաճյուղ";
	}

	internal static string GetField_CashWithdrawalApplication_IsReceiverAnotherClient(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Получатель другой клиент";
			}
			return "Receiver is another client";
		}
		return "Ստացողն այլ անձ է";
	}

	internal static string GetField_CashWithdrawalApplication_Receiver(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Имя";
			}
			return "Name";
		}
		return "Անուն";
	}

	internal static string GetField_CashWithdrawalApplication_ReceiverLastName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Фамилия";
			}
			return "Surname";
		}
		return "Ազգանուն";
	}

	internal static string GetField_CashWithdrawalApplication_ReceiverPassportNum(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер идентификационного документа";
			}
			return "Passport/ID Card Number";
		}
		return "Անձնագիր/Նույնականացման քարտ";
	}

	internal static string GetField_CashWithdrawalApplication_ReceiverPassportAuthority(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Орган, выдавший документ";
			}
			return "Authority";
		}
		return "Ում կողմից";
	}

	internal static string GetField_CashWithdrawalApplication_ReceiverIdentificationDocumentType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип";
			}
			return "Type";
		}
		return "Տիպ";
	}

	internal static string GetField_CashWithdrawalApplication_ReceiverPassportDateOfIssue(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата выдачи";
			}
			return "Date of Issue";
		}
		return "Տրվել է";
	}

	internal static string GetField_CashWithdrawalApplication_ReceiverPassportExpirationDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата окончания срока";
			}
			return "Date of Expiry";
		}
		return "Վավեր է մինչև";
	}

	internal static string GetField_ForeignPayment_TransferAdditionalInfo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дополнительная информация о переводе";
			}
			return "Transfer Additional Information";
		}
		return "Փոխանցման լրացուցիչ ինֆորմացիա";
	}

	internal static string GetField_ForeignPayment_ReceivingAmount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сумма в валюте получения";
			}
			return "Amount in Receiving Currency";
		}
		return "Գումարը ստացման արժույթով";
	}

	internal static string GetField_ForeignPayment_ReceivingCurrency(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Валюта получения";
			}
			return "Receiving Currency";
		}
		return "Ստացման արժույթ";
	}

	internal static string GetField_DocumentName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название документа";
			}
			return "Document Name";
		}
		return "Փաստաթղթի անվանում";
	}

	internal static string GetSecurityProviderRefreshTypeWithOtp(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "С ОТП";
			}
			return "With otp";
		}
		return "Անվտանգության սարքի կոդով";
	}

	internal static string GetSecurityProviderRefreshTypeWithBiometric(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "С биометрической проверкой";
			}
			return "With biometric verification";
		}
		return "Կենսաչափական ստուգմամբ";
	}

	internal static string GetField_ThirdPartyPassportOrTaxCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Паспорт/ИНН";
			}
			return "Passport/TIN";
		}
		return "Անձնագիր/ՀՎՀՀ";
	}

	internal static string GetField_VehicleUseTypeId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип использования";
			}
			return "Usage Type";
		}
		return "Շահագործման եղանակ";
	}

	internal static string GetField_InsuranceStartDate(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Дата начала контракта";
			}
			return "Contract Start Date";
		}
		return "Պայմանագրի սկիզբ";
	}

	internal static string GetField_InsuranceDuration(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Срок действия контракта";
			}
			return "Contract Duration";
		}
		return "Պայմանագրի տևողություն";
	}

	internal static string GetField_AswaContractMobile(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер телефона";
			}
			return "Phone Number";
		}
		return "Հեռախոսահամար";
	}

	internal static string GetField_AswaContractEmail(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Эл. почта";
			}
			return "Email";
		}
		return "Էլ. փոստ";
	}

	internal static string GetField_AswaContractDefaultAccount(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Счет возмещения";
			}
			return "Compensation Account";
		}
		return "Փոխհատուցման հաշվեհամար";
	}

	internal static string GetField_InsuranceCompanyId(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Страховая компания";
			}
			return "Insurance Company";
		}
		return "Ապահովագրական ընկերություն";
	}

	internal static string GetField_AmeriaSignableFileType_OperatorRoles(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Роли оператора";
			}
			return "Operator Roles";
		}
		return "Օպերատորի դերեր";
	}

	internal static string GetWorkerInstanceStatusRunning(string language)
	{
		return "The instance is currently running and active.";
	}

	internal static string GetWorkerInstanceStatusStoppedUnexpectedly(string language)
	{
		return "The instance is assumed to have stopped unexpectedly. This status is determined by a monitoring worker if no new iteration has started after a long delay.";
	}

	internal static string GetWorkerInstanceStatusStoppedGracefully(string language)
	{
		return "The instance was stopped gracefully with a StopDate recorded.";
	}

	internal static string GetWorkerInstanceStatusUnknown(string language)
	{
		return "The instance was found without a StopDate during startup, which means it was not properly shut down and is now marked as unknown.";
	}

	internal static string GetDocumentTypeCreditLineDisbursementApplication(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Предоставление кредитной линии";
			}
			return "Provision of a credit line";
		}
		return "Վարկային գծի տրամադրում";
	}

	internal static string GetField_AdvancedLocalPayment_CreditPaymentType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип оплаты кредита";
			}
			return "Credit Payment Type";
		}
		return "Վարկի վճարման տեսակ";
	}

	internal static string GetField_AdvancedLocalPayment_AdditionalInfo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сообщение (строка 1)";
			}
			return "Message (line 1)";
		}
		return "Հաղորդագրություն (տող 1)";
	}

	internal static string GetField_AdvancedLocalPayment_AdditionalInfo2(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сообщение (строка 2)";
			}
			return "Message (line 2)";
		}
		return "Հաղորդագրություն (տող 2)";
	}

	internal static string GetField_AdvancedLocalPayment_AdditionalInfo3(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сообщение (строка 3)";
			}
			return "Message (line 3)";
		}
		return "Հաղորդագրություն (տող 3)";
	}

	internal static string GetField_AdvancedLocalPayment_AdditionalInfo4(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сообщение (строка 4)";
			}
			return "Message (line 4)";
		}
		return "Հաղորդագրություն (տող 4)";
	}

	internal static string GetField_AdvancedLocalPayment_AdditionalInfo5(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сообщение (строка 5)";
			}
			return "Message (line 5)";
		}
		return "Հաղորդագրություն (տող 5)";
	}

	internal static string GetField_AdvancedLocalPayment_AdditionalInfo6(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сообщение (строка 6)";
			}
			return "Message (line 6)";
		}
		return "Հաղորդագրություն (տող 6)";
	}

	internal static string GetField_AdvancedLocalPayment_CreditContractOwner(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Имя заемщика";
			}
			return "Creditor name";
		}
		return "Վարկառուի անվանում";
	}

	internal static string GetField_AdvancedLocalPayment_CreditCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Код кредита";
			}
			return "Credit code";
		}
		return "Վարկի կոդ";
	}

	internal static string GetField_AdvancedLocalPayment_PaymentMethod(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Способ платежа";
			}
			return "Payment method";
		}
		return "Վճարման եղանակ";
	}

	internal static string GetField_AdvancedLocalPayment_ReceiverLegalState(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Юридический статус";
			}
			return "Legal status";
		}
		return "Իրավաբանական կարգավիճակ";
	}

	internal static string GetField_AdvancedLocalPayment_ReceiverTaxCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "ИНН";
			}
			return "TIN";
		}
		return "ՀՎՀՀ";
	}

	internal static string GetField_AdvancedLocalPayment_ReceiverSocialCard(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Социальная карта №";
			}
			return "Social Card No.";
		}
		return "Սոց. քարտի N/ՀԾՀ";
	}

	internal static string GetField_AdvancedLocalPayment_ReceiverPassportType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип паспорта";
			}
			return "Passport Type";
		}
		return "Անձնագրի տեսակ";
	}

	internal static string GetField_AdvancedLocalPayment_ReceiverPassportNo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Паспортные данные";
			}
			return "Passport Data";
		}
		return "Անձնագրի տվյալներ";
	}

	internal static string GetField_AdvancedLocalPayment_ReceiverPassportNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Паспортные данные";
			}
			return "Beneficiary Passport No.";
		}
		return "Անձնագրի տվյալներ";
	}

	internal static string GetField_AdvancedLocalPayment_ReceiverAuthorizedPerson(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Уполномоченное лицо";
			}
			return "Authorized Person";
		}
		return "Լիազորված անձ";
	}

	internal static string GetField_AdvancedLocalPayment_PayerAuthorizedPerson(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Уполномоченное лицо";
			}
			return "Authorized Person";
		}
		return "Լիազորված անձ";
	}

	internal static string GetField_AdvancedLocalPayment_ReceiverBankName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Банк получатель";
			}
			return "Receiver bank";
		}
		return "Ստացող բանկ";
	}

	internal static string GetField_AdvancedLocalPayment_PayerAdditionalInfo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Прочие данные";
			}
			return "Other Information";
		}
		return "Լրացուցիչ տվյալներ";
	}

	internal static string GetField_AdvancedLocalPayment_ReceiverAdditionalInfo(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Прочие данные";
			}
			return "Other Information";
		}
		return "Լրացուցիչ տվյալներ";
	}

	internal static string GetDocumentTypeQrPayment(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "QR-платёж";
			}
			return "QR Payment";
		}
		return "QR վճարում";
	}

	internal static string GetRegistrationTypeNewClient(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Новый клиент";
			}
			return "New Client";
		}
		return "Նոր հաճախորդ";
	}

	internal static string GetRegistrationTypeExistingClient(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Существующий клиент";
			}
			return "Existing Client";
		}
		return "Հաճախորդ";
	}

	internal static string GetCreditOverpaymentTypeUpcomingAmounts(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Ближайшие суммы";
			}
			return "Upcoming amounts";
		}
		return "Առաջիկա գումարները";
	}

	internal static string GetCreditOverpaymentTypeLastAmounts(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Последние суммы";
			}
			return "Last amounts";
		}
		return "Վերջին գումարները";
	}

	internal static string GetCreditOverpaymentTypeUpcomingAmountThenLast(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сначала ближайшая сумма, затем последняя";
			}
			return "Upcoming amount, then last";
		}
		return "Առաջիկա գումարը, հետո վերջինը";
	}

	internal static string GetCreditOverpaymentTypeWithScheduleRedistribution(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "С перераспределением графика";
			}
			return "With schedule redistribution";
		}
		return "Գրաֆիկի վերաբաշխումով";
	}

	internal static string GetCreditOverpaymentTypeUpcomingAmountsDescription(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Погашение будет направлено на уплату основной суммы по ближайшим датам погашения в порядке, предусмотренном графиком";
			}
			return "The repayment will be directed toward the principal amounts of the upcoming repayment dates, according to the schedule order";
		}
		return "Մարումը կուղղվի առաջիկա մարման ամսաթվերի մայր գումարների վճարմանը՝ ըստ գրաֆիկի հերթականության";
	}

	internal static string GetCreditOverpaymentTypeLastAmountsDescription(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Погашение будет осуществлено в обратном порядке, начиная с основной суммы, предусмотренной по последней дате погашения";
			}
			return "The repayment will be applied in reverse order, starting from the principal amount scheduled for the last repayment date";
		}
		return "Մարումը կկատարվի հակառակ ուղղությամբ՝ սկսած վերջին մարման ամսաթվին նախատեսված մայր գումարից";
	}

	internal static string GetCreditOverpaymentTypeUpcomingAmountThenLastDescription(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Сначала будет погашена основная сумма, подлежащая уплате по ближайшей дате погашения, затем в обратном порядке, начиная с основной суммы по последней дате погашения";
			}
			return "First, the principal amount due on the upcoming repayment date will be paid, then in reverse order starting from the principal amount scheduled for the last repayment date";
		}
		return "Նախ կմարվի առաջիկա վճարման ամսաթվին վճարվելիք մայր գումարը, ապա հակառակ հերթականությամբ՝ սկսած վերջին մարման ամսաթվին նախատեսված մայր գումարից";
	}

	internal static string GetCreditOverpaymentTypeWithScheduleRedistributionDescription(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "В случае досрочного погашения будет выполнен пересмотр всего графика на основе нового остатка основной суммы";
			}
			return "In the case of early repayment, the entire schedule will be reviewed based on the new remaining principal amount";
		}
		return "Վաղաժամկետ մարման դեպքում կկատարվի ամբողջ գրաֆիկի վերանայում՝ ըստ մայր գումարի նոր մնացորդի";
	}

	internal static string GetLinkUrl(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Адрес (URL)";
			}
			return "Address (URL)";
		}
		return "Հասցե (URL)";
	}

	internal static string GetLinkDocumentType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Документ";
			}
			return "Document";
		}
		return "Փաստաթուղթ";
	}

	internal static string GetLinkModule(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Страница";
			}
			return "Page";
		}
		return "Էջ";
	}

	internal static string GetField_QrPayment_MerchantName(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Получатель";
			}
			return "Receiver";
		}
		return "Ստացող";
	}

	internal static string GetField_QrPayment_ExternalCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер транзакции (RRN)";
			}
			return "Transaction Number (RRN)";
		}
		return "Գործարքի համար (RRN)";
	}

	internal static string GetField_OverpaymentType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Вид досрочного погашения основного долга по кредиту.";
			}
			return "Type of Early Repayment of Loan Principal.";
		}
		return "Վարկի մայր գումարի վաղաժամկետ մարման տեսակ";
	}

	internal static string GetField_FriendCreditRepayment_PayerBank(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Банк плательщика";
			}
			return "Remitter Bank";
		}
		return "Վճարողի բանկ";
	}

	internal static string GetField_FriendCreditRepayment_FriendCreditSearchType(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тип поиска";
			}
			return "Search Type";
		}
		return "Որոնման տեսակ";
	}

	internal static string GetField_FriendCreditRepayment_ContractNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Договор №";
			}
			return "Contract No.";
		}
		return "Պայմանագրի N";
	}

	internal static string GetField_FriendCreditRepayment_ContractCreditCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Кредитный код";
			}
			return "Credit Code";
		}
		return "Վարկային կոդ";
	}

	internal static string GetField_FriendCreditRepayment_PassportNumber(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер паспорта";
			}
			return "Passport No.";
		}
		return "Անձնագրի համար";
	}

	internal static string GetField_FriendCreditRepayment_ContractDetails(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Информация";
			}
			return "Information";
		}
		return "Տեղեկատվություն";
	}

	internal static string GetField_FriendCreditRepayment_BankPartner(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Партнер банка";
			}
			return "Bank Partner";
		}
		return "Բանկի գործընկեր";
	}

	internal static string GetField_ArCaPayTransfer_ReceiverContactData(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Номер телефона получателя";
			}
			return "Receiver Phone Number";
		}
		return "Ստացողի հեռախոսահամար";
	}

	internal static string GetField_ArCaPayTransfer_Receiver(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Название получателя";
			}
			return "Receiver Name";
		}
		return "Ստացողի անվանում";
	}

	internal static string GetField_ArCaPayTransfer_ReceiverBankArCaCode(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Банк получателя";
			}
			return "Receiver Bank";
		}
		return "Ստացողի բանկ";
	}

	internal static string GetField_AgreementStatement_OverviewBondStatement(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Выписка по облигации";
			}
			return "Bond Statement";
		}
		return "Պարտատոմսի քաղվածք";
	}

	internal static string GetServiceFeeRateDivisorCategoryDescription_Daily(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Ежедневная процентная ставка обслуживания.";
			}
			return "Daily service fee rate.";
		}
		return "Օրական սպասարկման տոկոսադրույք";
	}

	internal static string GetServiceFeeRateDivisorCategoryDescription_Weekly(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Еженедельная процентная ставка обслуживания.";
			}
			return "Weekly service fee rate.";
		}
		return "Շաբաթական սպասարկման տոկոսադրույք";
	}

	internal static string GetServiceFeeRateDivisorCategoryDescription_Monthly(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Ежемесячная процентная ставка обслуживания.";
			}
			return "Monthly service fee rate.";
		}
		return "Ամսական սպասարկման տոկոսադրույք";
	}

	internal static string GetServiceFeeRateDivisorCategoryDescription_Yearly(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Ежегодная процентная ставка обслуживания.";
			}
			return "Yearly service fee rate.";
		}
		return "Տարեկան սպասարկման տոկոսադրույք";
	}

	internal static string GetField_CompulsoryEnforcementPayment_Receiver(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отделение";
			}
			return "Department";
		}
		return "Բաժանմունք";
	}

	internal static string GetField_Message_Subject(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Тема";
			}
			return "Subject";
		}
		return "Թեմա";
	}

	internal static string GetField_Message_Body(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Содержание";
			}
			return "Body";
		}
		return "Պարունակություն";
	}

	internal static string GetMerchantCategoryCodeEatingPlacesAndRestaurants(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Ресторан";
			}
			return "Restaurant";
		}
		return "Ռեստորան";
	}

	internal static string GetMerchantCategoryCodeGroceryStoresAndSupermarkets(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Супермаркет";
			}
			return "Supermarket";
		}
		return "Սուպերմարկետ";
	}

	internal static string GetMerchantCategoryCodeLodgingHotelsMotelsResorts(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отель";
			}
			return "Hotel";
		}
		return "Հյուրանոց";
	}

	internal static string GetMerchantCategoryCodeFastFoodRestaurants(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Кафе / Фастфуд";
			}
			return "Café / Fast Food";
		}
		return "Սրճարան / Արագ սնունդ";
	}

	internal static string GetMerchantCategoryCodeMiscellaneousFoodStores(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Продуктовый магазин";
			}
			return "Food Store";
		}
		return "Սննդի խանութ";
	}

	internal static string GetMerchantCategoryCodeFamilyClothingStores(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Магазин одежды";
			}
			return "Clothing Store";
		}
		return "Հագուստի խանութ";
	}

	internal static string GetMerchantCategoryCodeTravelAgenciesAndTourOperators(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Туристическое агентство";
			}
			return "Travel Agency";
		}
		return "Տուրիստական գործակալություն";
	}

	internal static string GetMerchantCategoryCodeLumberAndBuildingMaterialsStores(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Магазин стройматериалов";
			}
			return "Building Materials Store";
		}
		return "Շինանյութի խանութ";
	}

	internal static string GetMerchantCategoryCodeMiscellaneousGeneralMerchandise(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Различные универсальные магазины";
			}
			return "General Store";
		}
		return "Խառը ապրանքների խանութ";
	}

	internal static string GetMerchantCategoryCodeMensAndWomensClothingStores(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Магазин одежды";
			}
			return "Clothing Store";
		}
		return "Հագուստի խանութ";
	}

	internal static string GetMerchantCategoryCodeJewelryStores(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Ювелирный магазин";
			}
			return "Jewelry Store";
		}
		return "Զարդերի խանութ";
	}

	internal static string GetMerchantCategoryCodeBusinessServicesNotElsewhereClassified(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Услуга";
			}
			return "Service";
		}
		return "Գործարար ծառայություններ";
	}

	internal static string GetMerchantCategoryCodeAirlines(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Авиакомпании";
			}
			return "Airlines";
		}
		return "Ավիաընկերություններ";
	}

	internal static string GetMerchantCategoryCodeCarRental(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Аренда автомобилей";
			}
			return "Car Rental";
		}
		return "Մեքենաների վարձույթ";
	}

	internal static string GetMerchantCategoryCodeTaxisAndLimousines(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Такси и лимузины";
			}
			return "Taxis and Limousines";
		}
		return "Տաքսի և լիմուզիններ";
	}

	internal static string GetMerchantCategoryCodeAutomatedFuelDispensers(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Автоматические топливные заправки";
			}
			return "Fuel Dispensers";
		}
		return "Վառելիքի կայաններ";
	}

	internal static string GetMerchantCategoryCodeServiceStations(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Станции обслуживания";
			}
			return "Service Stations";
		}
		return "Սպասարկման կայաններ";
	}

	internal static string GetMerchantCategoryCodeDoctors(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Врачи и медицинские услуги";
			}
			return "Doctors and Medical Services";
		}
		return "Բժիշկներ և բժշկական ծառայություններ";
	}

	internal static string GetMerchantCategoryCodeDentists(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Стоматологи и ортодонты";
			}
			return "Dentists and Orthodontists";
		}
		return "Ատամնաբույժներ և օրթոդոնտներ";
	}

	internal static string GetMerchantCategoryCodeHospitals(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Больницы";
			}
			return "Hospitals";
		}
		return "Հիվանդանոցներ";
	}

	internal static string GetMerchantCategoryCodeFoodStore(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Продуктовый магазин";
			}
			return "Food Store";
		}
		return "Սննդի խանութ";
	}

	internal static string GetMerchantCategoryCodeClothingStore(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Магазин одежды";
			}
			return "Clothing Store";
		}
		return "Հագուստի խանութ";
	}

	internal static string GetMerchantCategoryCodeTravelAgency(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Туристическое агентство";
			}
			return "Travel Agency";
		}
		return "Տուրիստական գործակալություն";
	}

	internal static string GetMerchantCategoryCodeBuildingMaterialsStore(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Магазин стройматериалов";
			}
			return "Building Materials Store";
		}
		return "Շինանյութի խանութ";
	}

	internal static string GetMerchantCategoryCodeGeneralStore(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Универсальный магазин";
			}
			return "General Store";
		}
		return "Խառը ապրանքների խանութ";
	}

	internal static string GetMerchantCategoryCodeApparelAndAccessoriesStore(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Одежда и аксессуары";
			}
			return "Apparel & Accessories Store";
		}
		return "Հագուստ և աքսեսուարներ";
	}

	internal static string GetMerchantCategoryCodeJewelryStore(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Ювелирный магазин";
			}
			return "Jewelry Store";
		}
		return "Զարդերի խանութ";
	}

	internal static string GetMerchantCategoryCodeService(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Услуга";
			}
			return "Service";
		}
		return "Գործարար ծառայություններ";
	}

	internal static string GetMerchantCategoryCodeRestaurant(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Ресторан";
			}
			return "Restaurant";
		}
		return "Ռեստորան";
	}

	internal static string GetMerchantCategoryCodeSupermarket(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Супермаркет";
			}
			return "Supermarket";
		}
		return "Սուպերմարկետ";
	}

	internal static string GetMerchantCategoryCodeHotel(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Отель";
			}
			return "Hotel";
		}
		return "Հյուրանոց";
	}

	internal static string GetMerchantCategoryCodeCafeFastFood(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Кафе / Фастфуд";
			}
			return "Café / Fast Food";
		}
		return "Սրճարան / Արագ սնունդ";
	}

	internal static string GetMerchantCategoryCodeOther(string language)
	{
		if (!(language == "hy-AM"))
		{
			if (language == "ru-RU")
			{
				return "Другое";
			}
			return "Other";
		}
		return "Այլ";
	}

	internal static string Get(string resourceName, string language = null)
	{
		if (string.IsNullOrEmpty(language))
		{
			language = Language;
		}
		MethodInfo method = typeof(Resources).GetMethod("Get" + resourceName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		if (method == null)
		{
			return string.Empty;
		}
		return (string)method.Invoke(null, new object[1] { language });
	}
}
