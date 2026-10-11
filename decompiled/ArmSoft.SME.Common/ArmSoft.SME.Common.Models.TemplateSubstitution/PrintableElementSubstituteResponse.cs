using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.TemplateSubstitution;

public class PrintableElementSubstituteResponse
{
	public string FileId { get; set; }

	public List<ExceptionModel> Exceptions { get; set; }
}
