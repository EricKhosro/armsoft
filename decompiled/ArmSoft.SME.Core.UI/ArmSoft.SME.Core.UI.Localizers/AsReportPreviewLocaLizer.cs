using DevExpress.XtraPrinting.Localization;

namespace ArmSoft.SME.Core.UI.Localizers;

internal class AsReportPreviewLocaLizer : PreviewLocalizer
{
	protected override void PopulateStringTable()
	{
		base.PopulateStringTable();
		AddString(PreviewStringId.ParametersRequest_Submit, "Հաստատել");
		AddString(PreviewStringId.ParametersRequest_Reset, "Վերականգ.");
		AddString(PreviewStringId.ParametersRequest_Caption, "Պարամետրեր");
	}
}
