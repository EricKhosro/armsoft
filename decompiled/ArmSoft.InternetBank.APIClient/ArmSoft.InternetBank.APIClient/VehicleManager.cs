using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class VehicleManager : BaseManager
{
	internal VehicleManager(Client client)
		: base(client)
	{
	}

	public async Task<List<Vehicle>> GetVehiclesAsync()
	{
		return await client.GetAsync<List<Vehicle>>("vehicles", requireAuthorization: true);
	}

	public async Task<Vehicle> GetVehicleDetailsAsync(int vehicleId)
	{
		return await client.GetAsync<Vehicle>($"vehicle/{vehicleId}", requireAuthorization: true);
	}

	public async Task SaveVehicleAsync(Vehicle vehicle, bool ignoreWarnings)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["IgnoreWarnings"] = ignoreWarnings };
		await client.PostAsync("vehicles/save", requireAuthorization: true, null, queryParams, vehicle);
	}

	public async Task DeleteVehicleAsync(Vehicle vehicle)
	{
		await client.PostAsync("vehicles/delete", requireAuthorization: true, null, null, vehicle);
	}
}
