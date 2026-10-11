using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Client.Models.TemplateSubstitution;

public class DocumentSubstituteRequest
{
	public Guid DocIsn { get; set; }

	public string TemplateName { get; set; }

	public PrintTemplateFileFormatSE OutputFileFormat { get; set; }

	public bool PrintASInfo { get; set; }

	public bool PrintDate { get; set; }
}
