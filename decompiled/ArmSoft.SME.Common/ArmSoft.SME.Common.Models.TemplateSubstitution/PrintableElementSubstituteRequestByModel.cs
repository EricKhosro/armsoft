using System.Collections.Generic;
using ArmSoft.SME.Common.Models.PrintableElements;

namespace ArmSoft.SME.Common.Models.TemplateSubstitution;

public class PrintableElementSubstituteRequestByModel
{
	public List<PrintableElement> Elements { get; set; }

	public string TemplateName { get; set; }

	public string OutputFileFormat { get; set; }

	public bool PrintASInfo { get; set; }

	public bool PrintDate { get; set; }
}
