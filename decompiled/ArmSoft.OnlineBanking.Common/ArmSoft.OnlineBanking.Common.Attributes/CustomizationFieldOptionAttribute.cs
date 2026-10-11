using System;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class CustomizationFieldOptionAttribute : Attribute
{
	public Type Type { get; set; }
}
