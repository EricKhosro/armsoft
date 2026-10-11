using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArmSoft.SME.Core.AddIn;

public class AuthenticationClient
{
	private readonly ApiClientBase _apiClient;

	public AuthenticationClient(ApiClientBase apiClient)
	{
		_apiClient = apiClient;
	}

	public Task<AuthenticateResponse> AuthenticateB2CAsync(string adb2cToken, int dbId, string appVersion, bool demoMode, CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = _apiClient.CreateURLBuilder("/api/Login");
		return _apiClient.ExecuteAsync<AuthenticateResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, new AuthenticateRequest
		{
			IdToken = adb2cToken,
			DBId = dbId,
			AppType = 4,
			AppVersion = appVersion,
			CompName = Environment.MachineName,
			DemoMode = demoMode
		});
	}

	public AuthenticateResponse AuthenticateB2C(string adb2cToken, int dbId, string appVersion, bool demoMode)
	{
		return AsyncHelper.RunSync(async () => await AuthenticateB2CAsync(adb2cToken, dbId, appVersion, demoMode, CancellationToken.None));
	}

	public Task<AuthenticateResponse> AuthenticateACSAsync(string adToken, int dbId, CancellationToken cancellationToken)
	{
		StringBuilder stringBuilder = _apiClient.CreateURLBuilder("/api/Login/ACS");
		return _apiClient.ExecuteAsync<AuthenticateResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, new AuthenticateACSRequest
		{
			ADToken = adToken,
			DBId = dbId,
			CompName = Environment.MachineName
		});
	}

	public AuthenticateResponse AuthenticateACS(string adToken, int dbId)
	{
		return AsyncHelper.RunSync(async () => await AuthenticateACSAsync(adToken, dbId, CancellationToken.None));
	}
}
