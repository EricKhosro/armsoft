using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class BaseReportParams
{
	public DateTime DateFrom { get; set; }

	public DateTime DateTo { get; set; }

	public string? BankId { get; set; }

	public string? ClientId { get; set; }

	public string? UserId { get; set; }

	public string? ClientName { get; set; }

	public SystemType? SystemType { get; set; }
}
