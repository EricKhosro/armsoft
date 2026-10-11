using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Authentication;

public class AuthenticateResponse
{
	public bool IsAdmin { get; set; }

	public string AccessToken { get; set; }

	public List<Role> Roles { get; set; }

	public string UserName { get; set; }

	public int UserId { get; set; }

	public int CloudUserId { get; set; }

	public string UserDescription { get; set; }

	public string Phone { get; set; }

	public string Database { get; set; }

	public string Key { get; set; }

	public string[] IPList { get; set; }

	public bool CacheStructChanged { get; set; }

	public bool PSMode { get; set; }
}
