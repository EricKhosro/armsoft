using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Routes;

public class ApplicationTokenRoutes
{
	private readonly ApiClient _client;

	internal ApplicationTokenRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<string> GetApplicationTokenAsync(string fingerprint, CancellationToken cancellationToken = default(CancellationToken))
	{
		ApplicationTokenRequest requestObject = new ApplicationTokenRequest
		{
			Fingerprint = fingerprint
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/ApplicationToken");
		return _client.ExecuteAsync<string>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public string GetApplicationToken(string fingerprint)
	{
		return AsyncHelper.RunSync(async () => await GetApplicationTokenAsync(fingerprint));
	}
}
