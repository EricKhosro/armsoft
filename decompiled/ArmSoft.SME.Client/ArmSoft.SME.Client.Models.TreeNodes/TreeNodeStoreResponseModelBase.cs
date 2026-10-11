using System.Collections.Generic;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.TreeNodes;

namespace ArmSoft.SME.Client.Models.TreeNodes;

public class TreeNodeStoreResponseModelBase
{
	public string TreeId { get; set; }

	public List<ExceptionModel> Exceptions { get; set; }

	public TreeNode TreeNode { get; set; }
}
