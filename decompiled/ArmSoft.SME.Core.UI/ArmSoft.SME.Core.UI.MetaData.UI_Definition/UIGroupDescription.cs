namespace ArmSoft.SME.Core.UI.MetaData.UI_Definition;

public class UIGroupDescription : UIElementDescription
{
	public string Name { get; set; }

	public bool IsHorizontal { get; set; }

	public bool IsCollapsible { get; set; } = true;

	public bool IsCollapsed { get; set; }

	public string Tab { get; set; }

	public string Group { get; set; }

	public string Mask => "UD_.*";
}
