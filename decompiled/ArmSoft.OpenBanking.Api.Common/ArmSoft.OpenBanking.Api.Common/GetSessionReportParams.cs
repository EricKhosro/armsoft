using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetSessionReportParams : BaseReportParams
{
	public Guid? SessionId { get; set; }
}
