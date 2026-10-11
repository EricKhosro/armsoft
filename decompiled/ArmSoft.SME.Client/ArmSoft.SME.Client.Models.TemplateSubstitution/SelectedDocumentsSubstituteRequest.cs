using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Client.Models.TemplateSubstitution;

public class SelectedDocumentsSubstituteRequest
{
	public List<Guid> DocIsns { get; set; }

	public string TemplateName { get; set; }

	public PrintTemplateFileFormatSE OutputFileFormat { get; set; }

	public bool InsertPageBreaks { get; set; }

	public bool PrintASInfo { get; set; }

	public bool PrintDate { get; set; }
}
