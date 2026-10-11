using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Authentication;

namespace ArmSoft.SME.Client;

public class AuthenticationClient
{
	private readonly ApiClient _client;

	public AuthenticationClient(ApiClient apiClient)
	{
		_client = apiClient;
	}

	public Task<AuthenticateResponse> AuthenticateACSAsync(string adToken, int dbId, string compName, AppType appType, CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Login/ACS");
		return _client.ExecuteAsync<AuthenticateResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, new AuthenticateACSRequest
		{
			ADToken = adToken,
			DBId = dbId,
			CompName = compName,
			AppType = appType
		});
	}

	public AuthenticateResponse AuthenticateACS(string adToken, int dbId, string compName, AppType appType = AppType.ACS)
	{
		return AsyncHelper.RunSync(async () => await AuthenticateACSAsync(adToken, dbId, compName, appType, CancellationToken.None));
	}

	public Task<AuthenticateResponse> AuthenticateB2CAsync(string adb2cToken, int dbId, AppType appType, string appVersion, string compName, int processId, Guid? compId = null, string baseDirectory = null, string osVersion = null, string netVersion = null, bool demoMode = false, string displayResolution = null, string displayScale = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Login");
		return _client.ExecuteAsync<AuthenticateResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, new AuthenticateRequest
		{
			IdToken = adb2cToken,
			DBId = dbId,
			AppType = appType,
			AppVersion = appVersion,
			CompName = compName,
			ProcessId = processId,
			CompId = (compId ?? Guid.Empty),
			BaseDirectory = baseDirectory,
			OSVersion = osVersion,
			DotNetVersion = netVersion,
			DemoMode = demoMode,
			DisplayResolution = displayResolution,
			DisplayScale = displayScale
		});
	}

	public AuthenticateResponse AuthenticateB2C(string adb2cToken, int dbId, AppType appType, string appVersion, string compName, int processId, Guid? compId = null, string baseDirectory = null, string osVersion = null, string netVersion = null, bool demoMode = false, string displayResolution = null, string displayScale = null)
	{
		return AsyncHelper.RunSync(async () => await AuthenticateB2CAsync(adb2cToken, dbId, appType, appVersion, compName, processId, compId, baseDirectory, osVersion, netVersion, demoMode, displayResolution, displayScale, CancellationToken.None));
	}

	public Task<AuthenticateResponse> AuthenticateDemoB2CAsync(AuthenticateDemoRequest request, CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Login/Demo");
		return _client.ExecuteAsync<AuthenticateResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public Task<AuthenticateResponse> AuthenticateApiKeyAsync(AuthenticateApiKeyRequest request, CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Login/ApiKey");
		return _client.ExecuteAsync<AuthenticateResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public Task<AuthenticatePublicApiResponse> AuthenticatePublicApiKeyAsync(AuthenticatePublicApiRequest request, CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Login/PublicApiKey");
		return _client.ExecuteAsync<AuthenticatePublicApiResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}
}
