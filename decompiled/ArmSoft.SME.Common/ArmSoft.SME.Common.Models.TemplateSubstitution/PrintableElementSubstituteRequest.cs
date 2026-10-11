using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.TemplateSubstitution;

public class PrintableElementSubstituteRequest
{
	public List<string> JSONElements { get; set; }

	public string TemplateName { get; set; }

	public string OutputFileFormat { get; set; }

	public bool PrintASInfo { get; set; }

	public bool PrintDate { get; set; }
}
