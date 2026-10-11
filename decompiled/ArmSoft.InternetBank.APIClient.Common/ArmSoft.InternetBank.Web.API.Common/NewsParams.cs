namespace ArmSoft.InternetBank.Web.API.Common;

public class NewsParams
{
	public string MenuItem { get; set; }

	public bool? IsAlwaysVisible { get; set; }

	public bool IncludeViewed { get; set; }

	public NewsParams()
	{
		IncludeViewed = false;
	}
}
