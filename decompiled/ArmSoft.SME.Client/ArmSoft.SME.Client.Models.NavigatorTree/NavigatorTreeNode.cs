using System.Collections.Generic;

namespace ArmSoft.SME.Client.Models.NavigatorTree;

public class NavigatorTreeNode
{
	public string Caption { get; set; }

	public string Name { get; set; }

	public string Module { get; set; }

	public string Icon { get; set; }

	public List<NavigatorTreeNode> Nodes { get; set; }

	public NavigatorTreeNodeType8X Type { get; set; }

	public string ActivationCondition { get; set; }

	public bool IsHidden { get; set; }
}
