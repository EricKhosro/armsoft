namespace ArmSoft.OpenBanking.Api.Common;

public class SeqLogFilterParams : BaseReportParams
{
	public string? ServiceName { get; set; }

	public string? AdditionalFilter { get; set; }

	public string? Level { get; set; }

	public int Count { get; set; } = 100;
}
