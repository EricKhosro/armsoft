namespace ArmSoft.SME.Common.Models.Cloud;

public class UserRolesRequest
{
	public string AdToken { get; set; }

	public int? UserId { get; set; }

	public int DatabaseId { get; set; }
}
