using System;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class SystemTypeAttribute : Attribute
{
	public bool HasAccessToMainApi { get; set; }

	public SystemTypeAttribute(bool hasApiAccess)
	{
		HasAccessToMainApi = hasApiAccess;
	}
}
