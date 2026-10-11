namespace ArmSoft.OnlineBanking.Common.Enums;

public enum PushNotificationType : byte
{
	NotSet,
	DocumentStateChanged,
	MessageStateChanged,
	NewMessage,
	NewNotification,
	DataMessage,
	UserOperation,
	UserTaskStateChanged,
	RateSuggestionStateChanged,
	DocumentSignQueueCompleted,
	StatementGenerationResult
}
