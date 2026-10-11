using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.Caches;

public class TemplateCache : CacheDefinition<TemplatesCacheRow, string>
{
	public override short CacheId => CacheIdSE.Templates.Value;

	public override bool StoreInLocalStorage => true;

	public IReadOnlyList<TemplatesCacheRow> GetTemplates(short docType, PrintTemplateFileFormatSE fileFormat = null)
	{
		List<TemplatesCacheRow> list = new List<TemplatesCacheRow>();
		list.AddRange(from x in GetList((TemplatesCacheRow p) => p.DocType == docType && (fileFormat == null || p.FileFormat == fileFormat))
			orderby x.Key
			select x);
		return new _003C_003Ez__ReadOnlyList<TemplatesCacheRow>(list);
	}

	public TemplatesCacheRow GetDefaultPrintTemplate(short docType, bool throwExceptionNotExits = false)
	{
		TemplatesCacheRow templatesCacheRow = null;
		string templateName = Settings.ParametersService.GetCurrentUserPrintTemplate(docType);
		IReadOnlyList<TemplatesCacheRow> templates = GetTemplates(docType);
		if (!string.IsNullOrWhiteSpace(templateName))
		{
			templatesCacheRow = templates.FirstOrDefault((TemplatesCacheRow p) => p.Name == templateName);
		}
		if (templatesCacheRow == null)
		{
			templatesCacheRow = ((templates.Count > 0) ? templates[0] : null);
		}
		if (throwExceptionNotExits && templatesCacheRow == null)
		{
			throw new RESTException(Resources.PrintTemplateNotFound);
		}
		return templatesCacheRow;
	}
}
