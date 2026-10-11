namespace ArmSoft.OpenBanking.Api.Common;

public enum AuthenticationProcessState
{
	NotSet,
	New,
	BankAuthentication,
	BankCallbackProcessing,
	AuthenticationCodeIssued,
	TokenIssuing,
	Completed,
	LoginFailed,
	Expired
}
