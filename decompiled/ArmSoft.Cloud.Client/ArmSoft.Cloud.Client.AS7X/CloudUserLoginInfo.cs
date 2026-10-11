using System.Collections.Generic;

namespace ArmSoft.Cloud.Client.AS7X;

public class CloudUserLoginInfo
{
	public int UserId { get; set; }

	public string Name { get; set; }

	public string Caption { get; set; }

	public string Email { get; set; }

	public List<string> AvailableKeys { get; set; }
}
