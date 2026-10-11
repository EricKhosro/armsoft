using System;

namespace ArmSoft.SME.Common.Models.TemplateSubstitution;

public class DocumentSubstituteRequest
{
	public Guid DocIsn { get; set; }

	public string TemplateName { get; set; }

	public string OutputFileFormat { get; set; }
}
