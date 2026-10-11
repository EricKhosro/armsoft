using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class PermissionGroupAttribute : Attribute
{
	public UserDomain Domain { get; set; }
}
