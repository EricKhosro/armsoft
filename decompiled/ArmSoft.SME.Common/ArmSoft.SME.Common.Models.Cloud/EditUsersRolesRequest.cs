using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Authentication;

namespace ArmSoft.SME.Common.Models.Cloud;

public class EditUsersRolesRequest
{
	public string AdToken { get; set; }

	public int DatabaseId { get; set; }

	public List<UserRoles> UsersRoles { get; set; }
}
