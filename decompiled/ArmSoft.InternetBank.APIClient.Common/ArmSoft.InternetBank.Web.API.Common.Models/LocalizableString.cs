using ArmSoft.OnlineBanking.Common;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class LocalizableString
{
	public string Hy { get; set; }

	public string En { get; set; }

	public string Ru { get; set; }

	public LocalizableString()
	{
		Hy = string.Empty;
		En = string.Empty;
		Ru = string.Empty;
	}

	public LocalizableString(string arm, string eng, string rus)
	{
		Hy = arm;
		En = eng;
		Ru = rus;
	}

	public LocalizableString(LocalizableLabel value)
	{
		Hy = value.GetTextByLanguage("hy-AM");
		En = value.GetTextByLanguage("en-US");
		Ru = value.GetTextByLanguage("ru-RU");
	}
}
