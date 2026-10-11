using System;

namespace ArmSoft.OpenBanking.Api.Common;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class NotSensitiveDataAttribute : Attribute
{
}
