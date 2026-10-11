using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class CreditManager(ApiClient client) : BaseManager(client)
{
	public async Task<IEnumerable<Credit>> GetCredits(CreditAccessLevels desiredAccess, List<CreditType>? creditTypes = null, List<CreditRelationType>? relationTypes = null, bool onlyActive = true)
	{
		GetCreditsParams getCreditsParams = new GetCreditsParams
		{
			DesiredAccess = desiredAccess,
			CreditTypes = creditTypes,
			OnlyActive = onlyActive
		};
		if (relationTypes != null)
		{
			getCreditsParams.RelationTypes = relationTypes;
		}
		return await client.Post<IEnumerable<Credit>>("credits/v2/get-list", requireAuthorization: true, null, getCreditsParams);
	}

	public async Task<ContractOnlineDataListResult<CreditOnlineData>> GetCreditsOnlineDataList(List<int> ids, CreditAccessLevels desiredAccess, bool includeSummary, List<CreditRelationType>? relationTypes = null, bool onlyActive = true)
	{
		GetCreditOnlineDataListParams getCreditOnlineDataListParams = new GetCreditOnlineDataListParams
		{
			Ids = ids,
			DesiredAccess = desiredAccess,
			OnlyActive = onlyActive,
			IncludeSummary = includeSummary
		};
		if (relationTypes != null)
		{
			getCreditOnlineDataListParams.RelationTypes = relationTypes;
		}
		return await client.Post<ContractOnlineDataListResult<CreditOnlineData>>("credits/v3/get-online-data-list", requireAuthorization: true, null, getCreditOnlineDataListParams);
	}
}
