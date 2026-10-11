using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class TemplateManager : BaseManager
{
	internal TemplateManager(Client client)
		: base(client)
	{
	}

	public async Task UpdateIndexesAsync(TemplatesIndexesParams indexesParams)
	{
		await client.PostAsync<List<UserAccountIndex>>("templates/updateIndexes", requireAuthorization: true, null, null, indexesParams);
	}
}
