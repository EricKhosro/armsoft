using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient.Core;

public class UtilityDebtManager : BaseManager
{
	internal UtilityDebtManager(Client client)
		: base(client)
	{
	}

	public async Task<UtilityOptions> GetUtilityOptionsAsync(string code)
	{
		return await client.GetAsync<UtilityOptions>($"utilityPayment/{code}/options", requireAuthorization: true);
	}

	public async Task<List<CommunalDebt>> GetUtilityDebtsV2Async(CommunalDebtParams communalDebtParams)
	{
		return await client.PostAsync<List<CommunalDebt>>("v2/communaldebts", requireAuthorization: true, null, null, communalDebtParams);
	}
}
