namespace ArmSoft.TaxService.Enums;

public enum SignState
{
	HashCalculating = 1,
	SignWaiting,
	Failed,
	SignProcessing,
	Signed,
	SignFailed
}
