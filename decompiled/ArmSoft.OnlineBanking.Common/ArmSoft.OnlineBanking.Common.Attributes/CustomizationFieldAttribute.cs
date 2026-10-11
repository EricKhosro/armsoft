using System;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class CustomizationFieldAttribute : Attribute
{
	public Type Type { get; set; }
}
