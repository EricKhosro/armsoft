using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.TreeNodes;

public class TreeNodeStoreResponseModel
{
	public List<ExceptionModel> Exceptions { get; set; }

	public string JSONDocument { get; set; }
}
