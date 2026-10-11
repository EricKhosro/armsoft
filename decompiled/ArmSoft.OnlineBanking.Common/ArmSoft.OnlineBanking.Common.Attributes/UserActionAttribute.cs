using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class UserActionAttribute : Attribute
{
	public UserActionCategory Category { get; set; }

	public UserActionReason Reason { get; set; }

	public UserActionAttribute(UserActionCategory category, UserActionReason reason = UserActionReason.NotSet)
	{
		Category = category;
		Reason = reason;
	}
}
