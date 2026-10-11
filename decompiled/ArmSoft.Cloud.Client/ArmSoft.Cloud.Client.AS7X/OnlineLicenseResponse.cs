using System;

namespace ArmSoft.Cloud.Client.AS7X;

internal class OnlineLicenseResponse
{
	public Guid LicenseId { get; set; }

	public string Message { get; set; }

	public int Status { get; set; }
}
