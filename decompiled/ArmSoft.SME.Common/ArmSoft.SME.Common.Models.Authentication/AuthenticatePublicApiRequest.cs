using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Authentication;

public class AuthenticatePublicApiRequest
{
	public int UserId { get; set; }

	public string UserName { get; set; }

	public string UserDescription { get; set; }

	public string Phone { get; set; }

	public bool IsAdmin { get; set; }

	public bool IsDBAdmin { get; set; }

	public string Database { get; set; }

	public int DatabaseId { get; set; }

	public int ServerId { get; set; }

	public List<Role> Roles { get; set; }

	public bool PSMode { get; set; }

	public string Key { get; set; }

	public string ServiceAddress { get; set; }
}
