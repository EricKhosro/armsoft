using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client;

public class UserRoutes
{
	protected readonly ApiClient _client;

	protected virtual string ControllerName => "User";

	public UserRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task UpdateUserMappingAsync(int outerUserId, int innerUserId, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("outerUserId", outerUserId);
		urlQuery.Add("innerUserId", innerUserId);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/UpdateUserMapping", urlQuery);
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken);
	}

	public void UpdateUserMapping(int outerUserId, int innerUserId)
	{
		AsyncHelper.RunSync(async () =>
		{
			await UpdateUserMappingAsync(outerUserId, innerUserId);
		});
	}

	public Task DeleteFromUsersMappingAsync(int outerUserId, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("outerUserId", outerUserId);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/DeleteFromUsersMapping", urlQuery);
		return _client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken);
	}

	public void DeleteFromUsersMapping(int outerUserId)
	{
		AsyncHelper.RunSync(async () =>
		{
			await DeleteFromUsersMappingAsync(outerUserId);
		});
	}

	public Task ValidateDeleteAsync(int userId, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("userId", userId);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/ValidateDelete", urlQuery);
		return _client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken);
	}

	public void ValidateDelete(int userId)
	{
		AsyncHelper.RunSync(async () =>
		{
			await ValidateDeleteAsync(userId);
		});
	}

	public Task DeleteAsync(string adb2cToken, int userId, CancellationToken cancellationToken = default(CancellationToken))
	{
		UserDeleteRequest requestObject = new UserDeleteRequest
		{
			IdToken = adb2cToken,
			UserId = userId
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName);
		return _client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void Delete(string adb2cToken, int userId)
	{
		AsyncHelper.RunSync(async () =>
		{
			await DeleteAsync(adb2cToken, userId);
		});
	}

	public Task UpdateUsersAsync(string adb2cToken, List<int> userIds, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName);
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, new UserUpdateRequest
		{
			IdToken = adb2cToken,
			UserIds = userIds
		});
	}

	public void UpdateUsers(string adb2cToken, List<int> userIds = null)
	{
		AsyncHelper.RunSync(async () =>
		{
			await UpdateUsersAsync(adb2cToken, userIds);
		});
	}

	public Task UpdateUsersByASAdminAsync(string adb2cToken, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/StoreByASAdmin");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, new UserUpdateRequest
		{
			IdToken = adb2cToken
		});
	}

	public void UpdateUsersByASAdmin(string adb2cToken)
	{
		AsyncHelper.RunSync(async () =>
		{
			await UpdateUsersByASAdminAsync(adb2cToken);
		});
	}
}
