namespace ArmSoft.InternetBank.Web.API.Common;

public class GetUserListItemsByClientCodeParams
{
	public string ClientCode { get; set; }

	public bool IncludeUnverifiedUsers { get; set; }

	public GetUserListItemsByClientCodeParams()
	{
		IncludeUnverifiedUsers = false;
	}
}
