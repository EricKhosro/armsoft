using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Models.TreeNodes;

public class TreeNodeStoreRequestModel
{
	public POStoreRequest StoreRequest { get; set; }

	public string JSONDocument { get; set; }
}
