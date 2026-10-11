namespace ArmSoft.Cloud.Client.AS7X;

public class CloudUserInfo
{
	public int UserId { get; set; }

	public string Caption { get; set; }

	public string IdentityProvider { get; set; }

	public string Email { get; set; }

	public string Phone { get; set; }

	public bool IsAdmin { get; set; }

	public bool LoginDisabled { get; set; }

	public bool IsDBAdmin { get; set; }
}
