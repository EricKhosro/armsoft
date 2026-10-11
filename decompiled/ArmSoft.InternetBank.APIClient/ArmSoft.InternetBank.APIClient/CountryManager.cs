using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class CountryManager : BaseManager
{
	internal CountryManager(Client client)
		: base(client)
	{
	}

	public List<Country> GetCountries()
	{
		return client.Get<List<Country>>("countries", requireAuthorization: true);
	}

	public async Task<List<Country>> GetCountriesAsync()
	{
		return await client.GetAsync<List<Country>>("countries", requireAuthorization: true);
	}
}
