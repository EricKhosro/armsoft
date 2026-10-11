namespace ArmSoft.OnlineBanking.Common.Enums;

public enum PeriodicTransferBehaviorOnNonWorkingDay : byte
{
	Perform,
	PerformPreviousWorkingDay,
	PerformNextWorkingDay,
	NotPerform
}
