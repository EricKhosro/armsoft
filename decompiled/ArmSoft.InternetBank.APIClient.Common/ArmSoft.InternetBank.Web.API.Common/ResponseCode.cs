namespace ArmSoft.InternetBank.Web.API.Common;

public class ResponseCode
{
	public const string DocumentConfirmed = "document_confirmed";

	public const string DocumentRefused = "document_refused";

	public const string DocumentInProgress = "document_in_progress";

	public const string CommissionChanged = "commission_changed";

	public const string DocumentCanceled = "document_canceled";

	public const string DocumentGroupSigned = "document_group_signed";

	public const string DocumentMustBeSignedByClient = "document_must_be_signed_by_client";

	public const string ViolationNotFoundByVehicleDetails = "violation_not_found_by_vehicle_details";

	public const string ViolationNotFoundByDecreeNumber = "violation_not_found_by_decree_number";

	public const string CommissionNotApplicable = "commission_not_applicable";

	public const string ArCaP2PPhoneNumberRequired = "arca_p2p_phone_number_required";

	public const string ArCaP2PInvalidPhoneFormat = "arca_p2p_invalid_phone_format";

	public const string ArCaP2PNoDefaultBank = "arca_p2p_no_dafault_bank";

	public const string ArCaP2PRequestFailed = "arca_p2p_request_failed";

	public const string ArCaP2PNotFoundRecipientByGivenPhoneNumber = "arca_p2p_not_found_recipient_by_given_phone_number";

	public const string ArCaP2PSenderAndReceiverBanksCanNotBeSame = "arca_p2p_sender_and_receiver_bank_can_not_be_same";

	public const string ArCaP2PMobileAlreadyRegistered = "arca_p2p_mobile_already_registered";

	public const string ArCaP2PNewMobileIsAlreadyRegistered = "arca_p2p_new_mobile_already_registered";

	public const string ArCaP2PMobileAlreadyRegisteredByAnotherBank = "arca_p2p_mobile_already_registered_by_another_bank";

	public const string ArCaP2PExpiredOtp = "arca_p2p_expired_otp";

	public const string ArCaP2PInvalidOtp = "arca_p2p_invalid_otp";

	public const string ArCaP2PUsedOtp = "arca_p2p_used_otp";

	public const string ArCaP2PNotMemberBank = "arca_p2p_not_member_bank";

	public const string ArCaP2PAccountNotFound = "arca_p2p_account_not_found";

	public const string VehicleNotFound = "vehicle_not_found";

	public const string VehicleNotFoundForCurrentClient = "vehicle_not_found_for_current_client";

	public const string BonusMalusNotFound = "bonus_malus_not_found";

	public const string VehicleUseTypesNotFound = "vehicle_use_types_not_found";

	public const string InsuranceCompaniesNotFound = "insurance_companies_not_found";

	public const string InsuranceCompanyOffersNotFound = "insurance_company_offers_not_found";

	public const string InvalidPhoneNumberFormat = "invalid_phone_number_format";

	public const string InvalidEmailFormat = "invalid_email_format";

	public const string InvalidInsuranceDuration = "invalid_insurance_duration";

	public const string UpdateContractInfoFailed = "update_contract_failed";

	public const string InvalidDefaultAccount = "invalid_default_account";

	public const string InvalidVehicleUseType = "invalid_vehicle_use_type";

	public const string AswaFailed = "aswa_failed";

	public const string OperationAlreadyConfirmed = "operation_already_confirmed";

	public const string OperationAlreadyRejected = "operation_already_rejected";

	public const string OperationProcessingFailed = "operationProcessingFailed";

	public const string OperationConfirmed = "operation_confirmed";

	public const string OperationRejected = "operation_rejected";

	public const string InvalidClientCode = "invalid_client_code";

	public const string DepositTemplateNotFound = "deposit_template_not_found";
}
