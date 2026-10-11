using System.Collections.Generic;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Client.Models.TemplateSubstitution;

public class SelectedDocumentsSubstituteResponseWithFile
{
	public PrintTemplateFileFormatSE FileFormat { get; set; }

	public byte[] File { get; set; }

	public List<DocumentSubstituteResponse> Exceptions { get; set; }
}
