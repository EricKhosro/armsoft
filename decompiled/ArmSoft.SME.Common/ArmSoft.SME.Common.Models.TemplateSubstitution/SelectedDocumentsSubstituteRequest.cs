using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.TemplateSubstitution;

public class SelectedDocumentsSubstituteRequest
{
	public List<Guid> DocIsns { get; set; }

	public string TemplateName { get; set; }

	public string OutputFileFormat { get; set; }

	public bool InsertPageBreaks { get; set; }
}
