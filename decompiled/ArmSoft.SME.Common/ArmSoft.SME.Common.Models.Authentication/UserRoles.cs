using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Authentication;

public class UserRoles
{
	public int UserId { get; set; }

	public List<Role> Roles { get; set; }
}
