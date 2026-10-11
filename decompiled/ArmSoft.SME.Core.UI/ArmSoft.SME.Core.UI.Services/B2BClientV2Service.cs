using ArmSoft.b2b.V2;

namespace ArmSoft.SME.Core.UI.Services;

public class B2BClientV2Service
{
	private static string _b2bServiceBaseAddress;

	public B2BClient Client { get; private set; }

	public static string B2BServiceBaseAddress
	{
		get
		{
			if (_b2bServiceBaseAddress == null && string.IsNullOrWhiteSpace(_b2bServiceBaseAddress))
			{
				_b2bServiceBaseAddress = "https://services.armsoft.am:4443/";
			}
			return _b2bServiceBaseAddress;
		}
	}

	public B2BClientV2Service()
	{
		string baseAddress = B2BOAuth2Helper.GetBaseAddress(B2BServiceBaseAddress);
		Client = new B2BClient(Settings.GetApplicationToken(), baseAddress);
	}
}
