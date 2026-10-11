using System;

namespace ArmSoft.SME.Common.SmartEnums;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class SmartEnumProviderSupportAttribute(short valueMemberLength, string captionResourceId) : Attribute
{
	public short ValueMemberLength { get; } = valueMemberLength;

	public string CaptionResourceId { get; } = captionResourceId;
}
