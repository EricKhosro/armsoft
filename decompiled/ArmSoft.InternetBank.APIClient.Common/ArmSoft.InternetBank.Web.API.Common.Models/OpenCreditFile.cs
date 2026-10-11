using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class OpenCreditFile
{
	public OpenCreditFileType Type { get; set; }

	public string Content { get; set; }

	public string Extension { get; set; }
}
