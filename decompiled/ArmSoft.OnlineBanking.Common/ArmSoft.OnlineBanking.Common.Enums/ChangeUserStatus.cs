namespace ArmSoft.OnlineBanking.Common.Enums;

public enum ChangeUserStatus : byte
{
	Success = 1,
	AssociatedUserNotFound,
	AssociatedUserMustChangePassword,
	AssociatedUserIsLockedOut,
	AssociatedUserMustActivateSecurityProvider
}
