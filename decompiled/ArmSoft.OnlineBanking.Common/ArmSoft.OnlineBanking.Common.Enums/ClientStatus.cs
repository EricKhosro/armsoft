using System.Runtime.Serialization;

namespace ArmSoft.OnlineBanking.Common.Enums;

[DataContract(Name = "ClientStatus")]
public enum ClientStatus : byte
{
	[EnumMember]
	NotSet = 0,
	[EnumMember]
	Legal = 1,
	[EnumMember]
	Natural = 2,
	[EnumMember]
	Entrepreneur = 3,
	All = byte.MaxValue
}
