namespace ArmSoft.SME.Common.Models;

public class CodeNameWithSelection : CodeName
{
	public bool IsSelected { get; set; }

	public CodeNameWithSelection()
	{
	}

	public CodeNameWithSelection(string code, string name, bool isSelected)
		: base(code, name)
	{
		IsSelected = isSelected;
	}
}
