using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class BondManager : BaseManager
{
	internal BondManager(Client client)
		: base(client)
	{
	}

	public List<Bond> GetBonds(BondAccessLevels desiredAccess = BondAccessLevels.View)
	{
		BondsParams postBody = new BondsParams
		{
			DesiredAccess = desiredAccess
		};
		return client.Post<List<Bond>>("bonds", requireAuthorization: true, null, null, postBody);
	}

	public async Task<List<Bond>> GetBondsAsync(BondAccessLevels desiredAccess = BondAccessLevels.View)
	{
		BondsParams postBody = new BondsParams
		{
			DesiredAccess = desiredAccess
		};
		return await client.PostAsync<List<Bond>>("bonds", requireAuthorization: true, null, null, postBody);
	}

	public Bond GetBondDetails(string contractNumber)
	{
		BondDetailsParams postBody = new BondDetailsParams
		{
			ContractNumber = contractNumber
		};
		return client.Post<Bond>("bonds/details", requireAuthorization: true, null, null, postBody);
	}

	public async Task<Bond> GetBondDetailsAsync(string contractNumber)
	{
		BondDetailsParams postBody = new BondDetailsParams
		{
			ContractNumber = contractNumber
		};
		return await client.PostAsync<Bond>("bonds/details", requireAuthorization: true, null, null, postBody);
	}

	public List<BondSchedule> GetBondSchedule(string contractNumber, int? count = null)
	{
		BondScheduleParams postBody = new BondScheduleParams
		{
			ContractNumber = contractNumber,
			Count = count
		};
		return client.Post<List<BondSchedule>>("bonds/schedule", requireAuthorization: true, null, null, postBody);
	}

	public async Task<List<BondSchedule>> GetBondScheduleAsync(string contractNumber, int? count = null)
	{
		BondScheduleParams postBody = new BondScheduleParams
		{
			ContractNumber = contractNumber,
			Count = count
		};
		return await client.PostAsync<List<BondSchedule>>("bonds/schedule", requireAuthorization: true, null, null, postBody);
	}
}
