using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class MenuItemIgnoreAttribute : Attribute
{
	public DomainRestrictions Domain { get; }

	public MenuItemIgnoreAttribute(DomainRestrictions domain = DomainRestrictions.All)
	{
		Domain = domain;
	}
}
