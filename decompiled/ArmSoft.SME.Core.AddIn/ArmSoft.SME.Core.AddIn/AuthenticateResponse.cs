using System.Collections.Generic;

namespace ArmSoft.SME.Core.AddIn;

public class AuthenticateResponse
{
	public bool IsAdmin { get; set; }

	public string AccessToken { get; set; }

	public List<Role> Roles { get; set; }

	public bool PSMode { get; set; }

	public string UserName { get; set; }

	public int UserId { get; set; }

	public string UserDescription { get; set; }

	public string Phone { get; set; }

	public string Database { get; set; }

	public string Key { get; set; }
}
