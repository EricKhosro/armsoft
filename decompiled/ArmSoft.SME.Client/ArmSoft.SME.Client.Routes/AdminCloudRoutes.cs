using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models.AppManager;
using ArmSoft.SME.Common.Models.Authentication;
using ArmSoft.SME.Common.Models.Cloud;

namespace ArmSoft.SME.Client.Routes;

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

	public List<Role> GetUserRoles(string adToken, int databaseId, int userId)
	{
		return AsyncHelper.RunSync(async () => await GetUserRolesAsync(adToken, databaseId, userId));
	}

	public Task<List<Role>> GetUserRolesAsync(string adToken, int databaseId, int userId, CancellationToken cancellationToken = default(CancellationToken))
	{
		UserRolesRequest requestObject = new UserRolesRequest
		{
			AdToken = adToken,
			DatabaseId = databaseId,
			UserId = userId
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetUserRoles");
		return _client.ExecuteAsync<List<Role>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<UserRoles> GetUsersRoles(string adToken, int databaseId)
	{
		return AsyncHelper.RunSync(async () => await GetUsersRolesAsync(adToken, databaseId));
	}

	public Task<List<UserRoles>> GetUsersRolesAsync(string adToken, int databaseId, CancellationToken cancellationToken = default(CancellationToken))
	{
		UserRolesRequest requestObject = new UserRolesRequest
		{
			AdToken = adToken,
			DatabaseId = databaseId
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetUsersRoles");
		return _client.ExecuteAsync<List<UserRoles>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void EditUsersRoles(string adToken, int databaseId, List<UserRoles> usersRoles)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditUsersRolesAsync(adToken, databaseId, usersRoles);
		});
	}

	public Task EditUsersRolesAsync(string adToken, int databaseId, List<UserRoles> usersRoles, CancellationToken cancellationToken = default(CancellationToken))
	{
		EditUsersRolesRequest requestObject = new EditUsersRolesRequest
		{
			AdToken = adToken,
			DatabaseId = databaseId,
			UsersRoles = usersRoles
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/EditUsersRoles");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public bool CheckCloudUserExistenceRequest(string adToken, string keySerial, int userId)
	{
		return AsyncHelper.RunSync(async () => await CheckCloudUserExistenceRequestAsync(adToken, keySerial, userId));
	}

	public Task<bool> CheckCloudUserExistenceRequestAsync(string adToken, string keySerial, int userId, CancellationToken cancellationToken = default(CancellationToken))
	{
		CloudUserExistenceRequest requestObject = new CloudUserExistenceRequest
		{
			AdToken = adToken,
			KeySerial = keySerial,
			UserId = userId
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/CheckCloudUserExistenceRequest");
		return _client.ExecuteAsync<bool>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void EditCloudKeyMapping(string adToken, string key, int userId, bool isAdmin, bool loginDisabled, bool isDBAdmin)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditCloudKeyMappingAsync(adToken, key, userId, isAdmin, loginDisabled, isDBAdmin);
		});
	}

	public Task EditCloudKeyMappingAsync(string adToken, string key, int userId, bool isAdmin, bool loginDisabled, bool isDBAdmin, CancellationToken cancellationToken = default(CancellationToken))
	{
		EditCloudKeyMappingRequest requestObject = new EditCloudKeyMappingRequest
		{
			AdToken = adToken,
			Key = key,
			UserId = userId,
			IsAdmin = isAdmin,
			LoginDisabled = loginDisabled,
			IsDBAdmin = isDBAdmin
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/EditCloudKeyMapping");
		return _client.ExecuteAsync(HttpMethod.Put, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void DeleteCloudKeyMapping(string adToken, string key, int userId)
	{
		AsyncHelper.RunSync(async () =>
		{
			await DeleteCloudKeyMappingAsync(adToken, key, userId);
		});
	}

	public Task DeleteCloudKeyMappingAsync(string adToken, string key, int userId, CancellationToken cancellationToken = default(CancellationToken))
	{
		DeleteCloudKeyMappingRequest requestObject = new DeleteCloudKeyMappingRequest
		{
			AdToken = adToken,
			Key = key,
			UserId = userId
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/DeleteCloudKeyMapping");
		return _client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void CloudDatabaseBackUpCreated(string adToken, int databaseId)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CloudDatabaseBackUpCreatedAsync(adToken, databaseId);
		});
	}

	public Task CloudDatabaseBackUpCreatedAsync(string adToken, int databaseId, CancellationToken cancellationToken = default(CancellationToken))
	{
		DatabaseInfoRequest requestObject = new DatabaseInfoRequest
		{
			AdToken = adToken,
			DatabaseId = databaseId
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/CloudDatabaseBackUpCreated");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void CloudDatabaseBackUpDeleted(string adToken, int databaseId)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CloudDatabaseBackUpDeletedAsync(adToken, databaseId);
		});
	}

	public Task CloudDatabaseBackUpDeletedAsync(string adToken, int databaseId, CancellationToken cancellationToken = default(CancellationToken))
	{
		DatabaseInfoRequest requestObject = new DatabaseInfoRequest
		{
			AdToken = adToken,
			DatabaseId = databaseId
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/CloudDatabaseBackUpDeleted");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void AddCloudDatabaseRestoreLog(string adToken, int databaseId)
	{
		AsyncHelper.RunSync(async () =>
		{
			await AddCloudDatabaseRestoreLogAsync(adToken, databaseId);
		});
	}

	public Task AddCloudDatabaseRestoreLogAsync(string adToken, int databaseId, CancellationToken cancellationToken = default(CancellationToken))
	{
		DatabaseInfoRequest requestObject = new DatabaseInfoRequest
		{
			AdToken = adToken,
			DatabaseId = databaseId
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/AddCloudDatabaseRestoreLog");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void AddCloudDatabaseUserLog(CloudDBUserLogCreationRequest request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await AddCloudDatabaseUserLogAsync(request);
		});
	}

	public Task AddCloudDatabaseUserLogAsync(CloudDBUserLogCreationRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/AddCloudDatabaseUserLog");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public UIVersionResponse GetUIRecommendedVersion(string adToken, string key)
	{
		return AsyncHelper.RunSync(async () => await GetUIRecommendedVersionAsync(adToken, key));
	}

	public Task<UIVersionResponse> GetUIRecommendedVersionAsync(string adToken, string key, CancellationToken cancellationToken = default(CancellationToken))
	{
		UIVersionRequestASAdmin requestObject = new UIVersionRequestASAdmin
		{
			AdToken = adToken,
			Key = key
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetUIRecommendedVersion");
		return _client.ExecuteAsync<UIVersionResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}
}
