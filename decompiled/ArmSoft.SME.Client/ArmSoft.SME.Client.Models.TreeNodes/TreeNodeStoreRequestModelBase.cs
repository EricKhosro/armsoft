using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.TreeNodes;

namespace ArmSoft.SME.Client.Models.TreeNodes;

public class TreeNodeStoreRequestModelBase
{
	public POStoreRequest StoreRequest { get; set; }

	public TreeNode TreeNode { get; set; }
}
