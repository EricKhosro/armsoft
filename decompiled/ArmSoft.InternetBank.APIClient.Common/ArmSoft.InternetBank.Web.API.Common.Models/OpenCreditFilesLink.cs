using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class OpenCreditFilesLink
{
	public string Url { get; set; }

	public string Name { get; set; }

	public string Text { get; set; }

	public OpenCreditFileType Type { get; set; }

	public bool Accepted { get; set; }
}
