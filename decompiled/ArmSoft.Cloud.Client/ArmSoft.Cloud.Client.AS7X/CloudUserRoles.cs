using System.Collections.Generic;

namespace ArmSoft.Cloud.Client.AS7X;

public class CloudUserRoles
{
	public int UserId { get; set; }

	public List<Role> Roles { get; set; }
}
