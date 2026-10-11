using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class PermissionAttribute : Attribute
{
	public PermissionGroup Group { get; set; }

	public string Module { get; set; } = string.Empty;

	public int Order { get; set; }

	public string DescriptionAm { get; set; } = string.Empty;

	public string DescriptionEn { get; set; } = string.Empty;

	public string DescriptionRu { get; set; } = string.Empty;
}
