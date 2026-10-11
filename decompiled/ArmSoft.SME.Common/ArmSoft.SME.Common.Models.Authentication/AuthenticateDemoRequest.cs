namespace ArmSoft.SME.Common.Models.Authentication;

public class AuthenticateDemoRequest
{
	public int UserId { get; set; }

	public string UserName { get; set; }

	public string UserDescription { get; set; }

	public string Phone { get; set; }

	public int DataBaseID { get; set; }

	public string CompName { get; set; }
}
