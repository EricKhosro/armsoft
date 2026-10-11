using System;

namespace ArmSoft.SME.Common.SmartEnums;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class SmartEnumPartialAttribute : Attribute
{
	public SmartEnumType SmartEnumType { get; }

	public SmartEnumPartialAttribute(SmartEnumType smartEnumType)
	{
		SmartEnumType = smartEnumType;
	}
}
