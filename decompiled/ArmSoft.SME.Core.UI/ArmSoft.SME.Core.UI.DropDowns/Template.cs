using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDownsData;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(9)]
public class Template : DropDownView<TemplateDataProvider.DataRow, TemplateDataProvider.Param>
{
	private readonly short _docType;

	private readonly IReadOnlyList<TemplatesCacheRow> _templates;

	private readonly PrintTemplateFileFormatSE _fileFormat;

	public override string HelpId => "Templates";

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public Template()
	{
		DataProvider = new TemplateDataProvider();
		FillColumnsFromDataProvider();
	}

	public Template(short docType, PrintTemplateFileFormatSE fileFormat = null)
	{
		_docType = docType;
		_fileFormat = fileFormat;
		DataProvider = new TemplateDataProvider();
		FillColumnsFromDataProvider();
	}

	public Template(IReadOnlyList<TemplatesCacheRow> templates)
	{
		_templates = templates;
		DataProvider = new TemplateDataProvider();
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.DocType = _docType;
		ParametersProvider.Templates = _templates;
		ParametersProvider.FileFormat = _fileFormat;
	}

	public override short ValueMemberLength()
	{
		return 20;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Name;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
