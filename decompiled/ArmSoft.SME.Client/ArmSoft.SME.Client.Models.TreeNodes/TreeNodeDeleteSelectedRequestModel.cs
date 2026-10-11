using System.Collections.Generic;

namespace ArmSoft.SME.Client.Models.TreeNodes;

public class TreeNodeDeleteSelectedRequestModel
{
	public List<string> TreeIds { get; set; }

	public List<string> Codes { get; set; }

	public bool ValidateDelete { get; set; }
}
