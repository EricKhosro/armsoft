using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class DocumentsCountsReportRow
{
	public string? Type { get; set; }

	public string? BankId { get; set; }

	public string? UserId { get; set; }

	public SystemType? SystemType { get; set; }

	public DateTime? GroupDate { get; set; }

	public int Count { get; set; }

	public int? BillableCount { get; set; }
}
