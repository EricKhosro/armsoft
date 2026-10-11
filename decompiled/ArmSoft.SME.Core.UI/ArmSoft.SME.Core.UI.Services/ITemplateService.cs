using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.Services;

public interface ITemplateService
{
	Template Store(Template template);

	Template Load(string code);

	void CopyFile(string source, string destination, string templateType);

	void Delete(string code);

	bool PrintASInfo(short printModelType);

	object InitDesignerByDocumentType(short printModelType, out string displayName);

	void ImportFile(string name, string templateType, string fileName);

	byte[] ExportFile(string name, string templateType);

	(PrintTemplateFileFormatSE FileFormat, byte[] File) LoadTemplateFileOffline(string name);
}
