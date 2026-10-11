using System;
using System.Threading.Tasks;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class RateManager(ApiClient client) : BaseManager(client)
{
	public async Task<Rate> GetRate(string currency1, string currency2, DealingRateType rateType = DealingRateType.NonCash, DateTime? date = null)
	{
		BankRateParams postBody = new BankRateParams
		{
			Currency1 = currency1,
			Currency2 = currency2,
			RateType = rateType,
			Date = date
		};
		return await client.Post<Rate>("rates/v1/get-rate", requireAuthorization: true, null, postBody);
	}
}
