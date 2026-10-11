using System.Collections.Generic;

namespace ArmSoft.SME.Client.Models.NavigatorTree;

public class NavigatorTreeResult
{
	public List<NavigatorTree> AvailableNavigatorTrees { get; set; }

	public string Default { get; set; }
}
