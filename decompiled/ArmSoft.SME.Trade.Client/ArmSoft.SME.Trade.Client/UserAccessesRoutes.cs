using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Trade.Client.Models;

namespace ArmSoft.SME.Trade.Client;

public class UserAccessesRoutes
{
	private readonly TradeClient _client;

	internal UserAccessesRoutes(TradeClient client)
	{
		_client = client;
	}

	public Task<UserAccessesDetails> UserAccessesDetails(CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UserAccess/UserAccessesDetailes");
		return _client.ExecuteAsync<UserAccessesDetails>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Task<List<CodeName>> AvailableDocumentTypes(string docTypes = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docTypes", docTypes);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UserAccess/AvailableDocumentTypes", urlQuery);
		return _client.ExecuteAsync<List<CodeName>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}
}
