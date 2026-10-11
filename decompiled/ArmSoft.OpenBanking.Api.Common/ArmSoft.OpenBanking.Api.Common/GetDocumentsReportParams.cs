using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetDocumentsReportParams : BaseReportParams
{
	public Guid? DocumentId { get; set; }

	public string? Type { get; set; }
}
