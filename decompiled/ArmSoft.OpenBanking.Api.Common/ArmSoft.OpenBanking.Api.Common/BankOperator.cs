using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class BankOperator
{
	public int Id { get; set; }

	public string Username { get; set; } = string.Empty;

	public string Ip { get; set; } = string.Empty;

	public string BankCode { get; set; } = string.Empty;

	public bool IsPasswordTemporary { get; set; }

	public bool IsDeleted { get; set; }

	public int FailedLoginAttempts { get; set; }

	public DateTime? LockoutEndUtc { get; set; }

	public DateTime? LastFailedLoginUtc { get; set; }

	public DateTime? LastLoginUtc { get; set; }

	public DateTime? LastPasswordChangedUtc { get; set; }
}
