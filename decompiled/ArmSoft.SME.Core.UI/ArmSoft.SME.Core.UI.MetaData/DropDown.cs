namespace ArmSoft.SME.Core.UI.MetaData;

public class DropDown
{
	public string Type { get; private set; }

	public string Name { get; set; }

	public string Group { get; set; }

	public bool CheckValue { get; set; }

	public bool ShowComment { get; set; }

	public bool AllowAdditionButtons { get; set; }

	public bool IsTextEditable { get; set; }

	public bool IsMultiSelect { get; set; }

	public bool IsDropDownComment { get; set; }

	public bool IsFileDialog { get; set; }

	public DropDown(string type)
	{
		CheckValue = true;
		ShowComment = true;
		AllowAdditionButtons = false;
		IsDropDownComment = false;
		IsTextEditable = true;
		IsMultiSelect = false;
		IsFileDialog = false;
		Type = type;
	}
}
