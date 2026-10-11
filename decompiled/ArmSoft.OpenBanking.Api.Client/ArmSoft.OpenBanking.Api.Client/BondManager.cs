using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class BondManager(ApiClient client) : BaseManager(client)
{
	public async Task<IEnumerable<Bond>> GetBonds(BondAccessLevels desiredAccess, bool onlyActive = true)
	{
		GetBondsParams postBody = new GetBondsParams
		{
			DesiredAccess = desiredAccess,
			OnlyActive = onlyActive
		};
		return await client.Post<IEnumerable<Bond>>("bonds/v2/get-list", requireAuthorization: true, null, postBody);
	}

	public async Task<ContractOnlineDataListResult<BondOnlineData>> GetBondsOnlineDataList(List<int> ids, BondAccessLevels desiredAccess, bool includeSummary, bool onlyActive = true)
	{
		GetBondOnlineDataListParams postBody = new GetBondOnlineDataListParams
		{
			Ids = ids,
			DesiredAccess = desiredAccess,
			OnlyActive = onlyActive,
			IncludeSummary = includeSummary
		};
		return await client.Post<ContractOnlineDataListResult<BondOnlineData>>("bonds/v3/get-online-data-list", requireAuthorization: true, null, postBody);
	}
}
