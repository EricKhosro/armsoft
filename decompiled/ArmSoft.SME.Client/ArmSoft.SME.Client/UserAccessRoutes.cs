using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client.Models;
using ArmSoft.SME.Common.Models.UserAccess;

namespace ArmSoft.SME.Client;

public class UserAccessRoutes
{
	private readonly ApiClient _client;

	public UserAccessRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<List<NavigatorNodeAccess>> GetAccessTreeAsync(int userId = -1, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/UserAccess/AccessTree/{userId}");
		return _client.ExecuteAsync<List<NavigatorNodeAccess>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<NavigatorNodeAccess> GetAccessTree(int userId = -1)
	{
		return AsyncHelper.RunSync(async () => await GetAccessTreeAsync(userId));
	}

	public Task<List<SubsystemAccess>> GetAccessibleSubsystemsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UserAccess/AccessibleSubsystems/");
		return _client.ExecuteAsync<List<SubsystemAccess>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<SubsystemAccess> GetAccessibleSubsystems()
	{
		return AsyncHelper.RunSync(async () => await GetAccessibleSubsystemsAsync());
	}

	public Task<List<POPermissionWithType>> GetDocumentsPermissionsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UserAccess/DocumentsPermissions/");
		return _client.ExecuteAsync<List<POPermissionWithType>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<POPermissionWithType> GetDocumentsPermissions()
	{
		return AsyncHelper.RunSync(async () => await GetDocumentsPermissionsAsync());
	}

	public Task<List<POPermissionWithType>> GetDirectoriesPermissionsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UserAccess/DirectoriesPermissions/");
		return _client.ExecuteAsync<List<POPermissionWithType>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<POPermissionWithType> GetDirectoriesPermissions()
	{
		return AsyncHelper.RunSync(async () => await GetDirectoriesPermissionsAsync());
	}

	public Task<List<ProviderPermissionWithType>> GetProvidersPermissionsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UserAccess/ProvidersPermissions/");
		return _client.ExecuteAsync<List<ProviderPermissionWithType>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<ProviderPermissionWithType> GetProvidersPermissions()
	{
		return AsyncHelper.RunSync(async () => await GetProvidersPermissionsAsync());
	}

	public List<NavigatorNodeAccess> GetUserAvailableAccessTree(int userID, string availableSubsystems)
	{
		return AsyncHelper.RunSync(async () => await GetUserAvailableAccessTreeAsync(userID, availableSubsystems));
	}

	public Task<List<NavigatorNodeAccess>> GetUserAvailableAccessTreeAsync(int userID, string availableSubsystems, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("userID", userID);
		urlQuery.Add("availableSubsystems", availableSubsystems);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UserAccess/UserAvailableAccessTree/", urlQuery);
		return _client.ExecuteAsync<List<NavigatorNodeAccess>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Task EditUserAccessAsync(int userId, UserAccessRow user, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("userId", userId);
		urlQuery.Add("AccessID", user.AccessID);
		urlQuery.Add("SubSystem", user.SubSystem);
		urlQuery.Add("View", user.View);
		urlQuery.Add("Operation", user.Operation);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UserAccess/EditUserAccess", urlQuery);
		return _client.ExecuteAsync(HttpMethod.Put, stringBuilder.ToString(), cancellationToken);
	}

	public void EditUserAccess(int userId, UserAccessRow user)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditUserAccessAsync(userId, user);
		});
	}

	public Task CopyUserAccessInfoFromOtherUserAsync(CopyUserAccessRequestModel copyUserAccessRequestModel, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("DestId", copyUserAccessRequestModel.DestId);
		urlQuery.Add("SourceId", copyUserAccessRequestModel.SourceId);
		urlQuery.Add("CopyUserPermissions", copyUserAccessRequestModel.CopyUserPermissions);
		urlQuery.Add("CopyStorageAccess", copyUserAccessRequestModel.CopyStorageAccess);
		urlQuery.Add("CopyCashesAccess", copyUserAccessRequestModel.CopyCashesAccess);
		urlQuery.Add("CopyDocOptions", copyUserAccessRequestModel.CopyDocOptions);
		urlQuery.Add("CopyMTFavourites", copyUserAccessRequestModel.CopyMTFavourites);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UserAccess/CopyUserAccessInfoFromOtherUser", urlQuery);
		return _client.ExecuteAsync(HttpMethod.Put, stringBuilder.ToString(), cancellationToken);
	}

	public void CopyUserAccessInfoFromOtherUser(CopyUserAccessRequestModel copyUserAccessRequestModel)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CopyUserAccessInfoFromOtherUserAsync(copyUserAccessRequestModel);
		});
	}

	public bool UserAccessHasBeenModified(int userID = -1)
	{
		return AsyncHelper.RunSync(async () => await UserAccessHasBeenModifiedAsync(userID));
	}

	public async Task<bool> UserAccessHasBeenModifiedAsync(int userID = -1, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("userID", userID);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UserAccess/UserAccessHasBeenModified", urlQuery);
		return await _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public void AddUserAccesses(List<UserAccessRow> accessIDs, int userID = -1)
	{
		AsyncHelper.RunSync(async () =>
		{
			await AddUserAccessesAsync(accessIDs, userID);
		});
	}

	public Task AddUserAccessesAsync(List<UserAccessRow> accessIDs, int userID = -1, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("userID", userID);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UserAccess/AddUserAccesses", urlQuery);
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, accessIDs);
	}

	public Task<bool> ExistUserRoleAsync(int userId, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/UserAccess/ExistUserRole/{userId}");
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool ExistUserRole(int userId = -1)
	{
		return AsyncHelper.RunSync(async () => await ExistUserRoleAsync(userId));
	}
}
