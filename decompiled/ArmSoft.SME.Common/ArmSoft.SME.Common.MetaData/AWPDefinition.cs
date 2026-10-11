using System.Collections.Generic;

namespace ArmSoft.SME.Common.MetaData;

public class AWPDefinition
{
	public string Name { get; set; }

	public string CaptionId { get; set; }

	public string Caption { get; set; }

	public bool System { get; set; }

	public List<AWPTreeNode> AWPNodes { get; set; }
}
