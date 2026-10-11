using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ArmSoft.OpenBanking.Api.Client.Core;

namespace ArmSoft.OpenBanking.Api.Client;

public abstract class BaseManager
{
	protected ApiClient Client { get; }

	protected BaseManager(ApiClient client)
	{
		Client = client;
	}

	public Task<T> PostAsync<T>(string url, object body)
	{
		JsonContent content = JsonContent.Create(body);
		return PostAsync<T>(url, content);
	}

	public async Task<T> PostAsync<T>(string url, HttpContent content)
	{
		AuthenticationHeaderValue authorization = null;
		if (!string.IsNullOrEmpty(Client.AccessToken))
		{
			authorization = new AuthenticationHeaderValue("Bearer", Client.AccessToken);
		}
		Client.HttpClient.DefaultRequestHeaders.Authorization = authorization;
		HttpResponseMessage response = await Client.HttpClient.PostAsync(url, content);
		HttpStatusCode statusCode;
		if (!response.IsSuccessStatusCode)
		{
			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new UnauthorizedException(await response.Content.ReadAsStringAsync());
			}
			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				throw new NotFoundException(await response.Content.ReadAsStringAsync());
			}
			if (response.StatusCode == HttpStatusCode.BadRequest)
			{
				throw new BadRequestException(await response.Content.ReadAsStringAsync());
			}
			if (response.StatusCode == HttpStatusCode.Forbidden)
			{
				throw new ForbiddenException(await response.Content.ReadAsStringAsync());
			}
			statusCode = response.StatusCode;
			throw new UnexpectedException(statusCode, await response.Content.ReadAsStringAsync());
		}
		T val = await response.Content.ReadFromJsonAsync<T>();
		if (val != null)
		{
			return val;
		}
		statusCode = response.StatusCode;
		throw new UnexpectedException(statusCode, await response.Content.ReadAsStringAsync());
	}

	public async Task PostAsync(string url, object body)
	{
		HttpContent content = JsonContent.Create(body);
		if (!string.IsNullOrEmpty(Client.AccessToken))
		{
			Client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Client.AccessToken);
		}
		else
		{
			Client.HttpClient.DefaultRequestHeaders.Authorization = null;
		}
		HttpResponseMessage httpResponseMessage = await Client.HttpClient.PostAsync(url, content);
		if (!httpResponseMessage.IsSuccessStatusCode)
		{
			if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new UnauthorizedException(httpResponseMessage.Content.ToString());
			}
			if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
			{
				throw new NotFoundException(httpResponseMessage.Content.ToString());
			}
			if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
			{
				throw new BadRequestException(httpResponseMessage.Content.ToString());
			}
			if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
			{
				throw new ForbiddenException(await httpResponseMessage.Content.ReadAsStringAsync());
			}
			HttpStatusCode statusCode = httpResponseMessage.StatusCode;
			throw new UnexpectedException(statusCode, await httpResponseMessage.Content.ReadAsStringAsync());
		}
	}
}
