using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient.Core;

public class GarbageCollectionManager : BaseManager
{
	internal GarbageCollectionManager(Client client)
		: base(client)
	{
	}

	public async Task<List<TaxProperty>> GetGarbageCollectionPropertiesAsync(GarbageCollectionRealitiesCriteria param)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object>
		{
			["Id"] = param.Id,
			["PassportNumber"] = param.PassportNumber,
			["SocialCard"] = param.SocialCard,
			["TaxCode"] = param.TaxCode
		};
		return (await client.PostAsync<ResponseResult<List<TaxProperty>>>("garbageCollectionProperties", requireAuthorization: true, null, null, postBody)).Value;
	}

	public async Task<List<TaxDebt>> GetGarbageCollectionDebtsAsync(GarbageCollectionDebtsCriteria param)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object> { ["Id"] = param.Id };
		return (await client.PostAsync<ResponseResult<List<TaxDebt>>>("garbageCollectionDebts", requireAuthorization: true, null, null, postBody)).Value;
	}
}
