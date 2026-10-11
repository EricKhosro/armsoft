using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Client;

namespace ArmSoft.SME.Trade.UI.Services;

public class TradeClientService
{
	public TradeClient Client { get; }

	public TradeClientService(LoginService loginService)
	{
		Settings.ThrowExceptionIfOffline();
		Client = new TradeClient(loginService.ServiceAddress, ApplicationServiceConfig.Token, Settings.HttpClient, null);
	}
}
