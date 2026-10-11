namespace ArmSoft.OnlineBanking.Common.Enums;

public enum DocumentImportStatus : byte
{
	NotSet,
	Success,
	ValidationError,
	NoAccess,
	Duplicate
}
