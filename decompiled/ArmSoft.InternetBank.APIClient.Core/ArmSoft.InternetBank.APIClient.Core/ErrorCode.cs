namespace ArmSoft.InternetBank.APIClient.Core;

public static class ErrorCode
{
	public const string InvalidUsernameOrPassword = "invalid_username_or_password";

	public const string SecureAuthenticationRequired = "secure_authentication_required";

	public const string InvalidSecureAuthenticationCredentials = "invalid_secure_authentication";

	public const string UnallowableIPAddress = "unallowable_ip_address";

	public const string NotVerified = "not_verified";

	public const string MustChangePassword = "must_change_password";

	public const string MustActivateSecurityProvider = "must_activate_security_provider";

	public const string UnsupportedVersion = "unsupported_version";

	public const string InvalidClientCode = "invalid_client_code";

	public const string RequestExists = "request_exists";

	public const string InvalidSignature = "invalid_signature";

	public const string LimitViolation = "limit_violation";

	public const string PaymentFailed = "payment_failed";

	public const string UserDeletionFailed = "user_deletion_failed";

	public const string UserTemplateDeletionFailed = "user_template_deletion_failed";

	public const string InvalidVerificationCode = "invalid_verification_code";

	public const string PropertyTaxNotFound = "property_tax_not_found";

	public const string InvalidAuthenticationData = "invalid_authentication_data";

	public const string SecurityProviderDataRefreshIsNotAllowed = "security_provider_data_refresh_is_not_allowed";

	public const string InvalidQr = "invalid_qr";

	public const string InvalidUserInfo = "invalid_user_information";

	public const string ExpiredSystemVerificationCode = "expired_system_verification_code";

	public const string InsufficientVerificationData = "insufficient_verification_data";

	public const string InvalidPassword = "invalid_password";

	public const string InvalidTokenResponse = "invalid_token_response";

	public const string SigningFailed = "signing_failed";

	public const string DocumentRefused = "document_refused";
}
