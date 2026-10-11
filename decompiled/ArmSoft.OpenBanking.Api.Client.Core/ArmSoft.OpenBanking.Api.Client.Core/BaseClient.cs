using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ArmSoft.OpenBanking.Api.Client.Core;

public abstract class BaseClient
{
	private readonly HttpClient httpClient;

	protected internal HttpClient HttpClient => httpClient;

	protected BaseClient(HttpClient httpClient)
	{
		this.httpClient = httpClient;
	}

	protected virtual void SetAuthorizationHeader(HttpRequestMessage request)
	{
	}

	protected virtual void SetRequestHeaders(HttpRequestMessage request)
	{
	}

	public async Task<T> GetAsync<T>(string path, Dictionary<string, string>? query = null, string? accessToken = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		string text = path;
		if (query != null && query.Any())
		{
			string text2 = string.Join("&", query.Select((KeyValuePair<string, string> kvp) => Uri.EscapeDataString(kvp.Key) + "=" + Uri.EscapeDataString(kvp.Value)));
			text = text + "?" + text2;
		}
		using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, text);
		SetRequestHeaders(request);
		ApplyAuthorizationHeader(request, accessToken);
		using HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
		if (!response.IsSuccessStatusCode)
		{
			await ValidateResponse(response, cancellationToken);
		}
		Response<T> response2 = await response.Content.ReadFromJsonAsync<Response<T>>(cancellationToken);
		if (response2 == null)
		{
			string content = await response.Content.ReadAsStringAsync(cancellationToken);
			throw new ApiException(response.StatusCode, content);
		}
		return response2.Value;
	}

	public async Task<T> PostAsync<T>(string path, object? body = null, Dictionary<string, string>? headers = null, Dictionary<string, string>? query = null, string? accessToken = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		return (await PostResponseAsync<T>(path, body, headers, query, accessToken, cancellationToken)).Value;
	}

	public async Task PostAsync(string path, object? body = null, Dictionary<string, string>? headers = null, Dictionary<string, string>? query = null, string? accessToken = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		using (await SendPostAsync(path, body, headers, query, accessToken, cancellationToken))
		{
		}
	}

	public async Task<Response<T>> PostResponseAsync<T>(string path, object? body = null, Dictionary<string, string>? headers = null, Dictionary<string, string>? query = null, string? accessToken = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		using HttpResponseMessage response = await SendPostAsync(path, body, headers, query, accessToken, cancellationToken);
		return await GetContent<T>(response, cancellationToken);
	}

	public async Task<Response<object?>> PostResponseAsync(string path, object? body = null, Dictionary<string, string>? headers = null, Dictionary<string, string>? query = null, string? accessToken = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		using HttpResponseMessage response = await SendPostAsync(path, body, headers, query, accessToken, cancellationToken);
		return await GetContent<object>(response, cancellationToken);
	}

	private async Task<HttpResponseMessage> SendPostAsync(string path, object? body, Dictionary<string, string>? headers, Dictionary<string, string>? query, string? accessToken, CancellationToken cancellationToken)
	{
		string text = path;
		if (query != null && query.Any())
		{
			string text2 = string.Join("&", query.Select((KeyValuePair<string, string> kvp) => Uri.EscapeDataString(kvp.Key) + "=" + Uri.EscapeDataString(kvp.Value)));
			text = text + "?" + text2;
		}
		using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, text);
		if (body != null)
		{
			request.Content = JsonContent.Create(body);
		}
		if (headers != null)
		{
			foreach (KeyValuePair<string, string> header in headers)
			{
				request.Headers.Add(header.Key, header.Value);
			}
		}
		SetRequestHeaders(request);
		ApplyAuthorizationHeader(request, accessToken);
		HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
		if (!response.IsSuccessStatusCode)
		{
			try
			{
				await ValidateResponse(response, cancellationToken);
			}
			catch
			{
				response.Dispose();
				throw;
			}
		}
		return response;
	}

	public async Task<T> PutAsync<T>(string path, object? body = null, string? accessToken = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, path);
		if (body != null)
		{
			request.Content = JsonContent.Create(body);
		}
		SetRequestHeaders(request);
		ApplyAuthorizationHeader(request, accessToken);
		using HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
		if (!response.IsSuccessStatusCode)
		{
			await ValidateResponse(response, cancellationToken);
		}
		return (await GetContent<T>(response, cancellationToken)).Value;
	}

	public async Task DeleteAsync(string path, string? accessToken = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, path);
		SetRequestHeaders(request);
		ApplyAuthorizationHeader(request, accessToken);
		using HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
		if (!response.IsSuccessStatusCode)
		{
			await ValidateResponse(response, cancellationToken);
		}
	}

	private async Task<Response<T>> GetContent<T>(HttpResponseMessage response, CancellationToken cancellationToken)
	{
		Response<T> response2 = await response.Content.ReadFromJsonAsync<Response<T>>(cancellationToken);
		if (response2 == null)
		{
			string content = await response.Content.ReadAsStringAsync(cancellationToken);
			throw new ApiException(response.StatusCode, content);
		}
		return response2;
	}

	private async Task ValidateResponse(HttpResponseMessage response, CancellationToken cancellationToken)
	{
		string text = await response.Content.ReadAsStringAsync(cancellationToken);
		Response<object> response2 = null;
		try
		{
			response2 = JsonSerializer.Deserialize<Response<object>>(text, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
		}
		catch
		{
		}
		switch (response.StatusCode)
		{
		case HttpStatusCode.BadRequest:
			if (response2?.ResponseCode == null)
			{
				try
				{
					ValidationProblemDetails validationProblemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(text, new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true
					});
					if (validationProblemDetails != null && validationProblemDetails.Title != null)
					{
						response2 = new Response<object>
						{
							ResponseCode = "standard-validation-errors",
							Message = validationProblemDetails.Title + " " + validationProblemDetails.Errors.FirstOrDefault().Value?.FirstOrDefault()
						};
					}
				}
				catch
				{
				}
			}
			throw new BadRequestException(response2?.ResponseCode ?? string.Empty, text, response2?.Message ?? "Bad Request");
		case HttpStatusCode.Unauthorized:
			throw new UnauthorizedException(response2?.ResponseCode ?? string.Empty, text, response2?.Message ?? "Unauthorized");
		case HttpStatusCode.NotFound:
			throw new NotFoundException(response2?.ResponseCode ?? string.Empty, text, response2?.Message ?? "Not Found");
		case HttpStatusCode.Forbidden:
			throw new ForbiddenException(response2?.ResponseCode ?? string.Empty, text, response2?.Message ?? "Opertaion not allowed");
		default:
			throw new ApiException(response.StatusCode, text);
		}
	}

	private void ApplyAuthorizationHeader(HttpRequestMessage request, string? accessToken)
	{
		if (accessToken != null)
		{
			request.Headers.Authorization = (string.IsNullOrWhiteSpace(accessToken) ? null : new AuthenticationHeaderValue("Bearer", accessToken));
		}
		else
		{
			SetAuthorizationHeader(request);
		}
	}
}
