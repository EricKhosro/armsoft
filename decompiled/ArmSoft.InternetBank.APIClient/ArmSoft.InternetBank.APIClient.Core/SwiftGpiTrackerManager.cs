using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient.Core;

public class SwiftGpiTrackerManager : BaseManager
{
	internal SwiftGpiTrackerManager(Client client)
		: base(client)
	{
	}

	public async Task<List<SwiftGpiTrackerRow>> GetSwiftGpiTrackerRowsAsync(int docExternalId)
	{
		return await client.GetAsync<List<SwiftGpiTrackerRow>>($"swiftGpiTrackerRows/{docExternalId}", requireAuthorization: true);
	}
}
