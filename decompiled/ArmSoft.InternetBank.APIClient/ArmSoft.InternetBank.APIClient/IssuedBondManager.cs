using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class IssuedBondManager : BaseManager
{
	internal IssuedBondManager(Client client)
		: base(client)
	{
	}

	public List<IssuedBond> GetIssuedBonds(string currency, BondIssuanceType issuedBondType)
	{
		IssuedBondParams postBody = new IssuedBondParams
		{
			Currency = currency,
			IssuedBondType = issuedBondType
		};
		return client.Post<List<IssuedBond>>("issuedBonds", requireAuthorization: true, null, null, postBody);
	}

	public async Task<List<IssuedBond>> GetIssuedBondsAsync(string currency, BondIssuanceType issuedBondType)
	{
		IssuedBondParams postBody = new IssuedBondParams
		{
			Currency = currency,
			IssuedBondType = issuedBondType
		};
		return await client.PostAsync<List<IssuedBond>>("issuedBonds", requireAuthorization: true, null, null, postBody);
	}

	public IssuedBondPrice GetIssuedBondPrice(string issuanceNumber)
	{
		IssuedBondPriceParams postBody = new IssuedBondPriceParams
		{
			IssuanceNumber = issuanceNumber
		};
		return client.Post<IssuedBondPrice>("issuedBondPrice", requireAuthorization: true, null, null, postBody);
	}
}
