using System;

namespace ArmSoft.Cloud.Client.AS7X;

internal class LicenseRenewalRequest
{
	public Guid LicenseId { get; set; }

	public int ProcessId { get; set; }
}
