using System.Collections.Generic;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Common.Models.TemplateSubstitution;

public class PrintableElementSubstituteResponseWithFile
{
	public PrintTemplateFileFormatSE FileFormat { get; set; }

	public byte[] File { get; set; }

	public List<ExceptionModel> Exceptions { get; set; }
}
