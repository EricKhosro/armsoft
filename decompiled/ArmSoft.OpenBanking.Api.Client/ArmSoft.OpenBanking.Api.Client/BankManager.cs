using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client;

public class BankManager(ApiClient client) : BaseManager(client)
{
	public async Task<IEnumerable<BankInfo>> GetBanks()
	{
		return await client.Post<IEnumerable<BankInfo>>("banks/v1/get-list", requireAuthorization: false);
	}

	public async Task<BankInfo> GetBank(string bankCode)
	{
		GetBankParams postBody = new GetBankParams
		{
			Code = bankCode
		};
		return await client.Post<BankInfo>("banks/v1/get", requireAuthorization: false, null, postBody);
	}

	public async Task<BankInfo> GetCurrentBank()
	{
		return await client.Post<BankInfo>("banks/v1/get-current", requireAuthorization: true);
	}
}
