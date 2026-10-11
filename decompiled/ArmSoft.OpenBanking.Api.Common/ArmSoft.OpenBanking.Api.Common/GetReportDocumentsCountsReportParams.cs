using System;
using System.Collections.Generic;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetReportDocumentsCountsReportParams
{
	public DateTime DateFrom { get; set; }

	public DateTime DateTo { get; set; }

	public string? ClientId { get; set; }

	public string? UserId { get; set; }

	public string? ClientName { get; set; }

	public bool GroupByUserId { get; set; }

	public bool GroupBySystemType { get; set; }

	public List<SystemType>? SystemTypes { get; set; } = new List<SystemType>();

	public bool GroupByDocumentType { get; set; }

	public List<string>? DocumentTypes { get; set; } = new List<string>();

	public DateGroupingType DateGroupingType { get; set; }

	public int? DateGroupingUnit { get; set; }
}
