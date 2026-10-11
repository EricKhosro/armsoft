using System.Collections.Generic;

namespace ArmSoft.Cloud.Client.AS7X;

public class UserLoginResponseV2
{
	public int UserId { get; set; }

	public string UserName { get; set; }

	public string UserDescription { get; set; }

	public List<AvailableDatabase> AvailableDatabases { get; set; }

	public List<KeyInfoExt> AvailableKeys { get; set; }
}
