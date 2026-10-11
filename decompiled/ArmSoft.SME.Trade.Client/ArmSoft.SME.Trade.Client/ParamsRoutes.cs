using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Trade.Client.Models;

namespace ArmSoft.SME.Trade.Client;

public class ParamsRoutes
{
	private readonly TradeClient client;

	internal ParamsRoutes(TradeClient client)
	{
		this.client = client;
	}

	public Task<CompanyDetails> CompanyDetails(CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Params/CompanyDetails");
		return client.ExecuteAsync<CompanyDetails>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Task<GeneralParams> GeneralParams(CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Params/GeneralParams");
		return client.ExecuteAsync<GeneralParams>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Task<UsersGeneralOptions> UsersGeneralOptions(CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Params/UsersGeneralOptions");
		return client.ExecuteAsync<UsersGeneralOptions>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Task<UsersWorkRestrictions> UsersWorkRestrictions(CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Params/UsersWorkRestrictions");
		return client.ExecuteAsync<UsersWorkRestrictions>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}
}
