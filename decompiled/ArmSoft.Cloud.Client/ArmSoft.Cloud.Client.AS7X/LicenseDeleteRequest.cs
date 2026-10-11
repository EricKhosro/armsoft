using System;

namespace ArmSoft.Cloud.Client.AS7X;

public class LicenseDeleteRequest
{
	public Guid LicenseId { get; set; }

	public string Key { get; set; }
}
