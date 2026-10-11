using System.Runtime.Serialization;

namespace ArmSoft.OnlineBanking.Common.Enums;

[DataContract(Name = "PassportType")]
public enum PassportType : byte
{
	[EnumMember]
	NotSet,
	[EnumMember]
	RA,
	[EnumMember]
	Foreign,
	[EnumMember]
	DeathCertificate
}
