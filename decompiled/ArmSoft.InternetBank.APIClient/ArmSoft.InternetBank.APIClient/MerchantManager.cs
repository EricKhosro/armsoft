using System;
using System.Collections.Generic;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class MerchantManager : BaseManager
{
	internal MerchantManager(Client client)
		: base(client)
	{
	}

	public List<Merchant> GetMerchants()
	{
		return client.Get<ResponseResult<List<Merchant>>>("merchants", requireAuthorization: true).Value;
	}

	public List<Merchant> GetAccountMerchants(string accountNumber)
	{
		MerchantParams postBody = new MerchantParams
		{
			AccountNumber = accountNumber
		};
		return client.Post<ResponseResult<List<Merchant>>>("merchants/accountMerchants", requireAuthorization: true, null, null, postBody).Value;
	}

	public List<MerchantTransaction> GetTransactions(DateTime startDate, DateTime endDate, List<string> merchantIds, List<string> terminalIds, string accountNumber = null)
	{
		MerchantTransactionParams postBody = new MerchantTransactionParams
		{
			StartDate = startDate,
			EndDate = endDate,
			MerchantIds = merchantIds,
			TerminalIds = terminalIds,
			AccountNumber = accountNumber
		};
		return client.Post<ResponseResult<List<MerchantTransaction>>>("merchants/transactions", requireAuthorization: true, null, null, postBody).Value;
	}
}
