using System.Collections.Generic;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Client.Models.TemplateSubstitution;

public class DocumentSubstituteResponseWithFile
{
	public PrintTemplateFileFormatSE FileFormat { get; set; }

	public byte[] File { get; set; }

	public byte[] PageSettings { get; set; }

	public List<ExceptionModel> Exceptions { get; set; }

	public DocumentSubstituteResponseWithFile()
	{
	}

	public DocumentSubstituteResponseWithFile(PrintTemplateFileFormatSE fileFormat, byte[] file, byte[] pageSettings)
	{
		FileFormat = fileFormat;
		File = file;
		PageSettings = pageSettings;
	}
}
