using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient.Core;

public class FriendLoanManager : BaseManager
{
	internal FriendLoanManager(Client client)
		: base(client)
	{
	}

	public async Task<FCContract> GetFriendLoanContractAsync(string contractNumber, string passportNumber, FriendCreditSearchType searchType = FriendCreditSearchType.ContractCode)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["ContractNo"] = contractNumber,
			["PassportNo"] = passportNumber,
			["SearchType"] = searchType
		};
		return await client.GetAsync<FCContract>("v2/payments/fc/contractDetails", requireAuthorization: true, null, queryParams);
	}
}
