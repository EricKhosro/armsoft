using System.IO;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.Services;

public class TemplateService : ITemplateService
{
	protected virtual ApiClient Client => Settings.CreateApiClient();

	public void CopyFile(string source, string destination, string templateType)
	{
		Client.TemplateRoutes.CopyFile(source, destination, templateType);
		Settings.CoreCacheService.Template.Refresh();
	}

	public void Delete(string code)
	{
		Client.TemplateRoutes.Delete(code);
		Settings.CoreCacheService.Template.Refresh();
	}

	public void ImportFile(string name, string templateType, string fileName)
	{
		Client.TemplateRoutes.ImportFile(name, templateType, fileName, File.ReadAllBytes(fileName));
		Settings.CoreCacheService.Template.Refresh();
	}

	public Template Load(string code)
	{
		return Client.TemplateRoutes.Load(code);
	}

	public byte[] ExportFile(string name, string templateType)
	{
		return Client.TemplateRoutes.ExportFile(name, templateType);
	}

	public virtual Template Store(Template template)
	{
		Template result = Client.TemplateRoutes.Store(template);
		Settings.CoreCacheService.Template.Refresh();
		return result;
	}

	public virtual bool PrintASInfo(short printModelType)
	{
		return true;
	}

	public virtual object InitDesignerByDocumentType(short printModelType, out string displayName)
	{
		displayName = null;
		return null;
	}

	public (PrintTemplateFileFormatSE FileFormat, byte[] File) LoadTemplateFileOffline(string name)
	{
		return (FileFormat: PrintTemplateFileFormatSE.XML, File: File.ReadAllBytes(Path.Combine(Settings.LocalTemplatesPath, name + ".xml")));
	}
}
