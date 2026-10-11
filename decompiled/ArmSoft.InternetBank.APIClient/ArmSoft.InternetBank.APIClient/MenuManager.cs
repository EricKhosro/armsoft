using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class MenuManager : BaseManager
{
	public MenuManager(Client client)
		: base(client)
	{
	}

	public async Task<List<MenuItem>> GetMenuItems()
	{
		return await client.GetAsync<List<MenuItem>>("menu", requireAuthorization: true);
	}
}
