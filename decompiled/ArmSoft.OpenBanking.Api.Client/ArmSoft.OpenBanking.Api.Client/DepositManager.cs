using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class DepositManager(ApiClient client) : BaseManager(client)
{
	public async Task<IEnumerable<Deposit>> GetDeposits(DepositAccessLevels desiredAccess = DepositAccessLevels.View, bool onlyActive = true)
	{
		GetDepositsParams postBody = new GetDepositsParams
		{
			DesiredAccess = desiredAccess,
			OnlyActive = onlyActive
		};
		return await client.Post<IEnumerable<Deposit>>("deposits/v2/get-list", requireAuthorization: true, null, postBody);
	}

	public async Task<ContractOnlineDataListResult<DepositOnlineData>> GetDepositsOnlineDataList(List<int> ids, DepositAccessLevels desiredAccess, bool includeSummary, bool onlyActive = true)
	{
		GetDepositOnlineDataListParams postBody = new GetDepositOnlineDataListParams
		{
			Ids = ids,
			DesiredAccess = desiredAccess,
			OnlyActive = onlyActive,
			IncludeSummary = includeSummary
		};
		return await client.Post<ContractOnlineDataListResult<DepositOnlineData>>("deposits/v3/get-online-data-list", requireAuthorization: true, null, postBody);
	}
}
