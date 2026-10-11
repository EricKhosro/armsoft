using ArmSoft.SME.Common.PersistableObject;

namespace ArmSoft.SME.Common.TreeNodes;

public class TreeNodeDefinition : PODefinition
{
	public short? NextCodeParamId { get; set; }

	public string TreeId { get; set; }
}
