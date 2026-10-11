using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Dialog;

public class DialogValuesTemplatesUpdateRequest
{
	public string Group { get; set; }

	public List<DialogValuesTemplate> Templates { get; set; }
}
