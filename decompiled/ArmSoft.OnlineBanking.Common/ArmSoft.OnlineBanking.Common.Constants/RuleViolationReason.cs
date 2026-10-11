namespace ArmSoft.OnlineBanking.Common.Constants;

public static class RuleViolationReason
{
	public const string Required = "required";

	public const string MustBeEmpty = "must_be_empty";

	public const string ReadOnly = "read_only";

	public const string MustBeNumeric = "must_be_numeric";

	public const string NotFound = "not_found";

	public const string ValueNotAllowed = "value_not_allowed";

	public const string ValueChanged = "value_changed";

	public const string ValueNotSupported = "value_not_supported";

	public const string MaxLength = "max_length";

	public const string FixedLength = "fixed_length";

	public const string MinLength = "min_length";

	public const string MinValue = "min_value";

	public const string MaxValue = "max_value";

	public const string AdditionalInfoExceededCount = "additional_info_exceeded_count";

	public const string MinAlphaNumericCharactersCount = "min_alpha_numeric_characters_count";

	public const string FormatRegexMismatch = "format_regex_mismatch";

	public const string InvalidFieldValue = "invalid_field_value";

	public const string InvalidFieldName = "invalid_field_name";

	public const string InvalidFieldType = "invalid_field_type";

	public const string InvalidFieldFormat = "invalid_field_format";

	public const string InvalidStringSymbol = "invalid_string_symbol";

	public const string InvalidFilterOptions = "invalid_filter_options";

	public const string InvalidPaymentType = "invalid_payment_type";

	public const string TransferBetweenSameAccountNotAllowed = "transfer_between_same_account_not_allowed";

	public const string InvalidLegalState = "invalid_legal_state";

	public const string InvalidSignaturesCount = "invalid_signatures_count";

	public const string SecondSignatureNotUsed = "second_signature_not_used";

	public const string ThirdSignatureNotUsed = "third_signature_not_used";

	public const string DefaultValueRequired = "default_value_required";

	public const string DuplicateRow = "duplicate_row";

	public const string DuplicateValue = "duplicate_value";

	public const string DuplicateControl = "duplicate_control";

	public const string TransferNotAllowed = "transfer_not_allowed";

	public const string UrgentTransferIsNotAllowed = "urgent_transfer_not_allowed";

	public const string InvalidUrgentTransferTime = "invalid_urgent_transfer_time";

	public const string NoAccess = "no_access";

	public const string ObjectAssociatedFieldMismatch = "object_associated_field_mismatch";

	public const string NotAllFieldsSelected = "not_all_fields_selected";

	public const string DirectoryValueNotExist = "directory_value_not_exist";

	public const string InsufficientAccountBalance = "insufficient_account_balance";

	public const string InsufficientAccountBalanceForNonProcessedDocuments = "insufficient_account_balance_for_non_processed_documents";

	public const string AttachmentNotAllowed = "attachment_not_allowed";

	public const string CommissionChanged = "commission_changed";

	public const string UnsupportedFileType = "unsupported_file_type";

	public const string FileSizeExceeded = "file_size_exceeded";

	public const string AmountIsNotRounded = "amount_is_not_rounded";

	public const string ValueMustBeInteger = "value_must_be_integer";

	public const string DepositAttractionFromCardAccountsNotAllowed = "deposit_attraction_from_card_accounts_not_allowed";

	public const string CurrencyMismatch = "currency_mismatch";

	public const string CurrencyMustBeAMD = "currency_must_be_amd";

	public const string CurrenciesMustBeDifferent = "currencies_must_be_different";

	public const string CurrencyNotAllowed = "currency_not_allowed";

	public const string NotReplenishable = "not_replenishable";

	public const string CustomizationCommandFieldDoesNotExist = "customization_command_field_does_not_exist";

	public const string CustomizationCommandTypeMismatch = "customization_command_type_mismatch";

	public const string CustomizationCommandNotSupportedForFieldValueType = "customization_command_not_supported_for_field_value_type";

	public const string CustomizationCommandNotSupportedForField = "customization_command_not_supported_for_field";

	public const string CustomizationCommandExpressionNotAllowedInCondition = "customization_command_expression_not_allowed_in_condition";

	public const string CustomizationCommandFieldDoesNotHaveCurrency = "customization_command_field_does_not_have_currency";

	public const string CustomizationConditionOperandMustBeString = "customization_condition_operand_must_be_string";

	public const string CustomizationCommandOperandsMustBeNumeric = "customization_command_operands_must_be_numeric";

	public const string CustomizationCommandOperandsMustBeString = "customization_command_operands_must_be_string";

	public const string CustomizationCommandExpressionInvalidSubstringOperandTypes = "customization_command_expression_invalid_substring_operand_types";

	public const string CustomizationCommandFirstOperandMustBeDate = "customization_command_first_operand_must_be_date";

	public const string CustomizationCommandSecondOperandMustBeInt = "customization_command_second_operand_must_be_int";

	public const string UserDoesNotHaveCurrentSecurityProvider = "user_does_not_have_current_security_provider";

	public const string SecurityProviderTransitionIsNotAllowed = "security_provider_transition_is_not_allowed";

	public const string SecurityProviderNotInAllowedSecurityProviders = "security_provider_not_in_allowed_security_providers";

	public const string UserSecurityProviderMustHaveAttachStateAndEmptyTokenSerial = "user_security_provider_must_have_attach_state_and_empty_token_serial";

	public const string UserMustHaveCurrentSecurityProvider = "user_must_have_current_security_provider";

	public const string UserSecurityProviderRemovingIsNotAllowed = "user_security_provider_removing_not_allowed";

	public const string VerificationOptionRequired = "verification_option_required";

	public const string NotApprovedUserExists = "not_approved_user_exists";

	public const string WindowsAuthSecurityParamsMustBeEmpty = "windows_auth_security_params_must_be_empty";

	public const string ServiceAgreementUsed = "service_agreement_used";

	public const string VersionMinValue = "version_min_value";

	public const string TaxAuthorityCodeIsNotActive = "tax_authority_code_is_not_active";

	public const string ClientDataRequired = "client_data_required";

	public const string LimitViolation = "limit_violation";

	public const string ViolationMismatch = "violation_mismatch";

	public const string ViolationHasInquest = "violation_has_inquest";

	public const string OnlineValidationFailed = "online_validation_failed";

	public const string PayeeClientMismatch = "payee_client_mismatch";

	public const string InvalidStateTransition = "invalid_state_transition";

	public const string TransferToOtherProcessingCardsNotAllowed = "transfer_to_other_processing_cards_not_allowed";

	public const string CardSettingsMobileIsRequired = "card_settings_mobile_is_required";

	public const string CardSettingsLanguageIsRequired = "card_settings_language_is_required";

	public const string CardSettingsEmailIsRequired = "card_settings_email_is_required";

	public const string CardSettingsReceivingOptionsAreRequired = "card_settings_receiving_options_are_required";

	public const string ClientHasManyUsers = "client_has_many_users";

	public const string ClientHasNoUser = "client_has_no_user";

	public const string UserHasAssociatedUsers = "user_has_associated_users";

	public const string ClientMustBeInactive = "client_must_be_inactive";

	public const string ClientIsAlreadyDeactivated = "client_is_already_deactivated";

	public const string MustUseBudgetaryPayment = "must_use_budgetary_payment";

	public const string MustUseSoldierPayment = "must_use_soldier_payment";

	public const string FieldsMustNotBeEqual = "fields_must_not_be_equal";

	public const string InvalidRatePrecision = "invalid_rate_precision";

	public const string TransferBetweenAccountsWithDifferentCurrenciesNotAllowed = "transfer_between_accounts_with_different_currencies_not_allowed";

	public const string AccountsCurrenciesMismatch = "accounts_currencies_mismatch";

	public const string RateNotAvailable = "rate_not_available";

	public const string RateChanged = "rate_changed";

	public const string InvalidCurrencyConversionAmounts = "invalid_currency_conversion_amounts";

	public const string RepetitionError = "repetition_error";

	public const string AscendingError = "ascending_error";

	public const string ForeignPaymentPayerDetailsError = "foreign_payment_payer_details_error";

	public const string ChargeTypeMustBeBen = "charge_type_must_be_ben";

	public const string InvalidIbanCountryCode = "invalid_iban_country_code";

	public const string InvalidIban = "invalid_iban";

	public const string CanBeSetOnlyForLegalClients = "foreign_payment_commission_change_error";

	public const string ChargeAccountCurrencyRequiredAMD = "charge_account_currency_required_amd";

	public const string ChargeAccountCurrencyDesiredAMD = "charge_account_currency_desired_amd";

	public const string RepayingCreditFromCardAccountsNotAllowed = "repaying_credit_from_card_accounts_not_allowed";

	public const string ThereAreNonProcessedCancelRequests = "there_are_non_processed_cancel_requests";

	public const string IncorrectTotalAmount = "incorrect_total_amount";

	public const string MultiplePaymentDifferentBanks = "multiple_payment_different_banks";

	public const string CollectionMaxCountExceeded = "collection_max_count_exceeded";

	public const string NonDigitalCardDesignCount = "non_digital_card_design_count";

	public const string GlobalCardTypeMismatch = "global_card_type_mismatch";

	public const string UtilityNotAvailable = "utility_not_available";

	public const string UtilityMinAmount = "utility_min_amount";

	public const string UtilityMaxAmount = "utility_max_amount";

	public const string UtilitySpecialSearchFieldsMismatch = "utility_special_serach_fields_mismatch";

	public const string UtilityDebtChanged = "utility_debt_changed";

	public const string UtilityAmountNotEditable = "utility_amount_not_editable";

	public const string DepositCannotBeReduced = "deposit_cannot_be_reduced";

	public const string DepositCannotBeTerminated = "deposit_cannot_be_terminated";

	public const string MaxValueExceeded = "max_value_exceeded";

	public const string AccountBalanceMustBeZero = "account_balance_must_be_zero";

	public const string InvalidCardType = "invalid_card_type";

	public const string CountLimit = "count_limit";

	public const string FileSignaturesMismatch = "file_signatures_mismatch";

	public const string NoNewSignaturesOnFile = "no_new_signatures";

	public const string FileContentMismatch = "file_content_mismatch";

	public const string MailRequired = "mail_required";

	public const string PhoneNumberUpdateNotAllowed = "phone_number_update_not_allowed";

	public const string EmailUpdateNotAllowed = "email_update_not_allowed";

	public const string InvalidPersonData = "invalid_person_data";

	public const string MismatchADriveFile = "file_adrive_mismatch";

	public const string SignableFileIncorrectUrl = "adrive_url_incorrect";

	public const string ADriveUrlRequired = "adrive_url_required";

	public const string OpenDepositFromCardAccountsNotAllowed = "open_deposit_from_card_accounts_not_allowed";

	public const string ArcaP2PFromCardAccountsNotAllowed = "arca_p2p_transfer_from_card_accounts_not_allowed";

	public const string AmountIsNotDecimalMultiple = "amount_is_not_decimal_multiple";

	public const string CheckDividend = "check_dividend";

	public const string MobileCountryCodeMustStartWithPlus = "mobile_country_code_must_start_with_plus";

	public const string MobileCountryCodeMustContainOnePlus = "mobile_country_code_must_contain_one_plus";

	public const string PurchaseBondFromCardAccountsNotAllowed = "purchase_bond_from_card_accounts_not_allowed";

	public const string BondInterestPaymentToCardAccountsNotAllowed = "bond_interest_payment_to_card_accounts_not_allowed";

	public const string CompulsoryEnforcementNotAvailable = "compulsory_enforcement_not_available";

	public const string PaymentNotAllowed = "payment_not_allowed";

	public const string NonProcessedPayments = "non_processed_payments";

	public const string ContactTransferNotAllowed = "transfer_cant_be_performed";

	public const string PropertyTaxPaymentReceiverAccountsCantBeDifferent = "property_tax_payment_receiver_accounts_cant_be_different";

	public const string NonProcessedApplications = "non_processed_applications";

	public const string NotNull = "not_null";

	public const string BranchIsClosed = "branch_is_closed";

	public const string WithdrawalDateLessDocumentDate = "withdrawal_date_less_document_date";

	public const string CardTransactionsRestricted = "card_transactions_restricted";

	public const string InvalidForeignPaymentAmounts = "invalid_foreign_payment_amounts";

	public const string TransferViaVisaB2BNotAllowed = "transfer_via_visa_b2b_not_allowed";

	public const string InsuranceStartDateLessSuggestedStartDate = "insurance_start_date_less_than_suggested";

	public const string InvalidInsuranceDuration = "invalid_insurance_duration";

	public const string InvalidInsuranceCompanyId = "invalid_insurance_company_id";

	public const string PayerAdditionalInfoExceededCount = "payer_additional_info_exceeded_count";

	public const string ReceiverAdditionalInfoExceededCount = "receiver_additional_info_exceeded_count";

	public const string InvalidBankCode = "invalid_bank_code";

	public const string CannotRepayContract = "cannot_repay_contract";

	public const string ContractDetailsUnavailable = "Contract_Details_Unavailable";

	public const string CreditLineDisbursementNotAllowed = "credit_line_disbursement_is_not_allowed";

	public const string InvalidAccountType = "invalid_account_type";

	public const string IncorrectReceiverAccount = "incorrect_receiver_account";

	public const string CannotModifySplittedDocument = "cannot_modify_splitted_document";

	public const string RowSystemTypesMustMatchGroupSystemTypes = "row_system_types_must_match_group_system_types";
}
