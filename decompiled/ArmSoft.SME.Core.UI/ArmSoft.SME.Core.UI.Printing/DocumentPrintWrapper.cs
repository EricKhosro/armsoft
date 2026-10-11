using System;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Core.UI.Printing;

public abstract class DocumentPrintWrapper
{
	protected readonly TemplatesCacheRow _template;

	public DocumentPrintWrapper()
	{
	}

	public DocumentPrintWrapper(TemplatesCacheRow template)
	{
		_template = template;
	}

	public TemplatesCacheRow GetTemplate()
	{
		return _template;
	}

	public TemplateSubstitution GetTemplateSubstitution()
	{
		return new TemplateSubstitution(Settings.ParametersService, this);
	}

	public virtual string InitXtraReportDataSource(ref object dataSource)
	{
		throw new NotImplementedException();
	}

	public virtual DocumentPrintWrapper ConstructTemplate()
	{
		throw new NotImplementedException();
	}
}
