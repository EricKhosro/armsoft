using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.TaxService;

namespace ArmSoft.SME.Core.UI.Services;

public class TaxService
{
	public TaxServiceClient Client { get; private set; }

	public static bool AuthenticationCompleted { get; set; }

	public static bool AuthenticateWithLoginWindow { get; set; }

	public void Authenticate(string tin, string login, string password)
	{
		Client = new TaxServiceClient(tin, login, password, ConfigHelper.EInvoicingConnectToTest);
	}

	public void Authenticate()
	{
		if (Client == null)
		{
			TaxServiceClient taxServiceClient = (Client = new TaxServiceClient());
		}
	}

	public void Dispose()
	{
		Client?.Dispose();
	}
}
