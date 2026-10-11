using System;
using System.Collections.Generic;

namespace ArmSoft.OpenBanking.Api.Common;

public class UserInfo
{
	public Guid Id { get; set; }

	public string? Email { get; set; }

	public string? Name { get; set; }

	public bool IsIntegrationService { get; set; }

	public bool IsArmSoftMobileApp { get; set; }

	public List<int> AcsRoles { get; set; } = new List<int>();
}
