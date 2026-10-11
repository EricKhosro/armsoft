using System.Collections.Generic;

namespace ArmSoft.Cloud.Client.AS7X;

public class OnlineLicenseInfoWithInstances : OnlineLicenseInfo
{
	public List<OnlineLicenseInstance> Instances { get; set; }
}
