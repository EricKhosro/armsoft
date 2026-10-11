using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.DPR;

public class CheckDefinitionsRequest
{
	public List<string> DefinitionTypes { get; set; }

	public short SysType { get; set; }
}
