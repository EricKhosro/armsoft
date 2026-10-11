namespace ArmSoft.OpenBanking.Api.Common;

public static class BankOperatorAuditEventType
{
	public const string UserCreated = "user_created";

	public const string UserUpdated = "user_updated";

	public const string UserReset = "user_reset";

	public const string UserDeleted = "user_deleted";

	public const string LoginSucceeded = "login_succeeded";

	public const string LoginFailed = "login_failed";

	public const string PasswordReset = "password_reset";

	public const string RefreshSucceeded = "refresh_succeeded";

	public const string Logout = "logout";

	public const string SessionsRevoked = "sessions_revoked";
}
