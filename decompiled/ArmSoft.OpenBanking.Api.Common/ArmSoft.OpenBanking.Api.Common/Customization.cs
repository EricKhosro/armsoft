using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class Customization
{
	public Guid Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;
}
