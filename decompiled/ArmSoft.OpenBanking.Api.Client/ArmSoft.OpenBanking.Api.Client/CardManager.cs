using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class CardManager(ApiClient client) : BaseManager(client)
{
	public async Task<IEnumerable<Card>> GetCards(bool includeAccountBalances)
	{
		return await client.Post<IEnumerable<Card>>("cards/v1/get-list", requireAuthorization: true, null, new GetCardParams
		{
			IncludeAccountBalances = includeAccountBalances
		});
	}

	public async Task<CardOnlineBalance> GetCardOnlineBalance(string cardId)
	{
		return await client.Post<CardOnlineBalance>("cards/v1/get-online-balance", requireAuthorization: true, null, new GetCardOnlineBalanceParams
		{
			Id = cardId
		});
	}
}
