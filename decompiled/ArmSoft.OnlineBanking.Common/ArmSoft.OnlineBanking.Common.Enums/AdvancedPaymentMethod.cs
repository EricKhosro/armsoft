using System.Runtime.Serialization;

namespace ArmSoft.OnlineBanking.Common.Enums;

[DataContract(Name = "AdvancedPaymentMethod")]
public enum AdvancedPaymentMethod : byte
{
	[EnumMember]
	NotSet,
	[EnumMember]
	ToAccount,
	[EnumMember]
	NoAccount
}
