using System.Runtime.Serialization;

namespace ArmSoft.OnlineBanking.Common.Enums;

[DataContract(Name = "ResidenceType")]
public enum ResidenceType : byte
{
	[EnumMember]
	NotSet,
	[EnumMember]
	Resident,
	[EnumMember]
	NonResident
}
