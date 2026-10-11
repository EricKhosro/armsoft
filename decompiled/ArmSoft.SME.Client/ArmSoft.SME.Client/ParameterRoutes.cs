using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models.Parameter;

namespace ArmSoft.SME.Client;

public class ParameterRoutes
{
	private readonly ApiClient client;

	internal ParameterRoutes(ApiClient client)
	{
		this.client = client;
	}

	public Task<List<ParameterResponseModel>> GetValuesAsync(int userId, bool alsoRoleParams = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("userId", userId);
		urlQuery.Add("alsoRoleParams", alsoRoleParams);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Parameter/Values", urlQuery);
		return client.ExecuteAsync<List<ParameterResponseModel>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<ParameterResponseModel> GetValues(int userId, bool alsoRoleParams = false)
	{
		return AsyncHelper.RunSync(async () => await GetValuesAsync(userId, alsoRoleParams));
	}

	public Task<List<ParameterResponseModel>> GetGeneralValuesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Parameter/GeneralValues");
		return client.ExecuteAsync<List<ParameterResponseModel>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<ParameterResponseModel> GetGeneralValues()
	{
		return AsyncHelper.RunSync(async () => await GetGeneralValuesAsync());
	}

	public Task<List<ParameterForUsersResponseModel>> GetValuesForUsersAsync(short paramId, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("paramId", paramId);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Parameter/ValuesForUsers", urlQuery);
		return client.ExecuteAsync<List<ParameterForUsersResponseModel>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<ParameterForUsersResponseModel> GetValuesForUsers(short paramId)
	{
		return AsyncHelper.RunSync(async () => await GetValuesForUsersAsync(paramId));
	}

	public Task<string> GetValueAsync(short id, int userId, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("userId", userId);
		StringBuilder stringBuilder = client.CreateURLBuilder($"/api/Parameter/Value/{id}", urlQuery);
		return client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public string GetValue(short id, int userId)
	{
		return AsyncHelper.RunSync(async () => await GetValueAsync(id, userId));
	}

	public Task<string> GetGeneralValueAsync(short id, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"/api/Parameter/GeneralValue/{id}");
		return client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public string GetGeneralValue(short id)
	{
		return AsyncHelper.RunSync(async () => await GetGeneralValueAsync(id));
	}

	public Task SetValuesAsync(List<ParameterStoreRequestModel> parameters, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Parameter/Values");
		return client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, parameters);
	}

	public void SetValues(List<ParameterStoreRequestModel> parameters)
	{
		AsyncHelper.RunSync(async () =>
		{
			await SetValuesAsync(parameters);
		});
	}

	public Task SetValueAsync(short id, string value, int userId, bool isDefault = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		ParameterStoreRequestModel requestObject = new ParameterStoreRequestModel
		{
			Id = id,
			Value = value,
			UserId = userId,
			IsDefault = isDefault
		};
		StringBuilder stringBuilder = client.CreateURLBuilder($"/api/Parameter/Value/{id}");
		return client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void SetValue(short id, string value, int userId = -1, bool isDefault = false)
	{
		AsyncHelper.RunSync(async () =>
		{
			await SetValueAsync(id, value, userId, isDefault);
		});
	}
}
