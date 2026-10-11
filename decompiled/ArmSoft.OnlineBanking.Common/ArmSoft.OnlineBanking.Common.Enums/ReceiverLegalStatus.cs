using System.Runtime.Serialization;

namespace ArmSoft.OnlineBanking.Common.Enums;

public enum ReceiverLegalStatus : byte
{
	[EnumMember]
	NotSet,
	[EnumMember]
	Legal,
	[EnumMember]
	Natural
}
