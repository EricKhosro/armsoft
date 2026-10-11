using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Core.AddIn.Client.Models;

namespace ArmSoft.SME.Core.AddIn;

public class AdminCloudRoutes
{
	private readonly ApiClient _client;

	private static string ControllerName => "AdminCloud";

	public AdminCloudRoutes(ApiClient client)
	{
		_client = client;
	}

	public List<Database> GetAvailableDatabases(string adToken, short? contractType = null, int? serverId = null)
	{
		return AsyncHelper.RunSync(async () => await GetAvailableDatabasesAsync(adToken, contractType, serverId));
	}

	public Task<List<Database>> GetAvailableDatabasesAsync(string adToken, short? contractType = null, int? serverId = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		AvailableDatabasesRequest requestObject = new AvailableDatabasesRequest
		{
			AdToken = adToken,
			ContractType = contractType,
			ServerId = serverId
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetAvailableDatabases");
		return _client.ExecuteAsync<List<Database>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public DatabaseInfo GetDatabaseInfo(string adToken, int databaseId)
	{
		return AsyncHelper.RunSync(async () => await GetDatabaseInfoAsync(adToken, databaseId));
	}

	public Task<DatabaseInfo> GetDatabaseInfoAsync(string adToken, int databaseId, CancellationToken cancellationToken = default(CancellationToken))
	{
		DatabaseInfoRequest requestObject = new DatabaseInfoRequest
		{
			AdToken = adToken,
			DatabaseId = databaseId
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetDatabaseInfo");
		return _client.ExecuteAsync<DatabaseInfo>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<DatabaseInfo> GetCloudDatabases(string adToken, int? databaseId = null)
	{
		return AsyncHelper.RunSync(async () => await GetCloudDatabasesAsync(adToken, databaseId));
	}

	public Task<List<DatabaseInfo>> GetCloudDatabasesAsync(string adToken, int? databaseId = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		DatabaseInfoRequest requestObject = new DatabaseInfoRequest
		{
			AdToken = adToken,
			DatabaseId = databaseId
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetCloudDatabases");
		return _client.ExecuteAsync<List<DatabaseInfo>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<CloudUserLoginInfo> GetCloudUsersInfo(string adToken, string email)
	{
		return AsyncHelper.RunSync(async () => await GetCloudUsersInfoAsync(adToken, email));
	}

	public Task<List<CloudUserLoginInfo>> GetCloudUsersInfoAsync(string adToken, string email, CancellationToken cancellationToken = default(CancellationToken))
	{
		CloudUserInfoRequest requestObject = new CloudUserInfoRequest
		{
			AdToken = adToken,
			Email = email
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetCloudUsersInfo");
		return _client.ExecuteAsync<List<CloudUserLoginInfo>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}
}
