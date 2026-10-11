using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Extensions;

public static class VerificationMethodExtension
{
	public static VerificationMethods SetFlag(this VerificationMethods method, VerificationMethods value)
	{
		return method | value;
	}
}
