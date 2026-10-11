namespace ArmSoft.SME.Common.SmartEnums;

public class PrintTemplateTypeSE : ASSmartEnum<PrintTemplateTypeSE, string>
{
	public static readonly PrintTemplateTypeSE Doc = new PrintTemplateTypeSE("Doc", "D");

	public static readonly PrintTemplateTypeSE OnlineReport = new PrintTemplateTypeSE("OnlineReport", "T");

	public static readonly PrintTemplateTypeSE User = new PrintTemplateTypeSE("User", "U");

	private PrintTemplateTypeSE(string name, string value)
		: base(name, value)
	{
	}
}
