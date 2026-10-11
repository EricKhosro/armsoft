using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.TreeNodes;

public class TreeNodeDeleteSelectedRequestModel
{
	public string TreeId { get; set; }

	public List<string> Codes { get; set; }

	public bool WithoutValidation { get; set; }
}
