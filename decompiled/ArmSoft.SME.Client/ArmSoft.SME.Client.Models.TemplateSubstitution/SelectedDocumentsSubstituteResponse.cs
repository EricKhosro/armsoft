using System.Collections.Generic;

namespace ArmSoft.SME.Client.Models.TemplateSubstitution;

public class SelectedDocumentsSubstituteResponse
{
	public string FileId { get; set; }

	public List<DocumentSubstituteResponse> Exceptions { get; set; }
}
