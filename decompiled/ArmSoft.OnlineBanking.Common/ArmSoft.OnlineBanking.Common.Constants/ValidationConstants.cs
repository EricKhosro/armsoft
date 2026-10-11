namespace ArmSoft.OnlineBanking.Common.Constants;

public static class ValidationConstants
{
	public const string AmountFormat = "#,###,###,##0.00";

	public const string NumberFormat = "#,###,###,##0";

	public const string RateFormat = "#,###,###,##0.####";

	public const string RateFormatFixed = "#,###,###,##0.0000";

	public const int PayeeNameMaxLength = 140;

	public const int PayeeReceiverIdMaxLength = 16;

	public const int CardNumberMinLength = 13;

	public const int PayeeNoteMaxLength = 32;

	public const int VehicleLicensePlateNumberMaxLength = 20;

	public const int VehicleRegistrationCertificateNumberMaxLength = 20;

	public const int VehicleNoteMaxLength = 32;

	public const string SwiftPattern = "^[A-Z]{6}[0-9A-Z]{2}([0-9A-Z]{3})?$";

	public const int SwiftPayerFieldMaxLength = 35;

	public const int SwiftPayerFieldCountryISOMaxLength = 34;

	public const int SwiftBankAccountMaxLength = 36;

	public const int SwiftEntityMaxLength = 35;

	public const int SwiftDetailsMaxLength = 35;

	public const int SwiftBankDetailsMaxLength = 140;

	public const int SwiftAdditionalInfoMaxLength = 35;

	public const int SwiftPayerDetailMaxLength = 35;

	public const string CodeForEmptyRows = "9";

	public const int ReceiverNameMaxLength = 99;

	public const int ReceiverAddressMaxLength = 66;

	public const int ReceiverNameAddressMaxLength = 99;

	public const int ReceiverCityMaxLength = 30;

	public const int ReceiverAccountMaxLength = 34;

	public const int ReceivingCountryCodeMaxLength = 2;

	public const int SwiftCodeMaxLength = 20;

	public const string SwiftPayerDetailRegEx = "^[1-8][\\/][0-9a-zA-Z/' ()+,.:?{}-]+$";

	public const string SwiftPayerFieldCountryISORegEx = "^[1-8][\\/][A-Z]{2}[\\/][0-9a-zA-Z/' ()+,.:?{}-]+$";

	public const int VisaB2BDetailsMinLength = 3;

	public const int VisaB2BDetailsMaxLength = 35;

	public const int VisaB2BPurposeCodeMaxLength = 35;

	public const int VisaB2BPAdditionalInfoMaxLength = 140;

	public const int TransferPurposeMaxLength = 4;

	public const int ReceiverINNMaxLength = 12;

	public const int ReceiverKPPMaxLength = 9;

	public const int SwiftBankIDMaxLength = 20;

	public const int SwiftBankBICMaxLength = 9;

	public const int UtilityDebtDeviation = 5;

	public const int UtilityAbonentCodeMaxLength = 21;

	public const int UtilityAbonentIdMaxLength = 19;

	public const int UtilityAbonentNameMaxLength = 70;

	public const int UtilityAbonentAddressMaxLength = 70;

	public const int UtilityLocationCodeMaxLength = 3;

	public const int UtilityAbonentDetailsMaxLength = 70;

	public const int UtilitySpecialMaxLength = 500;

	public const string PhoneNumberRegEx = "^((\\(\\+?[ \\d]*\\))?|(\\+?(\\([ \\d]*\\)))?|\\+?)?([ \\d]*?)$";

	public const string EmailRegEx = "^[\\w!#$%&@'\"\"*+\\-/=?\\^_`{|}~\\\\]+(\\.[\\w!#$%&@'\"\"*+\\-/=?\\^_`{|}~\\\\]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,63})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))$";

	public const string ContactDataRegEx = "^((\\(\\+?[ \\d]*\\))?|(\\+?(\\([ \\d]*\\)))?|\\+?)?([ \\d]*?)$|^[\\w!#$%&@'\"\"*+\\-/=?\\^_`{|}~\\\\]+(\\.[\\w!#$%&@'\"\"*+\\-/=?\\^_`{|}~\\\\]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,63})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))$";

	public const decimal MapPointLatitudeMinValue = -90m;

	public const decimal MapPointLatitudeMaxValue = 90m;

	public const decimal MapPointLongitudeMinValue = -180m;

	public const decimal MapPointLongitudeMaxValue = 180m;

	public const int MapPointPhoneNumberMaxLength = 20;

	public const string MapPointCoordinateFormat = "0-9/./-";

	public const int RequestCommentMaxLength = 50;

	public const int NewsTitleMaxLength = 80;

	public const int NewsNameMaxLength = 80;

	public const int MailTemplateNameMaxLength = 30;

	public const int MailTemplateSubjectMaxLength = 100;

	public const int AppAuthenticationNameMaxLength = 50;

	public const int ServiceAgreementTitleMaxLength = 250;

	public const int SignatureConfigurationNameMaxLength = 50;

	public const string DefaultSignatureConfigurationName = "Default";

	public const int AllowedMinimalSignatureRank = 1;

	public const int AllowedMaximalSignatureRank = 3;

	public const int ClientCodeMaxLength = 8;

	public const int PaymentGroupNameMaxLength = 50;

	public const int PaymentGroupDescriptionMaxLength = 100;

	public const int TaxPropertyInfoMaxLength = 20;

	public const int TaxPropertyIdMaxLength = 12;

	public const int TaxPayerAddressMaxLength = 255;

	public const int TaxPaymentDetailsMaxLength = 75;

	public const int TaxPaymentPayerNameMaxLength = 255;

	public const int TaxPayerInfoRowMaxLength = 255;

	public const int TaxPaymentRowYearMaxLength = 4;

	public const int TaxPaymentRowDebtIdMaxLength = 80;

	public const int SocialCardMaxLength = 10;

	public const int TaxCodeMaxLength = 10;

	public const int TaxCodeFixedLength = 8;

	public const int TaxAuthorityCodeFixedLength = 2;

	public const int ThirdPartyPassportNumberMaxLength = 100;

	public const int AdditionalInformationLineMaxLength = 35;

	public const int AdditionalInformationMaxLength = 46;

	public const int AdditionalInformationLineCount = 6;

	public const int ThirdPartyDetailsMaxLength = 33;

	public const string ReportCodeRegEx = "^(PTD/)|^$";

	public const string MessageLineRegEx = "^([A-Z0-9]{3}[/]{1})|^$";

	public const string EmptySocialCard = "0000000000";

	public const int DocumentLinkNameMaxLength = 50;

	public const int DocumentLinkLinkNameMaxLength = 500;

	public const int DocumentLinkLinkUrlMaxLength = 500;

	public const int DocumentInfoNameMaxLength = 50;

	public const int DocumentInfoUtilityTypesMaxLength = 60;

	public const int SmsCodeMaxExpiration = 3600;

	public const int SmsTextMaxLength = 100;

	public const int AuthenticatorTokenCommentMaxLength = 50;

	public const int DocumentSignPushNotificationMaxExpiration = 60;

	public const string UrlRegEx = "^(https?:\\/\\/(?:www\\.|(?!www))[^\\s\\.]+\\.[^\\s]{2,})$";

	public const int MaxRowsPerPageMinValue = 10;

	public const int MaxRowsPerPageMaxValue = 1000;

	public const int EncryptionKeyLength = 16;

	public const int ClientRegistrationTermsTextMaxLength = 100;

	public const int ClientRegistrationAdditionalInformationtMaxLength = 500;

	public const int ClientRegistrationCompletedMessageMaxLength = 250;

	public const int TransitionToFullUserTermsTextMaxLength = 40;

	public const int ArcaMerchantUsernameMaxLength = 100;

	public const int ArcaMerchantPasswordMaxLength = 100;

	public const int DigipassCommentMaxLength = 50;

	public const int QuestionAnswerMaxLengthAccountNo = 16;

	public const int QuestionAnswerLengthCardNumber = 16;

	public const int QuestionAnswerMaxLengthContractNo = 20;

	public const int QuestionAnswerMaxLengthSocialCard = 10;

	public const int QuestionAnswerMaxLengthTaxCode = 20;

	public const int QuestionAnswerLengthCardLastDigits = 6;

	public const int DepositAttractionDetailsMaxLength = 100;

	public const int ContractNumberMaxLength = 20;

	public const string VersionRegEx = "^\\d\\d\\.\\d\\d\\.\\d\\d$";

	public const int ApiCredentialSecretMaxLength = 32;

	public const int ApiCredentialNameMaxLength = 50;

	public const int NotificationTitleMaxLengthForMobile = 160;

	public const int NotificationTitleMaxLengthForWebsite = 250;

	public const int NotificationLinkDocumentTypeMaxLength = 20;

	public const int NotificationLinkModuleMaxLength = 100;

	public const int NotificationLinkUrlMaxLength = 500;

	public const int NotificationLinkTextMaxLength = 500;

	public const int AccountNoteMaxLength = 32;

	public const int AccountNumberMinLength = 12;

	public const int AccountNumberMaxLength = 16;

	public const string AccountListRegEx = "^\\d{12,16}((,\\d{12,16})+)?$";

	public const int DocumentNumberMaxLength = 6;

	public const int DocumentTaxCodeMinLength = 10;

	public const int DocumentTaxCodeMaxLength = 20;

	public const string DocumentAttachmentUnsupportedRegularExpression = "^([a-zA-Z]+(,[\\s]*[a-zA-Z]+)*)?$";

	public const int DocumentDetailsMaxLength = 140;

	public const int DocumentTemplateNameMaxLength = 50;

	public const int CurrencyMaxLength = 3;

	public const int DocumentExternalPaymentIdMaxLength = 128;

	public const int SubjectMaxlength = 50;

	public const decimal PaymentAmountMinValue = 0m;

	public const string PaymentAmountMinValueDisplay = "0";

	public const decimal PaymentAmountMaxValue = 9999999999999.99m;

	public const string PaymentAmountMaxValueDisplay = "9,999,999,999,999.99";

	public const int PaymentReceiverMaxLength = 140;

	public const int PassportNumberMaxLength = 32;

	public const int BankRequisiteNameMaxLength = 50;

	public const int BankRequisiteCurrencyLength = 3;

	public const int BankRequisiteDescriptionMaxLength = 50;

	public const int CustomDirectoryNameMaxLength = 50;

	public const int CustomDirectoryDescriptionMaxLength = 50;

	public const int CustomDirectoryValueLabelMaxLength = 50;

	public const int CustomDirectoryValueCodeMaxLength = 10;

	public const int RoleNameMaxLength = 10;

	public const int RoleDescriptionMaxLength = 50;

	public const int UtilityDisplayNameMaxLength = 100;

	public const int FailedAttemptsMaxCount = 5;

	public const int VerificationCodeLength = 4;

	public const int AdditionalMinutesToCheckVerificationCodeForBankClientRegistration = 10;

	public const int ClientLimitNameMaxLength = 50;

	public const int CustomizationNameMaxLength = 8;

	public const int CustomizationDescriptionMaxLength = 140;

	public const int CustomizationDetailsMaxLength = 140;

	public const int CustomizationObjectButtonLabelMaxLength = 140;

	public const int CustomizationListButtonLabelMaxLength = 140;

	public const int CustomizationIconMaxLength = 40;

	public const int CustomizationFieldNameMaxLength = 50;

	public const int CustomizationFieldLabelMaxLength = 965;

	public const int CustomizationFieldTooltipMaxLength = 200;

	public const int CardNoteMaxLength = 32;

	public const int CardSettingsTermsTextMaxLength = 40;

	public const int CardNumberMaxLength = 16;

	public const int PersonalDataModificationAcceptTermsTextMaxLength = 1000;

	public const int CustomerPersonalMobileEmailDataModificationAcceptTermsTextMaxLength = 1000;

	public const int CustomerPersonalPassportDataModificationAcceptTermsTextMaxLength = 1000;

	public const int NewWebSwitchingTextMaxLength = 500;

	public const int AuthenticationAvailabilityNameMaxLength = 50;

	public const int RateSuggestionAvailabilityNameMaxLength = 50;

	public const int RateSuggestionTermsTextMaxLength = 100;

	public const int UsernameMaxLength = 50;

	public const int FirstNameMaxLength = 50;

	public const int LastNameMaxLength = 50;

	public const int UserPassportNumberMaxLength = 20;

	public const int MobileMaxLength = 18;

	public const int EmailMaxLength = 70;

	public const string OperatorUsernameRegEx = "^[0-9a-zA-Z_]*$|^[a-zA-Z_0-9\\.\\-]+\\\\[a-zA-Z_0-9\\.\\s\\-\\!\\@\\#\\$\\%\\^\\&\\-\\`]*$";

	public const int AdditionalMinutesToCheckVerificationCodeForPasswordReset = 5;

	public const int AdditionalMinutesToCheckVerificationCodeForSecurityProviderActivation = 60;

	public const int UserTemplateNameMaxLength = 50;

	public const int UserTemplateDescriptionMaxLength = 50;

	public const int DigitalCardDesignMaxLength = 255;

	public const int CardTypeShortDescriptionMaxLength = 50;

	public const int ViolationIdMaxLength = 10;

	public const int ViolationTaskIdMaxLength = 10;

	public const int ViolationFullNameMaxLength = 255;

	public const int RateSuggestionCommentMaxLength = 500;

	public const int RateSuggestionRejectCommentMaxLength = 100;

	public const int RateSuggestionConfigurationRolesMaxLength = 1024;

	public const decimal RateSuggestionConfigurationAmountMinValue = 0m;

	public const decimal RateSuggestionConfigurationAmountMaxValue = 9999999999999.99m;

	public const int ArCaP2POtpMaxLength = 6;

	public const int ArCaP2PPhoneNumberMaxLength = 11;

	public const int ArCaPayCodeMaxLength = 2;

	public const int AccountTypeShortDescriptionMaxLength = 50;

	public const int AccountTypeMaxLength = 4;

	public const int AccountTypeNameMaxLength = 50;

	public const int AccountGoalMaxLength = 512;

	public const int InsuranceDurationMinValue = 3;

	public const int InsuranceDurationMaxValue = 12;

	public const int ContractDefaultAccountMinLength = 3;

	public const int ContractDefaultAccountMaxLength = 16;

	public const int TaxCodeMinLength = 8;

	public const int AswaTaxCodeMaxLength = 20;

	public const int ContractMobileMaxLength = 20;

	public const int ThirdPartyNameMaxLength = 65;

	public const int CreditContractNumberMaxLength = 20;

	public const int CreditDetailsMaxLength = 100;

	public const int MultiplePaymentReceiverMaxCount = 50;

	public const int CardDesignThemeTitleMaxLength = 500;

	public const int CardDesignNameMaxLength = 128;

	public const int CardDesignProfileIdMaxLength = 4;

	public const int UserTaskTitleMaxLength = 256;

	public const int UserTaskRewardTextMaxLength = 1024;

	public const int UserTaskDescriptionMaxLength = 1024;

	public const int NewsLinkTextMaxLength = 500;

	public const int NewsLinkUrlMaxLength = 500;

	public const int NewsLocalizableImageMaxLength = 500;

	public const int NewsLinkDocumentTypeMaxLength = 20;

	public const int NewsLinkModuleMaxLength = 100;

	public const int BankClientInformationDefaultMaxLength = 16;

	public const int RequestMoneyDescriptionMaxLength = 140;

	public const int RequestMoneyCommentMaxLength = 100;

	public const int OpenCreditApplicationProductDescriptionMaxLength = 512;

	public const decimal CheckCreditAndTotalAmountsMaxAmountValue = 300000m;

	public const int AmeriaSignableFileTypeDescriptionMaxLength = 512;

	public const int AmeriaSignableFileTypeQueryParamsMaxLength = 1024;

	public const string PercentFormat = "#0.00";

	public const decimal PercentMaxValue = 99.99m;

	public const string PercentMaxValueDisplay = "99.99";

	public const decimal PercentMinValue = 0m;

	public const string PercentMinValueDisplay = "0";

	public const int DepositTemplateCodeMaxLength = 4;

	public const int PurchaseBondIssuanceNumberMaxLength = 12;

	public const string BondAmountFormat = "#,###,###,##0.0000";

	public const int CardTypeDisplayNameMaxLength = 500;

	public const int DocumentConfigurationDetailsMaxLength = 140;

	public const int DocumentConfigurationDetailsMinLength = 5;

	public const int DocumentNameMaxLength = 140;

	public const int MessageSubjectTemplateNameMaxLength = 50;

	public const int MessageSubjectTemplateDescriptionMaxLength = 500;

	public const int MessageSubjectTemplateEmailToSentMaxLength = 1000;

	public const decimal CashOutByPhoneCompositeAmount = 1000m;

	public const string CashOutByPhoneCompositeAmountDisplay = "1,000";

	public const int CashOutByPhoneMobileMinLength = 8;

	public const int CashOutByPhoneMobileMaxLength = 18;

	public const int DebtorNameMaxLength = 256;

	public const int PlaintiffNameMaxLength = 256;

	public const int InquestIdMaxLength = 64;

	public const int PropertyTaxesMaxCount = 9;

	public const int PropertyTaxPaymentRowInfoMaxLength = 200;

	public const int PropertyTaxPaymentRowPropertyOwnerMaxLength = 256;

	public const int PropertyTaxPaymentRowYearMaxLength = 4;

	public const int PropertyTaxPaymentRowTaxDebtIdMaxLength = 10;

	public const int PropertyTaxPaymentDetailsMaxLength = 30;

	public const int ReceiverContactDataMaxLength = 32;

	public const int ReceiverPhoneNumberMinLength = 8;

	public const int CashWithdrawalReceiverLastNameMaxLength = 50;

	public const int CashWithdrawalReceiverPassportAuthorityMaxLength = 20;

	public const int ClientFinancialDeclarationInformationTextMaxLength = 500;

	public const int ImageUrlMaxLength = 1024;

	public const int DocumentCreationEmailTemplateSubjectMaxLength = 500;

	public const int PayerReceiverNameMaxLength = 140;

	public const int PayerReceiverDetailsMaxLength = 132;

	public const int CreditCodeMaxLength = 30;

	public const int CreditContractOwnerMaxLength = 100;

	public const int AdditionalInfoMaxLength = 35;

	public const int AdditionalInfoRowCount = 4;

	public const int AdditionalInfoRowLength = 33;

	public const int CreditRepaymentViaPartnerContractNumberMaxLength = 14;

	public const int CreditRepaymentViaPartnerCreditCodeMaxLength = 16;

	public const int CreditRepaymentViaPartnerBankPartnerCodeMaxLength = 5;

	public const int CreditRepaymentViaPartnerPassportNumberMaxLength = 20;

	public const int CreditLineDisbursementContractNumberMaxLength = 14;

	public const int MerchantCategoryCodeMaxLength = 4;

	public const int ExternalCodeMaxLength = 12;

	public const int MerchantIdMaxLength = 15;

	public const int MerchantNameMaxLength = 50;

	public const int MerchantCityMaxLength = 50;

	public const int TerminalIdMaxLength = 8;

	public const int ReceiverTaxCodeMaxLength = 8;

	public const int ClientCategoryConfigurationTitleMaxLength = 500;

	public const int ClientCategoryConfigurationLinkUrlMaxLength = 3999;
}
