using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ArmSoft.SME.Client;

public class ApiClientBase
{
	public const string Bearer = "Bearer";

	protected HttpClient _httpClient;

	public readonly Lazy<JsonSerializerOptions> Options;

	public readonly ILogger Logger;

	private static readonly TimeSpan _defaultTimeOut = new TimeSpan(0, 0, 180);

	public string BaseUrl { get; set; }

	public CultureInfo Culture { get; set; }

	public ApiClientBase(string baseUrl, HttpClient httpClient, ILogger logger)
	{
		if (httpClient == null)
		{
			_httpClient = new HttpClient();
			_httpClient.Timeout = _defaultTimeOut;
		}
		else
		{
			_httpClient = httpClient;
		}
		BaseUrl = baseUrl;
		Options = new Lazy<JsonSerializerOptions>(() =>
		{
			JsonSerializerOptions jsonSerializerOptions = JsonSerializerFactory.CloneJsonOptions();
			UpdateJsonSerializerOptions(jsonSerializerOptions);
			return jsonSerializerOptions;
		});
		Logger = logger ?? NullLogger.Instance;
	}

	protected virtual void UpdateJsonSerializerOptions(JsonSerializerOptions options)
	{
	}

	protected virtual void UpdateHttpRequestMessage(HttpRequestMessage requestMessage)
	{
	}

	public StringBuilder CreateURLBuilder(string url)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append((BaseUrl != null) ? BaseUrl.TrimEnd('/') : "").Append(url);
		return stringBuilder;
	}

	public StringBuilder CreateURLBuilder(string url, UrlQuery query)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append((BaseUrl != null) ? BaseUrl.TrimEnd('/') : "").Append(url);
		query?.AppendTo(stringBuilder);
		return stringBuilder;
	}

	private HttpRequestMessage CreateRequest(HttpMethod method, string url, object requestObject, string accept = "application/json", IEnumerable<KeyValuePair<string, string>> headers = null, string version = "1.0")
	{
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
		if (requestObject != null)
		{
			StringContent stringContent = new StringContent(JsonSerializer.Serialize(requestObject, Options.Value));
			stringContent.Headers.ContentType.MediaType = "application/json";
			stringContent.Headers.ContentType.CharSet = "utf-8";
			httpRequestMessage.Content = stringContent;
		}
		httpRequestMessage.Method = method;
		httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
		httpRequestMessage.RequestUri = new Uri(url, UriKind.Absolute);
		string text = ((Culture != null) ? Culture.Name.Substring(0, 2).ToLower() : Thread.CurrentThread.CurrentUICulture.Name.Substring(0, 2).ToLower());
		httpRequestMessage.Headers.Add("Accept-Language", text switch
		{
			"hy" => "hy-AM,hy;q=0.5", 
			"ka" => "ka-GE,ka;q=0.5", 
			"ru" => "ru-RU,ru;q=0.5", 
			_ => "en-US,en;q=0.5", 
		});
		httpRequestMessage.Headers.Add("X-Version", version);
		UpdateHttpRequestMessage(httpRequestMessage);
		if (headers != null)
		{
			foreach (KeyValuePair<string, string> header in headers)
			{
				httpRequestMessage.Headers.Add(header.Key, header.Value);
			}
		}
		return httpRequestMessage;
	}

	public Task<T> ExecAsync<T>(HttpMethod method, string url, object requestObject = null)
	{
		return ExecuteAsync<T>(method, url, CancellationToken.None, requestObject);
	}

	public T Execute<T>(HttpMethod method, string url, object requestObject = null, IEnumerable<KeyValuePair<string, string>> headers = null, string version = "1.0")
	{
		return AsyncHelper.RunSync(async () => await ExecuteAsync<T>(method, url, default, requestObject, headers, version));
	}

	public async Task<T> ExecuteAsync<T>(HttpMethod method, string url, CancellationToken cancellationToken, object requestObject = null, IEnumerable<KeyValuePair<string, string>> headers = null, string version = "1.0")
	{
		using HttpRequestMessage request = CreateRequest(method, url, requestObject, "application/json", headers, version);
		Logger.LogInformation("{method} {url}", method, url);
		using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (response.StatusCode == HttpStatusCode.OK)
		{
			if (response == null || response.Content == null)
			{
				return default;
			}
			string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				return JsonSerializer.Deserialize<T>(text, Options.Value);
			}
			catch (JsonException innerException)
			{
				string text2 = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
				Logger.LogError(text2 + " { method} {url}", method, url);
				throw new ApiException(text2, (int)response.StatusCode, text, innerException);
			}
		}
		if (response.StatusCode != HttpStatusCode.NoContent)
		{
			await HandleErrorAsync(response, method, url).ConfigureAwait(continueOnCapturedContext: false);
		}
		return default;
	}

	protected virtual Task TryRefreshAllCaches()
	{
		return Task.CompletedTask;
	}

	public void Execute(HttpMethod method, string url, object requestObject = null, IEnumerable<KeyValuePair<string, string>> headers = null, string version = "1.0")
	{
		AsyncHelper.RunSync(async () =>
		{
			await ExecuteAsync(method, url, default, requestObject, headers, version);
		});
	}

	public async Task ExecuteAsync(HttpMethod method, string url, CancellationToken cancellationToken, object requestObject = null, IEnumerable<KeyValuePair<string, string>> headers = null, string version = "1.0")
	{
		using HttpRequestMessage request = CreateRequest(method, url, requestObject, "application/json", headers, version);
		Logger.LogInformation("{method} {url}", method, url);
		using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
		{
			await HandleErrorAsync(response, method, url).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	public async Task<JsonDocument> GetDocumentAsync(HttpMethod method, string url, object requestObject = null, IEnumerable<KeyValuePair<string, string>> headers = null, string version = "1.0", CancellationToken cancellationToken = default(CancellationToken))
	{
		using HttpRequestMessage request = CreateRequest(method, url, requestObject, "application/json", headers, version);
		Logger.LogInformation("{method} {url}", method, url);
		using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (response.StatusCode == HttpStatusCode.OK)
		{
			if (response == null || response.Content == null)
			{
				return null;
			}
			try
			{
				using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				return await JsonDocument.ParseAsync(responseStream, default, cancellationToken);
			}
			catch (JsonException innerException)
			{
				string text = "Could not deserialize the response content.";
				Logger.LogError("{method} {url}.{message} ", method, url, text);
				throw new ApiException(text, (int)response.StatusCode, "", innerException);
			}
		}
		if (response.StatusCode != HttpStatusCode.NoContent)
		{
			await HandleErrorAsync(response, method, url).ConfigureAwait(continueOnCapturedContext: false);
		}
		return null;
	}

	public async Task<byte[]> GetBytesAsync(HttpMethod method, string url, object requestObject = null, IEnumerable<KeyValuePair<string, string>> headers = null, string version = "1.0", CancellationToken cancellationToken = default(CancellationToken))
	{
		using HttpRequestMessage request = CreateRequest(method, url, requestObject, "application/zip", headers, version);
		Logger.LogInformation("{method} {url}", method, url);
		using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (response.StatusCode == HttpStatusCode.OK)
		{
			if (response == null || response.Content == null)
			{
				return null;
			}
			return await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		if (response.StatusCode != HttpStatusCode.NoContent)
		{
			await HandleErrorAsync(response, method, url).ConfigureAwait(continueOnCapturedContext: false);
		}
		return null;
	}

	private async Task HandleErrorAsync(HttpResponseMessage response, HttpMethod method, string url)
	{
		string message = "The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").";
		if (response == null || response.Content == null)
		{
			Logger.LogError("{method} {url}. " + message, method, url);
			throw new ApiException(message, (int)response.StatusCode, "", null);
		}
		string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (string.IsNullOrWhiteSpace(text))
		{
			Logger.LogError("{method} {url}. " + message, method, url);
			throw new ApiException(message, (int)response.StatusCode, "", null);
		}
		ErrorResponse errorResponse;
		try
		{
			errorResponse = JsonSerializer.Deserialize<ErrorResponse>(text, Options.Value);
		}
		catch (JsonException innerException)
		{
			Logger.LogError("{method} {url}. " + message, method, url);
			throw new ApiException(message, (int)response.StatusCode, text, innerException);
		}
		if (errorResponse == null)
		{
			Logger.LogError("{method} {url}. " + message, method, url);
			throw new ApiException(message, (int)response.StatusCode, "", null);
		}
		if (!errorResponse.IsRestException)
		{
			Logger.LogError("{method} {url}.{message}", method, url, errorResponse.Error);
		}
		if (response.StatusCode == HttpStatusCode.Unauthorized && !errorResponse.IsRestException)
		{
			throw new ApiException("Unauthorized", (int)response.StatusCode, text, null);
		}
		throw new ApiException(errorResponse, (int)response.StatusCode, text)
		{
			IsRestException = errorResponse.IsRestException,
			FieldName = errorResponse.FieldName,
			Title = errorResponse.Title,
			Visible = errorResponse.Visible,
			ErrorDetails = errorResponse.ErrorDetails,
			Code = errorResponse.Code
		};
	}

	public async Task<byte[]> Download(HttpMethod method, string url, CancellationToken cancellationToken, object requestObject = null, IEnumerable<KeyValuePair<string, string>> headers = null, string version = "1.0")
	{
		using HttpRequestMessage request = CreateRequest(method, url, requestObject, "application/json", headers, version);
		Logger.LogInformation("{method} {url}", method, url);
		using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (response.StatusCode == HttpStatusCode.OK)
		{
			if (response == null || response.Content == null)
			{
				return null;
			}
			byte[] array = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				return array;
			}
			catch (JsonException innerException)
			{
				string text = "Could not deserialize the response body as ByteArray.";
				Logger.LogError(text + " { method} {url}", method, url);
				throw new ApiException(text, (int)response.StatusCode, array.ToString(), innerException);
			}
		}
		if (response.StatusCode != HttpStatusCode.NoContent)
		{
			await HandleErrorAsync(response, method, url).ConfigureAwait(continueOnCapturedContext: false);
		}
		return null;
	}
}
