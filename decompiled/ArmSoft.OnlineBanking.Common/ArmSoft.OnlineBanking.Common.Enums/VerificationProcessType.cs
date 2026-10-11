namespace ArmSoft.OnlineBanking.Common.Enums;

public enum VerificationProcessType : byte
{
	NotSet,
	Activation,
	ResetPassword,
	RefreshSecurityProviderData,
	Authentication,
	PersonalDataModification,
	Registration,
	RecoverUsername,
	SetCardPin,
	UserInformationRetrieve,
	ChangeSecurityProvider
}
