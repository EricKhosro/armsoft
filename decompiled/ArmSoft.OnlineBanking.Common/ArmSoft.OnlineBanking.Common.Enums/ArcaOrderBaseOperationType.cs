namespace ArmSoft.OnlineBanking.Common.Enums;

public enum ArcaOrderBaseOperationType : byte
{
	NotSet,
	Attach,
	AttachedCardPayment,
	DeactivateAttachedCard,
	AttachBySystem,
	TransferToAttachedCard
}
