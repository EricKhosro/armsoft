using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DataViews;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(13)]
public class OnlineReportTemplates : DropDownView<TemplatesDataProvider.DataRow, TemplatesDataProvider.Param>
{
	public override string Caption => Resources.OnlineReportsTemplates;

	public OnlineReportTemplates()
	{
		DataProvider = new TemplatesDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
		Columns["File"].Visible = false;
		Columns["DocType"].Visible = false;
		Columns["FileFormat"].Visible = false;
		Columns["IsValidFileExtension"].Visible = false;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.TemplateType = PrintTemplateTypeSE.OnlineReport;
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
