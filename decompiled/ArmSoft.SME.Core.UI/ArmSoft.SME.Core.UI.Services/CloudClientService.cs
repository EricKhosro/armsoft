using ArmSoft.Cloud.Client;

namespace ArmSoft.SME.Core.UI.Services;

public class CloudClientService
{
	private static string _b2bCloudBaseAddress;

	public static string CloudClientBaseAddress
	{
		get
		{
			if (_b2bCloudBaseAddress == null && string.IsNullOrWhiteSpace(_b2bCloudBaseAddress))
			{
				_b2bCloudBaseAddress = "https://cloud.armsoft.am/";
			}
			return _b2bCloudBaseAddress;
		}
	}

	public CloudClient Client { get; private set; }

	public CloudClientService()
	{
		CloudClient client = new CloudClient("", CloudClientBaseAddress)
		{
			UserToken = ADHelper.ADB2CToken
		};
		Client = client;
	}
}
