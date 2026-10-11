using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ArmSoft.SME.Core.AddIn;

public class ApiClientBase
{
	public const string Bearer = "Bearer";

	public const string Version1 = "1.0";

	public const string VersionHeaderKey = "X-Version";

	protected readonly string _token;

	protected HttpClient httpClient;

	public readonly Lazy<JsonSerializerOptions> options;

	private static readonly TimeSpan defaultTimeOut = new TimeSpan(0, 0, 180);

	public string BaseUrl { get; set; }

	public CultureInfo Culture { get; set; }

	public ApiClientBase(string baseUrl, string token, HttpClient httpClient)
	{
		_token = token;
		if (httpClient == null)
		{
			this.httpClient = new HttpClient();
		}
		else
		{
			this.httpClient = httpClient;
		}
		BaseUrl = baseUrl;
		options = new Lazy<JsonSerializerOptions>(() =>
		{
			JsonSerializerOptions result = JsonSerializerFactory.CloneJsonOptions();
			UpdateJsonSerializerOptions(result);
			return result;
		});
	}

	protected virtual void UpdateJsonSerializerOptions(JsonSerializerOptions options)
	{
	}

	protected virtual void UpdateHttpRequestMessage(HttpRequestMessage requestMessage)
	{
		if (!string.IsNullOrWhiteSpace(_token))
		{
			requestMessage.Headers.Add("Authorization", "Bearer " + _token);
		}
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
			StringContent stringContent = new StringContent(JsonSerializer.Serialize(requestObject, options.Value));
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

	public T Execute<T>(HttpMethod method, string url, object requestObject = null, TimeSpan? timeout = null, IEnumerable<KeyValuePair<string, string>> headers = null)
	{
		return AsyncHelper.RunSync(async () => await ExecuteAsync<T>(method, url, default, requestObject, timeout, headers));
	}

	public async Task<T> ExecuteAsync<T>(HttpMethod method, string url, CancellationToken cancellationToken, object requestObject = null, TimeSpan? timeout = null, IEnumerable<KeyValuePair<string, string>> headers = null, string version = "1.0")
	{
		using HttpRequestMessage request = CreateRequest(method, url, requestObject, "application/json", headers, version);
		SetTimeOut(timeout);
		using HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (response.StatusCode == HttpStatusCode.OK)
		{
			if (response == null || response.Content == null)
			{
				return default;
			}
			string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				return JsonSerializer.Deserialize<T>(text, options.Value);
			}
			catch (JsonException innerException)
			{
				throw new ApiException("Could not deserialize the response body string as " + typeof(T).FullName + ".", (int)response.StatusCode, text, innerException);
			}
		}
		if (response.StatusCode != HttpStatusCode.NoContent)
		{
			await HandleErrorAsync(response, method, url).ConfigureAwait(continueOnCapturedContext: false);
		}
		return default;
	}

	private async Task HandleErrorAsync(HttpResponseMessage response, HttpMethod method, string url)
	{
		string message = "The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").";
		if (response == null || response.Content == null)
		{
			throw new ApiException(message, (int)response.StatusCode, "", null);
		}
		string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
		ErrorResponse errorResponse;
		try
		{
			errorResponse = JsonSerializer.Deserialize<ErrorResponse>(text, options.Value);
		}
		catch (JsonException innerException)
		{
			throw new ApiException(message, (int)response.StatusCode, text, innerException);
		}
		if (errorResponse == null)
		{
			throw new ApiException(message, (int)response.StatusCode, "", null);
		}
		_ = errorResponse.IsRestException;
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

	private void SetTimeOut(TimeSpan? timeout)
	{
		if (timeout.HasValue)
		{
			httpClient.Timeout = timeout.Value;
		}
		else if (httpClient.Timeout != defaultTimeOut)
		{
			httpClient.Timeout = defaultTimeOut;
		}
	}
}
