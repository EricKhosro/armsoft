namespace ArmSoft.OpenBanking.Api.Common;

public static class ResponseCode
{
	public const string UnsupportedVersion = "unsupported_version";

	public const string Unauthorized = "unauthorized";

	public const string NotFound = "not_found";

	public const string NotSupported = "not_supported";

	public const string ActionNotSupported = "action_not_supported";

	public const string ConcurrencyViolation = "concurrency_violation";

	public const string DuplicateKey = "duplicate_key";

	public const string ValidationFailed = "validation_failed";

	public const string InternalServerError = "internal_server_error";

	public const string OperationNotAllowed = "operation_not_allowed";

	public const string AttractionNotAllowed = "attraction_not_allowed";

	public const string InvalidClientCredentials = "invalid_client_credentials";

	public const string UnexpectedException = "unexpected_exception";

	public const string AccountNotFound = "account_not_found";

	public const string AccountNotAvailable = "account_not_available";

	public const string CompulsoryEnforcementNotAvailable = "compulsory_enforcement_not_available";

	public const string InvalidAccessKey = "invalid_access_key";

	public const string AuthenticationSucceeded = "authentication_succeeded";

	public const string ChangePasswordRequired = "change_password_required";

	public const string InvalidUsernameOrPassword = "invalid_username_or_password";

	public const string InvalidSecureAuthenticationCredentials = "invalid_secure_authentication";

	public const string InvalidSignature = "invalid_signature";

	public const string RepeatedSignature = "repeated_signature";

	public const string ExpiredSignature = "expired_signature";

	public const string UserInvalidVerificationData = "user_invalid_verification_data";

	public const string VerificationRequired = "verification_required";

	public const string InvalidUserDataForSecurityProviderActivation = "invalid_user_data_for_security_provider_activation";

	public const string SecureAuthenticationRequired = "secure_authentication_required";

	public const string UnsupportedSystemType = "unsupported_system_type";

	public const string ClientDataMissingForDocumentCreate = "client_data_missing_for_document_create";

	public const string ViolationNotFound = "violation_not_found";

	public const string CurrentRateNotAvailable = "current_rate_not_available";

	public const string CreditDebtNotFound = "credit_debt_not_found";

	public const string StatementPeriodLimitViolation = "statement_period_limit_violation";

	public const string AssociatedUserNotFound = "associated_user_not_found";

	public const string AssociatedUserMustChangePassword = "associated_user_password_change_required";

	public const string AssociatedUserMustActivateSecurityProvider = "associated_user_security_provider_activation_required";

	public const string AssociatedUserIsLockedOut = "associated_user_locked_out";

	public const string InvalidUserInformation = "invalid_user_information";

	public const string InvalidClientInformation = "invalid_client_information";

	public const string BiometricIdentificationFailed = "biometric_identification_failed";

	public const string LoginFromNewDeviceNotAllowed = "login_from_new_device_not_allowed";

	public const string ClientHasUser = "client_has_user";

	public const string KYCFailed = "kyc_failed";

	public const string InvalidVerificationCode = "invalid_verification_code";

	public const string InvalidVerificationContact = "invalid_verification_contact";

	public const string ExpiredVerificationCode = "expired_verification_code";

	public const string InvalidSessionStateForLoginCode = "invalid_session_state_for_login_code";

	public const string LoginCodeExpired = "login_code_expired";

	public const string MaxVerificationAttemptsReached = "max_verification_attempts_reached";

	public const string InsufficientVerificationData = "insufficient_verification_data";

	public const string InformationForResetPasswordInvalid = "information_for_reset_password_invalid";

	public const string InformationForRecoverUsernameInvalid = "information_for_recover_username_invalid";

	public const string InvalidUsername = "invalid_username";

	public const string InvalidPassword = "invalid_password";

	public const string PasswordChanged = "password_changed";

	public const string CurrentPasswordInvalid = "current_password_invalid";

	public const string NewPasswordMustDiffer = "new_password_must_differ";

	public const string PasswordTooShort = "password_too_short";

	public const string PasswordComplexityFailed = "password_complexity_failed";

	public const string StatementPeriodLimitExceeded = "statement_period_limit_exceeded";

	public const string CardLimitViolation = "card_limit_violation";

	public const string CardSettingsReceivingOptionsAreRequired = "card_settings_receiving_options_are_required";

	public const string CardSettingsMobileIsRequired = "card_settings_mobile_is_required";

	public const string CardSettingsLanguageIsRequired = "card_settings_language_is_required";

	public const string CardSettingsEmailIsRequired = "card_settings_email_is_required";

	public const string CardNotFound = "card_not_found";

	public const string ArCaP2PRequestFailed = "arca_p2p_request_failed";

	public const string ArCaP2PInvalidPhoneFormat = "arca_p2p_invalid_phone_format";

	public const string ArCaP2PPhoneNumberRequired = "arca_p2p_phone_number_required";

	public const string ArCaP2PMobileAlreadyRegistered = "arca_p2p_mobile_already_registered";

	public const string ArCaP2PNewMobileIsAlreadyRegistered = "arca_p2p_new_mobile_is_already_registered";

	public const string ArCaP2PMobileAlreadyRegisteredByAnotherBank = "arca_p2p_mobile_already_registered_by_another_bank";

	public const string ArCaP2PExpiredOtp = "arca_p2p_expired_otp";

	public const string ArCaP2PInvalidOtp = "arca_p2p_invalid_otp";

	public const string ArCaP2PUsedOtp = "arca_p2p_used_otp";

	public const string ArCaP2PNotMemberBank = "arca_p2p_not_member_bank";

	public const string ArCaP2PAccountNotFound = "arca_p2p_account_not_found";

	public const string ArCaPaySenderAndReceiverBanksCanNotBeSame = "arca_pay_sender_and_receiver_bank_can_not_be_same";

	public const string CommissionNotApplicable = "commission_not_applicable";

	public const string DocumentConfirmed = "document_confirmed";

	public const string DocumentRefused = "document_refused";

	public const string DocumentInProgress = "document_in_progress";

	public const string DocumentPartiallySigned = "document_partially_signed";

	public const string PrintableFormTitleEmpty = "titles_cannot_be_empty";

	public const string PrintableFormNameEmpty = "printable_form_name_empty";

	public const string PrintableFormTitleMismatch = "printable_form_title_mismatch";

	public const string UserEditRequestExists = "user_edit_request_exists";

	public const string InvalidAuthenticatorKey = "invalid_authenticator_key";

	public const string InvalidDateRange = "invalid_date_range";

	public const string NotSupportedPaymentType = "not_supported_payment_type";

	public const string AbonentNotFound = "abonent_not_found";

	public const string InvalidFileFormat = "invalid_file_format";

	public const string MaxSizeExceeded = "max_size_exceeded";

	public const string DepositTemplateNotFound = "deposit_template_not_found";

	public const string ImportedDocumentsCountExceeded = "imported_documents_count_exceeded";

	public const string PropertyTaxDebtsNotFound = "property_tax_debts_not_found";

	public const string ReceiverInformationNotFound = "receiver_information_not_found";

	public const string TaxPropertyNotFound = "tax_property_not_found";

	public const string PropertyNotFound = "property_not_found";

	public const string MapPointNotFound = "map_point_not_found";

	public const string CashOutByPhoneExpirationTimeAlreadyExtended = "cash_out_by_phone_expiration_time_already_extended";

	public const string CashOutByPhoneSmsAlreadySent = "cash_out_by_phone_sms_already_sent";

	public const string IssuedBondNotFound = "issued_bond_not_found";

	public const string UserDeletionFailed = "user_deletion_failed";

	public const string LimitViolation = "limit_violation";

	public const string DuplicateDocumentNumber = "duplicate_document_number";

	public const string ClientFinancialDeclarationPeriodLimitViolation = "client_financial_declaration__period_limit_violation";

	public const string RequestExists = "request_exists";

	public const string VisaB2BValidationFailed = "visa_b2b_validation_failed";

	public const string VisaB2BAmountRecalculationFailed = "visa_b2b_amount_recalculation_failed";

	public const string OperationAlreadyConfirmed = "operation_already_confirmed";

	public const string OperationAlreadyRejected = "operation_already_rejected";

	public const string OperationProcessingFailed = "operation_processing_failed";

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

	public const string CreditContractDetailsNotFound = "credit_contract_details_not_found";

	public const string PassportNumberNotFound = "passport_number_not_found";

	public const string CreditInfoNotFound = "credit_info_not_found";

	public const string CreditContractNumberNotFound = "credit_contract_number_not_found";

	public const string InvalidIban = "invalid_iban";

	public const string InvalidParamId = "invalid_param_id";

	public const string PhotoNotModified = "photo-not-modified";

	public const string NoUserPhoto = "no-photo";

	public const string MustAcceptTerms = "must_accept_terms";

	public const string UnprocessedApplication = "unprocessed_application";

	public const string EmptyAmountCheckText = "empty_amount_check_text";

	public const string InvalidIpAddress = "invalid_ip_address";

	public const string UserLockedOut = "user_locked_out";

	public const string UserInactive = "user_inactive";

	public const string SessionRevoked = "session_revoked";

	public const string InvalidRefreshToken = "invalid_refresh_token";
}
