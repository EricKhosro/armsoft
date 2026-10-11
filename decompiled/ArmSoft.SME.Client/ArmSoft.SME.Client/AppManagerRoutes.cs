using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models.AppManager;

namespace ArmSoft.SME.Client;

public class AppManagerRoutes
{
	private readonly ApiClient _client;

	protected virtual string ControllerName => "AppManager";

	public AppManagerRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<UIVersionResponse> GetUIRecommendedVersionAsync(string idToken, int dbId, CancellationToken cancellationToken = default(CancellationToken))
	{
		UIVersionRequest requestObject = new UIVersionRequest
		{
			IdToken = idToken,
			DbId = dbId
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetUIRecommendedVersion");
		return _client.ExecuteAsync<UIVersionResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public UIVersionResponse GetUIRecommendedVersion(string idToken, int dbId)
	{
		return AsyncHelper.RunSync(async () => await GetUIRecommendedVersionAsync(idToken, dbId));
	}

	public string CheckDbUpgrade()
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/CheckDbUpgrade");
		return _client.Execute<string>(HttpMethod.Get, stringBuilder.ToString());
	}

	public Task CheckDbVersionCompatibilityAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/CheckDbVersionCompatibility");
		return _client.ExecuteAsync(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public void CheckDbVersionCompatibility()
	{
		AsyncHelper.RunSync(async () =>
		{
			await CheckDbVersionCompatibilityAsync();
		});
	}

	public void RefreshDbState()
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/RefreshDbState");
		_client.Execute(HttpMethod.Post, stringBuilder.ToString());
	}

	public void ChangeDbState(bool operational, string logComment = null)
	{
		ChangeDbStateRequest requestObject = new ChangeDbStateRequest
		{
			Operational = operational,
			LogComment = logComment
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/ChangeDbState");
		_client.Execute(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public void DeleteJobs()
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/DeleteJobs");
		_client.Execute(HttpMethod.Post, stringBuilder.ToString());
	}
}
