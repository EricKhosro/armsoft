namespace ArmSoft.SME.Common.Models.TreeNodes;

public class TreeNode : PersistableObject
{
	public string TreeId { get; set; }

	public string Code { get; set; }

	public string Caption { get; set; }

	public string Parent { get; set; }

	public string Path { get; set; }

	public bool Leaf { get; set; }

	public bool IsClosed { get; set; }

	public string Spec { get; set; }
}
