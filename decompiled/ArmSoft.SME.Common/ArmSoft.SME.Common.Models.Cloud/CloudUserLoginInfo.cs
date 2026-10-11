using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Cloud;

public class CloudUserLoginInfo
{
	public int UserId { get; set; }

	public string Name { get; set; }

	public string Caption { get; set; }

	public string Email { get; set; }

	public List<string> AvailableKeys { get; set; }
}
