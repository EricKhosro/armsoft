using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class MerchantManager(ApiClient client) : BaseManager(client)
{
	public async Task<List<Merchant>> GetAccountMerchants(string accountNumber)
	{
		MerchantParams postBody = new MerchantParams
		{
			AccountNumber = accountNumber
		};
		return await client.Post<List<Merchant>>("merchants/v1/get-account-merchants", requireAuthorization: true, null, postBody);
	}

	public async Task<List<MerchantTransaction>> GetMerchantTransactions(DateTime startDate, DateTime endDate, List<string>? merchantIds, List<string>? terminalIds, string? accountNumber = null)
	{
		MerchantTransactionParams postBody = new MerchantTransactionParams
		{
			StartDate = startDate,
			EndDate = endDate,
			MerchantIds = merchantIds,
			TerminalIds = terminalIds,
			AccountNumber = accountNumber
		};
		return await client.Post<List<MerchantTransaction>>("merchants/v1/get-transactions", requireAuthorization: true, null, postBody);
	}
}
