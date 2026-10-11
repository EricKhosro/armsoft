using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class PurchasedSecurityManager(ApiClient client) : BaseManager(client)
{
	public async Task<List<PurchasedSecurity>> GetPurchasedSecurities()
	{
		return await client.Post<List<PurchasedSecurity>>("purchased-securities/v2/get-list", requireAuthorization: true);
	}
}
