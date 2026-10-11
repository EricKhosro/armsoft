using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class MapPointManager : BaseManager
{
	internal MapPointManager(Client client)
		: base(client)
	{
	}

	public async Task<List<MapPoint>> GetMapPoints(string typeCode = null)
	{
		Dictionary<string, string> headers = new Dictionary<string, string> { ["typeCode"] = typeCode };
		return await client.GetAsync<List<MapPoint>>("mappoints", requireAuthorization: false, null, null, headers);
	}

	public async Task<MapPoint> GetMapPointDetails(int? id = null)
	{
		return await client.GetAsync<MapPoint>($"mappoints/{id}/details", requireAuthorization: false);
	}

	public async Task<List<MapPointType>> GetMapPointTypes()
	{
		return await client.GetAsync<List<MapPointType>>("mappoints/types", requireAuthorization: false);
	}
}
