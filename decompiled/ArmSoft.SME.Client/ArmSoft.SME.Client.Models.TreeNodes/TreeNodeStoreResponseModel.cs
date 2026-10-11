using System.Collections.Generic;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Models.TreeNodes;

public class TreeNodeStoreResponseModel
{
	public string TreeId { get; set; }

	public List<ExceptionModel> Exceptions { get; set; }

	public string JSONDocument { get; set; }
}
