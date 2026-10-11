using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class NewsManager : BaseManager
{
	public NewsManager(Client client)
		: base(client)
	{
	}

	public async Task<List<NewsInfo>> GetPublishedNewsAsync(string menuItem, bool isAlwaysVisible)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object>
		{
			["MenuItem"] = menuItem,
			["IsAlwaysVisible"] = isAlwaysVisible
		};
		return (await client.PostAsync<ResponseResult<List<NewsInfo>>>("v2/news", requireAuthorization: true, null, null, postBody)).Value;
	}
}
