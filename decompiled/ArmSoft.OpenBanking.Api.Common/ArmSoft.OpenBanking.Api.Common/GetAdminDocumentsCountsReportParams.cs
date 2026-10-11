namespace ArmSoft.OpenBanking.Api.Common;

public class GetAdminDocumentsCountsReportParams : BaseReportParams
{
	public string? Type { get; set; }

	public DateGroupingType DateGroupingType { get; set; }

	public int? DateGroupingUnit { get; set; }
}
