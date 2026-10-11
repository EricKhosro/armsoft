using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.TaxService.Models;

namespace ArmSoft.TaxService;

public class ApiClient
{
	public const string Bearer = "Bearer";

	public const string BaseUrl = "https://e-invoicing.taxservice.am";

	public const string BaseUrlTest = "https://pl-e-invoicing.taxservice.am";

	public string Token;

	public string EmarkToken;

	protected HttpClient httpClient;

	protected readonly Lazy<JsonSerializerOptions> options;

	public CultureInfo Culture { get; set; }

	public bool ConnectToTestServer { get; set; }

	public ApiClient(HttpClient httpClient, bool connectToTestServer = false)
	{
		if (httpClient == null)
		{
			this.httpClient = new HttpClient();
		}
		else
		{
			this.httpClient = httpClient;
		}
		options = new Lazy<JsonSerializerOptions>(() => new JsonSerializerOptions
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true,
			Converters = 
			{
				(JsonConverter)new TimeZoneConverter(),
				(JsonConverter)new TimeZoneNullableConverter(),
				(JsonConverter)new StringConverter()
			}
		});
		Culture = Thread.CurrentThread.CurrentUICulture;
		ConnectToTestServer = connectToTestServer;
		if (connectToTestServer)
		{
			this.httpClient.Timeout = TimeSpan.FromMinutes(5.0);
		}
	}

	public StringBuilder CreateURLBuilder(string url)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = (ConnectToTestServer ? "https://pl-e-invoicing.taxservice.am" : "https://e-invoicing.taxservice.am");
		stringBuilder.Append(text.TrimEnd('/')).Append(url);
		return stringBuilder;
	}

	public StringBuilder CreateURLBuilder(string url, UrlQuery query)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = (ConnectToTestServer ? "https://pl-e-invoicing.taxservice.am" : "https://e-invoicing.taxservice.am");
		stringBuilder.Append(text.TrimEnd('/')).Append(url);
		query?.AppendTo(stringBuilder);
		return stringBuilder;
	}

	private HttpWebRequest CreateRequest(string method, string url, object requestObject, TimeSpan? timeout = null, string fileFullName = "", bool isEmarkRequest = false)
	{
		if (ServicePointManager.DefaultConnectionLimit < 10)
		{
			ServicePointManager.DefaultConnectionLimit = 10;
		}
		if (ServicePointManager.Expect100Continue)
		{
			ServicePointManager.Expect100Continue = false;
		}
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
		httpWebRequest.KeepAlive = false;
		httpWebRequest.Method = method;
		httpWebRequest.Accept = "application/json";
		httpWebRequest.ContentType = "application/json; charset=utf-8";
		httpWebRequest.Headers.Add("Cookie", "jwt-auth-token=" + (isEmarkRequest ? EmarkToken : Token));
		if (requestObject == null)
		{
			if (string.IsNullOrWhiteSpace(fileFullName))
			{
				httpWebRequest.ContentLength = 0L;
			}
			else
			{
				string text = "----WebKitFormBoundary" + GetRandomString(16);
				byte[] bytes = Encoding.UTF8.GetBytes("\r\n--" + text + "\r\n");
				httpWebRequest.KeepAlive = true;
				httpWebRequest.Accept = "*/*";
				httpWebRequest.ContentType = "multipart/form-data; boundary=" + text;
				using Stream stream = httpWebRequest.GetRequestStream();
				string s = "Content-Disposition: form-data; name=\"file\"; filename=\"" + Path.GetFileName(fileFullName) + "\"\r\nContent-Type: text/csv\r\n\r\n";
				byte[] bytes2 = Encoding.UTF8.GetBytes(s);
				stream.Write(bytes, 0, bytes.Length);
				stream.Write(bytes2, 0, bytes2.Length);
				using (FileStream fileStream = new FileStream(fileFullName, FileMode.Open, FileAccess.Read))
				{
					fileStream.CopyTo(stream);
				}
				byte[] bytes3 = Encoding.UTF8.GetBytes("\r\n--" + text + "--\r\n");
				stream.Write(bytes3, 0, bytes3.Length);
			}
		}
		else
		{
			string s2 = JsonSerializer.Serialize(new Request
			{
				payload = requestObject
			}, options.Value);
			byte[] bytes4 = Encoding.UTF8.GetBytes(s2);
			httpWebRequest.ContentLength = bytes4.Length;
			using Stream stream2 = httpWebRequest.GetRequestStream();
			stream2.Write(bytes4, 0, bytes4.Length);
		}
		if (timeout.HasValue)
		{
			httpWebRequest.Timeout = Convert.ToInt32(timeout.Value.TotalMilliseconds);
		}
		if (ConnectToTestServer)
		{
			httpWebRequest.Timeout = 300000;
		}
		return httpWebRequest;
	}

	public Task<T> ExecAsync<T>(HttpMethod method, string url, object requestObject = null)
	{
		return ExecuteAsync<T>(method, url, CancellationToken.None, requestObject);
	}

	public Task<byte[]> ExecStreamAsync<T>(HttpMethod method, string url, object requestObject = null)
	{
		return ExecuteStreamAsync<T>(method, url, CancellationToken.None, requestObject);
	}

	public async Task<byte[]> ExecuteStreamAsync<T>(HttpMethod method, string url, CancellationToken cancellationToken, object requestObject = null)
	{
		HttpWebRequest httpWebRequest = CreateRequest(method.ToString(), url, requestObject);
		HttpWebResponse httpWebResponse = null;
		try
		{
			httpWebResponse = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());
		}
		catch (WebException ex)
		{
			if (ex.Response == null)
			{
				throw new ApiException(ex.Message);
			}
		}
		if (httpWebResponse == null)
		{
			throw new Exception(string.Format(Resources.Get(ResourceKey.ErrorAccuredDuringWebResponse), url));
		}
		using (httpWebResponse)
		{
			if (httpWebResponse.StatusCode == HttpStatusCode.OK)
			{
				Stream responseStream = httpWebResponse.GetResponseStream();
				using MemoryStream memoryStream = new MemoryStream();
				responseStream.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}
			if (httpWebResponse.StatusCode != HttpStatusCode.NoContent)
			{
				throw new ApiException(httpWebResponse.StatusDescription)
				{
					StatusCode = Convert.ToInt32(httpWebResponse.StatusCode)
				};
			}
			return null;
		}
	}

	protected virtual void CheckAndRefreshToken(bool isEmarkRequest = false)
	{
	}

	public async Task<T> ExecuteAsync<T>(HttpMethod method, string url, CancellationToken cancellationToken, object requestObject = null, string fileFullName = "")
	{
		CheckAndRefreshToken();
		HttpWebRequest httpWebRequest = CreateRequest(method.ToString(), url, requestObject, null, fileFullName);
		HttpWebResponse httpWebResponse = null;
		try
		{
			httpWebResponse = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());
		}
		catch (WebException ex)
		{
			if (ex.Response == null)
			{
				throw new ApiException(ex.Message);
			}
		}
		if (httpWebResponse == null)
		{
			throw new Exception(string.Format(Resources.Get(ResourceKey.ErrorAccuredDuringWebResponse), url));
		}
		using (httpWebResponse)
		{
			if (httpWebResponse.StatusCode == HttpStatusCode.OK)
			{
				return DeserializeResponse<T>(httpWebResponse);
			}
			if (httpWebResponse.StatusCode != HttpStatusCode.NoContent)
			{
				throw new ApiException(httpWebResponse.StatusDescription)
				{
					StatusCode = Convert.ToInt32(httpWebResponse.StatusCode)
				};
			}
			return default;
		}
	}

	private T DeserializeResponse<T>(HttpWebResponse response)
	{
		Stream responseStream = response.GetResponseStream();
		using (responseStream)
		{
			string json = null;
			if (response.ContentEncoding == "gzip")
			{
				using GZipStream stream = new GZipStream(responseStream, CompressionMode.Decompress);
				using StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
				json = streamReader.ReadToEnd();
			}
			else
			{
				using StreamReader streamReader2 = new StreamReader(responseStream, Encoding.UTF8);
				json = streamReader2.ReadToEnd();
			}
			try
			{
				Response<T> response2 = JsonSerializer.Deserialize<Response<T>>(json, options.Value);
				if (response2.ok)
				{
					return response2.payload;
				}
				if (response2.failure.Message == "Entity not found.")
				{
					throw new ApiException("EntityNotFound", Resources.Get(ResourceKey.EntityNotFound));
				}
				if (response2.failure.Code == "fault/client/accessDenied")
				{
					throw new ApiException("AccessDenied", Resources.Get(ResourceKey.AccessDeniedForOperation));
				}
				if (response2.failure.Codes != null && response2.failure.Codes.Count > 0)
				{
					throw new ApiException(response2.failure.Code, response2.failure.Message, response2.failure.Codes);
				}
				throw new ApiException(response2.failure.Code, response2.failure.Message);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	public async Task ExecuteAsync(HttpMethod method, string url, CancellationToken cancellationToken, object requestObject = null, bool isEmarkRequest = false)
	{
		CheckAndRefreshToken(isEmarkRequest);
		ApiClient apiClient = this;
		string method2 = method.ToString();
		bool isEmarkRequest2 = isEmarkRequest;
		HttpWebRequest httpWebRequest = apiClient.CreateRequest(method2, url, requestObject, null, "", isEmarkRequest2);
		HttpWebResponse httpWebResponse = null;
		try
		{
			httpWebResponse = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());
		}
		catch (WebException ex)
		{
			if (ex.Response == null)
			{
				throw new ApiException(ex.Message);
			}
		}
		if (httpWebResponse == null)
		{
			throw new Exception(string.Format(Resources.Get(ResourceKey.ErrorAccuredDuringWebResponse), url));
		}
		using (httpWebResponse)
		{
			if (httpWebResponse.StatusCode == HttpStatusCode.OK)
			{
				DeserializeResponse<object>(httpWebResponse);
				return;
			}
			if (httpWebResponse.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new ApiException("Unauthorized")
				{
					StatusCode = Convert.ToInt32(httpWebResponse.StatusCode)
				};
			}
			if (httpWebResponse.StatusCode == HttpStatusCode.ServiceUnavailable)
			{
				throw new ApiException("Service unavailable")
				{
					StatusCode = Convert.ToInt32(httpWebResponse.StatusCode)
				};
			}
			if (httpWebResponse.StatusCode == HttpStatusCode.NoContent)
			{
				return;
			}
			throw new ApiException(httpWebResponse.StatusDescription)
			{
				StatusCode = Convert.ToInt32(httpWebResponse.StatusCode)
			};
		}
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
		throw new ApiException(errorResponse, (int)response.StatusCode, text);
	}

	private static string GetRandomString(int length)
	{
		Random random = new Random();
		return new string((from s in Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", length)
			select s[random.Next(s.Length)]).ToArray());
	}
}
