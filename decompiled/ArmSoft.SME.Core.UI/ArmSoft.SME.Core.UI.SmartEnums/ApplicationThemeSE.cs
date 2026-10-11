using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.SmartEnums;

public class ApplicationThemeSE : ASSmartEnum<ApplicationThemeSE, string>
{
	public static readonly ApplicationThemeSE Office = new ApplicationThemeSE("Office", "Office", () => "Office");

	public static readonly ApplicationThemeSE Win11 = new ApplicationThemeSE("Win11", "Win11", () => "Win11");

	public const string OfficeConst = "Office";

	public const string Win11Const = "Win11";

	public ApplicationThemeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
