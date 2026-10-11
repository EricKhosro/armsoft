using CBA.ExchangeRates;

namespace ArmSoft.SME.Core.UI.Services;

public class CBAService
{
	public GateSoapClient Client { get; private set; }

	public CBAService()
	{
		Client = new GateSoapClient(GateSoapClient.EndpointConfiguration.GateSoap12);
	}
}
