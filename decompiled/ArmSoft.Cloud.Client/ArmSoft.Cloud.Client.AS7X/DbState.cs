namespace ArmSoft.Cloud.Client.AS7X;

public enum DbState : byte
{
	Operational,
	NameReserved,
	BlockedByAdmin,
	BlockedByServiceJob,
	Deleted
}
