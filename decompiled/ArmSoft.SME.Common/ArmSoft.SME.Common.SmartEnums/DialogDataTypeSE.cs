namespace ArmSoft.SME.Common.SmartEnums;

public class DialogDataTypeSE : ASSmartEnum<DialogDataTypeSE, string>
{
	public static readonly DialogDataTypeSE Setting = new DialogDataTypeSE("S", "S");

	public static readonly DialogDataTypeSE Values = new DialogDataTypeSE("V", "V");

	public static readonly DialogDataTypeSE Filters = new DialogDataTypeSE("F", "F");

	private DialogDataTypeSE(string name, string value)
		: base(name, value)
	{
	}
}
