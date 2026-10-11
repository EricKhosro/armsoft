namespace ArmSoft.SME.Common.Models;

public class CodeName
{
	public string Code { get; set; }

	public string Name { get; set; }

	public CodeName()
	{
	}

	public CodeName(string code, string name)
	{
		Code = code;
		Name = name;
	}
}
