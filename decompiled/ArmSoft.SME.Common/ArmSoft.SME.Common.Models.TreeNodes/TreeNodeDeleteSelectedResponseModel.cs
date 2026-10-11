using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.TreeNodes;

public class TreeNodeDeleteSelectedResponseModel
{
	public int SuccessCount { get; set; }

	public List<TreeNodeDeleteResponseModel> Exceptions { get; set; }
}
