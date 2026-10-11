using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models;

public class UserUpdateRequest
{
	public string IdToken { get; set; }

	public List<int> UserIds { get; set; }
}
