namespace ArmSoft.SME.Common.Models.Data;

public class Parameter
{
	public string Name { get; private set; }

	public string Caption { get; private set; }

	public SupportedType Type { get; private set; }

	public Parameter(string name, string caption, SupportedType type)
	{
		Name = name;
		Type = type;
		Caption = caption ?? "";
	}
}
