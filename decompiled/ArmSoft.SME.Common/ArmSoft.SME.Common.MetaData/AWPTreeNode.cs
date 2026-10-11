using System.Collections.Generic;

namespace ArmSoft.SME.Common.MetaData;

public class AWPTreeNode
{
	public int Order { get; set; }

	public AWPNode TreeNode { get; set; }

	public List<AWPTreeNode> AWPTreeNodes { get; set; } = new List<AWPTreeNode>();
}
